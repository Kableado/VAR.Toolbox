using System.Collections.Generic;
using System.Drawing;

namespace VAR.Toolbox.Code.Platforms.Windows;

internal class WindowsPlatform : IPlatform
{
    public string System_GetActiveWindowTitle()
    {
        return User32.GetActiveWindowTitle();
    }
    
    public uint System_GetLastInputTime()
    {
        return Win32.GetLastInputTime();
    }
    
    public bool System_SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent)
    {
        return Win32.SetSuspendState(hibernate, forceCritical, disableWakeEvent);
    }

    public void Mouse_Move(int dx, int dy)
    {
        Mouse.Move(dx, dy);
    }

    public void Mouse_GetPosition(out uint x, out uint y)
    {
        Mouse.GetPosition(out x, out y);
    }
    
    public void Mouse_SetPosition(uint x, uint y)
    {
        Mouse.SetPosition(x, y);
    }

    public Bitmap Screen_CaptureRegion(Bitmap? bmp, int left, int top, int width, int height)
    {
        return Screenshoter.CaptureScreenRegion(bmp, left, top, width, height);
    }

    public IWebcam Webcam_Create(string moniker)
    {
        return new Webcam(moniker);
    }
    
    public Dictionary<string, string> Webcam_ListDevices()
    {
        return Webcam.ListDevices();
    }
}