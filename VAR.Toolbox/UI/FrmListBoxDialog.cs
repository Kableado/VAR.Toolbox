using System.Collections.Generic;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace VAR.Toolbox.UI;

public class FrmListBoxDialog : Window
{
    private readonly ListBox _lsbItems;

    public FrmListBoxDialog()
    {
        Title = "ListBoxDialog";
        Width = 362;
        Height = 465;

        _lsbItems = new ListBox();
        _lsbItems.SelectionChanged += LsbItems_SelectionChanged;

        Button btnAccept = new() { Content = "Accept", };
        Button btnCancel = new() { Content = "Cancel", };
        btnAccept.Click += (_, _) => { DialogOk = true; Close(); };
        btnCancel.Click += (_, _) => { DialogOk = false; Close(); };

        StackPanel buttons = new()
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Right,
            Spacing = 5,
            Margin = new Thickness(0, 5, 0, 0),
        };
        buttons.Children.Add(btnAccept);
        buttons.Children.Add(btnCancel);

        DockPanel layout = new() { Margin = new Thickness(12), };
        DockPanel.SetDock(buttons, Dock.Bottom);
        layout.Children.Add(buttons);
        layout.Children.Add(_lsbItems);

        Content = layout;
    }

    public bool DialogOk { get; private set; }

    public new string? Title
    {
        get => base.Title;
        set => base.Title = value;
    }

    public void LoadItems(List<string> items)
    {
        _lsbItems.ItemsSource = items;
    }

    public string Value => (_lsbItems.SelectedItem as string) ?? string.Empty;

    private void LsbItems_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_lsbItems.SelectedItem != null)
        {
            DialogOk = true;
            Close();
        }
    }
}