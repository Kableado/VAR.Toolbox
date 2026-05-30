using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using VAR.Json;
using Avalonia.Threading;
using VAR.Toolbox.Code;
using VAR.Toolbox.Code.WorkLog;

namespace VAR.Toolbox.UI.Tools.WorkLog;

public class FrmWorkLog : Window, IToolForm
{
    #region IToolForm

    public string ToolName => "WorkLog";
    public bool HasIcon => false;

    #endregion IToolForm

    #region Declarations
        
    private readonly TextBox _txtName;
    private readonly Button _btnSave;
    private readonly ListBox _lsbWorkLog;
    private readonly CalendarDatePicker _dtToday;
    private readonly CalendarDatePicker _dtStartDate;
    private readonly TimePicker _tpStart;
    private readonly CalendarDatePicker _dtEndDate;
    private readonly TimePicker _tpEnd;
    private readonly TextBox _txtActivity;
    private readonly TextBox _txtDescription;
    private readonly TextBox _txtTags;
    private readonly TextBlock _lblWorkLogTime;
    private readonly TextBlock _lblWorkLogItemTime;
    private readonly Button _btnAdd;
    private readonly Button _btnDelete;
    private readonly Button _btnRename;
    private readonly ComboBox _cboImporters;
    private readonly CheckBox _chkImportMerging;

    #endregion Declarations
        
    #region Life cycle
        
    public FrmWorkLog()
    {
        Title = "WorkLog";
        Width = 750;
        Height = 600;

        // === Panel1 (LEFT) controls ===

        _txtName = new TextBox { PlaceholderText = "Name", Width = 104, };
        _txtName.TextChanged += txtName_TextChanged;

        Button btnLoad = new() { Content = "Load", };
        btnLoad.Click += btnLoad_Click;

        _btnSave = new Button { Content = "Save", IsEnabled = false, };
        _btnSave.Click += btnSave_Click;

        _cboImporters = new ComboBox
        {
            Width = 89,
            ItemsSource = WorkLogImporterFactory.GetNames().ToList(),
        };

        Button btnImport = new() { Content = "Imp", };
        btnImport.Click += btnImport_Click;

        Button btnExport = new() { Content = "Exp", };
        btnExport.Click += btnExport_Click;

        _chkImportMerging = new CheckBox();

        Button btnSummary = new() { Content = "Summary", };
        btnSummary.Click += btnSummary_Click;

        _lblWorkLogTime = new TextBlock { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Right, };

        // Toolbar row
        StackPanel toolbar = new() { Orientation = Orientation.Horizontal, Spacing = 4, };
        toolbar.Children.Add(_txtName);
        toolbar.Children.Add(btnLoad);
        toolbar.Children.Add(_btnSave);
        toolbar.Children.Add(_cboImporters);
        toolbar.Children.Add(btnImport);
        toolbar.Children.Add(_chkImportMerging);
        toolbar.Children.Add(btnExport);

        // Day navigation row
        _dtToday = new CalendarDatePicker { SelectedDate = DateTime.Now.Date, };
        _dtToday.SelectedDateChanged += dtToday_ValueChanged;

        Button btnPrevDay = new() { Content = "<-", };
        btnPrevDay.Click += btnPreviousDay_Click;
        Button btnNextDay = new() { Content = "->", };
        btnNextDay.Click += btnNextDay_Click;

        StackPanel dayNav = new() { Orientation = Orientation.Horizontal, Spacing = 4, };
        dayNav.Children.Add(btnPrevDay);
        dayNav.Children.Add(_dtToday);
        dayNav.Children.Add(btnNextDay);
        dayNav.Children.Add(btnSummary);
        dayNav.Children.Add(_lblWorkLogTime);

        _lsbWorkLog = new ListBox
        {
            Classes = { "mono", },
            SelectionMode = SelectionMode.Multiple,
            FontSize = 11,
        };
        _lsbWorkLog.SelectionChanged += lsbWorkLog_SelectionChanged;
        // Use AddHandler with handledEventsToo = true so we receive pointer events
        // even when a ListBoxItem captures or marks them handled during interaction.
        _lsbWorkLog.AddHandler(InputElement.PointerPressedEvent, lsbWorkLog_PointerPressed, handledEventsToo: true);
        _lsbWorkLog.AddHandler(InputElement.PointerMovedEvent, lsbWorkLog_PointerMoved, handledEventsToo: true);
        _lsbWorkLog.AddHandler(InputElement.PointerReleasedEvent, lsbWorkLog_PointerReleased, handledEventsToo: true);

        // === Panel2 (RIGHT) controls ===

        _btnAdd = new Button { Content = "Add", };
        _btnAdd.Click += btnAdd_Click;
        _btnDelete = new Button { Content = "Delete", IsEnabled = false, };
        _btnDelete.Click += btnDelete_Click;
        Button btnStats = new() { Content = "Stats", };
        btnStats.Click += btnStats_Click;

        Button btnDtStartMinus = new() { Content = "<-", };
        btnDtStartMinus.Click += btnDtStartMinus_Click;
        Button btnDtStartPlus = new() { Content = "->", };
        btnDtStartPlus.Click += btnDtStartPlus_Click;
        Button btnDtEndMinus = new() { Content = "<-", };
        btnDtEndMinus.Click += btnDtEndMinus_Click;
        Button btnDtEndPlus = new() { Content = "->", };
        btnDtEndPlus.Click += btnDtEndPlus_Click;

        _dtStartDate = new CalendarDatePicker { SelectedDate = DateTime.Now.Date, };
        _dtStartDate.SelectedDateChanged += WorkLogControls_Changed;
        _tpStart = new TimePicker { ClockIdentifier = "24HourClock", MinuteIncrement = 15, SelectedTime = TimeSpan.Zero, };
        _tpStart.SelectedTimeChanged += (sender, _) => WorkLogControls_Changed(sender, EventArgs.Empty);

        _dtEndDate = new CalendarDatePicker { SelectedDate = DateTime.Now.Date, };
        _dtEndDate.SelectedDateChanged += WorkLogControls_Changed;
        _tpEnd = new TimePicker { ClockIdentifier = "24HourClock", MinuteIncrement = 15, SelectedTime = TimeSpan.Zero, };
        _tpEnd.SelectedTimeChanged += (sender, _) => WorkLogControls_Changed(sender, EventArgs.Empty);

        _lblWorkLogItemTime = new TextBlock { VerticalAlignment = VerticalAlignment.Center, };

        _txtActivity = new TextBox { PlaceholderText = "Activity", };
        _txtActivity.TextChanged += WorkLogControls_Changed;

        _btnRename = new Button { Content = "Rename", IsEnabled = false, };
        _btnRename.Click += btnRename_Click;
        Button btnSearch = new() { Content = "Search", };
        btnSearch.Click += btnSearch_Click;

        _txtDescription = new TextBox { PlaceholderText = "Description", AcceptsReturn = true, };
        _txtDescription.TextChanged += WorkLogControls_Changed;

        _txtTags = new TextBox { PlaceholderText = "Tags", AcceptsReturn = true, Height = 55, };
        _txtTags.TextChanged += WorkLogControls_Changed;

        Button btnSearchTag = new() { Content = "Search", };
        btnSearchTag.Click += btnSearchTag_Click;

        // === Layout construction ===

        // Panel2 (RIGHT): Use DockPanel so Description fills remaining space
        DockPanel panel2 = new() { Margin = new Thickness(4, 0, 0, 0), };

        // Top row: Add, Delete ... Stats
        DockPanel topRow = new() { Margin = new Thickness(0, 0, 0, 4), };
        StackPanel topRowLeft = new() { Orientation = Orientation.Horizontal, Spacing = 4, };
        topRowLeft.Children.Add(_btnAdd);
        topRowLeft.Children.Add(_btnDelete);
        DockPanel.SetDock(btnStats, Dock.Right);
        topRow.Children.Add(btnStats);
        topRow.Children.Add(topRowLeft);
        DockPanel.SetDock(topRow, Dock.Top);
        panel2.Children.Add(topRow);

        // Start row: <- dtStartDate tpStart ->
        StackPanel startRow = new() { Orientation = Orientation.Horizontal, Spacing = 2, Margin = new Thickness(0, 0, 0, 4), };
        startRow.Children.Add(btnDtStartMinus);
        startRow.Children.Add(_dtStartDate);
        startRow.Children.Add(_tpStart);
        startRow.Children.Add(btnDtStartPlus);
        DockPanel.SetDock(startRow, Dock.Top);
        panel2.Children.Add(startRow);

        // End row: <- dtEndDate tpEnd -> lblTime
        StackPanel endRow = new() { Orientation = Orientation.Horizontal, Spacing = 2, Margin = new Thickness(0, 0, 0, 4), };
        endRow.Children.Add(btnDtEndMinus);
        endRow.Children.Add(_dtEndDate);
        endRow.Children.Add(_tpEnd);
        endRow.Children.Add(btnDtEndPlus);
        endRow.Children.Add(_lblWorkLogItemTime);
        DockPanel.SetDock(endRow, Dock.Top);
        panel2.Children.Add(endRow);

        // Activity textbox (full width)
        DockPanel.SetDock(_txtActivity, Dock.Top);
        _txtActivity.Margin = new Thickness(0, 0, 0, 4);
        panel2.Children.Add(_txtActivity);

        // Rename + Search row
        StackPanel renameRow = new() { Orientation = Orientation.Horizontal, Spacing = 4, Margin = new Thickness(0, 0, 0, 4), };
        renameRow.Children.Add(_btnRename);
        renameRow.Children.Add(btnSearch);
        DockPanel.SetDock(renameRow, Dock.Top);
        panel2.Children.Add(renameRow);

        // Bottom: Tags + SearchTag button
        StackPanel bottomPanel = new() { Spacing = 4, };
        bottomPanel.Children.Add(_txtTags);
        bottomPanel.Children.Add(btnSearchTag);
        DockPanel.SetDock(bottomPanel, Dock.Bottom);
        panel2.Children.Add(bottomPanel);

        // Fill: Description (takes remaining space)
        panel2.Children.Add(_txtDescription);

        // Panel1 (LEFT): toolbar, dayNav, then ListBox filling remaining space
        DockPanel panel1 = new();

        DockPanel.SetDock(toolbar, Dock.Top);
        panel1.Children.Add(toolbar);

        Border dayNavBorder = new() { Child = dayNav, Margin = new Thickness(0, 4), };
        DockPanel.SetDock(dayNavBorder, Dock.Top);
        panel1.Children.Add(dayNavBorder);

        // ListBox fills remaining space
        panel1.Children.Add(_lsbWorkLog);

        // Main layout: Grid with two columns (split) and a GridSplitter
        Grid mainLayout = new() { Margin = new Thickness(4), };
        mainLayout.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(3, GridUnitType.Star)));
        mainLayout.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(4, GridUnitType.Pixel)));
        mainLayout.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(2, GridUnitType.Star)));

        Grid.SetColumn(panel1, 0);
        mainLayout.Children.Add(panel1);

        GridSplitter splitter = new()
        {
            Width = 4,
            Background = Brushes.Gray,
            HorizontalAlignment = HorizontalAlignment.Stretch,
        };
        Grid.SetColumn(splitter, 1);
        mainLayout.Children.Add(splitter);

        Grid.SetColumn(panel2, 2);
        mainLayout.Children.Add(panel2);

        Content = mainLayout;

        WorkLog_LoadConfig();
        WorkLog_LoadData();

        Closing += FrmWorkLog_FormClosing;
    }

    private bool _closeConfirmed;
        
    private async void FrmWorkLog_FormClosing(object? sender, WindowClosingEventArgs e)
    {
        try
        {
            if (_closeConfirmed) { return; }
            e.Cancel = true;
                
            WorkLog_SaveConfig();

            if (_btnSave.IsEnabled)
            {
                _closeConfirmed = await Utils.MsgConfirm(this, "Close?", "There are unsaved changes. Close anyway?");
            }
            else
            {
                _closeConfirmed = true;
            }
            if (_closeConfirmed)
            {
                Close();
            }
        }
        catch (Exception ex)
        {
            Logger.Log(ex);
        }
    }

    #endregion Life cycle

    #region Events

    private void txtName_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is Control control && control.IsFocused == false) { return; }

        WorkLog_MarkDirty();
    }
        
    private void WorkLogControls_Changed(object? sender, EventArgs _)
    {
        if (sender is Control control && control.IsFocused == false) { return; }

        WorkLogItem_Update();
    }
        
    private void btnLoad_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) { WorkLog_LoadData(); }
        
    private void btnSave_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e) { WorkLog_SaveData(); }

    private async void btnImport_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        try
        {
            if (_cboImporters.SelectedItem == null) return;
            IWorkLogImporter workLogImporter =
                WorkLogImporterFactory.CreateFromName((string)_cboImporters.SelectedItem);

            List<WorkLogItem> newWorkLog =
                await System.Threading.Tasks.Task.Run(() => workLogImporter.Import(this));

            // Update UI on the UI thread
            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                _workLog = _chkImportMerging.IsChecked == true
                    ? WorkLogItemList_Merge(_workLog, newWorkLog)
                    : newWorkLog;
                WorkLog_Refresh();
                WorkLog_MarkDirty();
            });
            await Utils.MsgBox(this, "Importation", "OK");
        }
        catch (Exception ex)
        {
            Logger.Log(ex);
            await Utils.MsgBox(this, "Exportation", "Error");
        }
    }

    private async void btnExport_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        try
        {
            if (_cboImporters.SelectedItem == null) return;
            IWorkLogImporter workLogImporter =
                WorkLogImporterFactory.CreateFromName((string)_cboImporters.SelectedItem);

            bool result = await System.Threading.Tasks.Task.Run(() => workLogImporter.Export(_workLog, this));

            if (result)
            {
                await Utils.MsgBox(this, "Exportation", "OK");
            }
        }
        catch (Exception ex)
        {
            Logger.Log(ex);
            await Utils.MsgBox(this, "Exportation", "Error");
        }
    }

    private bool _selecting;
        
    private void lsbWorkLog_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_selecting) return;
        _selecting = true;

        List<WorkLogRow>? selectedRows = _lsbWorkLog.SelectedItems?.Cast<WorkLogRow>().ToList();
        if (selectedRows == null || selectedRows.Count == 0)
        {
            _lsbWorkLog.SelectedItems?.Clear();
            _selecting = false;
            return;
        }
            
        DateTime dateStart = DateTime.MaxValue;
        DateTime dateEnd = DateTime.MinValue;
        foreach (WorkLogRow rowAux in selectedRows)
        {
            if (rowAux.DateStart < dateStart) { dateStart = rowAux.DateStart; }

            if (rowAux.DateEnd > dateEnd) { dateEnd = rowAux.DateEnd; }
        }
            
        WorkLogRow selectedRow = selectedRows[0];
            
        if (selectedRow.Item == null)
        {
            WorkLogItem_Show(null);

            SetStartDateTime(dateStart);
            SetEndDateTime(dateEnd);

            _selecting = false;
            return;
        }

        _lsbWorkLog.SelectedItems?.Clear();
        IEnumerable<WorkLogRow> allRows = _lsbWorkLog.Items.Cast<WorkLogRow>();
        foreach (WorkLogRow row in allRows.Where(row => row.Item == selectedRow.Item))
        {
            _lsbWorkLog.SelectedItems?.Add(row);
        }
            
        WorkLogItem_Show(selectedRow.Item);
        _selecting = false;
    }

    #region Drag selection

    private bool _isDragging;
    private int _dragStartIndex = -1;

    private void lsbWorkLog_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        PointerPoint point = e.GetCurrentPoint(_lsbWorkLog);
        if (!point.Properties.IsLeftButtonPressed) { return; }
            
        int index = GetItemIndexAtPoint(e.GetPosition(_lsbWorkLog));
        if (index < 0) { return; }
            
        _isDragging = true;
        _dragStartIndex = index;
            
        _selecting = true;
        _lsbWorkLog.SelectedItems?.Clear();
        _selecting = false;
        object? item = _lsbWorkLog.Items.Cast<object>().ElementAt(index);
        _lsbWorkLog.SelectedItems?.Add(item);
    }

    private void lsbWorkLog_PointerMoved(object? sender, PointerEventArgs e)
    {
        if (!_isDragging) { return; }

        int index = GetItemIndexAtPoint(e.GetPosition(_lsbWorkLog));
        if (index < 0) { return; }

        int startIdx = Math.Min(_dragStartIndex, index);
        int endIdx = Math.Max(_dragStartIndex, index);

        _selecting = true;
        _lsbWorkLog.SelectedItems?.Clear();
        _selecting = false;
        for (int i = startIdx; i <= endIdx; i++)
        {
            if (i < _lsbWorkLog.ItemCount)
            {
                object? item = _lsbWorkLog.Items.Cast<object>().ElementAt(i);
                _lsbWorkLog.SelectedItems?.Add(item);
            }
        }
    }

    private void lsbWorkLog_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _isDragging = false;
    }

    private int GetItemIndexAtPoint(Point point)
    {
        // Calculate index based on item position within the ListBox
        ScrollViewer? scrollViewer = _lsbWorkLog.Scroll as ScrollViewer;
        double offsetY = scrollViewer?.Offset.Y ?? 0;
        double y = point.Y + offsetY;

        // Estimate item height from actual rendered items
        if (_lsbWorkLog.ItemCount == 0) return -1;
        Control? container = _lsbWorkLog.ContainerFromIndex(0);
        if (container == null) return -1;
        double itemHeight = container.Bounds.Height;
        if (itemHeight <= 0) itemHeight = 16;

        int index = (int)(y / itemHeight);
        if (index < 0) index = 0;
        if (index >= _lsbWorkLog.ItemCount) index = _lsbWorkLog.ItemCount - 1;
        return index;
    }

    #endregion Drag selection

    private void btnAdd_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        WorkLogItem item = new()
        {
            DateStart = GetStartDateTime(),
            DateEnd = GetEndDateTime(),
            Activity = _txtActivity.Text ?? string.Empty,
            Description = _txtDescription.Text ?? string.Empty,
            Tags = _txtTags.Text ?? string.Empty,
        };
        _workLog.Add(item);
        WorkLog_Refresh();
        WorkLogItem_Show(item);
        WorkLog_MarkDirty();
    }

    private void btnDelete_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_currentWorkLogItem == null) return;
        _workLog.Remove(_currentWorkLogItem);
        WorkLog_Refresh();
        WorkLogItem_Show(null);
        WorkLog_MarkDirty();
    }

    private void dtToday_ValueChanged(object? sender, SelectionChangedEventArgs e) { WorkLog_Refresh(); }

    private void btnPreviousDay_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _dtToday.SelectedDate = (_dtToday.SelectedDate ?? DateTime.Now).Date.AddDays(-1);
    }

    private void btnNextDay_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _dtToday.SelectedDate = (_dtToday.SelectedDate ?? DateTime.Now).Date.AddDays(1);
    }

    private void btnDtStartMinus_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        SetStartDateTime(GetStartDateTime().AddMinutes(-15));
        WorkLogItem_Update();
    }
    private void btnDtStartPlus_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        SetStartDateTime(GetStartDateTime().AddMinutes(15));
        WorkLogItem_Update();
    }
    private void btnDtEndMinus_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        SetEndDateTime(GetEndDateTime().AddMinutes(-15));
        WorkLogItem_Update();
    }
    private void btnDtEndPlus_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        SetEndDateTime(GetEndDateTime().AddMinutes(15));
        WorkLogItem_Update();
    }

    private async void btnRename_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        try
        {
            if (_currentWorkLogItem == null) return;
            FrmDialogString frmRename = new()
            {
                Title = "Rename",
                Description = $"\"{_currentWorkLogItem.Activity}\"",
                Value = _currentWorkLogItem.Activity,
            };
            await frmRename.ShowDialog(this);
            if (!frmRename.DialogOk) return;

            string activity = _currentWorkLogItem.Activity;
            string newActivity = frmRename.Value;
            foreach (WorkLogItem item in _workLog)
            {
                if (item.Activity == activity) item.Activity = newActivity;
            }
            WorkLog_MarkDirty();
            WorkLog_Refresh();
        }
        catch (Exception ex)
        {
            Logger.Log(ex);
        }
    }

    private void btnStats_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        FrmWorkLogStats frmStats = new()
        {
            Activity = _currentWorkLogItem?.Activity ?? string.Empty,
            WorkLog = _workLog,
            WorkerName = _txtName.Text ?? string.Empty,
        };
        frmStats.Show(this);
    }

    private void btnSummary_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        FrmWorkLogSummary frmSummary = new() { WorkLog = _workLog, };
        frmSummary.Show(this);
    }

    private async void btnSearch_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        try
        {
            List<string> listActivities = _workLog
                .Where(x => !string.IsNullOrEmpty(x.Activity))
                .GroupBy(x => x.Activity)
                .Select(g => g.OrderBy(x => x.DateStart).LastOrDefault())
                .Where(x => x != null)
                .Select(x => x!)
                .OrderByDescending(x => x.DateStart)
                .Select(x => x.Activity)
                .ToList();

            FrmListBoxDialog frmListDialog = new()
            {
                Title = "Search Activity",
            };
            frmListDialog.LoadItems(listActivities);
            await frmListDialog.ShowDialog(this);
            if (!frmListDialog.DialogOk || string.IsNullOrEmpty(frmListDialog.Value)) return;
            _txtActivity.Text = frmListDialog.Value;
        }
        catch (Exception ex)
        {
            Logger.Log(ex);
        }
    }

    private async void btnSearchTag_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        try
        {
            List<string> listTags = _workLog
                .Where(x => !string.IsNullOrEmpty(x.Tags))
                .GroupBy(x => x.Tags)
                .Select(g => g.OrderBy(x => x.DateStart).LastOrDefault())
                .Where(x => x != null)
                .Select(x => x!)
                .OrderByDescending(x => x.DateStart)
                .Select(x => x.Tags)
                .ToList();

            FrmListBoxDialog frmListDialog = new();
            frmListDialog.Title = "Search Tags";
            frmListDialog.LoadItems(listTags);
            await frmListDialog.ShowDialog(this);
            if (!frmListDialog.DialogOk || string.IsNullOrEmpty(frmListDialog.Value)) return;
            _txtTags.Text = frmListDialog.Value;
        }
        catch (Exception ex)
        {
            Logger.Log(ex);
        }
    }

    #endregion Events
        
    #region Private methods

    private const string ConfigFile = "WorkLog.Config.json";
    // Always keep a non-null config instance to avoid null dereference when saving
    private WorkLogConfig _config = new();

    private void WorkLog_LoadConfig()
    {
        if (File.Exists(ConfigFile))
        {
            JsonParser jsonParser = new();
            jsonParser.KnownTypes.Add(typeof(WorkLogConfig));
            string jsonConfig = File.ReadAllText(ConfigFile);
            if (jsonParser.Parse(jsonConfig) is WorkLogConfig parsed) _config = parsed;
        }
        _txtName.Text = _config.LastName;
    }

    private void WorkLog_SaveConfig()
    {
        _config.LastName = _txtName.Text ?? string.Empty;
        JsonWriter jsonWriter = new(new JsonWriterConfiguration(indent: true));
        using StreamWriter streamWriter = new(ConfigFile);
        jsonWriter.Write(_config, streamWriter);
    }

    // Maintain a non-null worklog collection to simplify callers
    private List<WorkLogItem> _workLog = [];

    private void WorkLog_LoadData()
    {
        _workLog.Clear();
        string fileName = $"{_txtName.Text ?? string.Empty}.WorkLog.json";
        if (File.Exists(fileName))
        {
            string rawFile = File.ReadAllText(fileName);
            JsonParser jsonParser = new();
            jsonParser.KnownTypes.Add(typeof(WorkLogItem));
            object? result = jsonParser.Parse(rawFile);
            if (result is IEnumerable<object> results)
            {
                foreach (object obj in results)
                {
                    if (obj is WorkLogItem item) _workLog.Add(item);
                }
            }
        }
        WorkLog_Refresh();
        WorkLog_CleanDirty();
    }

    private void WorkLog_SaveData()
    {
        string fileName = $"{_txtName.Text}.WorkLog.json";
        if (File.Exists(fileName)) File.Delete(fileName);
        JsonWriter jsonWriter = new(new JsonWriterConfiguration(indent: true));
        using (StreamWriter streamWriter = new(fileName))
        {
            jsonWriter.Write(_workLog, streamWriter);
        }
        WorkLog_CleanDirty();
    }

    private void WorkLog_MarkDirty() { _btnSave.IsEnabled = true; }
    private void WorkLog_CleanDirty() { _btnSave.IsEnabled = false; }

    private void WorkLog_Refresh()
    {
        DateTime today = _dtToday.SelectedDate ?? DateTime.Now;
        lsbWorkLog_BindData(_workLog, today.Year, today.Month, today.Day);
    }

    private WorkLogItem? _currentWorkLogItem;

    private void WorkLogItem_Show(WorkLogItem? item)
    {
        _currentWorkLogItem = null;
        if (item == null)
        {
            _txtActivity.Text = string.Empty;
            _txtDescription.Text = string.Empty;
            _txtTags.Text = string.Empty;
            _lblWorkLogItemTime.Text = string.Empty;
            WorkLogItem_EnableButtons(false);
            return;
        }

        SetStartDateTime(item.DateStart);
        SetEndDateTime(item.DateEnd);
        _txtActivity.Text = item.Activity;
        _txtDescription.Text = item.Description;
        _txtTags.Text = item.Tags;
        _lblWorkLogItemTime.Text = (item.DateEnd - item.DateStart).ToString();
        WorkLogItem_EnableButtons(true);
        _currentWorkLogItem = item;
    }

    private void WorkLogItem_EnableButtons(bool enable)
    {
        _btnAdd.IsEnabled = !enable;
        _btnDelete.IsEnabled = enable;
        _btnRename.IsEnabled = enable;
    }

    private void WorkLogItem_Update()
    {
        if (_currentWorkLogItem == null) return;
        _currentWorkLogItem.DateStart = GetStartDateTime();
        _currentWorkLogItem.DateEnd = GetEndDateTime();
        _currentWorkLogItem.Activity = _txtActivity.Text ?? string.Empty;
        _currentWorkLogItem.Description = _txtDescription.Text ?? string.Empty;
        _currentWorkLogItem.Tags = _txtTags.Text ?? string.Empty;
        WorkLog_Refresh();
        WorkLog_MarkDirty();
    }

    private void lsbWorkLog_BindData(IEnumerable<WorkLogItem> items, int year, int month, int day, int q = 15)
    {
        List<WorkLogRow> rows = [];
        IEnumerable<WorkLogItem> workLogItems = items.ToList();
        for (int h = 0; h < 24; h++)
        {
            for (int m = 0; m < 60; m += q)
            {
                DateTime dateStart = new(year, month, day, h, m, 0);
                DateTime dateEnd = dateStart.AddMinutes(q);
                WorkLogRow row = new() { DateStart = dateStart, DateEnd = dateEnd, };
                foreach (WorkLogItem item in workLogItems) row.SetItem(item);
                rows.Add(row);
            }
        }
        _lsbWorkLog.ItemsSource = rows;

        DateTime dateDay = new(year, month, day, 0, 0, 0);
        TimeSpan tsTotalTime = new(0);
        foreach (WorkLogItem item in workLogItems)
        {
            if (item.DateStart.Date != dateDay) continue;
            tsTotalTime += (item.DateEnd - item.DateStart);
        }
        _lblWorkLogTime.Text = tsTotalTime.ToString();
    }

    private static List<WorkLogItem> WorkLogItemList_Merge(List<WorkLogItem> workLogA, List<WorkLogItem> workLogB)
    {
        List<WorkLogItem> newWorkLog = [];
        foreach (WorkLogItem itemA in workLogA)
        {
            bool skip = false;
            foreach (WorkLogItem itemB in workLogB)
            {
                if (itemB.Overlaps(itemA))
                {
                    skip = true;
                    if (itemA.DateStart == itemB.DateStart && itemA.DateEnd == itemB.DateEnd)
                    {
                        if (string.IsNullOrEmpty(itemB.Activity)) itemB.Activity = itemA.Activity;
                        if (string.IsNullOrEmpty(itemB.Description)) itemB.Description = itemA.Description;
                        if (string.IsNullOrEmpty(itemB.Tags)) itemB.Tags = itemA.Tags;
                        break;
                    }
                }
            }
            if (skip) continue;
            newWorkLog.Add(itemA);
        }
        foreach (WorkLogItem itemB in workLogB) newWorkLog.Add(itemB);
        return newWorkLog;
    }

    #region DateTime helpers

    private DateTime GetStartDateTime()
    {
        DateTime date = (_dtStartDate.SelectedDate ?? DateTime.Now).Date;
        TimeSpan time = _tpStart.SelectedTime ?? TimeSpan.Zero;
        return date + time;
    }

    private void SetStartDateTime(DateTime dt)
    {
        _dtStartDate.SelectedDate = dt.Date;
        _tpStart.SelectedTime = dt.TimeOfDay;
    }

    private DateTime GetEndDateTime()
    {
        DateTime date = (_dtEndDate.SelectedDate ?? DateTime.Now).Date;
        TimeSpan time = _tpEnd.SelectedTime ?? TimeSpan.Zero;
        return date + time;
    }

    private void SetEndDateTime(DateTime dt)
    {
        _dtEndDate.SelectedDate = dt.Date;
        _tpEnd.SelectedTime = dt.TimeOfDay;
    }

    #endregion DateTime helpers
        
    #endregion Private methods
}

public class WorkLogRow
{
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public WorkLogItem? Item { get; private set; }

    public override string ToString()
    {
        StringBuilder sbRow = new();
        sbRow.Append($"{DateStart.Hour:00}:{DateStart.Minute:00} ");
        if (Item == null) return sbRow.ToString();

        int rowLength = 30;
        int textLength = Item.Activity.Length + 2;
        if (textLength > rowLength) rowLength = textLength;

        if (Item.DateStart >= DateStart && Item.DateStart < DateEnd)
        {
            if (Item.DateEnd >= DateStart && Item.DateEnd <= DateEnd)
            {
                sbRow.Append("─ ").Append(Item.Activity).Append(' ').Append(new string('─', (rowLength - textLength) + 1));
            }
            else
            {
                sbRow.Append("┌ ").Append(Item.Activity).Append(' ').Append(new string('─', rowLength - textLength)).Append('┐');
            }
        }
        else if (Item.DateEnd >= DateStart && Item.DateEnd <= DateEnd)
        {
            sbRow.Append('└').Append(new string('─', rowLength)).Append('┘');
        }
        else
        {
            sbRow.Append('│').Append(new string(' ', rowLength)).Append('│');
        }
        return sbRow.ToString();
    }

    public void SetItem(WorkLogItem? item)
    {
        if (item == null) { Item = null; return; }
        if (item.DateStart >= DateEnd || item.DateEnd <= DateStart) return;
        Item = item;
    }
}

public class WorkLogConfig
{
    public string LastName { get; set; } = string.Empty;
}