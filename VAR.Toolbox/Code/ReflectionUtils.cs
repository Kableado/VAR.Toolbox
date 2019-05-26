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
                .SelectMany(x => x.GetTypes())
                .Where(x =>
                    x.IsAbstract == false &&
                    x.IsInterface == false &&
                    interfaceType.IsAssignableFrom(x) &&
                    true);
        }

    }
}
