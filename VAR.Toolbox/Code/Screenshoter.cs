using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;

using VAR.Toolbox.Code.Platforms.Windows;

using Image = System.Drawing.Image;
using Point = Avalonia.Point;

namespace VAR.Toolbox.Code;

public static class Screenshoter
{
    public static Bitmap? CaptureControl(Control? ctrl, Bitmap? bmp = null, Window? window = null)
    {
        if (ctrl == null || window == null) { return bmp; }

        Point? relativeToWindow = ctrl.TranslatePoint(new Point(0, 0), window);
        if (relativeToWindow.HasValue == false) { return bmp; }

        PixelPoint screenPoint = window.PointToScreen(relativeToWindow.Value);
        int absoluteLeft = screenPoint.X;
        int absoluteTop = screenPoint.Y;

        double scale;
        try
        {
            scale = window.RenderScaling;
        }
        catch
        {
            scale = 1.0;
        }

        int offsetLeft = (int)Math.Ceiling(1 * scale);
        int offsetTop  = (int)Math.Ceiling(1 * scale);

        absoluteLeft += offsetLeft;
        absoluteTop  += offsetTop;

        int pixelWidth  = Math.Max(1, (int)Math.Round(ctrl.Bounds.Width * scale));
        int pixelHeight = Math.Max(1, (int)Math.Round(ctrl.Bounds.Height * scale));

        bmp = CaptureScreen(bmp: bmp,
            left: absoluteLeft,
            top: absoluteTop,
            width: pixelWidth,
            height: pixelHeight,
            window: window);
        return bmp;
    }

    public static Bitmap? CaptureScreen(Bitmap? bmp = null, int? left = null, int? top = null, int? width = null,
        int? height = null, Window? window = null)
    {
        if (window == null) { return bmp; }

        if (width <= 0 || height <= 0) { return bmp; }
            
        // Pseudocódigo para calcular el rect virtual
        int minLeft = int.MaxValue, minTop = int.MaxValue, maxRight = int.MinValue, maxBottom = int.MinValue;
        if(left == null || top == null || width == null || height == null) 
        {
            foreach (Screen screen in
                     window.Screens.All) // o screensService.Screens / screensService.Monitors según versión
            {
                minLeft = Math.Min(minLeft, screen.Bounds.X);
                minTop = Math.Min(minTop, screen.Bounds.Y);
                maxRight = Math.Max(maxRight, screen.Bounds.X + screen.Bounds.Width);
                maxBottom = Math.Max(maxBottom, screen.Bounds.Y + screen.Bounds.Height);
            }
        }
            
        // Determine the size of the "virtual screen", which includes all monitors.
        left ??= minLeft;
        top ??= minTop;
        width ??= (maxRight - minLeft);
        height ??= (maxBottom - minTop);

        // Create a bitmap of the appropriate size to receive the screenshot.
        if (bmp == null || bmp.Width != width || bmp.Height != height)
        {
            bmp = new Bitmap((int)width, (int)height);
        }

        try
        {
            // Draw the screenshot into our bitmap.
            using Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen((int)left, (int)top, 0, 0, bmp.Size);
        }
        catch (Exception)
        {
            /* Nom Nom Nom */
        }

        return bmp;
    }

    [DllImport("user32.dll", SetLastError = false)]
    private static extern IntPtr GetDesktopWindow();

    public static Image CaptureDesktop()
    {
        return CaptureWindow(GetDesktopWindow());
    }

    /// <summary>
    /// Creates an Image object containing a screenshot of a specific window
    /// </summary>
    /// <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
    /// <returns></returns>
    public static Image CaptureWindow(IntPtr handle)
    {
        // get te hDC of the target window
        IntPtr hdcSrc = User32.GetWindowDC(handle);
        // get the size
        User32.RECT windowRect = new();
        User32.GetWindowRect(handle, ref windowRect);
        int left = windowRect.left;
        int top = windowRect.top;
        int width = windowRect.right - left;
        int height = windowRect.bottom - top;
        // create a device context we can copy to
        IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
        // create a bitmap we can copy it to,
        // using GetDeviceCaps to get the width/height
        IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc, width, height);
        // select the bitmap object
        IntPtr hOld = GDI32.SelectObject(hdcDest, hBitmap);
        // bitblt over
        GDI32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, GDI32.SRCCOPY);
        // restore selection
        GDI32.SelectObject(hdcDest, hOld);
        // clean up 
        GDI32.DeleteDC(hdcDest);
        User32.ReleaseDC(handle, hdcSrc);

        // get a .NET image object for it
        Image img = Image.FromHbitmap(hBitmap);
        // free up the Bitmap object
        GDI32.DeleteObject(hBitmap);

        return img;
    }
}