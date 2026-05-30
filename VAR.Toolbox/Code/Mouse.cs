using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Linq;
// removed unused/incorrect usings
using VAR.Toolbox.Code.Windows;

namespace VAR.Toolbox.Code;

public static class Mouse
{
    public enum MouseButtons
    {
        Left,
        Middle,
        Right,
    }

    // Cross-platform Move
    public static void Move(int dx, int dy)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            WindowsMove(dx, dy);
            return;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            // If running on Wayland, prefer portal path (requires xdg-desktop-portal)
            string sessionType = Environment.GetEnvironmentVariable("XDG_SESSION_TYPE") ?? string.Empty;
            if (sessionType.Equals("wayland", StringComparison.OrdinalIgnoreCase) || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WAYLAND_DISPLAY")))
            {
                var info = WaylandProbePortal();
                if (!info.IsAvailable)
                    throw new PlatformNotSupportedException("Wayland detected but xdg-desktop-portal not available: " + info.Error);
                // Portal present; injection implementation via xdg-desktop-portal not yet implemented in this version.
                throw new NotImplementedException("Wayland portal detected (" + string.Join(',', info.Interfaces) + ") but input injection via portal is not yet implemented. Run Mouse.WaylandProbePortal() to inspect available interfaces and request implementation.");
            }

            // Fallback to X11 implementation
            LinuxMove(dx, dy);
            return;
        }

        throw new PlatformNotSupportedException("Mouse operations are supported on Windows and Linux only.");
    }

    // Cross-platform button press/release
    public static void SetButton(MouseButtons button, bool down)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            WindowsSetButton(button, down);
            return;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            string sessionType = Environment.GetEnvironmentVariable("XDG_SESSION_TYPE") ?? string.Empty;
            if (sessionType.Equals("wayland", StringComparison.OrdinalIgnoreCase) || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WAYLAND_DISPLAY")))
            {
                var info = WaylandProbePortal();
                if (!info.IsAvailable)
                    throw new PlatformNotSupportedException("Wayland detected but xdg-desktop-portal not available: " + info.Error);
                throw new NotImplementedException("Wayland portal detected (" + string.Join(',', info.Interfaces) + ") but input injection via portal is not yet implemented. Run Mouse.WaylandProbePortal() to inspect available interfaces and request implementation.");
            }

            LinuxSetButton(button, down);
            return;
        }

        throw new PlatformNotSupportedException("Mouse operations are supported on Windows and Linux only.");
    }

    public static void Click(MouseButtons button)
    {
        SetButton(button, true);
        System.Threading.Thread.Sleep(100);
        SetButton(button, false);
    }

    public static void GetPosition(out UInt32 x, out UInt32 y)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            User32.GetCursorPos(out User32.POINT lpPoint);
            x = (UInt32)lpPoint.X;
            y = (UInt32)lpPoint.Y;
            return;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            IntPtr d = X11.XOpenDisplay(IntPtr.Zero);
            if (d == IntPtr.Zero)
                throw new Exception("Unable to open X display");
            try
            {
                IntPtr root = X11.XRootWindow(d, X11.XDefaultScreen(d));
                if (!X11.XQueryPointer(d, root, out _, out _, out int root_x, out int root_y, out _, out _, out _))
                    throw new Exception("XQueryPointer failed");
                x = (UInt32)root_x;
                y = (UInt32)root_y;
            }
            finally
            {
                X11.XFlush(d);
                X11.XCloseDisplay(d);
            }
            return;
        }

        throw new PlatformNotSupportedException("Mouse operations are supported on Windows and Linux only.");
    }

    public static void SetPosition(UInt32 x, UInt32 y)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            User32.SetCursorPos(x, y);
            return;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            IntPtr d = X11.XOpenDisplay(IntPtr.Zero);
            if (d == IntPtr.Zero)
                throw new Exception("Unable to open X display");
            try
            {
                int screen = X11.XDefaultScreen(d);
                // XTestFakeMotionEvent takes absolute coordinates
                if (X11.XTestFakeMotionEvent(d, screen, (int)x, (int)y, 0) == 0)
                    throw new Exception("XTestFakeMotionEvent failed");
                X11.XFlush(d);
            }
            finally
            {
                X11.XCloseDisplay(d);
            }
            return;
        }

        throw new PlatformNotSupportedException("Mouse operations are supported on Windows and Linux only.");
    }

    // Windows-specific implementations (reuse existing User32 wrapper)
    private static void WindowsMove(int dx, int dy)
    {
        User32.INPUT input = new()
        {
            Type = User32.INPUT_MOUSE,
        };
        input.Data.Mouse.X = dx;
        input.Data.Mouse.Y = dy;
        input.Data.Mouse.Flags = User32.MOUSEEVENTF_MOVE;
        User32.INPUT[] inputs = new User32.INPUT[] { input };
        if (User32.SendInput(1, inputs, Marshal.SizeOf(typeof(User32.INPUT))) == 0)
            throw new Exception("SendInput failed");
    }

    private static void WindowsSetButton(MouseButtons button, bool down)
    {
        User32.INPUT input = new()
        {
            Type = User32.INPUT_MOUSE,
        };
        input.Data.Mouse.X = 0;
        input.Data.Mouse.Y = 0;
        if (button == MouseButtons.Left)
            input.Data.Mouse.Flags = down ? User32.MOUSEEVENTF_LEFTDOWN : User32.MOUSEEVENTF_LEFTUP;
        else if (button == MouseButtons.Middle)
            input.Data.Mouse.Flags = down ? User32.MOUSEEVENTF_MIDDLEDOWN : User32.MOUSEEVENTF_MIDDLEUP;
        else if (button == MouseButtons.Right)
            input.Data.Mouse.Flags = down ? User32.MOUSEEVENTF_RIGHTDOWN : User32.MOUSEEVENTF_RIGHTUP;

        User32.INPUT[] inputs = new User32.INPUT[] { input };
        if (User32.SendInput(1, inputs, Marshal.SizeOf(typeof(User32.INPUT))) == 0)
            throw new Exception("SendInput failed");
    }

    // Linux (X11 + XTest) implementations
    private static void LinuxMove(int dx, int dy)
    {
        IntPtr d = X11.XOpenDisplay(IntPtr.Zero);
        if (d == IntPtr.Zero)
            throw new Exception("Unable to open X display");
        try
        {
            // use relative motion
            if (X11.XTestFakeRelativeMotionEvent(d, dx, dy, 0) == 0)
                throw new Exception("XTestFakeRelativeMotionEvent failed");
            X11.XFlush(d);
        }
        finally
        {
            X11.XCloseDisplay(d);
        }
    }

    private static void LinuxSetButton(MouseButtons button, bool down)
    {
        IntPtr d = X11.XOpenDisplay(IntPtr.Zero);
        if (d == IntPtr.Zero)
            throw new Exception("Unable to open X display");
        try
        {
            uint btn = button == MouseButtons.Left ? 1u : button == MouseButtons.Middle ? 2u : 3u;
            if (X11.XTestFakeButtonEvent(d, btn, down ? 1 : 0, 0) == 0)
                throw new Exception("XTestFakeButtonEvent failed");
            X11.XFlush(d);
        }
        finally
        {
            X11.XCloseDisplay(d);
        }
    }

    private static class X11
    {
        const string libX11 = "libX11.so.6";
        const string libXtst = "libXtst.so.6";

        [DllImport(libX11)]
        public static extern IntPtr XOpenDisplay(IntPtr display_name);

        [DllImport(libX11)]
        public static extern int XDefaultScreen(IntPtr display);

        [DllImport(libX11)]
        public static extern IntPtr XRootWindow(IntPtr display, int screen_number);

        [DllImport(libX11)]
        public static extern bool XQueryPointer(IntPtr display, IntPtr w, out IntPtr root_return, out IntPtr child_return, out int root_x_return, out int root_y_return, out int win_x_return, out int win_y_return, out uint mask_return);

        [DllImport(libX11)]
        public static extern int XFlush(IntPtr display);

        [DllImport(libX11)]
        public static extern int XCloseDisplay(IntPtr display);

        [DllImport(libXtst)]
        public static extern int XTestFakeMotionEvent(IntPtr display, int screen, int x, int y, ulong delay);

        [DllImport(libXtst)]
        public static extern int XTestFakeRelativeMotionEvent(IntPtr display, int x, int y, ulong delay);

        [DllImport(libXtst)]
        public static extern int XTestFakeButtonEvent(IntPtr display, uint button, int is_press, ulong delay);
    }

    // Probe xdg-desktop-portal availability and interfaces on the session bus.
    public struct PortalInfo
    {
        public bool IsAvailable { get; set; }
        public string[] Interfaces { get; set; }
        public string IntrospectXml { get; set; }
        public string Error { get; set; }
    }

    public static PortalInfo WaylandProbePortal()
    {
        // Try gdbus (glib)
        string[] tryCommands = new[]
        {
            "gdbus introspect --session --dest org.freedesktop.portal.Desktop --object-path /org/freedesktop/portal/desktop",
            // busctl --user introspect SERVICE PATH
            "busctl --user introspect org.freedesktop.portal.Desktop /org/freedesktop/portal/desktop",
            // dbus-send fallback: call Introspect method
            "dbus-send --session --dest=org.freedesktop.portal.Desktop --print-reply /org/freedesktop/portal/desktop org.freedesktop.DBus.Introspectable.Introspect"
        };

        foreach (var cmd in tryCommands)
        {
            try
            {
                var parts = SplitCommand(cmd);
                var psi = new ProcessStartInfo(parts.file, parts.args)
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                };
                using var p = Process.Start(psi);
                if (p == null)
                    continue;
                string outp = p.StandardOutput.ReadToEnd();
                string err = p.StandardError.ReadToEnd();
                p.WaitForExit(3000);
                if (p.ExitCode != 0 && string.IsNullOrWhiteSpace(outp))
                {
                    // try next
                    continue;
                }

                // Parse interfaces from introspect XML or busctl output
                string xml = outp;
                var interfaces = ParseInterfacesFromIntrospect(xml);
                if (interfaces.Length == 0 && !string.IsNullOrWhiteSpace(err))
                {
                    // sometimes busctl prints to stderr
                    interfaces = ParseInterfacesFromIntrospect(err);
                }

                return new PortalInfo
                {
                    IsAvailable = true,
                    Interfaces = interfaces,
                    IntrospectXml = xml,
                    Error = string.Empty
                };
            }
            catch (Exception ex)
            {
                // try next command
            }
        }

        return new PortalInfo { IsAvailable = false, Interfaces = Array.Empty<string>(), IntrospectXml = string.Empty, Error = "xdg-desktop-portal not found or introspect tools (gdbus/busctl/dbus-send) missing" };
    }

    private static (string file, string args) SplitCommand(string cmd)
    {
        int idx = cmd.IndexOf(' ');
        if (idx < 0)
            return (cmd, string.Empty);
        var file = cmd.Substring(0, idx);
        var args = cmd.Substring(idx + 1);
        return (file, args);
    }

    private static string[] ParseInterfacesFromIntrospect(string xml)
    {
        if (string.IsNullOrWhiteSpace(xml))
            return Array.Empty<string>();
        try
        {
            // simple regex to capture interface name="..."
            var matches = Regex.Matches(xml, "interface\\s+name=\"([^\"]+)\"");
            return matches.Cast<Match>().Select(m => m.Groups[1].Value).Distinct().ToArray();
        }
        catch
        {
            return Array.Empty<string>();
        }
    }
}