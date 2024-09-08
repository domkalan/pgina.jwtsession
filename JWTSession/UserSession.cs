using System;

namespace pGina.Plugin.JWTSession
{
    public class UserSession
    {
        public int sessionId;
        public string username;

        public DateTime signOnTime;
        public DateTime signOffTime;
        public DateTime lockedAt;

        public bool managedSession = true;

        /**
         * 0 - Signed Out
         * 1 - Signed In
         * 2 - Locked
         */
        public int sessionState;
    }
}
