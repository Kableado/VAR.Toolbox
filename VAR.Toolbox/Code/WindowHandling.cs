using System;
using System.Diagnostics;
#if WINDOWS
using System.Windows.Forms;
using VAR.Toolbox.Code.Windows;
#endif

namespace VAR.Toolbox.Code;

// ReSharper disable InconsistentNaming

public static class WindowHandling
{
#if WINDOWS

    public static void WindowSetTopLevel(Form form, bool top = true)
    {
        User32.SetWindowPos(form.Handle, top
                ? User32.HWND_TOPMOST
                : User32.HWND_NOTOPMOST,
            0, 0, 0, 0, User32.TOPMOST_FLAGS);
    }

    public static bool ApplicationIsActivated()
    {
        IntPtr activatedHandle = User32.GetForegroundWindow();
        if (activatedHandle == IntPtr.Zero)
        {
            return false;
        }

        int procId = Process.GetCurrentProcess().Id;
        User32.GetWindowThreadProcessId(activatedHandle, out int activeProcId);
        return activeProcId == procId;
    }
#else
    // No-op implementations for non-Windows platforms so project compiles on Linux.
    public static void WindowSetTopLevel(object form, bool top = true)
    {
        // Not applicable on non-Windows platforms
    }

    public static bool ApplicationIsActivated()
    {
        // Assume activated on non-Windows for simplicity
        return true;
    }
#endif
}