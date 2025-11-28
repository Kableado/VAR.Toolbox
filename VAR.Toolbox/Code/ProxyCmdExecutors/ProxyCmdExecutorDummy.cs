namespace VAR.Toolbox.Code.ProxyCmdExecutors
{
    public class ProxyCmdExecutorDummy : BaseProxyCmdExecutor
    {
        private readonly string _config;
        public override string Name => "Dummy";

        public ProxyCmdExecutorDummy(string config)
        {
            if (config == null)
            {
                throw new System.ArgumentNullException(nameof(config));
            }

            _config = config;
        }

        public override bool ExecuteCmd(string cmdString, IOutputHandler outputHandler)
        {
            outputHandler.AddLine($"DummyExecution: {cmdString} | {_config}");
            return true;
        }
    }
}