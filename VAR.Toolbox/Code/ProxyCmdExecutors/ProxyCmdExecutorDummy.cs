﻿namespace VAR.Toolbox.Code.ProxyCmdExecutors
{
    public class ProxyCmdExecutorDummy : IProxyCmdExecutor
    {
        public string Name { get { return "Dummy"; } }

        public ProxyCmdExecutorDummy(string config)
        {
            if (config == null)
            {
                throw new System.ArgumentNullException(nameof(config));
            }
        }

        public bool ExecuteCmd(string cmdString, IOutputHandler outputHandler)
        {
            outputHandler.OutputLine(string.Format("DummyExecution: {0}", cmdString));
            return true;
        }
    }
}
