#pragma warning disable IDE1006

using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable CommentTypo

namespace VAR.Toolbox.Code.Windows;

public static class Win32
{
#if WINDOWS
    [DllImport("ole32.dll")]
    public static extern int CreateBindCtx(int reserved, out IBindCtx ppbc);

    [DllImport("ole32.dll", CharSet = CharSet.Unicode)]
    public static extern int MkParseDisplayName(IBindCtx pbc, string szUserName, ref int pchEaten, out IMoniker ppmk);

    [DllImport("ntdll.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern unsafe int memcpy(byte* dst, byte* src, int count);

    [DllImport("oleaut32.dll")]
    public static extern int OleCreatePropertyFrame(
        IntPtr hwndOwner,
        int x,
        int y,
        [MarshalAs(UnmanagedType.LPWStr)] string caption,
        int cObjects,
        [MarshalAs(UnmanagedType.Interface, ArraySubType = UnmanagedType.IUnknown)] ref object ppUnk,
        int cPages,
        IntPtr lpPageClsID,
        int lcid,
        int dwReserved,
        IntPtr lpvReserved);

    [DllImport("PowrProf.dll")]
    public static extern Boolean SetSuspendState(Boolean hibernate, Boolean forceCritical, Boolean disableWakeEvent);

    public static uint GetLastInputTime()
    {
        uint idleTime = 0;
        User32.LASTINPUTINFO lastInputInfo = new();
        lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
        lastInputInfo.dwTime = 0;
        uint envTicks = (uint)Environment.TickCount;
        if (User32.GetLastInputInfo(ref lastInputInfo))
        {
            uint lastInputTick = lastInputInfo.dwTime;
            idleTime = envTicks - lastInputTick;
        }

        return ((idleTime > 0) ? (idleTime / 1000) : 0);
    }
#else
    // Non-Windows stubs to avoid P/Invoke resolution on Linux
    public static int CreateBindCtx(int reserved, out IBindCtx ppbc) { ppbc = null!; return -1; }
    public static int MkParseDisplayName(IBindCtx pbc, string szUserName, ref int pchEaten, out IMoniker ppmk) { ppmk = null!; return -1; }
    public static unsafe int memcpy(byte* dst, byte* src, int count) { if (dst == null || src == null) return 0; for (int i = 0; i < count; i++) dst[i] = src[i]; return 0; }
    public static int OleCreatePropertyFrame(IntPtr hwndOwner, int x, int y, string caption, int cObjects, ref object ppUnk, int cPages, IntPtr lpPageClsID, int lcid, int dwReserved, IntPtr lpvReserved) { return -1; }
    public static Boolean SetSuspendState(Boolean hibernate, Boolean forceCritical, Boolean disableWakeEvent) { return false; }

    public static uint GetLastInputTime() { return 0; }
#endif
}