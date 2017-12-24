namespace VAR.Toolbox.Code
{
    public interface ICoder
    {
        string Encode(string input, string key);
        string Decode(string input, string key);
    }
}
