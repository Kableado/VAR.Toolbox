using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Avalonia;
using VAR.Toolbox.Code;

namespace VAR.Toolbox;

public static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main(string[] args)
    {
        // Load plug-ins
        string executingAssemblyPath = Assembly.GetExecutingAssembly().Location;
        string? dirName = Path.GetDirectoryName(executingAssemblyPath);
        string execName = Path.GetFileNameWithoutExtension(executingAssemblyPath);
        if (dirName != null)
        {
            string[] assemblyPaths = Directory.GetFiles(dirName, $"{execName}*.dll");
            foreach (string assemblyPath in assemblyPaths) { AssemblyLoadFull(assemblyPath); }
        }

        try
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            Logger.Log(ex);
        }
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
    }

    private static void AssemblyLoadFull(string fullPath, List<string?>? allAssemblyNames = null)
    {
        allAssemblyNames ??= AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetName().Name).ToList();

        if (File.Exists(fullPath) == false) { return; }

        AssemblyName asmNameCurrent = AssemblyName.GetAssemblyName(fullPath);
        if (allAssemblyNames.Contains(asmNameCurrent.Name)) { return; }

        Assembly? asm = null;
        try
        {
            asm = Assembly.LoadFrom(fullPath);
        }
        catch (Exception)
        {
            // ignored
        }

        if (asm == null) { return; }

        allAssemblyNames.Add(asm.GetName().Name);

        // Load dependencies
        string? dirPath = Path.GetDirectoryName(fullPath);
        AssemblyName[] asmNames = asm.GetReferencedAssemblies();
        foreach (AssemblyName asmName in asmNames)
        {
            if (allAssemblyNames.Contains(asmName.Name) == false)
            {
                string fullPathAux = Path.Combine(dirPath ?? string.Empty, $"{asmName.Name}.dll");
                AssemblyLoadFull(fullPathAux, allAssemblyNames);
            }
        }
    }
}