using System.Text;

namespace VAR.Toolbox.Code
{
    public class TextCoderHexToUtf8 : ITextCoder
    {
        public const string Name = "HexToUtf8";

        public bool NeedsKey { get { return false; } }

        public string Decode(string input, string key)
        {
            byte[] bytes = HexUtils.HexStringToBytes(input);
            string returnValue = Encoding.UTF8.GetString(bytes);
            return returnValue;
        }

        public string Encode(string input, string key)
        {
            byte[] toEncodeAsBytes = Encoding.UTF8.GetBytes(input);
            return HexUtils.BytesToHexString(toEncodeAsBytes);
        }
    }
}
