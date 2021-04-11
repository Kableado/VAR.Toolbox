using System.Drawing;
using System.Windows.Forms;

namespace VAR.Toolbox.Controls
{
    public class CGroupBox : System.Windows.Forms.GroupBox
    {
        public CGroupBox()
        {
            BackColor = Color.FromArgb(255, 32, 32, 32);
            ForeColor = Color.FromArgb(255, 192, 192, 192);
            BorderColor = Color.FromArgb(255, 64, 64, 64);
            FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        }

        private Color _borderColor = Color.Black;

        public Color BorderColor
        {
            get { return this._borderColor; }
            set { this._borderColor = value; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Size tSize = TextRenderer.MeasureText(this.Text, this.Font);

            Rectangle borderRect = e.ClipRectangle;
            borderRect.Y = (borderRect.Y + (tSize.Height / 2));
            borderRect.Height = (borderRect.Height - (tSize.Height / 2));
            ControlPaint.DrawBorder(e.Graphics, borderRect, this._borderColor, ButtonBorderStyle.Solid);

            Rectangle textRect = e.ClipRectangle;
            textRect.X = (textRect.X + 6);
            textRect.Width = tSize.Width;
            textRect.Height = tSize.Height;
            e.Graphics.FillRectangle(new SolidBrush(this.BackColor), textRect);
            e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), textRect);
        }
    }
}
