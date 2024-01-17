using Core;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;

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
                    case "/problem5": Problem5(); break;
                    case "/problem6": Problem6(); break;
                    case "/misc1": Misc1(); break;
                    case "/misc2": Misc2(); break;
                    case "/misc3": Misc3(); break;
                    case "/misc4": Misc4(); break;
                    case "/misc5": Misc5(); break;
                    case "/misc6": Misc6(); break;
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

            List<int> palindromes = new();

            for (int x = 999; x > 99; x--)
            {
                for (int y = 999; y > 99; y--)
                {
                    int z = x * y;
                    if (z.IsPalindrome())
                    {
                        palindromes.Add(z);
                    }
                }
            }

            palindromes.Sort();

            Console.WriteLine(palindromes[^1]);
        }

        static void Problem5()
        {
            // 2520 is the smallest number that can be divided by each of the numbers from 1 to 10 without any remainder.
            // What is the smallest positive number that is evenly divisible by all of the numbers from 1 to 20?

            int limit = 20;
            int result = 0;
            int x = 0;

            while (result == 0)
            {
                x++;

                bool ok = true;

                for (int i = 2; i <= limit; i++)
                {
                    if (x % i != 0)
                        ok = false;
                }

                if (ok)
                {
                    result = x;
                }
            }

            Console.WriteLine(result);
        }

        static void Problem6()
        {
            // Find the difference between the sum of the squares of the first one hundred natural numbers and the square of the sum.

            BigInteger sumOfSquares = BigInteger.Zero;
            BigInteger squareOfSums = BigInteger.Zero;

            for(int i = 1; i <= 100; i++)
            {
                sumOfSquares += i * i;
                squareOfSums += i;
            }

            squareOfSums *= squareOfSums;

            Console.WriteLine(squareOfSums - sumOfSquares);
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

        static void Misc4()
        {
            // Show convergence of pi via infinite series.

            // pi/4 = 1 - 1/3 + 1/5 - 1/7 + 1/9...

            const int iterations = 1000;

            double[] dataX = new double[iterations];
            double[] dataY = new double[iterations];

            double currentPi = 1;
            bool currentPos = false;
            double currentDenom = 1;

            for (long n = 1; n <= iterations; n++)
            {
                dataX[n - 1] = n;

                currentDenom += 2d;

                if (currentPos)
                {
                    currentPi += (1d / currentDenom);
                    currentPos = false;
                }
                else
                {
                    currentPi -= (1d / currentDenom);
                    currentPos = true;
                }

                Console.WriteLine("d: {0}, pi: {1}, actual: {2}", currentDenom, currentPi, currentPi * 4);
                dataY[n - 1] = 4 * currentPi;
            }

            var plt = new ScottPlot.Plot(1200, 800);
            plt.AddScatter(dataX, dataY, null, 1, 5, ScottPlot.MarkerShape.filledCircle, ScottPlot.LineStyle.None, null);
            new ScottPlot.FormsPlotViewer(plt).ShowDialog();
        }

        static void Misc5()
        {
            // Find integer solutions to a^3 + b^3 = 22c^3
            // Known solution is: a=17299,b=25469,c=9954. Is this the first?

            for (BigInteger a = new(1); a < 20000; a++)
            {
                for(BigInteger b = new(1); b < 30000; b++)
                {
                    Console.WriteLine("Check a: {0}, b: {1}", a, b);

                    for (BigInteger c = new(1); c < 10000; c++)
                    {
                        if (a * a * a + b * b * b == 22 * c * c * c)
                        {
                            Console.WriteLine("Solution found: {0}, {1}, {2}", a, b, c);
                            return;
                        }
                    }
                }
            }
        }

        static void Misc6()
        {
            // Ramsey theory: https://en.wikipedia.org/wiki/Ramsey%27s_theorem#R(3,_3)_=_6
            // https://en.wikipedia.org/wiki/Theorem_on_friends_and_strangers
            // Suppose you are at a party. How many people need to be present such that you are guaranteed that at least three of them are(pairwise) mutual strangers or at least three of them are(pairwise) mutual friends?

            // Proposed preliminary algorithm...
            // Create all possible unique pairwise combinations of 6 people.
            // Randomly assign a relationship (known or strangers) to each pair. 
            // Iterate

            Random rnd = new();
            const int iterations = 100;
            const int people = 6;
            
            for(int iteration = 0; iteration < iterations; iteration++)
            {
                int strangers = 0;
                int friends = 0;

                for (int i = 1; i <= people; i++)
                {
                    for (int j = i + 1; j <= people; j++)
                    {
                        if (rnd.Next(2) == 0)
                        {
                            friends++;
                        }
                        else
                        {
                            strangers++;
                        }
                    }
                }

                Console.WriteLine("Pairs: {0}, Strangers: {1}, Friends: {2}", friends + strangers, strangers, friends);
            }
        }

        #endregion

        #region Helpers

        #endregion
    }
}