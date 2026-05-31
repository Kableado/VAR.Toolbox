using System.Collections.Generic;
using System.Drawing;

namespace VAR.Toolbox.Code.Platforms;

public interface IPlatform
{
    string GetActiveWindowTitle();
    uint GetLastInputTime();
    bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);
    
    // Mouse
    void Move(int dx, int dy);
    void GetPosition(out uint x, out uint y);
    void SetPosition(uint x, uint y);
    
    // Screenshoter
    Bitmap CaptureScreenRegion(Bitmap? bmp, int left, int top, int width, int height);

    // Webcam
    IWebcam CreateWebcam(string moniker);
    Dictionary<string, string> ListDevices();
    
}