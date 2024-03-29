﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using VAR.Toolbox.Code;
using VAR.Toolbox.Controls;

namespace VAR.Toolbox.UI.Tools
{
    public partial class FrmWebcam : Frame, IToolForm
    {
        public string ToolName => "Webcam";

        public bool HasIcon => false;

        private Webcam _webcam;

        public FrmWebcam()
        {
            InitializeComponent();
        }

        private void FrmWebcam_Load(object sender, EventArgs e)
        {
            CboWebcams_LoadData();
        }

        private void Webcam_NewFrame(object sender, Bitmap frame)
        {
            picWebcam.ImageShow = frame;
        }

        private void FrmWebcam_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_webcam != null)
            {
                _webcam.Stop();
            }
        }

        private void BtnStartStop_Click(object sender, EventArgs e)
        {
            if (_webcam == null)
            {
                InitWebcam();
            }

            if (_webcam != null)
            {
                if (_webcam.Active)
                {
                    _webcam.Stop();
                    btnStartStop.Text = "Start";
                    picWebcam.ImageShow = null;
                    _webcam = null;
                }
                else
                {
                    _webcam.Start();
                    btnStartStop.Text = "Stop";
                }
            }
        }

        private void InitWebcam()
        {
            if (cboWebcams.SelectedIndex < 0) { return; }

            WebcamObject webcamObject = (WebcamObject)cboWebcams.SelectedItem;
            _webcam = new Webcam(webcamObject.Moniker);
            _webcam.NewFrame += Webcam_NewFrame;
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

        private void CboWebcams_LoadData()
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