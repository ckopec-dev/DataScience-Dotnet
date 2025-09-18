using ExtendedNumerics;
using SkiaSharp;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Core
{
    public static class NumberExtensions
    {
        public static short Reverse(this short n)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(n);

            char[] charArray = n.ToString().ToCharArray();
            Array.Reverse(charArray);
            string numberReversed = new(charArray);

            return Convert.ToInt16(numberReversed);
        }

        public static ushort Reverse(this ushort n)
        {
            char[] charArray = n.ToString().ToCharArray();
            Array.Reverse(charArray);
            string numberReversed = new(charArray);

            return Convert.ToUInt16(numberReversed);
        }

        public static int Reverse(this int n)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(n);

            char[] charArray = n.ToString().ToCharArray();
            Array.Reverse(charArray);
            string numberReversed = new(charArray);

            return Convert.ToInt32(numberReversed);

        }
        
        public static uint Reverse(this uint n)
        {
            char[] charArray = n.ToString().ToCharArray();
            Array.Reverse(charArray);
            string numberReversed = new(charArray);

            return Convert.ToUInt32(numberReversed);
        }

        public static long Reverse(this long n)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(n);

            char[] charArray = n.ToString().ToCharArray();
            Array.Reverse(charArray);
            string numberReversed = new(charArray);

            return Convert.ToInt64(numberReversed);
        }

        public static ulong Reverse(this ulong n)
        {
            char[] charArray = n.ToString().ToCharArray();
            Array.Reverse(charArray);
            string numberReversed = new(charArray);

            return Convert.ToUInt64(numberReversed);
        }

        public static long SumOfDigits(this byte n)
        {
            return ((long)n).SumOfDigits();
        }

        public static long SumOfDigits(this short n)
        {
            return ((long)n).SumOfDigits();
        }

        public static long SumOfDigits(this int n)
        {
            return ((long)n).SumOfDigits();
        }

        public static long SumOfDigits(this long n)
        {
            long sum = 0;

            while (n != 0)
            {
                long last = n % 10;
                sum += last;
                n /= 10;
            }

            return sum;
        }

        public static BigInteger SumOfDigits(this BigInteger n)
        {
            BigInteger sum = 0;

            while (n != 0)
            {
                BigInteger last = n % 10;
                sum += last;
                n /= 10;
            }

            return sum;
        }

        public static BigInteger SumOfDecimalDigits(this BigDecimal n)
        {
            string s = n.ToString();
            int idx = n.GetDecimalIndex() + 1;
            s = s[idx..];

            BigInteger sum = BigInteger.Parse(s);
            sum = sum.SumOfDigits();

            sum += n.WholeValue.SumOfDigits();

            return sum;
        }

        public static bool IsArmstrong(this int n)
        {
            // Returns true if the sum of the digits of n raised to a power are 
            // equal to n.
            // Example:
            // 9474 = 9^4 + 4^4 + 7^4 + 4^4

            string number = n.ToString();

            int length = number.Length;
            int output = 0;

            foreach (char c in number)
            {
                output += (int)Math.Pow(c - '0', length);
            }

            if (output == int.Parse(number))
                return true;
            else
                return false;
        }

        public static List<int> GetSquareRootDigits(this int n, int digitCount)
        {
            // Use the long division method
            List<int> digits = [];

            // Add initial integer part
            int intPart = (int)Math.Sqrt(n);
            BigInteger intPartSquared = intPart * intPart;

            // Subtract integer part squared from n to get initial remainder
            BigInteger remainder = n - intPartSquared;
            digits.Add(intPart);

            // Multiply remainder by 100 to simulate decimal expansion
            remainder *= 100;

            BigInteger result = intPart;

            for (int i = 0; i < digitCount - 1; i++) // already got 1 digit
            {
                // Multiply result by 20 and find next digit
                BigInteger candidateBase = result * 20;
                int digit = 0;
                while (true)
                {
                    BigInteger test = (candidateBase + digit + 1) * (digit + 1);
                    if (test > remainder)
                        break;
                    digit++;
                }

                // Subtract and update remainder
                BigInteger subtrahend = (candidateBase + digit) * digit;
                remainder -= subtrahend;
                remainder *= 100;

                result = result * 10 + digit;
                digits.Add(digit);
            }

            return digits;
        }

        public static int SquareDigitChain(this int n)
        {
            // A number chain is created by continuously adding the square of
            // the digits in a number to form a new number until
            // it has been seen before.

            // EVERY starting number will eventually arrive at 1 or 89.

            // Returns the final repeating number (i.e. 1 or 89).

            // E.g.
            // 44 -> 32 -> 13 -> 10 -> 1 -> 1

            ArgumentOutOfRangeException.ThrowIfLessThan(n, 1);

            while (n != 1 && n != 89)
            {
                List<int> digits = n.ToListOfDigits();

                int sum = 0;

                foreach(int digit in digits)
                {
                    sum += checked(digit * digit);
                }

                n = sum;
            }

            return n;
        }

        public static bool IsPositive(this ulong n)
        {
            if (n > 0)
                return true;
            else
                return false;
        }

        public static bool IsPerfectSquare(this ulong n)
        {
            double sqrt = Math.Sqrt(n);

            if (Math.Abs(Math.Ceiling(sqrt) - Math.Floor(sqrt)) < Double.Epsilon)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsSquare(this double n)
        {
            double root = Math.Sqrt(n);
            return (root - Math.Floor(root)) == 0;
        }

        public static bool IsSquare(this BigDecimal n, int decimalPlaces)
        {
            BigDecimal root = BigDecimal.SquareRoot(n, decimalPlaces);
            return (root - BigDecimal.Floor(root)) == 0;
        }

        public static ulong ContinuedFraction(this ulong x, out List<ulong> repeat)
        {
            // Returns the integer value of the root.
            // Out repeat value is a list of the repeating values of the root.
            // E.g.: square root of 23 = 4;1,3,1,8

            repeat = [];

            ulong original_root = (ulong)Math.Truncate(Math.Sqrt(x));
            
            ulong step_a = 0;
            ulong a = original_root;
            ulong numerator = 0;
            ulong denominator = 1;

            while (a != 2 * original_root)
            {
                numerator = denominator * a - numerator;
                denominator = (x - numerator * numerator) / denominator;
                a = (original_root + numerator) / denominator;

                repeat.Add(a);
                step_a++;
            }

            return original_root;
        }

        public static string PrettyPrint(this List<int> list)
        {
            return String.Format("{{ {0} }}", String.Join(", ", list));
        }

        public static string PrettyPrint(this List<long> list)
        {
            return String.Format("{{ {0} }}", String.Join(", ", list));
        }

        public static int Concatenate(this int num1, int num2)
        {
            int num2Length = 10;

            while (num2Length <= num2)
            {
                num2Length *= 10;
            }

            return (num1 * num2Length) + num2;
        }

        public static bool HasSameDigits(this int num1, int num2)
        {
            int n1 = num1.ToString().Length;
            
            char[] c1 = num1.ToString().ToCharArray();
            char[] c2 = num2.ToString().ToCharArray();

            Array.Sort(c1);
            Array.Sort(c2);

            for (int i = 0; i < n1; i++)
                if (c1[i] != c2[i])
                    return false;

            return true;
        }

        public static bool HasSameDigits(this ulong num1, ulong num2)
        {
            int n1 = num1.ToString().Length;
            int n2 = num2.ToString().Length;

            if (n1 != n2)
                return false;

            char[] c1 = num1.ToString().ToCharArray();
            char[] c2 = num2.ToString().ToCharArray();

            Array.Sort(c1);
            Array.Sort(c2);

            for (int i = 0; i < n1; i++)
                if (c1[i] != c2[i])
                    return false;

            return true;
        }

        public static string ToDelimitedString(this int[] numbers, char delimiter)
        {
            StringBuilder sb = new();

            for (int i = 0; i < numbers.Length; i++)
            {
                if (i > 0)
                    sb.Append(delimiter);

                sb.Append(numbers[i]);
            }

            return sb.ToString();
        }

        public static string ToDelimitedString(this double[] numbers, char delimiter)
        {
            StringBuilder sb = new();

            for (int i = 0; i < numbers.Length; i++)
            {
                if (i > 0)
                    sb.Append(delimiter);

                sb.Append(numbers[i]);
            }

            return sb.ToString();
        }

        public static decimal[] ToDecimalArray(this string[] n)
        {
            decimal[] d = new decimal[n.Length];

            for (int i = 0; i < n.Length; i++)
                d[i] = Convert.ToDecimal(n[i]);

            return d;
        }

        public static List<int> ToIntList(this string n, char delimiter)
        {
            List<int> retval = [];

            string[] items = n.Split(delimiter);

            foreach (string item in items)
                retval.Add(Convert.ToInt32(item));

            return retval;
        }

        public static string ToBinary(this int n)
        {
            return Convert.ToString(n, 2);
        }

        public static int ToInt32FromBinary(this string binaryNumber)
        {
            return Convert.ToInt32(binaryNumber, 2);
        }

        public static string ToHex(this int n)
        {
            return n.ToString("X");
        }

        public static int ToInt32FromHex(this string hexNumber)
        {
            return int.Parse(hexNumber, NumberStyles.HexNumber);
        }

        public static List<string> ToNibbleList(this int n)
        {
            // Also known as binary coded decimal. Each decimal digit is represented as a 4-bit nibble.

            List<string> nibbles = [];
            List<int> digits = n.ToListOfDigits();

            foreach (int d in digits)
            {
                nibbles.Add(d.ToBinary().PadLeft(4, '0'));
            }

            return nibbles;
        }

        public static int ToInt32FromNibbleList(this List<string> nibbleList)
        {
            List<int> list = [];
            List<int> digits = list;

            foreach (string n in nibbleList)
            {
                digits.Add(n.ToInt32FromBinary());
            }

            return digits.ToIntFromDigitList();
        }

        public static decimal ToMicrons(this decimal millimeters)
        {
            return millimeters * 0.0001m;
        }

        public static bool IsProbablyPrime(this int number)
        {
            return number.IsProbablyPrime(10);
        }

        public static bool IsProbablyPrime(this int number, int certainty)
        {
            if (number < 2)
                return false;
            if (number == 2 || number == 3 || number == 5 || number == 7)
                return true;

            Random rnd = new();
            
            checked
            {
                for (int i = 0; i < certainty; i++)
                {
                    long j = rnd.Next(2, number);

                    long k = j;
                    for (int power = 1; power < number - 1; power++)
                    {
                        k = (k * j) % number;
                    }

                    if (k != 1) return false;
                }
            }

            return true;
        }

        public static bool IsEven(this int number)
        {
            if ((number & 1) == 0)
                return true;
            else
                return false;
        }

        public static bool IsEven(this BigInteger number)
        {
            if ((number & 1) == 0)
                return true;
            else
                return false;
        }

        public static bool IsOdd(this int number)
        {
            if ((number & 1) == 1)
                return true;
            else
                return false;
        }

        public static bool IsOdd(this BigInteger number)
        {
            if ((number & 1) == 1)
                return true;
            else
                return false;
        }

        public static bool IsPalindrome(this int number)
        {
            return ((long)number).IsPalindrome();
        }

        public static bool IsPalindrome(this long number)
        {
            string numberReversed = number.Reverse().ToString();

            if (number.ToString() == numberReversed)
                return true;
            else
                return false;
        }

        public static int TriangleNumber(this int number)
        {
            // The sequence of triangle numbers is generated by adding the natural numbers. So the 7th triangle number would be 1 + 2 + 3 + 4 + 5 + 6 + 7 = 28.

            if (number < 0)
                throw new NotSupportedException("Only natural numbers are supported.");

            int result = 0;

            for (int i = 1; i <= number; i++)
            {
                result += i;
            }

            return result;
        }

        public static List<long> ProperDivisors(this long number)
        {
            List<long> divisors = [];

            for (long i = 1; i < number; i++)
            {
                if (number % i == 0)
                    divisors.Add(i);
            }

            return divisors;
        }

        public static List<int> ProperDivisors(this int number)
        {
            List<int> divisors = [];

            for (int i = 1; i < number; i++)
            {
                if (number % i == 0)
                    divisors.Add(i);
            }

            return divisors;
        }

        public static bool IsPerfect(this int number)
        {
            if (number == number.ProperDivisors().Sum())
                return true;
            else
                return false;
        }

        public static int Sum(this int n)
        {
            // Sums all numbers from 1 to n.

            return n * (n + 1) / 2;
        }

        public static int Sum(this List<int> numbers)
        {
            int sum = 0;

            foreach (int n in numbers)
                sum += n;

            return sum;
        }

        public static int Product(this List<int> numbers)
        {
            int product = numbers[0];

            for (int n = 1; n < numbers.Count; n++)
                product *= numbers[n];

            return product;
        }

        public static bool SameDigits(this int number, List<int> numbersToCompare)
        {
            // Do all the numbers in numbersToCompare have the same digits (order insensitive) as number?
            string n = number.ToStringOfOrderedDigits();

            foreach (int c in numbersToCompare)
            {
                if (n != c.ToStringOfOrderedDigits())
                    return false;
            }

            return true;
        }

        public static string ToStringOfOrderedDigits(this int number)
        {
            // Rearrange the digits into an ordered string.
            List<int> digits = number.ToListOfDigits();

            digits.Sort();

            StringBuilder sb = new();
            foreach (int digit in digits)
                sb.Append(digit);

            return sb.ToString();
        }

        public static int GetDigit(this int number, int index)
        {
            // Returns digit at index. 
            // E.g. 1234.GetDigit(2) = 3.

            return Convert.ToInt32(number.ToString().Substring(index, 1));
        }

        public static int GetDigit(this long number, int index)
        {
            // Returns digit at index. 
            // E.g. 1234.GetDigit(2) = 3.

            return Convert.ToInt32(number.ToString().Substring(index, 1));
        }

        public static int RemoveDigit(this int number, int digitToRemove)
        {
            // Remove the digits. 
            // E.g. 1234423.RemoveDigit(2) = 13443.

            string s = number.ToString();

            s = s.Replace(digitToRemove.ToString(), "");

            return Convert.ToInt32(s);
        }

        public static int ReplaceDigit(this int number, int index, int newDigit)
        {
            // Replaces digit at index with newDigit.
            // E.g. 12345.ReplaceDigit(2,7) = 12745.

            string n = number.ToString();

            string start = n[..index];
            string end = n[(index + 1)..];

            return Convert.ToInt32(start + newDigit.ToString() + end);
        }

        public static List<int> CommonDigits(this int number, int numberToMatch)
        {
            // Returns a list of unique single digit ints that occur in both numbers.
            // E.g. number = 234, numberToMatch = 328, returns list containing 2, 3.

            string s1 = number.ToString();
            string s2 = numberToMatch.ToString();

            List<int> result = [];

            for (int i = 0; i < s1.Length; i++)
            {
                for (int j = 0; j < s2.Length; j++)
                {
                    if (s1.Substring(i, 1) == s2.Substring(j, 1))
                    {
                        int n = Convert.ToInt32(s1.Substring(i, 1));

                        // If not in list, add it.
                        if (!result.Contains(n))
                        {
                            result.Add(n);
                        }
                    }
                }
            }

            return result;
        }

        public static BigInteger Factorial(this int val)
        {
            if (val < 0)
                throw new Exception("Negative factorials are undefined.");
            else if (val == 0)
                return new BigInteger(1);
            else
            {
                BigInteger b = 1;

                for (int i = 1; i <= val; i++)
                {
                    b *= i;
                }

                return b;
            }
        }

        public static List<int> ToListOfDigits(this int n)
        {
            return ToListOfDigits((ulong)n);   
        }

        public static List<int> ToListOfDigits(this ulong n)
        {
            // Given a number such as 147, return a list of all the digits. E.g. 1, 4, 7.
            // For positive numbers only.

            List<int> vals = [];

            string v = n.ToString();

            for (int i = 0; i < v.Length; i++)
            {
                vals.Add(Convert.ToInt32(v.Substring(i, 1)));
            }

            return vals;
        }

        public static int ToIntFromDigitList(this List<int> vals)
        {
            StringBuilder sb = new();
            for (int i = 0; i < vals.Count; i++)
                sb.Append(vals[i]);

            return Convert.ToInt32(sb.ToString());
        }

        public static int[] ToArray(this string[] vals)
        {
            int[] s = new int[vals.Length];
            for (int i = 0; i < vals.Length; i++)
                s[i] = Convert.ToInt32(vals[i]);
            return s;
        }

        public static char[] ToCharArray(this int[] vals)
        {
            char[] c = new char[vals.Length];
            for (long i = 0; i < vals.LongLength; i++)
                c[i] = Convert.ToChar(vals[i]);
            return c;
        }

        public static long[] ToLongArray(this string[] vals)
        {
            long[] s = new long[vals.LongLength];
            for (long i = 0; i < vals.LongLength; i++)
                s[i] = Convert.ToInt64(vals[i]);
            return s;
        }

        public static long PandigitalMultiple(this int n, List<int> m)
        {
            // Take the number n = 192 and multiply it by each of m = 1, 2, and 3:

            // 192 × 1 = 192
            // 192 × 2 = 384
            // 192 × 3 = 576

            // By concatenating each product we get the 1 to 9 pandigital, 192384576. We will call 192384576 the concatenated product of 192 and(1, 2, 3)

            StringBuilder sb = new();

            for (int i = 0; i < m.Count; i++)
            {
                long r = n * m[i];

                sb.Append(r);
            }

            long result = Convert.ToInt64(sb.ToString());

            return result;
        }

        public static int ToInt32(this long n, int startIndex, int length)
        {
            return n.ToString().ToInt32(startIndex, length);
        }

        public static int ToInt32(this string s, int startIndex, int length)
        {
            return Convert.ToInt32(s.Substring(startIndex, length));
        }

        public static int Min(this int[] intArray)
        {
            int m = Int32.MaxValue;

            foreach (int n in intArray)
            {
                if (n < m)
                    m = n;
            }

            return m;
        }

        public static int Min(this int[] intArray, out int indexPosition)
        {
            int m = Int32.MaxValue;
            int idx = 0;

            for (int i = 0; i < intArray.Length; i++)
            {
                int n = intArray[i];

                if (n < m)
                {
                    m = n;
                    idx = i;
                }
            }

            indexPosition = idx;

            return m;
        }

        public static int Max(this int[] intArray)
        {
            int m = Int32.MinValue;

            foreach (int n in intArray)
            {
                if (n > m)
                    m = n;
            }

            return m;
        }

        public static List<int[]> ToIntListArray(this string val, char rowDelimiter, char colDelimiter)
        {
            List<int[]> data = [];

            string[] lines = val.Split(rowDelimiter);

            for (int i = 1; i <= lines.Length; i++)
            {
                string[] fields = lines[i - 1].Split(colDelimiter);

                int[] d = new int[fields.Length];

                for (int j = 0; j < fields.Length; j++)
                {
                    d[j] = Convert.ToInt32(fields[j].Trim());
                }

                data.Add(d);
            }

            return data;
        }

        public static List<double[]> ToDoubleListArray(this string val, char rowDelimiter, char colDelimiter)
        {
            List<double[]> data = [];

            string[] lines = val.Split(rowDelimiter);

            //Console.WriteLine("lines: {0}", lines);
            for (int i = 1; i <= lines.Length; i++)
            {
                string[] fields = lines[i - 1].Split(colDelimiter);

                double[] d = new double[fields.Length];

                for (int j = 0; j < fields.Length; j++)
                {
                    d[j] = Convert.ToDouble(fields[j].Trim());
                }

                data.Add(d);
            }

            return data;
        }

        public static bool IsPerfectSquare(this int n)
        {
            if (Math.Sqrt(n) % 1 == 0)
                return true;
            else
                return false;
        }

        public static bool IsPerfectCube(this int n)
        {
            double power = 1d / 3d;
            if (Math.Pow(n, power) % 1 == 0)
                return true;
            else
                return false;
        }

        public static string EncryptXOR(this string message, string password)
        {
            var msg = Encoding.UTF8.GetBytes(message);
            var pwd = Encoding.UTF8.GetBytes(password);

            return Convert.ToBase64String(msg.Select((b, i) => (byte)(b ^ pwd[i % pwd.Length])).ToArray());
        }

        public static string DecryptXOR(this string message, string password)
        {
            var msg = Convert.FromBase64String(message);
            var pwd = Encoding.UTF8.GetBytes(password);

            return Encoding.UTF8.GetString([.. msg.Select((b, i) => (byte)(b ^ pwd[i % pwd.Length]))]);
        }

        public static double ToRadians(this double degrees)
        {
            return ((Math.PI / 180d) * degrees);
        }

        public static double ToDegrees(this double radians)
        {
            return ((180d / Math.PI) * radians);
        }
    }
}
