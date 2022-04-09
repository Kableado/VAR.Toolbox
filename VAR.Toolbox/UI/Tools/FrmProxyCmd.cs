#pragma warning disable IDE0019

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using VAR.Toolbox.Code;
using VAR.Toolbox.Code.ProxyCmdExecutors;
using VAR.Toolbox.Controls;

namespace VAR.Toolbox.UI.Tools
{
    public partial class FrmProxyCmd : Frame, IOutputHandler, IToolForm
    {
        public string ToolName => "ProxyCmd";

        public bool HasIcon => false;

        #region Declarations

        private readonly object _executionLock = new object();

        private readonly List<string> _cmdHistory = new List<string>();
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

        private void TxtInput_KeyDown(object sender, KeyEventArgs e)
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
                    AddLine(cmd);
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
            }
        }

        private void DdlCurrentConfig_SelectedIndexChanged(object sender, EventArgs e)
        {
            CleanProxyCmdExecutor();
        }

        private void BtnConfig_Click(object sender, EventArgs e)
        {
            FrmToolbox.StaticCreateWindow(typeof(FrmProxyCmdConfig));
        }

        #endregion UI events

        #region ProxyCmdExecutor

        private IProxyCmdExecutor _proxyCmdExecutor;

        private void PrepareProxyCmdExecutor()
        {
            if (_proxyCmdExecutor == null)
            {
                _proxyCmdExecutor = ProxyCmdExecutorFactory.CreateFromConfig(GetCurrentConfig()) ??
                                    new ProxyCmdExecutorDummy(string.Empty);
            }
        }

        private void CleanProxyCmdExecutor()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (_proxyCmdExecutor is IDisposable disposableProxyCmdExecutor)
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
                AddLine(ex.Message);
            }

            Monitor.Exit(_executionLock);
        }

        #endregion Private methods

        #region IOutputHandler
        
        public void Clean()
        {
            BeginInvoke(new MethodInvoker(delegate
            {
                ctrOutput.Clean();
                Application.DoEvents();
            }));
        }

        public void AddLine(string line, object data = null)
        {
            BeginInvoke(new MethodInvoker(delegate
            {
                ctrOutput.AddLine(line, data);
                Application.DoEvents();
            }));
        }

        #endregion IOutputHandler
        
        #region Config

        public void LoadConfig()
        {
            CleanProxyCmdExecutor();

            List<ProxyCmdConfigItem> configItems = FrmProxyCmdConfig.GetConfigurationItems();

            string previousSelectedName = null;
            if (ddlCurrentConfig.SelectedItem is ProxyCmdConfigItem selectedConfig)
            {
                previousSelectedName = selectedConfig.Name;
            }

            ddlCurrentConfig.Items.Clear();
            ddlCurrentConfig.Items.AddRange(configItems.ToArray<object>());
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
            var selectedConfig = ddlCurrentConfig.SelectedItem as ProxyCmdConfigItem;
            if (selectedConfig == null) { return null; }

            return selectedConfig.Config;
        }

        #endregion Config
    }
}