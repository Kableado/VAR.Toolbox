using System;

namespace VAR.Toolbox.Code
{
    public class TextCoderFactory
    {
        public static string[] GetSupportedCoders()
        {
            return new string[] {
                "Base64Ascii",
                "Base64Utf8",
            };
        }

        public static ITextCoder CreateFromName(string name)
        {
            if (name == "Base64Ascii")
            {
                return new TextCoderBase64Ascii();
            }
            if (name == "Base64Utf8")
            {
                return new TextCoderBase64Utf8();
            }
            throw new NotImplementedException(string.Format("Cant create ITextCoder with this name: {0}", name));
        }
    }
}
