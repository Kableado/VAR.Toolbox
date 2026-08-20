using System.Collections.Generic;

using SkiaSharp;

namespace VAR.Toolbox.Code.Platforms;

public interface IPlatform
{
    // System
    string System_GetActiveWindowTitle();
    uint System_GetLastInputTime();
    bool System_SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);
    
    // Mouse
    void Mouse_Move(int dx, int dy);
    void Mouse_GetPosition(out uint x, out uint y);
    void Mouse_SetPosition(uint x, uint y);
    
    // Screen
    SKBitmap Screen_CaptureRegion(SKBitmap? bmp, int left, int top, int width, int height);

    // Webcam
    IWebcam Webcam_Create(string moniker);
    Dictionary<string, string> Webcam_ListDevices();
    
}