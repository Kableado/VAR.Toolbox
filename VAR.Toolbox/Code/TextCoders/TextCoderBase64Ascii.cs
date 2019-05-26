using System;
using System.Text;

namespace VAR.Toolbox.Code.TextCoders
{
    public class TextCoderBase64ToAscii : ITextCoder
    {
        public string Name { get { return "Base64ToAscii"; } }

        public bool NeedsKey { get { return false; } }

        public string Decode(string input, string key)
        {
            byte[] encodedDataAsBytes = Convert.FromBase64String(input);
            string returnValue = Encoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }

        public string Encode(string input, string key)
        {
            byte[] toEncodeAsBytes = Encoding.ASCII.GetBytes(input);
            string returnValue = Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
    }
}
