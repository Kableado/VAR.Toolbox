using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using VAR.Toolbox.Code;

namespace VAR.Toolbox.UI
{
    public partial class FrmToolbox : Form
    {
        #region Declarations

        private bool _closing = false;

        private NotifyIcon niTray = null;

        private static FrmToolbox _currentInstance = null;

        #endregion Declarations

        #region Form life cycle

        public FrmToolbox()
        {
            InitializeComponent();

            InitializeCustomControls();

            MouseDown += DragWindow_MouseDown;
            lblToolbox.MouseDown += DragWindow_MouseDown;

            _currentInstance = this;
        }

        private void InitializeCustomControls()
        {
            niTray = new NotifyIcon();
            niTray.Text = "VAR.Toolbox";
            niTray.Visible = true;
            niTray.MouseClick += niTray_MouseClick;
        }

        private void FrmToolbox_Load(object sender, EventArgs e)
        {
            Icon ico = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            Icon = ico;
            niTray.Icon = ico;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown || _closing)
            {
                base.OnFormClosing(e);
                return;
            }

            HideChildWindows();
            Hide();
            e.Cancel = true;
        }

        #endregion Form life cycle

        #region UI events

        private void DragWindow_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                User32.ReleaseCapture();
                User32.SendMessage(Handle, User32.WM_NCLBUTTONDOWN, User32.HT_CAPTION, 0);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure want to exit?", "Exit?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                _closing = true;
                CloseChildWindows();
                Close();
            }
        }

        private void niTray_MouseClick(object sender, MouseEventArgs e)
        {
            if (Visible)
            {
                HideChildWindows();
                Hide();
                return;
            }

            WindowState = FormWindowState.Minimized;
            Show();
            ShowChildWindows();
            WindowState = FormWindowState.Normal;
        }

        private void btnCoder_Click(object sender, EventArgs e)
        {
            CreateWindow(typeof(FrmCoder));
        }

        private void btnProxyCmd_Click(object sender, EventArgs e)
        {
            CreateWindow(typeof(FrmProxyCmd));
        }

        private void btnWebcam_Click(object sender, EventArgs e)
        {
            CreateWindow(typeof(FrmWebcam));
        }

        private void btnTunnelTCP_Click(object sender, EventArgs e)
        {
            CreateWindow(typeof(FrmTunnelTCP));
        }

        private void btnTestSoapService_Click(object sender, EventArgs e)
        {
            CreateWindow(typeof(FrmTestSoapService));
        }

        private void btnTestRestService_Click(object sender, EventArgs e)
        {
            CreateWindow(typeof(FrmTestRestService));
        }

        private void btnScreenshooter_Click(object sender, EventArgs e)
        {
            CreateWindow(typeof(FrmScreenshooter));
        }

        private void btnIPScan_Click(object sender, EventArgs e)
        {
            CreateWindow(typeof(FrmIPScan));
        }

        #endregion UI events

        #region Window handling

        private Form CreateWindow(Type type)
        {
            Form frm = Activator.CreateInstance(type) as Form;
            if (frm == null)
            {
                return null;
            }
            _forms.Add(frm);
            frm.FormClosing += frmChild_FormClosing;
            if ((frm is IFormWithIcon) == false)
            {
                frm.Icon = Icon;
            }
            frm.Show();
            return frm;
        }

        private List<Form> _forms = new List<Form>();

        private void frmChild_FormClosing(object sender, FormClosingEventArgs e)
        {
            _forms.Remove((Form)sender);
        }

        private void FrmToolbox_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseChildWindows();
        }

        private bool _wasMinimized = false;

        private void FrmToolbox_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                _wasMinimized = true;
                HideChildWindows();
            }
            if (FormWindowState.Normal == WindowState && _wasMinimized)
            {
                _wasMinimized = false;
                ShowChildWindows();
            }
        }

        private void CloseChildWindows()
        {
            while (_forms.Count > 0)
            {
                _forms[0].Close();
            }
        }

        private void ShowChildWindows()
        {
            foreach (Form frm in _forms)
            {
                frm.Show();
            }
        }

        private void HideChildWindows()
        {
            foreach (Form frm in _forms)
            {
                frm.Hide();
            }
        }

        public static void StaticCreateWindow(Type type)
        {
            _currentInstance?.CreateWindow(type);
        }

        public static List<T> StaticGetWindowsOfType<T>()
        {
            List<T> list = new List<T>();
            if (_currentInstance == null) { return list; }
            foreach (Form frm in _currentInstance._forms)
            {
                if (frm is T)
                {
                    list.Add((T)(object)frm);
                }
            }
            return list;
        }

        #endregion Window handling

    }
}
