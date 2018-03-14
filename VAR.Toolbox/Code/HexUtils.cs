using System;
using System.Text;

namespace VAR.Toolbox.Code
{
    public class HexUtils
    {
        public static byte[] HexStringToBytes(string input)
        {
            int[] hexValues = new int[] {
                0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
            if (input.Length % 2 == 1)
            {
                throw new Exception("Unenven number of hex digits");
            }
            byte[] bytes = new byte[input.Length / 2];
            int count = input.Length;
            for (int x = 0, i = 0; i < count; i += 2, x += 1)
            {
                bytes[x] = (byte)(
                    hexValues[Char.ToUpper(input[i + 0]) - '0'] << 4 |
                    hexValues[Char.ToUpper(input[i + 1]) - '0']
                    );
            }

            return bytes;
        }

        public static string BytesToHexString(byte[] toEncodeAsBytes)
        {
            string HexAlphabet = "0123456789ABCDEF";
            StringBuilder sbOutput = new StringBuilder();
            int count = toEncodeAsBytes.Length;
            for (int i = 0; i < count; i++)
            {
                byte b = toEncodeAsBytes[i];
                sbOutput.Append(HexAlphabet[(b >> 4)]);
                sbOutput.Append(HexAlphabet[(b & 0xF)]);
            }
            return sbOutput.ToString();
        }
    }
}
