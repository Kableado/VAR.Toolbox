using System.Windows.Forms;

namespace VAR.Toolbox.Controls
{
    public static class ControlsUtils
    {
        public static float GetFontSize(Control ctrl, float size)
        {
            return size * 96f / ctrl.CreateGraphics().DpiX;
        }
    }
}
