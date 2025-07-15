using System.Drawing;
using System.Numerics;
using System.Xml.XPath;

namespace Core
{
    public static class MathHelper
    {
        public const double GAMMA = 0.57721566490153286060651209008240243;

        public static ulong  CoinPartitions(int[] coinValues, int sum)
        {
            long n = coinValues.Length;
            ulong[] m = new ulong[sum + 1];
            m[0] = 1;
            for (int i = 0; i < n; i++)
                for (int j = coinValues[i]; j <= sum; j++)
                    m[j] += m[j - coinValues[i]];

            return m[sum];
        }

        public static bool IsConcatSquare(ulong a, ulong b)
        {
            // Given positive integers a and b, the concatenation ab we call
            // a ConcatSquare number if ab = (a + b)^2.
            // Example: 2025 = (20 + 25)^2.

            if (a == 0 || b == 0) return false;

            ulong ab = Convert.ToUInt64(a.ToString() + b.ToString());

            if (ab == (a + b) * (a + b))
                return true;
            else
                return false;
        }

        public static BigInteger RSA129
        {
            get { return BigInteger.Parse("114381625757888867669235779976146612010218296721242362562561842935706935245733897830597123563958705058989075147599290026879543541"); }
        } 

        public static bool IsPrime(long n)
        {
            if (n < 2) return false;
            if (n == 2) return true;
            if (n % 2 == 0) return false;

            for (int i = 3; i * i <= n; i += 2)
            {
                if (n % i == 0)
                    return false;
            }

            return true;
        }

        public static List<long> ProperDivisors(long n)
        {
            List<long> d = [];

            if (n == 0)
                return d;

            d.Add(1);

            if (n < 2)
            {
                return d;
            }
            else
            {
                for (int i = 2; i < n; i++)
                {
                    if (n % i == 0)
                        d.Add(i);
                }
            }

            return d;
        }

        public static long Fibonacci(int n)
        {
            // 0,1,1,2,3,5,8,13,21,34

            if (n < 0) throw new Exception("n must be >= 0.");
            if (n == 0) return 0;
            if (n == 1) return 1;

            long n_minus1 = 1;
            long n_minus2 = 0;

            long output = 0;

            for (int i = 2; i <= n; i++)
            {
                output = n_minus1 + n_minus2;

                n_minus2 = n_minus1;
                n_minus1 = output;
            }

            return output;
        }

        public static long FibonacciRecursive(int n)
        {
            if (n < 0) throw new Exception("n must be >= 0.");
            if (n == 0) return 0;
            if (n == 1) return 1;

            // f(n) = f(n-1) + f(n-2)
            // e.g. f(2) = f(1) + f(0) = 1

            return FibonacciRecursive(n - 1) + FibonacciRecursive(n - 2);
        }

        public static List<long> PrimeFactors(long n)
        {
            var factors = new List<long>();

            for (int divisor = 2; divisor <= n; divisor++)
                while (n % divisor == 0)
                {
                    factors.Add(divisor);
                    n /= divisor;
                }

            return factors;
        }

        public static bool Is3x3MagicSquare(long[,] square)
        {
            // A magic square is a 3x3 grid of integers such that:
            //      Every column, every row, and the two diagonals add up to the same number.
            //      No number is repeated.

            if (square == null) return false;
            if (square.GetLength(0) != 3) return false;
            if (square.GetLength(1) != 3) return false;

            // Columns
            long sum = square[0, 0] + square[0, 1] + square[0, 2];
            if (square[1, 0] + square[1, 1] + square[1, 2] != sum) return false;
            if (square[2, 0] + square[2, 1] + square[2, 2] != sum) return false;

            // Rows
            if (square[0, 0] + square[1, 0] + square[2, 0] != sum) return false;
            if (square[0, 1] + square[1, 1] + square[2, 1] != sum) return false;
            if (square[0, 2] + square[1, 2] + square[2, 2] != sum) return false;

            // Diagonals
            if (square[0, 0] + square[1, 1] + square[2, 2] != sum) return false;
            if (square[2, 0] + square[1, 1] + square[0, 2] != sum) return false;

            // Require unique values
            List<long> values = GetValues(square);
            if (values.Contains(square[0, 1])) return false; else values.Add(square[0, 1]);
            if (values.Contains(square[0, 2])) return false; else values.Add(square[0, 2]);

            if (values.Contains(square[1, 0])) return false; else values.Add(square[1, 0]);
            if (values.Contains(square[1, 1])) return false; else values.Add(square[1, 1]);
            if (values.Contains(square[1, 2])) return false; else values.Add(square[1, 2]);

            if (values.Contains(square[2, 0])) return false; else values.Add(square[2, 0]);
            if (values.Contains(square[2, 1])) return false; else values.Add(square[2, 1]);
            if (values.Contains(square[2, 2])) return false; else values.Add(square[2, 2]);

            return true;
        }

        private static List<long> GetValues(long[,] square)
        {
            return
            [
                square[0, 0]
            ];
        }

        public static bool AreFriendly(long m, long n)
        {
            // Two numbers are considered "friendly" if each is equal to the sum of the other's proper divisors.

            if (ProperDivisors(n).Sum() == m &&
                ProperDivisors(m).Sum() == n)
                return true;
            else
                return false;
        }

        public static bool IsPerfect(long n)
        {
            // A perfect number is a number that is the sum of it's proper divisors.

            if (n < 2)
                return false;

            return n == ProperDivisors(n).Sum();
        }

        public static uint GCD(uint a, uint b)
        {
            if (a == 0)
                return b;
            return GCD(b % a, a);
        }

        public static ulong GCD(ulong a, ulong b)
        {
            if (a == 0)
                return b;
            return GCD(b % a, a);
        }

        public static double Li(double x)
        {
            return Li(x, 10);
        }

        public static double Li(double x, int terms)
        {
            // https://en.wikipedia.org/wiki/Logarithmic_integral_function
            // https://math.stackexchange.com/questions/700391/integration-by-parts-of-the-logarithmic-integral

            double result = GAMMA;
            double logX = Math.Log(x);
            //Console.WriteLine("logX: {0}", logX);
            result += Math.Log(logX);
            //Console.WriteLine("result: {0}", result);

            for (int n = 1; n <= terms; n++)
            {
                double nFactorial = Convert.ToDouble(n.Factorial().ToString());
                //Console.WriteLine("nFactorial({0}): {1}", n, nFactorial);

                double pow = Math.Pow(logX, n);
                //Console.WriteLine("pow({0}): {1}", n, pow);

                double nn = n * nFactorial;
                //Console.WriteLine("nn({0}): {1}", n, nn);

                result += pow / nn;

                //Console.WriteLine("result({0}): {1}", n, result);
            }

            return result;
        }

        public static long Pi(long x)
        {
            // The Prime Counting Function
            // The count of all primes up to (and including) x.
            // E.g. Pi(1000) returns 168.

            long count = 0;

            for(long i = 0; i <= x; i++)
            {
                if (i.IsPrime())
                    count++;
            }

            return count;
        }

        public static int PiProbably(int x)
        {
            int count = 0;

            for (int i = 0; i <= x; i++)
            {
                if (i.IsProbablyPrime())
                    count++;
            }

            return count;
        }
    }
}
