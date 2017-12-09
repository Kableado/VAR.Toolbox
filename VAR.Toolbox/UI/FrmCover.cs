using System;
using System.Drawing;
using System.Windows.Forms;
using VAR.Toolbox.Code;

namespace VAR.Toolbox.UI
{
    internal class FrmCover : Form
    {
        #region Declarations

        private Random rnd = new Random();
        private Timer _timer = new Timer();

        private UInt32 _mouseX = 0;
        private UInt32 _mouseY = 0;

        #endregion Declarations

        #region Form life cycle

        public FrmCover()
        {
            Mouse.GetPosition(out _mouseX, out _mouseY);

            TopMost = true;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.Black;

            Load += FrmCover_Load;
            Click += FrmCover_Click;

            _timer.Interval = 1000;
            _timer.Enabled = true;
            _timer.Tick += timer_Tick;
        }

        private void FrmCover_Load(object sender, EventArgs e)
        {
            Rectangle r = new Rectangle();
            foreach (Screen s in Screen.AllScreens)
            {
                r = Rectangle.Union(r, s.Bounds);
            }
            Top = r.Top;
            Left = r.Left;
            Width = r.Width;
            Height = r.Height;
            Cursor.Hide();
            _timer.Start();
        }

        #endregion Form life cycle

        #region UI events

        private void FrmCover_Click(object sender, EventArgs e)
        {
            Cursor.Show();
            _timer.Stop();
            _timer.Enabled = false;
            Mouse.SetPosition(_mouseX, _mouseY);

            Close();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Mouse.Move(
                (rnd.Next() % 11) - 5,
                (rnd.Next() % 11) - 5);

            _timer.Stop();
            _timer.Start();
        }

        #endregion UI events
    }
}