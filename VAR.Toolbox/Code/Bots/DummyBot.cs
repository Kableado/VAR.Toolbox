using System.Drawing;
using VAR.Toolbox.Code.Configuration;

namespace VAR.Toolbox.Code.Bots
{
    public class DummyBot : IAutomationBot
    {
        public string Name => "Dummy";

        public IConfiguration GetDefaultConfiguration()
        {
            return null;
        }

        public void Init(VAR.Toolbox.Code.IOutputHandler output, IConfiguration config)
        {
            output.Clean();
        }

        public Bitmap Process(Bitmap bmpInput, VAR.Toolbox.Code.IOutputHandler output)
        {
            return bmpInput;
        }

        public string ResponseKeys()
        {
            // https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.sendkeys?view=netframework-4.8
            //return "{UP}";
            return string.Empty;
        }
    }
}