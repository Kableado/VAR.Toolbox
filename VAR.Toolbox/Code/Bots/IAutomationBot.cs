using System.Drawing;
using VAR.Toolbox.Code.Configuration;

namespace VAR.Toolbox.Code.Bots
{
    public interface IAutomationBot: INamed
    {
        IConfiguration GetDefaultConfiguration();
        void Init(IOutputHandler output, IConfiguration config);
        Bitmap Process(Bitmap bmpInput, IOutputHandler output);
        string ResponseKeys();
    }
}