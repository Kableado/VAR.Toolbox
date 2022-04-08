using System.Drawing;

namespace VAR.ScreenAutomation.Interfaces
{
    public interface IAutomationBot
    {
        string Name { get; }
        IConfiguration GetDefaultConfiguration();
        void Init(IOutputHandler output, IConfiguration config);
        Bitmap Process(Bitmap bmpInput, IOutputHandler output);
        string ResponseKeys();
    }
}