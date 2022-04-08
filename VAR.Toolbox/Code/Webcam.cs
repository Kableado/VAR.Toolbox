#pragma warning disable IDE0018
#pragma warning disable IDE0059
// ReSharper disable SuspiciousTypeConversion.Global
// ReSharper disable ConvertToAutoProperty

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using VAR.Toolbox.Code.DirectShow;
using VAR.Toolbox.Code.Windows;


namespace VAR.Toolbox.Code
{
    public class Webcam
    {
        #region Declarations

        private readonly IMediaControl _control;
        private readonly IBaseFilter _sourceFilter;
        private readonly IBaseFilter _sampleGrabberFilter;
        private readonly IBaseFilter _nullRenderer;

        private readonly int _width;
        private readonly int _height;
        private readonly int _bpp;

        private bool _active;

        private static Dictionary<string, string> _deviceDescriptions;

        #endregion Declarations

        #region Properties

        public int Width => _width;

        public int Height => _height;

        public int BPP => _bpp;

        public bool Active => _active;

        #endregion Properties

        #region Lifecycle

        public Webcam(string monikerString)
        {
            IFilterGraph2 graph = CreateInstanceFromClsid<IFilterGraph2>(Clsid.FilterGraph);
            ICaptureGraphBuilder2 capture = CreateInstanceFromClsid<ICaptureGraphBuilder2>(Clsid.CaptureGraphBuilder2);
            _control = (IMediaControl)graph;
            capture.SetFiltergraph((IGraphBuilder)graph);

            int n = 0;

            if (Win32.CreateBindCtx(0, out IBindCtx bindCtx) != 0)
            {
                throw new Exception("Failed to create binding context");
            }

            if (Win32.MkParseDisplayName(bindCtx, monikerString, ref n, out IMoniker moniker) != 0)
            {
                throw new Exception("Failed to create binding moniker");
            }

            graph.AddSourceFilterForMoniker(moniker, bindCtx, monikerString, out _sourceFilter);

            _sampleGrabberFilter = CreateInstanceFromClsid<IBaseFilter>(Clsid.SampleGrabber);
            graph.AddFilter(_sampleGrabberFilter, $"SampleGrabber {monikerString}");

            ISampleGrabber sampleGrabber = (ISampleGrabber)_sampleGrabberFilter;

            // Set media type
            AMMediaType mediaType = new AMMediaType
            {
                MajorType = MediaType.Video,
                SubType = MediaSubType.RGB24
            };
            sampleGrabber.SetMediaType(mediaType);

            var grabber = new Grabber(this);
            int result = sampleGrabber.SetCallback(grabber, 1);
            if (result < 0) throw new Exception("Failure creating Webcam device");

            //set the null renderer
            _nullRenderer = CreateInstanceFromClsid<IBaseFilter>(Clsid.NullRenderer);
            graph.AddFilter(_nullRenderer, $"NullRenderer {monikerString}");

            result = capture.RenderStream(PinCategory.Preview, MediaType.Video, _sourceFilter, _sampleGrabberFilter,
                _nullRenderer);
            if (result < 0) throw new Exception("Failure creating Webcam device");

            AMMediaType queryMediaType = new AMMediaType();
            result = sampleGrabber.GetConnectedMediaType(queryMediaType);
            if (result == 0)
            {
                if (queryMediaType.FormatType == FormatType.VideoInfo)
                {
                    VideoInfoHeader videoInfo =
                        (VideoInfoHeader)Marshal.PtrToStructure(queryMediaType.FormatPtr, typeof(VideoInfoHeader));
                    _width = videoInfo.BmiHeader.Width;
                    _height = videoInfo.BmiHeader.Height;
                    _bpp = videoInfo.BmiHeader.BitCount;
                }
            }

            _control.Run();
            Stop();
        }

        #endregion Lifecycle

        #region Public methods

        public void Start()
        {
            _control.Run();
            int result = _nullRenderer.Run(0);
            if (result < 0) throw new Exception("Webcam Start failure");
            result = _sampleGrabberFilter.Run(0);
            if (result < 0) throw new Exception("Webcam Start failure");
            result = _sourceFilter.Run(0);
            if (result < 0) throw new Exception("Webcam Start failure");
            _active = true;
        }

        public void Stop()
        {
            int result = _sourceFilter.Stop();
            if (result < 0) throw new Exception("Webcam Stop failure");
            result = _sampleGrabberFilter.Stop();
            if (result < 0) throw new Exception("Webcam Stop failure");
            result = _nullRenderer.Stop();
            if (result < 0) throw new Exception("Webcam Stop failure");
            _control.Stop();
            _active = false;
        }

        public static Dictionary<string, string> ListDevices()
        {
            if (_deviceDescriptions != null) { return _deviceDescriptions; }

            Dictionary<string, string> devices = new Dictionary<string, string>();
            ICreateDevEnum devEnum = CreateInstanceFromClsid<ICreateDevEnum>(Clsid.SystemDeviceEnum);

            Guid category = FilterCategory.VideoInputDevice;
            int result = devEnum.CreateClassEnumerator(ref category, out IEnumMoniker enumMon, 0);
            if (result != 0)
                throw new ApplicationException("No devices of the category");

            IMoniker[] devMoniker = new IMoniker[1];
            IntPtr n = IntPtr.Zero;
            while (true)
            {
                // Get next filter
                result = enumMon.Next(1, devMoniker, n);
                if ((result != 0) || (devMoniker[0] == null))
                    break;

                // Add device description
                string deviceName = new string(GetMonikerName(devMoniker[0]).ToCharArray());
                string deviceString = new string(GetMonikerString(devMoniker[0]).ToCharArray());
                devices.Add(deviceName, deviceString);

                // Release COM object
                Marshal.ReleaseComObject(devMoniker[0]);
                devMoniker[0] = null;
            }

            _deviceDescriptions = devices;

            Marshal.ReleaseComObject(devEnum);
            Marshal.ReleaseComObject(enumMon);

            return devices;
        }

        #endregion Public methods

        #region Private methods

        private static T CreateInstanceFromClsid<T>(Guid clsid)
        {
            Type srvType = Type.GetTypeFromCLSID(clsid);
            if (srvType == null)
                throw new ApplicationException("Failed creating device enumerator");

            object comObj = Activator.CreateInstance(srvType);
            return (T)comObj;
        }

        //
        // Get moniker string of the moniker
        //
        private static string GetMonikerString(IMoniker moniker)
        {
            moniker.GetDisplayName(null, null, out string str);
            return str;
        }

        //
        // Get moniker name represented
        //
        private static string GetMonikerName(IMoniker moniker)
        {
            Object bagObj = null;

            try
            {
                Guid bagId = typeof(IPropertyBag).GUID;
                // get property bag of the moniker
                moniker.BindToStorage(null, null, ref bagId, out bagObj);
                IPropertyBag bag = (IPropertyBag)bagObj;

                // read FriendlyName
                object val = "";
                int hr = bag.Read("FriendlyName", ref val, IntPtr.Zero);
                if (hr != 0)
                    Marshal.ThrowExceptionForHR(hr);

                // get it as string
                string ret = (string)val;
                if ((ret == null) || (ret.Length < 1))
                    throw new ApplicationException();

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

        #endregion Private methods

        #region NewFrameEvent

        public delegate void NewFrameEventHandler(object sender, Bitmap frame);

        public event NewFrameEventHandler NewFrame;

        #endregion NewFrameEvent

        #region Grabber

        private class Grabber : ISampleGrabberCB
        {
            private readonly Webcam _parent;

            private readonly Bitmap[] _frames;
            private readonly int _numFrames = 10;
            private int _currentFrameIndex = -1;

            public Grabber(Webcam parent)
            {
                _parent = parent;
                _frames = new Bitmap[_numFrames];
            }

            private Bitmap GetNextFrame()
            {
                _currentFrameIndex = (_currentFrameIndex + 1) % _numFrames;
                Bitmap currentBitmap = _frames[_currentFrameIndex];
                if (currentBitmap == null || currentBitmap.Width != _parent._width ||
                    currentBitmap.Height != _parent._height)
                {
                    currentBitmap = new Bitmap(_parent._width, _parent._height, PixelFormat.Format24bppRgb);
                    _frames[_currentFrameIndex] = currentBitmap;
                }

                return currentBitmap;
            }

            public int SampleCB(double sampleTime, IntPtr sample)
            {
                return 0;
            }

            public int BufferCB(double sampleTime, IntPtr buffer, int bufferLen)
            {
                if (_parent.NewFrame != null)
                {
                    // create new image
                    Bitmap image = GetNextFrame();
                    Rectangle imageRect = new Rectangle(0, 0, _parent._width, _parent._height);

                    // lock bitmap data
                    BitmapData imageData = image.LockBits(
                        imageRect,
                        ImageLockMode.ReadWrite,
                        PixelFormat.Format24bppRgb);

                    // copy image data
                    int srcStride = imageData.Stride;
                    int dstStride = imageData.Stride;

                    unsafe
                    {
                        byte* dst = (byte*)imageData.Scan0.ToPointer() + dstStride * (_parent._height - 1);
                        byte* src = (byte*)buffer.ToPointer();

                        for (int y = 0; y < _parent._height; y++)
                        {
                            Win32.memcpy(dst, src, srcStride);
                            dst -= dstStride;
                            src += srcStride;
                        }
                    }

                    // unlock bitmap data
                    image.UnlockBits(imageData);

                    // notify parent
                    _parent.NewFrame?.Invoke(this, image);
                }

                return 0;
            }
        }

        #endregion Grabber
    }
}