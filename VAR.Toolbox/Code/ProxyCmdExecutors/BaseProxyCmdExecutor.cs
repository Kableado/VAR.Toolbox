using System;

namespace VAR.Toolbox.Code.ProxyCmdExecutors;

public abstract class BaseProxyCmdExecutor : IProxyCmdExecutor
{
    public abstract string Name { get; }

    public virtual bool Disable()
    {
        throw new NotImplementedException();
    }

    public virtual bool Enable()
    {
        return true;
    }

    public abstract bool ExecuteCmd(string cmd, IOutputHandler outputHandler);
}