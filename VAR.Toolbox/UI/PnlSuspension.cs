using System;
using System.Windows.Forms;
using VAR.Toolbox.Code.Windows;

namespace VAR.Toolbox.UI
{
    public partial class PnlSuspension : UserControl
    {
        private Random rnd = new Random();

        public PnlSuspension()
        {
            InitializeComponent();
        }

        private void PnlSuspension_Load(object sender, EventArgs e)
        {
            ddlCustomHour_Load();
            ddlCustomMinute_Load();
            RandomizeOffset();
        }

        private void btnCustomSuspenedNow_Click(object sender, EventArgs e)
        {
            CustomHourMinute_SetNow();
        }

        private void btnRandOffset_Click(object sender, EventArgs e)
        {
            RandomizeOffset();
        }

        private void timTicker_Tick(object sender, EventArgs e)
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

                if (DateTime.Compare(now, dtSuspendAtCustom) > 0)
                {
                    if (chkSuspendAtCustom.Checked)
                    {
                        chkSuspendAtCustom.Checked = false;
                        RandomizeOffset();
                        SuspendSystem();
                    }
                    else
                    {
                        chkSuspendAtCustom.Enabled = false;
                    }
                }
                else
                {
                    SetCountdown(dtSuspendAtCustom, now);
                    chkSuspendAtCustom.Enabled = true;
                }
            }

            timTicker.Stop();
            timTicker.Start();
        }


        private void ddlCustomHour_Load()
        {
            for (int i = 0; i < 24; i++)
            {
                ddlCustomHour.Items.Add(string.Format("{0:00}", i));
            }
        }

        private void ddlCustomMinute_Load()
        {
            for (int i = 0; i < 60; i++)
            {
                ddlCustomMinute.Items.Add(string.Format("{0:00}", i));
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
            numOffset.Value = (rnd.Next() % 599) + 1;
        }

        private void ResetCountdown()
        {
            lblCountdown.Text = "00:00:00:00";
        }

        private void SetCountdown(DateTime dateTime, DateTime now)
        {
            TimeSpan timeSpan = dateTime - now;
            lblCountdown.Text = string.Format("{0:00}:{1:00}:{2:00}:{3:00}", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }

        private void CheckTime(CheckBox checkBox, DateTime dtSuspension, DateTime now)
        {
            if (DateTime.Compare(now, dtSuspension) > 0)
            {
                if (checkBox.Checked)
                {
                    chkSuspendAtCustom.Checked = false;
                    RandomizeOffset();
                    SuspendSystem();
                }
                else
                {
                    checkBox.Enabled = false;
                }
            }
            else
            {
                checkBox.Enabled = true;
            }
            if (dtSuspension > now)
            {
                SetCountdown(dtSuspension, now);
            }
        }

        private void SuspendSystem()
        {
            Win32.SetSuspendState(false, true, false);
        }

    }
}
