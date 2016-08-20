using System;
using System.Text;

namespace Sharik
{
    // Modified Base-64 dictionary is 0-9 A-Z a-z - _. There is no = padding at the end of string.
    // Base-64 dictionary is 0-9 A-Z a-z + /. The string is padded with = symbol. 
    // Modified Base-64 string can be used in URI (in contrast to Base-64 string).
    // Note, URI unreserved characters are: A-Z a-z - _ . ~
    public static class ConvertString
    {
        public static byte[] FromModifiedBase64String(string s)
        {
            var base64 = ConvertModifiedBase64StringToBase64String(s);
            var result = Convert.FromBase64String(base64);
            return result;
        }

        public static string ToModifiedBase64String(byte[] inArray)
        {
            var base64 = Convert.ToBase64String(inArray);
            var result = ConvertBase64StringToModifiedBase64String(base64);
            return result;
        }

        public static string ToModifiedBase64String(byte[] inArray, Base64FormattingOptions options)
        {
            var base64 = Convert.ToBase64String(inArray, options);
            var result = ConvertBase64StringToModifiedBase64String(base64);
            return result;
        }

        public static string ToModifiedBase64String(byte[] inArray, int offset, int length)
        {
            var base64 = Convert.ToBase64String(inArray, offset, length);
            var result = ConvertBase64StringToModifiedBase64String(base64);
            return result;
        }

        public static string ToModifiedBase64String(byte[] inArray, int offset, int length, Base64FormattingOptions options)
        {
            var base64 = Convert.ToBase64String(inArray, offset, length, options);
            var result = ConvertBase64StringToModifiedBase64String(base64);
            return result;
        }

        public static string ToBase32String(long value)
        {
            var buffer = new char[cMaxNumberOfDigitsForInt64];
            var pos = cMaxNumberOfDigitsForInt64 - 1;
            do
            {
                buffer[pos] = gDigits[value & (cBaseNumber - 1)];
                value = value >> cBitsPerDigit;
                pos -= 1;
            }
            while (value != 0 && pos >= 0);
            return new string(buffer, pos + 1, cMaxNumberOfDigitsForInt64 - pos - 1);
        }

        public static long FromBase32String(string s)
        {
            return FromBase32String(s, 0, s.Length);
        }

        public static long FromBase32String(string s, int start, int length)
        {
            long result = 0;
            var i = 0;
            do
            {
                var digit = gDigitValues[s[i]];
                if (digit >= cBaseNumber)
                    throw new FormatException(string.Format("unsupported symbol '{0}'", s[i]));
                result = (result << cBitsPerDigit) | digit;
                ++i;
            }
            while (i < s.Length);
            return result;
        }

        // Internal

        private static string ConvertBase64StringToModifiedBase64String(string base64String)
        {
            StringBuilder result = new StringBuilder(base64String);
            result.Replace('+', '-');
            result.Replace('/', '_');
            // Remove '=' padding to make result shorter
            int paddingCount = 0;
            for (int i = result.Length - 1; i >= 0 && result[i] == '='; i--)
                paddingCount++;
            return result.ToString(0, result.Length - paddingCount);
        }

        private static string ConvertModifiedBase64StringToBase64String(string modifiedBase64String)
        {
            StringBuilder result = new StringBuilder(modifiedBase64String);
            result.Replace('-', '+');
            result.Replace('_', '/');
            while (result.Length % 4 != 0)
                result.Append('=');
            return result.ToString();
        }

        private static byte[] InvertedArray(char[] digits)
        {
            var result = new byte[char.MaxValue];
            for (var i = 0; i < result.Length; ++i)
                result[i] = 255;
            for (byte i = 0; i < digits.Length; ++i)
                result[digits[i]] = i;
            return result;
        }

        private const byte cBitsPerDigit = 5;
        private const byte cBaseNumber = 1 << cBitsPerDigit;
        private const byte cMaxNumberOfDigitsForInt64 = 64 / cBitsPerDigit + 1;

        private static char[] gDigits = new char[]
        {
            // Base-32 Digits
            '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A',
            'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'M',
            'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            // Base-64 Digits
            '0', 'I', 'O', 'L', // visually confusing characters
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
            'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
            'u', 'v', 'w', 'x', 'y', 'z',
            '$', '!'
        };

        private static byte[] gDigitValues = InvertedArray(gDigits);
    }
}
