using System;
using VAR.Toolbox.Code.Windows;

namespace VAR.Toolbox.Code.Platform;

public class WindowsPlatformService : IPlatformService
{
    public string GetActiveWindowTitle()
    {
        return User32.GetActiveWindowTitle();
    }

    public uint GetLastInputTimeSeconds()
    {
        return Win32.GetLastInputTime();
    }

    public void SetWindowTopMost(IntPtr windowHandle, bool top)
    {
        if (windowHandle == IntPtr.Zero) return;
        User32.SetWindowPos(windowHandle, top ? User32.HWND_TOPMOST : User32.HWND_NOTOPMOST, 0, 0, 0, 0, User32.TOPMOST_FLAGS);
    }

    public void MoveMouseRelative(int dx, int dy)
    {
        User32.INPUT input = new()
        {
            Type = User32.INPUT_MOUSE,
        };
        input.Data.Mouse.X = dx;
        input.Data.Mouse.Y = dy;
        input.Data.Mouse.Flags = User32.MOUSEEVENTF_MOVE;
        User32.INPUT[] inputs = new User32.INPUT[] { input };
        if (User32.SendInput(1, inputs, System.Runtime.InteropServices.Marshal.SizeOf(typeof(User32.INPUT))) == 0)
            throw new Exception("SendInput failed");
    }

    public void SetMouseButton(VAR.Toolbox.Code.Mouse.MouseButtons button, bool down)
    {
        User32.INPUT input = new()
        {
            Type = User32.INPUT_MOUSE,
        };
        input.Data.Mouse.X = 0;
        input.Data.Mouse.Y = 0;
        if (button == VAR.Toolbox.Code.Mouse.MouseButtons.Left)
            input.Data.Mouse.Flags = down ? User32.MOUSEEVENTF_LEFTDOWN : User32.MOUSEEVENTF_LEFTUP;
        else if (button == VAR.Toolbox.Code.Mouse.MouseButtons.Middle)
            input.Data.Mouse.Flags = down ? User32.MOUSEEVENTF_MIDDLEDOWN : User32.MOUSEEVENTF_MIDDLEUP;
        else if (button == VAR.Toolbox.Code.Mouse.MouseButtons.Right)
            input.Data.Mouse.Flags = down ? User32.MOUSEEVENTF_RIGHTDOWN : User32.MOUSEEVENTF_RIGHTUP;

        User32.INPUT[] inputs = new User32.INPUT[] { input };
        if (User32.SendInput(1, inputs, System.Runtime.InteropServices.Marshal.SizeOf(typeof(User32.INPUT))) == 0)
            throw new Exception("SendInput failed");
    }

    public void GetCursorPosition(out uint x, out uint y)
    {
        User32.GetCursorPos(out User32.POINT lpPoint);
        x = lpPoint.X;
        y = lpPoint.Y;
    }

    public void SetCursorPosition(uint x, uint y)
    {
        User32.SetCursorPos(x, y);
    }

    public System.Drawing.Image? CaptureWindow(IntPtr handle)
    {
        try
        {
            // replicate previous Screenshoter behavior using User32/GDI32
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            User32.RECT windowRect = new();
            User32.GetWindowRect(handle, ref windowRect);
            int left = windowRect.left;
            int top = windowRect.top;
            int width = windowRect.right - left;
            int height = windowRect.bottom - top;
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
            IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
            GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
            GDI32.SelectObject(hdcDest, hOld);
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);

            System.Drawing.Image img = System.Drawing.Image.FromHbitmap(hBitmap);
            GDI32.DeleteObject(hBitmap);
            return img;
        }
        catch
        {
            return null;
        }
    }
}

