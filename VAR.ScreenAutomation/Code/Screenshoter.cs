using System;
using System.Drawing;
using System.Windows.Forms;

namespace VAR.ScreenAutomation.Code
{
    public class Screenshoter
    {
        public static Bitmap CaptureControl(Control ctrl, Bitmap bmp = null)
        {
            if (ctrl == null) { return bmp; }
            Point picCapturerOrigin = ctrl.PointToScreen(new Point(0, 0));
            bmp = CaptureScreen(bmp, picCapturerOrigin.X, picCapturerOrigin.Y, ctrl.Width, ctrl.Height);
            return bmp;
        }

        public static Bitmap CaptureScreen(Bitmap bmp = null, int? left = null, int? top = null, int? width = null, int? height = null)
        {
            if (width <= 0 || height <= 0) { return bmp; }

            // Determine the size of the "virtual screen", which includes all monitors.
            left = left ?? SystemInformation.VirtualScreen.Left;
            top = top ?? SystemInformation.VirtualScreen.Top;
            width = width ?? SystemInformation.VirtualScreen.Width;
            height = height ?? SystemInformation.VirtualScreen.Height;

            // Create a bitmap of the appropriate size to receive the screenshot.
            if (bmp == null || bmp?.Width != width || bmp?.Height != height)
            {
                bmp = new Bitmap(width ?? 0, height ?? 0);
            }

            try
            {
                // Draw the screenshot into our bitmap.
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(left ?? 0, top ?? 0, 0, 0, bmp.Size);
                }
            }
            catch (Exception) { /* Nom Nom Nom */}
            return bmp;
        }
    }
}
