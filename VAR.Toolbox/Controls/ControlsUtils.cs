using System.Reflection;
using Avalonia.Controls;
using Avalonia.Threading;

namespace VAR.Toolbox.Controls;

public static class ControlsUtils
{
    public static float GetFontSize(object ctrl, float size)
    {
        return size;
    }

    public static void SetControlPropertyThreadSafe(
        Control control,
        string propertyName,
        object propertyValue)
    {
        Dispatcher.UIThread.Post(() =>
        {
            PropertyInfo? prop = control.GetType().GetProperty(propertyName);
            prop?.SetValue(control, propertyValue);
        });
    }
}