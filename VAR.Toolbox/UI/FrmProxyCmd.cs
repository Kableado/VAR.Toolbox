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
                string cmd = txtInput.Text.Replace("\n", "").Replace("\r", "");
                txtInput.Text = string.Empty;
                Application.DoEvents();
                txtInput.Text = string.Empty;
                OutputLine(cmd);
                new Thread(() => ExecuteCmd(cmd)).Start();
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
                return;
            }
            if (e.KeyCode == Keys.Down)
            {
                e.Handled = true;
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
