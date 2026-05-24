using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Threading;
using VAR.Toolbox.Controls;

namespace VAR.Toolbox.UI.Tools
{
    public class FrmIPScan : Window, IToolForm
    {
        public string ToolName => "IPScan";
        public bool HasIcon => false;

        private readonly CtrOutput _ctrOutput;
        private readonly Button _btnScan;
        private readonly TextBox _txtSubnet;
        private readonly TextBlock _lblStatus;

        public FrmIPScan()
        {
            Title = "IPScan";
            Width = 365;
            Height = 307;

            _btnScan = new Button { Content = "Scan", };
            _btnScan.Click += BtnScan_Click;

            Button btnStop = new() { Content = "Stop", };
            btnStop.Click += BtnStop_Click;

            _lblStatus = new TextBlock { Text = "Status: Idle", VerticalAlignment = VerticalAlignment.Center, };
            _txtSubnet = new TextBox { Classes = { "mono", }, Text = "192.168.0.", Width = 150, };
            _ctrOutput = new CtrOutput();

            StackPanel topRow = new() { Orientation = Orientation.Horizontal, Spacing = 5, };
            topRow.Children.Add(_btnScan);
            topRow.Children.Add(btnStop);
            topRow.Children.Add(_lblStatus);

            DockPanel layout = new() { Margin = new Thickness(8), };
            DockPanel.SetDock(topRow, Dock.Top);
            Border subnetBorder = new() { Child = _txtSubnet, Margin = new Thickness(0, 5), };
            DockPanel.SetDock(subnetBorder, Dock.Top);
            layout.Children.Add(topRow);
            layout.Children.Add(subnetBorder);
            layout.Children.Add(_ctrOutput);

            Content = layout;
            Closed += (_, _) => { _running = false; };
        }

        private void PrintStatus(string status)
        {
            Dispatcher.UIThread.Post(() => { _lblStatus.Text = $"Status: {status}"; });
        }

        private void BtnScan_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            string txtSubnet = _txtSubnet.Text ?? "192.168.0.";
            Thread thread = new(() => { IPScan(txtSubnet); });
            thread.Start();
        }

        private void BtnStop_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            _running = false;
        }

        private bool _running;

        private void IPScan(string ipBase)
        {
            Dispatcher.UIThread.Post(() => { _btnScan.IsEnabled = false; });
            _running = true;
            _ctrOutput.AddLine($"IPScan started at {DateTime.UtcNow:s}");
            for (int i = 1; i < 255 && _running; i++)
            {
                string ip = ipBase + i.ToString();
                PrintStatus($"Scanning {ip}");
                Ping p = new();
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

                    _ctrOutput.AddLine($"{ip} ({name}) is up: ({pingReply.RoundtripTime} ms)");
                }
            }

            PrintStatus("Idle");
            _ctrOutput.AddLine($"IPScan ended at {DateTime.UtcNow:s}");
            Dispatcher.UIThread.Post(() => { _btnScan.IsEnabled = true; });
        }
    }
}