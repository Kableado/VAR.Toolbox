using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Threading;
using VAR.Toolbox.Code;
using VAR.Toolbox.Code.Platforms;

namespace VAR.Toolbox.UI.Tools;

public class PnlSuspension : UserControl, IToolPanel
{
    public const string PreSuspendEventName = "PreSuspend";

    private readonly Random _rnd = new();
    private readonly CheckBox _chkSuspendAtCustom;
    private readonly NumericUpDown _numOffset;
    private readonly ComboBox _ddlCustomHour;
    private readonly ComboBox _ddlCustomMinute;
    private readonly TextBlock _lblCountdown;
    private readonly DispatcherTimer _timTicker;

    public PnlSuspension()
    {
        _chkSuspendAtCustom = new CheckBox { Content = "SuspendAtCustom", };
        _numOffset = new NumericUpDown { Minimum = 1, Maximum = 600, Value = 180, Width = 50, FormatString = "0", };

        _ddlCustomHour = new ComboBox { Width = 60, };
        _ddlCustomMinute = new ComboBox { Width = 60, };

        _lblCountdown = new TextBlock { Text = "00:00:00:00", FontSize = 20, FontWeight = Avalonia.Media.FontWeight.Bold, };

        Button btnRandOffset = new() { Content = "Rand", };
        btnRandOffset.Click += BtnRandOffset_Click;

        Button btnCustomSuspendNow = new() { Content = "N", Width = 30, };
        btnCustomSuspendNow.Click += BtnCustomSuspendNow_Click;

        // Layout
        StackPanel hourMinRow = new() { Orientation = Orientation.Horizontal, Spacing = 4, };
        hourMinRow.Children.Add(_ddlCustomHour);
        hourMinRow.Children.Add(_ddlCustomMinute);
        hourMinRow.Children.Add(btnCustomSuspendNow);

        StackPanel offsetRow = new() { Orientation = Orientation.Horizontal, Spacing = 4, };
        offsetRow.Children.Add(btnRandOffset);
        offsetRow.Children.Add(_numOffset);
        offsetRow.Children.Add(new TextBlock { Text = "Secs.", VerticalAlignment = VerticalAlignment.Center, });

        HeaderedContentControl grp = new() { Header = "Suspension", };
        StackPanel stack = new() { Spacing = 4, };
        stack.Children.Add(_chkSuspendAtCustom);
        stack.Children.Add(hourMinRow);
        stack.Children.Add(offsetRow);
        stack.Children.Add(_lblCountdown);
        grp.Content = stack;

        Content = new Border { Padding = new Thickness(4), Child = grp, Width = 200, };

        DdlCustomHour_Load();
        DdlCustomMinute_Load();
        RandomizeOffset();

        _timTicker = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1), };
        _timTicker.Tick += TimTicker_Tick;
        _timTicker.Start();
    }

    private void BtnCustomSuspendNow_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        CustomHourMinute_SetNow();
    }

    private void BtnRandOffset_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        RandomizeOffset();
    }

    private void TimTicker_Tick(object? sender, EventArgs e)
    {
        ResetCountdown();
        DateTime now = DateTime.Now;

        if (_ddlCustomHour.SelectedIndex >= 0 && _ddlCustomMinute.SelectedIndex >= 0)
        {
            DateTime dtSuspendAtCustom =
                new DateTime(now.Year, now.Month, now.Day,
                        _ddlCustomHour.SelectedIndex, _ddlCustomMinute.SelectedIndex, 0)
                    .AddSeconds(Convert.ToInt32(_numOffset.Value ?? 0));

            CheckTime(dtSuspendAtCustom, now);
        }

        _timTicker.Stop();
        _timTicker.Start();
    }

    private void DdlCustomHour_Load()
    {
        List<string> items = new();
        for (int i = 0; i < 24; i++) items.Add($"{i:00}");
        _ddlCustomHour.ItemsSource = items;
    }

    private void DdlCustomMinute_Load()
    {
        List<string> items = new();
        for (int i = 0; i < 60; i++) items.Add($"{i:00}");
        _ddlCustomMinute.ItemsSource = items;
    }

    private void CustomHourMinute_SetNow()
    {
        DateTime now = DateTime.Now;
        _ddlCustomHour.SelectedIndex = now.Hour;
        _ddlCustomMinute.SelectedIndex = now.Minute;
    }

    private void RandomizeOffset()
    {
        _numOffset.Value = (_rnd.Next() % 599) + 1;
    }

    private void ResetCountdown()
    {
        _lblCountdown.Text = "00:00:00:00";
    }

    private void SetCountdown(DateTime dateTime, DateTime now)
    {
        TimeSpan timeSpan = dateTime - now;
        _lblCountdown.Text = $"{timeSpan.Days:00}:{timeSpan.Hours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
    }

    private void CheckTime(DateTime dtSuspendAtCustom, DateTime now)
    {
        if (DateTime.Compare(now, dtSuspendAtCustom) > 0)
        {
            if (_chkSuspendAtCustom.IsChecked == true)
            {
                _chkSuspendAtCustom.IsChecked = false;
                RandomizeOffset();
                SuspendSystem();
            }
            else
            {
                _chkSuspendAtCustom.IsEnabled = false;
            }
        }
        else
        {
            SetCountdown(dtSuspendAtCustom, now);
            _chkSuspendAtCustom.IsEnabled = true;
        }
    }

    private void SuspendSystem()
    {
        EventDispatcher.EmitEvent(PreSuspendEventName, null);
        Platform.Current.System_SetSuspendState(false, true, false);
    }
}