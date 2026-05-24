#pragma warning disable IDE0019

using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Threading;

namespace VAR.Toolbox.UI.Tools
{
    public class FrmNetworkInfo : Window, IToolForm
    {
        public string ToolName => "NetworkInfo";
        public bool HasIcon => false;

        private readonly ComboBox _ddlNetworkInterfaces;
        private readonly TextBox _txtID, _txtName, _txtStatus, _txtSpeed, _txtDescription, _txtInterface, _txtMac;
        private readonly TextBox _txtIPs;
        private readonly DispatcherTimer _timRefresh;

        public FrmNetworkInfo()
        {
            Title = "NetworkInfo";
            Width = 600;
            Height = 500;

            _ddlNetworkInterfaces = new ComboBox { HorizontalAlignment = HorizontalAlignment.Stretch, };
            _ddlNetworkInterfaces.SelectionChanged += DdlNetworkInterfaces_SelectionChanged;

            _txtID = new TextBox { Classes = { "mono", }, IsReadOnly = true, };
            _txtName = new TextBox { Classes = { "mono", }, IsReadOnly = true, };
            _txtStatus = new TextBox { Classes = { "mono", }, IsReadOnly = true, };
            _txtSpeed = new TextBox { Classes = { "mono", }, IsReadOnly = true, };
            _txtDescription = new TextBox { Classes = { "mono", }, IsReadOnly = true, };
            _txtInterface = new TextBox { Classes = { "mono", }, IsReadOnly = true, };
            _txtMac = new TextBox { Classes = { "mono", }, IsReadOnly = true, };
            _txtIPs = new TextBox { Classes = { "mono", }, IsReadOnly = true, AcceptsReturn = true, Height = 200, };

            StackPanel fieldsPanel = new() { Spacing = 4, };
            fieldsPanel.Children.Add(_ddlNetworkInterfaces);
            fieldsPanel.Children.Add(MakeRow("ID", _txtID));
            fieldsPanel.Children.Add(MakeRow("Name", _txtName));
            fieldsPanel.Children.Add(MakeRow("Status", _txtStatus));
            fieldsPanel.Children.Add(MakeRow("Speed", _txtSpeed));
            fieldsPanel.Children.Add(MakeRow("Description", _txtDescription));
            fieldsPanel.Children.Add(MakeRow("Interface", _txtInterface));
            fieldsPanel.Children.Add(MakeRow("MAC", _txtMac));
            fieldsPanel.Children.Add(_txtIPs);

            Content = new ScrollViewer { Content = new Border { Padding = new Thickness(8), Child = fieldsPanel, }, };

            RefreshInterfaces();
            if (_ddlNetworkInterfaces.ItemCount > 0)
                _ddlNetworkInterfaces.SelectedIndex = 0;

            _timRefresh = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2), };
            _timRefresh.Tick += TimRefresh_Tick;

            Opened += (_, _) => { _timRefresh.Start(); };
            Closing += (_, _) => { _timRefresh.Stop(); };
        }

        private static Control MakeRow(string label, Control control)
        {
            DockPanel row = new();
            TextBlock lbl = new() { Text = label, Width = 80, VerticalAlignment = VerticalAlignment.Center, };
            DockPanel.SetDock(lbl, Dock.Left);
            row.Children.Add(lbl);
            row.Children.Add(control);
            return row;
        }

        private void DdlNetworkInterfaces_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            ListItem? listItem = _ddlNetworkInterfaces.SelectedItem as ListItem;
            if (listItem == null) { return; }
            RefreshInterface(listItem.ID);
        }

        private void TimRefresh_Tick(object? sender, EventArgs e)
        {
            RefreshInterfaces();
            RefreshInterface();
        }

        private void RefreshInterfaces()
        {
            ObservableCollection<ListItem>? items = _ddlNetworkInterfaces.ItemsSource as ObservableCollection<ListItem>;
            if (items == null)
            {
                items = [];
                _ddlNetworkInterfaces.ItemsSource = items;
            }

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                bool found = false;
                foreach (ListItem it in items)
                {
                    if (it.ID == nic.Id) { found = true; break; }
                }
                if (!found)
                {
                    items.Add(new ListItem { ID = nic.Id, Text = nic.Description, });
                }
            }
        }

        private string _networkInterfaceId = string.Empty;

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
            _txtID.Text = nic.Id;
            _txtName.Text = nic.Name;
            _txtStatus.Text = nic.OperationalStatus.ToString();
            _txtSpeed.Text = FormatNetworkSpeedUnits(nic.Speed);
            _txtDescription.Text = nic.Description;
            _txtInterface.Text = nic.NetworkInterfaceType.ToString();
            _txtMac.Text = FormatPhysicalAddress(nic.GetPhysicalAddress().GetAddressBytes());

            StringBuilder sbIPs = new();
            IPInterfaceProperties ipProps = nic.GetIPProperties();
            sbIPs.AppendLine("****** IPs *****");
            foreach (UnicastIPAddressInformation uniAddress in ipProps.UnicastAddresses)
                sbIPs.AppendLine(uniAddress.Address.ToString());
            sbIPs.AppendLine();
            sbIPs.AppendLine("****** Gateways *****");
            foreach (GatewayIPAddressInformation gwAddress in ipProps.GatewayAddresses)
                sbIPs.AppendLine(gwAddress.Address.ToString());
            sbIPs.AppendLine();
            sbIPs.AppendLine("****** DNSs *****");
            foreach (IPAddress dnsAddress in ipProps.DnsAddresses)
                sbIPs.AppendLine(dnsAddress.ToString());
            _txtIPs.Text = sbIPs.ToString();
        }

        private static string FormatNetworkSpeedUnits(long speed)
        {
            if (speed < 1000) return $"{speed}bps";
            decimal dSpeed = speed / (decimal)1000;
            if (dSpeed < 1000) return $"{Math.Round(dSpeed, 2)}kbps";
            dSpeed /= 1000;
            if (dSpeed < 1000) return $"{Math.Round(dSpeed, 2)}mbps";
            dSpeed /= 1000;
            if (dSpeed < 1000) return $"{Math.Round(dSpeed, 2)}gbps";
            dSpeed /= 1000;
            return $"{Math.Round(dSpeed, 2)}tbps";
        }

        private static string FormatPhysicalAddress(byte[] address)
        {
            StringBuilder sbAddress = new();
            foreach (byte b in address)
            {
                if (sbAddress.Length > 0) sbAddress.Append(":");
                sbAddress.Append($"{b:X2}");
            }
            return sbAddress.ToString();
        }
    }

    public class ListItem
    {
        public string Text { get; set; } = string.Empty;
        public string ID { get; set; } = string.Empty;
        public override string ToString()
        {
            return Text;
        }
    }
}