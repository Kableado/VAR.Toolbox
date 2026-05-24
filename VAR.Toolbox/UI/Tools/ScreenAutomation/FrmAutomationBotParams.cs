using System.Collections.ObjectModel;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using VAR.Toolbox.Code.Configuration;

namespace VAR.Toolbox.UI.Tools.ScreenAutomation
{
    public class FrmAutomationBotParams : Window
    {
        private readonly FileBackedConfiguration _config;
        private readonly ObservableCollection<Pair> _pairs;
        private readonly StackPanel _paramsPanel;

        public FrmAutomationBotParams(FileBackedConfiguration config)
        {
            _config = config;
            Title = "Automation Bot Parameters";
            Width = 500;
            Height = 400;

            _pairs = [];
            _paramsPanel = new StackPanel { Spacing = 4, };

            foreach (string key in _config.GetKeys())
            {
                Pair pair = new() { Key = key, Value = _config.Get(key, string.Empty), };
                _pairs.Add(pair);
                AddPairRow(pair);
            }

            Content = new Border { Padding = new Thickness(8), Child = new ScrollViewer { Content = _paramsPanel, }, };

            Closing += FrmAutomationBotParams_Closing;
        }

        private void AddPairRow(Pair pair)
        {
            DockPanel row = new() { Margin = new Thickness(0, 2), };
            TextBlock lbl = new() { Text = pair.Key, Width = 150, VerticalAlignment = VerticalAlignment.Center, };
            TextBox txt = new() { Text = pair.Value, Tag = pair, };
            txt.TextChanged += (_, _) => { if (txt.Tag is Pair p) p.Value = txt.Text ?? string.Empty; };
            DockPanel.SetDock(lbl, Dock.Left);
            row.Children.Add(lbl);
            row.Children.Add(txt);
            _paramsPanel.Children.Add(row);
        }

        private void FrmAutomationBotParams_Closing(object? sender, WindowClosingEventArgs e)
        {
            foreach (Pair pair in _pairs)
            {
                if (string.IsNullOrEmpty(pair.Key)) continue;
                _config.Set(pair.Key, pair.Value);
            }
            _config.Save();
        }

        internal class Pair
        {
            public string Key { get; set; } = string.Empty;
            public string Value { get; set; } = string.Empty;
        }
    }
}