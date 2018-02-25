using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using VAR.Toolbox.Code;

namespace VAR.Toolbox.UI
{
    public partial class FrmProxyCmd : Form, IOutputHandler
    {
        #region Declarations

        private object _executionLock = new object();

        private List<string> _cmdHistory = new List<string>();
        private int _currentHistoryIndex = -1;

        #endregion Declarations

        #region Life cycle

        public FrmProxyCmd()
        {
            InitializeComponent();
            LoadConfig();
        }

        #endregion Life cycle

        #region UI events

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (Monitor.IsEntered(_executionLock))
            {
                e.Handled = true;
                Application.DoEvents();
                return;
            }
            if (e.KeyCode == Keys.Return)
            {
                e.Handled = true;
                string cmd = txtInput.Text.TrimStart().Replace("\n", "").Replace("\r", "");
                if (string.IsNullOrEmpty(cmd) == false)
                {
                    txtInput.Text = string.Empty;
                    Application.DoEvents();
                    txtInput.Text = string.Empty;
                    OutputLine(cmd);
                    PrepareProxyCmdExecutor();
                    new Thread(() => ExecuteCmd(cmd)).Start();
                }
                return;
            }
            if(e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                return;
            }
            if (e.KeyCode == Keys.LineFeed)
            {
                e.Handled = true;
                return;
            }
            if (e.KeyCode == Keys.Up)
            {
                e.Handled = true;
                if (_currentHistoryIndex == -1) { _currentHistoryIndex = _cmdHistory.Count; }
                _currentHistoryIndex--;
                if (_currentHistoryIndex < 0)
                {
                    _currentHistoryIndex = 0;
                }
                if (_currentHistoryIndex>=0 && _currentHistoryIndex < _cmdHistory.Count)
                {
                    txtInput.Text = _cmdHistory[_currentHistoryIndex];
                    txtInput.SelectionStart = txtInput.Text.Length;
                    txtInput.SelectionLength = 0;
                }
                return;
            }
            if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
                if (_currentHistoryIndex > -1)
                {
                    _currentHistoryIndex++;
                    if (_currentHistoryIndex >= _cmdHistory.Count)
                    {
                        txtInput.Text = string.Empty;
                        _currentHistoryIndex = -1;
                    }
                    else
                    {
                        txtInput.Text = _cmdHistory[_currentHistoryIndex];
                        txtInput.SelectionStart = txtInput.Text.Length;
                        txtInput.SelectionLength = 0;
                    }
                }
                return;
            }
        }

        private void ddlCurrentConfig_SelectedIndexChanged(object sender, EventArgs e)
        {
            CleanProxyCmdExecutor();
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException("Implement btnConfig_Click");
        }

        #endregion UI events

        #region ProxyCmdExecutor

        private IProxyCmdExecutor _proxyCmdExecutor = null;

        private void PrepareProxyCmdExecutor()
        {
            if (_proxyCmdExecutor == null)
            {
                _proxyCmdExecutor = ProxyCmdExecutorFactory.CreateFromConfig(GetCurrentConfig());
            }
        }
        private void CleanProxyCmdExecutor()
        {
            IDisposable disposableProxyCmdExecutor = _proxyCmdExecutor as IDisposable;
            if (disposableProxyCmdExecutor !=null)
            {
                disposableProxyCmdExecutor.Dispose();
            }
            _proxyCmdExecutor = null;
        }

        #endregion ProxyCmdExecutor

        #region Private methods

        private void ExecuteCmd(string cmdString)
        {
            Monitor.Enter(_executionLock);
            try
            {
                _cmdHistory.Add(cmdString);
                _currentHistoryIndex = -1;
                _proxyCmdExecutor.ExecuteCmd(cmdString, this);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                OutputLine(ex.Message);
            }
            Monitor.Exit(_executionLock);
        }
        
        public void OutputLine(string line)
        {
            BeginInvoke(new MethodInvoker(delegate
            {
                txtOutput.AppendText(line);
                txtOutput.AppendText(Environment.NewLine);
                Application.DoEvents();
            }));
        }

        #endregion Private methods

        #region Config

        public void LoadConfig()
        {
            CleanProxyCmdExecutor();
            string configFile = GetConfigFileName();
            string[] config = null;
            if (File.Exists(configFile) == false)
            {
                config = new string[] { "Dummy|Dummy:" };
            }
            else
            {
                config = File.ReadAllLines(configFile);
            }
            SetLoadedConfig(config);
        }

        private void SetLoadedConfig(string[] configLines)
        {
            string previousSelectedName = null;
            ProxyCmdConfigItem selectedConfig = ddlCurrentConfig.SelectedItem as ProxyCmdConfigItem;
            if (selectedConfig != null) { previousSelectedName = selectedConfig.Name; }

            ddlCurrentConfig.Items.Clear();
            foreach (string configLine in configLines)
            {
                int idxSplit = configLine.IndexOf('|');
                if(idxSplit < 0) { continue; }
                string configName = configLine.Substring(0, idxSplit);
                string configData = configLine.Substring(idxSplit + 1);

                ddlCurrentConfig.Items.Add(new ProxyCmdConfigItem { Name = configName, Config = configData, });
            }
            if (string.IsNullOrEmpty(previousSelectedName))
            {
                ddlCurrentConfig.SelectedIndex = 0;
                return;
            }
            
            foreach(ProxyCmdConfigItem configItem in ddlCurrentConfig.Items)
            {
                if(configItem.Name == previousSelectedName)
                {
                    ddlCurrentConfig.SelectedItem = configItem;
                    break;
                }
            }
        }

        private string GetConfigFileName()
        {
            string location = System.Reflection.Assembly.GetEntryAssembly().Location;
            string path = Path.GetDirectoryName(location);
            string filenameWithoutExtension = Path.GetFileNameWithoutExtension(location);

            string configFile = string.Format("{0}/{1}.ProxyCmd.cfg", path, filenameWithoutExtension);
            return configFile;
        }

        private string GetCurrentConfig()
        {
            ProxyCmdConfigItem selectedConfig = ddlCurrentConfig.SelectedItem as ProxyCmdConfigItem;
            if (selectedConfig == null) { return null; }
            return selectedConfig.Config;
        }

        #endregion Config
    }

    public class ProxyCmdConfigItem
    {
        public string Name { get; set; }
        public string Config { get; set; }
        public override string ToString() { return Name; }
    }
}
