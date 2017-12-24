namespace VAR.Toolbox.Code
{
    public interface IProxyCmdExecutor
    {
        bool ExecuteCmd(string cmd, IOutputHandler outputHandler);
    }
}
