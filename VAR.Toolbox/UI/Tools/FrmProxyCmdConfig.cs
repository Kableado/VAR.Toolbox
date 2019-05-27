#pragma warning disable IDE0019

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace VAR.Toolbox.UI
{
    public partial class FrmProxyCmdConfig : Form
    {
        public FrmProxyCmdConfig()
        {
            InitializeComponent();

            LoadData();
        }


        private void LsvCmdProxyConfigs_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProxyCmdConfigItem selectedConfig = lsvCmdProxyConfigs.SelectedItem as ProxyCmdConfigItem;
            if (selectedConfig == null) { CleanConfig(); return; }
            ShowConfig(selectedConfig);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DeleteSelected();
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            CreateNew();
        }

        private void LoadData()
        {
            lsvCmdProxyConfigs.Items.Clear();
            List<ProxyCmdConfigItem> configItems = FrmProxyCmdConfig.GetConfigurationItems();
            lsvCmdProxyConfigs.Items.AddRange(configItems.ToArray());
        }

        private void SaveData()
        {
            StringBuilder sbConfig = new StringBuilder();
            foreach (object o in lsvCmdProxyConfigs.Items)
            {
                ProxyCmdConfigItem config = o as ProxyCmdConfigItem;
                if (config == null) { continue; }
                sbConfig.AppendFormat("{0}|{1}\n", config.Name, config.Config);
            }
            string configFileName = GetConfigFileName();
            File.WriteAllText(configFileName, sbConfig.ToString());

            List<FrmProxyCmd> listForms = FrmToolbox.StaticGetWindowsOfType<FrmProxyCmd>();
            foreach (FrmProxyCmd frm in listForms)
            {
                frm.LoadConfig();
            }
        }

        private void CleanConfig()
        {
            txtCmdProxyConfigName.Text = string.Empty;
            txtCmdProxyConfigContent.Text = string.Empty;
        }

        private void ShowConfig(ProxyCmdConfigItem config)
        {
            txtCmdProxyConfigName.Text = config.Name;
            txtCmdProxyConfigContent.Text = config.Config;
        }

        private void SaveConfig()
        {
            ProxyCmdConfigItem selectedConfig = lsvCmdProxyConfigs.SelectedItem as ProxyCmdConfigItem;
            if (selectedConfig == null)
            {
                selectedConfig = new ProxyCmdConfigItem
                {
                    Name = txtCmdProxyConfigName.Text,
                    Config = txtCmdProxyConfigContent.Text
                };
                lsvCmdProxyConfigs.Items.Add(selectedConfig);
            }
            else
            {
                selectedConfig.Name = txtCmdProxyConfigName.Text;
                selectedConfig.Config = txtCmdProxyConfigContent.Text;
            }
            SaveData();
        }

        private void DeleteSelected()
        {
            ProxyCmdConfigItem selectedConfig = lsvCmdProxyConfigs.SelectedItem as ProxyCmdConfigItem;
            if (selectedConfig == null) { return; }
            List<ProxyCmdConfigItem> configItems = new List<ProxyCmdConfigItem>();
            foreach (object o in lsvCmdProxyConfigs.Items)
            {
                ProxyCmdConfigItem config = o as ProxyCmdConfigItem;
                if (config == null) { continue; }
                if (config.Name == selectedConfig.Name) { continue; }
                configItems.Add(config);
            }
            lsvCmdProxyConfigs.Items.Clear();
            lsvCmdProxyConfigs.Items.AddRange(configItems.ToArray());
            SaveData();
            CleanConfig();
        }

        private void CreateNew()
        {
            lsvCmdProxyConfigs.SelectedIndex = -1;
            CleanConfig();
        }

        public static string GetConfigFileName()
        {
            string location = System.Reflection.Assembly.GetEntryAssembly().Location;
            string path = Path.GetDirectoryName(location);
            string filenameWithoutExtension = Path.GetFileNameWithoutExtension(location);

            string configFile = string.Format("{0}/{1}.ProxyCmd.cfg", path, filenameWithoutExtension);
            return configFile;
        }

        public static string[] GetConfigurationLines()
        {
            string configFile = GetConfigFileName();
            string[] config;
            if (File.Exists(configFile) == false)
            {
                config = new string[] { "Dummy|Dummy:" };
            }
            else
            {
                config = File.ReadAllLines(configFile);
            }
            return config;
        }

        public static List<ProxyCmdConfigItem> GetConfigurationItems()
        {
            string[] configLines = GetConfigurationLines();
            List<ProxyCmdConfigItem> configItems = new List<ProxyCmdConfigItem>();
            foreach (string configLine in configLines)
            {
                int idxSplit = configLine.IndexOf('|');
                if (idxSplit < 0) { continue; }
                string configName = configLine.Substring(0, idxSplit);
                string configData = configLine.Substring(idxSplit + 1);

                configItems.Add(new ProxyCmdConfigItem { Name = configName, Config = configData, });
            }
            return configItems;
        }
    }

    public class ProxyCmdConfigItem
    {
        public string Name { get; set; }
        public string Config { get; set; }
        public override string ToString() { return Name; }
    }
}
