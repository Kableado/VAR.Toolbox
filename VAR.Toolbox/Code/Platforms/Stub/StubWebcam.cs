using System.Drawing;

namespace VAR.Toolbox.Code.Platforms.Stub;

public class StubWebcam : IWebcam
{
    public void Start()
    {
        NewFrame?.Invoke(null, new Bitmap(100,100));
    }

    public void Stop() { }

    public bool Active => false;

    public event IWebcam.NewFrameEventHandler? NewFrame;
}