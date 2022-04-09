using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using VAR.Toolbox.Code.Windows;

namespace VAR.Toolbox.Code
{
    public static class Screenshoter
    {
        public static Bitmap CaptureControl(Control ctrl, Bitmap bmp = null)
        {
            if (ctrl == null) { return bmp; }

            Point picCapturerOrigin = ctrl.PointToScreen(new Point(0, 0));
            bmp = CaptureScreen(bmp, picCapturerOrigin.X, picCapturerOrigin.Y, ctrl.Width, ctrl.Height);
            return bmp;
        }

        public static Bitmap CaptureScreen(Bitmap bmp = null, int? left = null, int? top = null, int? width = null,
            int? height = null)
        {
            if (width <= 0 || height <= 0) { return bmp; }

            // Determine the size of the "virtual screen", which includes all monitors.
            left = left ?? SystemInformation.VirtualScreen.Left;
            top = top ?? SystemInformation.VirtualScreen.Top;
            width = width ?? SystemInformation.VirtualScreen.Width;
            height = height ?? SystemInformation.VirtualScreen.Height;

            // Create a bitmap of the appropriate size to receive the screenshot.
            if (bmp == null || bmp.Width != width || bmp.Height != height)
            {
                bmp = new Bitmap((int)width, (int)height);
            }

            try
            {
                // Draw the screenshot into our bitmap.
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen((int)left, (int)top, 0, 0, bmp.Size);
                }
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
        /// Creates an Image object containing a screen shot of a specific window
        /// </summary>
        /// <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
        /// <returns></returns>
        public static Image CaptureWindow(IntPtr handle)
        {
            // get te hDC of the target window
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            // get the size
            User32.RECT windowRect = new User32.RECT();
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
}