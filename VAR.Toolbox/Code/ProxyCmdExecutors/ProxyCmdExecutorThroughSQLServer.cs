using System;
using Microsoft.Data.SqlClient;

namespace VAR.Toolbox.Code.ProxyCmdExecutors;

public class ProxyCmdExecutorThroughSQLServer : BaseProxyCmdExecutor
{
    public override string Name => "SqlServer";

    private readonly string _connectionString;

    public ProxyCmdExecutorThroughSQLServer(string connectionString)
    {
        _connectionString = connectionString;
    }

    public override bool ExecuteCmd(string cmdString, IOutputHandler outputHandler)
    {
        SqlConnection cnx = new(_connectionString);
        SqlCommand cmd = cnx.CreateCommand();
        cmd.CommandText = "exec master.dbo.xp_cmdshell @cmd";
        cmd.Parameters.Add(new SqlParameter("cmd", cmdString));
        cnx.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            string output = Convert.ToString(reader[0]) ?? string.Empty;
            outputHandler.AddLine(output);
        }

        cnx.Close();
        return true;
    }

    public override bool Enable()
    {
        try
        {
            SqlConnection cnx = new(_connectionString);
            SqlCommand cmd = cnx.CreateCommand();
            cmd.CommandText = @"
                    EXEC sp_configure 'show advanced options', '1'
                    RECONFIGURE
                    EXEC sp_configure 'xp_cmdshell', '1' 
                    RECONFIGURE
                ";
            cnx.Open();
            cmd.ExecuteNonQuery();
            cnx.Close();
            return true;
        }
        catch (Exception ex)
        {
            Logger.Log(ex);
            return false;
        }
    }

    public override bool Disable()
    {
        try
        {
            SqlConnection cnx = new(_connectionString);
            SqlCommand cmd = cnx.CreateCommand();
            cmd.CommandText = @"
                    EXEC sp_configure 'show advanced options', '1'
                    RECONFIGURE
                    EXEC sp_configure 'xp_cmdshell', '0' 
                    RECONFIGURE
                ";
            cnx.Open();
            cmd.ExecuteNonQuery();
            cnx.Close();
            return true;
        }
        catch (Exception ex)
        {
            Logger.Log(ex);
            return false;
        }
    }
}