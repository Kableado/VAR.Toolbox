using System;
using System.Runtime.InteropServices;
using System.Text;

// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable IdentifierTypo

namespace VAR.Toolbox.Code.Windows;

public static class User32
{
#if WINDOWS
    // Keep original P/Invoke declarations on Windows builds
    [StructLayout(LayoutKind.Sequential)]
    public struct INPUT
    {
        public uint Type;
        public MOUSEKEYBDHARDWAREINPUT Data;
    }

    public const int INPUT_MOUSE = 0;
    public const int INPUT_KEYBOARD = 1;
    public const int INPUT_HARDWARE = 2;

    [StructLayout(LayoutKind.Explicit)]
    public struct MOUSEKEYBDHARDWAREINPUT
    {
        [FieldOffset(0)] public HARDWAREINPUT Hardware;
        [FieldOffset(0)] public KEYBDINPUT Keyboard;
        [FieldOffset(0)] public MOUSEINPUT Mouse;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct HARDWAREINPUT
    {
        public uint Msg;
        public ushort ParamL;
        public ushort ParamH;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct KEYBDINPUT
    {
        public ushort Vk;
        public ushort Scan;
        public uint Flags;
        public uint Time;
        public IntPtr ExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MOUSEINPUT
    {
        public int X;
        public int Y;
        public uint MouseData;
        public uint Flags;
        public uint Time;
        public IntPtr ExtraInfo;
    }

    public const int MOUSEEVENTD_XBUTTON1 = 0x0001;
    public const int MOUSEEVENTD_XBUTTON2 = 0x0002;

    public const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
    public const uint MOUSEEVENTF_HWHEEL = 0x01000;
    public const uint MOUSEEVENTF_MOVE = 0x0001;
    public const uint MOUSEEVENTF_MOVE_NOCOALESCE = 0x2000;
    public const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    public const uint MOUSEEVENTF_LEFTUP = 0x0004;
    public const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
    public const uint MOUSEEVENTF_RIGHTUP = 0x0010;
    public const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
    public const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
    public const uint MOUSEEVENTF_VIRTUALDESK = 0x4000;
    public const uint MOUSEEVENTF_WHEEL = 0x0800;
    public const uint MOUSEEVENTF_XDOWN = 0x0080;
    public const uint MOUSEEVENTF_XUP = 0x0100;

    [DllImport("User32.dll")]
    public static extern int SendInput(int nInputs, INPUT[] pInputs, int cbSize);

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public UInt32 X;
        public UInt32 Y;
    }

    [DllImport("user32.dll")]
    public static extern bool GetCursorPos(out POINT lpPoint);

    [DllImport("User32.dll")]
    public static extern Boolean SetCursorPos(UInt32 x, UInt32 y);

    [StructLayout(LayoutKind.Sequential)]
    public struct LASTINPUTINFO
    {
        public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));
        [MarshalAs(UnmanagedType.U4)] public UInt32 cbSize;
        [MarshalAs(UnmanagedType.U4)] public UInt32 dwTime;
    }

    [DllImport("user32.dll")]
    public static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

    [DllImport("user32.dll")]
    public static extern short GetAsyncKeyState(int vKey);

    public const int VK_MBUTTON = 0x04;
    public const int VK_LBUTTON = 0x01;
    public const int VK_RBUTTON = 0x02;

    public const int WM_NCLBUTTONDOWN = 0xA1;
    public const int HT_CAPTION = 0x2;

    [DllImport("user32.dll")]
    public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    [DllImport("user32.dll")]
    public static extern IntPtr GetDesktopWindow();

    [DllImport("user32.dll")]
    public static extern IntPtr GetWindowDC(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [DllImport("user32.dll")]
    public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
        
    public static string GetActiveWindowTitle()
    {
        const int NChars = 256;
        StringBuilder buff = new(NChars);
        IntPtr handle = GetForegroundWindow();

        if (GetWindowText(handle, buff, NChars) > 0)
        {
            return buff.ToString();
        }

        return string.Empty;
    }
        
    public static readonly IntPtr HWND_TOPMOST = new(-1);
    public static readonly IntPtr HWND_NOTOPMOST = new(-2);
    public const UInt32 SWP_NOSIZE = 0x0001;
    public const UInt32 SWP_NOMOVE = 0x0002;
    public const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy,
        uint uFlags);

        
    /// <summary>
    /// defines the callback type for the hook
    /// </summary>
    public delegate int keyboardHookProc(int code, int wParam, ref keyboardHookStruct lParam);

    public struct keyboardHookStruct {
        public int vkCode;
        public int scanCode;
        public int flags;
        public int time;
        public int dwExtraInfo;
    }

    public const int WH_KEYBOARD_LL = 13;
    public const int WM_KEYDOWN = 0x100;
    public const int WM_KEYUP = 0x101;
    public const int WM_SYSKEYDOWN = 0x104;
    public const int WM_SYSKEYUP = 0x105;
        
    [DllImport("user32.dll")]
    public static extern IntPtr SetWindowsHookEx(int idHook, keyboardHookProc callback, IntPtr hInstance, uint threadId);

    [DllImport("user32.dll")]
    public static extern bool UnhookWindowsHookEx(IntPtr hInstance);

    [DllImport("user32.dll")]
    public static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref keyboardHookStruct lParam);

    [DllImport("kernel32.dll")]
    public static extern IntPtr LoadLibrary(string lpFileName);

#else
    // Non-Windows stubs: provide the same API surface but without P/Invoke
    // so the assembly can be loaded and used on Linux. Methods return safe defaults.

    public struct INPUT { public uint Type; public MOUSEKEYBDHARDWAREINPUT Data; }
    public const int INPUT_MOUSE = 0;
    public const int INPUT_KEYBOARD = 1;
    public const int INPUT_HARDWARE = 2;

    public struct MOUSEKEYBDHARDWAREINPUT { public HARDWAREINPUT Hardware; public KEYBDINPUT Keyboard; public MOUSEINPUT Mouse; }
    public struct HARDWAREINPUT { public uint Msg; public ushort ParamL; public ushort ParamH; }
    public struct KEYBDINPUT { public ushort Vk; public ushort Scan; public uint Flags; public uint Time; public IntPtr ExtraInfo; }
    public struct MOUSEINPUT { public int X; public int Y; public uint MouseData; public uint Flags; public uint Time; public IntPtr ExtraInfo; }

    public const int MOUSEEVENTD_XBUTTON1 = 0x0001;
    public const int MOUSEEVENTD_XBUTTON2 = 0x0002;

    public const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
    public const uint MOUSEEVENTF_HWHEEL = 0x01000;
    public const uint MOUSEEVENTF_MOVE = 0x0001;
    public const uint MOUSEEVENTF_MOVE_NOCOALESCE = 0x2000;
    public const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
    public const uint MOUSEEVENTF_LEFTUP = 0x0004;
    public const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
    public const uint MOUSEEVENTF_RIGHTUP = 0x0010;
    public const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
    public const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
    public const uint MOUSEEVENTF_VIRTUALDESK = 0x4000;
    public const uint MOUSEEVENTF_WHEEL = 0x0800;
    public const uint MOUSEEVENTF_XDOWN = 0x0080;
    public const uint MOUSEEVENTF_XUP = 0x0100;

    public static int SendInput(int nInputs, INPUT[] pInputs, int cbSize) => 0;

    public struct POINT { public UInt32 X; public UInt32 Y; }
    public static bool GetCursorPos(out POINT lpPoint) { lpPoint = new POINT(); return false; }
    public static Boolean SetCursorPos(UInt32 x, UInt32 y) => false;

    public struct LASTINPUTINFO { public UInt32 cbSize; public UInt32 dwTime; }
    public static bool GetLastInputInfo(ref LASTINPUTINFO plii) => false;
    public static short GetAsyncKeyState(int vKey) => 0;

    public const int VK_MBUTTON = 0x04;
    public const int VK_LBUTTON = 0x01;
    public const int VK_RBUTTON = 0x02;

    public const int WM_NCLBUTTONDOWN = 0xA1;
    public const int HT_CAPTION = 0x2;

    public static int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam) => 0;
    public static bool ReleaseCapture() => false;

    public struct RECT { public int left; public int top; public int right; public int bottom; }
    public static IntPtr GetDesktopWindow() => IntPtr.Zero;
    public static IntPtr GetWindowDC(IntPtr hWnd) => IntPtr.Zero;
    public static IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC) => IntPtr.Zero;
    public static IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect) => IntPtr.Zero;
    public static bool SetForegroundWindow(IntPtr hWnd) => false;
    public static IntPtr GetForegroundWindow() => IntPtr.Zero;
    public static int GetWindowText(IntPtr hWnd, StringBuilder text, int count) { return 0; }
    public static int GetWindowThreadProcessId(IntPtr handle, out int processId) { processId = 0; return 0; }

    public static string GetActiveWindowTitle() => string.Empty;

    public static readonly IntPtr HWND_TOPMOST = new(-1);
    public static readonly IntPtr HWND_NOTOPMOST = new(-2);
    public const UInt32 SWP_NOSIZE = 0x0001;
    public const UInt32 SWP_NOMOVE = 0x0002;
    public const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

    public static bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags) => false;

    public delegate int keyboardHookProc(int code, int wParam, ref keyboardHookStruct lParam);
    public struct keyboardHookStruct { public int vkCode; public int scanCode; public int flags; public int time; public int dwExtraInfo; }

    public const int WH_KEYBOARD_LL = 13;
    public const int WM_KEYDOWN = 0x100;
    public const int WM_KEYUP = 0x101;
    public const int WM_SYSKEYDOWN = 0x104;
    public const int WM_SYSKEYUP = 0x105;

    public static IntPtr SetWindowsHookEx(int idHook, keyboardHookProc callback, IntPtr hInstance, uint threadId) => IntPtr.Zero;
    public static bool UnhookWindowsHookEx(IntPtr hInstance) => false;
    public static int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref keyboardHookStruct lParam) => 0;
    public static IntPtr LoadLibrary(string lpFileName) => IntPtr.Zero;

#endif
}