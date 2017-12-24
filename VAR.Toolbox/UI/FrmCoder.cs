using System;
using System.Windows.Forms;
using VAR.Toolbox.Code;

namespace VAR.Toolbox.UI
{
    public partial class FrmCoder : Form
    {
        public FrmCoder()
        {
            InitializeComponent();

            cboCode.SelectedItem = "Base64";
            
        }

        private ICoder _coder = null;
        
        private void btnDecodeBase64_Click(object sender, EventArgs e)
        {
            string output = string.Empty;
            try
            {
                output = _coder.Decode(txtInput.Text, txtKey.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            txtOutput.Text = output;
        }
        
        private void btnEncodeBase64_Click(object sender, EventArgs e)
        {
            string output = string.Empty;
            try
            {
                output = _coder.Encode(txtInput.Text, txtKey.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            txtOutput.Text = output;
        }

        private void btnSwap_Click(object sender, EventArgs e)
        {
            string temp = txtOutput.Text;
            txtOutput.Text = txtInput.Text;
            txtInput.Text = temp;
        }

        private void cboCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string code = (string)cboCode.SelectedItem;
            if(code == "Base64")
            {
                txtKey.Enabled = false;
                _coder = new CoderBase64();
                return;
            }
        }
    }
}
