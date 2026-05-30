using System;
using System.Collections.Generic;
using System.Globalization;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using VAR.Toolbox.Code.WorkLog;

namespace VAR.Toolbox.UI.Tools.WorkLog;

public class FrmWorkLogStats : Window
{
    private readonly TextBox _txtActivity;
    private readonly CalendarDatePicker _dtpStart;
    private readonly CalendarDatePicker _dtpEnd;
    private readonly TextBlock _lblName;
    private readonly TextBlock _lblDateStart;
    private readonly TextBlock _lblDateEnd;
    private readonly TextBlock _lblTotalTime;
    private readonly ListBox _lsbDays;

    public FrmWorkLogStats()
    {
        Title = "WorkLogStats";
        Width = 500;
        Height = 450;
            
        Grid activityGrid = new()
        {
            ColumnDefinitions = new ColumnDefinitions("*, Auto")
        };
        Grid.SetRow(activityGrid, 0);
        _txtActivity = new TextBox { PlaceholderText = "Activity", };
        Grid.SetColumn(_txtActivity, 0);
        activityGrid.Children.Add(_txtActivity);
        Button btnSearch = new() { Content = "Search", };
        Grid.SetColumn(btnSearch, 1);
        btnSearch.Click += (_, _) => { WorkLog_ProcessStats(); };
        activityGrid.Children.Add(btnSearch);
            
        StackPanel topRow = new() { Orientation = Orientation.Horizontal, Spacing = 4, };
        Grid.SetRow(topRow, 1);
        _dtpStart = new CalendarDatePicker { SelectedDate = DateTime.Now.Date.AddMonths(-1), };
        topRow.Children.Add(_dtpStart);
        _dtpEnd = new CalendarDatePicker { SelectedDate = DateTime.Now.Date.AddMonths(1).AddDays(1).AddSeconds(-1), };
        topRow.Children.Add(_dtpEnd);
            
        StackPanel infoRow = new() { Orientation = Orientation.Horizontal, Spacing = 10, };
        Grid.SetRow(infoRow, 2);
        _lblName = new TextBlock();
        infoRow.Children.Add(_lblName);
        _lblDateStart = new TextBlock();
        infoRow.Children.Add(_lblDateStart);
        _lblDateEnd = new TextBlock();
        infoRow.Children.Add(_lblDateEnd);
        _lblTotalTime = new TextBlock();
        infoRow.Children.Add(_lblTotalTime);

        _lsbDays = new ListBox { Classes = { "mono", }, };
        Grid.SetRow(_lsbDays, 3);
            
        StackPanel bottomRow = new() { Orientation = Orientation.Horizontal, Spacing = 4, HorizontalAlignment = HorizontalAlignment.Right, };
        Grid.SetRow(bottomRow, 4);
        Button btnClose = new() { Content = "Close", };
        btnClose.Click += (_, _) => { Close(); };
        bottomRow.Children.Add(btnClose);
            
        Grid mainGrid = new()
        {
            Margin = new Thickness(8),
            RowDefinitions = new RowDefinitions("Auto, Auto, Auto, *, Auto"),
        };
        mainGrid.Children.Add(activityGrid);
        mainGrid.Children.Add(topRow);
        mainGrid.Children.Add(infoRow);
        mainGrid.Children.Add(_lsbDays);
        mainGrid.Children.Add(bottomRow);

        Content = mainGrid;

        Opened += (_, _) => { WorkLog_ProcessStats(); };
    }

    public string? Activity { get => _txtActivity.Text; set => _txtActivity.Text = value; }
    private List<WorkLogItem>? _workLog;
    public List<WorkLogItem>? WorkLog { get => _workLog; set => _workLog = value; }
    public string? WorkerName { get => _lblName.Text; set => _lblName.Text = value; }

    private void WorkLog_ProcessStats()
    {
        if (string.IsNullOrWhiteSpace(_txtActivity.Text)) { CleanList(); return; }
        if (_workLog == null) { CleanList(); return; }

        bool found = false;
        DateTime dateStart = DateTime.MaxValue;
        DateTime dateEnd = DateTime.MinValue;
        Dictionary<DateTime, TimeSpan> dictDaysHours = new();

        DateTime filterStart = _dtpStart.SelectedDate ?? DateTime.MinValue;
        DateTime filterEnd = _dtpEnd.SelectedDate ?? DateTime.MaxValue;

        foreach (WorkLogItem item in _workLog)
        {
            if (!item.Activity.Contains(_txtActivity.Text)) continue;
            if (item.DateEnd < filterStart || item.DateStart > filterEnd) continue;
            found = true;
            if (item.DateStart < dateStart) dateStart = item.DateStart;
            if (item.DateEnd > dateEnd) dateEnd = item.DateEnd;
            DateTime dateItemDay = item.DateStart.Date;
            TimeSpan tsItem = item.DateEnd - item.DateStart;
            if (!dictDaysHours.TryAdd(dateItemDay, tsItem)) { dictDaysHours[dateItemDay] += tsItem; }
        }

        if (!found) { CleanList(); return; }

        _lblDateStart.Text = dateStart.ToString("yyyy-MM-dd HH:mm:ss");
        _lblDateEnd.Text = dateEnd.ToString("yyyy-MM-dd HH:mm:ss");

        List<string> strDays = [];
        DateTime dateDayCurrent = dateStart.Date;
        DateTime dateDayEnd = dateEnd.Date;
        TimeSpan tsTotal = new(0);
        int? week = null;
        TimeSpan tsWeek = new(0);
        CultureInfo currentCulture = CultureInfo.CurrentCulture;
        do
        {
            if (dictDaysHours.TryGetValue(dateDayCurrent, out TimeSpan tsDay))
            {
                int weekCurrent = currentCulture.Calendar.GetWeekOfYear(dateDayCurrent,
                    currentCulture.DateTimeFormat.CalendarWeekRule, currentCulture.DateTimeFormat.FirstDayOfWeek);
                if (week != null && week != weekCurrent)
                {
                    strDays.Add($"  [{week:00}] -- {tsWeek.TotalHours} h");
                    tsWeek = new TimeSpan(0);
                }

                strDays.Add($"[{weekCurrent:00}] {dateDayCurrent:yyyy-MM-dd} -- {tsDay.TotalHours} h");
                tsTotal += tsDay;
                tsWeek += tsDay;
                week = weekCurrent;
            }
            dateDayCurrent = dateDayCurrent.AddDays(1);
        } while (dateDayCurrent <= dateDayEnd);

        if (tsWeek.TotalHours > 0)
            strDays.Add($"  [{week:00}] -- {tsWeek.TotalHours} h");

        _lsbDays.ItemsSource = strDays;
        _lblTotalTime.Text = $"{tsTotal} - {tsTotal.TotalHours}";
    }

    private void CleanList()
    {
        _lblDateStart.Text = string.Empty;
        _lblDateEnd.Text = string.Empty;
        _lsbDays.ItemsSource = null;
        _lblTotalTime.Text = string.Empty;
    }
}