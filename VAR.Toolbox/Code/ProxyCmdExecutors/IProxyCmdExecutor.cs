namespace VAR.Toolbox.Code.ProxyCmdExecutors
{
    public interface IProxyCmdExecutor
    {
        string Name { get; }

        bool ExecuteCmd(string cmd, IOutputHandler outputHandler);
    }
}
