using System;
using System.Threading;
using System.Windows.Forms;
using VAR.Toolbox.Code;

namespace VAR.Toolbox.UI
{
    public partial class FrmProxyCmd : Form, IOutputHandler
    {
        public FrmProxyCmd()
        {
            InitializeComponent();
        }
        
        private object _executionLock = new object();

        private List<string> _cmdHistory = new List<string>();
        private int _currentHistoryIndex = -1;

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

        private IProxyCmdExecutor _proxyCmdExecutor = null;

        private void ExecuteCmd(string cmdString)
        {
            if (_proxyCmdExecutor == null)
            {
                _proxyCmdExecutor = ProxyCmdExecutorFactory.CreateFromConfig(Properties.Settings.Default.ProxyCmdConfig);
            }
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
    }
}
