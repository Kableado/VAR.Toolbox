using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VAR.Toolbox.Code;
using VAR.Toolbox.Controls;

namespace VAR.Toolbox.UI
{
    public partial class FrmTestWebService : Frame, IToolForm
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
                string namespaceUrl = txtNamespaceUrlSoap.Text;
                string method = txtMethodSoap.Text;
                Dictionary<string, object> parms = StringToDictionary(txtParametersSoap.Text).ToDictionary(p => p.Key, p => (object)p.Value);

                string result = WebServicesUtils.CallSoapMethod(url, method, parms, namespaceUrl);

                txtResultSoap.Text = result;
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

                string result = WebServicesUtils.CallApi(url, urlApiMethod, parms, null, stringContent: body);

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


    }
}
