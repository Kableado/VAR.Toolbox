using System.Collections.Generic;
using SkiaSharp;

namespace VAR.Toolbox.Code.Platforms.Linux;

public class LinuxWebcam : IWebcam
{
    private string _moniker;

    public LinuxWebcam(string moniker)
    {
        _moniker = moniker;
    }

    public void Start()
    {
        NewFrame?.Invoke(this, new SKBitmap(100, 100));
    }

    public void Stop()
    {
    }

    public bool Active => false;

    public event IWebcam.NewFrameEventHandler? NewFrame;

    public static Dictionary<string, string> ListDevices()
    {
        Dictionary<string, string> devices = new();
        string[] videoDevices = System.IO.Directory.GetFiles("/dev", "video*");
        foreach (string videoDevice in videoDevices)
        {
            devices.Add(videoDevice, videoDevice);
        }
        return devices;
    }
}
