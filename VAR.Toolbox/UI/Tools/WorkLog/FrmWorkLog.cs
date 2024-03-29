﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VAR.Json;
using VAR.Toolbox.Code;
using VAR.Toolbox.Code.WorkLog;
using VAR.Toolbox.Controls;

namespace VAR.Toolbox.UI.Tools.WorkLog
{
    public partial class FrmWorkLog : Frame, IToolForm
    {
        #region IToolForm

        public string ToolName => "WorkLog";

        public bool HasIcon => false;

        #endregion IToolForm

        #region Form life cycle

        public FrmWorkLog()
        {
            InitializeComponent();
            WorkLog_LoadConfig();
            WorkLog_LoadData();

            cboImporters.Items.AddRange(WorkLogImporterFactory.GetNames().ToArray<object>());

            ttPanel.SetToolTip(chkImportMerging, "ImportMerging");
        }

        private void FrmWorkLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnSave.Enabled)
            {
                DialogResult result = MessageBox.Show("There are unsaved changes. Close anyway?", "Close?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            WorkLog_SaveConfig();
        }

        #endregion Form life cycle

        #region UI events

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            WorkLog_MarkDirty();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            WorkLog_LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            WorkLog_SaveData();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboImporters.SelectedItem == null) { return; }

                IWorkLogImporter workLogImporter =
                    WorkLogImporterFactory.CreateFromName(cboImporters.SelectedItem as string);
                List<WorkLogItem> newWorkLog = workLogImporter.Import(this);
                if (newWorkLog == null) { return; }

                _workLog = chkImportMerging.Checked ? WorkLogItemList_Merge(_workLog, newWorkLog) : newWorkLog;

                WorkLog_Refresh();
                MessageBox.Show("OK");
                WorkLog_MarkDirty();
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Error");
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboImporters.SelectedItem == null) { return; }

                IWorkLogImporter workLogImporter =
                    WorkLogImporterFactory.CreateFromName(cboImporters.SelectedItem as string);
                bool result = workLogImporter.Export(_workLog, this);
                if (result)
                {
                    MessageBox.Show("OK");
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                MessageBox.Show("Error");
            }
        }

        private bool _selecting;

        private void lsbWorkLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_selecting) { return; }

            _selecting = true;
            var row = lsbWorkLog.SelectedItem as WorkLogRow;
            if (row == null)
            {
                lsbWorkLog.ClearSelected();
                _selecting = false;
                return;
            }

            if (row.Item == null)
            {
                WorkLogItem_Show(null);
                DateTime dateStart = DateTime.MaxValue;
                DateTime dateEnd = DateTime.MinValue;
                foreach (WorkLogRow rowAux in lsbWorkLog.SelectedItems)
                {
                    if (rowAux.DateStart < dateStart) { dateStart = rowAux.DateStart; }

                    if (rowAux.DateEnd > dateEnd) { dateEnd = rowAux.DateEnd; }
                }

                dtStart.Value = dateStart;
                dtEnd.Value = dateEnd;

                _selecting = false;
                return;
            }

            for (int i = 0; i < lsbWorkLog.Items.Count; i++)
            {
                WorkLogRow auxRow = lsbWorkLog.Items[i] as WorkLogRow;
                if (auxRow == null) { continue; }

                lsbWorkLog.SetSelected(i, (row.Item == auxRow.Item));
            }

            WorkLogItem_Show(row.Item);
            _selecting = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            WorkLogItem item = new WorkLogItem
            {
                DateStart = dtStart.Value,
                DateEnd = dtEnd.Value,
                Activity = txtActivity.Text,
                Description = txtDescription.Text,
                Tags = txtTags.Text,
            };
            _workLog.Add(item);
            WorkLog_Refresh();
            WorkLog_SelectDate(item.DateStart);
            WorkLogItem_Show(item);
            WorkLog_MarkDirty();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_currentWorkLogItem == null) { return; }

            if (MessageBox.Show("Delete Log?", "Delete Log?", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            _workLog.Remove(_currentWorkLogItem);
            WorkLog_Refresh();
            WorkLogItem_Show(null);
            WorkLog_MarkDirty();
        }

        private void dtStart_ValueChanged(object sender, EventArgs e)
        {
            WorkLogItem_Update();
        }

        private void dtEnd_ValueChanged(object sender, EventArgs e)
        {
            WorkLogItem_Update();
        }

        private void txtActivity_TextChanged(object sender, EventArgs e)
        {
            WorkLogItem_Update();
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            WorkLogItem_Update(refresh: false);
        }

        private void txtTags_TextChanged(object sender, EventArgs e)
        {
            WorkLogItem_Update(refresh: false);
        }

        private void dtToday_ValueChanged(object sender, EventArgs e)
        {
            WorkLog_Refresh();
        }

        private void btnPreviousDay_Click(object sender, EventArgs e)
        {
            dtToday.Value = dtToday.Value.Date.AddDays(-1);
        }

        private void btnNextDay_Click(object sender, EventArgs e)
        {
            dtToday.Value = dtToday.Value.Date.AddDays(1);
        }

        private void btnDtStartMinus_Click(object sender, EventArgs e)
        {
            dtStart.Value = dtStart.Value.AddMinutes(-15);
        }

        private void btnDtStartPlus_Click(object sender, EventArgs e)
        {
            dtStart.Value = dtStart.Value.AddMinutes(15);
        }

        private void btnDtEndMinus_Click(object sender, EventArgs e)
        {
            dtEnd.Value = dtEnd.Value.AddMinutes(-15);
        }

        private void btnDtEndPlus_Click(object sender, EventArgs e)
        {
            dtEnd.Value = dtEnd.Value.AddMinutes(15);
        }

        private void lsbWorkLog_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(Brushes.DarkRed, e.Bounds);
                e.Graphics.DrawString(lsbWorkLog.Items[e.Index].ToString(), lsbWorkLog.Font, Brushes.Black, e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(Brushes.Black, e.Bounds);
                e.Graphics.DrawString(lsbWorkLog.Items[e.Index].ToString(), lsbWorkLog.Font, Brushes.Gray, e.Bounds);
            }
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            if (_currentWorkLogItem == null) { return; }

            FrmDialogString frmRename = new FrmDialogString
            {
                Title = "Rename", Description = string.Concat("\"", _currentWorkLogItem.Activity, "\""),
                Value = _currentWorkLogItem.Activity
            };
            DialogResult result = frmRename.ShowDialog(this);
            if (result != DialogResult.OK) { return; }

            string activity = _currentWorkLogItem.Activity;
            string newActivity = frmRename.Value;

            foreach (WorkLogItem item in _workLog)
            {
                if (item.Activity == activity)
                {
                    item.Activity = newActivity;
                }
            }

            WorkLog_MarkDirty();
            WorkLog_Refresh();
        }

        private void btnStats_Click(object sender, EventArgs e)
        {
            if (_currentWorkLogItem == null) { return; }

            FrmWorkLogStats frmStats = new FrmWorkLogStats
                { Activity = _currentWorkLogItem.Activity, WorkLog = _workLog };
            frmStats.Show(this);
        }

        private void btnSummary_Click(object sender, EventArgs e)
        {
            FrmWorkLogSummary frmStats = new FrmWorkLogSummary { WorkLog = _workLog };
            frmStats.Show(this);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (_workLog == null) { return; }

            List<string> listActivities = _workLog
                .Where(x => string.IsNullOrEmpty(x.Activity) == false)
                .GroupBy(x => x.Activity)
                .Select(g => g.OrderBy(x => x.DateStart).LastOrDefault())
                .Where(x => x != null)
                .OrderByDescending(x => x.DateStart)
                .Select(x => x.Activity)
                .ToList();

            FrmListBoxDialog frmListDialog = new FrmListBoxDialog();
            frmListDialog.Title = "Search Activity";
            frmListDialog.LoadItems(listActivities);

            DialogResult result = frmListDialog.ShowDialog(this);
            if (result != DialogResult.OK) { return; }

            if (string.IsNullOrEmpty(frmListDialog.Value)) { return; }

            txtActivity.Text = frmListDialog.Value;
        }

        private void btnSearchTag_Click(object sender, EventArgs e)
        {
            if (_workLog == null) { return; }

            List<string> listTags = _workLog
                .Where(x => string.IsNullOrEmpty(x.Tags) == false)
                .GroupBy(x => x.Tags)
                .Select(g => g.OrderBy(x => x.DateStart).LastOrDefault())
                .Where(x => x != null)
                .OrderByDescending(x => x.DateStart)
                .Select(x => x.Tags)
                .ToList();

            FrmListBoxDialog frmListDialog = new FrmListBoxDialog();
            frmListDialog.Title = "Search Tags";
            frmListDialog.LoadItems(listTags);

            DialogResult result = frmListDialog.ShowDialog(this);
            if (result != DialogResult.OK) { return; }

            if (string.IsNullOrEmpty(frmListDialog.Value)) { return; }

            txtTags.Text = frmListDialog.Value;
        }

        #endregion UI events

        #region Private methods

        private const string ConfigFile = "WorkLog.Config.json";
        private WorkLogConfig _config;

        private void WorkLog_LoadConfig()
        {
            if (File.Exists(ConfigFile))
            {
                JsonParser jsonParser = new JsonParser();
                jsonParser.KnownTypes.Add(typeof(WorkLogConfig));
                string jsonConfig = File.ReadAllText(ConfigFile);
                _config = jsonParser.Parse(jsonConfig) as WorkLogConfig;
            }

            if (_config == null)
            {
                _config = new WorkLogConfig();
            }

            txtName.Text = _config.LastName;
        }

        private void WorkLog_SaveConfig()
        {
            _config.LastName = txtName.Text;

            JsonWriter jsonWriter = new JsonWriter(new JsonWriterConfiguration(indent: true));
            using (StreamWriter streamWriter = new StreamWriter(ConfigFile))
            {
                jsonWriter.Write(_config, streamWriter);
            }
        }

        private List<WorkLogItem> _workLog;

        private void WorkLog_LoadData()
        {
            _workLog = null;
            string fileName = $"{txtName.Text}.WorkLog.json";
            if (File.Exists(fileName))
            {
                string rawFile = File.ReadAllText(fileName);
                JsonParser jsonParser = new JsonParser();
                jsonParser.KnownTypes.Add(typeof(WorkLogItem));
                object result = jsonParser.Parse(rawFile);
                if (result is IEnumerable<object> results)
                {
                    _workLog = new List<WorkLogItem>();
                    foreach (object obj in results)
                    {
                        WorkLogItem item = obj as WorkLogItem;
                        if (item == null) { continue; }

                        _workLog.Add(item);
                    }
                }
            }

            if (_workLog == null)
            {
                _workLog = new List<WorkLogItem>();
            }

            WorkLog_Refresh();
            WorkLog_SelectDate(DateTime.Now);
            WorkLog_CleanDirty();
        }

        private void WorkLog_SaveData()
        {
            string fileName = $"{txtName.Text}.WorkLog.json";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            JsonWriter jsonWriter = new JsonWriter(new JsonWriterConfiguration(indent: true));
            using (StreamWriter streamWriter = new StreamWriter(fileName))
            {
                jsonWriter.Write(_workLog, streamWriter);
            }

            WorkLog_CleanDirty();
        }

        private void WorkLog_MarkDirty()
        {
            btnSave.Enabled = true;
        }

        private void WorkLog_CleanDirty()
        {
            btnSave.Enabled = false;
        }

        private void WorkLog_SelectDate(DateTime date)
        {
            for (int i = 0; i < lsbWorkLog.Items.Count; i++)
            {
                WorkLogRow row = lsbWorkLog.Items[i] as WorkLogRow;
                if (row?.DateEnd > date)
                {
                    lsbWorkLog.SetSelected(i, true);
                    break;
                }
            }
        }

        private void WorkLog_Refresh()
        {
            int selectedIndex = lsbWorkLog.SelectedIndex;
            DateTime today = dtToday.Value;
            lsbWorkLog_BindData(_workLog, today.Year, today.Month, today.Day);
            if (selectedIndex > -1)
            {
                lsbWorkLog.SelectedIndex = selectedIndex;
            }
        }

        private WorkLogItem _currentWorkLogItem;

        private void WorkLogItem_Show(WorkLogItem item)
        {
            _currentWorkLogItem = null;
            if (item == null)
            {
                dtStart.Value = DateTime.UtcNow.Date;
                dtEnd.Value = DateTime.UtcNow.Date;
                txtActivity.Text = string.Empty;
                txtDescription.Text = string.Empty;
                txtTags.Text = string.Empty;
                lblWorkLogItemTime.Text = string.Empty;
                WorkLogItem_EnableButtons(false);
                return;
            }

            dtStart.Value = item.DateStart;
            dtEnd.Value = item.DateEnd;
            txtActivity.Text = item.Activity;
            txtDescription.Text = item.Description;
            txtTags.Text = item.Tags;
            lblWorkLogItemTime.Text = (item.DateEnd - item.DateStart).ToString();

            WorkLogItem_EnableButtons(true);
            _currentWorkLogItem = item;
        }

        private void WorkLogItem_EnableButtons(bool enable)
        {
            btnAdd.Enabled = !enable;
            btnDelete.Enabled = enable;
            btnRename.Enabled = enable;
            btnStats.Enabled = enable;
        }

        private void WorkLogItem_Update(bool refresh = true)
        {
            if (_currentWorkLogItem == null) { return; }

            DateTime dateStart = dtStart.Value;
            _currentWorkLogItem.DateStart = dateStart;
            _currentWorkLogItem.DateEnd = dtEnd.Value;
            _currentWorkLogItem.Activity = txtActivity.Text;
            _currentWorkLogItem.Description = txtDescription.Text;
            _currentWorkLogItem.Tags = txtTags.Text;
            if (refresh)
            {
                lsbWorkLog.SelectedIndex = -1;
                WorkLog_Refresh();
                WorkLog_SelectDate(dateStart);
            }

            WorkLog_MarkDirty();
        }

        private void lsbWorkLog_BindData(IEnumerable<WorkLogItem> items, int year, int month, int day, int q = 15)
        {
            int topIndex = lsbWorkLog.TopIndex;
            List<WorkLogRow> rows = new List<WorkLogRow>();
            IEnumerable<WorkLogItem> workLogItems = items.ToList();
            for (int h = 0; h < 24; h++)
            {
                for (int m = 0; m < 60; m += q)
                {
                    DateTime dateStart = new DateTime(year, month, day, h, m, 0);
                    DateTime dateEnd = dateStart.AddMinutes(q);
                    WorkLogRow row = new WorkLogRow { DateStart = dateStart, DateEnd = dateEnd, };
                    foreach (WorkLogItem item in workLogItems)
                    {
                        row.SetItem(item);
                    }

                    rows.Add(row);
                }
            }

            lsbWorkLog.Items.Clear();
            lsbWorkLog.Items.AddRange(rows.ToArray<object>());
            lsbWorkLog.TopIndex = topIndex;

            DateTime dateDay = new DateTime(year, month, day, 0, 0, 0);
            TimeSpan tsTotalTime = new TimeSpan(0);
            foreach (WorkLogItem item in workLogItems)
            {
                if (item.DateStart.Date != dateDay) { continue; }

                tsTotalTime += (item.DateEnd - item.DateStart);
            }

            lblWorkLogTime.Text = tsTotalTime.ToString();
        }

        private static List<WorkLogItem> WorkLogItemList_Merge(List<WorkLogItem> workLogA, List<WorkLogItem> workLogB)
        {
            List<WorkLogItem> newWorkLog = new List<WorkLogItem>();

            // Add non-overlapping from A
            foreach (WorkLogItem itemA in workLogA)
            {
                bool skip = false;
                foreach (WorkLogItem itemB in workLogB)
                {
                    if (itemB.Overlaps(itemA))
                    {
                        skip = true;

                        // If there is matching overlap, try to keep Activity, description or tags
                        if (itemA.DateStart == itemB.DateStart && itemA.DateEnd == itemB.DateEnd)
                        {
                            if (string.IsNullOrEmpty(itemB.Activity))
                            {
                                itemB.Activity = itemA.Activity;
                            }

                            if (string.IsNullOrEmpty(itemB.Description))
                            {
                                itemB.Description = itemA.Description;
                            }

                            if (string.IsNullOrEmpty(itemB.Tags))
                            {
                                itemB.Tags = itemA.Tags;
                            }

                            break;
                        }
                    }
                }

                if (skip) { continue; }

                newWorkLog.Add(itemA);
            }

            // All all from B
            foreach (WorkLogItem itemB in workLogB)
            {
                newWorkLog.Add(itemB);
            }

            return newWorkLog;
        }

        #endregion Private methods
    }

    public class WorkLogRow
    {
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public WorkLogItem Item { get; private set; }

        public override string ToString()
        {
            StringBuilder sbRow = new StringBuilder();

            sbRow.AppendFormat("{0:00}:{1:00} ", DateStart.Hour, DateStart.Minute);

            if (Item == null)
            {
                return sbRow.ToString();
            }

            int rowLenght = 30;
            int textLenght = Item.Activity.Length + 2;
            if (textLenght > rowLenght) { rowLenght = textLenght; }

            if (Item.DateStart >= DateStart && Item.DateStart < DateEnd)
            {
                if (Item.DateEnd >= DateStart && Item.DateEnd <= DateEnd)
                {
                    sbRow.Append("─ ");
                    sbRow.Append(Item.Activity);
                    sbRow.Append(" ");
                    sbRow.Append(new string('─', (rowLenght - textLenght) + 1));
                }
                else
                {
                    sbRow.Append("┌ ");
                    sbRow.Append(Item.Activity);
                    sbRow.Append(" ");
                    sbRow.Append(new string('─', rowLenght - textLenght));
                    sbRow.Append("┐");
                }
            }
            else if (Item.DateEnd >= DateStart && Item.DateEnd <= DateEnd)
            {
                sbRow.Append("└");
                sbRow.Append(new string('─', rowLenght));
                sbRow.Append("┘");
            }
            else
            {
                sbRow.Append("│");
                sbRow.Append(new string(' ', rowLenght));
                sbRow.Append("│");
            }

            return sbRow.ToString();
        }

        public void SetItem(WorkLogItem item)
        {
            if (item == null)
            {
                Item = null;
                return;
            }

            if (item.DateStart >= DateEnd || item.DateEnd <= DateStart) { return; }

            Item = item;
        }
    }

    public class WorkLogConfig
    {
        public string LastName { get; set; }
    }
}