using System;

namespace VAR.Toolbox.Code
{
    public class TextCoderFactory
    {
        public static string[] GetSupportedCoders()
        {
            return new string[] {
                TextCoderBase64ToAscii.Name,
                TextCoderBase64ToUtf8.Name,
                TextCoderHexToUtf8.Name,
                TextCoderHexToAscii.Name,
            };
        }

        public static ITextCoder CreateFromName(string name)
        {
            if (name == TextCoderBase64ToAscii.Name)
            {
                return new TextCoderBase64ToAscii();
            }
            if (name == TextCoderBase64ToUtf8.Name)
            {
                return new TextCoderBase64ToUtf8();
            }
            if (name == TextCoderHexToUtf8.Name)
            {
                return new TextCoderHexToUtf8();
            }
            if (name == TextCoderHexToAscii.Name)
            {
                return new TextCoderHexToAscii();
            }
            throw new NotImplementedException(string.Format("Cant create ITextCoder with this name: {0}", name));
        }
    }
}
