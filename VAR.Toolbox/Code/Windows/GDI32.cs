using System;
using System.Runtime.InteropServices;

namespace VAR.Toolbox.Code.Windows;

public static class GDI32
{
#if WINDOWS
    public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter

    [DllImport("gdi32.dll")]
    public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
        int nWidth, int nHeight, IntPtr hObjectSource,
        int nXSrc, int nYSrc, int dwRop);

    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
        int nHeight);

    [DllImport("gdi32.dll")]
    public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

    [DllImport("gdi32.dll")]
    public static extern bool DeleteDC(IntPtr hDC);

    [DllImport("gdi32.dll")]
    public static extern bool DeleteObject(IntPtr hObject);

    [DllImport("gdi32.dll")]
    public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
#else
    public const int SRCCOPY = 0x00CC0020;
    public static bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjectSource, int nXSrc, int nYSrc, int dwRop) => false;
    public static IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight) => IntPtr.Zero;
    public static IntPtr CreateCompatibleDC(IntPtr hDC) => IntPtr.Zero;
    public static bool DeleteDC(IntPtr hDC) => false;
    public static bool DeleteObject(IntPtr hObject) => false;
    public static IntPtr SelectObject(IntPtr hDC, IntPtr hObject) => IntPtr.Zero;
#endif
}