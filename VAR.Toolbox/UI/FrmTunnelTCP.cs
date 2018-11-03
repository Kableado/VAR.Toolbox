using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace VAR.Toolbox.UI
{
    public partial class FrmTunnelTCP : Form
    {
        private bool _running = false;

        private class ConnectedClient
        {
            public string hostRemoto = null;
            public int puertoRemoto = 0;
            public Socket socketCliente = null;
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

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (_running == false) { return; }

            _running = false;
            btnStop.Enabled = false;
            btnRun.Enabled = true;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            if (_running == true) { return; }

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
                Socket sock;

                sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
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
                            hostRemoto = remoteHost,
                            puertoRemoto = remotePort,
                            socketCliente = sockCliente,
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
                ctrOutput.AddLine("Excepcion: " + ex.Message);
                ctrOutput.AddLine("Backtrace:");
                ctrOutput.AddLine(ex.StackTrace);
            }
        }

        private void ProcessClient(ConnectedClient client)
        {
            try
            {
                Socket remoteSock = null;
                Socket clientSock = client.socketCliente;
                bool threadRunning;
                byte[] buffer = new byte[4096];
                int len;
                long totalSent = 0;
                long totalRecv = 0;

                // Info
                ctrOutput.AddLine(
                    DateTime.Now.ToString() +
                    " Nuevo Cliente: " +
                    ((IPEndPoint)clientSock.RemoteEndPoint).Address);

                // Conectar al host remoto
                IPHostEntry EntryHostRemoto = Dns.GetHostEntry(client.hostRemoto);
                IPAddress ipHostRemoto = null;
                foreach (IPAddress addr in EntryHostRemoto.AddressList)
                {
                    if (addr.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipHostRemoto = addr;
                        break;
                    }
                }
                IPEndPoint endpHostRemoto = new IPEndPoint(ipHostRemoto, client.puertoRemoto);
                remoteSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                remoteSock.Connect(endpHostRemoto);

                // Bucle de recepcion y envio entre el cliente y el servidor remoto
                threadRunning = true;
                while (threadRunning && _running)
                {
                    // Comprobar recepcion del cliente
                    if (clientSock.Poll(100, SelectMode.SelectRead))
                    {
                        if (clientSock.Available == 0)
                        {
                            // Cliente cierra conexion
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
                                totalRecv += enviado;
                            } while (enviado <= 0 && remoteSock.Connected);
                        }
                    }

                    // Comprobar recepcion del remoto
                    if (remoteSock.Poll(100, SelectMode.SelectRead))
                    {
                        if (remoteSock.Available == 0)
                        {
                            // Remoto cierra conexion
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
                    DateTime.Now.ToString() +
                    " Desconexion de cliente: " +
                    ((IPEndPoint)clientSock.RemoteEndPoint).Address +
                    " IN/OUT: " + UnidadesBytes(totalRecv) + "/" + UnidadesBytes(totalSent));

                // Cerrar conexiones
                remoteSock.Close();
                clientSock.Close();
            }
            catch (Exception ex)
            {
                ctrOutput.AddLine("Excepcion: " + ex.Message);
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
                return string.Format("{0} B", number);
            }

            number = number / 1024.0;
            if (number < 1024)
            {
                return string.Format("{0:#.00} KiB", number);
            }

            number = number / 1024.0;
            if (number < 1024)
            {
                return string.Format("{0:#.00} MiB", number);
            }

            number = number / 1024.0;
            if (number < 1024)
            {
                return string.Format("{0:#.00} GiB", number);
            }

            number = number / 1024.0;

            return string.Format("{0:#.00} TiB", number);
        }

    }
}
