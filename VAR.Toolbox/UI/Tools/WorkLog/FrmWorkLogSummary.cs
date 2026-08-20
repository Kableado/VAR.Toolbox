using System;
using System.Collections.Generic;
using System.Linq;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using VAR.Toolbox.Code.WorkLog;

namespace VAR.Toolbox.UI.Tools.WorkLog;

public class FrmWorkLogSummary : Window
{
    private readonly CalendarDatePicker _dtpStart;
    private readonly CalendarDatePicker _dtpEnd;
    private readonly TextBlock _lblDateStart;
    private readonly TextBlock _lblDateEnd;
    private readonly TextBlock _lblTotalTime;
    private readonly ListBox _lsbActivities;
    private readonly TextBox _txtActivity;
    private readonly CheckBox _chkOnlyGroups;

    public FrmWorkLogSummary()
    {
        Title = "WorkLogSummary";
        Width = 550;
        Height = 450;
            
        StackPanel searchRow = new() { Orientation = Orientation.Horizontal, Spacing = 4, HorizontalAlignment =  HorizontalAlignment.Right, };
        Grid.SetRow(searchRow, 0);
        Button btnSearch = new() { Content = "Search", };
        btnSearch.Click += (_, _) => { WorkLog_ProcessStats(); };
        searchRow.Children.Add(btnSearch);
            
        StackPanel topRow = new() { Orientation = Orientation.Horizontal, Spacing = 4, };
        Grid.SetRow(topRow, 1);
        _dtpStart = new CalendarDatePicker { SelectedDate = DateTime.Now.Date.AddMonths(-1), };
        topRow.Children.Add(_dtpStart);
        _dtpEnd = new CalendarDatePicker { SelectedDate = DateTime.Now.Date.AddMonths(1).AddDays(1).AddSeconds(-1), };
        topRow.Children.Add(_dtpEnd);
        _chkOnlyGroups = new CheckBox { Content = "Only Groups", };
        topRow.Children.Add(_chkOnlyGroups);

        StackPanel infoRow = new() { Orientation = Orientation.Horizontal, Spacing = 10, };
        Grid.SetRow(infoRow, 2);
        _lblDateStart = new TextBlock();
        infoRow.Children.Add(_lblDateStart);
        _lblDateEnd = new TextBlock();
        infoRow.Children.Add(_lblDateEnd);
        _lblTotalTime = new TextBlock();
        infoRow.Children.Add(_lblTotalTime);

        _lsbActivities = new ListBox { Classes = { "mono", }, };
        Grid.SetRow(_lsbActivities, 3);
        _lsbActivities.SelectionChanged += lsbActivities_SelectionChanged;

        Grid bottomGrid = new()
        {
            ColumnDefinitions = new ColumnDefinitions("*, Auto, Auto"),
        };
        Grid.SetRow(bottomGrid, 4);
        _txtActivity = new TextBox();
        Grid.SetColumn(_txtActivity, 0);
        bottomGrid.Children.Add(_txtActivity);
        Button btnStats = new() { Content = "Stats", };
        btnStats.Click += btnStats_Click;
        Grid.SetColumn(btnStats, 1);
        bottomGrid.Children.Add(btnStats);
        Button btnClose = new() { Content = "Close", };
        btnClose.Click += (_, _) => { Close(); };
        Grid.SetColumn(btnClose, 2);
        bottomGrid.Children.Add(btnClose);

        Grid mainGrid = new()
        {
            Margin = new Thickness(8),
            RowDefinitions = new RowDefinitions("Auto, Auto, Auto, *, Auto"),
        };
        mainGrid.Children.Add(searchRow);
        mainGrid.Children.Add(topRow);
        mainGrid.Children.Add(infoRow);
        mainGrid.Children.Add(_lsbActivities);
        mainGrid.Children.Add(bottomGrid);

        Content = mainGrid;

        Opened += (_, _) => { WorkLog_ProcessStats(); };
    }

    private List<WorkLogItem>? _workLog;
    public List<WorkLogItem>? WorkLog { get => _workLog; set => _workLog = value; }

    private void WorkLog_ProcessStats()
    {
        if (_workLog == null) return;

        bool found = false;
        DateTime dateStart = DateTime.MaxValue;
        DateTime dateEnd = DateTime.MinValue;
        Dictionary<string, TimeSpan> dictActivityHours = new();

        DateTime filterStart = _dtpStart.SelectedDate ?? DateTime.MinValue;
        DateTime filterEnd = _dtpEnd.SelectedDate ?? DateTime.MaxValue;

        foreach (WorkLogItem item in _workLog)
        {
            if (item.DateEnd < filterStart || item.DateStart > filterEnd) continue;
            found = true;
            if (item.DateStart < dateStart) dateStart = item.DateStart;
            if (item.DateEnd > dateEnd) dateEnd = item.DateEnd;
            TimeSpan tsItem = item.DateEnd - item.DateStart;
            if (!dictActivityHours.TryAdd(item.Activity, tsItem)) { dictActivityHours[item.Activity] += tsItem; }
        }

        if (!found)
        {
            _lblDateStart.Text = string.Empty;
            _lblDateEnd.Text = string.Empty;
            _lsbActivities.ItemsSource = null;
            _lblTotalTime.Text = string.Empty;
            return;
        }

        _lblDateStart.Text = dateStart.ToString("yyyy-MM-dd HH:mm:ss");
        _lblDateEnd.Text = dateEnd.ToString("yyyy-MM-dd HH:mm:ss");

        List<string> strActivities = [];
        TimeSpan tsTotal = new(0);
        IOrderedEnumerable<IGrouping<string?, KeyValuePair<string, TimeSpan>>> activityGroups = dictActivityHours
            .GroupBy(p => p.Key.Split(' ').FirstOrDefault(), p => p)
            .OrderBy(x => x.Key);

        foreach (IGrouping<string?, KeyValuePair<string, TimeSpan>> activityGroup in activityGroups)
        {
            TimeSpan tsActivityGroup = new(0);
            foreach (KeyValuePair<string, TimeSpan> pair in activityGroup) tsActivityGroup += pair.Value;

            strActivities.Add($"{activityGroup.Key} -- {tsActivityGroup.TotalHours} h");
            if (_chkOnlyGroups.IsChecked != true)
            {
                foreach (KeyValuePair<string, TimeSpan> pair in activityGroup)
                {
                    strActivities.Add($"    {pair.Key} -- {pair.Value.TotalHours} h");
                    tsTotal += pair.Value;
                }
            }
            else
            {
                tsTotal += tsActivityGroup;
            }
        }

        _lsbActivities.ItemsSource = strActivities;
        _lblTotalTime.Text = $"{tsTotal} - {tsTotal.TotalHours}";
    }

    private void lsbActivities_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        _txtActivity.Text = (_lsbActivities.SelectedItem as string) ?? string.Empty;
    }

    private void btnStats_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        FrmWorkLogStats frmStats = new() { Activity = _txtActivity.Text ?? string.Empty, WorkLog = _workLog, };
        frmStats.Show(this);
    }
}