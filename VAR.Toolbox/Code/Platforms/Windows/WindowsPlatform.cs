using System.Collections.Generic;
using System.Drawing;
using System.IO;
using SkiaSharp;

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

    public SKBitmap Screen_CaptureRegion(SKBitmap? bmp, int left, int top, int width, int height)
    {
        Bitmap? winBmp = null;
        if (bmp != null)
        {
            // This is inefficient but necessary for the interface change
            using MemoryStream ms = new();
            using SKImage image = SKImage.FromBitmap(bmp);
            using SKData data = image.Encode(SKEncodedImageFormat.Bmp, 100);
            ms.Write(data.ToArray());
            ms.Seek(0, SeekOrigin.Begin);
            winBmp = new Bitmap(ms);
        }

        Bitmap resultWinBmp = Screenshoter.CaptureScreenRegion(winBmp, left, top, width, height);
        
        using MemoryStream resultMs = new();
        resultWinBmp.Save(resultMs, System.Drawing.Imaging.ImageFormat.Bmp);
        resultMs.Seek(0, SeekOrigin.Begin);
        SKBitmap result = SKBitmap.Decode(resultMs);
        
        if (winBmp != null && !ReferenceEquals(winBmp, resultWinBmp))
        {
            winBmp.Dispose();
        }
        // resultWinBmp might be owned by Screenshoter or newly created
        
        return result;
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