using System.Diagnostics;

namespace VAR.Toolbox.Code.ProxyCmdExecutors
{
    public class ProxyCmdExecutorWMIC : IProxyCmdExecutor
    {
        public string Name => "WMIC";

        private readonly string _configWMIC;

        public ProxyCmdExecutorWMIC(string configWMIC)
        {
            _configWMIC = configWMIC;
        }

        public bool ExecuteCmd(string cmd, IOutputHandler outputHandler)
        {
            string parameters =
                $" /node:\"{_configWMIC.Replace("\"", "\\\"")}\" process call create \"cmd.exe /c \\\"{cmd.Replace("\"", "\\\"")}\\\"\"";
            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = "WMIC";
            process.StartInfo.Arguments = parameters;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            return true;
        }
    }
}