using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VAR.Toolbox.Code;

namespace VAR.Toolbox
{
    public partial class FrmWebcam : Form
    {
        private Webcam webcam = null;

        public FrmWebcam()
        {
            InitializeComponent();
        }

        private void FrmWebcam_Load(object sender, EventArgs e)
        {
            cboWebcams_LoadData();
        }

        void webcam_NewFrame(object sender, Bitmap frame)
        {
            picWebcam.ImageShow = frame;
        }

        private void FrmWebcam_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(webcam!=null){
                webcam.Stop();
            }
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (webcam == null)
            {
                InitWebcam();
            }
            if (webcam != null)
            {
                if (webcam.Active)
                {
                    webcam.Stop();
                    btnStartStop.Text = "Start";
                    picWebcam.ImageShow = null;
                    webcam = null;
                }
                else
                {
                    webcam.Start();
                    btnStartStop.Text = "Stop";
                }
            }
        }

        private void InitWebcam()
        {
            if (cboWebcams.SelectedIndex < 0) { return; }
            WebcamObject webcamObject = (WebcamObject)cboWebcams.SelectedItem;
            webcam = new Webcam(webcamObject.Moniker);
            webcam.NewFrame += webcam_NewFrame;
        }

        private class WebcamObject
        {
            public string Name;
            public string Moniker;

            public override string ToString()
            {
                return Name;
            }
        };

        private void cboWebcams_LoadData()
        {
            try
            {
                Dictionary<string, string> devices = Webcam.ListDevices();
                foreach (KeyValuePair<string, string> pair in devices)
                {
                    cboWebcams.Items.Add(new WebcamObject { Name = pair.Key, Moniker = pair.Value });
                }
                if (cboWebcams.Items.Count > 0)
                {
                    cboWebcams.SelectedIndex = 0;
                }
            }
            catch (Exception)
            {
                cboWebcams.Items.Clear();
            }
        }
    }
}
