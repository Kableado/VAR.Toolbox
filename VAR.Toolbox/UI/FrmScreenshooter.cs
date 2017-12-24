using System;
using System.Windows.Forms;
using VAR.Toolbox.Code;

namespace VAR.Toolbox.UI
{
    public partial class FrmScreenshooter : Form
    {
        public FrmScreenshooter()
        {
            InitializeComponent();
        }

        private void btnScreenshoot_Click(object sender, EventArgs e)
        {
            picViewer.ImageShow = Screenshooter.CaptureScreen();
        }
    }
}
