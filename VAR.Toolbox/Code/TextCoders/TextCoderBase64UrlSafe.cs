
using System;
using System.Text;

namespace VAR.Toolbox.Code.TextCoders;

public class TextCoderBase64UrlSafe : ITextCoder
{
    public string Name => "Base64UrlSafe";
    
    public bool NeedsKey => false;
    
    public string Encode(string input, string key)
    {
        byte[] toEncodeAsBytes = Encoding.UTF8.GetBytes(input);
        string encoded = Convert.ToBase64String(toEncodeAsBytes);
        string encodedFix = encoded.Replace("+", "-").Replace("/", "_").Replace("=", "");
        return encodedFix;
    }
    
    public string Decode(string input, string key)
    {
        string encodedFix = input.Replace("-", "+").Replace("_", "/").Replace(".", "=").Replace("~", "=");
        encodedFix = Base64_FixPadding(encodedFix);
        byte[] decoded = Convert.FromBase64String(encodedFix);
        string returnValue = Encoding.UTF8.GetString(decoded);
        return returnValue;
    }

    private static string Base64_FixPadding(string str)
    {
        int mod = str.Length % 4;
        if (mod == 2)
        {
            return $"{str}==";
        }

        if (mod == 3)
        {
            return $"{str}=";
        }

        return str;
    }
}