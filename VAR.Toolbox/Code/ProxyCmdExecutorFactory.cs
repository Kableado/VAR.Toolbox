namespace VAR.Toolbox.Code
{
    public class ProxyCmdExecutorFactory
    {
        public static IProxyCmdExecutor CreateFromConfig(string config)
        {
            if (string.IsNullOrEmpty(config))
            {
                return new ProxyCmdExecutorDummy();
            }
            return new ProxyCmdExecutorThroughSQLServer(config);
        }
    }
}
