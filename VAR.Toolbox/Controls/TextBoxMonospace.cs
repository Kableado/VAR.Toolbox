using System.Drawing;
using System.Windows.Forms;

namespace VAR.Toolbox.Controls
{
    public class TextBoxMonospace : TextBox
    {
        public TextBoxMonospace()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Font = new Font("Consolas", ControlsUtils.GetFontSize(this, 9.0f));
            BackColor = Color.FromArgb(255, 0, 0, 0);
            ForeColor = Color.FromArgb(255, 192, 192, 192);
            BorderStyle = BorderStyle.FixedSingle;
        }
    }
}