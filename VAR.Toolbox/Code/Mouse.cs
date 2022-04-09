using System;
using System.Runtime.InteropServices;
using VAR.Toolbox.Code.Windows;

namespace VAR.Toolbox.Code
{
    public static class Mouse
    {
        public static void Move(int dx, int dy)
        {
            User32.INPUT input = new User32.INPUT
            {
                Type = User32.INPUT_MOUSE
            };
            input.Data.Mouse.X = dx;
            input.Data.Mouse.Y = dy;
            input.Data.Mouse.Flags = User32.MOUSEEVENTF_MOVE;
            User32.INPUT[] inputs = new User32.INPUT[] { input };
            if (User32.SendInput(1, inputs, Marshal.SizeOf(typeof(User32.INPUT))) == 0)
                throw new Exception();
        }

        public enum MouseButtons
        {
            Left,
            Middle,
            Right
        }

        public static void SetButton(MouseButtons button, bool down)
        {
            User32.INPUT input = new User32.INPUT
            {
                Type = User32.INPUT_MOUSE
            };
            input.Data.Mouse.X = 0;
            input.Data.Mouse.Y = 0;
            if (button == MouseButtons.Left)
            {
                input.Data.Mouse.Flags = down ? User32.MOUSEEVENTF_LEFTDOWN : User32.MOUSEEVENTF_LEFTUP;
            }

            if (button == MouseButtons.Middle)
            {
                input.Data.Mouse.Flags = down ? User32.MOUSEEVENTF_MIDDLEDOWN : User32.MOUSEEVENTF_MIDDLEUP;
            }

            if (button == MouseButtons.Right)
            {
                input.Data.Mouse.Flags = down ? User32.MOUSEEVENTF_RIGHTDOWN : User32.MOUSEEVENTF_RIGHTUP;
            }

            User32.INPUT[] inputs = new User32.INPUT[] { input };
            if (User32.SendInput(1, inputs, Marshal.SizeOf(typeof(User32.INPUT))) == 0)
                throw new Exception();
        }

        public static void Click(MouseButtons button)
        {
            SetButton(button, true);
            System.Threading.Thread.Sleep(500);
            SetButton(button, false);
        }

        public static void GetPosition(out UInt32 x, out UInt32 y)
        {
            User32.GetCursorPos(out User32.POINT lpPoint);
            x = lpPoint.X;
            y = lpPoint.Y;
        }

        public static void SetPosition(UInt32 x, UInt32 y)
        {
            User32.SetCursorPos(x, y);
        }
    }
}