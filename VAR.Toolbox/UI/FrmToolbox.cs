using System;
using System.Windows.Forms;

namespace VAR.Toolbox.UI
{
    public partial class FrmToolbox : Form
    {
        public FrmToolbox()
        {
            InitializeComponent();
        }

        private void btnBase64_Click(object sender, EventArgs e)
        {
            var frmBase64 = new FrmBase64();
            frmBase64.Show();
        }

        private void btnProxyCmd_Click(object sender, EventArgs e)
        {
            var frmProxyCmd = new FrmProxyCmd();
            frmProxyCmd.Show();
        }

        private void btnWebcam_Click(object sender, EventArgs e)
        {
            var frmWebcam = new FrmWebcam();
            frmWebcam.Show();
        }
    }
}
