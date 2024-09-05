using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace pGina.Plugin.JWTSession
{
    [Serializable]
    internal class LoginRequest
    {
        public string username;
        public string password;
        public string hostname;
    }

    [Serializable]
    public class LoginResponse
    {
        public string error;
        public string username;
        public string password;
        public string accountName;
        public string email;
        public string[] groups;
    }

    [Serializable]
    public class LoginResponseToken
    {
        public string token;
    }

    [Serializable]
    internal class ChangePasswordRequest
    {
        public string username;
        public string password;
        public string oldPassword;
    }

    [Serializable]
    internal class ChangePasswordResponse
    {
        public string error;
    }

    [Serializable]
    internal class SessionCheckInRequest
    {
        public string username;
    }

    [Serializable]
    public class SessionCheckInResponse
    {
        public string error;
    }
}
