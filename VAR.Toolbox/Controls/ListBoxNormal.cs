using System.Drawing;
using System.Windows.Forms;

namespace VAR.Toolbox.Controls
{
    public class ListBoxNormal : ListBox
    {
        public ListBoxNormal()
        {
            FormattingEnabled = true;
            Font = new Font("Microsoft Sans Serif", ControlsUtils.GetFontSize(this, 8.25f));
            BackColor = Color.Black;
            ForeColor = Color.Gray;
            BorderStyle = BorderStyle.FixedSingle;
        }
    }
}
