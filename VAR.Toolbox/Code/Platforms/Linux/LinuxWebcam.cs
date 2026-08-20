using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using SkiaSharp;

namespace VAR.Toolbox.Code.Platforms.Linux;

public class LinuxWebcam : IWebcam
{
    private readonly string _moniker;
    private Process? _process;
    private Thread? _readerThread;
    private bool _active;

    public LinuxWebcam(string moniker)
    {
        _moniker = moniker;
    }

    public void Start()
    {
        if (_active) return;

        string[] attemptArgs = new[]
        {
            $"-hide_banner -loglevel error -f v4l2 -input_format mjpeg -video_size 1920x1080 -i \"{_moniker}\" -f image2pipe -c:v copy -",
            $"-hide_banner -loglevel error -f v4l2 -input_format mjpeg -video_size 1280x720 -i \"{_moniker}\" -f image2pipe -c:v copy -",
            $"-hide_banner -loglevel error -f v4l2 -i \"{_moniker}\" -f image2pipe -vcodec mjpeg -q:v 2 -",
        };

        foreach (string args in attemptArgs)
        {
            ProcessStartInfo psi = new("ffmpeg", args)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            try
            {
                _process = Process.Start(psi);
                if (_process == null) continue;

                // Give it a moment to see if it fails immediately
                Thread.Sleep(200);
                if (!_process.HasExited)
                {
                    _active = true;
                    _readerThread = new Thread(ReaderLoop) { IsBackground = true, };
                    _readerThread.Start();
                    return;
                }

                _process.Dispose();
                _process = null;
            }
            catch
            {
                // Try next
            }
        }

        throw new Exception("Failed to start webcam");
    }

    private void ReaderLoop()
    {
        if (_process == null) return;

        try
        {
            using Stream stream = _process.StandardOutput.BaseStream;
            byte[] buffer = new byte[2 * 1024 * 1024]; // 2MB buffer to hold at least one full HD frame
            int bufferCount = 0;

            while (_active && !_process.HasExited)
            {
                int read = stream.Read(buffer, bufferCount, buffer.Length - bufferCount);
                if (read <= 0) break;
                bufferCount += read;

                int pos = 0;
                while (pos < bufferCount - 1)
                {
                    // Search for MJPEG SOI (FF D8)
                    if (buffer[pos] == 0xFF && buffer[pos + 1] == 0xD8)
                    {
                        // Search for MJPEG EOI (FF D9)
                        int eoiPos = -1;
                        for (int i = pos + 2; i < bufferCount - 1; i++)
                        {
                            if (buffer[i] == 0xFF && buffer[i + 1] == 0xD9)
                            {
                                eoiPos = i + 2;
                                break;
                            }
                        }

                        if (eoiPos != -1)
                        {
                            // Found a full MJPEG frame
                            using (MemoryStream ms = new(buffer, pos, eoiPos - pos))
                            {
                                SKBitmap? bitmap = SKBitmap.Decode(ms);
                                if (bitmap != null)
                                {
                                    NewFrame?.Invoke(this, bitmap);
                                }
                            }
                            pos = eoiPos;
                            continue;
                        }
                        else
                        {
                            // SOI found but no EOI yet. Move the partial frame to the start and read more.
                            if (pos > 0)
                            {
                                Array.Copy(buffer, pos, buffer, 0, bufferCount - pos);
                                bufferCount -= pos;
                            }
                            goto next_read;
                        }
                    }
                    pos++;
                }

                // If we reach here, we didn't find a full frame or we processed all frames in the buffer.
                if (pos >= bufferCount - 1)
                {
                    if (pos >= bufferCount)
                    {
                        bufferCount = 0;
                    }
                    else
                    {
                        // Keep the last byte in case it's the start of a marker
                        buffer[0] = buffer[bufferCount - 1];
                        bufferCount = 1;
                    }
                }

                // Safety: if buffer is full and we still haven't found a full frame, clear it
                if (bufferCount == buffer.Length)
                {
                    bufferCount = 0;
                }

                next_read:;
            }
        }
        catch (Exception)
        {
            // Ignore errors during shutdown
        }
        finally
        {
            _active = false;
        }
    }

    public void Stop()
    {
        _active = false;
        if (_process != null)
        {
            try
            {
                if (!_process.HasExited)
                {
                    _process.Kill();
                }
            }
            catch
            {
                // ignored
            }

            _process.Dispose();
            _process = null;
        }

        _readerThread = null;
    }

    public bool Active => _active;

    public event IWebcam.NewFrameEventHandler? NewFrame;

    public static Dictionary<string, string> ListDevices()
    {
        Dictionary<string, string> devices = new();
        try
        {
            ProcessStartInfo psi = new("ffmpeg", "-sources v4l2")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = false,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            using Process? process = Process.Start(psi);
            if (process != null)
            {
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                // Parse output
                string[] lines = output.Split(new[] { '\r', '\n', }, StringSplitOptions.RemoveEmptyEntries);
                bool startParsing = false;
                foreach (string line in lines)
                {
                    if (line.Contains("Auto-detected sources"))
                    {
                        startParsing = true;
                        continue;
                    }

                    if (startParsing && line.Trim().StartsWith("/dev/video"))
                    {
                        string[] parts = line.Trim().Split(new[] { ' ', }, 2, StringSplitOptions.RemoveEmptyEntries);
                        string path = parts[0];

                        // Probe if it's a capture device
                        if (!IsCaptureDevice(path)) continue;

                        string name = path;
                        if (parts.Length > 1)
                        {
                            name = parts[1];
                            int openBracket = name.IndexOf('[');
                            int closeBracket = name.IndexOf(']');
                            if (openBracket >= 0 && closeBracket > openBracket)
                            {
                                name = name.Substring(openBracket + 1, closeBracket - openBracket - 1).Trim(':', ' ');
                            }
                        }

                        if (string.IsNullOrWhiteSpace(name)) name = path;

                        if (!devices.ContainsKey(name))
                        {
                            devices.Add(name, path);
                        }
                        else
                        {
                            devices.Add($"{name} ({path})", path);
                        }
                    }
                }
            }
        }
        catch
        {
            // ignored
        }

        // Fallback to simple listing if no devices found or ffmpeg failed
        if (devices.Count == 0)
        {
            string[] videoDevices = Directory.GetFiles("/dev", "video*");
            foreach (string videoDevice in videoDevices)
            {
                if (!devices.ContainsValue(videoDevice) && IsCaptureDevice(videoDevice))
                {
                    devices.Add(videoDevice, videoDevice);
                }
            }
        }

        return devices;
    }

    private static bool IsCaptureDevice(string device)
    {
        try
        {
            ProcessStartInfo psi = new("ffmpeg", $"-f v4l2 -list_formats all -i \"{device}\"")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            using Process? process = Process.Start(psi);
            if (process == null) return false;
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();
            return error.Contains("Compressed:") || error.Contains("Raw       :");
        }
        catch
        {
            return false;
        }
    }
}
