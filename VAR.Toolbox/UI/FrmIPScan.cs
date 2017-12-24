using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace VAR.Toolbox.UI
{
    public partial class FrmIPScan : Form
    {
        public FrmIPScan()
        {
            InitializeComponent();

            PrintStatus("Idle");
        }

        private void PrintStatus(string status)
        {
            if (lblStatus.InvokeRequired)
            {
                lblStatus.Invoke((MethodInvoker)(() => { lblStatus.Text = string.Format("Status: {0}", status); }));
            }
            else
            {
                lblStatus.Text = string.Format("Status: {0}", status);
                Application.DoEvents();
            }
        }

        private void ResultsAddLine(string line)
        {
            if (lsvResult.InvokeRequired)
            {
                lsvResult.Invoke((MethodInvoker)(() => { lsvResult.Items.Add(line); }));
            }
            else
            {
                lsvResult.Items.Add(line);
                Application.DoEvents();
            }
        }

        private void Control_SetEnabled(Control ctrl, bool enabled)
        {
            if (ctrl.InvokeRequired)
            {
                ctrl.Invoke((MethodInvoker)(() => { ctrl.Enabled = enabled; }));
            }
            else
            {
                ctrl.Enabled = enabled;
                Application.DoEvents();
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => { IPScan(); });
            thread.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            running = false;
        }

        private bool running = false;

        private void IPScan()
        {
            Control_SetEnabled(btnScan, false);
            running = true;
            ResultsAddLine(string.Format("IPScan started at {0}", DateTime.UtcNow.ToString("s")));
            string ipBase = "192.168.0.";
            for (int i = 1; i < 255 && running; i++)
            {
                string ip = ipBase + i.ToString();
                PrintStatus(string.Format("Scanning {0}", ip));
                Ping p = new Ping();
                PingReply pingReply = p.Send(ip, 100);
                if (pingReply.Status == IPStatus.Success)
                {
                    string name = "?";
                    try
                    {
                        IPHostEntry hostEntry = Dns.GetHostEntry(ip);
                        name = hostEntry.HostName;
                    }
                    catch (SocketException) { }
                    ResultsAddLine(string.Format("{0} ({1}) is up: ({2} ms)", ip, name, pingReply.RoundtripTime));
                }
                Application.DoEvents();
            }
            PrintStatus("Idle");
            ResultsAddLine(string.Format("IPScan ended at {0}", DateTime.UtcNow.ToString("s")));
            Control_SetEnabled(btnScan, true);
        }

    }
}
