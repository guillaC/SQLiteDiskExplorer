using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SQLiteDiskExplorer.Utils
{
    public static class StringHex
    {
        public static List<string> ExtractStrings(byte[] data)
        {
            List<string> stringsList = new();

            int startIndex = 0;

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == 0x00)
                {
                    byte[] substringBytes = new byte[i - startIndex];
                    Array.Copy(data, startIndex, substringBytes, 0, i - startIndex);
                    string substring = Encoding.Default.GetString(substringBytes);
                    if (!string.IsNullOrEmpty(substring) && substring.Length > 3 && IsStringReadable(substring))
                        stringsList.Add(substring);
                    startIndex = i + 1;
                }
            }

            return stringsList;
        }

        public static bool IsStringReadable(string str)
        {
            int invalidCharCount = str.Count(c => !char.IsLetterOrDigit(c) && !char.IsPunctuation(c) && !char.IsWhiteSpace(c));

            return invalidCharCount <= str.Length * 0.05; // 5% de caractères invalides max
        }
    }
}
