namespace VAR.Toolbox.Code.ProxyCmdExecutors
{
    public interface IProxyCmdExecutor : INamed
    {
        bool ExecuteCmd(string cmd, IOutputHandler outputHandler);
    }
}