using System;
using System.Drawing;
using System.Runtime.InteropServices;

using Image = System.Drawing.Image;

namespace VAR.Toolbox.Code.Platforms.Windows;

public static class Screenshoter
{
    public static Bitmap CaptureScreenRegion(Bitmap? bmp, int left, int top, int width, int height)
    {
        // Create a bitmap of the appropriate size to receive the screenshot.
        if (bmp == null || bmp.Width != width || bmp.Height != height)
        {
            bmp = new Bitmap(width, height);
        }

        try
        {
            // Draw the screenshot into our bitmap.
            using Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(left, top, 0, 0, bmp.Size);
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