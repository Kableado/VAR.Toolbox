using System;
using System.Drawing;
using System.Windows.Forms;
using VAR.Toolbox.Code;
using VAR.Toolbox.Code.Windows;

namespace VAR.Toolbox.UI
{
    public class FrmCover : Form
    {
        #region Declarations

        private readonly Random _rnd = new Random();
        private readonly Timer _timer = new Timer();

        private readonly uint _mouseX = 0;
        private readonly uint _mouseY = 0;

        #endregion Declarations

        #region Form life cycle

        public FrmCover()
        {
            Mouse.GetPosition(out _mouseX, out _mouseY);

            Text = User32.GetActiveWindowTitle();

            TopMost = true;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.Black;

            Load += FrmCover_Load;
            Click += FrmCover_Click;
            KeyPress += FrmCover_KeyPress;

            _timer.Interval = 1000;
            _timer.Enabled = true;
            _timer.Tick += Timer_Tick;
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
            User32.SetForegroundWindow(Handle);
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

        private void FrmCover_KeyPress(object sender, KeyPressEventArgs e)
        {
            Cursor.Show();
            _timer.Stop();
            _timer.Enabled = false;
            Mouse.SetPosition(_mouseX, _mouseY);

            Close();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            User32.SetForegroundWindow(Handle);
            try
            {
                Mouse.Move(
                    (_rnd.Next() % 11) - 5,
                    (_rnd.Next() % 11) - 5);
            }
            catch (Exception) { } // ignore exceptions moving mouse
            _timer.Stop();
            _timer.Start();
        }

        #endregion UI events
    }
}