#pragma warning disable IDE0019

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Controls.Primitives;
using Avalonia.Platform;
using Avalonia.Threading;
using VAR.Toolbox.Code;

namespace VAR.Toolbox.UI;

public class FrmToolbox : Window
{
    #region Declarations

    private bool _closing;
    private TrayIcon? _trayIcon;
    private static FrmToolbox? _currentInstance;

    #endregion Declarations

    #region Form life cycle

    public FrmToolbox()
    {
        InitializeDynamicComponents();
        _currentInstance = this;
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        if (_closing)
        {
            base.OnClosing(e);
            return;
        }

        HideChildWindows();
        Hide();
        e.Cancel = true;
    }

    #endregion Form life cycle

    #region Dynamic layout

    private void InitializeDynamicComponents()
    {
        Title = "Toolbox";
        CanResize = false;
        Width = 440;

        // Get list of ToolForms
        Type iToolForm = typeof(IToolForm);
        IEnumerable<Type> toolFormTypes = ReflectionUtils.GetTypesOfInterface(iToolForm);
        Dictionary<string, Type> dictToolFormTypes = toolFormTypes.ToDictionary(t =>
        {
            IToolForm? toolForm =
                System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(t) as IToolForm;
            return toolForm?.ToolName ?? t.Name;
        });

        // Get list of ToolPanels
        Type iToolPanel = typeof(IToolPanel);
        IEnumerable<Type> toolPanelTypes = ReflectionUtils.GetTypesOfInterface(iToolPanel).OrderBy(t => t.Name);

        StackPanel mainStack = new() { Spacing = 5, Margin = new Thickness(10), };

        // lblToolbox
        TextBlock lblToolbox = new()
        {
            Text = "Toolbox",
            FontSize = 28,
            FontWeight = FontWeight.Bold,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(0, 0, 0, 10),
        };
        mainStack.Children.Add(lblToolbox);

        // Tool buttons in a 2-column WrapPanel
        WrapPanel buttonsPanel = new()
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Center,
        };

        IEnumerable<KeyValuePair<string, Type>> sortedToolForms = dictToolFormTypes.OrderBy(p => p.Key);
        foreach (KeyValuePair<string, Type> p in sortedToolForms)
        {
            Button btn = new()
            {
                Content = p.Key,
                Width = 200,
                Height = 40,
                Margin = new Thickness(2),
            };
            btn.Click += (_, _) => { CreateWindow(p.Value); };
            buttonsPanel.Children.Add(btn);
        }

        mainStack.Children.Add(buttonsPanel);

        // Tool panels in a WrapPanel
        WrapPanel panelsWrap = new()
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Center,
        };

        foreach (Type t in toolPanelTypes)
        {
            Control? pnl = Activator.CreateInstance(t) as Control;
            if (pnl == null) { continue; }

            pnl.Margin = new Thickness(2);
            panelsWrap.Children.Add(pnl);
        }

        mainStack.Children.Add(panelsWrap);

        // btnExit
        Button btnExit = new()
        {
            Content = "Exit",
            Height = 40,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Margin = new Thickness(0, 10, 0, 0),
        };
        btnExit.Click += BtnExit_Click;
        mainStack.Children.Add(btnExit);

        ScrollViewer scroll = new()
        {
            Content = mainStack,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
        };

        Content = scroll;

        // Load the application icon from embedded resource (Toolbox.ico) if the Window.Icon is not already set
        if (this.Icon == null)
        {
            try
            {
                var asm = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
                string? resName = asm.GetManifestResourceNames()
                    .FirstOrDefault(n => n.EndsWith("Toolbox.ico", StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(resName))
                {
                    using var rs = asm.GetManifestResourceStream(resName);
                    if (rs != null)
                    {
                        this.Icon = new WindowIcon(rs);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        // Tray icon (Avalonia TrayIcon)
        try
        {
            _trayIcon = new TrayIcon
            {
                ToolTipText = "VAR.Toolbox",
                IsVisible = true,
                // If the Window has an Icon set, use it; otherwise leave null
                Icon = this.Icon
            };

            _trayIcon.Clicked += (_, _) => NiTray_MouseClick();
        }
        catch (Exception ex)
        {
            // If TrayIcon can't be created on this platform/version, ignore silently
            Logger.Log(ex);
            _trayIcon = null;
        }

        // When the window is opened, read the actual layout height of the content and
        // set the window Height accordingly (capped at 90% primary screen). Use dispatcher
        // to run after layout has been processed.
        Opened += (_, _) =>
        {
            // Post to dispatcher to ensure layout pass has completed
            Dispatcher.UIThread.Post(() =>
            {
                double contentHeight = mainStack.DesiredSize.Height;

                double maxAllowed = double.PositiveInfinity;
                try
                {
                    Screen? primary = Screens.Primary;
                    if (primary != null)
                    {
                        PixelRect working = primary.WorkingArea;
                        if (!double.IsNaN(working.Height) && working.Height > 0)
                        {
                            maxAllowed = working.Height * 0.9;
                        }
                    }
                }
                catch
                {
                    maxAllowed = double.PositiveInfinity;
                }

                double finalHeight = double.IsPositiveInfinity(maxAllowed)
                    ? contentHeight
                    : Math.Min(contentHeight, maxAllowed);

                Height = finalHeight;
                scroll.MaxHeight = finalHeight;
            }, DispatcherPriority.Background);
        };
    }

    #endregion Dynamic layout

    #region UI events

    private async void BtnExit_Click(object? sender, RoutedEventArgs e)
    {
        try
        {
            bool confirmed = await Utils.MsgConfirm(this, "Exit?", "Are you sure want to exit?");

            if (confirmed)
            {
                _closing = true;
                CloseChildWindows();
                try
                {
                    if (_trayIcon != null)
                    {
                        _trayIcon.IsVisible = false;
                        _trayIcon = null;
                    }
                }
                catch { }
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    desktop.Shutdown();
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Log(ex);
        }
    }

    #endregion UI events

    #region Window handling

    private void CreateWindow(Type type)
    {
        Window? wnd = Activator.CreateInstance(type) as Window;
        if (wnd == null)
        {
            return;
        }

        _windows.Add(wnd);
        wnd.Closing += WndChild_Closing;
        wnd.Show();
    }

    private readonly List<Window> _windows = new();

    private void WndChild_Closing(object? sender, WindowClosingEventArgs e)
    {
        if (sender is Window w)
        {
            _windows.Remove(w);
        }
    }

    private void CloseChildWindows()
    {
        while (_windows.Count > 0)
        {
            _windows[0].Close();
        }
    }

    // ShowChildWindows was removed because it's not referenced anywhere in the codebase.

    private void HideChildWindows()
    {
        foreach (Window wnd in _windows)
        {
            wnd.Hide();
        }
    }

    private void NiTray_MouseClick()
    {

        // If visible, hide; otherwise show (and bring to front)
        if (IsVisible)
        {
            HideChildWindows();
            Hide();
            return;
        }

        WindowState = WindowState.Minimized;
        Show();
        Activate();
        foreach (Window wnd in _windows)
        {
            try { wnd.Show(); wnd.Activate(); } catch { }
        }
        WindowState = WindowState.Normal;
    }

    public static void StaticCreateWindow(Type type)
    {
        _currentInstance?.CreateWindow(type);
    }

    public static List<T> StaticGetWindowsOfType<T>()
    {
        List<T> list = new();
        if (_currentInstance == null) { return list; }

        foreach (Window wnd in _currentInstance._windows)
        {
            if (wnd is T)
            {
                list.Add((T)(object)wnd);
            }
        }

        return list;
    }

    #endregion Window handling
}