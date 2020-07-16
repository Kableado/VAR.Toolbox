using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VAR.Toolbox.Controls;

namespace VAR.Toolbox.UI.Tools.WorkLog
{
    public partial class FrmWorkLogSumary : Frame
    {
        public FrmWorkLogSumary()
        {
            InitializeComponent();
        }

        private List<WorkLogItem> _workLog = null;

        public List<WorkLogItem> WorkLog
        {
            get { return _workLog; }
            set { _workLog = value; }
        }

        private void FrmWorkLogStats_Load(object sender, EventArgs e)
        {
            dtpStart.Value = DateTime.Now.Date;
            dtpEnd.Value = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            WorkLog_ProcessStats();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            WorkLog_ProcessStats();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void WorkLog_ProcessStats()
        {
            bool found = false;
            DateTime dateStart = DateTime.MaxValue;
            DateTime dateEnd = DateTime.MinValue;
            Dictionary<string, TimeSpan> dictActivityHours = new Dictionary<string, TimeSpan>();
            foreach (WorkLogItem item in _workLog)
            {
                if (item.DateEnd < dtpStart.Value || item.DateStart > dtpEnd.Value) { continue; }

                found = true;
                if (item.DateStart < dateStart) { dateStart = item.DateStart; }
                if (item.DateEnd > dateEnd) { dateEnd = item.DateEnd; }

                TimeSpan tsItem = item.DateEnd - item.DateStart;

                if (dictActivityHours.ContainsKey(item.Activity))
                {
                    TimeSpan tsDay = tsItem + dictActivityHours[item.Activity];
                    dictActivityHours[item.Activity] = tsDay;
                }
                else
                {
                    dictActivityHours.Add(item.Activity, tsItem);
                }
            }

            if (found == false)
            {
                lblDateStart.Text = string.Empty;
                lblDateEnd.Text = string.Empty;
                lsbActivities.Items.Clear();
                lblTotalTime.Text = string.Empty;
                return;
            }

            lblDateStart.Text = dateStart.ToString("yyyy-MM-dd HH:mm:ss");
            lblDateEnd.Text = dateEnd.ToString("yyyy-MM-dd HH:mm:ss");

            List<string> strActivities = new List<string>();
            TimeSpan tsTotal = new TimeSpan(0);
            IEnumerable<IGrouping<string, KeyValuePair<string, TimeSpan>>> activityGroups = dictActivityHours
                .GroupBy(p => p.Key.Split(' ').FirstOrDefault(), p => p)
                .OrderBy(x => x.Key);
            foreach (IGrouping<string, KeyValuePair<string, TimeSpan>> activityGroup in activityGroups)
            {
                TimeSpan tsActivityGroup = new TimeSpan(0);
                foreach (KeyValuePair<string, TimeSpan> pair in activityGroup)
                {
                    tsActivityGroup += pair.Value;
                }
                strActivities.Add(string.Format("{0} -- {1} h", activityGroup.Key, tsActivityGroup.TotalHours));
                if (chkOnlyGroups.Checked == false)
                {
                    foreach (KeyValuePair<string, TimeSpan> pair in activityGroup)
                    {
                        strActivities.Add(string.Format("    {0} -- {1} h", pair.Key, pair.Value.TotalHours));
                        tsTotal += pair.Value;
                    }
                }
                else
                {
                    tsTotal += tsActivityGroup;
                }
            }
            lsbActivities.Items.Clear();
            lsbActivities.Items.AddRange(strActivities.ToArray());
            lblTotalTime.Text = string.Format("{0} - {1}", tsTotal.ToString(), tsTotal.TotalHours);
        }

        private void lsbActivities_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                CopyToClipboard();
            }
        }
        private void CopyToClipboard()
        {
            StringBuilder sbText = new StringBuilder();
            foreach (string item in lsbActivities.SelectedItems)
            {
                sbText.AppendLine(item);
            }
            if (sbText.Length > 0)
            {
                Clipboard.SetText(sbText.ToString());
            }
        }

        private void lsbActivities_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtActivity.Text = (lsbActivities.SelectedItem as string) ?? string.Empty;
        }
    }
}
