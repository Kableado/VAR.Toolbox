using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace VAR.Toolbox.UI
{
    public class FrmDialogString : Window
    {
        private readonly TextBlock _lblDescription;
        private readonly TextBox _txtValue;

        public FrmDialogString()
        {
            Title = "DialogString";
            Width = 440;
            Height = 284;
            CanResize = true;

            _lblDescription = new TextBlock { TextWrapping = Avalonia.Media.TextWrapping.Wrap, };
            _txtValue = new TextBox { AcceptsReturn = true, };

            Button btnAccept = new() { Content = "Accept", };
            Button btnCancel = new() { Content = "Cancel", };

            btnAccept.Click += (_, _) => { DialogOk = true; Close(); };
            btnCancel.Click += (_, _) => { DialogOk = false; Close(); };

            StackPanel buttons = new()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Spacing = 5,
            };
            buttons.Children.Add(btnAccept);
            buttons.Children.Add(btnCancel);

            DockPanel layout = new() { Margin = new Thickness(12), };
            DockPanel.SetDock(buttons, Dock.Bottom);
            DockPanel.SetDock(_lblDescription, Dock.Top);
            layout.Children.Add(buttons);
            layout.Children.Add(_lblDescription);
            layout.Children.Add(_txtValue);

            Content = layout;
        }

        public bool DialogOk { get; private set; }

        public new string? Title
        {
            get => base.Title;
            set => base.Title = value;
        }

        public string? Description
        {
            get => _lblDescription.Text;
            set => _lblDescription.Text = value;
        }

        public string? Value
        {
            get => _txtValue.Text;
            set => _txtValue.Text = value;
        }
    }
}