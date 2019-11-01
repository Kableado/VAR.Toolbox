using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VAR.ScreenAutomation.Code;
using VAR.ScreenAutomation.Interfaces;

namespace VAR.ScreenAutomation
{
    public partial class FrmScreenAutomation : Form
    {
        private bool _running = false;
        private IAutomationBot _automationBot = null;

        private Timer timTicker;
        private Bitmap bmpScreen = null;

        public FrmScreenAutomation()
        {
            AutoScaleMode = AutoScaleMode.None;
            AutoScaleDimensions = new SizeF(1, 1);
            InitializeComponent();
        }

        private void FrmScreenAutomation_Load(object sender, EventArgs e)
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            TransparencyKey = Color.LimeGreen;
            picCapturer.BackColor = Color.LimeGreen;


            ddlAutomationBot.Items.AddRange(AutomationBotFactory.GetAllAutomationBots());
            ddlAutomationBot.SelectedIndex = 0;
            _automationBot = AutomationBotFactory.CreateFromName((string)ddlAutomationBot.SelectedItem);
            _automationBot?.Init(ctrOutput);


            if (components == null) { components = new Container(); }
            timTicker = new Timer(components)
            {
                Interval = 16,
            };
            timTicker.Tick += TimTicker_Tick;
            timTicker.Enabled = true;
            timTicker.Start();

            WindowHandling.WindowSetTopLevel(this);
        }

        private void TimTicker_Tick(object sender, EventArgs e)
        {
            timTicker.Stop();

            bmpScreen = Screenshoter.CaptureControl(picCapturer, bmpScreen);

            if (_automationBot != null)
            {
                bmpScreen = _automationBot.Process(bmpScreen, ctrOutput);
                if (_running)
                {
                    string responseKeys = _automationBot.ResponseKeys();
                    if (string.IsNullOrEmpty(responseKeys) == false && WindowHandling.ApplicationIsActivated() == false)
                    {
                        SendKeys.Send(responseKeys);
                    }
                }
            }
            picPreview.ImageShow = bmpScreen;

            timTicker.Start();
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

        private void Start()
        {
            if (_running) { return; }
            _running = true;
            btnStartEnd.Text = "End";
            _automationBot = AutomationBotFactory.CreateFromName((string)ddlAutomationBot.SelectedItem);
            _automationBot?.Init(ctrOutput);
            Point pointCapturerCenter = picCapturer.PointToScreen(new Point(picCapturer.Width / 2, picCapturer.Height / 2));
            Mouse.SetPosition((uint)pointCapturerCenter.X, (uint)pointCapturerCenter.Y);
            Mouse.Click(Mouse.MouseButtons.Left);
        }

        private void End()
        {
            if (_running == false) { return; }
            _running = false;
            btnStartEnd.Text = "Start";
        }

        private void DdlAutomationBot_SelectedIndexChanged(object sender, EventArgs e)
        {
            _automationBot = AutomationBotFactory.CreateFromName((string)ddlAutomationBot.SelectedItem);
            _automationBot?.Init(ctrOutput);
        }
    }
}
