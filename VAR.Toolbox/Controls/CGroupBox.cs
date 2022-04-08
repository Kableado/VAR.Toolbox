using System.Drawing;
using System.Windows.Forms;

namespace VAR.Toolbox.Controls
{
    public class CGroupBox : GroupBox
    {
        public CGroupBox()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            BackColor = Color.FromArgb(255, 32, 32, 32);
            ForeColor = Color.FromArgb(255, 192, 192, 192);
            BorderColor = Color.FromArgb(255, 64, 64, 64);
            FlatStyle = FlatStyle.Flat;
        }

        private Color _borderColor = Color.Black;

        public Color BorderColor
        {
            get => _borderColor;
            set => _borderColor = value;
        }

        private SolidBrush _brushBackColor;
        private SolidBrush _brushForeColor;

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_brushBackColor == null || _brushBackColor.Color != BackColor)
            {
                _brushBackColor = new SolidBrush(BackColor);
            }

            if (_brushForeColor == null || _brushForeColor.Color != ForeColor)
            {
                _brushForeColor = new SolidBrush(ForeColor);
            }

            Size tSize = TextRenderer.MeasureText(Text, Font);

            Rectangle borderRect = new Rectangle(0, 0, Width, Height);
            borderRect.Y = (borderRect.Y + (tSize.Height / 2));
            borderRect.Height = (borderRect.Height - (tSize.Height / 2));
            ControlPaint.DrawBorder(e.Graphics, borderRect, _borderColor, ButtonBorderStyle.Solid);

            Rectangle textRect = new Rectangle(0, 0, Width, Height);
            textRect.X = (textRect.X + 6);
            textRect.Width = tSize.Width;
            textRect.Height = tSize.Height;
            e.Graphics.FillRectangle(_brushBackColor, textRect);
            e.Graphics.DrawString(Text, Font, _brushForeColor, textRect);
        }
    }
}