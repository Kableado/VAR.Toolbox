using SkiaSharp;
using VAR.Toolbox.Code.Configuration;

namespace VAR.Toolbox.Code.Bots;

public interface IAutomationBot: INamed
{
    IConfiguration? GetDefaultConfiguration();
    void Init(IOutputHandler output, IConfiguration? config);
    SKBitmap Process(SKBitmap bmpInput, IOutputHandler output);
    string ResponseKeys();
}