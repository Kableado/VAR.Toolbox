namespace VAR.Toolbox.Controls
{
    public class Frame : System.Windows.Forms.Form
    {
        public Frame()
        {
            Font = new System.Drawing.Font(Font.Name, ControlsUtils.GetFontSize(this, 8.25f), Font.Style, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
        }
    }
}
