using System;
using System.Text;

namespace VAR.Toolbox.Code
{
    public class CoderBase64 : ICoder
    {
        public string Decode(string input, string key)
        {
            byte[] encodedDataAsBytes
                = Convert.FromBase64String(input);
            string returnValue =
               Encoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }

        public string Encode(string input, string key)
        {
            byte[] toEncodeAsBytes
              = Encoding.ASCII.GetBytes(input);
            string returnValue
                  = Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
    }
}
