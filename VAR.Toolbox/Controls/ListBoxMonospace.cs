using System.Drawing;
using System.Windows.Forms;

namespace VAR.Toolbox.Controls
{
    public class ListBoxMonospace : ListBox
    {
        public ListBoxMonospace()
        {
            FormattingEnabled = true;
            Font = new Font("Consolas", ControlsUtils.GetFontSize(this, 9));
            BackColor = Color.Black;
            ForeColor = Color.Gray;
            SelectionMode = SelectionMode.MultiExtended;
        }
    }
}
