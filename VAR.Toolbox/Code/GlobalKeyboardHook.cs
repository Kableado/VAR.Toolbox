using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows.Forms;
using VAR.Toolbox.Code.Windows;

namespace VAR.Toolbox.Code
{
    public class GlobalKeyboardHook
    {
        #region Declarations
        
        private bool _capturing = false;
        private bool _captureAll = false;
        
        /// <summary>
        /// The collections of keys to watch for
        /// </summary>
        private readonly List<Keys> _hookedKeys = new List<Keys>();
        
        /// <summary>
        /// Handle to the hook, need this to unhook and call the next hook
        /// </summary>
        private IntPtr _hHook = IntPtr.Zero;
        
        #endregion Declarations
        
        #region Private methods
        
        /// <summary>
        /// The callback for the keyboard hook
        /// </summary>
        /// <param name="code">The hook code, if it isn't >= 0, the function shouldn't do anyting</param>
        /// <param name="wParam">The event type</param>
        /// <param name="lParam">The keyhook event information</param>
        /// <returns></returns>
        private int HookProc(int code, int wParam, ref User32.keyboardHookStruct lParam) {
                try
                {
                    if (code >= 0)
                    {
                        Keys key = (Keys)lParam.vkCode;
                        if (_hookedKeys.Contains(key) || _captureAll)
                        {
                            KeyEventArgs kea = new KeyEventArgs(key);
                            if ((wParam == User32.WM_KEYDOWN || wParam == User32.WM_SYSKEYDOWN) && (KeyDown != null))
                            {
                                KeyDown(this, kea);
                            }
                            else if ((wParam == User32.WM_KEYUP || wParam == User32.WM_SYSKEYUP) && (KeyUp != null))
                            {
                                KeyUp(this, kea);
                            }

                            if (kea.Handled)
                                return 1;
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }

                return User32.CallNextHookEx(_hHook, code, wParam, ref lParam);
        }

        #endregion Private methods
        
        #region Public events
        
        /// <summary>
        /// Occurs when one of the hooked keys is pressed
        /// </summary>
        public event KeyEventHandler KeyDown;
        
        /// <summary>
        /// Occurs when one of the hooked keys is released
        /// </summary>
        public event KeyEventHandler KeyUp;
        
        #endregion Public events
        
        #region Public methods
        
        public void Start(bool all = false)
        {
            if (_capturing) { return; }

            _captureAll = all;
            
            //IntPtr hInstance = User32.LoadLibrary("User32");
            //_hHook = User32.SetWindowsHookEx(User32.WH_KEYBOARD_LL, HookProc, hInstance, 0);
            IntPtr iModule = System.Runtime.InteropServices.Marshal.GetHINSTANCE(
                    System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0]);
            _hHook = User32.SetWindowsHookEx(User32.WH_KEYBOARD_LL, HookProc, iModule, 0);
            
            _capturing = true;
        }

        public void Stop()
        {
            if (_capturing == false) { return; }
            
            User32.UnhookWindowsHookEx(_hHook);
            _capturing = false;
        }

        public bool IsCapturing()
        {
            return _capturing;
        }

        public void AddHook(Keys key)
        {
            _hookedKeys.Add(key);
        }
        
        #endregion Public methods
    }
}