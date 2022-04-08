using System.Drawing;
using System.Windows.Forms;

namespace VAR.Toolbox.Controls
{
    public class CComboBox : ComboBox
    {
        public CComboBox()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            BackColor = Color.FromArgb(255, 0, 0, 0);
            ForeColor = Color.FromArgb(255, 192, 192, 192);

            FlatStyle = FlatStyle.Flat;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Rectangle rectangle = new Rectangle(0, 0, Width, Height);
            pevent.Graphics.FillRectangle(Brushes.CadetBlue, rectangle);
        }
    }
}