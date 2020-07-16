using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VAR.Toolbox.Code;
using VAR.Toolbox.Controls;

namespace VAR.Toolbox.UI
{
    public partial class FrmScreenshooter : Frame, IToolForm
    {
        public string ToolName { get { return "Screenshooter"; } }

        public bool HasIcon { get { return false; } }

        private bool _repetitiveScreenshots = false;
        private readonly Timer timTicker;
        private Bitmap bmpScreen = null;

        public FrmScreenshooter()
        {
            InitializeComponent();

            if (components == null) { components = new Container(); }
            timTicker = new Timer(components)
            {
                Interval = 16,
                Enabled = false
            };
            timTicker.Tick += TimTicker_Tick;
        }

        private void BtnScreenshoot_Click(object sender, EventArgs e)
        {
            bmpScreen = Screenshooter.CaptureScreen(bmpScreen);
            picViewer.ImageShow = bmpScreen;
        }

        private void TimTicker_Tick(object sender, EventArgs e)
        {
            timTicker.Stop();
            bmpScreen = Screenshooter.CaptureScreen(bmpScreen);
            picViewer.ImageShow = bmpScreen;
            timTicker.Start();
        }

        private void BtnStartStop_Click(object sender, EventArgs e)
        {
            GC.Collect();
            if (_repetitiveScreenshots)
            {
                _repetitiveScreenshots = false;
                btnStartStop.Text = "Start";
                timTicker.Stop();
                timTicker.Enabled = false;
            }
            else
            {
                _repetitiveScreenshots = true;
                btnStartStop.Text = "Stop";
                timTicker.Enabled = true;
                timTicker.Start();
            }
        }
    }
}
