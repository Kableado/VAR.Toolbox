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
        // Delegate to platform abstraction which encapsulates platform-specific details.
        VAR.Toolbox.Code.Platform.Platform.Current.MoveMouseRelative(dx, dy);
    }

    // Cross-platform button press/release
    public static void SetButton(MouseButtons button, bool down)
    {
        VAR.Toolbox.Code.Platform.Platform.Current.SetMouseButton(button, down);
    }

    public static void Click(MouseButtons button)
    {
        SetButton(button, true);
        System.Threading.Thread.Sleep(100);
        SetButton(button, false);
    }

    public static void GetPosition(out UInt32 x, out UInt32 y)
    {
        VAR.Toolbox.Code.Platform.Platform.Current.GetCursorPosition(out x, out y);
    }

    public static void SetPosition(UInt32 x, UInt32 y)
    {
        VAR.Toolbox.Code.Platform.Platform.Current.SetCursorPosition(x, y);
    }

    // Platform-specific implementations are provided by Platform.Current

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