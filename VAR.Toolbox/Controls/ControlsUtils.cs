using System.Reflection;
using System.Windows.Forms;

namespace VAR.Toolbox.Controls
{
    public static class ControlsUtils
    {
        public static float GetFontSize(Control ctrl, float size)
        {
            return size * 96f / ctrl.CreateGraphics().DpiX;
        }

        private delegate void SetControlPropertyThreadSafeDelegate(
            Control control,
            string propertyName,
            object propertyValue);

        public static void SetControlPropertyThreadSafe(
            Control control,
            string propertyName,
            object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new SetControlPropertyThreadSafeDelegate
                        (SetControlPropertyThreadSafe),
                    new[] { control, propertyName, propertyValue });
            }
            else
            {
                control.GetType().InvokeMember(
                    propertyName,
                    BindingFlags.SetProperty,
                    null,
                    control,
                    new[] { propertyValue });
            }
        }
    }
}