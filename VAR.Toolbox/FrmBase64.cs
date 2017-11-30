using System;
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
        
        static public string Base64Encode(string toEncode)
        {
            byte[] toEncodeAsBytes
                  = Encoding.ASCII.GetBytes(toEncode);
            string returnValue
                  = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
        
        static public string Base64Decode(string encodedData)
        {
            byte[] encodedDataAsBytes
                = System.Convert.FromBase64String(encodedData);
            string returnValue =
               Encoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }

        private void btnDecodeBase64_Click(object sender, EventArgs e)
        {
            txtOutput.Text = Base64Decode(txtInput.Text);
        }
        
        private void btnEncodeBase64_Click(object sender, EventArgs e)
        {
            txtOutput.Text = Base64Encode(txtInput.Text);
        }
    }
}
