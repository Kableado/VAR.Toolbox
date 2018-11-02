using System;
using System.Collections.Generic;
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
            if (e.KeyCode == Keys.Enter)
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
                if (_currentHistoryIndex >= 0 && _currentHistoryIndex < _cmdHistory.Count)
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
            FrmToolbox.StaticCreateWindow(typeof(FrmProxyCmdConfig));
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
            if (disposableProxyCmdExecutor != null)
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
            
            List<ProxyCmdConfigItem> configItems = FrmProxyCmdConfig.GetConfigurationItems();

            string previousSelectedName = null;
            ProxyCmdConfigItem selectedConfig = ddlCurrentConfig.SelectedItem as ProxyCmdConfigItem;
            if (selectedConfig != null) { previousSelectedName = selectedConfig.Name; }
            ddlCurrentConfig.Items.Clear();
            ddlCurrentConfig.Items.AddRange(configItems.ToArray());
            ddlCurrentConfig.SelectedIndex = 0;
            if (string.IsNullOrEmpty(previousSelectedName) == false)
            {
                foreach (ProxyCmdConfigItem configItem in ddlCurrentConfig.Items)
                {
                    if (configItem.Name == previousSelectedName)
                    {
                        ddlCurrentConfig.SelectedItem = configItem;
                        break;
                    }
                }
            }
        }

        private string GetCurrentConfig()
        {
            ProxyCmdConfigItem selectedConfig = ddlCurrentConfig.SelectedItem as ProxyCmdConfigItem;
            if (selectedConfig == null) { return null; }
            return selectedConfig.Config;
        }

        #endregion Config
    }
}
