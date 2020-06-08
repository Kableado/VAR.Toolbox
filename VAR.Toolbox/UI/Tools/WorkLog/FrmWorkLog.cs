﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using VAR.Json;
using VAR.Toolbox.Code;

namespace VAR.Toolbox.UI.Tools.WorkLog
{
    public partial class FrmWorkLog : Form, IToolForm
    {
        #region IToolForm

        public string ToolName { get { return "WorkLog"; } }

        public bool HasIcon { get { return false; } }

        #endregion IToolForm

        #region Form life cycle

        public FrmWorkLog()
        {
            InitializeComponent();
            WorkLog_LoadConfig();
            WorkLog_LoadData();

            cboImporters.Items.AddRange(WorkLogImporterFactory.GetNames());
        }

        private void FrmWorkLog_FormClosing(object sender, FormClosingEventArgs e)
        {
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
                IWorkLogImporter workLogImporter = WorkLogImporterFactory.CreateFromName(cboImporters.SelectedItem as string);
                List<WorkLogItem> newWorkLog = workLogImporter.Import(this);
                if (newWorkLog != null)
                {
                    _workLog = newWorkLog;
                    WorkLog_Refresh();
                    MessageBox.Show("OK");
                }
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
                IWorkLogImporter workLogImporter = WorkLogImporterFactory.CreateFromName(cboImporters.SelectedItem as string);
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

        private bool _selecting = false;

        private void lsbWorkLog_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (_selecting) { return; }
            _selecting = true;
            WorkLogRow row = lsbWorkLog.SelectedItem as WorkLogRow;
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
                lsbWorkLog.SetSelected(i, (row.Item == auxRow.Item));
            }
            WorkLogItem_Show(row.Item);
            _selecting = false;
        }

        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            WorkLogItem item = new WorkLogItem
            {
                DateStart = dtStart.Value,
                DateEnd = dtEnd.Value,
                Activity = txtActivity.Text,
                Description = txtDescription.Text,
            };
            _workLog.Add(item);
            WorkLog_Refresh();
            WorkLog_SelectDate(item.DateStart);
            WorkLogItem_Show(item);
            WorkLog_MarkDirty();
        }

        private void btnDelete_Click(object sender, System.EventArgs e)
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

        private void txtActivity_TextChanged(object sender, System.EventArgs e)
        {
            WorkLogItem_Update();
        }

        private void txtDescription_TextChanged(object sender, System.EventArgs e)
        {
            WorkLogItem_Update(refresh: false);
        }

        private void dtToday_ValueChanged(object sender, System.EventArgs e)
        {
            WorkLog_Refresh();
        }

        private void btnPreviousDay_Click(object sender, System.EventArgs e)
        {
            dtToday.Value = dtToday.Value.Date.AddDays(-1);
        }

        private void btnNextDay_Click(object sender, System.EventArgs e)
        {
            dtToday.Value = dtToday.Value.Date.AddDays(1);
        }

        #endregion UI events

        #region Private methods

        private const string ConfigFile = "WorkLog.Config.json";
        private WorkLogConfig _config = null;

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

        private List<WorkLogItem> _workLog = null;

        private void WorkLog_LoadData()
        {
            _workLog = null;
            string fileName = string.Format("{0}.WorkLog.json", txtName.Text);
            if (File.Exists(fileName))
            {
                string rawFile = File.ReadAllText(fileName);
                JsonParser jsonParser = new JsonParser();
                jsonParser.KnownTypes.Add(typeof(WorkLogItem));
                object result = jsonParser.Parse(rawFile);
                if (result is IEnumerable<object>)
                {
                    _workLog = new List<WorkLogItem>();
                    foreach (object obj in (IEnumerable<object>)result)
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
            string fileName = string.Format("{0}.WorkLog.json", txtName.Text);
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
                if (row.DateEnd > date)
                {
                    lsbWorkLog.SetSelected(i, true);
                    break;
                }
            }
        }

        private void WorkLog_Refresh()
        {
            DateTime today = dtToday.Value;
            lsbWorkLog_BindData(_workLog, today.Year, today.Month, today.Day);
        }

        private WorkLogItem _currentWorkLogItem = null;

        private void WorkLogItem_Show(WorkLogItem item)
        {
            _currentWorkLogItem = null;
            if (item == null)
            {
                dtStart.Value = DateTime.UtcNow.Date;
                dtEnd.Value = DateTime.UtcNow.Date;
                txtActivity.Text = string.Empty;
                txtDescription.Text = string.Empty;
                btnAdd.Enabled = true;
                btnDelete.Enabled = false;
                return;
            }

            dtStart.Value = item.DateStart;
            dtEnd.Value = item.DateEnd;
            txtActivity.Text = item.Activity;
            txtDescription.Text = item.Description;

            btnAdd.Enabled = false;
            btnDelete.Enabled = true;
            _currentWorkLogItem = item;
        }

        private void WorkLogItem_Update(bool refresh = true)
        {
            if (_currentWorkLogItem == null) { return; }
            _currentWorkLogItem.DateStart = dtStart.Value;
            _currentWorkLogItem.DateEnd = dtEnd.Value;
            _currentWorkLogItem.Activity = txtActivity.Text;
            _currentWorkLogItem.Description = txtDescription.Text;
            if (refresh)
            {
                WorkLog_Refresh();
                WorkLog_SelectDate(_currentWorkLogItem.DateStart);
            }
            WorkLog_MarkDirty();
        }

        public void lsbWorkLog_BindData(IEnumerable<WorkLogItem> items, int year, int month, int day, int q = 15)
        {
            List<WorkLogRow> rows = new List<WorkLogRow>();
            for (int h = 0; h < 24; h++)
            {
                for (int m = 0; m < 60; m += q)
                {
                    DateTime dateStart = new DateTime(year, month, day, h, m, 0);
                    DateTime dateEnd = dateStart.AddMinutes(q);
                    WorkLogRow row = new WorkLogRow { DateStart = dateStart, DateEnd = dateEnd, };
                    if (items != null)
                    {
                        foreach (WorkLogItem item in items)
                        {
                            row.SetItem(item);
                        }
                    }
                    rows.Add(row);
                }
            }
            lsbWorkLog.Items.Clear();
            lsbWorkLog.Items.AddRange(rows.ToArray());

            EnableRepaint(new HandleRef(lsbWorkLog, lsbWorkLog.Handle), true);
            lsbWorkLog.Invalidate();
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        private static extern IntPtr SendMessage(HandleRef hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);

        private static void EnableRepaint(HandleRef handle, bool enable)
        {
            const int WM_SETREDRAW = 0x000B;
            SendMessage(handle, WM_SETREDRAW, new IntPtr(enable ? 1 : 0), IntPtr.Zero);
        }

        #endregion Private methods

    }

    public class WorkLogRow
    {
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public WorkLogItem Item { get; set; }

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
                    sbRow.Append("- ");
                    sbRow.Append(Item.Activity);
                    sbRow.Append(" ");
                    sbRow.Append(new string('-', (rowLenght - textLenght) + 1));
                }
                else
                {
                    sbRow.Append("┌ ");
                    sbRow.Append(Item.Activity);
                    sbRow.Append(" ");
                    sbRow.Append(new string('-', rowLenght - textLenght));
                    sbRow.Append("┐");
                }
            }
            else if (Item.DateEnd >= DateStart && Item.DateEnd <= DateEnd)
            {
                sbRow.Append("└ ");
                sbRow.Append(Item.Activity);
                sbRow.Append(" ");
                sbRow.Append(new string('-', rowLenght - textLenght));
                sbRow.Append("┘");
            }
            else
            {
                sbRow.Append("│");
                sbRow.Append(new string('#', rowLenght));
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

    public class WorkLogItem
    {
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Activity { get; set; }
        public string Description { get; set; }
    }

    public class WorkLogConfig
    {
        public string LastName { get; set; }
    }
}