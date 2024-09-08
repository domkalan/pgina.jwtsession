using System;
using System.Collections.Generic;

using pGina.Shared.Settings;
using log4net;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace pGina.Plugin.JWTSession
{
    class Settings
    {
        private static dynamic m_settings = new pGinaDynamicSettings(PluginImpl.PluginUuid);
        private static ILog m_logger = LogManager.GetLogger("JWTSessionSettings");
        private static string DEFAULT_URL = "https://pginaloginserver/login";

        static Settings()
        {
            try
            {
                m_settings.SetDefault("Loginserver", @DEFAULT_URL);
                m_settings.SetDefault("ChangePassword", DEFAULT_URL);
                m_settings.SetDefault("Session", DEFAULT_URL);
            }
            catch (Exception)
            {
                // do nothing
            }    
        }

        public static dynamic Store
        {
            get { return m_settings; }
        }

        public static string getLoginServer()
        {
            string loginServer = _urlByEnvVar("PGINALOGINSERVER");
            if (loginServer == null)
            {
                // try to get URL from DNS
                try
                {
                    List<string> entries = _getTxtRecords("pginaloginserver");
                    if (entries.Count > 0)
                    {
                        loginServer = entries[0].ToString();    // gets the first item
                        m_logger.DebugFormat("Login server from DNS: {0}", loginServer);
                        _persist("Loginserver", loginServer);
                    }
                    else
                    {
                        loginServer = m_settings.Loginserver;
                        m_logger.DebugFormat("Login server from GinaSettings: {0}", loginServer);
                    }
                }
                catch (KeyNotFoundException)
                {
                    loginServer = DEFAULT_URL;
                    m_logger.DebugFormat("default Login server url: {0}", loginServer);
                }
                catch (Exception dnsex)
                {
                    m_logger.ErrorFormat("Response: {0}", dnsex.ToString());
                    loginServer = m_settings.Loginserver;
                    m_logger.DebugFormat("Login server from GinaSettings: {0}", loginServer);
                }
            }
            else
            {
                m_logger.DebugFormat("Login server from ENVVar: {0}", loginServer);
                _persist("Loginserver", loginServer);
            }
            return loginServer;
        }

        public static string getLoginServerPwdChange()
        {
            string loginServer = _urlByEnvVar("PGINALOGINSERVERPWD");
            if (loginServer == null)
            {
                // try to get URL from DNS
                try
                {
                    List<string> entries = _getTxtRecords("pginaloginserverpwd");
                    if (entries.Count > 0)
                    {
                        loginServer = entries[0].ToString();    // gets the first item
                        m_logger.DebugFormat("Login server from DNS: {0}", loginServer);
                        _persist("ChangePassword", loginServer);
                    }
                    else
                    {
                        loginServer = m_settings.ChangePassword;
                        m_logger.DebugFormat("Login server from GinaSettings: {0}", loginServer);
                    }
                }
                catch (KeyNotFoundException)
                {
                    loginServer = DEFAULT_URL;
                    m_logger.DebugFormat("default Login server url: {0}", loginServer);
                }
                catch (Exception dnsex)
                {
                    m_logger.ErrorFormat("Response: {0}", dnsex.ToString());
                    loginServer = m_settings.ChangePassword;
                    m_logger.DebugFormat("Login server from GinaSettings: {0}", loginServer);
                }
            }
            else
            {
                m_logger.DebugFormat("Login server from ENVVar: {0}", loginServer);
                _persist("ChangePassword", loginServer);
            }
            return loginServer;
        }

        public static string getLoginServerSession()
        {
            string loginServer = _urlByEnvVar("PGINALOGINSERVERSESSION");
            if (loginServer == null)
            {
                // try to get URL from DNS
                try
                {
                    List<string> entries = _getTxtRecords("pginaloginserversession");
                    if (entries.Count > 0)
                    {
                        loginServer = entries[0].ToString();    // gets the first item
                        m_logger.DebugFormat("Login server from DNS: {0}", loginServer);
                        _persist("Session", loginServer);
                    }
                    else
                    {
                        loginServer = m_settings.ChangePassword;
                        m_logger.DebugFormat("Login server from GinaSettings: {0}", loginServer);
                    }
                }
                catch (KeyNotFoundException)
                {
                    loginServer = DEFAULT_URL;
                    m_logger.DebugFormat("default Login server url: {0}", loginServer);
                }
                catch (Exception dnsex)
                {
                    m_logger.ErrorFormat("Response: {0}", dnsex.ToString());
                    loginServer = m_settings.Session;
                    m_logger.DebugFormat("Login server from GinaSettings: {0}", loginServer);
                }
            }
            else
            {
                m_logger.DebugFormat("Login server from ENVVar: {0}", loginServer);
                _persist("Session", loginServer);
            }
            return loginServer;
        }

        private static void _persist(string key, string url) {
            try
            {
                m_settings.SetSetting(key, url);
            }
            catch (Exception e)
            {
                m_logger.ErrorFormat("Cannot save settings: {0}", e.ToString());
            }
        }

        /*
         * returns PGINALOGINSERVER environment variable content if set, otherwise null.
         * Setting by environment variable allows easy override of login endpoint address.
         */
        private static string _urlByEnvVar(string key)
        {
            try
            {
                return Environment.GetEnvironmentVariable(key);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /*
         * Uses http://www.robertsindall.co.uk/blog/getting-dns-txt-record-using-c-sharp/
         * because c# and whole M$osft is crap and has no tools to resolve TXT recs!!!
         */
        private static List<string> _getTxtRecords(string hostname)
        {
            List<string> txtRecords = new List<string>();
            string output;

            var startInfo = new ProcessStartInfo("nslookup");
            startInfo.Arguments = string.Format("-type=TXT {0}", hostname);
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;

            using (var cmd = Process.Start(startInfo))
            {
                output = cmd.StandardOutput.ReadToEnd();
            }

            MatchCollection matches = Regex.Matches(output, "\"([^\"]*)\"", RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                if (match.Success)
                    txtRecords.Add(match.Groups[1].Value);
            }

            return txtRecords;
        }
    }
}
