using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Windows.Forms;

namespace VAR.Toolbox
{
    public partial class FrmProxyCmd : Form
    {
        public FrmProxyCmd()
        {
            InitializeComponent();
        }

        private void txtOutput_AppendLine(string line)
        {
            BeginInvoke(new MethodInvoker(delegate
            {
                txtOutput.AppendText(line);
                txtOutput.AppendText(Environment.NewLine);
                Application.DoEvents();
            }));
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
                Application.DoEvents();
                string cmd = txtInput.Text.Replace("\n", "").Replace("\r", "");
                txtOutput_AppendLine(cmd);
                txtInput.Text = string.Empty;
                new Thread(() => ExecuteCmd(cmd)).Start();
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

        private void ExecuteCmd(string cmdString)
        {
            Monitor.Enter(_executionLock);
            SqlConnection cnx = new SqlConnection(Properties.Settings.Default.ProxyCmdConfig);
            SqlCommand cmd = cnx.CreateCommand();
            cmd.CommandText = "exec master.dbo.xp_cmdshell @cmd";
            cmd.Parameters.Add(new SqlParameter("cmd", cmdString));
            cnx.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string output = Convert.ToString(reader[0]);
                txtOutput_AppendLine(output);
            }
            cnx.Close();
            Monitor.Exit(_executionLock);
        }
    }
}
