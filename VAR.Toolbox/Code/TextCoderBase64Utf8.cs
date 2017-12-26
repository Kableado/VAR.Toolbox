using System;
using System.Text;

namespace VAR.Toolbox.Code
{
    public class TextCoderBase64Utf8 : ITextCoder
    {
        public string Decode(string input, string key)
        {
            byte[] encodedDataAsBytes
                = Convert.FromBase64String(input);
            string returnValue =
               Encoding.UTF8.GetString(encodedDataAsBytes);
            return returnValue;
        }

        public string Encode(string input, string key)
        {
            byte[] toEncodeAsBytes
              = Encoding.UTF8.GetBytes(input);
            string returnValue
                  = Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
    }
}
