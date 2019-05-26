using System;
using System.Data.SqlClient;

namespace VAR.Toolbox.Code.ProxyCmdExecutors
{
    public class ProxyCmdExecutorThroughSQLServer : IProxyCmdExecutor
    {
        public string Name { get { return "SqlServer"; } }

        private readonly string _connectionString = null;

        public ProxyCmdExecutorThroughSQLServer(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool ExecuteCmd(string cmdString, IOutputHandler outputHandler)
        {
            SqlConnection cnx = new SqlConnection(_connectionString);
            SqlCommand cmd = cnx.CreateCommand();
            cmd.CommandText = "exec master.dbo.xp_cmdshell @cmd";
            cmd.Parameters.Add(new SqlParameter("cmd", cmdString));
            cnx.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string output = Convert.ToString(reader[0]);
                outputHandler.OutputLine(output);
            }
            cnx.Close();
            return true;
        }
    }
}
