using System.Drawing;
using System.Windows.Forms;

namespace VAR.Toolbox.Controls
{
    public class CComboBox : System.Windows.Forms.ComboBox
    {
        public CComboBox()
        {
            BackColor = Color.FromArgb(255, 0, 0, 0);
            ForeColor = Color.FromArgb(255, 192, 192, 192);

            FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            pevent.Graphics.FillRectangle(Brushes.CadetBlue, pevent.ClipRectangle);
        }
    }
}
