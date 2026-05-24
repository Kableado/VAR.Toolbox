using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VAR.Toolbox.Code
{
    public static class WebServicesUtils
    {
        private static readonly CookieContainer _cookieJar = new();

        public static string CallApi(string? urlService, string urlApiMethod, Dictionary<string, string>? parameters,
            object? content, CookieContainer? cookieJar = null, string? stringContent = null,
            Dictionary<string, string>? customHeaders = null, string verb = "POST",
            bool disableCertificateValidation = false)
        {
            if (urlService?.StartsWith("!") == true)
            {
                urlService = urlService.Substring(1);
                disableCertificateValidation = true;
            }

            cookieJar ??= _cookieJar;

            StringBuilder sbRequestUrl = new();
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
                    sbRequestUrl.Append($"&{pair.Key}={HttpServer.HttpUtility.UrlEncode(pair.Value)}");
                }
            }

            if (sbRequestUrl.Length > 2048)
            {
                throw new Exception($"CallApi: Request URL longer than 2048: url: \"{sbRequestUrl}\"");
            }

            HttpClientHandler handler = new() { CookieContainer = cookieJar, };
#if UNIFIKAS_COMMONS
                if (disableCertificateValidation)
                {
                    handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;
                }
#else
            if (disableCertificateValidation)
            {
                throw new NotImplementedException("ApiHelper.CallApi: Can't disable certificate validation");
            }
#endif
            using HttpClient client = new(handler);
            client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            if (customHeaders != null)
            {
                foreach (KeyValuePair<string, string> customHeader in customHeaders)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(customHeader.Key, customHeader.Value);
                }
            }

            HttpResponseMessage response;
            Uri requestUri = new(sbRequestUrl.ToString());

            if (verb == "POST")
            {
                string parsedContent = Json.JsonWriter.WriteObject(content ?? new object());
                if (string.IsNullOrEmpty(stringContent) == false)
                {
                    parsedContent = stringContent;
                }

                StringContent httpContent = new(parsedContent, Encoding.UTF8, "application/json");
                Task<HttpResponseMessage> responseTask = client.PostAsync(requestUri, httpContent);
                responseTask.Wait();
                response = responseTask.Result;
            }
            else
            {
                HttpRequestMessage request = new(new HttpMethod(verb), requestUri);
                Task<HttpResponseMessage> responseTask = client.SendAsync(request);
                responseTask.Wait();
                response = responseTask.Result;
            }

            Task<string> readTask = response.Content.ReadAsStringAsync();
            readTask.Wait();
            return readTask.Result;
        }

        public static string CallSoapMethod(string url, string method, Dictionary<string, object> parameters,
            string namespaceUrl = "http://tempuri.org", ICredentials? credentials = null)
        {
            // Los servicios SOAP se llaman siempre a través de HTTP.
            if (url.ToLower().StartsWith("https://"))
            {
                url = $"http://{url.Substring("https://".Length)}";
            }

            // Construir petición
            StringBuilder sbData = new();
            sbData.AppendFormat("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            sbData.AppendFormat(
                "<soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">");
            sbData.AppendFormat("<soap:Body>");
            sbData.Append($"<{method} xmlns=\"{namespaceUrl}\">");
            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                if (!(parameter.Value is DBNull))
                {
                    sbData.AppendFormat("<{0}>{1}</{0}>", parameter.Key, parameter.Value);
                }
                else
                {
                    sbData.Append(
                        $"<{parameter.Key} i:nil=\"true\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" />");
                }
            }

            sbData.Append($"</{method}>");
            sbData.AppendFormat("</soap:Body>");
            sbData.AppendFormat("</soap:Envelope>");
            Console.WriteLine(sbData.ToString());
            byte[] postData = Encoding.UTF8.GetBytes(sbData.ToString());

            // Realizar petición
            HttpClientHandler handler = new();
            if (credentials != null)
            {
                handler.Credentials = credentials;
            }

            using HttpClient client = new(handler);
            client.DefaultRequestHeaders.Accept.ParseAdd("text/xml");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "UTF-8");
            client.DefaultRequestHeaders.TryAddWithoutValidation("SOAPAction",
                namespaceUrl.ToLower().StartsWith("http")
                    ? $"\"{namespaceUrl}/{method}\""
                    : $"\"{namespaceUrl}:{method}\"");

            ByteArrayContent content = new(postData);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/xml") { CharSet = "UTF-8", };

            byte[] data;
            try
            {
                Task<HttpResponseMessage> responseTask = client.PostAsync(url, content);
                responseTask.Wait();
                HttpResponseMessage response = responseTask.Result;
                Task<byte[]> dataTask = response.Content.ReadAsByteArrayAsync();
                dataTask.Wait();
                data = dataTask.Result;
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