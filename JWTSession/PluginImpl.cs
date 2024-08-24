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
        private SessionCache m_cache;
        private Timer m_timer;

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
            m_cache = new SessionCache();
            StartTimer();
        }

        public void Stopping()
        {
            StopTimer();
            m_cache = null;
        }

        private void StartTimer()
        {
            m_logger.Debug("Starting timer");
            m_timer = new Timer(new TimerCallback(SessionLimitTimerCallback), null, TimeSpan.FromSeconds(0),
                TimeSpan.FromSeconds(60));
        }

        private void StopTimer()
        {
            m_logger.Debug("Stopping timer");
            m_timer.Dispose();
            m_timer = null;
        }

        private void SessionLimitTimerCallback(object state)
        {
            int limit = Settings.Store.GlobalLimit;

            if (limit > 0)
            {
                m_logger.Debug("Checking for sessions to logoff");
                List<int> sessions = m_cache.SessionsLoggedOnLongerThan(TimeSpan.FromMinutes(limit));
                m_logger.DebugFormat("Found {0} sessions.", sessions.Count);
                foreach (int sess in sessions)
                {
                    m_logger.DebugFormat("Logging off session {0}", sess);
                    bool result = Abstractions.WindowsApi.pInvokes.LogoffSession(sess);
                    if (result)
                        m_logger.Debug("Log off successful.");
                    else
                        m_logger.Debug("Log off failed.");
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

            return HttpAccessor.getResponse(userInfo.Username, userInfo.Password);
        }

        public BooleanResult AuthenticatedUserGateway(SessionProperties properties)
        {
            // this method shall perform some other tasks ...

            UserInformation userInfo = properties.GetTrackedSingle<UserInformation>();

            UInfo uinfo = HttpAccessor.getUserInfo(userInfo.Username);
            if (uinfo != null)
            {
                m_logger.DebugFormat("AuthenticatedUserGateway: Uinfo: {0}", uinfo.ToString());
                foreach (string group in uinfo.groups)
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

            return HttpAccessor.getPwChangeResponse(userInfo.Username, userInfo.Password, userInfo.oldPassword);
        }

        public void SessionChange(int SessionId, System.ServiceProcess.SessionChangeReason Reason, SessionProperties properties)
        {
            // Only applies to pGina sessions!
            if (properties != null)
            {
                switch (Reason)
                {
                    case System.ServiceProcess.SessionChangeReason.SessionLogon:
                        LogonEvent(SessionId);
                        break;
                    case System.ServiceProcess.SessionChangeReason.SessionLogoff:
                        LogoffEvent(SessionId);
                        break;
                }
            }
        }

        private void LogonEvent(int sessId)
        {
            m_logger.DebugFormat("LogonEvent: {0}", sessId);
            m_cache.Add(sessId);
        }

        private void LogoffEvent(int sessId)
        {
            m_logger.DebugFormat("LogoffEvent: {0}", sessId);
            m_cache.Remove(sessId);
        }
    }
}
