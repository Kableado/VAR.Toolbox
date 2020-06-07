using System;
using System.Collections.Generic;
using System.Linq;

namespace VAR.Toolbox.Code
{
    public abstract class BaseFactory<T> where T : INamed
    {
        private static Dictionary<string, Type> _dictTypes = null;

        private static Dictionary<string, Type> GetDict()
        {
            if (_dictTypes != null)
            {
                return _dictTypes;
            }

            Type iType = typeof(T);
            IEnumerable<Type> types = ReflectionUtils.GetTypesOfInterface(iType);
            _dictTypes = types.ToDictionary(t =>
            {
                T type = (T)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(t);
                return type.Name;
            });

            return _dictTypes;
        }

        public static string[] GetNames()
        {
            Dictionary<string, Type> dict = GetDict();
            return dict.Select(p => p.Key).ToArray();
        }

        public static T CreateFromName(string name)
        {
            Dictionary<string, Type> dict = GetDict();
            if (dict.ContainsKey(name) == false)
            {
                throw new NotImplementedException(string.Format("Cant create {1} with this name: {0}", name, typeof(T).Name));
            }
            Type type = dict[name];

            T instance = (T)Activator.CreateInstance(type);
            return instance;
        }

        public static T CreateFromConfig(string config)
        {
            Dictionary<string, Type> dict = GetDict();
            int indexOfColon = config.IndexOf(':');
            string name = config.Substring(0, indexOfColon < 0 ? config.Length : indexOfColon);
            string nextConfig = config.Substring(indexOfColon + 1);
            if (string.IsNullOrEmpty(name))
            {
                return (T)(object)null;
            }
            if (dict.ContainsKey(name) == false)
            {
                throw new NotImplementedException(string.Format("Cant create {1} with this config: {0}", config, typeof(T).Name));
            }
            Type type = dict[name];

            T instance = (T)Activator.CreateInstance(type, new object[] { nextConfig });
            return instance;
        }
    }

    public interface INamed
    {
        string Name { get; }
    }
}
