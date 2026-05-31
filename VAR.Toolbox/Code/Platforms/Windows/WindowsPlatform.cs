using System.Collections.Generic;
using System.Drawing;

namespace VAR.Toolbox.Code.Platforms.Windows;

internal class WindowsPlatform : IPlatform
{
    public string GetActiveWindowTitle()
    {
        return User32.GetActiveWindowTitle();
    }
    
    public uint GetLastInputTime()
    {
        return Win32.GetLastInputTime();
    }
    
    public bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent)
    {
        return Win32.SetSuspendState(hibernate, forceCritical, disableWakeEvent);
    }

    public void Move(int dx, int dy)
    {
        Mouse.Move(dx, dy);
    }

    public void GetPosition(out uint x, out uint y)
    {
        Mouse.GetPosition(out x, out y);
    }
    
    public void SetPosition(uint x, uint y)
    {
        Mouse.SetPosition(x, y);
    }

    public Bitmap CaptureScreenRegion(Bitmap? bmp, int left, int top, int width, int height)
    {
        return Screenshoter.CaptureScreenRegion(bmp, left, top, width, height);
    }

    public IWebcam CreateWebcam(string moniker)
    {
        return new Webcam(moniker);
    }
    
    public Dictionary<string, string> ListDevices()
    {
        return Webcam.ListDevices();
    }
}