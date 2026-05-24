using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

using VAR.Toolbox.Controls;

namespace VAR.Toolbox.UI.Tools
{
    public class FrmTunnelTCP : Window, IToolForm
    {
        public string ToolName => "TunnelTCP";
        public bool HasIcon => false;

        private readonly Button _btnRun;
        private readonly Button _btnStop;
        private readonly TextBox _txtRemoteHost;
        private readonly TextBox _txtRemotePort;
        private readonly TextBox _txtLocalPort;
        private readonly CtrOutput _ctrOutput;
        private bool _running;

        private class ConnectedClient
        {
            public string RemoteHost { get; }
            public int RemotePort { get; }
            public Socket ClientSocket { get; }

            public ConnectedClient(string remoteHost, int remotePort, Socket clientSocket)
            {
                RemoteHost = remoteHost;
                RemotePort = remotePort;
                ClientSocket = clientSocket;
            }
        }

        public FrmTunnelTCP()
        {
            Title = "TunnelTCP";
            Width = 451;
            Height = 425;
            MinWidth = 473;
            MinHeight = 372;

            _txtRemoteHost = new TextBox { Classes = { "mono", }, };
            _txtRemotePort = new TextBox { Classes = { "mono", }, Width = 100, };
            _txtLocalPort = new TextBox { Classes = { "mono", }, Width = 100, };
            _ctrOutput = new CtrOutput();

            _btnRun = new Button { Content = "Run", Width = 83, Height = 45, };
            _btnRun.Click += BtnRun_Click;

            _btnStop = new Button { Content = "Stop", Width = 75, Height = 45, IsEnabled = false, };
            _btnStop.Click += BtnStop_Click;

            StackPanel fields = new() { Spacing = 4, };
            fields.Children.Add(MakeRow("RemoteHost", _txtRemoteHost));
            fields.Children.Add(MakeRow("RemotePort", _txtRemotePort));
            fields.Children.Add(MakeRow("LocalPort", _txtLocalPort));

            StackPanel buttons = new() { Orientation = Orientation.Horizontal, Spacing = 5, HorizontalAlignment = HorizontalAlignment.Right, };
            buttons.Children.Add(_btnStop);
            buttons.Children.Add(_btnRun);

            DockPanel layout = new() { Margin = new Thickness(8), };
            DockPanel.SetDock(fields, Dock.Top);
            DockPanel.SetDock(buttons, Dock.Bottom);
            layout.Children.Add(fields);
            layout.Children.Add(buttons);
            layout.Children.Add(_ctrOutput);

            Content = layout;
        }

        private static Control MakeRow(string label, Control control)
        {
            DockPanel row = new();
            TextBlock lbl = new() { Text = label, Width = 100, VerticalAlignment = VerticalAlignment.Center, };
            DockPanel.SetDock(lbl, Dock.Left);
            row.Children.Add(lbl);
            row.Children.Add(control);
            return row;
        }

        private void BtnStop_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (!_running) return;
            _running = false;
            _btnStop.IsEnabled = false;
            _btnRun.IsEnabled = true;
        }

        private void BtnRun_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (_running) return;
            _running = true;
            _btnStop.IsEnabled = true;
            _btnRun.IsEnabled = false;
            _ctrOutput.Clean();

            string remoteHost = _txtRemoteHost.Text ?? string.Empty;
            int remotePort = Convert.ToInt32(_txtRemotePort.Text);
            int localPort = Convert.ToInt32(_txtLocalPort.Text);

            Thread thread = new(() => { TunnelTCP(remoteHost, remotePort, localPort); });
            thread.Start();
        }

        private void TunnelTCP(string remoteHost, int remotePort, int localPort)
        {
            try
            {
                Socket sock = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sock.Bind(new IPEndPoint(IPAddress.Any, localPort));
                sock.Listen(1000);
                while (_running)
                {
                    if (sock.Poll(100, SelectMode.SelectRead))
                    {
                        Socket sockCliente = sock.Accept();
                        sockCliente.Blocking = false;
                        sockCliente.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
                        ConnectedClient client = new(remoteHost, remotePort, sockCliente);
                        Thread thread = new(() => { ProcessClient(client); });
                        thread.Start();
                    }
                }
                sock.Close();
            }
            catch (Exception ex)
            {
                _ctrOutput.AddLine("Exception: " + ex.Message);
                _ctrOutput.AddLine("Backtrace:");
                _ctrOutput.AddLine(ex.StackTrace ?? string.Empty);
            }
        }

        private void ProcessClient(ConnectedClient client)
        {
            try
            {
                Socket clientSock = client.ClientSocket;
                byte[] buffer = new byte[4096];
                long totalSent = 0;
                long totalReceived = 0;
                _ctrOutput.AddLine(DateTime.Now.ToString("s") + " Nuevo Cliente: " + ((IPEndPoint)clientSock.RemoteEndPoint!).Address);

                IPHostEntry entryHostRemoto = Dns.GetHostEntry(client.RemoteHost);
                IPAddress? ipRemoteHost = null;
                foreach (IPAddress address in entryHostRemoto.AddressList)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipRemoteHost = address;
                        break;
                    }
                }
                if (ipRemoteHost == null) return;

                IPEndPoint endPointRemoteHost = new(ipRemoteHost, client.RemotePort);
                Socket remoteSock = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                remoteSock.Connect(endPointRemoteHost);

                bool threadRunning = true;
                while (threadRunning && _running)
                {
                    int len;
                    if (clientSock.Poll(100, SelectMode.SelectRead))
                    {
                        if (clientSock.Available == 0) { threadRunning = false; }
                        else
                        {
                            len = clientSock.Receive(buffer, 0, buffer.Length, 0);
                            int enviado = 0;
                            do
                            {
                                try { enviado = remoteSock.Send(buffer, 0, len, 0); }
                                catch (Exception ex) { _ctrOutput.AddLine("No se pudo enviar... (" + ex.Message + ")"); }
                                totalReceived += enviado;
                            } while (enviado <= 0 && remoteSock.Connected);
                        }
                    }
                    if (remoteSock.Poll(100, SelectMode.SelectRead))
                    {
                        if (remoteSock.Available == 0) { threadRunning = false; }
                        else
                        {
                            len = remoteSock.Receive(buffer, 0, buffer.Length, 0);
                            int enviado = 0;
                            do
                            {
                                try { enviado = clientSock.Send(buffer, 0, len, 0); }
                                catch (Exception ex) { _ctrOutput.AddLine("No se pudo enviar... (" + ex.Message + ")"); Thread.Sleep(10); }
                                totalSent += enviado;
                            } while (enviado <= 0 && clientSock.Connected);
                        }
                    }
                }
                _ctrOutput.AddLine(DateTime.Now.ToString("s") + " Client disconnection: " +
                    ((IPEndPoint)clientSock.RemoteEndPoint!).Address + " IN/OUT: " + UnidadesBytes(totalReceived) + "/" + UnidadesBytes(totalSent));
                remoteSock.Close();
                clientSock.Close();
            }
            catch (Exception ex)
            {
                _ctrOutput.AddLine("Exception: " + ex.Message);
                _ctrOutput.AddLine("Backtrace:");
                _ctrOutput.AddLine(ex.StackTrace ?? string.Empty);
            }
        }

        private static string UnidadesBytes(long n)
        {
            double number = n;
            if (number < 1024) return $"{number} B";
            number /= 1024.0;
            if (number < 1024) return $"{number:#.00} KiB";
            number /= 1024.0;
            if (number < 1024) return $"{number:#.00} MiB";
            number /= 1024.0;
            if (number < 1024) return $"{number:#.00} GiB";
            number /= 1024.0;

            return $"{number:#.00} TiB";
        }
    }
}