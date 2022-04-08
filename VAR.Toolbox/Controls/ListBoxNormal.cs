using System.Drawing;
using System.Windows.Forms;

namespace VAR.Toolbox.Controls
{
    public class ListBoxNormal : ListBox
    {
        public ListBoxNormal()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            FormattingEnabled = true;
            Font = new Font("Microsoft Sans Serif", ControlsUtils.GetFontSize(this, 8.25f));
            BackColor = Color.Black;
            ForeColor = Color.Gray;
            BorderStyle = BorderStyle.FixedSingle;
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