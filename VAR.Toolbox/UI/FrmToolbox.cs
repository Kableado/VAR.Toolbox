#pragma warning disable IDE0019

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VAR.Toolbox.Code;
using VAR.Toolbox.Code.Windows;
using VAR.Toolbox.Controls;

namespace VAR.Toolbox.UI
{
    public class FrmToolbox : Frame
    {
        #region Declarations

        private bool _closing;

        private Label _lblToolbox;
        private CButton _btnExit;

        private NotifyIcon _niTray;

        private static FrmToolbox _currentInstance;

        #endregion Declarations

        #region Form life cycle

        public FrmToolbox()
        {
            InitializeDynamicComponents();
            _currentInstance = this;
        }

        private void FrmToolbox_Load(object sender, EventArgs e)
        {
            Icon ico = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            Icon = ico;
            _niTray.Icon = ico;
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

        private void DragWindow_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                User32.ReleaseCapture();
                User32.SendMessage(Handle, User32.WM_NCLBUTTONDOWN, User32.HT_CAPTION, 0);
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure want to exit?", "Exit?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                _closing = true;
                _niTray.Visible = false;
                CloseChildWindows();
                Close();
            }
        }

        private void NiTray_MouseClick(object sender, MouseEventArgs e)
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

        #endregion UI events

        #region Dynamic layout

        private void InitializeDynamicComponents()
        {
            SuspendLayout();
            const int ToolSpacing = 5;
            const int ToolWidth = 200;
            const int WindowSpacing = 10;

            // Get list of ToolForms
            Type iToolForm = typeof(IToolForm);
            IEnumerable<Type> toolFormTypes = ReflectionUtils.GetTypesOfInterface(iToolForm);
            Dictionary<string, Type> dictToolFormTypes = toolFormTypes.ToDictionary(t =>
            {
                IToolForm toolForm =
                    System.Runtime.Serialization.FormatterServices.GetUninitializedObject(t) as IToolForm;
                return toolForm?.ToolName ?? t.Name;
            });

            // Get list of ToolPanels
            Type iToolPanel = typeof(IToolPanel);
            IEnumerable<Type> toolPanelTypes = ReflectionUtils.GetTypesOfInterface(iToolPanel).OrderBy(t => t.Name);

            // lblToolbox
            _lblToolbox = new Label
            {
                Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right),
                Font = new Font(Font.FontFamily, ControlsUtils.GetFontSize(this, 27.75F), FontStyle.Bold,
                    GraphicsUnit.Point, 0),
                Location = new Point(WindowSpacing, WindowSpacing),
                Margin = new Padding(0, 0, 0, 0),
                Name = "lblToolbox",
                Size = new Size(ToolWidth * 2 + ToolSpacing, 50),
                TabIndex = 6,
                Text = "Toolbox",
                TextAlign = ContentAlignment.MiddleCenter
            };
            _lblToolbox.MouseDown += DragWindow_MouseDown;
            int nextYLocation = _lblToolbox.Location.Y + _lblToolbox.Size.Height + WindowSpacing;

            // Tool buttons
            int idxButton = 0;
            int xStartButtons = WindowSpacing;
            int xStepButtons = ToolWidth + ToolSpacing;
            int yStartButtons = nextYLocation;
            int yStepButtons = 40 + ToolSpacing;
            IEnumerable<KeyValuePair<string, Type>> sortedToolForms = dictToolFormTypes.OrderBy(p => p.Key);
            foreach (KeyValuePair<string, Type> p in sortedToolForms)
            {
                int x = xStartButtons + (idxButton % 2) * xStepButtons;
                int y = yStartButtons + (idxButton / 2) * yStepButtons;
                CButton btn = new CButton
                {
                    Location = new Point(x, y),
                    Name = $"btn{p.Key}",
                    Size = new Size(ToolWidth, 40),
                    TabIndex = idxButton,
                    Text = p.Key,
                };
                btn.Click += (s, e) => { CreateWindow(p.Value); };
                Controls.Add(btn);

                nextYLocation = btn.Location.Y + btn.Size.Height + WindowSpacing;

                idxButton++;
            }

            // Tool panels
            int idxPanel = 0;
            int yStartPanels = nextYLocation;
            int xStartPanels = WindowSpacing;
            int xNextPanels = xStartPanels;
            foreach (Type t in toolPanelTypes)
            {
                ContainerControl pnl = Activator.CreateInstance(t) as ContainerControl;
                if (pnl == null) { continue; }

                pnl.Location = new Point(xNextPanels, yStartPanels);
                Controls.Add(pnl);

                int tempNextYLocation = pnl.Location.Y + pnl.Size.Height + WindowSpacing;
                if (nextYLocation < tempNextYLocation)
                {
                    nextYLocation = tempNextYLocation;
                }

                xNextPanels = pnl.Location.X + pnl.Size.Width + ToolSpacing;

                if ((idxPanel % 2) == 1)
                {
                    yStartPanels = nextYLocation;
                    xNextPanels = xStartPanels;
                }

                idxPanel++;
            }

            // btnExit
            _btnExit = new CButton
            {
                Anchor = ((AnchorStyles.Bottom | AnchorStyles.Left)
                          | AnchorStyles.Right),
                Location = new Point(WindowSpacing, nextYLocation),
                Name = "btnExit",
                Size = new Size(ToolWidth * 2 + ToolSpacing, 40),
                TabIndex = 7,
                Text = "Exit",
            };
            _btnExit.Click += BtnExit_Click;
            nextYLocation = _btnExit.Location.Y + _btnExit.Size.Height + WindowSpacing;

            // FrmToolbox
            ClientSize = new Size(425, nextYLocation);
            Controls.Add(_btnExit);
            Controls.Add(_lblToolbox);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            Name = "FrmToolbox";
            Text = "Toolbox";
            FormClosing += FrmToolbox_FormClosing;
            Load += FrmToolbox_Load;
            Resize += FrmToolbox_Resize;
            MouseDown += DragWindow_MouseDown;
            ResumeLayout(false);

            // niTray
            _niTray = new NotifyIcon
            {
                Text = "VAR.Toolbox",
                Visible = true
            };
            _niTray.MouseClick += NiTray_MouseClick;


            ResumeLayout();
        }

        #endregion Dynamic layout

        #region Window handling

        private void CreateWindow(Type type)
        {
            var frm = Activator.CreateInstance(type) as Form;
            if (frm == null)
            {
                return;
            }

            _forms.Add(frm);
            frm.FormClosing += FrmChild_FormClosing;
            if ((frm as IToolForm)?.HasIcon == false)
            {
                frm.Icon = Icon;
            }

            frm.Show();
        }

        private readonly List<Form> _forms = new List<Form>();

        private void FrmChild_FormClosing(object sender, FormClosingEventArgs e)
        {
            _forms.Remove((Form)sender);
        }

        private void FrmToolbox_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseChildWindows();
        }

        private bool _wasMinimized;

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