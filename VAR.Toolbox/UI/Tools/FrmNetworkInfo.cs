﻿#pragma warning disable IDE0019

using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;
using VAR.Toolbox.Controls;

namespace VAR.Toolbox.UI.Tools
{
    public partial class FrmNetworkInfo : Frame, IToolForm
    {
        public string ToolName => "NetworkInfo";

        public bool HasIcon => false;

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

        private void DdlNetworkInterfaces_SelectedIndexChanged(object sender, EventArgs e)
        {
            var listItem = ddlNetworkInterfaces.SelectedItem as ListItem;
            if (listItem == null) { return; }

            RefreshInterface(listItem.ID);
        }

        private void TimRefresh_Tick(object sender, EventArgs e)
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

        private string _networkInterfaceId;

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
            if (speed < 1000)
            {
                return $"{speed}bps";
            }

            decimal dSpeed = speed / (decimal)1000;
            if (dSpeed < 1000)
            {
                return $"{Math.Round(dSpeed, 2)}kbps";
            }

            dSpeed /= 1000;
            if (dSpeed < 1000)
            {
                return $"{Math.Round(dSpeed, 2)}mbps";
            }

            dSpeed /= 1000;
            if (dSpeed < 1000)
            {
                return $"{Math.Round(dSpeed, 2)}gbps";
            }

            dSpeed /= 1000;
            return $"{Math.Round(dSpeed, 2)}tbps";
        }

        private static string FormatPhysicalAddress(byte[] address)
        {
            StringBuilder sbAddress = new StringBuilder();
            foreach (byte b in address)
            {
                if (sbAddress.Length > 0) { sbAddress.Append(":"); }

                sbAddress.AppendFormat("{0:X2}", b);
            }

            return sbAddress.ToString();
        }
    }

    public class ListItem
    {
        public string Text { get; set; }
        public string ID { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}