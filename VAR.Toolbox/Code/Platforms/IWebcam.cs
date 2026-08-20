
namespace VAR.Toolbox.Code.Platforms;

using SkiaSharp;

public interface IWebcam
{
    void Start();
    void Stop();

    bool Active { get; }
    
    public delegate void NewFrameEventHandler(object? sender, SKBitmap frame);

    event NewFrameEventHandler? NewFrame;
}