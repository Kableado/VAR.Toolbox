using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;

namespace VAR.Toolbox.UI;

public static class Utils
{
    public static async Task<bool> MsgConfirm(Window owner, string title, string message)
    {
        Window dlg = new()
        {
            Title = title,
            Width = 300,
            MaxHeight = 300,
            SizeToContent = SizeToContent.Height,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false,
        };

        bool confirmed = false;
        StackPanel stack = new() { Margin = new Thickness(20), Spacing = 15, };
        stack.Children.Add(new TextBlock { Text = message, TextWrapping = TextWrapping.Wrap,});

        StackPanel buttons = new() { Orientation = Orientation.Horizontal, Spacing = 10, HorizontalAlignment = HorizontalAlignment.Right, };
        Button btnYes = new() { Content = "Yes", };
        Button btnNo = new() { Content = "No", };
        btnYes.Click += (_, _) => { confirmed = true; dlg.Close(); };
        btnNo.Click += (_, _) => { confirmed = false; dlg.Close(); };
        buttons.Children.Add(btnYes);
        buttons.Children.Add(btnNo);
        stack.Children.Add(buttons);

        ScrollViewer sv = new()
        {
            Content = stack,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            MaxHeight = dlg.MaxHeight,
        };

        dlg.Content = sv;

        await dlg.ShowDialog(owner);
        return confirmed;
    }
    
    public static async Task MsgBox(Window owner, string title, string message)
    {
        Window dlg = new()
        {
            Title = title,
            Width = 300,
            MaxHeight = 300,
            SizeToContent = SizeToContent.Height,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false,
        };

        StackPanel stack = new() { Margin = new Thickness(20), Spacing = 15, };
        stack.Children.Add(new TextBlock { Text = message, });

        StackPanel buttons = new() { Orientation = Orientation.Horizontal, Spacing = 10, HorizontalAlignment = HorizontalAlignment.Right, };
        Button btnOk = new() { Content = "Ok", };
        btnOk.Click += (_, _) => { dlg.Close(); };
        buttons.Children.Add(btnOk);
        stack.Children.Add(buttons);

        ScrollViewer sv = new()
        {
            Content = stack,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            MaxHeight = dlg.MaxHeight,
        };

        dlg.Content = sv;

        await dlg.ShowDialog(owner);
    }
}