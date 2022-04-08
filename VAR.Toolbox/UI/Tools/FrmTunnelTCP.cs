using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using VAR.Toolbox.Controls;

namespace VAR.Toolbox.UI.Tools
{
    public partial class FrmTunnelTCP : Frame, IToolForm
    {
        public string ToolName => "TunnelTCP";

        public bool HasIcon => false;

        private bool _running;

        private class ConnectedClient
        {
            public string RemoteHost;
            public int RemotePort;
            public Socket ClientSocket;
        }

        public FrmTunnelTCP()
        {
            InitializeComponent();
        }

        private void FrmTunnelTCP_Load(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            btnRun.Enabled = true;
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (_running == false) { return; }

            _running = false;
            btnStop.Enabled = false;
            btnRun.Enabled = true;
        }

        private void BtnRun_Click(object sender, EventArgs e)
        {
            if (_running) { return; }

            _running = true;
            btnStop.Enabled = true;
            btnRun.Enabled = false;
            ctrOutput.Clean();

            string remoteHost = txtRemoteHost.Text;
            int remotePort = Convert.ToInt32(txtRemotePort.Text);
            int localPort = Convert.ToInt32(txtLocalPort.Text);

            Thread thread = new Thread(() =>
            {
                TunnelTCP(remoteHost, remotePort, localPort);
            });
            thread.Start();
        }

        private void TunnelTCP(string remoteHost, int remotePort, int localPort)
        {
            try
            {
                Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sock.Bind(new IPEndPoint(IPAddress.Any, localPort));
                sock.Listen(1000);

                while (_running)
                {
                    if (sock.Poll(100, SelectMode.SelectRead))
                    {
                        Socket sockCliente = sock.Accept();
                        sockCliente.Blocking = false;
                        sockCliente.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);

                        ConnectedClient client = new ConnectedClient
                        {
                            RemoteHost = remoteHost,
                            RemotePort = remotePort,
                            ClientSocket = sockCliente,
                        };
                        Thread thread = new Thread(() =>
                        {
                            ProcessClient(client);
                        });
                        thread.Start();
                    }
                }

                sock.Close();
            }
            catch (Exception ex)
            {
                ctrOutput.AddLine("Exception: " + ex.Message);
                ctrOutput.AddLine("Backtrace:");
                ctrOutput.AddLine(ex.StackTrace);
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

                // Info
                ctrOutput.AddLine(
                    DateTime.Now.ToString("s") +
                    " Nuevo Cliente: " +
                    ((IPEndPoint)clientSock.RemoteEndPoint).Address);

                // Conectar al host remoto
                IPHostEntry entryHostRemoto = Dns.GetHostEntry(client.RemoteHost);
                IPAddress ipRemoteHost = null;
                foreach (IPAddress address in entryHostRemoto.AddressList)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipRemoteHost = address;
                        break;
                    }
                }

                if (ipRemoteHost == null) { return; }

                IPEndPoint endPointRemoteHost = new IPEndPoint(ipRemoteHost, client.RemotePort);
                Socket remoteSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                remoteSock.Connect(endPointRemoteHost);


                // Bucle de recepción y envío entre el cliente y el servidor remoto
                bool threadRunning = true;
                while (threadRunning && _running)
                {
                    // Comprobar recepción del cliente
                    int len;
                    if (clientSock.Poll(100, SelectMode.SelectRead))
                    {
                        if (clientSock.Available == 0)
                        {
                            // Cliente cierra conexión
                            threadRunning = false;
                        }
                        else
                        {
                            // Obtener y enviar al remoto
                            len = clientSock.Receive(buffer, 0, buffer.Length, 0);
                            int enviado = 0;
                            do
                            {
                                try
                                {
                                    enviado = remoteSock.Send(buffer, 0, len, 0);
                                }
                                catch (Exception ex)
                                {
                                    ctrOutput.AddLine("No se pudo enviar... (" + ex.Message + ")");
                                }

                                totalReceived += enviado;
                            } while (enviado <= 0 && remoteSock.Connected);
                        }
                    }

                    // Comprobar recepción del remoto
                    if (remoteSock.Poll(100, SelectMode.SelectRead))
                    {
                        if (remoteSock.Available == 0)
                        {
                            // Remoto cierra conexión
                            threadRunning = false;
                        }
                        else
                        {
                            // Obtener y enviar al cliente
                            len = remoteSock.Receive(buffer, 0, buffer.Length, 0);
                            int enviado = 0;
                            do
                            {
                                try
                                {
                                    enviado = clientSock.Send(buffer, 0, len, 0);
                                }
                                catch (Exception ex)
                                {
                                    ctrOutput.AddLine("No se pudo enviar... (" + ex.Message + ")");
                                    Thread.Sleep(10);
                                }

                                totalSent += enviado;
                            } while (enviado <= 0 && clientSock.Connected);
                        }
                    }
                }

                // Info
                ctrOutput.AddLine(
                    DateTime.Now.ToString("s") +
                    " Client disconnection: " +
                    ((IPEndPoint)clientSock.RemoteEndPoint).Address +
                    " IN/OUT: " + UnidadesBytes(totalReceived) + "/" + UnidadesBytes(totalSent));

                // Cerrar conexiones
                remoteSock.Close();
                clientSock.Close();
            }
            catch (Exception ex)
            {
                ctrOutput.AddLine("Exception: " + ex.Message);
                ctrOutput.AddLine("Backtrace:");
                ctrOutput.AddLine(ex.StackTrace);
            }
        }

        public static byte[] StrToByteArray(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }

        private static string UnidadesBytes(long n)
        {
            double number = n;
            if (number < 1024)
            {
                return $"{number} B";
            }

            number /= 1024.0;
            if (number < 1024)
            {
                return $"{number:#.00} KiB";
            }

            number /= 1024.0;
            if (number < 1024)
            {
                return $"{number:#.00} MiB";
            }

            number /= 1024.0;
            if (number < 1024)
            {
                return $"{number:#.00} GiB";
            }

            number /= 1024.0;

            return $"{number:#.00} TiB";
        }
    }
}