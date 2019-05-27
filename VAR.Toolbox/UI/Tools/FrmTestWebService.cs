﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace VAR.Toolbox.UI
{
    public partial class FrmTestWebService : Form, IToolForm
    {
        public string ToolName { get { return "TestWebService"; } }

        public bool HasIcon { get { return false; } }

        public FrmTestWebService()
        {
            InitializeComponent();
        }

        private void BtnTestSoap_Click(object sender, EventArgs e)
        {
            try
            {
                string url = txtUrlSoap.Text;
                string iface = txtInterfaceSoap.Text;
                string method = txtMethodSoap.Text;
                Dictionary<string, string> parms = StringToDictionary(txtParametersSoap.Text);

                Dictionary<string, string> result = CallSoapMethod(url, iface, method, parms);

                txtResultSoap.Text = DictionaryToString(result);
            }
            catch (Exception ex)
            {
                StringBuilder sbException = new StringBuilder();
                while (ex != null)
                {
                    sbException.AppendFormat("{0}\r\n{1}\r\n\r\n", ex.Message, ex.StackTrace);
                    ex = ex.InnerException;
                }
                txtResultSoap.Text = sbException.ToString();
            }
        }

        private void BtnTestRest_Click(object sender, EventArgs e)
        {
            try
            {
                string url = txtUrlRest.Text;
                string urlApiMethod = txtUrlApiMethodRest.Text;
                Dictionary<string, string> parms = StringToDictionary(txtParametersRest.Text);
                string body = txtBodyRest.Text;

                string result = CallApi(url, urlApiMethod, parms, body);

                txtResultRest.Text = result;
            }
            catch (Exception ex)
            {
                StringBuilder sbException = new StringBuilder();
                while (ex != null)
                {
                    sbException.AppendFormat("{0}\r\n{1}\r\n\r\n", ex.Message, ex.StackTrace);
                    ex = ex.InnerException;
                }
                txtResultRest.Text = sbException.ToString();
            }
        }

        /// <summary>
        /// Deseria una cadena a un diccionario string,string
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        /// <author>VAR</author>
        public static Dictionary<string, string> StringToDictionary(string str)
        {
            var dic = new Dictionary<string, string>();
            List<string> pairs = SplitUnscaped(str, ',');
            foreach (string pair in pairs)
            {
                List<string> values = SplitUnscaped(pair, ':');
                if (values.Count < 2) continue;
                string key = values[0].Replace("\\:", ":").Replace("\\,", ",");
                string val = values[1].Replace("\\:", ":").Replace("\\,", ",");
                dic.Add(key, val);
            }
            return dic;
        }

        /// <summary>
        /// Serializa un diccionario string,string a una cadena.
        /// </summary>
        /// <param name="dic">The dic.</param>
        /// <returns></returns>
        /// <author>VAR</author>
        public static string DictionaryToString(Dictionary<string, string> dic)
        {
            var sb = new StringBuilder();
            foreach (KeyValuePair<string, string> entrada in dic)
            {
                string sKey = entrada.Key.Replace(":", "\\:").Replace(",", "\\,");
                string sVal = entrada.Value.Replace(":", "\\:").Replace(",", "\\,");
                sb.AppendFormat("{0}:{1},", sKey, sVal);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Parte una cadena usando un caracter, evitando usar las ocurrencias escapadas con '\\'
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="splitter">The splitter.</param>
        /// <returns></returns>
        /// <author>VAR</author>
        public static List<string> SplitUnscaped(string str, char splitter)
        {
            var strs = new List<string>();
            int j, i;
            int n = str.Length;

            for (j = 0, i = 0; i < n; i++)
            {
                if (str[i] == '\\') i++;
                else if (str[i] == splitter)
                {
                    strs.Add(str.Substring(j, i - j));
                    j = i + 1;
                }
            }
            if (i >= j) strs.Add(str.Substring(j, n - j));

            return strs;
        }

        /// <summary>
        /// Llama a un metodo SOAP. Esto requiere que el binding del servicio WCF sea de tipo "basicHttpBinding"
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="iface">The iface.</param>
        /// <param name="method">The method.</param>
        /// <param name="parms">The parms.</param>
        /// <returns></returns>
        /// <date>12/05/2014</date>
        /// <author>VAR</author>
        public static Dictionary<string, string> CallSoapMethod(string url, string iface, string method, Dictionary<string, string> parms)
        {
            // Los servicios SOAP se llaman siempre a traves de HTTP.
            if (url.ToLower().StartsWith("https://"))
            {
                url = string.Format("http://{0}", url.Substring("https://".Length));
            }

            // Construir peticion
            var sbData = new StringBuilder();
            sbData.AppendFormat("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>\n");
            sbData.AppendFormat("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">\n");
            sbData.AppendFormat("<s:Body>\n");
            sbData.AppendFormat("<{0} xmlns=\"http://tempuri.org/\">\n", method);
            foreach (KeyValuePair<string, string> parm in parms)
            {
                sbData.AppendFormat("<{0}>{1}</{0}>\n", parm.Key, parm.Value);

                // FIXME: Accept null values
                //sbData.AppendFormat("<{0} i:nil=\"true\" xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" />\n", parm.Key);
            }
            sbData.AppendFormat("</{0}>\n", method);
            sbData.AppendFormat("</s:Body>\n");
            sbData.AppendFormat("</s:Envelope>\n");
            byte[] postData = Encoding.UTF8.GetBytes(sbData.ToString());

            // Realizar peticion
            var client = new System.Net.WebClient();
            client.Headers.Add("Accept", "text/xml");
            client.Headers.Add("Accept-Charset", "UTF-8");
            client.Headers.Add("Content-Type", "text/xml;  charset=UTF-8");
            client.Headers.Add("SOAPAction", string.Format("\"{0}/{1}/{2}\"", "http://tempuri.org", iface, method));
            byte[] data;
            try
            {
                data = client.UploadData(url, "POST", postData);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Failure calling SoapService: URL: {0}", url), ex);
            }
            string strData = System.Text.Encoding.UTF8.GetString(data);
            var strReader = new StringReader(strData);
            var xmlReader = new XmlTextReader(strReader);

            // Parsear resultado
            Dictionary<string, string> resultObject = new Dictionary<string, string>();
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    string name = xmlReader.Name;
                    if (name.Contains(":"))
                    {
                        name = name.Split(':')[1];
                    }
                    resultObject.Add(name, xmlReader.ReadString());
                }
            }
            return resultObject;
        }


        private static readonly CookieContainer _cookieJar = new CookieContainer();

        public static string CallApi(string urlService, string urlApiMethod, Dictionary<string, string> prms, string content)
        {
            var sbRequestUrl = new StringBuilder();
            sbRequestUrl.Append(urlService);
            sbRequestUrl.Append(urlApiMethod);
            if (prms != null)
            {
                foreach (KeyValuePair<string, string> pair in prms)
                {
                    sbRequestUrl.AppendFormat("&{0}={1}", pair.Key, Uri.EscapeUriString(pair.Value));
                }
            }
            if (sbRequestUrl.Length > 2048)
            {
                throw new Exception(string.Format("CallApi: Request URL longer than 2048: url: \"{0}\"", sbRequestUrl.ToString()));
            }

            var http = (HttpWebRequest)WebRequest.Create(new Uri(sbRequestUrl.ToString()));
            http.CookieContainer = _cookieJar;
            http.Accept = "application/json";
            http.ContentType = "application/json; charset=utf-8";
            http.Method = "POST";

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] bytes = encoding.GetBytes(content);

            Task<Stream> requestStreamTask = http.GetRequestStreamAsync();
            requestStreamTask.Wait();
            Stream requestStream = requestStreamTask.Result;
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Flush();

            Task<WebResponse> responseTask = http.GetResponseAsync();
            responseTask.Wait();
            WebResponse response = responseTask.Result;
            var stream = response.GetResponseStream();
            var sr = new StreamReader(stream);
            return sr.ReadToEnd();
        }

    }
}
