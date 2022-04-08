using System.Drawing;
using System.Windows.Forms;

namespace VAR.Toolbox.Controls
{
    public class ListBoxMonospace : ListBox
    {
        public ListBoxMonospace()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
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
            ((HandledMouseEventArgs)e).Handled = true;
            const int Rows = 5;
            if (e.Delta > 0)
            {
                if (TopIndex < Rows) { TopIndex = 0; }
                else { TopIndex -= Rows; }
            }
            else
            {
                TopIndex += Rows;
            }
        }
    }
}