using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace VAR.Toolbox.UI.Tools;

public class FrmProxyCmdConfig : Window
{
    private readonly ListBox _lsvCmdProxyConfigs;
    private readonly TextBox _txtCmdProxyConfigName;
    private readonly TextBox _txtCmdProxyConfigContent;

    public FrmProxyCmdConfig()
    {
        Title = "ProxyCmdConfig";
        Width = 500;
        Height = 400;

        _lsvCmdProxyConfigs = new ListBox();
        _lsvCmdProxyConfigs.SelectionChanged += LsvCmdProxyConfigs_SelectionChanged;

        _txtCmdProxyConfigName = new TextBox { Classes = { "mono", }, PlaceholderText = "Name", };
        _txtCmdProxyConfigContent = new TextBox { Classes = { "mono", }, AcceptsReturn = true, PlaceholderText = "Config", };

        Button btnSave = new() { Content = "Save", };
        btnSave.Click += BtnSave_Click;
        Button btnDelete = new() { Content = "Delete", };
        btnDelete.Click += BtnDelete_Click;
        Button btnNew = new() { Content = "New", };
        btnNew.Click += BtnNew_Click;

        StackPanel buttons = new() { Orientation = Orientation.Horizontal, Spacing = 5, };
        buttons.Children.Add(btnNew);
        buttons.Children.Add(btnSave);
        buttons.Children.Add(btnDelete);

        DockPanel rightPanel = new();
        DockPanel.SetDock(_txtCmdProxyConfigName, Dock.Top);
        DockPanel.SetDock(buttons, Dock.Bottom);
        rightPanel.Children.Add(_txtCmdProxyConfigName);
        rightPanel.Children.Add(buttons);
        rightPanel.Children.Add(_txtCmdProxyConfigContent);

        Grid grid = new() { Margin = new Thickness(8), };
        grid.ColumnDefinitions.Add(new ColumnDefinition(1, GridUnitType.Star));
        grid.ColumnDefinitions.Add(new ColumnDefinition(5, GridUnitType.Pixel));
        grid.ColumnDefinitions.Add(new ColumnDefinition(2, GridUnitType.Star));
        Grid.SetColumn(_lsvCmdProxyConfigs, 0);
        Grid.SetColumn(rightPanel, 2);
        grid.Children.Add(_lsvCmdProxyConfigs);
        grid.Children.Add(rightPanel);

        Content = grid;
        LoadData();
    }

    private void LsvCmdProxyConfigs_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        ProxyCmdConfigItem? selectedConfig = _lsvCmdProxyConfigs.SelectedItem as ProxyCmdConfigItem;
        if (selectedConfig == null) { CleanConfig(); return; }
        ShowConfig(selectedConfig);
    }

    private void BtnSave_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) { SaveConfig(); }
    private void BtnDelete_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) { DeleteSelected(); }
    private void BtnNew_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) { CreateNew(); }

    private void LoadData()
    {
        List<ProxyCmdConfigItem> configItems = GetConfigurationItems();
        _lsvCmdProxyConfigs.ItemsSource = configItems;
    }

    private void SaveData()
    {
        List<ProxyCmdConfigItem>? items = _lsvCmdProxyConfigs.ItemsSource as List<ProxyCmdConfigItem>;
        if (items == null) return;

        StringBuilder sbConfig = new();
        foreach (ProxyCmdConfigItem config in items)
        {
            sbConfig.Append($"{config.Name}|{config.Config}\n");
        }

        string configFileName = GetConfigFileName();
        File.WriteAllText(configFileName, sbConfig.ToString());

        List<FrmProxyCmd> listForms = FrmToolbox.StaticGetWindowsOfType<FrmProxyCmd>();
        foreach (FrmProxyCmd frm in listForms)
        {
            frm.LoadConfig();
        }
    }

    private void CleanConfig()
    {
        _txtCmdProxyConfigName.Text = string.Empty;
        _txtCmdProxyConfigContent.Text = string.Empty;
    }

    private void ShowConfig(ProxyCmdConfigItem config)
    {
        _txtCmdProxyConfigName.Text = config.Name;
        _txtCmdProxyConfigContent.Text = config.Config;
    }

    private void SaveConfig()
    {
        List<ProxyCmdConfigItem>? items = _lsvCmdProxyConfigs.ItemsSource as List<ProxyCmdConfigItem> ?? [];

        ProxyCmdConfigItem? selectedConfig = _lsvCmdProxyConfigs.SelectedItem as ProxyCmdConfigItem;
        if (selectedConfig == null)
        {
            selectedConfig = new ProxyCmdConfigItem
            {
                Name = _txtCmdProxyConfigName.Text ?? string.Empty,
                Config = _txtCmdProxyConfigContent.Text ?? string.Empty,
            };
            items.Add(selectedConfig);
            _lsvCmdProxyConfigs.ItemsSource = null;
            _lsvCmdProxyConfigs.ItemsSource = items;
        }
        else
        {
            selectedConfig.Name = _txtCmdProxyConfigName.Text ?? string.Empty;
            selectedConfig.Config = _txtCmdProxyConfigContent.Text ?? string.Empty;
            _lsvCmdProxyConfigs.ItemsSource = null;
            _lsvCmdProxyConfigs.ItemsSource = items;
        }
        SaveData();
    }

    private void DeleteSelected()
    {
        ProxyCmdConfigItem? selectedConfig = _lsvCmdProxyConfigs.SelectedItem as ProxyCmdConfigItem;
        if (selectedConfig == null) return;

        List<ProxyCmdConfigItem>? items = _lsvCmdProxyConfigs.ItemsSource as List<ProxyCmdConfigItem>;
        if (items == null) return;

        items = items.Where(c => c.Name != selectedConfig.Name).ToList();
        _lsvCmdProxyConfigs.ItemsSource = items;
        SaveData();
        CleanConfig();
    }

    private void CreateNew()
    {
        _lsvCmdProxyConfigs.SelectedIndex = -1;
        CleanConfig();
    }

    private static string GetConfigFileName()
    {
        string location = System.Reflection.Assembly.GetEntryAssembly()?.Location ??
                          System.Reflection.Assembly.GetExecutingAssembly().Location;
        string? path = Path.GetDirectoryName(location);
        string filenameWithoutExtension = Path.GetFileNameWithoutExtension(location);
        return $"{path}/{filenameWithoutExtension}.ProxyCmd.cfg";
    }

    private static string[] GetConfigurationLines()
    {
        string configFile = GetConfigFileName();
        return File.Exists(configFile) == false
            ? ["Dummy|Dummy:",]
            : File.ReadAllLines(configFile);
    }

    public static List<ProxyCmdConfigItem> GetConfigurationItems()
    {
        string[] configLines = GetConfigurationLines();
        List<ProxyCmdConfigItem> configItems = [];
        foreach (string configLine in configLines)
        {
            int idxSplit = configLine.IndexOf('|');
            if (idxSplit < 0) continue;
            string configName = configLine.Substring(0, idxSplit);
            string configData = configLine.Substring(idxSplit + 1);
            configItems.Add(new ProxyCmdConfigItem { Name = configName, Config = configData, });
        }
        return configItems;
    }
}

public class ProxyCmdConfigItem
{
    public string Name { get; set; } = string.Empty;
    public string Config { get; set; } = string.Empty;
    public override string ToString() => Name;
}