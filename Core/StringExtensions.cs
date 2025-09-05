using System.Globalization;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace Core
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns the substring found between the first occurrence of the start and end strings.
        /// </summary>
        /// <param name="source">The source string to search in</param>
        /// <param name="start">The starting delimiter string</param>
        /// <param name="end">The ending delimiter string</param>
        /// <param name="includeDelimiters">Whether to include the start and end delimiters in the result</param>
        /// <returns>The substring between the delimiters, or empty string if not found</returns>
        public static string Between(this string source, string start, string end, bool includeDelimiters = false)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(start) || string.IsNullOrEmpty(end))
                return string.Empty;

            int startIndex = source.IndexOf(start);
            if (startIndex == -1)
                return string.Empty;

            int startPos = includeDelimiters ? startIndex : startIndex + start.Length;
            int endIndex = source.IndexOf(end, startPos);
            if (endIndex == -1)
                return string.Empty;

            int endPos = includeDelimiters ? endIndex + end.Length : endIndex;
            int length = endPos - startPos;

            return length > 0 ? source.Substring(startPos, length) : string.Empty;
        }

        /// <summary>
        /// Returns the substring found between the first occurrence of the start and end strings (case-insensitive).
        /// </summary>
        /// <param name="source">The source string to search in</param>
        /// <param name="start">The starting delimiter string</param>
        /// <param name="end">The ending delimiter string</param>
        /// <param name="includeDelimiters">Whether to include the start and end delimiters in the result</param>
        /// <returns>The substring between the delimiters, or empty string if not found</returns>
        public static string BetweenIgnoreCase(this string source, string start, string end, bool includeDelimiters = false)
        {
            int startIndex = source.IndexOf(start, StringComparison.OrdinalIgnoreCase);
            
            int startPos = includeDelimiters ? startIndex : startIndex + start.Length;
            int endIndex = source.IndexOf(end, startPos, StringComparison.OrdinalIgnoreCase);
            if (endIndex == -1)
                return string.Empty;

            int endPos = includeDelimiters ? endIndex + end.Length : endIndex;
            int length = endPos - startPos;

            return length > 0 ? source.Substring(startPos, length) : string.Empty;
        }

        /// <summary>
        /// Finds the largest common substring among all strings in the collection.
        /// Returns null if no common substring is found.
        /// </summary>
        public static string? LargestCommonSubstring(this IEnumerable<string> strings)
        {
            if (strings.IsNullOrEmpty() || !strings.Any())
                return null;

            var list = strings.ToList();
            string shortest = list.OrderBy(s => s.Length).First();

            for (int length = shortest.Length; length > 0; length--)
            {
                for (int start = 0; start <= shortest.Length - length; start++)
                {
                    string candidate = shortest.Substring(start, length);

                    if (list.All(s => s.Contains(candidate)))
                    {
                        return candidate; // Largest found
                    }
                }
            }

            return null; // No common substring
        }
        
        public static bool IsSubsequence(this string s, string strToFind)
        {
            // Is strToFind a subsequence of s?
            // Given: "AXY", s = "ADXCPY", strToFind = "AXY"
            // Returns: true

            int n = strToFind.Length, m = s.Length;
            int i = 0, j = 0;
            while (i < n && j < m)
            {
                if (strToFind[i] == s[j])
                    i++;
                j++;
            }
            
            return i == n;
        }

        public static bool IsPangram(this string s)
        {
            // A pangram contains every letter from the English alphabet.

            bool[] v = new bool[26];
            
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (c >= 'A' && c <= 'Z')
                    v[c - 'A'] = true;
                else if (c >= 'a' && c <= 'z')
                    v[c - 'a'] = true;
            }
            
            for (int i = 0; i < 26; i++)
            {
                if (!v[i])
                    return false;
            }

            return true;
        }

        public static string Shift(this string s, int count)
        {
            if (count == 0) return s;
            else if (count < 0)
            {
                // Shift left
                // Hello World! -> lo World!Hel
                return s[(count * -1)..] + s[..(count * -1)];
            }
            else
            {
                // Shift right
                // Hello World! -> ld!Hello Wor
                return s.Right(count) + s.Left(s.Length - 3);
            }
        }

        public static IEnumerable<string> Permute(this string s)
        {
            if (s.Length <= 1)
            {
                yield return s;
                yield break;
            }

            var first = s[0];

            foreach (var rem in Permute(s[1..]))
            {
                yield return first + rem;
                yield return first + " " + rem;
            }
        }

        public static string Left(this string s, int length)
        {
            // E.g. helloworld, 2 returns "he"

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
            return encoding.GetBytes(s);
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
            return str.AllIndexesOf(substring, 0);
        }

        public static List<int> AllIndexesOf(this string str, string substring, int base_index)
        {
            List<int> indexes = [];

            for (int index = 0; index < str.Length; index++)
            {
                index = str.IndexOf(substring, index);
                if (index == -1)
                    break;
                indexes.Add(index + base_index);
            }

            return indexes;
        }

        /// <summary>
        /// Converts any object to a CSV-safe string value.
        /// Handles null values, escapes quotes, and wraps values containing special characters in quotes.
        /// </summary>
        /// <typeparam name="T">The type of the item to convert</typeparam>
        /// <param name="item">The item to convert to a CSV value</param>
        /// <returns>A CSV-safe string representation of the item</returns>
        public static string ToCsvValue<T>(this T item)
        {
            if (item == null)
            {
                return string.Empty;
            }

            string? value = item.ToString();

            // If the value is empty, return empty string
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            // Check if the value contains special CSV characters that require quoting
            bool needsQuoting = value.Contains(',') ||
                               value.Contains('"') ||
                               value.Contains('\n') ||
                               value.Contains('\r') ||
                               value.StartsWith(' ') ||
                               value.EndsWith(' ');

            if (needsQuoting)
            {
                // Escape any existing quotes by doubling them
                value = value.Replace("\"", "\"\"");

                // Wrap the entire value in quotes
                return $"\"{value}\"";
            }

            return value;
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

        /// <summary>
        /// Parses a row-delimited string into a list of strings.
        /// Supports multiple line ending formats: \n, \r\n, and \r
        /// </summary>
        /// <param name="input">The row-delimited string to parse</param>
        /// <param name="removeEmptyEntries">Whether to remove empty entries from the result</param>
        /// <returns>A list of strings representing each row</returns>
        public static List<string> ParseRowDelimitedString(this string input, bool removeEmptyEntries = false)
        {
            var options = removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None;

            return [.. input.Split(["\r\n", "\r", "\n"], options)];
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
                xmlString = xmlString[byteOrderMarkUtf8.Length..];
            }

            return xmlString;
        }

        public static string? Merge(this List<string> stringList, char delimiter)
        {
            // Inverse of split for a list.
            
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

        public static bool IsPermutation(this long n, long numToMatch)
        {
            return IsPermutation(n.ToString(), numToMatch.ToString());
        }

        public static bool IsPermutation(this string str, string strToMatch)
        {
            if (str.Length != strToMatch.Length)
                return false;

            char[] c1 = str.ToCharArray();
            char[] c2 = strToMatch.ToCharArray();

            Array.Sort(c1);
            Array.Sort(c2);

            for (int i = 0; i < c1.Length; i++)
                if (c1[i] != c2[i])
                    return false;

            return true;
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

        private static readonly JsonSerializerOptions s_writeOptions = new()
        {
            WriteIndented = true
        };

        public static string ToPrettyJson(this string unprettyJson)
        {
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(unprettyJson);

            return JsonSerializer.Serialize(jsonElement, s_writeOptions);
        }
    }
}
