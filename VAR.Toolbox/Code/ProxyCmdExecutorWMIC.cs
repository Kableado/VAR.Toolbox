using System.Diagnostics;

namespace VAR.Toolbox.Code
{
    public class ProxyCmdExecutorWMIC : IProxyCmdExecutor
    {
        private string _configWMIC;

        public ProxyCmdExecutorWMIC(string configWMIC)
        {
            _configWMIC = configWMIC;
        }

        public bool ExecuteCmd(string cmd, IOutputHandler outputHandler)
        {
            string parameters = string.Format(" /node:\"{0}\" process call create \"cmd.exe /c \\\"{1}\\\"\"", _configWMIC.Replace("\"", "\\\""), cmd.Replace("\"", "\\\""));
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