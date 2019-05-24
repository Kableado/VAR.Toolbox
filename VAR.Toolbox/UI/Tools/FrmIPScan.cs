using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace VAR.Toolbox.UI
{
    public partial class FrmIPScan : Form, IToolForm
    {
        public string ToolName { get { return "IPScan"; } }

        public bool HasIcon { get { return false; } }

        public FrmIPScan()
        {
            InitializeComponent();
            Disposed += FrmIPScan_Disposed;

            PrintStatus("Idle");
        }

        private void FrmIPScan_Disposed(object sender, EventArgs e)
        {
            running = false;
        }

        private void PrintStatus(string status)
        {
            if (lblStatus.IsDisposed) { return; }
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

        private void Control_SetEnabled(Control ctrl, bool enabled)
        {
            if (ctrl.IsDisposed) { return; }
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

        private void BtnScan_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => { IPScan(txtSubnet.Text); });
            thread.Start();
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            running = false;
        }

        private bool running = false;

        private void IPScan(string ipBase)
        {
            Control_SetEnabled(btnScan, false);
            running = true;
            ctrOutput.AddLine(string.Format("IPScan started at {0}", DateTime.UtcNow.ToString("s")));
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
                    ctrOutput.AddLine(string.Format("{0} ({1}) is up: ({2} ms)", ip, name, pingReply.RoundtripTime));
                }
                Application.DoEvents();
            }
            PrintStatus("Idle");
            ctrOutput.AddLine(string.Format("IPScan ended at {0}", DateTime.UtcNow.ToString("s")));
            Control_SetEnabled(btnScan, true);
        }

    }
}
