using Avalonia;
using Avalonia.Controls;
using VAR.Toolbox.UI;

namespace VAR.Toolbox.TestPlugin
{
    // Minimal Avalonia Window implementation for the test plugin
    public class FrmTestPlugin : Window, IToolForm
    {
        public FrmTestPlugin()
        {
            Title = "Test";
            Width = 400;
            Height = 300;

            // Simple content to show the window is working
            Content = new StackPanel
            {
                Margin = new Thickness(10),
                Children =
                {
                    new TextBlock { Text = "Test Plugin", FontWeight = Avalonia.Media.FontWeight.Bold, FontSize = 18, },
                    new TextBlock { Text = "This plugin has been migrated to Avalonia.", },
                },
            };
        }

        public string ToolName => "Test";

        public bool HasIcon => false;
    }
}
