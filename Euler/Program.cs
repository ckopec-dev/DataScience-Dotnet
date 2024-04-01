using Core;
using System.Numerics;
using System.Reflection;

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
                    case "/problem7": Problem7(); break;
                    case "/problem8": Problem8(); break;
                    case "/problem9": Problem9(); break;
                    case "/problem10": Problem10(); break;
                    case "/problem11": Problem11(); break;
                    case "/problem12": Problem12(); break;
                    case "/problem13": Problem13(); break;
                    case "/problem14": Problem14(); break;
                    case "/problem15": Problem15(); break;
                    case "/problem16": Problem16(); break;
                    case "/problem17": Problem17(); break;
                    case "/problem18": Problem18(); break;
                    case "/problem19": Problem19(); break;
                    case "/misc1": Misc1(); break;
                    case "/misc2": Misc2(); break;
                    case "/misc3": Misc3(); break;
                    case "/misc4": Misc4(); break;
                    case "/misc5": Misc5(); break;
                    case "/misc6": Misc6(); break;
                    case "/misc7": Misc7(); break;
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

        static void Problem7()
        {
            // By listing the first six prime numbers: 2,3,5,7,11 and 13, we can see that the 6th prime is 13.
            // What is the 10001st prime number?

            int x = 0;
            int n = 1;

            while (x < 10001)
            {
                n++;
                if (Core.MathHelper.IsPrime(n))
                    x++;
            }

            Console.WriteLine(n);
        }

        static void Problem8()
        {
            // The four adjacent digits in the 1000 digit number n that have the greatest product are 9 x 9 x 8 x 9 = 5832.
            // Find the thirteen adjacent digits in the 1000 digit number that have the greatest product. What is the value of this product?

            string n = "731671765313306249192251196744265747423553491949349698352031277450632623957831801698480186947885184385861560789112949495459501737958331952853208805511" +
                        "125406987471585238630507156932909632952274430435576689664895044524452316173185640309871112172238311362229893423380308135336276614282806444486645238749" +
                        "303589072962904915604407723907138105158593079608667017242712188399879790879227492190169972088809377665727333001053367881220235421809751254540594752243" +
                        "525849077116705560136048395864467063244157221553975369781797784617406495514929086256932197846862248283972241375657056057490261407972968652414535100474" +
                        "821663704844031998900088952434506585412275886668811642717147992444292823086346567481391912316282458617866458359124566529476545682848912883142607690042" +
                        "242190226710556263211111093705442175069416589604080719840385096245544436298123098787992724428490918884580156166097919133875499200524063689912560717606" +
                        "0588611646710940507754100225698315520005593572972571636269561882670428252483600823257530420752963450";

            int limit = 13;

            long highest = 0;

            for (int i = 0; i <= n.Length - limit; i++)
            {
                string subn = n.Substring(i, limit);

                long product = 1;
                for (int j = 0; j < limit; j++)
                {
                    product *= Convert.ToInt32(subn.Substring(j, 1));
                    if (product > highest)
                        highest = product;
                }
            }

            Console.WriteLine(highest);
        }

        static void Problem9()
        {
            // A Pythagorean triplet is a set of three natural numbers, a < b < c, for which, a^2 + b^2 = c^2.
            // There exists exactly one Pythagorean triplet for which a + b + c = 1000. Find the product a * b * c.

            for (long a = 1; a < 1000; a++)
            {
                for (long b = 1; b < 1000; b++)
                {
                    for (long c = 1; c < 1000; c++)
                    {
                        if (a * a + b * b == c * c && a + b + c == 1000)
                        {
                            Console.WriteLine(a * b * c);
                            return;
                        }
                    }
                }
            }
        }

        static void Problem10()
        {
            // The sum of the primes below 10 is 2 + 3 + 5 + 7 = 17.
            // Find the sum of all the primes below two million.

            long sum = 0;

            for(int i = 2; i < 2000000; i++)
            {
                if (i.IsPrime())
                    sum += i;
            }

            Console.WriteLine(sum);
        }

        static void Problem11()
        {
            string n = 
                
                "08 02 22 97 38 15 00 40 00 75 04 05 07 78 52 12 50 77 91 08\r\n" +
                "49 49 99 40 17 81 18 57 60 87 17 40 98 43 69 48 04 56 62 00\r\n" +
                "81 49 31 73 55 79 14 29 93 71 40 67 53 88 30 03 49 13 36 65\r\n" +
                "52 70 95 23 04 60 11 42 69 24 68 56 01 32 56 71 37 02 36 91\r\n" +
                "22 31 16 71 51 67 63 89 41 92 36 54 22 40 40 28 66 33 13 80\r\n" +
                "24 47 32 60 99 03 45 02 44 75 33 53 78 36 84 20 35 17 12 50\r\n" +
                "32 98 81 28 64 23 67 10 26 38 40 67 59 54 70 66 18 38 64 70\r\n" +
                "67 26 20 68 02 62 12 20 95 63 94 39 63 08 40 91 66 49 94 21\r\n" +
                "24 55 58 05 66 73 99 26 97 17 78 78 96 83 14 88 34 89 63 72\r\n" +
                "21 36 23 09 75 00 76 44 20 45 35 14 00 61 33 97 34 31 33 95\r\n" +
                "78 17 53 28 22 75 31 67 15 94 03 80 04 62 16 14 09 53 56 92\r\n" +
                "16 39 05 42 96 35 31 47 55 58 88 24 00 17 54 24 36 29 85 57\r\n" +
                "86 56 00 48 35 71 89 07 05 44 44 37 44 60 21 58 51 54 17 58\r\n" +
                "19 80 81 68 05 94 47 69 28 73 92 13 86 52 17 77 04 89 55 40\r\n" +
                "04 52 08 83 97 35 99 16 07 97 57 32 16 26 26 79 33 27 98 66\r\n" +
                "88 36 68 87 57 62 20 72 03 46 33 67 46 55 12 32 63 93 53 69\r\n" +
                "04 42 16 73 38 25 39 11 24 94 72 18 08 46 29 32 40 62 76 36\r\n" +
                "20 69 36 41 72 30 23 88 34 62 99 69 82 67 59 85 74 04 36 16\r\n" +
                "20 73 35 29 78 31 90 01 74 31 49 71 48 86 81 16 23 57 05 54\r\n" +
                "01 70 54 71 83 51 54 69 16 92 33 48 61 43 52 01 89 19 67 48";

            string[] data = n.Split("\r\n");

            // Load data into grid.
            int[,] g = new int[20, 20];
            for (int y = 0; y < data.Length; y++)
            {
                string row = data[y];
                string[] cols = row.Split(' ');

                for (int x = 0; x < data.Length; x++)
                {
                    g[x, y] = Convert.ToInt32(cols[x]);
                }
            }

            // What is the greatest product of four adjacent numbers in the same direction(up, down, left, right, or diagonally) in the 20×20 grid ?

            int gp = 0;
            int gp_x = 0;
            int gp_y = 0;
            int dir = 0;

            // Up/Down
            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y <= 16; y++)
                {
                    int p = g[x, y] * g[x, y + 1] * g[x, y + 2] * g[x, y + 3];
                    if (p > gp)
                    {
                        gp = p;
                        gp_x = x;
                        gp_y = y;
                        dir = 0;
                    }
                }
            }

            // Left/Right
            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x <= 16; x++)
                {
                    int p = g[x, y] * g[x + 1, y] * g[x + 2, y] * g[x + 3, y];
                    if (p > gp)
                    {
                        gp = p;
                        gp_x = x;
                        gp_y = y;
                        dir = 1;
                    }
                }
            }

            // Diagonally (upper left to lower right)
            for (int x = 0; x <= 16; x++)
            {
                for (int y = 0; y <= 16; y++)
                {
                    int p = g[x, y] * g[x + 1, y + 1] * g[x + 2, y + 2] * g[x + 3, y + 3];
                    if (p > gp)
                    {
                        gp = p;
                        gp_x = x;
                        gp_y = y;
                        dir = 2;
                    }
                }
            }

            // Lower left to upper right
            for (int x = 3; x < 20; x++)
            {
                for (int y = 0; y <= 16; y++)
                {
                    int p = g[x, y] * g[x - 1, y + 1] * g[x - 2, y + 2] * g[x - 3, y + 3];
                    if (p > gp)
                    {
                        gp = p;
                        gp_x = x;
                        gp_y = y;
                        dir = 3;
                    }
                }
            }

            Console.WriteLine("{0},{1},dir: {2}", gp_x, gp_y, dir);
            Console.WriteLine(gp);
        }

        static void Problem12()
        {
            // Takes quite a while to brute force. Would be a good candidate for optimization.

            long div_count = 0;
            long n = 0;
            long sum = 0;
            long max_div_count = 0;

            while (div_count <= 500)
            {
                n++;

                sum = 0;

                for(int i = 1; i <= n; i++)
                {
                    sum += i;
                }

                div_count = sum.ProperDivisors().Count + 1;

                if (div_count > max_div_count)
                {
                    max_div_count = div_count;
                    Console.WriteLine("New max div count: {0}", div_count);
                }

                //Console.WriteLine("n: {0}, sum: {1}, div_count: {2}", n, sum, div_count);
            }

            Console.WriteLine(sum);
        }

        static void Problem13()
        {
            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Euler.Inputs.Problem13.txt") ?? throw new Exception("Resource not found: Problem13.txt");
            using StreamReader sr = new(mrs);

            BigInteger sum = BigInteger.Zero;
            while (!sr.EndOfStream)
            {
                string? line = sr.ReadLine();
                if (line == null) break;
                BigInteger bi = BigInteger.Parse(line);
                sum += bi;
            }

            Console.WriteLine(sum.ToString()[..10]);
        }

        static void Problem14()
        {
            long max_n = 0;
            long max_iter = 0;

            for(long i = 1; i < 1000000; i++)
            {
                long n = i;
                long iter = 0;

                while (n > 1)
                {
                    iter++;
                    
                    if (n % 2 == 0)
                        n /= 2;
                    else
                        n = 3 * n + 1;

                    if (iter > max_iter)
                    {
                        max_n = i;
                        max_iter = iter;
                    }
                }
            }

            Console.WriteLine("{0}: {1}", max_n, max_iter);
        }

        static void Problem15()
        {
            throw new NotImplementedException();
        }

        static void Problem16()
        {
            throw new NotImplementedException();
        }

        static void Problem17()
        {
            throw new NotImplementedException();
        }

        static void Problem18()
        {
            throw new NotImplementedException();
        }

        static void Problem19()
        {
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

        static void Misc7()
        {
            // See https://arxiv.org/pdf/2403.08306.pdf

            for(long n = 1; n <= 10000; n++)
            {
                long n1 = n * n;
                long n2 = (n + 1) * (n + 1);

                long primes = 0;

                for(long i = n1 + 1; i < n2; i++)
                {
                    if (i.IsPrime())
                        primes++;
                }

                Console.WriteLine("n:{0}, n1: {1}, n2: {2}, primes between: {3}", n, n1, n2, primes);
            }
        }

        #endregion

        #region Helpers

        #endregion
    }
}