using System;
using System.Collections.Generic;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Threading;
using VAR.Toolbox.Code;
using System.Diagnostics.CodeAnalysis;
using VAR.Toolbox.Code.ProxyCmdExecutors;
using VAR.Toolbox.Controls;

namespace VAR.Toolbox.UI.Tools;

public class FrmProxyCmd : Window, IOutputHandler, IToolForm
{
    public string ToolName => "ProxyCmd";
    public bool HasIcon => false;

    private readonly object _executionLock = new();
    private readonly List<string> _cmdHistory = [];
    private int _currentHistoryIndex = -1;

    private readonly CtrOutput _ctrOutput;
    private readonly TextBox _txtInput;
    private readonly ComboBox _ddlCurrentConfig;

    public FrmProxyCmd()
    {
        Title = "ProxyCmd";
        Width = 600;
        Height = 400;

        _ctrOutput = new CtrOutput();
        _txtInput = new TextBox { Classes = { "mono", }, };
        _txtInput.KeyDown += TxtInput_KeyDown;

        _ddlCurrentConfig = new ComboBox { Width = 200, };
        _ddlCurrentConfig.SelectionChanged += DdlCurrentConfig_SelectionChanged;

        Button btnEnable = new() { Content = "Enable", };
        btnEnable.Click += BtnEnable_Click;
        Button btnDisable = new() { Content = "Disable", };
        btnDisable.Click += BtnDisable_Click;
        Button btnConfig = new() { Content = "Config", };
        btnConfig.Click += BtnConfig_Click;

        StackPanel topRow = new() { Orientation = Orientation.Horizontal, Spacing = 5, };
        topRow.Children.Add(_ddlCurrentConfig);
        topRow.Children.Add(btnEnable);
        topRow.Children.Add(btnDisable);
        topRow.Children.Add(btnConfig);

        DockPanel layout = new() { Margin = new Thickness(8), };
        DockPanel.SetDock(topRow, Dock.Top);
        DockPanel.SetDock(_txtInput, Dock.Bottom);
        layout.Children.Add(topRow);
        layout.Children.Add(_txtInput);
        layout.Children.Add(_ctrOutput);

        Content = layout;

        LoadConfig();
    }

    private void TxtInput_KeyDown(object? sender, KeyEventArgs e)
    {
        if (Monitor.IsEntered(_executionLock))
        {
            e.Handled = true;
            return;
        }

        if (e.Key == Key.Return || e.Key == Key.Enter)
        {
            e.Handled = true;
            string cmd = (_txtInput.Text ?? string.Empty).TrimStart().Replace("\n", "").Replace("\r", "");
            if (!string.IsNullOrEmpty(cmd))
            {
                _txtInput.Text = string.Empty;
                AddLine(cmd);
                PrepareProxyCmdExecutor();
                new Thread(() => ExecuteCmd(cmd)).Start();
            }
            return;
        }

        if (e.Key == Key.Up)
        {
            e.Handled = true;
            if (_currentHistoryIndex == -1) _currentHistoryIndex = _cmdHistory.Count;
            _currentHistoryIndex--;
            if (_currentHistoryIndex < 0) _currentHistoryIndex = 0;
            if (_currentHistoryIndex >= 0 && _currentHistoryIndex < _cmdHistory.Count)
            {
                _txtInput.Text = _cmdHistory[_currentHistoryIndex];
            }
            return;
        }

        if (e.Key == Key.Down)
        {
            e.Handled = true;
            if (_currentHistoryIndex > -1)
            {
                _currentHistoryIndex++;
                if (_currentHistoryIndex >= _cmdHistory.Count)
                {
                    _txtInput.Text = string.Empty;
                    _currentHistoryIndex = -1;
                }
                else
                {
                    _txtInput.Text = _cmdHistory[_currentHistoryIndex];
                }
            }
        }
    }

    private void BtnEnable_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        PrepareProxyCmdExecutor();
        bool result = _proxyCmdExecutor.Enable();
        AddLine($"Enable: {result}");
    }

    private void BtnDisable_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        PrepareProxyCmdExecutor();
        bool result = _proxyCmdExecutor.Disable();
        AddLine($"Disable: {result}");
    }

    private void DdlCurrentConfig_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        CleanProxyCmdExecutor();
    }

    private void BtnConfig_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        FrmToolbox.StaticCreateWindow(typeof(FrmProxyCmdConfig));
    }

    private IProxyCmdExecutor? _proxyCmdExecutor;

    [MemberNotNull(nameof(_proxyCmdExecutor))]
    private void PrepareProxyCmdExecutor()
    {
        if (_proxyCmdExecutor == null)
        {
            _proxyCmdExecutor = ProxyCmdExecutorFactory.CreateFromConfig(GetCurrentConfig()) ??
                                new ProxyCmdExecutorDummy(string.Empty);
        }
    }

    private void CleanProxyCmdExecutor()
    {
        if (_proxyCmdExecutor is IDisposable disposable) disposable.Dispose();
        _proxyCmdExecutor = null;
    }

    private void ExecuteCmd(string cmdString)
    {
        Monitor.Enter(_executionLock);
        try
        {
            _cmdHistory.Add(cmdString);
            _currentHistoryIndex = -1;
            PrepareProxyCmdExecutor();
            _proxyCmdExecutor.ExecuteCmd(cmdString, this);
        }
        catch (Exception ex)
        {
            Logger.Log(ex);
            AddLine(ex.Message);
        }
        Monitor.Exit(_executionLock);
    }

    public void Clean()
    {
        Dispatcher.UIThread.Post(() => { _ctrOutput.Clean(); });
    }

    public void AddLine(string line, object? data = null)
    {
        _ctrOutput.AddLine(line, data);
    }

    public void LoadConfig()
    {
        CleanProxyCmdExecutor();
        List<ProxyCmdConfigItem> configItems = FrmProxyCmdConfig.GetConfigurationItems();

        string? previousSelectedName = null;
        if (_ddlCurrentConfig.SelectedItem is ProxyCmdConfigItem selectedConfig)
        {
            previousSelectedName = selectedConfig.Name;
        }

        _ddlCurrentConfig.ItemsSource = configItems;
        if (configItems.Count > 0) _ddlCurrentConfig.SelectedIndex = 0;
        if (!string.IsNullOrEmpty(previousSelectedName))
        {
            foreach (ProxyCmdConfigItem configItem in configItems)
            {
                if (configItem.Name == previousSelectedName)
                {
                    _ddlCurrentConfig.SelectedItem = configItem;
                    break;
                }
            }
        }
    }

    private string GetCurrentConfig()
    {
        ProxyCmdConfigItem? selectedConfig = _ddlCurrentConfig.SelectedItem as ProxyCmdConfigItem;
        return selectedConfig?.Config ?? string.Empty;
    }

}