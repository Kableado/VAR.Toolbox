using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VAR.Toolbox.Code.WorkLog;
using VAR.Toolbox.Controls;

namespace VAR.Toolbox.UI.Tools.WorkLog
{
    public partial class FrmWorkLogStats : Frame
    {
        public FrmWorkLogStats()
        {
            InitializeComponent();
        }

        public string Activity
        {
            get => txtActivity.Text;
            set => txtActivity.Text = value;
        }

        private List<WorkLogItem> _workLog;

        public List<WorkLogItem> WorkLog
        {
            get => _workLog;
            set => _workLog = value;
        }

        private void FrmWorkLogStats_Load(object sender, EventArgs e)
        {
            dtpStart.Value = DateTime.Now.Date.AddMonths(-1);
            dtpEnd.Value = DateTime.Now.Date.AddMonths(1).AddDays(1).AddSeconds(-1);
            WorkLog_ProcessStats();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            WorkLog_ProcessStats();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void WorkLog_ProcessStats()
        {
            bool found = false;
            DateTime dateStart = DateTime.MaxValue;
            DateTime dateEnd = DateTime.MinValue;
            Dictionary<DateTime, TimeSpan> dictDaysHours = new Dictionary<DateTime, TimeSpan>();
            foreach (WorkLogItem item in _workLog)
            {
                if (item.Activity.Contains(txtActivity.Text) == false) { continue; }

                if (item.DateEnd < dtpStart.Value || item.DateStart > dtpEnd.Value) { continue; }

                found = true;
                if (item.DateStart < dateStart) { dateStart = item.DateStart; }

                if (item.DateEnd > dateEnd) { dateEnd = item.DateEnd; }

                DateTime dateItemDay = item.DateStart.Date;
                TimeSpan tsItem = item.DateEnd - item.DateStart;

                if (dictDaysHours.ContainsKey(dateItemDay))
                {
                    TimeSpan tsDay = tsItem + dictDaysHours[dateItemDay];
                    dictDaysHours[dateItemDay] = tsDay;
                }
                else
                {
                    dictDaysHours.Add(dateItemDay, tsItem);
                }
            }

            if (found == false)
            {
                lblDateStart.Text = string.Empty;
                lblDateEnd.Text = string.Empty;
                lsbDays.Items.Clear();
                lblTotalTime.Text = string.Empty;
                return;
            }

            lblDateStart.Text = dateStart.ToString("yyyy-MM-dd HH:mm:ss");
            lblDateEnd.Text = dateEnd.ToString("yyyy-MM-dd HH:mm:ss");

            List<string> strDays = new List<string>();
            DateTime dateDayStart = dateStart.Date;
            DateTime dateDayEnd = dateEnd.Date;
            DateTime dateDayCurrent = dateDayStart;
            TimeSpan tsTotal = new TimeSpan(0);
            int? week = null;
            TimeSpan tsWeek = new TimeSpan(0);
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            do
            {
                if (dictDaysHours.ContainsKey(dateDayCurrent))
                {
                    int weekCurrent = currentCulture.Calendar.GetWeekOfYear(dateDayCurrent,
                        currentCulture.DateTimeFormat.CalendarWeekRule, currentCulture.DateTimeFormat.FirstDayOfWeek);
                    if (week != null && week != weekCurrent)
                    {
                        strDays.Add(string.Format("  [{0:00}] -- {1} h",
                            week,
                            tsWeek.TotalHours));
                        tsWeek = new TimeSpan(0);
                    }

                    TimeSpan tsDay = dictDaysHours[dateDayCurrent];
                    strDays.Add(string.Format("[{0:00}] {1:yyyy-MM-dd} -- {2} h",
                        weekCurrent, dateDayCurrent,
                        tsDay.TotalHours));
                    tsTotal += tsDay;
                    tsWeek += tsDay;
                    week = weekCurrent;
                }

                dateDayCurrent = dateDayCurrent.AddDays(1);
            } while (dateDayCurrent <= dateDayEnd);

            if (tsWeek.TotalHours > 0)
            {
                strDays.Add(string.Format("  [{0:00}] -- {1} h",
                    week,
                    tsWeek.TotalHours));
            }

            lsbDays.Items.Clear();
            lsbDays.Items.AddRange(strDays.ToArray<object>());
            lblTotalTime.Text = $"{tsTotal.ToString()} - {tsTotal.TotalHours}";
        }
    }
}