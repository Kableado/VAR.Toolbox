using System;
using System.Runtime.InteropServices;

namespace VAR.Toolbox.Code.Platform;

public class LinuxPlatformService : IPlatformService
{
    public string GetActiveWindowTitle()
    {
        // Best-effort: not implemented fully. Return empty string to indicate unknown.
        return string.Empty;
    }

    public uint GetLastInputTimeSeconds()
    {
        // Not implemented: returning large value to indicate idle by default.
        return uint.MaxValue / 2;
    }

    public void SetWindowTopMost(IntPtr windowHandle, bool top)
    {
        // No-op on Linux for now (different platforms / compositors have different APIs)
    }

    public void MoveMouseRelative(int dx, int dy)
    {
        // Use XTest relative motion for X11 sessions
        IntPtr d = X11.XOpenDisplay(IntPtr.Zero);
        if (d == IntPtr.Zero)
            throw new Exception("Unable to open X display");
        try
        {
            if (X11.XTestFakeRelativeMotionEvent(d, dx, dy, 0) == 0)
                throw new Exception("XTestFakeRelativeMotionEvent failed");
            X11.XFlush(d);
        }
        finally
        {
            X11.XCloseDisplay(d);
        }
    }

    public void SetMouseButton(VAR.Toolbox.Code.Mouse.MouseButtons button, bool down)
    {
        IntPtr d = X11.XOpenDisplay(IntPtr.Zero);
        if (d == IntPtr.Zero)
            throw new Exception("Unable to open X display");
        try
        {
            uint btn = button == VAR.Toolbox.Code.Mouse.MouseButtons.Left ? 1u : button == VAR.Toolbox.Code.Mouse.MouseButtons.Middle ? 2u : 3u;
            if (X11.XTestFakeButtonEvent(d, btn, down ? 1 : 0, 0) == 0)
                throw new Exception("XTestFakeButtonEvent failed");
            X11.XFlush(d);
        }
        finally
        {
            X11.XCloseDisplay(d);
        }
    }

    public void GetCursorPosition(out uint x, out uint y)
    {
        IntPtr d = X11.XOpenDisplay(IntPtr.Zero);
        if (d == IntPtr.Zero)
            throw new Exception("Unable to open X display");
        try
        {
            IntPtr root = X11.XRootWindow(d, X11.XDefaultScreen(d));
            if (!X11.XQueryPointer(d, root, out _, out _, out int root_x, out int root_y, out _, out _, out _))
                throw new Exception("XQueryPointer failed");
            x = (uint)root_x;
            y = (uint)root_y;
        }
        finally
        {
            X11.XFlush(d);
            X11.XCloseDisplay(d);
        }
    }

    public void SetCursorPosition(uint x, uint y)
    {
        IntPtr d = X11.XOpenDisplay(IntPtr.Zero);
        if (d == IntPtr.Zero)
            throw new Exception("Unable to open X display");
        try
        {
            int screen = X11.XDefaultScreen(d);
            if (X11.XTestFakeMotionEvent(d, screen, (int)x, (int)y, 0) == 0)
                throw new Exception("XTestFakeMotionEvent failed");
            X11.XFlush(d);
        }
        finally
        {
            X11.XCloseDisplay(d);
        }
    }

    public System.Drawing.Image? CaptureWindow(IntPtr handle)
    {
        // Not implemented for Linux yet; return null so caller can fallback.
        return null;
    }

    private static class X11
    {
        const string libX11 = "libX11.so.6";
        const string libXtst = "libXtst.so.6";

        [DllImport(libX11)]
        public static extern IntPtr XOpenDisplay(IntPtr display_name);

        [DllImport(libX11)]
        public static extern int XDefaultScreen(IntPtr display);

        [DllImport(libX11)]
        public static extern IntPtr XRootWindow(IntPtr display, int screen_number);

        [DllImport(libX11)]
        public static extern bool XQueryPointer(IntPtr display, IntPtr w, out IntPtr root_return, out IntPtr child_return, out int root_x_return, out int root_y_return, out int win_x_return, out int win_y_return, out uint mask_return);

        [DllImport(libX11)]
        public static extern int XFlush(IntPtr display);

        [DllImport(libX11)]
        public static extern int XCloseDisplay(IntPtr display);

        [DllImport(libXtst)]
        public static extern int XTestFakeMotionEvent(IntPtr display, int screen, int x, int y, ulong delay);

        [DllImport(libXtst)]
        public static extern int XTestFakeRelativeMotionEvent(IntPtr display, int x, int y, ulong delay);

        [DllImport(libXtst)]
        public static extern int XTestFakeButtonEvent(IntPtr display, uint button, int is_press, ulong delay);
    }
}

