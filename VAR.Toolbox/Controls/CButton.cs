﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace VAR.Toolbox.Controls
{
    public class CButton : System.Windows.Forms.Button
    {
        private Brush _foreColorBrush;
        private Brush _foreColorDisableBrush;
        private Brush _backColorBrush;
        private Brush _backColorOverBrush;
        private Brush _backColorDownBrush;

        private bool _mouseIsDown = false;
        private bool _mouseIsOver = false;

        public CButton()
        {
            _foreColorBrush = new SolidBrush(Color.FromArgb(255, 192, 192, 192));
            _foreColorDisableBrush = new SolidBrush(Color.FromArgb(255, 96, 96, 96));
            _backColorBrush = new SolidBrush(Color.FromArgb(255, 64, 64, 64));
            _backColorOverBrush = new SolidBrush(Color.FromArgb(255, 128, 0, 0));
            _backColorDownBrush = new SolidBrush(Color.FromArgb(255, 192, 64, 64));
        }

        protected override void OnLostFocus(EventArgs e)
        {
            _mouseIsDown = false;
            base.OnLostFocus(e);
        }

        protected override void OnMouseEnter(EventArgs eventargs)
        {
            _mouseIsOver = true;
            base.OnMouseEnter(eventargs);
        }

        protected override void OnMouseLeave(EventArgs eventargs)
        {
            _mouseIsOver = false;
            base.OnMouseLeave(eventargs);
        }

        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            if (mevent.Button != MouseButtons.None)
            {
                Rectangle r = ClientRectangle;
                if (!r.Contains(mevent.X, mevent.Y))
                {
                    _mouseIsDown = false;
                }
                else
                {
                    _mouseIsDown = true;
                }
            }
            base.OnMouseMove(mevent);
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            _mouseIsDown = true;
            base.OnMouseDown(mevent);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            _mouseIsDown = false;
            base.OnMouseUp(mevent);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            if (Enabled)
            {
                if (_mouseIsDown)
                {
                    pevent.Graphics.FillRectangle(_backColorDownBrush, pevent.ClipRectangle);
                }
                else
                {
                    if (_mouseIsOver)
                    {
                        pevent.Graphics.FillRectangle(_backColorOverBrush, pevent.ClipRectangle);
                    }
                    else
                    {
                        pevent.Graphics.FillRectangle(_backColorBrush, pevent.ClipRectangle);
                    }
                }
            }
            else
            {
                pevent.Graphics.FillRectangle(_backColorBrush, pevent.ClipRectangle);
            }

            var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            pevent.Graphics.DrawString(Text, Font, Enabled ? _foreColorBrush : _foreColorDisableBrush, pevent.ClipRectangle, sf);
            sf.Dispose();
        }

    }
}