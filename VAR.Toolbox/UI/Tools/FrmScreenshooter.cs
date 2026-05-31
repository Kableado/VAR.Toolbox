using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Threading;
using VAR.Toolbox.Code;
using VAR.Toolbox.Controls;

namespace VAR.Toolbox.UI.Tools;

public class FrmScreenshooter : Window, IToolForm
{
    public string ToolName => "Screenshooter";
    public bool HasIcon => false;

    private bool _repetitiveScreenshots;
    private readonly DispatcherTimer _timTicker;
    private System.Drawing.Bitmap? _bmpScreen;
    private readonly CtrImageViewer _picViewer;
    private readonly Button _btnStartStop;

    public FrmScreenshooter()
    {
        Title = "Screenshooter";
        Width = 600;
        Height = 450;

        _picViewer = new CtrImageViewer();

        Button btnScreenshot = new() { Content = "Screenshot", };
        btnScreenshot.Click += BtnScreenshot_Click;

        _btnStartStop = new Button { Content = "Start", };
        _btnStartStop.Click += BtnStartStop_Click;

        StackPanel buttons = new() { Orientation = Orientation.Horizontal, Spacing = 5, };
        buttons.Children.Add(btnScreenshot);
        buttons.Children.Add(_btnStartStop);

        DockPanel layout = new() { Margin = new Thickness(8), };
        DockPanel.SetDock(buttons, Dock.Top);
        layout.Children.Add(buttons);
        layout.Children.Add(_picViewer);

        Content = layout;

        _timTicker = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16), };
        _timTicker.Tick += TimTicker_Tick;
    }

    private void BtnScreenshot_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _bmpScreen = Screenshoter.CaptureScreen(_bmpScreen, window: this);
        _picViewer.ImageShow = BitmapConverter.ConvertToAvalonia(_bmpScreen);
    }

    private void TimTicker_Tick(object? sender, EventArgs e)
    {
        _timTicker.Stop();
        _bmpScreen = Screenshoter.CaptureScreen(_bmpScreen, window: this);
        _picViewer.ImageShow = BitmapConverter.ConvertToAvalonia(_bmpScreen);
        _timTicker.Start();
    }

    private void BtnStartStop_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        GC.Collect();
        if (_repetitiveScreenshots)
        {
            _repetitiveScreenshots = false;
            _btnStartStop.Content = "Start";
            _timTicker.Stop();
        }
        else
        {
            _repetitiveScreenshots = true;
            _btnStartStop.Content = "Stop";
            _timTicker.Start();
        }
    }
}

internal static class BitmapConverter
{
    public static Avalonia.Media.Imaging.Bitmap? ConvertToAvalonia(System.Drawing.Bitmap? bmp)
    {
        if (bmp == null) return null;
        using MemoryStream stream = new();
        bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        stream.Seek(0, SeekOrigin.Begin);
        return new Avalonia.Media.Imaging.Bitmap(stream);
    }
}