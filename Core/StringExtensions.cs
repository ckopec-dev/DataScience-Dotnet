using System.Globalization;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Text;

namespace Core
{
    public static class StringExtensions
    {
        public static string Left(this string s, int length)
        {
            return s[..length];
        }

        public static string Right(this string s, int length)
        {
            // E.g. helloworld, 2 returns "ld"

            return s[^length..];
        }

        public static string ToProperCase(this string s)
        {
            StringBuilder sb = new();
            bool fEmptyBefore = true;

            foreach (char ch in s)
            {
                char chThis = ch;

                if (Char.IsWhiteSpace(chThis))
                    fEmptyBefore = true;
                else
                {
                    if (Char.IsLetter(chThis) && fEmptyBefore)
                        chThis = Char.ToUpper(chThis);
                    else
                        chThis = Char.ToLower(chThis);
                    fEmptyBefore = false;
                }

                sb.Append(chThis);
            }

            return sb.ToString();
        }

        public static Byte[] ToUTFByteArray(this string s)
        {
            UTF8Encoding encoding = new();
            Byte[] byteArray = encoding.GetBytes(s);

            return byteArray;
        }

        public static string ToString(this Byte[] byteArray)
        {
            UTF8Encoding uTF8Encoding = new();
            UTF8Encoding encoding = uTF8Encoding;
            String constructedString = encoding.GetString(byteArray);

            return (constructedString);
        }

        public static byte[] ToBytes(this string s)
        {
            byte[] bytes = new byte[s.Length * sizeof(char)];
            Buffer.BlockCopy(s.ToCharArray(), 0, bytes, 0, bytes.Length);

            return bytes;
        }

        public static string Reverse(this string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);

            return new string(charArray);
        }

        public static int[] ToIntArray(this string s, char separator)
        {
            string[] stringArray = s.Split(separator);
            int[] intArray = new int[stringArray.Length];

            for (int i = 0; i < intArray.Length; i++)
                intArray[i] = Convert.ToInt32(stringArray[i]);

            return intArray;
        }

        public static List<string> ToList(this string[] data)
        {
            // Converts a string (of multiple rows) to a list. Ignores blank rows.

            List<string> list = [];

            foreach (string s in data)
            {
                if (!String.IsNullOrWhiteSpace(s))
                    list.Add(s);
            }

            return list;
        }

        public static List<int> AllIndexesOf(this string str, string substring)
        {
            List<int> indexes = [];

            for (int index = 0; index < str.Length; index++)
            {
                index = str.IndexOf(substring, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }

            return indexes;
        }

        public static string ToCsv<T>(this IEnumerable<T> items)
            where T : class
        {
            var csvBuilder = new StringBuilder();
            var properties = typeof(T).GetProperties();

            foreach (T item in items)
            {
                string line = string.Join(",", properties.Select(p => p.GetValue(item, null).ToCsvValue()).ToArray());
                csvBuilder.AppendLine(line);
            }

            return csvBuilder.ToString();
        }

        private static string ToCsvValue<T>(this T item)
        {
            if (item == null) return "\"\"";

            if (item is string)
            {
                string? s = item.ToString();
                if (s != null)
                    return string.Format("\"{0}\"", s.Replace("\"", "\\\""));
                else
                    return "\"\"";
            }

            if (double.TryParse(item.ToString(), out _))
            {
                return string.Format("{0}", item);
            }

            return string.Format("\"{0}\"", item);
        }


        public static bool ContainsAny(this string sourceString, string searchString)
        {
            if (String.IsNullOrWhiteSpace(sourceString) || String.IsNullOrWhiteSpace(searchString))
                return false;

            string lowerSourceString = sourceString.ToLower();
            string lowerSearchString = searchString.ToLower();

            if (lowerSourceString.Contains(lowerSearchString))
                return true;
            else
                return false;
        }

        public static int[] ToNodePair(this string nodes)
        {
            string[] list = nodes.Split(' ');

            int a1 = Convert.ToInt32(list[0]);
            int a2 = Convert.ToInt32(list[1]);

            return [a1, a2];
        }

        public static string ToFriendlyDuration(this TimeSpan timeSpan, int numberDecimalPlaces)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.NumberDecimalDigits = numberDecimalPlaces;

            if (timeSpan.TotalSeconds < 60d)
                return timeSpan.TotalSeconds.ToString("N", nfi) + " seconds";
            else if (timeSpan.TotalMinutes < 60d)
                return timeSpan.TotalMinutes.ToString("N", nfi) + " minutes";
            else if (timeSpan.TotalHours < 24d)
                return timeSpan.TotalHours.ToString("N", nfi) + " hours";
            else
                return timeSpan.TotalDays.ToString("N", nfi) + " days";
        }

        public static string ToString(this List<string> list, bool includeNewLine)
        {
            StringBuilder sb = new();

            for (int i = 0; i < list.Count; i++)
            {
                if (includeNewLine)
                    sb.AppendLine(list[i]);
                else
                    sb.Append(list[i]);
            }

            return sb.ToString();
        }

        public static string? ToString(this List<string> data, char delimiter)
        {
            string? s = null;

            for (int i = 0; i < data.Count; i++)
            {
                if (i != 0)
                    s += delimiter;

                s += data[i];
            }

            return s;
        }

        public static List<string> ToList(this string data)
        {
            // Converts a string (of multiple rows) to a list. Ignores blank rows.

            string[] rows = (Regex.Split(data, Environment.NewLine));

            List<string> list = [];

            foreach (string row in rows)
            {
                if (!String.IsNullOrWhiteSpace(row))
                    list.Add(row);
            }

            return list;
        }

        public static List<string> ToList(this string data, char delimiter)
        {
            // Converts a string (of multiple rows) to a list. Ignores blank rows.
            
            string[] rows = data.Split(delimiter);

            List<string> list = [];

            foreach (string row in rows)
            {
                if (!String.IsNullOrWhiteSpace(row))
                    list.Add(row);
            }

            return list;
        }

        public static Byte[] ToUTF8ByteArray(this string str)
        {
            UTF8Encoding encoding = new();
            Byte[] byteArray = encoding.GetBytes(str);

            return byteArray;
        }

        public static string ToStringFromUTF8Bytes(this Byte[] bytes)
        {
            UTF8Encoding encoding = new();
            String constructedString = encoding.GetString(bytes);

            return (constructedString);
        }

        public static string RemoveUtf8ByteOrderMark(this string xmlString)
        {
            string byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());

            if (xmlString.StartsWith(byteOrderMarkUtf8))
            {
                xmlString = xmlString.Remove(0, byteOrderMarkUtf8.Length);
            }

            return xmlString;
        }

        public static string? Merge(this List<string> stringList, char delimiter)
        {
            // Inverse of split for a list.
            if (stringList == null)
                return null;

            string? result = null;

            for (int i = 0; i < stringList.Count; i++)
            {
                if (i > 0)
                    result += delimiter.ToString();
                result += stringList[i];
            }

            return result;
        }

        public static string Repeat(this char charToRepeat, int repeat)
        {
            return new string(charToRepeat, repeat);
        }

        public static string Repeat(this string stringToRepeat, int repeat)
        {
            var builder = new StringBuilder(repeat * stringToRepeat.Length);

            for (int i = 0; i < repeat; i++)
            {
                builder.Append(stringToRepeat);
            }

            return builder.ToString();
        }

        public static string LargestCommonSubstring(this List<string> strings)
        {
            string lcs = "";
            string baseString = strings[0];

            // Iterate through every possible length of substrings in the base string.
            for (int i = 1; i <= baseString.Length; i++)
            {
                // Iterate through every possible starting position of the substring of given length.
                for (int j = 0; j < baseString.Length - i; j++)
                {
                    string ss = baseString.Substring(j, i);

                    bool existsInAll = true;

                    for (int k = 1; k < strings.Count; k++)
                    {
                        if (!strings[k].Contains(ss))
                        {
                            existsInAll = false;
                            break;
                        }
                    }

                    if (existsInAll)
                        lcs = ss;
                }
            }

            return lcs;
        }

        public static string Compressed(this string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();

            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];

            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];

            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);

            return Convert.ToBase64String(gZipBuffer);
        }

        public static string Decompress(this string compressedText)
        {
            byte[] gZipBuffer = Convert.FromBase64String(compressedText);
            using var memoryStream = new MemoryStream();

            int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
            memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

            var buffer = new byte[dataLength];

            memoryStream.Position = 0;
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
            {
                gZipStream.Read(buffer, 0, buffer.Length);
            }

            return Encoding.UTF8.GetString(buffer);
        }

        public static List<int> ToAlphabetIndexes(this string word)
        {
            const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            List<int> indexes = [];
            for (int i = 0; i < word.Length; i++)
            {
                indexes.Add(ALPHABET.IndexOf(word[i]) + 1);
            }

            return indexes;
        }

        public static bool IsPalindrome(this string text)
        {
            if (text == text.Reverse())
                return true;
            else
                return false;
        }

        public static string ToBase64(this string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            return Convert.ToBase64String(plainTextBytes);
        }

        public static string FromBase64(this string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);

            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
