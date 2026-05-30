using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Threading;
using VAR.Toolbox.Code;

namespace VAR.Toolbox.Controls;

public class CtrOutput : UserControl, IOutputHandler
{
    private readonly ListBox _listBox;
    private readonly DispatcherTimer _timer;
    private readonly ObservableCollection<OutputItem> _items = [];

    private class OutputItem
    {
        public string Text { get; set; } = string.Empty;
        public object? Data { get; set; }
        public override string ToString() => Text;
    }

    public event EventHandler? ItemDoubleClick;

    public CtrOutput()
    {
        _listBox = new ListBox
        {
            Classes = { "mono", },
            SelectionMode = SelectionMode.Multiple,
            ItemsSource = _items,
        };
        Content = _listBox;
        _listBox.DoubleTapped += (s, e) => ItemDoubleClick?.Invoke(s, e);

        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100), };
        _timer.Tick += Timer_Tick;
        _timer.Start();
    }

    private bool _updated;
    private readonly List<OutputItem> _pendingOutput = [];

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (_updated) UpdatePosition();
    }

    private void UpdatePosition()
    {
        lock (_pendingOutput)
        {
            foreach (OutputItem item in _pendingOutput)
            {
                _items.Add(item);
            }

            _pendingOutput.Clear();
            _updated = false;

            if (_items.Count > 0)
            {
                _listBox.ScrollIntoView(_items[_items.Count - 1]);
            }
        }
    }

    public void Clean()
    {
        Dispatcher.UIThread.Post(() => { _items.Clear(); });
    }

    public void AddLine(string line, object? data = null)
    {
        lock (_pendingOutput)
        {
            _pendingOutput.Add(new OutputItem { Text = line, Data = data, });
            _updated = true;
        }
    }

    public string? GetCurrentText()
    {
        return (_listBox.SelectedItem as OutputItem)?.Text;
    }

    public object? GetCurrentData()
    {
        return (_listBox.SelectedItem as OutputItem)?.Data;
    }
}