using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VAR.ScreenAutomation.Code;
using VAR.ScreenAutomation.Interfaces;
using VAR.Toolbox.Code;
using VAR.Toolbox.Code.Bots;
using Mouse = VAR.ScreenAutomation.Code.Mouse;
using Screenshoter = VAR.ScreenAutomation.Code.Screenshoter;

// ReSharper disable LocalizableElement

namespace VAR.ScreenAutomation
{
    public partial class FrmScreenAutomation : Form, Toolbox.UI.IToolForm
    {
        public string ToolName => "ScreenAutomation";

        public bool HasIcon => false;

        private bool _running;
        private IAutomationBot _automationBot;

        private Timer _timTicker;
        private Bitmap _bmpScreen;

        public FrmScreenAutomation()
        {
            PreInitializeComponent();
            InitializeComponent();
        }

        private void PreInitializeComponent()
        {
            AutoScaleMode = AutoScaleMode.None;
            Font = new Font(Font.Name, 8.25f * 96f / CreateGraphics().DpiX, Font.Style, Font.Unit, Font.GdiCharSet,
                Font.GdiVerticalFont);
        }

        private void FrmScreenAutomation_Load(object sender, EventArgs e)
        {
            var configuration = new FileBackedConfiguration();
            configuration.Load();
            Top = configuration.Get("Top", Top);
            Left = configuration.Get("Left", Left);
            Width = configuration.Get("Width", Width);
            Height = configuration.Get("Height", Height);
            splitMain.SplitterDistance = configuration.Get("splitMain.SplitterDistance", splitMain.SplitterDistance);
            splitOutput.SplitterDistance =
                configuration.Get("splitOutput.SplitterDistance", splitOutput.SplitterDistance);

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            TransparencyKey = Color.LimeGreen;
            picCapturer.BackColor = Color.LimeGreen;

            ddlAutomationBot.Items.AddRange(AutomationBotFactory.GetNames().ToArray<object>());
            ddlAutomationBot.SelectedItem =
                configuration.Get("ddlAutomationBot", (string)ddlAutomationBot.SelectedItem);
            if (ddlAutomationBot.SelectedIndex < 0)
            {
                ddlAutomationBot.SelectedIndex = 0;
            }

            numFPS.Value = configuration.Get("numFPS", (int)numFPS.Value);

            chkKeepToplevel.Checked = configuration.Get("chkKeepToplevel", chkKeepToplevel.Checked);
            chkClick.Checked = configuration.Get("chkClick", chkClick.Checked);

            if (components == null) { components = new Container(); }

            _timTicker = new Timer(components)
            {
                Interval = Convert.ToInt32(1000 / numFPS.Value),
            };
            _timTicker.Tick += TimTicker_Tick;
            _timTicker.Enabled = true;
            _timTicker.Start();
        }

        private void FrmScreenAutomation_FormClosing(object sender, FormClosingEventArgs e)
        {
            var configuration = new FileBackedConfiguration();
            configuration.Set("Top", Top);
            configuration.Set("Left", Left);
            configuration.Set("Width", Width);
            configuration.Set("Height", Height);
            configuration.Set("splitMain.SplitterDistance", splitMain.SplitterDistance);
            configuration.Set("splitOutput.SplitterDistance", splitOutput.SplitterDistance);
            configuration.Set("ddlAutomationBot", (string)ddlAutomationBot.SelectedItem);
            configuration.Set("numFPS", (int)numFPS.Value);
            configuration.Set("chkKeepToplevel", chkKeepToplevel.Checked);
            configuration.Set("chkClick", chkClick.Checked);
            configuration.Save();
        }

        private void TimTicker_Tick(object sender, EventArgs e)
        {
            _timTicker.Enabled = false;
            _timTicker.Stop();

            _bmpScreen = Screenshoter.CaptureControl(picCapturer, _bmpScreen);

            if (_automationBot != null)
            {
                _bmpScreen = _automationBot.Process(_bmpScreen, ctrOutput);
                if (_running)
                {
                    string responseKeys = _automationBot.ResponseKeys();
                    if (string.IsNullOrEmpty(responseKeys) == false && WindowHandling.ApplicationIsActivated() == false)
                    {
                        SendKeys.Send(responseKeys);
                    }
                }
            }

            picPreview.ImageShow = _bmpScreen;

            _timTicker.Interval = Convert.ToInt32(1000 / numFPS.Value);
            _timTicker.Enabled = true;
            _timTicker.Start();
        }

        private void BtnStartEnd_Click(object sender, EventArgs e)
        {
            if (_running)
            {
                End();
            }
            else
            {
                Start();
            }
        }

        private void DdlAutomationBot_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitBot((string)ddlAutomationBot.SelectedItem);
        }

        private void BtnAutomationBotConfig_Click(object sender, EventArgs e)
        {
            if (_automationBot == null) { return; }

            IConfiguration defaultConfig = _automationBot.GetDefaultConfiguration();
            var config = new FileBackedConfiguration(_automationBot.Name);
            config.Load(defaultConfig);
            var frmAutomationBotParameters = new FrmAutomationBotParams(config)
            {
                StartPosition = FormStartPosition.CenterParent
            };
            if (_isToplevel)
            {
                WindowHandling.WindowSetTopLevel(this, false);
            }

            frmAutomationBotParameters.ShowDialog();
            InitBot(_automationBot.Name);
            if (_isToplevel)
            {
                WindowHandling.WindowSetTopLevel(this);
            }
        }

        private void Start()
        {
            if (_running) { return; }

            _running = true;
            btnStartEnd.Text = "End";
            InitBot((string)ddlAutomationBot.SelectedItem);
            if (!chkClick.Checked) { return; }

            Point pointCapturerCenter =
                picCapturer.PointToScreen(new Point(picCapturer.Width / 2, picCapturer.Height / 2));
            Mouse.SetPosition((uint)pointCapturerCenter.X, (uint)pointCapturerCenter.Y);
            Mouse.Click(Mouse.MouseButtons.Left);
        }

        private void End()
        {
            if (_running == false) { return; }

            _running = false;
            btnStartEnd.Text = "Start";
        }

        private void InitBot(string botName)
        {
            _automationBot = AutomationBotFactory.CreateFromName(botName);
            var botConfiguration = new FileBackedConfiguration(botName);
            botConfiguration.Load();
            _automationBot?.Init(ctrOutput, botConfiguration);
        }

        private void chkKeepToplevel_CheckedChanged(object sender, EventArgs e)
        {
            WindowSetTopLevel(chkKeepToplevel.Checked);
        }

        private bool _isToplevel;

        private void WindowSetTopLevel(bool toplevel)
        {
            if (toplevel)
            {
                if (_isToplevel) { return; }

                WindowHandling.WindowSetTopLevel(this);
                _isToplevel = true;
            }
            else
            {
                if (_isToplevel == false) { return; }

                WindowHandling.WindowSetTopLevel(this, false);
                _isToplevel = false;
            }
        }
    }
}