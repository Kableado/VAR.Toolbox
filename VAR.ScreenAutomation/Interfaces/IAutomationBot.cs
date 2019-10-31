using System.Drawing;

namespace VAR.ScreenAutomation.Interfaces
{
    public interface IAutomationBot
    {
        void Init(IOutputHandler output);
        Bitmap Process(Bitmap bmpInput, IOutputHandler output);
        string ResponseKeys();
    }
}
