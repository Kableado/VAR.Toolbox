using System;
using System.Runtime.InteropServices;

namespace VAR.Toolbox.Code.Platforms.Linux;

internal static class X11
{
    [DllImport("libX11.so.6")]
    public static extern IntPtr XOpenDisplay(string? display_name);

    [DllImport("libX11.so.6")]
    public static extern int XCloseDisplay(IntPtr display);

    [DllImport("libX11.so.6")]
    public static extern IntPtr XDefaultRootWindow(IntPtr display);

    [DllImport("libX11.so.6")]
    public static extern bool XQueryPointer(IntPtr display, IntPtr window, out IntPtr root, out IntPtr child, out int root_x, out int root_y, out int win_x, out int win_y, out uint mask);

    [DllImport("libX11.so.6")]
    public static extern int XWarpPointer(IntPtr display, IntPtr src_w, IntPtr dest_w, int src_x, int src_y, uint src_width, uint src_height, int dest_x, int dest_y);
    
    [DllImport("libX11.so.6")]
    public static extern int XFlush(IntPtr display);
}
