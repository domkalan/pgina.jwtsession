using Newtonsoft.Json;
using System;
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
    public class LoginPayload
    {
        public string username;
        public string password;
        public string accountName;
        public string email;
        public string[] groups;
    }

    [Serializable]
    public class LoginTokenResponse
    {
        public string error;
        public string token;

        public LoginPayload ExtractToken()
        {
            
            string[] tokenParts = token.Split('.');

            if (tokenParts.Length != 3)
                throw new Exception("JSONWebToken did not contain valid parts");


            // Adapted from https://stackoverflow.com/questions/62111548/could-not-decode-jwt-payload-from-base64
            string tokenValue = tokenParts[1];
            tokenValue = tokenValue.Replace('_', '/').Replace('-', '+');

            switch (tokenValue.Length % 4)
            {
                case 2: tokenValue += "=="; break;
                case 3: tokenValue += "="; break;
            }

            byte[] decodedBase64 = Convert.FromBase64String(tokenValue);
            string decodedToken = System.Text.Encoding.Default.GetString(decodedBase64);

            return JsonConvert.DeserializeObject<LoginPayload>(decodedToken);
        }
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
        public string hostname;

        public int state;

        public double signOnTime;
        public double signOffTime;
        public double lockedAt;
    }

    [Serializable]
    public class SessionCheckInResponse
    {
        public string error;
        public string action;
        public string actionContext;
    }
}
