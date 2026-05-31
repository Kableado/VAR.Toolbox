using System.Drawing;

namespace VAR.Toolbox.Code.Platforms;

public interface IWebcam
{
    void Start();
    void Stop();

    bool Active { get; }
    
    public delegate void NewFrameEventHandler(object? sender, Bitmap frame);

    event NewFrameEventHandler? NewFrame;
}