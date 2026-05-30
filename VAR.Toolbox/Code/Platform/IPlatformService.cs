using System;

namespace VAR.Toolbox.Code.Platform;

public interface IPlatformService
{
    // Returns the title of the currently active/top window for the platform.
    string GetActiveWindowTitle();

    // Returns number of seconds since last input (keyboard/mouse) on the system.
    // If not available on the platform, return a large value.
    uint GetLastInputTimeSeconds();

    // Set a window (by handle) to topmost / not topmost. On platforms without
    // window handles (or if not applicable) this can be a no-op.
    void SetWindowTopMost(IntPtr windowHandle, bool top);

    // Mouse operations
    void MoveMouseRelative(int dx, int dy);
    void SetMouseButton(VAR.Toolbox.Code.Mouse.MouseButtons button, bool down);
    void GetCursorPosition(out uint x, out uint y);
    void SetCursorPosition(uint x, uint y);

    // Capture a window as a System.Drawing.Image. May return null on unsupported platforms.
    System.Drawing.Image? CaptureWindow(IntPtr handle);
}

