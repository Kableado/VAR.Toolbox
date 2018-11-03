using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VAR.Toolbox.UI
{
    public partial class FrmTestRestService : Form
    {
        public FrmTestRestService()
        {
            InitializeComponent();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                string url = txtUrl.Text;
                string urlApiMethod = txtUrlApiMethod.Text;
                Dictionary<string, string> parms = StringToDictionary(txtParameters.Text);
                string body = txtBody.Text;

                string result = CallApi(url, urlApiMethod, parms, body);

                txtResult.Text = result;
            }
            catch (Exception ex)
            {
                StringBuilder sbException = new StringBuilder();
                while (ex != null)
                {
                    sbException.AppendFormat("{0}\r\n{1}\r\n\r\n", ex.Message, ex.StackTrace);
                    ex = ex.InnerException;
                }
                txtResult.Text = sbException.ToString();
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

        private static CookieContainer _cookieJar = new CookieContainer();

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
