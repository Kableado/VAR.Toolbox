using System.Drawing;

namespace VAR.ScreenAutomation.Interfaces
{
    public interface IAutomationBot
    {
        string Name { get; }
        void Init(IOutputHandler output);
        Bitmap Process(Bitmap bmpInput, IOutputHandler output);
        string ResponseKeys();
    }
}
