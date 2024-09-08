using System;

namespace pGina.Plugin.JWTSession
{
    public class UserSession
    {
        public int sessionId;
        public string username;

        public DateTime signOnTime;
        public DateTime lockedAt;

        public bool managedSession = true;
    }
}
