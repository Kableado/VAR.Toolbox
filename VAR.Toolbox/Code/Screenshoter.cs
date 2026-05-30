using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;

using VAR.Toolbox.Code.Windows;

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

        double scale = 1.0;
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

    // Use platform-safe User32 wrapper instead of direct P/Invoke here
    public static Image CaptureDesktop()
    {
        return CaptureWindow(User32.GetDesktopWindow());
    }

    /// <summary>
    /// Creates an Image object containing a screenshot of a specific window
    /// </summary>
    /// <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
    /// <returns></returns>
    public static Image CaptureWindow(IntPtr handle)
    {
        // Delegate to platform-specific implementation. On Linux this may return null.
        System.Drawing.Image? img = VAR.Toolbox.Code.Platform.Platform.Current.CaptureWindow(handle);
        if (img != null) return img;

        // Fallback: create empty bitmap if capture not supported on platform
        return new System.Drawing.Bitmap(1, 1);
    }
}