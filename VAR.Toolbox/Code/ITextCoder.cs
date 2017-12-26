namespace VAR.Toolbox.Code
{
    public interface ITextCoder
    {
        string Encode(string input, string key);
        string Decode(string input, string key);
    }
}
