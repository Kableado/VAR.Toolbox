using System.Drawing;

namespace VAR.Toolbox.Controls
{
    public class CDateTimePicker : System.Windows.Forms.DateTimePicker
    {
        public CDateTimePicker()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            BackColor = Color.DarkSlateGray;
            ForeColor = Color.Gray;
        }
    }
}