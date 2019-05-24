using System;
using System.Windows.Forms;
using VAR.Toolbox.Code;

namespace VAR.Toolbox.UI
{
    public partial class FrmCoder : Form, IToolForm
    {
        public string ToolName { get { return "Coder"; } }

        public bool HasIcon { get { return false; } }

        public FrmCoder()
        {
            InitializeComponent();

            cboCode.Items.AddRange(TextCoderFactory.GetSupportedCoders());
            cboCode.SelectedIndex = 1;
        }

        private ITextCoder _coder = null;

        private void BtnDecode_Click(object sender, EventArgs e)
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

        private void BtnEncode_Click(object sender, EventArgs e)
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

        private void BtnSwap_Click(object sender, EventArgs e)
        {
            string temp = txtOutput.Text;
            txtOutput.Text = txtInput.Text;
            txtInput.Text = temp;
        }

        private void CboCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string code = (string)cboCode.SelectedItem;
            _coder = TextCoderFactory.CreateFromName(code);
            txtKey.Enabled = _coder.NeedsKey;
        }
    }
}
