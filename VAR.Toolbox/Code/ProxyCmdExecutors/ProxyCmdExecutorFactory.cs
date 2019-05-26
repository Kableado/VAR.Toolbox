using System;
using System.Collections.Generic;
using System.Linq;

namespace VAR.Toolbox.Code.ProxyCmdExecutors
{
    public class ProxyCmdExecutorFactory
    {
        private static Dictionary<string, Type> _dictProxyCmdExecutors = null;

        private static Dictionary<string, Type> GetDict()
        {
            if (_dictProxyCmdExecutors != null)
            {
                return _dictProxyCmdExecutors;
            }

            Type iTextCoder = typeof(IProxyCmdExecutor);
            IEnumerable<Type> toolFormTypes = ReflectionUtils.GetTypesOfInterface(iTextCoder);
            _dictProxyCmdExecutors = toolFormTypes.ToDictionary(t =>
            {
                IProxyCmdExecutor proxyCmdExecutor = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(t) as IProxyCmdExecutor;
                return proxyCmdExecutor.Name;
            });

            return _dictProxyCmdExecutors;
        }

        public static IProxyCmdExecutor CreateFromConfig(string config)
        {
            Dictionary<string, Type> dict = GetDict();
            int indexOfColon = config.IndexOf(':');
            string name = config.Substring(0, indexOfColon < 0 ? config.Length : indexOfColon);
            string nextConfig = config.Substring(indexOfColon + 1);
            if (string.IsNullOrEmpty(name))
            {
                return new ProxyCmdExecutorDummy(string.Empty);
            }
            if (dict.ContainsKey(name) == false)
            {
                throw new NotImplementedException(string.Format("Cant create IProxyCmdExecutor with this config: {0}", config));
            }
            Type proxyCmdExecutorType = dict[name];

            IProxyCmdExecutor proxyCmdExecutor = Activator.CreateInstance(proxyCmdExecutorType, new object[] { nextConfig }) as IProxyCmdExecutor;

            return proxyCmdExecutor;
        }
    }
}
