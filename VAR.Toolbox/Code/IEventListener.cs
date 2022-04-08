namespace VAR.Toolbox.Code
{
    public interface IEventListener
    {
        void ProcessEvent(string eventName, object eventData);
    }
}