﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace VAR.Toolbox.Controls
{
    public class CButton : Button
    {
        private readonly Brush _foreColorBrush;
        private readonly Brush _foreColorDisableBrush;
        private readonly Brush _backColorBrush;
        private readonly Brush _backColorOverBrush;
        private readonly Brush _backColorDownBrush;

        private bool _mouseIsDown;
        private bool _mouseIsOver;

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
                _mouseIsDown = r.Contains(mevent.X, mevent.Y);
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

        private readonly StringFormat _stringFormat = new StringFormat
            { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Rectangle rectangle = new Rectangle(0, 0, Width, Height);
            if (Enabled)
            {
                if (_mouseIsDown)
                {
                    pevent.Graphics.FillRectangle(_backColorDownBrush, rectangle);
                }
                else
                {
                    pevent.Graphics.FillRectangle(_mouseIsOver ? _backColorOverBrush : _backColorBrush, rectangle);
                }
            }
            else
            {
                pevent.Graphics.FillRectangle(_backColorBrush, rectangle);
            }

            pevent.Graphics.DrawString(Text, Font, Enabled ? _foreColorBrush : _foreColorDisableBrush, rectangle,
                _stringFormat);
        }
    }
}