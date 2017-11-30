using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VAR.Toolbox
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
    }
}
