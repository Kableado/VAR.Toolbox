using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using VAR.Json;
using VAR.Toolbox.Code.Platforms;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace VAR.Toolbox.UI.Tools;

public class PnlActivity : UserControl, IToolPanel
{
    private readonly TextBox _txtCurrentActivity;
    private readonly TextBlock _lblActiveWindowTitle;
    private readonly TextBlock _lblActive;
    private readonly DispatcherTimer _timTicker;

    public PnlActivity()
    {
        _txtCurrentActivity = new TextBox { AcceptsReturn = true, Height = 58, };
        _lblActiveWindowTitle = new TextBlock { Text = "ActiveWindowTitle", };
        _lblActive = new TextBlock { Text = "Active", };

        HeaderedContentControl grp = new() { Header = "Activity", };
        StackPanel stack = new() { Spacing = 4, };
        stack.Children.Add(_txtCurrentActivity);
        stack.Children.Add(_lblActiveWindowTitle);
        stack.Children.Add(_lblActive);
        grp.Content = stack;

        Content = new Border { Padding = new Thickness(4), Child = grp, Width = 200, };

        _timTicker = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1), };
        _timTicker.Tick += TimTicker_Tick;
        _timTicker.Start();
    }

    private void TimTicker_Tick(object? sender, EventArgs e)
    {
        _timTicker.Stop();

        string activeWindowTitle = Platform.Current.System_GetActiveWindowTitle();
        bool active = Platform.Current.System_GetLastInputTime() < 2;
        DateTime date = DateTime.UtcNow;

        _lblActiveWindowTitle.Text = activeWindowTitle;
        _lblActive.Text = active ? "Active" : "Inactive";

        Activity_Register(activeWindowTitle, active, date);

        _timTicker.Start();
    }

    private class ActivityPoint
    {
        public string ActiveWindowTitle { get; set; } = string.Empty;
        public bool Active { get; set; }
        public DateTime Date { get; set; }
    }

    private DateTime _currentDate = DateTime.MinValue;
    private readonly List<ActivityPoint> _currentActivityPoints = [];
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
        _currentActivityPoints.Add(new ActivityPoint { ActiveWindowTitle = activeWindowTitle, Active = active, Date = date, });
    }

    private class ActivityFrame
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CurrentActivity { get; set; } = string.Empty;
        public List<string> ActiveWindowTitles { get; set; } = [];
        public float ActivityFactor { get; set; }
    }

    private void Activity_EndFrame()
    {
        if (_currentActivityPoints.Count == 0) return;

        ActivityFrame frame = new()
        {
            StartDate = _currentActivityPoints.Min(ap => ap.Date),
            EndDate = _currentActivityPoints.Max(ap => ap.Date),
            CurrentActivity = _txtCurrentActivity.Text ?? string.Empty,
            ActiveWindowTitles = _currentActivityPoints.Select(ap => ap.ActiveWindowTitle).Distinct().ToList(),
            ActivityFactor = _currentActivityPoints.Count(ap => ap.Active) / (float)_currentActivityPoints.Count,
        };

        JsonWriter jsonWriter = new();
        string line = jsonWriter.Write(frame);
        try
        {
            StreamWriter? outStream = GetOutputStreamWriter();
            outStream?.WriteLine(line);
            CloseOutputStreamWriter(outStream);
        }
        catch (Exception) { /* Ignore */ }
    }

    private static StreamWriter? GetOutputStreamWriter()
    {
        try
        {
            string location = System.Reflection.Assembly.GetEntryAssembly()?.Location ??
                              System.Reflection.Assembly.GetExecutingAssembly().Location;
            string? assemblyPath = Path.GetDirectoryName(location);
            string path = Path.Combine(assemblyPath ?? string.Empty, "Activity");
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            string fileOut = $"{path}/Activity.{DateTime.UtcNow:yyyy-MM-dd}.txt";
            return File.AppendText(fileOut);
        }
        catch (Exception) { return null; }
    }

    private static void CloseOutputStreamWriter(StreamWriter? stream)
    {
        stream?.Close();
    }
}