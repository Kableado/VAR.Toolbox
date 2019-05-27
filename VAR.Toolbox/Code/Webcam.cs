#pragma warning disable IDE0018
#pragma warning disable IDE0059

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

        private readonly IFilterGraph2 graph;
        private readonly ICaptureGraphBuilder2 capture;
        private readonly IMediaControl control;
        private readonly IBaseFilter sourceFilter;
        private readonly IBaseFilter samplegrabberfilter;
        private readonly IBaseFilter nullrenderer;

        private readonly Grabber grabber;

        private readonly int width = 0;
        private readonly int height = 0;
        private readonly int bpp = 0;

        private bool active = false;

        private static Dictionary<string, string> _deviceDescriptions;

        #endregion Declarations

        #region Properties

        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public int BPP { get { return bpp; } }

        public bool Active { get { return active; } }

        #endregion Properties

        #region Lifecycle

        public Webcam(string monikerString)
        {
            int result;

            graph = CreateInstanceFromClsid<IFilterGraph2>(Clsid.FilterGraph);
            capture = CreateInstanceFromClsid<ICaptureGraphBuilder2>(Clsid.CaptureGraphBuilder2);
            control = (IMediaControl)graph;
            capture.SetFiltergraph((IGraphBuilder)graph);

            IBindCtx bindCtx = null;
            IMoniker moniker = null;

            int n = 0;

            if (Win32.CreateBindCtx(0, out bindCtx) != 0)
            {
                throw new Exception("Failed to create binding context");
            }
            if (Win32.MkParseDisplayName(bindCtx, monikerString, ref n, out moniker) != 0)
            {
                throw new Exception("Failed to create binding moniker");
            }

            graph.AddSourceFilterForMoniker(moniker, bindCtx, monikerString, out sourceFilter);

            samplegrabberfilter = CreateInstanceFromClsid<IBaseFilter>(Clsid.SampleGrabber);
            graph.AddFilter(samplegrabberfilter, string.Format("SampleGrabber {0}", monikerString));

            ISampleGrabber sampleGrabber = (ISampleGrabber)samplegrabberfilter;

            // Set media type
            AMMediaType mediaType = new AMMediaType
            {
                MajorType = MediaType.Video,
                SubType = MediaSubType.RGB24
            };
            sampleGrabber.SetMediaType(mediaType);

            grabber = new Grabber(this);
            result = sampleGrabber.SetCallback(grabber, 1);
            if (result < 0) throw new Exception("Failure creating Webcam device");

            //set the null renderer
            nullrenderer = CreateInstanceFromClsid<IBaseFilter>(Clsid.NullRenderer);
            graph.AddFilter(nullrenderer, string.Format("NullRenderer {0}", monikerString));

            result = capture.RenderStream(PinCategory.Preview, MediaType.Video, sourceFilter, samplegrabberfilter, nullrenderer);
            if (result < 0) throw new Exception("Failure creating Webcam device");

            AMMediaType queryMediaType = new AMMediaType();
            result = sampleGrabber.GetConnectedMediaType(queryMediaType);
            if (result == 0)
            {
                if (queryMediaType.FormatType == FormatType.VideoInfo)
                {
                    VideoInfoHeader videoInfo = (VideoInfoHeader)Marshal.PtrToStructure(queryMediaType.FormatPtr, typeof(VideoInfoHeader));
                    width = videoInfo.BmiHeader.Width;
                    height = videoInfo.BmiHeader.Height;
                    bpp = videoInfo.BmiHeader.BitCount;
                }
            }

            control.Run();
            Stop();
        }

        #endregion Lifecycle

        #region Public methods

        public void Start()
        {
            control.Run();
            int result;
            result = nullrenderer.Run(0);
            if (result < 0) throw new Exception("Webcam Start failure");
            result = samplegrabberfilter.Run(0);
            if (result < 0) throw new Exception("Webcam Start failure");
            result = sourceFilter.Run(0);
            if (result < 0) throw new Exception("Webcam Start failure");
            active = true;
        }

        public void Stop()
        {
            int result;
            result = sourceFilter.Stop();
            if (result < 0) throw new Exception("Webcam Stop failure");
            result = samplegrabberfilter.Stop();
            if (result < 0) throw new Exception("Webcam Stop failure");
            result = nullrenderer.Stop();
            if (result < 0) throw new Exception("Webcam Stop failure");
            control.Stop();
            active = false;
        }

        public static Dictionary<string, string> ListDevices()
        {
            if (_deviceDescriptions != null) { return _deviceDescriptions; }

            int result;
            Dictionary<string, string> devices = new Dictionary<string, string>();
            ICreateDevEnum devEnum = CreateInstanceFromClsid<ICreateDevEnum>(Clsid.SystemDeviceEnum);

            IEnumMoniker enumMon = null;
            Guid category = FilterCategory.VideoInputDevice;
            result = devEnum.CreateClassEnumerator(ref category, out enumMon, 0);
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
            string str;
            moniker.GetDisplayName(null, null, out str);
            return str;
        }

        //
        // Get moniker name represented
        //
        private static string GetMonikerName(IMoniker moniker)
        {
            Object bagObj = null;
            IPropertyBag bag = null;

            try
            {
                Guid bagId = typeof(IPropertyBag).GUID;
                // get property bag of the moniker
                moniker.BindToStorage(null, null, ref bagId, out bagObj);
                bag = (IPropertyBag)bagObj;

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
                bag = null;
                if (bagObj != null)
                {
                    Marshal.ReleaseComObject(bagObj);
                    bagObj = null;
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

            private readonly Bitmap[] _frames = null;
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
                if (currentBitmap == null || currentBitmap?.Width != _parent.width || currentBitmap?.Height != _parent.height)
                {
                    currentBitmap = new Bitmap(_parent.width, _parent.height, PixelFormat.Format24bppRgb);
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
                    Bitmap _image = GetNextFrame();
                    Rectangle _imageRect = new Rectangle(0, 0, _parent.width, _parent.height);

                    // lock bitmap data
                    BitmapData imageData = _image.LockBits(
                        _imageRect,
                        ImageLockMode.ReadWrite,
                        PixelFormat.Format24bppRgb);

                    // copy image data
                    int srcStride = imageData.Stride;
                    int dstStride = imageData.Stride;

                    unsafe
                    {
                        byte* dst = (byte*)imageData.Scan0.ToPointer() + dstStride * (_parent.height - 1);
                        byte* src = (byte*)buffer.ToPointer();

                        for (int y = 0; y < _parent.height; y++)
                        {
                            Win32.memcpy(dst, src, srcStride);
                            dst -= dstStride;
                            src += srcStride;
                        }
                    }

                    // unlock bitmap data
                    _image.UnlockBits(imageData);

                    // notify parent
                    _parent.NewFrame?.Invoke(this, _image);
                }

                return 0;
            }
        }

        #endregion Grabber
    }
}
