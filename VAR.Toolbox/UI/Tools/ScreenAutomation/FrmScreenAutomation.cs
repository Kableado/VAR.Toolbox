using System;
using SkiaSharp;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using VAR.Toolbox.Code;
using VAR.Toolbox.Code.Bots;
using VAR.Toolbox.Code.Configuration;
using VAR.Toolbox.Code.Platforms;
using VAR.Toolbox.Controls;

using Brushes = Avalonia.Media.Brushes;
using Color = Avalonia.Media.Color;
using Point = Avalonia.Point;

namespace VAR.Toolbox.UI.Tools.ScreenAutomation;

public class FrmScreenAutomation : Window, IToolForm
{
    public string ToolName => "ScreenAutomation";
    public bool HasIcon => false;

    private bool _running;
    private IAutomationBot? _automationBot;
    private DispatcherTimer? _timTicker;
    private SKBitmap? _bmpScreen;

    private readonly CtrImageViewer _picPreview;
    private readonly CtrOutput _ctrOutput;
    private readonly ComboBox _ddlAutomationBot;
    private readonly NumericUpDown _numFps;
    private readonly CheckBox _chkKeepToplevel;
    private readonly CheckBox _chkClick;
    private readonly Button _btnStartEnd;

    private readonly Panel _panelCover;
    private readonly Control _ctrHole;

    private readonly CombinedGeometry _geometryMask;
    private readonly RectangleGeometry _geometryWindow;
    private readonly RectangleGeometry _geometryHole;

    public FrmScreenAutomation()
    {
        Title = "ScreenAutomation";
        Width = 700;
        Height = 500;
        TransparencyLevelHint = [WindowTransparencyLevel.Transparent, ];
        Background = Brushes.Transparent;

        _geometryWindow = new RectangleGeometry();
        _geometryHole = new  RectangleGeometry();
        _geometryMask = new CombinedGeometry
        {
            GeometryCombineMode = GeometryCombineMode.Exclude,
            Geometry1 = _geometryWindow,
            Geometry2 = _geometryHole,
        };

        _panelCover = new Panel
        {
            Background = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0)),
            Clip = _geometryMask,
        };

        _ctrHole = new Border
        {
            Background = Brushes.Transparent,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Margin = new Thickness(5),
        };
        Grid.SetColumn(_ctrHole, 2);

        StackPanel toolbar = new() { Orientation = Orientation.Horizontal, Spacing = 4, };
        Grid.SetRow(toolbar, 0);
        _ddlAutomationBot = new ComboBox { Width = 150, };
        _ddlAutomationBot.SelectionChanged += DdlAutomationBot_SelectionChanged;
        toolbar.Children.Add(_ddlAutomationBot);
        Button btnConfig = new() { Content = "Config", };
        btnConfig.Click += BtnAutomationBotConfig_Click;
        toolbar.Children.Add(btnConfig);
            
        StackPanel toolbar2 = new() { Orientation = Orientation.Horizontal, Spacing = 4, };
        Grid.SetRow(toolbar2, 1);
        toolbar2.Children.Add(new TextBlock { Text = "FPS:", VerticalAlignment = VerticalAlignment.Center, });
        _numFps = new NumericUpDown { Minimum = 1, Maximum = 60, Value = 10, Width = 50, FormatString = "0", };
        toolbar2.Children.Add(_numFps);
        _chkKeepToplevel = new CheckBox { Content = "TopLevel", };
        _chkKeepToplevel.IsCheckedChanged += ChkKeepToplevel_CheckedChanged;
        toolbar2.Children.Add(_chkKeepToplevel);
        _chkClick = new CheckBox { Content = "Click", };
        toolbar2.Children.Add(_chkClick);
        _btnStartEnd = new Button { Content = "Start", };
        _btnStartEnd.Click += BtnStartEnd_Click;
        toolbar2.Children.Add(_btnStartEnd);

            
            
        _picPreview = new CtrImageViewer();
        Grid.SetRow(_picPreview, 2);
            
        GridSplitter splitterToolbars = new()
        {
            Height = 4,
            Background = Brushes.Gray,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            ResizeDirection = GridResizeDirection.Rows,
        };
        Grid.SetRow(splitterToolbars, 3);
            
        _ctrOutput = new CtrOutput();
        Grid.SetRow(_ctrOutput, 4);
            
        Grid toolGrid = new();
        Grid.SetColumn(toolGrid, 0);
        toolGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        toolGrid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
        toolGrid.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Star));
        toolGrid.RowDefinitions.Add(new RowDefinition(4, GridUnitType.Pixel));
        toolGrid.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Star));
        toolGrid.Children.Add(toolbar);
        toolGrid.Children.Add(toolbar2);
        toolGrid.Children.Add(_picPreview);
        toolGrid.Children.Add(splitterToolbars);
        toolGrid.Children.Add(_ctrOutput);

        GridSplitter splitterMain = new()
        {
            Width = 4,
            Background = Brushes.Gray,
            VerticalAlignment = VerticalAlignment.Stretch,
        };
        Grid.SetColumn(splitterMain, 1);
            
        Grid mainGrid = new();
        mainGrid.ColumnDefinitions.Add(new ColumnDefinition(1, GridUnitType.Star));
        mainGrid.ColumnDefinitions.Add(new ColumnDefinition(4, GridUnitType.Pixel));
        mainGrid.ColumnDefinitions.Add(new ColumnDefinition(1, GridUnitType.Star));
        mainGrid.Children.Add(_panelCover);
        mainGrid.Children.Add(toolGrid);
        mainGrid.Children.Add(splitterMain);
        mainGrid.Children.Add(_ctrHole);

        Content = mainGrid;

        Opened += FrmScreenAutomation_Opened;
        Closing += FrmScreenAutomation_Closing;

        _ctrHole.PropertyChanged += (_, e) =>
        {
            if (e.Property == BoundsProperty)
            {
                CtrHole_SyncMask();
            }
        };
        PropertyChanged += (_, e) =>
        {
            if (e.Property == BoundsProperty)
            {
                CtrHole_SyncMask();
            }
        };
    }

    private void CtrHole_SyncMask()
    {
        _geometryWindow.Rect = new Rect(0, 0, Bounds.Width, Bounds.Height);

        Point? relativePosition = _ctrHole.TranslatePoint(new Point(0, 0), this);
        if (relativePosition.HasValue)
        {
            _geometryHole.Rect = new Rect(
                x: relativePosition.Value.X,
                y: relativePosition.Value.Y,
                width: _ctrHole.Bounds.Width,
                height: _ctrHole.Bounds.Height);
        }
    }

    private void FrmScreenAutomation_Opened(object? sender, EventArgs e)
    {
        FileBackedConfiguration configuration = new();
        configuration.Load();

        _ddlAutomationBot.ItemsSource = AutomationBotFactory.GetNames().ToList();
        string selectedBot = configuration.Get("ddlAutomationBot", string.Empty);
        if (!string.IsNullOrEmpty(selectedBot))
        {
            _ddlAutomationBot.SelectedItem = selectedBot;
        }
        if (_ddlAutomationBot is { SelectedIndex: < 0, ItemCount: > 0, })
        {
            _ddlAutomationBot.SelectedIndex = 0;
        }

        _numFps.Value = configuration.Get("numFPS", 10);
        _chkKeepToplevel.IsChecked = configuration.Get("chkKeepToplevel", false);
        _chkClick.IsChecked = configuration.Get("chkClick", false);

        _timTicker = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(1000.0 / (double)(_numFps.Value ?? 10)),
        };
        _timTicker.Tick += TimTicker_Tick;
        _timTicker.Start();
    }

    private void FrmScreenAutomation_Closing(object? sender, WindowClosingEventArgs e)
    {
        _timTicker?.Stop();

        FileBackedConfiguration configuration = new();
        configuration.Set("ddlAutomationBot", _ddlAutomationBot.SelectedItem as string ?? string.Empty);
        configuration.Set("numFPS", (int)(_numFps.Value ?? 10));
        configuration.Set("chkKeepToplevel", _chkKeepToplevel.IsChecked == true);
        configuration.Set("chkClick", _chkClick.IsChecked == true);
        configuration.Save();
    }

    private void TimTicker_Tick(object? sender, EventArgs e)
    {
        _timTicker?.Stop();

        _bmpScreen = CaptureControl(_ctrHole, _bmpScreen, window: this);
            
        if (_automationBot != null && _bmpScreen != null)
        {
            _bmpScreen = _automationBot.Process(_bmpScreen, _ctrOutput);
        }

        var oldImage = _picPreview.ImageShow;
        _picPreview.ImageShow = _bmpScreen != null ? BitmapConverter.ConvertToAvalonia(_bmpScreen) : null;
        oldImage?.Dispose();

        if (_timTicker != null)
        {
            _timTicker.Interval = TimeSpan.FromMilliseconds(1000.0 / (double)(_numFps.Value ?? 10));
            _timTicker.Start();
        }
    }

    private void BtnStartEnd_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_running) End();
        else Start();
    }

    private void DdlAutomationBot_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_ddlAutomationBot.SelectedItem is string botName)
            InitBot(botName);
    }

    private void BtnAutomationBotConfig_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_automationBot == null) return;
        IAutomationBot bot = _automationBot!;
        IConfiguration defaultConfig = bot.GetDefaultConfiguration() ?? new MemoryBackedConfiguration();
        FileBackedConfiguration config = new(bot.Name);
        config.Load(defaultConfig);
        FrmAutomationBotParams frmParams = new(config);
        frmParams.ShowDialog(this);
        InitBot(bot.Name);
    }

    private void Start()
    {
        if (_running) return;
        _running = true;
        _btnStartEnd.Content = "End";
        if (_ddlAutomationBot.SelectedItem is string botName)
            InitBot(botName);
    }

    private void End()
    {
        if (!_running) return;
        _running = false;
        _btnStartEnd.Content = "Start";
    }

    private void InitBot(string botName)
    {
        _automationBot = AutomationBotFactory.CreateFromName(botName);
        FileBackedConfiguration botConfiguration = new(botName);
        botConfiguration.Load();
        _automationBot?.Init(_ctrOutput, botConfiguration);
    }

    private void ChkKeepToplevel_CheckedChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Topmost = _chkKeepToplevel.IsChecked == true;
    }
    
    
    public static SKBitmap? CaptureControl(Control? ctrl, SKBitmap? bmp = null, Window? window = null)
    {
        if (ctrl == null || window == null) { return bmp; }

        Point? relativeToWindow = ctrl.TranslatePoint(new Point(0, 0), window);
        if (relativeToWindow.HasValue == false) { return bmp; }

        PixelPoint screenPoint = window.PointToScreen(relativeToWindow.Value);
        int absoluteLeft = screenPoint.X;
        int absoluteTop = screenPoint.Y;

        double scale;
        try
        {
            scale = window.RenderScaling;
        }
        catch
        {
            scale = 1.0;
        }

        int offsetLeft = (int)Math.Ceiling(1 * scale);
        int offsetTop  = (int)Math.Ceiling(1 * scale);

        absoluteLeft += offsetLeft;
        absoluteTop  += offsetTop;

        int pixelWidth  = Math.Max(1, (int)Math.Round(ctrl.Bounds.Width * scale));
        int pixelHeight = Math.Max(1, (int)Math.Round(ctrl.Bounds.Height * scale));

        bmp = Platform.Current.Screen_CaptureRegion(bmp: bmp,
            left: absoluteLeft,
            top: absoluteTop,
            width: pixelWidth,
            height: pixelHeight);
        return bmp;
    }

}