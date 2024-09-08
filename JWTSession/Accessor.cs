using System;
using System.Collections.Generic;
using System.Net;
using System.IO;

using pGina.Shared.Types;
using log4net;
using System.Text;
using Newtonsoft.Json;

namespace pGina.Plugin.JWTSession
{
    public class JsonAccessor
    {
        private static Dictionary<string, LoginTokenResponse> resps = new Dictionary<string, LoginTokenResponse>();
        private static ILog m_logger = LogManager.GetLogger("JWTSessionAccessor");

        static JsonAccessor()
        {
        }

        public static BooleanResult getResponse(String uname, String pwd)
        {
            try
            {
                // Create a request using a URL that can receive a post. 
                WebRequest request = WebRequest.Create(Settings.getLoginServer());
                // Set the Method property of the request to POST.
                request.Method = "POST";
                request.Timeout = 2000;

                LoginRequest loginRequest = new LoginRequest();
                loginRequest.username = uname;
                loginRequest.password = pwd;
                loginRequest.hostname = Environment.GetEnvironmentVariable("COMPUTERNAME");

                // Convert object to json
                string postData = JsonConvert.SerializeObject(loginRequest);

                // Convert json to byte array
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/json";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }
    
                // Get the response.
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access.
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            // Read the content.
                            string responseFromServer = reader.ReadToEnd();

                            // Display the content.
                            m_logger.InfoFormat("Response: {0}", responseFromServer);

                            // save it for later use
                            if (resps.ContainsKey(uname))
                            {
                                resps.Remove(uname);
                            }

                            LoginTokenResponse loginTokenResponse = JsonConvert.DeserializeObject<LoginTokenResponse>(responseFromServer);

                            if (loginTokenResponse.error != null)
                                return new BooleanResult() { Success = false, Message = loginTokenResponse.error };

                            resps.Add(uname, loginTokenResponse);
                        }
                    }
                }

                return new BooleanResult() { Success = true };
            }
            catch(WebException webx)
            {
                m_logger.ErrorFormat("Accessor.WebException: {0}", webx.Message);

                using (HttpWebResponse res = (HttpWebResponse)webx.Response)
                {
                    if (res != null)
                    {
                        using (StreamReader resReader = new StreamReader(res.GetResponseStream()))
                        {
                            string responseBody = resReader.ReadLine();
                            if (responseBody.Length > 0)
                            {
                                return new BooleanResult() { Success = false, Message = responseBody };
                            }
                        }
                    }
                }

                return new BooleanResult() { Success = false, Message = webx.Message };
            }
            catch (Exception e)
            {
                // very bad scenario
                m_logger.ErrorFormat("Accessor.Exception: {0}", e.StackTrace);
                return new BooleanResult() { Success = false, Message = e.Message };
            }
            
        }

        public static BooleanResult getPwChangeResponse(String uname, String pwd, String old)
        {
            try
            {
                // Create a request using a URL that can receive a post. 
                WebRequest request = WebRequest.Create(Settings.getLoginServerPwdChange());
                // Set the Method property of the request to POST.
                request.Method = "POST";
                request.Timeout = 2000;

                // Append token if exists
                if (!resps.ContainsKey(uname))
                {
                    LoginTokenResponse storedToken = resps[uname];

                    request.Headers.Add("authorization", "Bearer " + storedToken.token);
                }

                // Create password change object
                ChangePasswordRequest changePwd = new ChangePasswordRequest();
                changePwd.username = uname;
                changePwd.password = pwd;
                changePwd.oldPassword = old;

                // Create POST data and convert it to a byte array.
                string postData = JsonConvert.SerializeObject(changePwd);

                // Convert json to bytes
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/json";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                // Get the response.
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access.
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            // Read the content.
                            string responseFromServer = reader.ReadToEnd();

                            // Display the content.
                            m_logger.InfoFormat("PWDCHResponse: {0}", responseFromServer);

                            ChangePasswordResponse changePwdResp = JsonConvert.DeserializeObject<ChangePasswordResponse>(responseFromServer);

                            if (changePwdResp.error != null)
                            {
                                return new BooleanResult() { Success = false, Message = changePwdResp.error };
                            }

                            return new BooleanResult() { Success = true };
                        }
                    }
                }
            }
            catch (WebException webx)
            {
                m_logger.ErrorFormat("PWDCHAccessor.WebException: {0}", webx.Message);

                using (HttpWebResponse res = (HttpWebResponse)webx.Response)
                {
                    if (res != null)
                    {
                        using (StreamReader resReader = new StreamReader(res.GetResponseStream()))
                        {
                            string responseBody = resReader.ReadLine();
                            if (responseBody.Length > 0)
                            {
                                return new BooleanResult() { Success = false, Message = responseBody };
                            }
                        }
                    }
                }

                return new BooleanResult() { Success = false, Message = webx.Message };
            }
            catch (Exception e)
            {
                // very bad scenario
                m_logger.ErrorFormat("PWDCHAccessor.Exception: {0}", e.StackTrace);
                return new BooleanResult() { Success = false, Message = e.Message };
            }

        }

        public static SessionCheckInResponse checkActiveSession(UserSession sessionObj)
        {
            try
            {
                // Create a request using a URL that can receive a post. 
                WebRequest request = WebRequest.Create(Settings.getLoginServerSession());
                // Set the Method property of the request to POST.
                request.Method = "POST";
                request.Timeout = 2000;

                // Append token if exists
                if (!resps.ContainsKey(sessionObj.username))
                {
                    LoginTokenResponse storedToken = resps[sessionObj.username];

                    request.Headers.Add("authorization", "Bearer " + storedToken.token);
                }

                // Session check in request
                SessionCheckInRequest sessionCheckIn = new SessionCheckInRequest();
                sessionCheckIn.username = sessionObj.username;
                sessionCheckIn.hostname = Environment.GetEnvironmentVariable("COMPUTERNAME");
                sessionCheckIn.state = sessionObj.sessionState;

                sessionCheckIn.signOnTime = Math.Floor(sessionObj.signOnTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                sessionCheckIn.signOffTime = Math.Floor(sessionObj.signOffTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                sessionCheckIn.lockedAt = Math.Floor(sessionObj.lockedAt.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);

                // Create POST data and convert it to a byte array.
                string postData = JsonConvert.SerializeObject(sessionCheckIn);

                // Convert json to bytes
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/json";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                // Get the response.
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access.
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            // Read the content.
                            string responseFromServer = reader.ReadToEnd();

                            // Display the content.
                            m_logger.InfoFormat("SessionCheckInResponse: {0}", responseFromServer);

                            SessionCheckInResponse sessionResp = JsonConvert.DeserializeObject<SessionCheckInResponse>(responseFromServer);

                            return sessionResp;
                        }
                    }
                }
            }
            catch (WebException webx)
            {
                m_logger.ErrorFormat("PWDCHAccessor.WebException: {0}", webx.Message);

                using (HttpWebResponse res = (HttpWebResponse)webx.Response)
                {
                    if (res != null)
                    {
                        using (StreamReader resReader = new StreamReader(res.GetResponseStream()))
                        {
                            string responseBody = resReader.ReadLine();
                            if (responseBody.Length > 0)
                            {
                                return new SessionCheckInResponse() { error = responseBody };
                            }
                        }
                    }
                }

                return new SessionCheckInResponse() { error = webx.Message };
            }
            catch (Exception e)
            {
                // very bad scenario
                m_logger.ErrorFormat("PWDCHAccessor.Exception: {0}", e.StackTrace);

                return new SessionCheckInResponse() { error = e.Message };
            }

        }

        public static LoginTokenResponse getUserInfo(String uname)
        {
            if (! resps.ContainsKey(uname))
            {
                return null;
            }
            return resps[uname];
        }
    }
}
