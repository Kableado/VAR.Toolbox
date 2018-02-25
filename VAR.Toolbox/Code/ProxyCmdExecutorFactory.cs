﻿using System;

namespace VAR.Toolbox.Code
{
    public class ProxyCmdExecutorFactory
    {
        public static IProxyCmdExecutor CreateFromConfig(string config)
        {
            if (string.IsNullOrEmpty(config) || config.StartsWith("Dummy:"))
            {
                return new ProxyCmdExecutorDummy();
            }
            if (config.StartsWith("SqlServer:"))
            {
                string configSqlServer = config.Substring("SqlServer:".Length);
                return new ProxyCmdExecutorThroughSQLServer(configSqlServer);
            }
            throw new NotImplementedException(string.Format("Cant create IProxyCmdExecutor with this config: {0}", config));
        }
    }
}
