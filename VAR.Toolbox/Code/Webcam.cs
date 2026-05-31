#pragma warning disable IDE0018
#pragma warning disable IDE0059
// ReSharper disable SuspiciousTypeConversion.Global
// ReSharper disable ConvertToAutoProperty

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using OpenCvSharp;
using VAR.Toolbox.Code.DirectShow;

namespace VAR.Toolbox.Code;

public class Webcam : IDisposable
{
    #region Declarations

    private VideoCapture? _capture;
    private Task? _captureTask;
    private CancellationTokenSource? _cts;
    private readonly int _deviceIndex;

    private int _width;
    private int _height;
    private int _bpp;

    private bool _active;

    private static Dictionary<string, string>? _deviceDescriptions;

    #endregion Declarations

    #region Properties

    public int Width => _width;

    public int Height => _height;

    public int BPP => _bpp;

    public bool Active => _active;

    #endregion Properties

    #region Lifecycle

    // The constructor accepts a "moniker" which for this OpenCvSharp implementation is
    // the device index as string ("0", "1", ...). We delay opening the device until Start()
    public Webcam(string monikerString)
    {
        if (!int.TryParse(monikerString, out _deviceIndex))
        {
            throw new ArgumentException("Moniker must be the device index (e.g. \"0\").", nameof(monikerString));
        }

        // default values until first frame
        _width = 0;
        _height = 0;
        _bpp = 24;
    }

    ~Webcam()
    {
        Dispose(false);
    }

    #endregion Lifecycle

    #region Public methods

    public void Start()
    {
        // Start in background to avoid blocking UI. Prefer using StartAsync for result.
        _ = StartAsync();
    }

    // Asynchronously open the camera and start capture loop. Returns true on success.
    public async Task<bool> StartAsync(int openTimeoutMs = 5000)
    {
        if (_active) return true;

        // ensure previous cancellation is cleaned
        try { _cts?.Dispose(); } catch { }
        _cts = new CancellationTokenSource();
        CancellationToken token = _cts.Token;

        VideoCapture? openedCapture = null;
        try
        {
            // Open capture on a threadpool thread to avoid blocking caller
            Task<VideoCapture?> openTask = Task.Run(() =>
            {
                // Try a set of backends that commonly work on Windows.
                VideoCapture cap = new();
                try
                {
                    // Prefer DirectShow on Windows
                    try
                    {
                        if (cap.Open(_deviceIndex, OpenCvSharp.VideoCaptureAPIs.DSHOW))
                        {
                            return cap;
                        }
                    }
                    catch { }

                    // Try Media Foundation
                    try
                    {
                        if (cap.Open(_deviceIndex, OpenCvSharp.VideoCaptureAPIs.MSMF))
                        {
                            return cap;
                        }
                    }
                    catch { }

                    // Fallback to default open
                    try
                    {
                        if (cap.Open(_deviceIndex))
                        {
                            return cap;
                        }
                    }
                    catch { }
                }
                catch { }

                try { cap.Dispose(); } catch { }
                return null;
            }, token);

            Task completed = await Task.WhenAny(openTask, Task.Delay(openTimeoutMs, token)).ConfigureAwait(false);
            if (completed != openTask)
            {
                // timeout or cancelled
                try { _cts.Cancel(); } catch { }
                return false;
            }

            openedCapture = await openTask.ConfigureAwait(false);
            if (openedCapture == null || !openedCapture.IsOpened())
            {
                try { openedCapture?.Dispose(); } catch { }
                return false;
            }

            // assign capture and start loop
            _capture = openedCapture;
            _active = true;
            _captureTask = Task.Run(async () => await CaptureLoop(token), token);
            return true;
        }
        catch (OperationCanceledException)
        {
            try { openedCapture?.Dispose(); } catch { }
            return false;
        }
        catch (Exception)
        {
            try { openedCapture?.Dispose(); } catch { }
            return false;
        }
    }

    public void Stop()
    {
        if (!_active) return;

        try
        {
            _cts?.Cancel();
            if (_captureTask != null)
            {
                _captureTask.Wait(1000);
            }
        }
        catch (AggregateException) { }
        catch (Exception) { }

        try
        {
            _capture?.Release();
            _capture?.Dispose();
        }
        catch (Exception) { }
        finally
        {
            _capture = null;
        }

        try
        {
            _cts?.Dispose();
        }
        catch (Exception) { }
        finally
        {
            _cts = null;
            _captureTask = null;
            _active = false;
        }
    }

    // Asynchronous enumeration of cameras that returns friendly names -> moniker (index as string)
    public static Task<Dictionary<string, string>> ListDevicesAsync(CancellationToken ct = default)
    {
        if (_deviceDescriptions != null) return Task.FromResult(_deviceDescriptions);

        return Task.Run(() =>
        {
            Dictionary<string, string> devices = new();

#if WINDOWS
            try
            {
                ICreateDevEnum devEnum = CreateInstanceFromClsid<ICreateDevEnum>(Clsid.SystemDeviceEnum);

                Guid category = FilterCategory.VideoInputDevice;
                int result = devEnum.CreateClassEnumerator(ref category, out IEnumMoniker enumMon, 0);
                if (result != 0)
                {
                    Marshal.ReleaseComObject(devEnum);
                    return devices;
                }

                IMoniker[] devMoniker = new IMoniker[1];
                IntPtr n = IntPtr.Zero;
                int index = 0;
                while (true)
                {
                    // Get next filter
                    result = enumMon.Next(1, devMoniker, n);
                    if ((result != 0))
                        break;

                    // Add device description (use enumerator order as index)
                    IMoniker mon = devMoniker[0];
                    string deviceName = new(GetMonikerName(mon).ToCharArray());
                    string deviceString = index.ToString();
                    devices.Add(deviceName, deviceString);
                    index++;

                    // Release COM object
                    Marshal.ReleaseComObject(devMoniker[0]);
                    devMoniker[0] = null!;
                }

                _deviceDescriptions = devices;

                Marshal.ReleaseComObject(devEnum);
                Marshal.ReleaseComObject(enumMon);
            }
            catch (Exception)
            {
                // swallow and fall back
            }
#else
            // Non-Windows fallback: probe indices (done off UI thread)
            const int maxProbe = 10;
            for (int i = 0; i <= maxProbe; i++)
            {
                using VideoCapture probe = new VideoCapture(i);
                if (probe.IsOpened())
                {
                    string name = $"Camera {i}";
                    string moniker = i.ToString();
                    devices.Add(name, moniker);
                    probe.Release();
                }
            }
            _deviceDescriptions = devices;
#endif

            return devices;
        }, ct);
    }

    // Synchronous wrapper for compatibility (calls async enumerator and waits)
    public static Dictionary<string, string> ListDevices()
    {
        return ListDevicesAsync().GetAwaiter().GetResult();
    }

    #endregion Public methods

    #region Private methods

#if WINDOWS
    private static T CreateInstanceFromClsid<T>(Guid clsid)
    {
        Type? srvType = Type.GetTypeFromCLSID(clsid);
        if (srvType == null)
            throw new ApplicationException("Failed creating device enumerator");

        object? comObj = Activator.CreateInstance(srvType);
        if (comObj == null) throw new ApplicationException("Failed creating COM instance");
        return (T)comObj;
    }

    // Get moniker string of the moniker
    private static string GetMonikerString(IMoniker moniker)
    {
        moniker.GetDisplayName(null!, null, out string str);
        return str;
    }

    // Get moniker friendly name
    private static string GetMonikerName(IMoniker moniker)
    {
        object? bagObj = null;

        try
        {
            Guid bagId = typeof(IPropertyBag).GUID;
            // get property bag of the moniker
            moniker.BindToStorage(null!, null!, ref bagId, out bagObj);
            IPropertyBag bag = (IPropertyBag)bagObj;

            // read FriendlyName
            object val = "";
            int hr = bag.Read("FriendlyName", ref val, IntPtr.Zero);
            if (hr != 0)
                Marshal.ThrowExceptionForHR(hr);

            // get it as string
            string ret = Convert.ToString(val) ?? string.Empty;
            if (ret.Length < 1) throw new ApplicationException();

            return ret;
        }
        catch (Exception)
        {
            return string.Empty;
        }
        finally
        {
            // release all COM objects
            if (bagObj != null)
            {
                Marshal.ReleaseComObject(bagObj);
            }
        }
    }
#endif

    private async Task CaptureLoop(CancellationToken token)
    {
        Mat mat = new();
        try
        {
            while (!token.IsCancellationRequested)
            {
                if (_capture == null || !_capture.IsOpened())
                {
                    await Task.Delay(50, token).ConfigureAwait(false);
                    continue;
                }

                bool ok = _capture.Read(mat);
                if (!ok || mat.Empty())
                {
                    // short delay to avoid busy loop
                    await Task.Delay(10, token).ConfigureAwait(false);
                    continue;
                }

                // Convert Mat (BGR) to System.Drawing.Bitmap using ImEncode -> Bitmap stream.
                Bitmap bitmap;
                try
                {
                    Cv2.ImEncode(".bmp", mat, out byte[] buf);
                    using MemoryStream ms = new(buf);
                    using Bitmap tmp = new Bitmap(ms);
                    // clone the bitmap to detach from the underlying stream
                    bitmap = new Bitmap(tmp);
                }
                catch (Exception)
                {
                    // conversion failed, skip frame
                    await Task.Delay(10, token).ConfigureAwait(false);
                    continue;
                }

                // update cached properties
                _width = bitmap.Width;
                _height = bitmap.Height;
                _bpp = Image.GetPixelFormatSize(bitmap.PixelFormat);

                // Raise event (subscribers should handle UI thread marshaling)
                try
                {
                    NewFrame?.Invoke(this, bitmap);
                }
                catch (Exception)
                {
                    // swallow subscriber exceptions to keep capture loop alive
                }

                // small pause - keep responsive but avoid spinning too fast
                await Task.Delay(1, token).ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            mat.Dispose();
        }
    }

    #endregion Private methods

    #region NewFrameEvent

    public delegate void NewFrameEventHandler(object? sender, Bitmap frame);

    public event NewFrameEventHandler? NewFrame;

    #endregion NewFrameEvent

    #region IDisposable

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Stop();
        }
    }

    #endregion IDisposable
}