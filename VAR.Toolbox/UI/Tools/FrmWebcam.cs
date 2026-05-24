using System;
using System.Collections.Generic;
using System.Drawing;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using VAR.Toolbox.Code;
using VAR.Toolbox.Controls;

namespace VAR.Toolbox.UI.Tools
{
    public class FrmWebcam : Window, IToolForm
    {
        public string ToolName => "Webcam";
        public bool HasIcon => false;

        private Webcam? _webcam;
        private readonly CtrImageViewer _picWebcam;
        private readonly ComboBox _cboWebcams;
        private readonly Button _btnStartStop;

        public FrmWebcam()
        {
            Title = "Webcam";
            Width = 600;
            Height = 450;

            _picWebcam = new CtrImageViewer();
            _cboWebcams = new ComboBox { Width = 300, };

            _btnStartStop = new Button { Content = "Start", };
            _btnStartStop.Click += BtnStartStop_Click;

            StackPanel topRow = new() { Orientation = Orientation.Horizontal, Spacing = 5, };
            topRow.Children.Add(_cboWebcams);
            topRow.Children.Add(_btnStartStop);

            DockPanel layout = new() { Margin = new Thickness(8), };
            DockPanel.SetDock(topRow, Dock.Top);
            layout.Children.Add(topRow);
            layout.Children.Add(_picWebcam);

            Content = layout;

            Opened += (_, _) => CboWebcams_LoadData();
            Closed += (_, _) => _webcam?.Stop();
        }

        private void Webcam_NewFrame(object? sender, Bitmap frame)
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                _picWebcam.ImageShow = BitmapConverter.ConvertToAvalonia(frame);
            });
        }

        private void BtnStartStop_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (_webcam == null) { InitWebcam(); }

            if (_webcam != null)
            {
                if (_webcam.Active)
                {
                    _webcam.Stop();
                    _btnStartStop.Content = "Start";
                    _picWebcam.ImageShow = null;
                    _webcam = null;
                }
                else
                {
                    _webcam.Start();
                    _btnStartStop.Content = "Stop";
                }
            }
        }

        private void InitWebcam()
        {
            if (_cboWebcams.SelectedIndex < 0) return;
            WebcamObject? webcamObject = _cboWebcams.SelectedItem as WebcamObject;
            if (webcamObject == null || string.IsNullOrEmpty(webcamObject.Moniker)) return;
            _webcam = new Webcam(webcamObject.Moniker);
            _webcam.NewFrame += Webcam_NewFrame;
        }

        private class WebcamObject
        {
            public string? Name;
            public string? Moniker;
            public override string ToString()
            {
                return Name ?? string.Empty;
            }
        }

        private void CboWebcams_LoadData()
        {
            try
            {
                Dictionary<string, string> devices = Webcam.ListDevices();
                List<WebcamObject> items = new();
                foreach (KeyValuePair<string, string> pair in devices)
                {
                    items.Add(new WebcamObject { Name = pair.Key, Moniker = pair.Value, });
                }
                _cboWebcams.ItemsSource = items;
                if (items.Count > 0) _cboWebcams.SelectedIndex = 0;
            }
            catch (Exception)
            {
                _cboWebcams.ItemsSource = null;
            }
        }
    }
}