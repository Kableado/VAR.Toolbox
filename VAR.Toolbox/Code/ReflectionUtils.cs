using System;
using System.Collections.Generic;
using System.Linq;

namespace VAR.Toolbox.Code
{
    public static class ReflectionUtils
    {
        public static IEnumerable<Type> GetTypesOfInterface(Type interfaceType)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x =>
                {
                    Type[] types = null;
                    try
                    {
                        types = x.GetTypes();
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex);
                        types = [];
                    }
                    return types;
                })
                .Where(x =>
                    x.IsAbstract == false &&
                    x.IsInterface == false &&
                    interfaceType.IsAssignableFrom(x) &&
                    true);
        }
    }
}