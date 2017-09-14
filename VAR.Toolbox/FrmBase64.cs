using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VAR.Toolbox
{
    public partial class FrmBase64 : Form
    {
        public FrmBase64()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Codifica una cadena en base64.
        /// </summary>
        /// <param name="toEncode">Cadena a codificar</param>
        /// <returns>Cadena codificada</returns>
        static public string Base64Encode(string toEncode)
        {
            byte[] toEncodeAsBytes
                  = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue
                  = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        /// <summary>
        /// Decodifica una cadena desde base64.
        /// </summary>
        /// <param name="encodedData">Cadena codificada</param>
        /// <returns>Cadena decodificada</returns>
        static public string Base64Decode(string encodedData)
        {
            byte[] encodedDataAsBytes
                = System.Convert.FromBase64String(encodedData);
            string returnValue =
               System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }

        private void btnDecodeBase64_Click(object sender, EventArgs e)
        {
            txtOutput.Text = Base64Decode(txtInput.Text);
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void btnEncodeBase64_Click(object sender, EventArgs e)
        {
            txtOutput.Text = Base64Encode(txtInput.Text);
        }
    }
}
