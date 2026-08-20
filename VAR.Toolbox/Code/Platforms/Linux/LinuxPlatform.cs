using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using SkiaSharp;

namespace VAR.Toolbox.Code.Platforms.Linux;

internal class LinuxPlatform : IPlatform
{
    private static bool IsWayland() => Environment.GetEnvironmentVariable("XDG_SESSION_TYPE") == "wayland" ||
                                       Environment.GetEnvironmentVariable("WAYLAND_DISPLAY") != null;

    private static bool IsKde() =>
        (Environment.GetEnvironmentVariable("XDG_CURRENT_DESKTOP") ?? string.Empty).Contains("KDE");

    private static string RunCommand(string fileName, string arguments)
    {
        try
        {
            ProcessStartInfo psi = new()
            {
                FileName = fileName,
                Arguments = arguments,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            using Process? process = Process.Start(psi);
            if (process == null) return string.Empty;
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return output.Trim();
        }
        catch
        {
            return string.Empty;
        }
    }

    private static string RunKwinScript(string scriptContent, string marker)
    {
        string scriptFile = Path.Combine(Path.GetTempPath(), $"kwin_script_{Guid.NewGuid()}.js");
        try
        {
            File.WriteAllText(scriptFile, scriptContent);
            string loadResult = RunCommand("gdbus",
                $"call --session --dest org.kde.KWin --object-path /Scripting --method org.kde.kwin.Scripting.loadScript \"{scriptFile}\"");

            // Extract ID from (ID,)
            int start = loadResult.IndexOf('(');
            int end = loadResult.IndexOf(',');
            if (start >= 0 && end > start)
            {
                string scriptId = loadResult.Substring(start + 1, end - start - 1).Trim();
                RunCommand("gdbus",
                    $"call --session --dest org.kde.KWin --object-path /Scripting/Script{scriptId} --method org.kde.kwin.Script.run");

                // Read from journal
                string journal = RunCommand("journalctl", $"--user -t kwin_wayland -n 50");
                int markerIndex = journal.LastIndexOf(marker, StringComparison.Ordinal);
                if (markerIndex >= 0)
                {
                    string line = journal.Substring(markerIndex);
                    int colonIndex = line.IndexOf(':');
                    if (colonIndex >= 0)
                    {
                        return line.Substring(colonIndex + 1).Split('\n')[0].Trim();
                    }
                }
            }
        }
        catch { /* Ignore */ }
        finally
        {
            if (File.Exists(scriptFile)) File.Delete(scriptFile);
        }
        return string.Empty;
    }

    public string System_GetActiveWindowTitle()
    {
        if (IsWayland() && IsKde())
        {
            string marker = $"JUNIE_WINDOW_{Guid.NewGuid()}";
            string script = $"if (workspace.activeWindow) {{ console.log('{marker}:' + workspace.activeWindow.caption); }}";
            string result = RunKwinScript(script, marker);
            if (!string.IsNullOrEmpty(result))
            {
                return result;
            }
        }

        try
        {
            ProcessStartInfo psi = new()
            {
                FileName = "xprop",
                Arguments = "-root _NET_ACTIVE_WINDOW",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            using Process? process = Process.Start(psi);
            if (process == null) { return string.Empty; }
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            string[] parts = output.Split('#');
            if (parts.Length < 2) { return string.Empty; }
            string windowId = parts[1].Trim();

            psi.Arguments = $"-id {windowId} _NET_WM_NAME";
            using Process? process2 = Process.Start(psi);
            if (process2 == null) { return string.Empty; }
            output = process2.StandardOutput.ReadToEnd();
            process2.WaitForExit();

            int firstQuote = output.IndexOf('"');
            int lastQuote = output.LastIndexOf('"');
            if (firstQuote >= 0 && lastQuote > firstQuote)
            {
                return output.Substring(firstQuote + 1, lastQuote - firstQuote - 1);
            }

            // Fallback to WM_NAME
            psi.Arguments = $"-id {windowId} WM_NAME";
            using Process? process3 = Process.Start(psi);
            if (process3 == null) { return string.Empty; }
            output = process3.StandardOutput.ReadToEnd();
            process3.WaitForExit();

            firstQuote = output.IndexOf('"');
            lastQuote = output.LastIndexOf('"');
            if (firstQuote >= 0 && lastQuote > firstQuote)
            {
                return output.Substring(firstQuote + 1, lastQuote - firstQuote - 1);
            }
        }
        catch (Exception)
        {
            // Ignore
        }
        return string.Empty;
    }

    public uint System_GetLastInputTime()
    {
        return 0;
    }

    public bool System_SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent)
    {
        try
        {
            Process.Start("systemctl", hibernate ? "hibernate" : "suspend");
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public void Mouse_Move(int dx, int dy)
    {
        IntPtr display = X11.XOpenDisplay(null);
        if (display == IntPtr.Zero) return;
        try
        {
            X11.XWarpPointer(display, IntPtr.Zero, IntPtr.Zero, 0, 0, 0, 0, dx, dy);
            X11.XFlush(display);
        }
        finally
        {
            X11.XCloseDisplay(display);
        }
    }

    public void Mouse_GetPosition(out uint x, out uint y)
    {
        x = 0;
        y = 0;

        if (IsWayland() && IsKde())
        {
            string marker = $"JUNIE_MOUSE_{Guid.NewGuid()}";
            string script = $"console.log('{marker}:' + workspace.cursorPos.x + ':' + workspace.cursorPos.y);";
            string result = RunKwinScript(script, marker);
            if (!string.IsNullOrEmpty(result))
            {
                string[] parts = result.Split(':');
                if (parts.Length >= 2)
                {
                    if (uint.TryParse(parts[0], out uint px) && uint.TryParse(parts[1], out uint py))
                    {
                        x = px;
                        y = py;
                        return;
                    }
                }
            }
        }

        IntPtr display = X11.XOpenDisplay(null);
        if (display == IntPtr.Zero) return;
        try
        {
            IntPtr rootWindow = X11.XDefaultRootWindow(display);
            X11.XQueryPointer(display, rootWindow, out _, out _, out int rootX, out int rootY, out _, out _, out _);
            x = (uint)rootX;
            y = (uint)rootY;
        }
        finally
        {
            X11.XCloseDisplay(display);
        }
    }

    public void Mouse_SetPosition(uint x, uint y)
    {
        IntPtr display = X11.XOpenDisplay(null);
        if (display == IntPtr.Zero) return;
        try
        {
            IntPtr rootWindow = X11.XDefaultRootWindow(display);
            X11.XWarpPointer(display, IntPtr.Zero, rootWindow, 0, 0, 0, 0, (int)x, (int)y);
            X11.XFlush(display);
        }
        finally
        {
            X11.XCloseDisplay(display);
        }
    }

    public SKBitmap Screen_CaptureRegion(SKBitmap? bmp, int left, int top, int width, int height)
    {
        string tempFile = Path.Combine(Path.GetTempPath(), $"screenshot_{Guid.NewGuid()}.png");
        try
        {
            if (IsWayland() && IsKde())
            {
                // On Wayland, magick import doesn't work well for global screen.
                // We use spectacle to capture the whole screen and then crop.
                RunCommand("spectacle", $"-b -n -o \"{tempFile}\"");
            }
            else
            {
                ProcessStartInfo psi = new()
                {
                    FileName = "magick",
                    Arguments = $"import -window root -crop {width}x{height}+{left}+{top} \"{tempFile}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };
                using Process? process = Process.Start(psi);
                process?.WaitForExit();
            }

            if (File.Exists(tempFile))
            {
                using SKBitmap capturedBmp = SKBitmap.Decode(tempFile);
                SKBitmap resultBmp = bmp ?? new SKBitmap(width, height);
                using SKCanvas canvas = new(resultBmp);

                if (IsWayland() && IsKde())
                {
                    // Crop from the full screenshot
                    canvas.DrawBitmap(capturedBmp, new SKRect(left, top, left + width, top + height), new SKRect(0, 0, width, height));
                }
                else
                {
                    canvas.DrawBitmap(capturedBmp, 0, 0);
                }
                return resultBmp;
            }
        }
        catch (Exception)
        {
            // Ignore
        }
        finally
        {
            if (File.Exists(tempFile)) { File.Delete(tempFile); }
        }

        return bmp ?? new SKBitmap(width, height);
    }

    public IWebcam Webcam_Create(string moniker)
    {
        return new LinuxWebcam(moniker);
    }

    public Dictionary<string, string> Webcam_ListDevices()
    {
        return LinuxWebcam.ListDevices();
    }
}
