using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Threading;
using VAR.Toolbox.Code;
using VAR.Toolbox.Code.Platforms;

namespace VAR.Toolbox.UI.Tools;

public class FrmCover : Window
{
    private readonly Random _rnd = new();
    private readonly DispatcherTimer _timer;
    private uint _mouseX;
    private uint _mouseY;

    public FrmCover()
    {
        Platform.Current.Mouse_GetPosition(out _mouseX, out _mouseY);

        Title = Platform.Current.System_GetActiveWindowTitle();
        Topmost = true;
        WindowDecorations = WindowDecorations.None;
        Background = Brushes.Black;

        PointerPressed += FrmCover_PointerPressed;
        KeyDown += FrmCover_KeyDown;

        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1), };
        _timer.Tick += Timer_Tick;

        Opened += FrmCover_Opened;
    }

    private void FrmCover_Opened(object? sender, EventArgs e)
    {
        Screens screens = Screens;
        if (screens.All.Count > 0)
        {
            double left = double.MaxValue, top = double.MaxValue, right = double.MinValue, bottom = double.MinValue;
            foreach (Screen s in screens.All)
            {
                if (s.Bounds.X < left) left = s.Bounds.X;
                if (s.Bounds.Y < top) top = s.Bounds.Y;
                if (s.Bounds.X + s.Bounds.Width > right) right = s.Bounds.X + s.Bounds.Width;
                if (s.Bounds.Y + s.Bounds.Height > bottom) bottom = s.Bounds.Y + s.Bounds.Height;
            }
            Position = new PixelPoint((int)left, (int)top);
            Width = right - left;
            Height = bottom - top;
        }
        Cursor = new Cursor(StandardCursorType.None);
        _timer.Start();
        Activate();
    }

    private void RestoreAndClose()
    {
        Cursor = Cursor.Default;
        _timer.Stop();
        Platform.Current.Mouse_SetPosition(_mouseX, _mouseY);
        Close();
        EventDispatcher.EmitEvent(PnlCover.PostCoverEventName, null);
    }

    private void FrmCover_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        RestoreAndClose();
    }

    private void FrmCover_KeyDown(object? sender, KeyEventArgs e)
    {
        RestoreAndClose();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        Activate();
        try
        {
            Platform.Current.Mouse_Move((_rnd.Next() % 11) - 5, (_rnd.Next() % 11) - 5);
        }
        catch (Exception) { /* Ignore */ }
        _timer.Stop();
        _timer.Start();
    }
}