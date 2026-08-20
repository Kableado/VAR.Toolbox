using System;
using SkiaSharp;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Platform;
using Avalonia.Threading;
using VAR.Toolbox.Code.Platforms;
using VAR.Toolbox.Controls;

namespace VAR.Toolbox.UI.Tools;

public class FrmScreenshooter : Window, IToolForm
{
    public string ToolName => "Screenshooter";
    public bool HasIcon => false;

    private bool _repetitiveScreenshots;
    private readonly DispatcherTimer _timTicker;
    private SKBitmap? _bmpScreen;
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
        _bmpScreen = CaptureScreen(_bmpScreen, window: this);
        _picViewer.ImageShow = BitmapConverter.ConvertToAvalonia(_bmpScreen);
    }

    private void TimTicker_Tick(object? sender, EventArgs e)
    {
        _timTicker.Stop();
        _bmpScreen = CaptureScreen(_bmpScreen, window: this);
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
    
    
    public static SKBitmap? CaptureScreen(SKBitmap? bmp = null, int? left = null, int? top = null, int? width = null,
        int? height = null, Window? window = null)
    {
        if (window == null) { return bmp; }

        if (width <= 0 || height <= 0) { return bmp; }
            
        // Calculare virtual rect
        int minLeft = int.MaxValue, minTop = int.MaxValue, maxRight = int.MinValue, maxBottom = int.MinValue;
        if(left == null || top == null || width == null || height == null) 
        {
            foreach (Screen screen in
                     window.Screens.All) // o screensService.Screens / screensService.Monitors según versión
            {
                minLeft = Math.Min(minLeft, screen.Bounds.X);
                minTop = Math.Min(minTop, screen.Bounds.Y);
                maxRight = Math.Max(maxRight, screen.Bounds.X + screen.Bounds.Width);
                maxBottom = Math.Max(maxBottom, screen.Bounds.Y + screen.Bounds.Height);
            }
        }
            
        // Determine the size of the "virtual screen", which includes all monitors.
        left ??= minLeft;
        top ??= minTop;
        width ??= (maxRight - minLeft);
        height ??= (maxBottom - minTop);

        return Platform.Current.Screen_CaptureRegion(bmp, left.Value, top.Value, width.Value, height.Value);
    }

    
}

internal static class BitmapConverter
{
    public static Avalonia.Media.Imaging.Bitmap? ConvertToAvalonia(SKBitmap? bmp)
    {
        if (bmp == null) return null;
        using SKImage image = SKImage.FromBitmap(bmp);
        using SKData data = image.Encode(SKEncodedImageFormat.Png, 100);
        using Stream stream = data.AsStream();
        return new Avalonia.Media.Imaging.Bitmap(stream);
    }
}