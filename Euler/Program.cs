using Core;

namespace Euler
{
    /// <summary>
    /// https://projecteuler.net/archives
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            string switchErr = "Switch missing or invalid.";

            if (args != null && args.Length == 1)
            {
                switch (args[0].ToLower())
                {
                    case "/problem1": Problem1(); break;
                    case "/problem2": Problem2(); break;
                    case "/problem3": Problem3(); break;
                    case "/problem4": Problem4(); break;
                    case "/misc1": Misc1(); break;
                    case "/misc2": Misc2(); break;
                    case "/misc3": Misc3(); break;
                    default: Console.WriteLine(switchErr); break;
                }
            }
            else
            {
                Console.WriteLine(switchErr);
            }
        }

        #region Problems

        static void Problem1()
        {
            // If we list all the natural numbers below 10 that are multiples of 3 or 5, we get 3, 5, 6 and 9.
            // The sum of these multiples is 23.

            // Find the sum of all the multiples of 3 or 5 below 1000.

            int sum = 0;

            for (int i = 3; i < 1000; i++)
            {
                if (i % 3 == 0 || i % 5 == 0)
                    sum += i;
            }

            Console.WriteLine(sum);
        }

        static void Problem2()
        {
            // By considering the terms in the Fibonacci sequence whose values do not exceed four million,
            // find the sum of the even-valued terms.

            long sum = 0;
            int n = 1;
            long term = 0;

            while (term < 4000000)
            {
                term = MathHelper.Fibonacci(n);

                if (term % 2 == 0)
                    sum += term;

                n++;
            }

            Console.WriteLine(sum);
        }

        static void Problem3()
        {
            // The prime factors of 13195 are 5, 7, 13 and 29.

            // What is the largest prime factor of the number 600851475143 ?

            var factors = MathHelper.PrimeFactors(600851475143);

            Console.WriteLine(factors.Last());
        }

        static void Problem4()
        {
            // A palindromic number reads the same both ways. The largest palindrome made from the product of two 
            // 2-digit numbers is 9009 = 91 x 99.

            // Find the largest palindrome made from the product of two 
            // 3-digit numbers.

            throw new NotImplementedException();
        }

        #endregion

        #region Misc experiments

        static void Misc1()
        {
            // Search for friendly numbers
            
            for(long m = 2; m <= 10000; m++)
            {
                for(long n = 3; n <= 10000; n++)
                {
                    if (m == n || m > n)
                        continue;

                    if (MathHelper.AreFriendly(m, n))
                    {
                        Console.WriteLine("AreFriendly: {0}, {1}", m, n);
                    }
                }
            }
        }

        static void Misc2()
        {
            for(long n = 1; n <= 10; n++)
            {
                Console.WriteLine("{0} is perfect: {1}", n, MathHelper.IsPerfect(n));
            }

        }

        static void Misc3()
        {
            // Iterate through square values and check solutions. Nine nested loops...

            long limit = 5; // iterations = limit^9
            long found = 0;
            long iterations = 0;

            for (long i0 = 1; i0 <= limit; i0++)
            {
                Console.WriteLine($"Processing outer loop: {i0}");
                for (long i1 = 1; i1 <= limit; i1++)
                    for (long i2 = 1; i2 <= limit; i2++)
                        for (long i3 = 1; i3 <= limit; i3++)
                            for (long i4 = 1; i4 <= limit; i4++)
                                for (long i5 = 1; i5 <= limit; i5++)
                                    for (long i6 = 1; i6 <= limit; i6++)
                                        for (long i7 = 1; i7 <= limit; i7++)
                                            for (long i8 = 1; i8 <= limit; i8++)
                                            {
                                                iterations++;

                                                long[,] a = new long[3, 3]
                                                {
                                                    {i0 * i0, i1 * i1, i2 * i2 },
                                                    {i3 * i3, i4 * i4, i5 * i5 },
                                                    {i6 * i6, i7 * i7, i8 * i8 }
                                                };

                                                if (MathHelper.Is3x3MagicSquare(a))
                                                {
                                                    Console.WriteLine($"Magic square of squares found: {i0}, {i1}, {i2}, {i3}, {i4}, {i5}, {i6}, {i7}, {i8}");
                                                    found++;
                                                }
                                            }
            }

            Console.WriteLine($"Total iterations: {iterations}");
            Console.WriteLine($"Total found: {found}");
        }

        #endregion

        #region Helpers

        #endregion
    }
}