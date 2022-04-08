using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using VAR.Toolbox.Controls;

namespace VAR.Toolbox.UI.Tools
{
    public partial class FrmIPScan : Frame, IToolForm
    {
        public string ToolName => "IPScan";

        public bool HasIcon => false;

        public FrmIPScan()
        {
            InitializeComponent();
            Disposed += FrmIPScan_Disposed;

            PrintStatus("Idle");
        }

        private void FrmIPScan_Disposed(object sender, EventArgs e)
        {
            _running = false;
        }

        private void PrintStatus(string status)
        {
            if (lblStatus.IsDisposed) { return; }

            if (lblStatus.InvokeRequired)
            {
                lblStatus.Invoke((MethodInvoker)(() => { lblStatus.Text = $"Status: {status}"; }));
            }
            else
            {
                lblStatus.Text = $"Status: {status}";
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
            _running = false;
        }

        private bool _running;

        private void IPScan(string ipBase)
        {
            Control_SetEnabled(btnScan, false);
            _running = true;
            ctrOutput.AddLine($"IPScan started at {DateTime.UtcNow:s}");
            for (int i = 1; i < 255 && _running; i++)
            {
                string ip = ipBase + i.ToString();
                PrintStatus($"Scanning {ip}");
                Ping p = new Ping();
                PingReply pingReply = p.Send(ip, 100);
                if (pingReply != null && pingReply.Status == IPStatus.Success)
                {
                    string name = "?";
                    try
                    {
                        IPHostEntry hostEntry = Dns.GetHostEntry(ip);
                        name = hostEntry.HostName;
                    }
                    catch (SocketException) { }

                    ctrOutput.AddLine($"{ip} ({name}) is up: ({pingReply.RoundtripTime} ms)");
                }

                Application.DoEvents();
            }

            PrintStatus("Idle");
            ctrOutput.AddLine($"IPScan ended at {DateTime.UtcNow:s}");
            Control_SetEnabled(btnScan, true);
        }
    }
}