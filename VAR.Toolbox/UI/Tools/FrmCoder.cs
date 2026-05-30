using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using VAR.Toolbox.Code.TextCoders;

namespace VAR.Toolbox.UI.Tools;

public class FrmCoder : Window, IToolForm
{
    public string ToolName => "Coder";
    public bool HasIcon => false;

    private readonly TextBox _txtInput;
    private readonly TextBox _txtOutput;
    private readonly TextBox _txtKey;
    private readonly ComboBox _cboCode;

    public FrmCoder()
    {
        Title = "Coder";
        Width = 743;
        Height = 425;

        _txtInput = new TextBox { Classes = { "mono", }, AcceptsReturn = true, TextWrapping = Avalonia.Media.TextWrapping.Wrap, };
        _txtOutput = new TextBox { Classes = { "mono", }, AcceptsReturn = true, IsReadOnly = true, TextWrapping = Avalonia.Media.TextWrapping.Wrap, };
        _txtKey = new TextBox { Classes = { "mono", }, AcceptsReturn = true, Height = 60, };

        _cboCode = new ComboBox();
        List<string> codeNames = TextCoderFactory.GetNames().ToList();
        _cboCode.ItemsSource = codeNames;
        _cboCode.SelectedIndex = 1;
        _cboCode.SelectionChanged += CboCode_SelectionChanged;

        Button btnDecode = new() { Content = "Decode", };
        btnDecode.Click += BtnDecode_Click;

        Button btnEncode = new() { Content = "Encode", };
        btnEncode.Click += BtnEncode_Click;

        Button btnSwap = new() { Content = "Swap", };
        btnSwap.Click += BtnSwap_Click;

        // Left panel
        StackPanel leftButtons = new() { Orientation = Orientation.Horizontal, Spacing = 5, };
        leftButtons.Children.Add(_cboCode);
        leftButtons.Children.Add(btnDecode);
        leftButtons.Children.Add(btnEncode);

        DockPanel leftPanel = new();
        DockPanel.SetDock(leftButtons, Dock.Bottom);
        DockPanel.SetDock(_txtKey, Dock.Bottom);
        leftPanel.Children.Add(leftButtons);
        leftPanel.Children.Add(_txtKey);
        leftPanel.Children.Add(_txtInput);

        // Right panel
        StackPanel rightButtons = new() { Orientation = Orientation.Horizontal, Spacing = 5, };
        rightButtons.Children.Add(btnSwap);

        DockPanel rightPanel = new();
        DockPanel.SetDock(rightButtons, Dock.Bottom);
        rightPanel.Children.Add(rightButtons);
        rightPanel.Children.Add(_txtOutput);

        Grid grid = new();
        grid.ColumnDefinitions.Add(new ColumnDefinition(1, GridUnitType.Star));
        grid.ColumnDefinitions.Add(new ColumnDefinition(5, GridUnitType.Pixel));
        grid.ColumnDefinitions.Add(new ColumnDefinition(1, GridUnitType.Star));

        Grid.SetColumn(leftPanel, 0);
        Grid.SetColumn(rightPanel, 2);
        grid.Children.Add(leftPanel);
        grid.Children.Add(rightPanel);

        Content = new Border { Padding = new Thickness(8), Child = grid, };

        // Init coder
        if (_cboCode.SelectedItem is string code)
        {
            _coder = TextCoderFactory.CreateFromName(code);
            _txtKey.IsEnabled = _coder.NeedsKey;
        }
    }

    private ITextCoder _coder = null!;

    private void BtnDecode_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        string output;
        try
        {
            output = _coder.Decode(_txtInput.Text ?? string.Empty, _txtKey.Text ?? string.Empty);
        }
        catch (Exception ex)
        {
            output = ex.Message;
        }
        _txtOutput.Text = output;
    }

    private void BtnEncode_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        string output;
        try
        {
            output = _coder.Encode(_txtInput.Text ?? string.Empty, _txtKey.Text ?? string.Empty);
        }
        catch (Exception ex)
        {
            output = ex.Message;
        }
        _txtOutput.Text = output;
    }

    private void BtnSwap_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        (_txtOutput.Text, _txtInput.Text) = (_txtInput.Text, _txtOutput.Text);
    }

    private void CboCode_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_cboCode.SelectedItem is string code)
        {
            _coder = TextCoderFactory.CreateFromName(code);
            _txtKey.IsEnabled = _coder.NeedsKey;
        }
    }
}