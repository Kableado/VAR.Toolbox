namespace VAR.Toolbox.Code
{
    public interface ITextCoder
    {
        bool NeedsKey { get; }

        string Encode(string input, string key);
        string Decode(string input, string key);
    }
}
