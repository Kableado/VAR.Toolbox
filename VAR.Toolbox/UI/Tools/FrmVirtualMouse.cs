using Avalonia;
using Avalonia.Controls;

using VAR.Toolbox.Code;

namespace VAR.Toolbox.UI.Tools;

public class FrmVirtualMouse : Window, IToolForm
{
    public string ToolName => "VirtualMouse";
    public bool HasIcon => false;

    private readonly GlobalKeyboardHook _globalKeyboard = new();
    private readonly ListBox _lsbInputs;
    private readonly Button _btnStartStop;

    public FrmVirtualMouse()
    {
        Title = "VirtualMouse";
        Width = 400;
        Height = 350;

        _lsbInputs = new ListBox();

        _btnStartStop = new Button { Content = "Start", };
        _btnStartStop.Click += BtnStartStop_Click;

        DockPanel layout = new() { Margin = new Thickness(8), };
        DockPanel.SetDock(_btnStartStop, Dock.Top);
        layout.Children.Add(_btnStartStop);
        layout.Children.Add(_lsbInputs);

        Content = layout;

        _globalKeyboard.KeyDown += GlobalKeyboard_OnKeyDown;
    }

    private void GlobalKeyboard_OnKeyDown(object? sender, System.Windows.Forms.KeyEventArgs keyEvent)
    {
        string key = keyEvent.KeyCode.ToString();
        Avalonia.Threading.Dispatcher.UIThread.Post(() => { _lsbInputs.Items.Add(key); });

        if (key == "F1") { Mouse.SetButton(Mouse.MouseButtons.Left, true); }
        if (key == "F2") { Mouse.Move(0, -10000); }
        if (key == "F3") { Mouse.SetButton(Mouse.MouseButtons.Left, false); }
    }

    private void BtnStartStop_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_globalKeyboard.IsCapturing())
        {
            _globalKeyboard.Stop();
            _btnStartStop.Content = "Start";
            return;
        }

        _globalKeyboard.Start(true);
        _btnStartStop.Content = "Stop";
    }
}