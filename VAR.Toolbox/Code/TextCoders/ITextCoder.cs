namespace VAR.Toolbox.Code.TextCoders
{
    public interface ITextCoder
    {
        string Name { get; }

        bool NeedsKey { get; }

        string Encode(string input, string key);
        string Decode(string input, string key);
    }
}
