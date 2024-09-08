using System;
using System.DirectoryServices.AccountManagement;
using System.Diagnostics;

using pGina.Shared.Interfaces;
using pGina.Shared.Types;
using log4net;
using System.Threading;
using System.Collections.Generic;

namespace pGina.Plugin.JWTSession
{
    public class PluginImpl : IPluginConfiguration, IPluginAuthentication, IPluginAuthenticationGateway, IPluginChangePassword, IPluginEventNotifications
    {
        private static ILog m_logger = LogManager.GetLogger("JWTSession");
 
        private Timer m_backgroundTask;

        private Dictionary<string, UserSession> m_sessions;

        #region Init-plugin
        public static Guid PluginUuid
        {
            get { return new Guid("{A7830D5E-B6A1-47CF-A6B0-F355F6D063E2}"); }
        }


        public PluginImpl()
        {
            using (Process me = Process.GetCurrentProcess())
            {
                m_logger.DebugFormat("Plugin initialized on {0} in PID: {1} Session: {2}", Environment.MachineName, me.Id, me.SessionId);
            }
        }

        public string Name
        {
            get { return "JWTSession"; }
        }

        public string Description
        {
            get { return "Uses http(s) request to obtain user info"; }
        }

        public Guid Uuid
        {
            get { return PluginUuid; }
        }

        public string Version
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }
        #endregion

        public void Starting()
        {
            m_sessions = new Dictionary<string, UserSession>();

            StartBackgroundTask();
        }

        public void Stopping()
        {
            m_sessions.Clear();

            StopBackgroundTask();
            
        }

        private void StartBackgroundTask()
        {
            m_logger.Debug("Starting background task timer");
            m_backgroundTask = new Timer(new TimerCallback(SessionBackgroundTask), null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(60));
        }

        private void StopBackgroundTask()
        {
            m_logger.Debug("Stopping background task timer");
            m_backgroundTask.Dispose();
            m_backgroundTask = null;
        }

        private void SessionBackgroundTask(object state)
        {
            /*foreach (int sess in sessions)
            {
                m_logger.DebugFormat("Logging off session {0}", sess);
                bool result = Abstractions.WindowsApi.pInvokes.LogoffSession(sess);
                if (result)
                    m_logger.Debug("Log off successful.");
                else
                    m_logger.Debug("Log off failed.");
            }*/

            foreach(KeyValuePair<string, UserSession> sessionPair in m_sessions)
            {
                SessionCheckInResponse sessionResponse = JsonAccessor.checkActiveSession(sessionPair.Value);

                if (sessionResponse.error != null)
                {
                    Abstractions.WindowsApi.pInvokes.SendMessageToUser(sessionPair.Value.sessionId, "Server Session Error", sessionResponse.error);

                    return;
                }

                switch(sessionResponse.action)
                {
                    case "logout":
                        bool result = Abstractions.WindowsApi.pInvokes.LogoffSession(sessionPair.Value.sessionId);

                        if (result)
                        {
                            m_logger.Debug("Log off successful.");

                            m_sessions.Remove(sessionPair.Key);
                        } else
                        {
                            m_logger.Debug("Log off failed.");
                        }
                            

                        break;
                    case "message":
                        Abstractions.WindowsApi.pInvokes.SendMessageToUser(sessionPair.Value.sessionId, "Server Message", sessionResponse.actionContext);

                        break;
                    case "processOpen":
                        Abstractions.WindowsApi.pInvokes.StartUserProcessInSession(sessionPair.Value.sessionId, sessionResponse.actionContext);

                        break;
                }
            }
        }


        public void Configure()
        {
            Configuration dialog = new Configuration();
            dialog.ShowDialog();
        }

        public BooleanResult AuthenticateUser(SessionProperties properties)
        {
            // this method shall say if our credentials are valid
            UserInformation userInfo = properties.GetTrackedSingle<UserInformation>();

            return JsonAccessor.getResponse(userInfo.Username, userInfo.Password);
        }

        public BooleanResult AuthenticatedUserGateway(SessionProperties properties)
        {
            // this method shall perform some other tasks ...

            UserInformation userInfo = properties.GetTrackedSingle<UserInformation>();

            LoginTokenResponse uinfo = JsonAccessor.getUserInfo(userInfo.Username);
            LoginPayload upayload = uinfo.ExtractToken();

            if (uinfo != null)
            {
                m_logger.DebugFormat("AuthenticatedUserGateway: LoginResponse: {0}", uinfo.ToString());

                foreach (string group in upayload.groups)
                {
                    userInfo.AddGroup(new GroupInformation() { Name = group });
                }

                properties.AddTrackedSingle<UserInformation>(userInfo);

                // and what else ??? :)
                
            }

            return new BooleanResult() { Success = true };
        }

        public BooleanResult ChangePassword(SessionProperties properties, ChangePasswordPluginActivityInfo pluginInfo)
        {
            UserInformation userInfo = properties.GetTrackedSingle<UserInformation>();

            m_logger.DebugFormat("ChangePassword(): {0}", userInfo.ToString());

            // Verify the old password
            if (Abstractions.WindowsApi.pInvokes.ValidateCredentials(userInfo.Username, userInfo.oldPassword))
            {
                m_logger.DebugFormat("Authenticated via old password: {0}", userInfo.Username);
            }
            else
            {
                return new BooleanResult { Success = false, Message = "Current password or username is not valid." };
            }

            return JsonAccessor.getPwChangeResponse(userInfo.Username, userInfo.Password, userInfo.oldPassword);
        }

        public void SessionChange(int SessionId, System.ServiceProcess.SessionChangeReason Reason, SessionProperties properties)
        {
            // Only applies to pGina sessions!
            if (properties != null)
            {
                switch (Reason)
                {
                    case System.ServiceProcess.SessionChangeReason.SessionLogon:
                        ProcessSessionStart(properties);

                        break;
                    case System.ServiceProcess.SessionChangeReason.SessionLogoff:
                        ProcessSessionSignOff(properties);
                        
                        break;
                    case System.ServiceProcess.SessionChangeReason.SessionLock:
                        ProcessSessionLock(properties);

                        break;
                    case System.ServiceProcess.SessionChangeReason.SessionUnlock: 
                        
                        break;
                }
            }
        }

        public void ProcessSessionStart(SessionProperties properties)
        {
            UserInformation userInfo = properties.GetTrackedSingle<UserInformation>();

            UserSession createdSession = new UserSession();
            createdSession.username = userInfo.Username;
            createdSession.sessionId = userInfo.SessionID;
            createdSession.signOnTime = DateTime.Now;
            createdSession.sessionState = 1;

            m_sessions.Add(userInfo.Username, createdSession);

            m_logger.DebugFormat("Session created for {0}", userInfo.Username);
        }

        public void ProcessSessionSignOff(SessionProperties properties)
        {
            UserInformation userInfo = properties.GetTrackedSingle<UserInformation>();

            if (m_sessions.ContainsKey(userInfo.Username))
            {
                m_sessions[userInfo.Username].sessionState = 0;
                m_sessions[userInfo.Username].signOffTime = DateTime.Now;
            }
        }

        public void ProcessSessionLock(SessionProperties properties)
        {
            UserInformation userInfo = properties.GetTrackedSingle<UserInformation>();

            if (m_sessions.ContainsKey(userInfo.Username))
            {
                m_sessions[userInfo.Username].sessionState = 2;
                m_sessions[userInfo.Username].lockedAt = DateTime.Now;
            }
                
        }
    }
}
