using System;
using System.Windows.Forms;
using VAR.Toolbox.Code;
using VAR.Toolbox.Code.Windows;
using VAR.Toolbox.Controls;

namespace VAR.Toolbox.UI
{
    public partial class PnlCover : SubFrame, IToolPanel
    {
        public const string PreCoverEventName = "PreCover";
        public const string PostCoverEventName = "PostCover";

        public PnlCover()
        {
            InitializeComponent();
        }

        private void TimTicker_Tick(object sender, EventArgs e)
        {
            if (DesignMode) { return; }
            timTicker.Stop();
            uint inactiveTime = Win32.GetLastInputTime();

            lblInactive.Text = string.Format("Inactive by {0} seconds", inactiveTime);

            if (chkAutoCover.Checked)
            {
                if (inactiveTime > numInactive.Value)
                {
                    CoverScreen();
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

            EventDispatcher.EmitEvent(PreCoverEventName, null);

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
