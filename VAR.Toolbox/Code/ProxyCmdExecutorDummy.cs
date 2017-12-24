namespace VAR.Toolbox.Code
{
    public class ProxyCmdExecutorDummy : IProxyCmdExecutor
    {
        public bool ExecuteCmd(string cmdString, IOutputHandler outputHandler)
        {
            outputHandler.OutputLine(string.Format("DummyExecution: {0}", cmdString));
            return true;
        }
    }
}
