using System;
using System.Collections.Generic;

namespace pGina.Plugin.JWTSession
{
    class SessionCache
    {
        Dictionary<int, DateTime> m_cache;

        public SessionCache()
        {
            m_cache = new Dictionary<int, DateTime>();
        }

        public void Add(int sessId)
        {
            lock (this)
            {
                if (m_cache.ContainsKey(sessId))
                    m_cache[sessId] = DateTime.Now;
                else
                    m_cache.Add(sessId, DateTime.Now);
            }
        }

        public void Remove(int sessId)
        {
            lock (this)
            {
                m_cache.Remove(sessId);
            }
        }

        private TimeSpan LoggedInTimeSpan(int sessId)
        {
            if (m_cache.ContainsKey(sessId))
                return DateTime.Now - m_cache[sessId];
            else
                return TimeSpan.Zero;
        }

        public List<int> SessionsLoggedOnLongerThan(TimeSpan span)
        {
            lock (this)
            {
                List<int> sessionList = new List<int>();

                foreach (int sessId in m_cache.Keys)
                {
                    TimeSpan sessSpan = LoggedInTimeSpan(sessId);
                    if (sessSpan >= span)
                    {
                        sessionList.Add(sessId);
                    }
                }
                return sessionList;
            }
        }
    }
}