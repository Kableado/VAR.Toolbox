using System.Collections.Generic;
using SkiaSharp;

namespace VAR.Toolbox.Code.Platforms.Stub;

internal class StubPlatform : IPlatform
{
    public string System_GetActiveWindowTitle()
    {
        return string.Empty;
    }
    
    public uint System_GetLastInputTime()
    {
        return 0;
    }
    
    public bool System_SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent)
    {
        return false;
    }

    public void Mouse_Move(int dx, int dy) { }
    
    public void Mouse_GetPosition(out uint x, out uint y)
    {
        x = 0;
        y = 0;
    }
    
    public void Mouse_SetPosition(uint x, uint y) { }
    
    public SKBitmap Screen_CaptureRegion(SKBitmap? bmp, int left, int top, int width, int height)
    {
        return bmp ?? new SKBitmap(width, height);
    }

    public IWebcam Webcam_Create(string moniker)
    {
        return new StubWebcam();
    }
    
    public Dictionary<string, string> Webcam_ListDevices()
    {
        return new Dictionary<string, string>();
    }
}