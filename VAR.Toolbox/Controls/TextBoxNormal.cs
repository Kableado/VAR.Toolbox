using System.Drawing;
using System.Windows.Forms;

namespace VAR.Toolbox.Controls
{
    public class TextBoxNormal : TextBox
    {
        public TextBoxNormal()
        {
            Font = new Font("Microsoft Sans Serif", ControlsUtils.GetFontSize(this, 8.25f));
            BackColor = Color.FromArgb(255, 0, 0, 0);
            ForeColor = Color.FromArgb(255, 192, 192, 192);
            BorderStyle = BorderStyle.FixedSingle;
        }
    }
}
