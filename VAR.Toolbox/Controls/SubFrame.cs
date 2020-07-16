namespace VAR.Toolbox.Controls
{
    public class SubFrame : System.Windows.Forms.UserControl
    {
        public SubFrame()
        {
            Font = new System.Drawing.Font(Font.Name, ControlsUtils.GetFontSize(this, 8.25f), Font.Style, Font.Unit, Font.GdiCharSet, Font.GdiVerticalFont);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
        }
    }
}
