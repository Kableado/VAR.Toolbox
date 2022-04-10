using System;
using System.Windows.Forms;
using VAR.Toolbox.Code;
using VAR.Toolbox.Controls;

namespace VAR.Toolbox.UI.Tools
{
    public partial class FrmVirtualMouse : Frame, IToolForm
    {
        public string ToolName => "VirtualMouse";
        
        public bool HasIcon => false;

        private readonly GlobalKeyboardHook _globalKeyboard = new GlobalKeyboardHook();
        
        public FrmVirtualMouse()
        {
            InitializeComponent();
            PostInitializeComponent();
        }

        private void PostInitializeComponent()
        {
            _globalKeyboard.KeyDown += GlobalKeyboard_OnKeyDown;
        }

        private void GlobalKeyboard_OnKeyDown(object sender, KeyEventArgs keyEvent)
        {
            string key = keyEvent.KeyCode.ToString();
            lsbInputs.Items.Add(key);

            if (key == "F1")
            {
                Mouse.SetButton(Mouse.MouseButtons.Left, true);
            }
            if (key == "F2")
            {
                Mouse.Move(0, -10000);
            }
            if (key == "F3")
            {
                Mouse.SetButton(Mouse.MouseButtons.Left, false);
            }
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (_globalKeyboard.IsCapturing())
            {
                _globalKeyboard.Stop();
                btnStartStop.Text = "Start";
                return;
            }

            _globalKeyboard.Start(true);
            btnStartStop.Text = "Stop";
        }
    }
}