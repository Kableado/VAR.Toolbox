namespace VAR.Toolbox.Code
{
    public interface IOutputHandler
    {
        void Clean();
        void AddLine(string line, object data = null);
    }
}