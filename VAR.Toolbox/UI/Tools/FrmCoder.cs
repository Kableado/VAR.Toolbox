using System;
using System.Linq;
using System.Windows.Forms;
using VAR.Toolbox.Code.TextCoders;
using VAR.Toolbox.Controls;

namespace VAR.Toolbox.UI.Tools
{
    public partial class FrmCoder : Frame, IToolForm
    {
        public string ToolName => "Coder";

        public bool HasIcon => false;

        public FrmCoder()
        {
            InitializeComponent();

            cboCode.Items.AddRange(TextCoderFactory.GetNames().ToArray<object>());
            cboCode.SelectedIndex = 1;
        }

        private ITextCoder _coder;

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
            (txtOutput.Text, txtInput.Text) = (txtInput.Text, txtOutput.Text);
        }

        private void CboCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            string code = (string)cboCode.SelectedItem;
            _coder = TextCoderFactory.CreateFromName(code);
            txtKey.Enabled = _coder.NeedsKey;
        }
    }
}