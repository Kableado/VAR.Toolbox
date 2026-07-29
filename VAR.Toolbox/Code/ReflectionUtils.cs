using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace VAR.Toolbox.Code;

public static class ReflectionUtils
{
    public static IEnumerable<Type> GetTypesOfInterface(Type interfaceType)
    {
        return AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(x =>
            {
                Type[] types;
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
                x is { IsAbstract: false, IsInterface: false, } &&
                interfaceType.IsAssignableFrom(x));
    }
    
    public static (string path, string filenameWithoutExtension) GetCurrentPathAndExecName()
    {
        string path = AppContext.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        string filenameWithoutExtension = AppDomain.CurrentDomain.FriendlyName;
        
        return (path, filenameWithoutExtension);
    }
    
}