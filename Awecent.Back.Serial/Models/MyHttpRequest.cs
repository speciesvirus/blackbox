using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Awecent.Back.Serial.Models
{
    public class MyHttpRequest
    {

        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public string sendHttpRequestAndGetResponse(string url, string parameter)
        {

            string responseData = null;
            int myTimeout = (1000 * 2);
            var code = 0;

            for (int i = 0; i < 3; i++)
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = parameter.Length;
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Timeout = myTimeout;
                try
                {
                    StreamWriter requestWriter = new StreamWriter(request.GetRequestStream());

                    requestWriter.Write(parameter);
                    requestWriter.Close();

                    var response = (HttpWebResponse)request.GetResponse();
                    code = (int)response.StatusCode;
                    if (code == 200)
                    {
                        StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream(), Encoding.UTF8);
                        responseData = responseReader.ReadToEnd();
                        responseReader.Close();
                        response.Close();

                        return responseData;
                        break;
                    }
                    else
                    {
                        response.Close();
                        continue;
                    }
                }
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.Timeout)
                    {
                        // Handle timeout exception
                        //MySQLWebSecurity.LogFileConnection(new myLogFile { IsRequest = false, reponse = "Connection time out form url : " + url + "?" + parameter });
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    // MySQLWebSecurity.LogFileConnection(new myLogFile { IsRequest = false, reponse = "Connection exception : " + ex.Message });
                }
            }

            return responseData;

        }


        public MyHttpResponse Send(string url, string method, string parameter)
        {

            try
            {
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

                WebRequest request = null;
                byte[] byteArray = null;
                if (method == "GET")
                {
                    url = url + parameter;
                    request = WebRequest.Create(url);
                    request.Credentials = CredentialCache.DefaultCredentials;
                    request.Method = method;
                    //string postData = "";
                    //byteArray = Encoding.UTF8.GetBytes(postData);
                }
                else if (method == "POST")
                {
                    request = WebRequest.Create(url);
                    request.Credentials = CredentialCache.DefaultCredentials;
                    request.Method = method;
                    string postData = parameter;
                    //id=558&code=1rjsrf8
                    byteArray = Encoding.UTF8.GetBytes(postData);
                    request.ContentLength = byteArray.Length;
                    request.ContentType = "application/json; charset=utf-8";
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                }

                WebResponse response = request.GetResponse();



                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                int aStatus = (int)((HttpWebResponse)response).StatusCode;
                Stream data = response.GetResponseStream();
                StreamReader reader = new StreamReader(data);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                response.Close();
                return new MyHttpResponse { status = aStatus, response = responseFromServer };

            }
            catch (TimeoutException ex)
            {
                return new MyHttpResponse { status = 408, response = "Request Timeout : " + ex.Message };
            }
            catch (Exception ex)
            {
                return new MyHttpResponse { status = 500, response = "Internal Server Error : " + ex.Message };
            }


        }

    }
}