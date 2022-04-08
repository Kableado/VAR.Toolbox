using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VAR.Toolbox.Code;
using VAR.Toolbox.Controls;

namespace VAR.Toolbox.UI.Tools
{
    public partial class FrmScreenshooter : Frame, IToolForm
    {
        public string ToolName => "Screenshooter";

        public bool HasIcon => false;

        private bool _repetitiveScreenshots;
        private readonly Timer _timTicker;
        private Bitmap _bmpScreen;

        public FrmScreenshooter()
        {
            InitializeComponent();

            if (components == null) { components = new Container(); }

            _timTicker = new Timer(components)
            {
                Interval = 16,
                Enabled = false
            };
            _timTicker.Tick += TimTicker_Tick;
        }

        private void BtnScreenshot_Click(object sender, EventArgs e)
        {
            _bmpScreen = Screenshoter.CaptureScreen(_bmpScreen);
            picViewer.ImageShow = _bmpScreen;
        }

        private void TimTicker_Tick(object sender, EventArgs e)
        {
            _timTicker.Stop();
            _bmpScreen = Screenshoter.CaptureScreen(_bmpScreen);
            picViewer.ImageShow = _bmpScreen;
            _timTicker.Start();
        }

        private void BtnStartStop_Click(object sender, EventArgs e)
        {
            GC.Collect();
            if (_repetitiveScreenshots)
            {
                _repetitiveScreenshots = false;
                btnStartStop.Text = "Start";
                _timTicker.Stop();
                _timTicker.Enabled = false;
            }
            else
            {
                _repetitiveScreenshots = true;
                btnStartStop.Text = "Stop";
                _timTicker.Enabled = true;
                _timTicker.Start();
            }
        }
    }
}