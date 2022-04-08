using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using VAR.Toolbox.Code;
using VAR.Toolbox.UI;

namespace VAR.Toolbox
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.ThreadException += Application_ThreadException;

            // Load plug-ins
            string executingAssemblyPath = Assembly.GetExecutingAssembly().Location;
            string dirName = Path.GetDirectoryName(executingAssemblyPath);
            string execName = Path.GetFileNameWithoutExtension(executingAssemblyPath);
            if (dirName != null)
            {
                string[] assemblyPaths = Directory.GetFiles(dirName, $"{execName}*.dll");
                foreach (string assemblyPath in assemblyPaths) { AssemblyLoadFull(assemblyPath); }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new FrmToolbox());
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                Application.Exit();
            }
        }

        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Logger.Log(e.Exception);
            Application.Exit();
        }

        private static void AssemblyLoadFull(string fullPath, List<string> allAssemblyNames = null)
        {
            if (allAssemblyNames == null)
            {
                allAssemblyNames = AppDomain.CurrentDomain.GetAssemblies().Select(a => a.GetName().Name).ToList();
            }

            Assembly asm = null;
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
            string dirPath = Path.GetDirectoryName(fullPath);
            AssemblyName[] asmNames = asm.GetReferencedAssemblies();
            foreach (AssemblyName asmName in asmNames)
            {
                if (allAssemblyNames.Contains(asmName.Name) == false)
                {
                    string fullPathAux = $"{dirPath}/{asmName.Name}.dll";
                    AssemblyLoadFull(fullPathAux, allAssemblyNames);
                }
            }
        }
    }
}