using System.Drawing;

namespace VAR.ScreenAutomation.Interfaces
{
    public interface IAutomationBot
    {
        string Name { get; }
        IConfiguration GetDefaultConfiguration();
        void Init(VAR.Toolbox.Code.IOutputHandler output, IConfiguration config);
        Bitmap Process(Bitmap bmpInput, VAR.Toolbox.Code.IOutputHandler output);
        string ResponseKeys();
    }
}