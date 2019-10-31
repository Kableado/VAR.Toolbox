namespace VAR.ScreenAutomation.Interfaces
{
    public interface IOutputHandler
    {
        void Clean();
        void AddLine(string line, object data = null);
    }
}
