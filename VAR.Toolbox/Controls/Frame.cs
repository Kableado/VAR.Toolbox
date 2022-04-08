using System.Drawing;

namespace VAR.Toolbox.Controls
{
    public class Frame : System.Windows.Forms.Form
    {
        public Frame()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Font = new Font(Font.Name, ControlsUtils.GetFontSize(this, 8.25f), Font.Style, Font.Unit,
                Font.GdiCharSet, Font.GdiVerticalFont);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            BackColor = Color.FromArgb(255, 32, 32, 32);
            ForeColor = Color.FromArgb(255, 192, 192, 192);
        }
    }
}