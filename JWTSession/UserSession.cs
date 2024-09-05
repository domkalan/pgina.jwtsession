using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
