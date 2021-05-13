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
            BorderStyle = BorderStyle.FixedSingle;
            SelectionMode = SelectionMode.MultiExtended;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            (e as HandledMouseEventArgs).Handled = true;
            const int rows = 5;
            if (e.Delta > 0)
            {
                if (TopIndex < rows) { TopIndex = 0; }
                else { TopIndex -= rows; }
            }
            else
            {
                TopIndex += rows;
            }
        }
    }
}
