using System;
using System.Text;

namespace VAR.Toolbox.Code.TextCoders
{
    public class TextCoderBase64ToUtf8 : ITextCoder
    {
        public string Name { get { return "Base64ToUtf8"; } }

        public bool NeedsKey { get { return false; } }

        public string Decode(string input, string key)
        {
            byte[] encodedDataAsBytes = Convert.FromBase64String(input);
            string returnValue = Encoding.UTF8.GetString(encodedDataAsBytes);
            return returnValue;
        }

        public string Encode(string input, string key)
        {
            byte[] toEncodeAsBytes = Encoding.UTF8.GetBytes(input);
            string returnValue = Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
    }
}
