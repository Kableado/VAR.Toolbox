using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VAR.Toolbox.Code
{
    public static class WebServicesUtils
    {
        private static readonly CookieContainer _cookieJar = new CookieContainer();

        public static string CallApi(string urlService, string urlApiMethod, Dictionary<string, string> parameters,
            object content, CookieContainer cookieJar = null, string stringContent = null,
            Dictionary<string, string> customHeaders = null, string verb = "POST",
            bool disableCertificateValidation = false)
        {
            if (urlService?.StartsWith("!") == true)
            {
                urlService = urlService.Substring(1);
                disableCertificateValidation = true;
            }

            if (cookieJar == null) { cookieJar = _cookieJar; }

            try
            {
                var sbRequestUrl = new StringBuilder();
                sbRequestUrl.Append(urlService);
                if (urlService != null && urlService.EndsWith("/") && urlApiMethod.StartsWith("/"))
                {
                    sbRequestUrl.Append(urlApiMethod.Substring(1));
                }
                else
                {
                    sbRequestUrl.Append(urlApiMethod);
                }

                if (parameters != null)
                {
                    foreach (KeyValuePair<string, string> pair in parameters)
                    {
                        sbRequestUrl.AppendFormat("&{0}={1}", pair.Key,
                            pair.Value == null ? string.Empty : HttpServer.HttpUtility.UrlEncode(pair.Value));
                    }
                }

                if (sbRequestUrl.Length > 2048)
                {
                    throw new Exception($"CallApi: Request URL longer than 2048: url: \"{sbRequestUrl}\"");
                }

                var http = (HttpWebRequest)WebRequest.Create(new Uri(sbRequestUrl.ToString()));

#if UNIFIKAS_COMMONS
                if (disableCertificateValidation)
                {
                    http.ServerCertificateValidationCallback =
 (sender, certificate, chain, sslPolicyErrors) => { return true; };
                }
#else
                if (disableCertificateValidation)
                {
                    throw new NotImplementedException("ApiHelper.CallApi: Can't disable certificate validation");
                }
#endif
                http.CookieContainer = cookieJar;
                http.Accept = "application/json";
                http.ContentType = "application/json; charset=utf-8";
                http.Method = verb;
                if (customHeaders != null)
                {
                    foreach (KeyValuePair<string, string> customHeader in customHeaders)
                    {
                        http.Headers[customHeader.Key] = customHeader.Value;
                    }
                }

                if (verb == "POST")
                {
                    string parsedContent = Json.JsonWriter.WriteObject(content);
                    if (string.IsNullOrEmpty(stringContent) == false)
                    {
                        parsedContent = stringContent;
                    }

                    UTF8Encoding encoding = new UTF8Encoding();
                    byte[] bytes = encoding.GetBytes(parsedContent);

                    Task<Stream> requestStreamTask = http.GetRequestStreamAsync();
                    requestStreamTask.Wait();
                    Stream requestStream = requestStreamTask.Result;
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Flush();
                }

                Task<WebResponse> responseTask = http.GetResponseAsync();
                responseTask.Wait();
                WebResponse response = responseTask.Result;
                var stream = response.GetResponseStream();
                if (stream == null) { return null; }

                var sr = new StreamReader(stream);
                return sr.ReadToEnd();
            }
            catch (Exception ex)
            {
                // ReSharper disable once PossibleIntendedRethrow
                throw ex;
            }
        }

        public static string CallSoapMethod(string url, string method, Dictionary<string, object> parameters,
            string namespaceUrl = "http://tempuri.org", ICredentials credentials = null)
        {
            // Los servicios SOAP se llaman siempre a través de HTTP.
            if (url.ToLower().StartsWith("https://"))
            {
                url = $"http://{url.Substring("https://".Length)}";
            }

            // Construir petición
            var sbData = new StringBuilder();
            sbData.AppendFormat("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            sbData.AppendFormat(
                "<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            sbData.AppendFormat("<soap:Body>");
            sbData.AppendFormat("<{0} xmlns=\"{1}\">", method, namespaceUrl);
            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                if (parameter.Value != null && !(parameter.Value is DBNull))
                {
                    sbData.AppendFormat("<{0}>{1}</{0}>", parameter.Key, parameter.Value);
                }
                else
                {
                    sbData.AppendFormat("<{0} i:nil=\"true\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" />",
                        parameter.Key);
                }
            }

            sbData.AppendFormat("</{0}>", method);
            sbData.AppendFormat("</soap:Body>");
            sbData.AppendFormat("</soap:Envelope>");
            Console.WriteLine(sbData.ToString());
            byte[] postData = Encoding.UTF8.GetBytes(sbData.ToString());

            // Realizar petición
            var client = new WebClient();
            if (credentials != null)
            {
                client.Credentials = credentials;
            }

            client.Headers.Add("Accept", "text/xml");
            client.Headers.Add("Accept-Charset", "UTF-8");
            client.Headers.Add("Content-Type", "text/xml;  charset=UTF-8");
            client.Headers.Add("SOAPAction",
                namespaceUrl.ToLower().StartsWith("http")
                    ? $"\"{namespaceUrl}/{method}\""
                    : $"\"{namespaceUrl}:{method}\"");

            byte[] data;
            try
            {
                data = client.UploadData(url, "POST", postData);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failure calling SoapService: URL: {url}", ex);
            }

            string strData = Encoding.UTF8.GetString(data);
            return strData;
        }
    }
}