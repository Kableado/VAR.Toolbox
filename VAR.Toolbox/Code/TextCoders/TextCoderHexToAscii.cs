using System.Text;

namespace VAR.Toolbox.Code.TextCoders
{
    public class TextCoderHexToAscii : ITextCoder
    {
        public string Name { get { return "HexToAscii"; } }

        public bool NeedsKey { get { return false; } }

        public string Decode(string input, string key)
        {
            byte[] bytes = HexUtils.HexStringToBytes(input);
            string returnValue = Encoding.ASCII.GetString(bytes);
            return returnValue;
        }

        public string Encode(string input, string key)
        {
            byte[] toEncodeAsBytes = Encoding.ASCII.GetBytes(input);
            return HexUtils.BytesToHexString(toEncodeAsBytes);
        }

    }
}
