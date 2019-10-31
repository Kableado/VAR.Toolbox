using System.Drawing;
using VAR.ScreenAutomation.Interfaces;

namespace VAR.ScreenAutomation.Bots
{
    public class DummyBot : IAutomationBot
    {
        private int frameCount = 0;

        public void Init(IOutputHandler output)
        {
            frameCount = 0;
            output.Clean();
        }

        public Bitmap Process(Bitmap bmpInput, IOutputHandler output)
        {
            frameCount++;
            output.AddLine(string.Format("Frame: {0}", frameCount));
            return bmpInput;
        }

        public string ResponseKeys()
        {
            return "{UP}";
        }
    }
}
