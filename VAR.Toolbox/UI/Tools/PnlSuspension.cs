using System;
using System.Windows.Forms;
using VAR.Toolbox.Code;
using VAR.Toolbox.Code.Windows;
using VAR.Toolbox.Controls;

namespace VAR.Toolbox.UI.Tools
{
    public partial class PnlSuspension : SubFrame, IToolPanel
    {
        public const string PreSuspendEventName = "PreSuspend";

        private readonly Random _rnd = new Random();

        public PnlSuspension()
        {
            InitializeComponent();
            DdlCustomHour_Load();
            DdlCustomMinute_Load();
            RandomizeOffset();
        }

        private void BtnCustomSuspendNow_Click(object sender, EventArgs e)
        {
            CustomHourMinute_SetNow();
        }

        private void BtnRandOffset_Click(object sender, EventArgs e)
        {
            RandomizeOffset();
        }

        private void TimTicker_Tick(object sender, EventArgs e)
        {
            if (DesignMode) { return; }

            ResetCountdown();
            DateTime now = DateTime.Now;

            if (ddlCustomHour.SelectedIndex >= 0 && ddlCustomMinute.SelectedIndex >= 0)
            {
                DateTime dtSuspendAtCustom =
                    new DateTime(
                            now.Year,
                            now.Month,
                            now.Day,
                            ddlCustomHour.SelectedIndex,
                            ddlCustomMinute.SelectedIndex,
                            0)
                        .AddSeconds(Convert.ToInt32(numOffset.Value));

                CheckTime(chkSuspendAtCustom, dtSuspendAtCustom, now);
            }

            timTicker.Stop();
            timTicker.Start();
        }

        private void DdlCustomHour_Load()
        {
            for (int i = 0; i < 24; i++)
            {
                ddlCustomHour.Items.Add($"{i:00}");
            }
        }

        private void DdlCustomMinute_Load()
        {
            for (int i = 0; i < 60; i++)
            {
                ddlCustomMinute.Items.Add($"{i:00}");
            }
        }

        private void CustomHourMinute_SetNow()
        {
            DateTime now = DateTime.Now;
            ddlCustomHour.SelectedIndex = now.Hour;
            ddlCustomMinute.SelectedIndex = now.Minute;
        }

        private void RandomizeOffset()
        {
            numOffset.Value = (_rnd.Next() % 599) + 1;
        }

        private void ResetCountdown()
        {
            lblCountdown.Text = "00:00:00:00";
        }

        private void SetCountdown(DateTime dateTime, DateTime now)
        {
            TimeSpan timeSpan = dateTime - now;
            lblCountdown.Text = string.Format("{0:00}:{1:00}:{2:00}:{3:00}", timeSpan.Days, timeSpan.Hours,
                timeSpan.Minutes, timeSpan.Seconds);
        }

        private void CheckTime(CheckBox chk, DateTime dtSuspendAtCustom, DateTime now)
        {
            if (DateTime.Compare(now, dtSuspendAtCustom) > 0)
            {
                if (chk.Checked)
                {
                    chk.Checked = false;
                    RandomizeOffset();
                    SuspendSystem();
                }
                else
                {
                    chk.Enabled = false;
                }
            }
            else
            {
                SetCountdown(dtSuspendAtCustom, now);
                chk.Enabled = true;
            }
        }

        private void SuspendSystem()
        {
            EventDispatcher.EmitEvent(PreSuspendEventName, null);
            Win32.SetSuspendState(false, true, false);
        }
    }
}