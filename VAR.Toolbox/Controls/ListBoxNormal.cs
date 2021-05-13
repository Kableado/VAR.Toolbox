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
