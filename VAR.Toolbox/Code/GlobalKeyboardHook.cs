using System;
using System.Collections.Generic;
using VAR.Toolbox.Code.Windows;

namespace VAR.Toolbox.Code
{

// Provide a windows implementation when building for Windows; otherwise provide a harmless stub
#if WINDOWS
using System.Windows.Forms;
using VAR.Toolbox.Code.Windows;

public class GlobalKeyboardHook
{
    #region Declarations
    private bool _capturing;
    private bool _captureAll;
    /// <summary>
    /// The collections of keys to watch for
    /// </summary>
    private readonly List<Keys> _hookedKeys = new();
    /// <summary>
    /// Handle to the hook, need this to unhook and call the next hook
    /// </summary>
    private IntPtr _hHook = IntPtr.Zero;
    #endregion

    private int HookProc(int code, int wParam, ref User32.keyboardHookStruct lParam)
    {
        try
        {
            if (code >= 0)
            {
                Keys key = (Keys)lParam.vkCode;
                if (_hookedKeys.Contains(key) || _captureAll)
                {
                    KeyEventArgs kea = new(key);
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

    public event KeyEventHandler? KeyDown;
    public event KeyEventHandler? KeyUp;

    public void Start(bool all = false)
    {
        if (_capturing) { return; }
        _captureAll = all;
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

    public bool IsCapturing() => _capturing;

    public void AddHook(Keys key) => _hookedKeys.Add(key);
}
#else
// Minimal stub implementation for non-Windows builds. The platform-specific types (Keys, KeyEventArgs,
// KeyEventHandler) are provided in a separate PlatformStubs file under VAR.Toolbox.Code.Windows so they
// are always available to the code that needs them without polluting the VAR.Toolbox.Code namespace.
public class GlobalKeyboardHook
{
    private bool _capturing;
    public VAR.Toolbox.Code.Windows.KeyEventHandler? KeyDown;
    public VAR.Toolbox.Code.Windows.KeyEventHandler? KeyUp;

    public void Start(bool all = false) => _capturing = true;
    public void Stop() => _capturing = false;
    public bool IsCapturing() => _capturing;
    public void AddHook(VAR.Toolbox.Code.Windows.Keys key) { /* no-op on non-windows */ }
}
#endif
}
