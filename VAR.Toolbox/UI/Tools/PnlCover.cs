using System;
using System.Windows.Forms;
using VAR.Toolbox.Code.Windows;

namespace VAR.Toolbox.UI
{
    public partial class PnlCover : UserControl, IToolPanel
    {
        public PnlCover()
        {
            InitializeComponent();
        }

        private void TimTicker_Tick(object sender, EventArgs e)
        {
            if (DesignMode) { return; }
            timTicker.Stop();
            bool userInactive = false;
            uint inactiveTime = Win32.GetLastInputTime();

            lblInactive.Text = string.Format("Inactive by {0} seconds", inactiveTime);

            if (chkAutoCover.Checked)
            {
                if (!userInactive && inactiveTime > numInactive.Value)
                {
                    userInactive = true;
                    CoverScreen();
                }
                if (inactiveTime < 1)
                {
                    userInactive = false;
                }
            }
            timTicker.Start();
        }

        private void BtnCover_Click(object sender, EventArgs e)
        {
            CoverScreen();
        }

        private FrmCover _frmCover = null;

        private void CoverScreen()
        {
            if (DesignMode) { return; }
            if (_frmCover != null)
            {
                _frmCover.Show();
                return;
            }
            _frmCover = new FrmCover();
            _frmCover.FormClosing += (sender, e) => { _frmCover = null; };
            _frmCover.Show();

            if (Parent is Form frmParent)
            {
                frmParent.WindowState = FormWindowState.Minimized;
            }
        }

    }
}
