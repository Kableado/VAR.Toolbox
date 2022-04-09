using System.Drawing;
using VAR.Toolbox.Code.Configuration;

namespace VAR.Toolbox.Code.Bots
{
    public interface IAutomationBot: INamed
    {
        IConfiguration GetDefaultConfiguration();
        void Init(VAR.Toolbox.Code.IOutputHandler output, IConfiguration config);
        Bitmap Process(Bitmap bmpInput, VAR.Toolbox.Code.IOutputHandler output);
        string ResponseKeys();
    }
}