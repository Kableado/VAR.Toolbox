using System.Collections.Generic;
using System.Drawing;

namespace VAR.Toolbox.Code.Platforms.Stub;

internal class StubPlatform : IPlatform
{
    public string GetActiveWindowTitle()
    {
        return string.Empty;
    }
    
    public uint GetLastInputTime()
    {
        return 0;
    }
    
    public bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent)
    {
        return false;
    }

    public void Move(int dx, int dy) { }
    
    public void GetPosition(out uint x, out uint y)
    {
        x = 0;
        y = 0;
    }
    
    public void SetPosition(uint x, uint y) { }
    
    public Bitmap CaptureScreenRegion(Bitmap? bmp, int left, int top, int width, int height)
    {
        return bmp ?? new Bitmap(width, height);
    }

    public IWebcam CreateWebcam(string moniker)
    {
        return new StubWebcam();
    }
    
    public Dictionary<string, string> ListDevices()
    {
        return new Dictionary<string, string>();
    }
}