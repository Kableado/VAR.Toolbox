using System;
using System.Windows.Forms;
using VAR.Toolbox.Code;
using VAR.Toolbox.UI;

namespace VAR.Toolbox
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += Application_ThreadException;

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
    }
}
