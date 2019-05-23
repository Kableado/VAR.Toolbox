using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;

namespace VAR.Toolbox.UI
{
    public partial class FrmNetworkInfo : Form, IToolForm
    {
        public string ToolName { get { return "NetworkInfo"; } }

        public FrmNetworkInfo()
        {
            InitializeComponent();

            RefreshInterfaces();
            ddlNetworkInterfaces.SelectedIndex = 0;
        }

        private void FrmNetworkInfo_Load(object sender, EventArgs e)
        {
            timRefresh.Enabled = true;
            timRefresh.Start();
        }

        private void FrmNetworkInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            timRefresh.Enabled = false;
            timRefresh.Stop();
        }

        private void ddlNetworkInterfaces_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem listItem = ddlNetworkInterfaces.SelectedItem as ListItem;
            if (listItem == null) { return; }
            RefreshInterface(listItem.ID);
        }

        private void timRefresh_Tick(object sender, EventArgs e)
        {
            RefreshInterfaces();
            RefreshInterface();
        }

        private void RefreshInterfaces()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                bool found = false;
                foreach (ListItem it in ddlNetworkInterfaces.Items)
                {
                    if (it.ID == nic.Id)
                    {
                        found = true;
                        if (it.Text != nic.Description) { it.Text = nic.Description; }
                        break;
                    }
                }
                if (found == false)
                {
                    ddlNetworkInterfaces.Items.Add(new ListItem { ID = nic.Id, Text = nic.Description });
                }
            }
        }

        private string _networkInterfaceId = null;

        private void RefreshInterface()
        {
            RefreshInterface(_networkInterfaceId);
        }

        private void RefreshInterface(string id)
        {
            _networkInterfaceId = id;
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.Id == id)
                {
                    RefreshInterface(nic);
                    break;
                }
            }
        }

        private void RefreshInterface(NetworkInterface nic)
        {
            if (txtID.Text != nic.Id) { txtID.Text = nic.Id; }
            if (txtName.Text != nic.Name) { txtName.Text = nic.Name; }
            string status = nic.OperationalStatus.ToString();
            if (txtStatus.Text != status) { txtStatus.Text = status; }
            string speed = FormatNetworkSpeedUnits(nic.Speed);
            if (txtSpeed.Text != speed) { txtSpeed.Text = speed; }
            if (txtDescription.Text != nic.Description) { txtDescription.Text = nic.Description; }
            string strInterface = nic.NetworkInterfaceType.ToString();
            if (txtInterface.Text != strInterface) { txtInterface.Text = strInterface; }
            string strMac = FormatPhysicalAddress(nic.GetPhysicalAddress().GetAddressBytes());
            if (txtMAC.Text != strMac) { txtMAC.Text = strMac; }
            StringBuilder sbIPs = new StringBuilder();
            IPInterfaceProperties ipInterfaceProperties = nic.GetIPProperties();
            sbIPs.AppendLine("****** IPs *****");
            foreach (UnicastIPAddressInformation uniAddress in ipInterfaceProperties.UnicastAddresses)
            {
                sbIPs.AppendLine(uniAddress.Address.ToString());
            }
            sbIPs.AppendLine();
            sbIPs.AppendLine("****** Gateways *****");
            foreach (GatewayIPAddressInformation gwAddress in ipInterfaceProperties.GatewayAddresses)
            {
                sbIPs.AppendLine(gwAddress.Address.ToString());
            }
            sbIPs.AppendLine();
            sbIPs.AppendLine("****** DNSs *****");
            foreach (IPAddress dnsAddress in ipInterfaceProperties.DnsAddresses)
            {
                sbIPs.AppendLine(dnsAddress.ToString());
            }
            string strIPs = sbIPs.ToString();
            if (txtIPs.Text != strIPs) { txtIPs.Text = strIPs; }
        }


        private static string FormatNetworkSpeedUnits(long speed)
        {
            decimal dSpeed;
            if (speed < 1000)
            {
                return string.Format("{0}bps", speed);
            }
            dSpeed = speed / (decimal)1000;
            if (dSpeed < 1000)
            {
                return string.Format("{0}kbps", Math.Round(dSpeed, 2));
            }
            dSpeed = dSpeed / 1000;
            if (dSpeed < 1000)
            {
                return string.Format("{0}mbps", Math.Round(dSpeed, 2));
            }
            dSpeed = dSpeed / 1000;
            if (dSpeed < 1000)
            {
                return string.Format("{0}gbps", Math.Round(dSpeed, 2));
            }
            dSpeed = dSpeed / 1000;
            return string.Format("{0}tbps", Math.Round(dSpeed, 2));
        }

        private static string FormatPhysicalAddress(byte[] address)
        {
            StringBuilder sbAddres = new StringBuilder();
            foreach (byte b in address)
            {
                if (sbAddres.Length > 0) { sbAddres.Append(":"); }
                sbAddres.AppendFormat("{0:X2}", b);
            }
            return sbAddres.ToString();
        }
    }

    public class ListItem
    {
        public string Text { get; set; }
        public string ID { get; set; }

        public override string ToString() { return Text; }

    }
}

