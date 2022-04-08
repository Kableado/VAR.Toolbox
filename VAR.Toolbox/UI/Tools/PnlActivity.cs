using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VAR.Json;
using VAR.Toolbox.Code.Windows;
using VAR.Toolbox.Controls;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace VAR.Toolbox.UI.Tools
{
    public partial class PnlActivity : SubFrame, IToolPanel
    {
        public PnlActivity()
        {
            InitializeComponent();
        }

        private void TimTicker_Tick(object sender, EventArgs e)
        {
            if (DesignMode) { return; }

            timTicker.Stop();

            string activeWindowTitle = User32.GetActiveWindowTitle();
            bool active = Win32.GetLastInputTime() < 2;
            DateTime date = DateTime.UtcNow;

            lblActiveWindowTitle.Text = activeWindowTitle;
            lblActive.Text = active ? "Active" : "Inactive";

            Activity_Register(activeWindowTitle, active, date);

            timTicker.Start();
        }

        private class ActivityPoint
        {
            public string ActiveWindowTitle { get; set; }
            public bool Active { get; set; }
            public DateTime Date { get; set; }
        }

        private DateTime _currentDate = DateTime.MinValue;
        private readonly List<ActivityPoint> _currentActivityPoints = new List<ActivityPoint>();

        private const int SecondsPerFrame = 30;

        private void Activity_Register(string activeWindowTitle, bool active, DateTime date)
        {
            TimeSpan diffTime = date - _currentDate;
            if (diffTime.TotalSeconds > SecondsPerFrame)
            {
                Activity_EndFrame();
                _currentActivityPoints.Clear();
                _currentDate = date;
            }

            _currentActivityPoints.Add(new ActivityPoint
            {
                ActiveWindowTitle = activeWindowTitle,
                Active = active,
                Date = date,
            });
        }


        private class ActivityFrame
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string CurrentActivity { get; set; }
            public List<string> ActiveWindowTitles { get; set; } = new List<string>();
            public float ActivityFactor { get; set; }
        }

        private void Activity_EndFrame()
        {
            if (_currentActivityPoints.Count == 0)
            {
                return;
            }

            // Prepare frame
            ActivityFrame frame = new ActivityFrame
            {
                StartDate = _currentActivityPoints.Min(ap => ap.Date),
                EndDate = _currentActivityPoints.Max(ap => ap.Date),
                CurrentActivity = txtCurrentActivity.Text,
                ActiveWindowTitles = _currentActivityPoints.Select(ap => ap.ActiveWindowTitle).Distinct().ToList(),
                ActivityFactor = _currentActivityPoints.Count(ap => ap.Active) / (float)_currentActivityPoints.Count,
            };

            // Write frame
            JsonWriter jsonWriter = new JsonWriter();
            string line = jsonWriter.Write(frame);
            try
            {
                StreamWriter outStream = GetOutputStreamWriter();
                outStream.WriteLine(line);
                CloseOutputStreamWriter(outStream);
            }
            catch (Exception)
            {
                /* Nom Nom Nom */
            }
        }

        private static StreamWriter GetOutputStreamWriter()
        {
            try
            {
                string location = System.Reflection.Assembly.GetEntryAssembly()?.Location;
                string path = Path.GetDirectoryName(location);

                string fileOut = $"{path}/Activity.{DateTime.UtcNow:yyyy-MM-dd}.txt";
                return File.AppendText(fileOut);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static void CloseOutputStreamWriter(StreamWriter stream)
        {
            if (stream != null)
            {
                stream.Close();
            }
        }
    }
}