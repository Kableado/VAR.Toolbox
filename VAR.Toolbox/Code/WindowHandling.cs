using System;
using System.Diagnostics;
using System.Windows.Forms;
using VAR.Toolbox.Code.Windows;

// ReSharper disable InconsistentNaming

namespace VAR.Toolbox.Code
{
    public static class WindowHandling
    {
        public static void WindowSetTopLevel(Form form, bool top = true)
        {
            User32.SetWindowPos(form.Handle, top
                    ? User32.HWND_TOPMOST
                    : User32.HWND_NOTOPMOST,
                0, 0, 0, 0, User32.TOPMOST_FLAGS);
        }

        public static bool ApplicationIsActivated()
        {
            var activatedHandle = User32.GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
            {
                return false;
            }

            var procId = Process.GetCurrentProcess().Id;
            User32.GetWindowThreadProcessId(activatedHandle, out int activeProcId);
            return activeProcId == procId;
        }
    }
}