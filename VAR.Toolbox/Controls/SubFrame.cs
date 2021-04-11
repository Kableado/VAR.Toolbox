using System.Drawing;

namespace VAR.Toolbox.Controls
{
    public class SubFrame : System.Windows.Forms.UserControl
    {
        public SubFrame()
        {
            Font = new System.Drawing.Font(Font.Name, ControlsUtils.GetFontSize(this, 8.25f), Font.Style, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            BackColor = Color.FromArgb(255, 32, 32, 32);
            ForeColor = Color.FromArgb(255, 192, 192, 192);
        }
    }
}
