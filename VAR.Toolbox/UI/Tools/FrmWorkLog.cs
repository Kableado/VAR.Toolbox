using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace VAR.Toolbox.UI.Tools
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
            WorkLog_LoadData();
        }

        #endregion Form life cycle

        #region UI events

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

        private List<WorkLogItem> _workLog = null;

        private void WorkLog_LoadData()
        {
            _workLog = new List<WorkLogItem>();
            WorkLog_Refresh();
            WorkLog_SelectDate(DateTime.Now);
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
}
