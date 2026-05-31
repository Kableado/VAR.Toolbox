using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Threading;
using VAR.Toolbox.Code;
using VAR.Toolbox.Code.Platforms.Windows;

namespace VAR.Toolbox.UI.Tools;

public class PnlCover : UserControl, IToolPanel
{
    public const string PreCoverEventName = "PreCover";
    public const string PostCoverEventName = "PostCover";

    private readonly CheckBox _chkAutoCover;
    private readonly NumericUpDown _numInactive;
    private readonly TextBlock _lblInactive;
    private readonly DispatcherTimer _timTicker;

    public PnlCover()
    {
        _chkAutoCover = new CheckBox { Content = "AutoCover", IsChecked = true, };
        _numInactive = new NumericUpDown { Minimum = 1, Maximum = 300, Value = 180, Width = 50, FormatString = "0", };
        _lblInactive = new TextBlock { Text = "lblInactive", };

        Button btnCover = new() { Content = "Cover", };
        btnCover.Click += BtnCover_Click;

        StackPanel autoRow = new() { Orientation = Orientation.Horizontal, Spacing = 5, };
        autoRow.Children.Add(_chkAutoCover);
        autoRow.Children.Add(_numInactive);

        HeaderedContentControl grp = new() { Header = "Cover", };
        StackPanel stack = new() { Spacing = 4, };
        stack.Children.Add(autoRow);
        stack.Children.Add(_lblInactive);
        stack.Children.Add(btnCover);
        grp.Content = stack;

        Content = new Border { Padding = new Thickness(4), Child = grp, Width = 200, };

        _timTicker = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1), };
        _timTicker.Tick += TimTicker_Tick;
        _timTicker.Start();
    }

    private void TimTicker_Tick(object? sender, EventArgs e)
    {
        _timTicker.Stop();
        uint inactiveTime = Win32.GetLastInputTime();
        _lblInactive.Text = $"Inactive by {inactiveTime} seconds";

        if (_chkAutoCover.IsChecked == true)
        {
            if (inactiveTime > (uint)(_numInactive.Value ?? 180))
            {
                CoverScreen();
            }
        }
        _timTicker.Start();
    }

    private void BtnCover_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        CoverScreen();
    }

    private FrmCover? _frmCover;

    private void CoverScreen()
    {
        EventDispatcher.EmitEvent(PreCoverEventName, null);

        if (_frmCover != null)
        {
            _frmCover.Show();
            return;
        }

        _frmCover = new FrmCover();
        _frmCover.Closing += (_, _) => { _frmCover = null; };
        _frmCover.Show();
    }
}