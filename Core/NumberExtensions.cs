using ExtendedNumerics;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace Core
{
    public static class NumberExtensions
    {
        public static long SumOfSquares(this int n)
        {
            // Returns the sum of squares of 1 to n (inclusive).
            // E.g. 2 -> 5
            // 1^2 + 2^2 = 5
            return (n * (n + 1) / 2) * (2 * n + 1) / 3;
        }

        public static List<int> GeneratePrimes(this int n)
        {
            List<int> primes = new List<int>();
            for (int i = 2; i <= n; i++)
            {
                if (IsPrime(i))
                    primes.Add(i);
            }
            return primes;
        }

        public static int PrimePartitionCount(this int n)
        {
            var primes = GeneratePrimes(n);
            int[] dp = new int[n + 1];
            dp[0] = 1;

            foreach (int prime in primes)
            {
                for (int i = prime; i <= n; i++)
                {
                    dp[i] += dp[i - prime];
                }
            }

            return dp[n];
        }

        public static BigInteger PartitionCount(this int n)
        {
            // Returns the number of ways n can be partitioned with integers.

            if (n < 0) return 0;
            if (n == 0) return 1;

            BigInteger[] p = new BigInteger[n + 1];
            p[0] = 1;

            for (int i = 1; i <= n; i++)
            {
                int k = 1;
                while (true)
                {
                    int j1 = k * (3 * k - 1) / 2;
                    if (j1 > i) break;
                    p[i] += (k % 2 == 1 ? 1 : -1) * p[i - j1];

                    int j2 = k * (3 * k + 1) / 2;
                    if (j2 > i) break;
                    p[i] += (k % 2 == 1 ? 1 : -1) * p[i - j2];

                    k++;
                }
            }

            return p[n];
        }

        public static bool ReversiblePrimeSquare(this int n)
        {
            // We call a number a reversible prime square if:
            //  It is not a palindrome, and
            //  It is the square of a prime, and
            //  Its reverse is also the square of a prime.

            // E.g.: 169

            //Console.WriteLine("IsPalindrome: {0}", n.IsPalindrome());
            //Console.WriteLine("IsPrimeSquare: {0}", n.IsPrimeSquare());
            //Console.WriteLine("ReverseIsPrimeSquare: {0}", n.Reverse().IsPrimeSquare());

            if (!n.IsPalindrome() &&
                n.IsPrimeSquare() && 
                n.Reverse().IsPrimeSquare()
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsPrimeSquare(this int n)
        {
            // Returns true if n is the square of a prime.
            // E.g. 169 returns true because 13 * 13 = 169.

            bool isPrimeSquare = false;

            if (n.IsPerfectSquare())
            {
                int root = Convert.ToInt32(Math.Sqrt(n));
                
                if (root.IsPrime())
                {
                    isPrimeSquare = true;
                }
            }

            return isPrimeSquare;
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
            if (n == 1 || n == 89) return n;

            while (n != 1 && n != 89)
            {
                //Console.WriteLine(n);

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

        public static bool IsSNumber(this ulong n)
        {
            // We define an S-number to be a natural number, n,
            // that is a perfect square and its square root can be obtained by
            // splitting the decimal representation of n into 2 or more 
            // numbers then adding the numbers.

            // Examples:
            // sqr(81) = 8 + 1 = 9.
            // sqr(6724) = 6 + 72 + 4 = 82.

            if (n < 10)
                return false;

            if (n.IsPerfectSquare())
            {
                double sqrt = Math.Floor(Math.Sqrt(n));
                //Console.WriteLine("sqrt: {0}", sqrt);

                string s = n.ToString();
                List<string> list = [.. s.Permute()];
                foreach (string l in list)
                {
                    string[] parts = l.Split(' ');

                    if (parts.Length > 1)
                    {
                        //Console.WriteLine(l);

                        ulong sum = 0;
                        foreach(string p in parts)
                        {
                            sum += Convert.ToUInt64(p);
                        }

                        //Console.WriteLine("sum: {0}", sum);

                        if (sum == sqrt)
                            return true;
                    }
                }

                return false;
            }
            else
            {
                return false;
            }
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

        public static List<BigInteger> FactorialDigitCycle(this BigInteger n)
        {
            // Returns chain of numbers formed by recursively iterating SumFactorialDigits(n).
            // Only non-repeating part of the chain is returned.
            // Examples:
            // 169 -> 363601 -> 1454 (-> 169)
            // 78 -> 45360 -> 871 -> 45361 (-> 871)
            // 540 -> 145 ( -> 145)

            List<BigInteger> result = [];
            BigInteger next = SumFactorialDigits(n);
            result.Add(n);
            bool repeat = false;

            while(!repeat)
            {
                result.Add(next);
                next = SumFactorialDigits(next);
                repeat = result.Contains(next);
            }

            return result;
        }

        public static BigInteger SumFactorialDigits(this BigInteger n)
        {
            // Returns the sum of the factorial of n's digits.
            // E.g. SumFactorialDigits(145) = 1! + 4! + 5! = 1 + 24 + 120 = 145.

            List<int> digits = n.ToListOfDigits();

            BigInteger sum = BigInteger.Zero;

            foreach(int digit in digits)
            {
                sum += digit.Factorial();
            }

            return sum;
        }

        public static ulong Phi(this ulong n)
        {
            if (n < 3) return 1;
            if (n == 3) return 2;

            ulong totient = n;

            if ((n & 1) == 0)
            {
                totient >>= 1;
                while (((n >>= 1) & 1) == 0) ;
            }

            for (ulong i = 3; i * i <= n; i += 2)
            {
                if (n % i == 0)
                {
                    totient -= totient / i;
                    while ((n /= i) % i == 0) ;
                }
            }
            if (n > 1) totient -= totient / n;
            return totient;
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

        public static bool HasMinimalDifference(this List<double> values, double n, double margin = 0.00001)
        {
            // Returns true if any member of the list has a minimal difference with n.
            foreach (var v in values)
            {
                if (v.HasMinimalDifference(n, margin))
                    return true;
            }
            return false;
        }

        public static bool HasMinimalDifference(this double value1, double value2, double margin = 0.00001)
        {
            double difference = Math.Abs(value1 * margin);

            if (Math.Abs(value1 - value2) <= difference) 
                return true;
            else
                return false;
        }

        public static ulong ContinuedFraction(this ulong x, out List<ulong> repeat)
        {
            // Returns the integer value of the root.
            // Out repeat value is a list of the repeating values of the root.
            // E.g.: square root of 23 = 4;1,3,1,8

            repeat = [];

            ulong original_root = (ulong)Math.Truncate(Math.Sqrt(x));
            //Console.WriteLine("Original root: {0}", original_root);

            if (original_root * original_root == x) return x;

            ulong step_a = 0;
            ulong a = original_root;
            ulong numerator = 0;
            ulong denominator = 1;

            while (a != 2 * original_root)
            {
                //Console.WriteLine("step a({0}): n: {1}, d: {2}, a: {3}", step_a, numerator, denominator, a);

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

        public static bool IsCyclicSet(this List<int> set, int cycleSize)
        {
            for(int i = 0; i < set.Count; i++)
            {
                if (i == set.Count - 1)
                {
                    // It's the last item in the set, so the cycle continues with the first item of the set.
                    
                    string right = set[i].ToString().Right(cycleSize);
                    string left = set[0].ToString().Left(cycleSize);

                    if (right != left)
                        return false;
                }
                else
                {
                    if (set[i].ToString().Right(cycleSize) != set[i + 1].ToString().Left(cycleSize))
                        return false;
                }
            }

            return true;
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

        public static bool IsApocalypseNumber(this BigInteger n)
        {
            if (n.ToString().Length == 666)
                return true;
            else
                return false;
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

            if (String.IsNullOrWhiteSpace(n))
                return retval;

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

        public static string ToRomanNumeral(this int n)
        {
            string invalidValueMsg = "Valid range is from -3999 to 3999 inclusive.";

            if (n < -3999 || n > 3999) throw new ArgumentOutOfRangeException(nameof(n), invalidValueMsg);

            if (n == 0) return "N";
            if (n < 0) return "-" + n.ToRomanNumeral();
            if (n >= 1000) return "M" + (n - 1000).ToRomanNumeral();
            if (n >= 900) return "CM" + (n - 900).ToRomanNumeral();
            if (n >= 500) return "D" + (n - 500).ToRomanNumeral();
            if (n >= 400) return "CD" + (n - 400).ToRomanNumeral();
            if (n >= 100) return "C" + (n - 100).ToRomanNumeral();
            if (n >= 90) return "XC" + (n - 90).ToRomanNumeral();
            if (n >= 50) return "L" + (n - 50).ToRomanNumeral();
            if (n >= 40) return "XL" + (n - 40).ToRomanNumeral();
            if (n >= 10) return "X" + (n - 10).ToRomanNumeral();
            if (n >= 9) return "IX" + (n - 9).ToRomanNumeral();
            if (n >= 5) return "V" + (n - 5).ToRomanNumeral();
            if (n >= 4) return "IV" + (n - 4).ToRomanNumeral();
            if (n >= 1) return "I" + (n - 1).ToRomanNumeral();

            throw new ArgumentOutOfRangeException(nameof(n), invalidValueMsg);
        }

        public static BigInteger Factorial(this BigInteger n)
        {
            // Returns n!

            BigInteger result = new(1);

            for (BigInteger i = n; i > 1; i--)
            {
                result *= i;
            }

            return result;
        }

        public static List<long> Factor(this long n)
        {
            List<long> result = [];

            while (n % 2 == 0)
            {
                result.Add(2);
                n /= 2;
            }

            // Take out other primes.
            long factor = 3;
            while (factor * factor <= n)
            {
                if (n % factor == 0)
                {
                    // This is a factor.
                    result.Add(factor);
                    n /= factor;
                }
                else
                {
                    // Go to the next odd number.
                    factor += 2;
                }
            }

            if (n > 1)
                result.Add(n);

            return result;
        }

        public static decimal ToMicrons(this decimal millimeters)
        {
            return millimeters * 0.0001m;
        }

        public static int ToFloorInt(this decimal n)
        {
            return (int)Math.Floor((double)n);
        }

        public static int ToIntegerPart(this decimal n)
        {
            // Returns the portion of the number to the left of the decimal point.

            decimal t = Math.Abs(n);
            t = Math.Floor(t);

            if (n < 0)
                return Convert.ToInt32(t * -1m);
            else
                return (int)t;
        }

        public static decimal ToFractionalPart(this decimal n)
        {
            // Returns the portion of the number to the right of the decimal point. This is always a non-negative value.

            decimal t = n.ToIntegerPart();
            t = n - t;
            t = Math.Abs(t);

            return t;
        }

        public static bool IsPrime(this int number)
        {
            //Console.WriteLine("entering IsPrime(int)");

            return ((long)number).IsPrime();
        }

        public static bool IsPrime(this ulong number)
        {
            if (number < 2)
                return false;

            if (number == 2 || number == 3 || number == 5 || number == 7)
                return true;

            if (number % 2 == 0)
            {
                return false;
            }

            ulong sqrt = Convert.ToUInt64(Math.Sqrt(number));
            for (ulong t = 3; t <= sqrt; t += 2)
            {
                if (number % t == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsPrime(this long number)
        {
            //Console.WriteLine("entering IsPrime(long)");

            return ((ulong)number).IsPrime();
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

        public static bool ArePrime(this List<int> numbers)
        {
            // Returns true if all members of the list are prime.

            foreach (int n in numbers)
                if (!n.IsPrime())
                    return false;

            return true;
        }

        public static int Reverse(this int number)
        {
            char[] charArray = number.ToString().ToCharArray();
            Array.Reverse(charArray);
            string numberReversed = new(charArray);

            return Convert.ToInt32(numberReversed);
        }

        public static long Reverse(this long number)
        {
            char[] charArray = number.ToString().ToCharArray();
            Array.Reverse(charArray);
            string numberReversed = new(charArray);

            return Convert.ToInt64(numberReversed);
        }

        public static BigInteger Reverse(this BigInteger number)
        {
            char[] charArray = number.ToString().ToCharArray();
            Array.Reverse(charArray);
            string numberReversed = new(charArray);

            if (numberReversed == null)
                return BigInteger.Zero;

            return BigInteger.Parse(numberReversed);
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

        public static bool IsPalindrome(this BigInteger number)
        {
            BigInteger reversed = number.Reverse();

            if (number.ToString() == reversed.ToString())
                return true;
            else
                return false;
        }

        public static bool IsTruncatablePrime(this long number)
        {
            // See https://projecteuler.net/problem=37.

            if (!number.IsPrime())
                return false;

            if (number < 10)
                return false;

            for (int i = 1; i < number.ToString().Length; i++)
            {
                long newNum = Convert.ToInt64(number.ToString()[i..]);

                if (!newNum.IsPrime())
                    return false;

                newNum = Convert.ToInt64(number.ToString()[..i]);

                if (!newNum.IsPrime())
                    return false;
            }

            return true;
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

        public static string ToWords(this int number)
        {
            // Converts number to words. e.g. 100 = one hundred.
            // Int max size is 2,147,483,647
            // Some redundant code once we get in the 100s. If exdpanding this to support longs, it would be useful to chunk it out into a helper function.

            string[] list = [ "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen",
                "sixteen", "seventeen", "eighteen", "nineteen", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" ];

            Dictionary<int, string> d = [];

            // Add everything under 20.
            for (int i = 0; i <= 20; i++)
                d.Add(i, list[i]);

            // Add the 10s.
            int idx = 20;
            for (int i = idx + 1; i < 28; i++)
            {
                idx += 10;
                d.Add(idx, list[i]);
            }

            // Handle negative numbers...
            if (number < 0)
                return "negative " + (number * -1).ToWords();

            if (number <= 20)
                return d[number];

            if (number > 20 && number < 100)
            {
                // E.g. 38 becomes 30.
                int tens = number / 10 * 10;
                int ones = number - tens;
                if (ones == 0)
                    return d[tens];
                else
                    return d[tens] + " " + d[ones];
            }

            // Hundreds
            if (number >= 100 && number < 1000)
            {
                int hundreds = number / 100;
                string val = hundreds.ToWords() + " hundred";
                int rem = number - (hundreds * 100);
                if (rem == 0)
                    return val;
                else
                    return val + " and " + rem.ToWords();
            }

            // Thousands
            if (number >= 1000 && number < 1000000)
            {
                int thousands = number / 1000;
                string val = thousands.ToWords() + " thousand";
                int rem = number - (thousands * 1000);
                if (rem == 0)
                    return val;
                else
                    return val + " and " + rem.ToWords();
            }

            // Millions
            if (number >= 1000000 && number < 1000000000)
            {
                int millions = number / 1000000;
                string val = millions.ToWords() + " million";
                int rem = number - (millions * 1000000);
                if (rem == 0)
                    return val;
                else
                    return val + " and " + rem.ToWords();
            }

            // Billions
            if (number >= 1000000000)
            {
                int billions = number / 1000000000;
                string val = billions.ToWords() + " billion";
                int rem = number - (billions * 1000000000);
                if (rem == 0)
                    return val;
                else
                    return val + " and " + rem.ToWords();
            }

            if (number == 1000)
                return "one thousand";

            throw new Exception("This number is not supported.");
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

        public static int NumberOfDigits(this BigInteger number)
        {
            return number.ToString().Length;
        }

        public static int SumOfDigits(this int number)
        {
            List<int> digits = number.ToListOfDigits();

            int sum = 0;

            foreach (int digit in digits)
            {
                sum += digit;
            }

            return sum;
        }

        public static int SumOfDigits(this BigInteger number)
        {
            string digits = number.ToString();
            int sum = 0;

            for (int i = 0; i < digits.Length; i++)
            {
                sum += Convert.ToInt32(digits.Substring(i, 1));
            }

            return sum;
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

        public static bool IsPandigital(this long n, int lowDigit, int highDigit)
        {
            // We shall say that an n-digit number is pandigital if it makes use of all the digits lowDigit to highDigit exactly once;
            // for example, the 5-digit number, 15234, is 1 through 5 pandigital.

            string s = n.ToString();
            
            for(int i = lowDigit; i <= highDigit; i++)
            {
                if (s.AllIndexesOf(i.ToString()).Count != 1)
                {
                    return false;
                }
            }

            return true;
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

            if (String.IsNullOrWhiteSpace(s))
                return 0;

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

        public static string? ToSpaceDelimitedString(this int[] values)
        {
            string? s = null;

            for (int i = 0; i < values.Length; i++)
            {
                if (i > 0)
                    s += " ";
                s += values[i].ToString();
            }

            return s;
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

        public static BigInteger SumOfFactorialDigits(this int val)
        {
            if (val < 0)
                throw new Exception("Negative values are undefined for this method.");
            else if (val == 0)
                return new BigInteger(1);
            else
            {
                BigInteger b = 0;

                for (int i = 0; i < val.ToString().Length; i++)
                {
                    int v = val.GetDigit(i);

                    b += v.Factorial();
                }

                return b;
            }
        }

        public static List<int> ToListOfDigits(this BigInteger val)
        {
            if (val < 0)
                throw new Exception("Negative values are not supported.");

            List<int> vals = [];

            string v = val.ToString();

            for (int i = 0; i < v.Length; i++)
            {
                vals.Add(Convert.ToInt32(v.Substring(i, 1)));
            }

            return vals;
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

        public static int ToIntFromDigitArray(this int[] vals)
        {
            // Given array of digits such as 1, 4, 7, return the number they represent. E.g. 147.

            StringBuilder sb = new();
            for (int i = 0; i < vals.Length; i++)
                sb.Append(vals[i]);

            return Convert.ToInt32(sb.ToString());
        }

        public static int ToIntFromDigitList(this List<int> vals)
        {
            StringBuilder sb = new();
            for (int i = 0; i < vals.Count; i++)
                sb.Append(vals[i]);

            return Convert.ToInt32(sb.ToString());
        }

        public static List<int> ToRotations(this int val)
        {
            // Given a number such as 197, return all rotations of its digits. E.g. 197, 971, 719

            List<int> r = [];

            List<int> digits = val.ToListOfDigits();

            for (int i = 0; i < digits.Count; i++)
            {
                int newval = digits.ToIntFromDigitList();
                r.Add(newval);

                // Rotate.
                int currentDigit = digits[0];
                digits.RemoveAt(0);
                digits.Insert(digits.Count, currentDigit);
            }

            return r;
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

        public static int WordValue(this string word)
        {
            decimal ttl = 0;

            for (int i = 0; i < word.Length; i++)
            {
                Char c = Convert.ToChar(word.Substring(i, 1));

                decimal d = (decimal)c - 64;

                if (d < 1 || d > 26)
                    throw new Exception("Unsupported characters in word.");

                ttl += d;
            }

            return Convert.ToInt32(ttl);
        }

        public static int ToInt32(this long n, int startIndex, int length)
        {
            return n.ToString().ToInt32(startIndex, length);
        }

        public static int ToInt32(this string s, int startIndex, int length)
        {
            return Convert.ToInt32(s.Substring(startIndex, length));
        }

        public static bool AreDigitsInAscendingOrder(this int n)
        {
            // Returns true if each digit is larger than the previous digit, reading left to right. 
            // E.g. n = 12345, return true.

            List<int> list = n.ToListOfDigits();

            if (list.Count == 1)
                return true;

            for (int i = 1; i < list.Count; i++)
            {
                if (list[i] <= list[i - 1])
                    return false;
            }

            return true;
        }

        public static bool AreDigitsInDescendingOrder(this int n)
        {
            // E.g. n = 54321, return true.

            List<int> list = n.ToListOfDigits();

            if (list.Count == 1)
                return true;

            for (int i = 1; i < list.Count; i++)
            {
                if (list[i] >= list[i - 1])
                    return false;
            }

            return true;
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

        public static bool Identical(this List<int> intList)
        {
            if (intList.Count < 1)
                return true;

            for (int i = 1; i < intList.Count; i++)
            {
                if (intList[0] != intList[i])
                    return false;
            }

            return true;
        }

        public static bool HasDupes(this List<int> intList)
        {
            if (intList.Count <= 1)
                return false;

            List<int> list = [];

            foreach (int i in intList)
            {
                if (list.Contains(i))
                {
                    return true;
                }
                else
                {
                    list.Add(i);
                }
            }

            return false;
        }

        public static List<int[]> ToIntListArray(this string val, char rowDelimiter, char colDelimiter)
        {
            List<int[]> data = [];

            string[] lines = val.Split(rowDelimiter);

            //Console.WriteLine("lines: {0}", lines);
            for (int i = 1; i <= lines.Length; i++)
            {
                string[] fields = lines[i - 1].Split(colDelimiter);

                int[] d = new int[fields.Length];

                for (int j = 0; j < fields.Length; j++)
                {
                    try
                    {
                        d[j] = Convert.ToInt32(fields[j].Trim());
                    }
                    catch
                    {
                        throw new InvalidDataException(String.Format("Line {0} is in incorrect format.", i));
                    }
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
                    try
                    {
                        d[j] = Convert.ToDouble(fields[j].Trim());
                    }
                    catch
                    {
                        throw new InvalidDataException(String.Format("Line {0} is in incorrect format.", i));
                    }
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

        #region Trig methods 

        public static double ToRadians(this double degrees)
        {
            return ((Math.PI / 180d) * degrees);
        }

        public static decimal ToRadians(this decimal degrees)
        {
            return (((decimal)Math.PI / 180m) * degrees);
        }

        public static double ToDegrees(this double radians)
        {
            return ((180d / Math.PI) * radians);
        }

        public static decimal ToDegrees(this decimal radians)
        {
            return ((180m / (decimal)Math.PI) * radians);
        }

        public static decimal Sin(this decimal degrees)
        {
            return (decimal)Math.Sin((double)degrees.ToRadians());
        }

        public static decimal InverseSin(this decimal sin)
        {
            return (decimal)Math.Asin((double)sin).ToDegrees();
        }

        public static decimal Cos(this decimal degrees)
        {
            return (decimal)Math.Cos((double)degrees.ToRadians());
        }

        public static decimal InverseCos(this decimal cos)
        {
            return (decimal)Math.Acos((double)cos).ToDegrees();
        }

        public static decimal Tan(this decimal degrees)
        {
            return (decimal)Math.Tan((double)degrees.ToRadians());
        }

        public static decimal InverseTan(this decimal tan)
        {
            return (decimal)Math.Atan((double)tan).ToDegrees();
        }

        #endregion
    }
}
