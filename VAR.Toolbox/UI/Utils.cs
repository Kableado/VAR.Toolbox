using System.Threading.Tasks;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace VAR.Toolbox.UI;

public static class Utils
{
    public static async Task<bool> MsgConfirm(Window owner, string title, string message)
    {
        // Simple confirmation - create a dialog
        Window dlg = new()
        {
            Title = title,
            Width = 300,
            Height = 100,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false,
        };

        bool confirmed = false;
        StackPanel stack = new() { Margin = new Thickness(20), Spacing = 15, };
        stack.Children.Add(new TextBlock { Text = message, });

        StackPanel buttons = new() { Orientation = Orientation.Horizontal, Spacing = 10, HorizontalAlignment = HorizontalAlignment.Right, };
        Button btnYes = new() { Content = "Yes", };
        Button btnNo = new() { Content = "No", };
        btnYes.Click += (_, _) => { confirmed = true; dlg.Close(); };
        btnNo.Click += (_, _) => { confirmed = false; dlg.Close(); };
        buttons.Children.Add(btnYes);
        buttons.Children.Add(btnNo);
        stack.Children.Add(buttons);
        dlg.Content = stack;

        await dlg.ShowDialog(owner);
        return confirmed;
    }
    
    public static async Task MsgBox(Window owner, string title, string message)
    {
        Window dlg = new()
        {
            Title = title,
            Width = 300,
            Height = 100,
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
        dlg.Content = stack;

        await dlg.ShowDialog(owner);
    }
}