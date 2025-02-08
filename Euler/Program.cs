using Combinatorics.Collections;
using ConsoleTables;
using Core;
using Core.GameTheory;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Text;

namespace Euler
{
    /// <summary>
    /// https://projecteuler.net/archives
    /// Collapse all: CTRL + M + O
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            string switchErr = "Switch missing or invalid.";

            if (args != null && args.Length == 1)
            {
                CallMethod(args[0]);
            }
            else
            {
                Console.WriteLine(switchErr);
            }
        }

        static void CallMethod(string cmd)
        {
            string arg0 = cmd.ToLower();
            string? prefix = null, suffix = null;

            if (arg0.Contains("problem", StringComparison.CurrentCultureIgnoreCase))
            {
                prefix = "Problem";
                suffix = arg0.Split("problem")[1];
            }
            else if (arg0.Contains("misc"))
            {
                prefix = "Misc";
                suffix = arg0.Split("misc")[1];
            }

            if (prefix == null || suffix == null)
            {
                throw new Exception("Invalid command: " + cmd);
            }

            Stopwatch watch = Stopwatch.StartNew();

            string methodName = prefix + suffix;
            Console.WriteLine("Invoking {0}.", methodName);
            MethodInfo? method = typeof(Program).GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic) ?? throw new Exception("Method not found: " + methodName);
            method?.Invoke(null, null);
            
            watch.Stop();
            Console.WriteLine("Invocation completed in {0}.", watch.Elapsed.ToFriendlyDuration(2));
        }

        #pragma warning disable IDE0051 // Remove unused private members

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

            List<int> palindromes = [];

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
            const int gridSize = 20;
            BigInteger r = Statistics.BinomialCoefficient(gridSize + gridSize, gridSize);
            Console.WriteLine(r);
        }

        static void Problem16()
        {
            BigInteger n = new(2);
            const int power = 1000;
            n = BigInteger.Pow(n, power);

            string digits = n.ToString();
            int sum = 0;

            for (int i = 0; i < digits.Length; i++)
            {
                sum += Convert.ToInt32(digits.Substring(i, 1));
            }

            Console.WriteLine(sum);
        }

        static void Problem17()
        {
            int len = 0;

            for (int i = 1; i <= 1000; i++)
            {
                len += i.ToWords().Replace(" ", "").Length;
                Console.WriteLine(i.ToWords());
            }

            Console.WriteLine(len);
        }

        static void Problem18()
        {
            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Euler.Inputs.Problem18.txt") ?? throw new Exception("Resource not found: Problem18.txt");
            using StreamReader sr = new(mrs);

            #pragma warning disable IDE0305 // Simplify collection initialization
            List<string> input = sr.ReadToEnd().ToList();
            #pragma warning restore IDE0305 // Simplify collection initialization

            // Convert it to a list of int arrays.
            List<int[]> data = [];
            for (int i = 0; i < input.Count; i++)
            {
                int[] d = input[i].ToIntArray(' ');
                data.Add(d);
            }

            // Start at the bottom and work up. 
            for (int i = data.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    Console.WriteLine(data[0][0]);
                    return;
                }

                int idxNextRow = i - 1;

                for (int j = 0; j < data[i - 1].Length; j++)
                {
                    if (data[i][j] > data[i][j + 1])
                    {
                        data[i - 1][j] += data[i][j];
                    }
                    else
                    {
                        data[i - 1][j] += data[i][j + 1];
                    }
                }
            }
        }

        static void Problem19()
        {
            int totalSundays = 0;

            DateTime dt = new(1901, 1, 1);

            while (dt <= new DateTime(2000, 12, 31))
            {
                if (dt.DayOfWeek == DayOfWeek.Sunday)
                    totalSundays++;

                dt = dt.AddMonths(1);
            }

            Console.WriteLine(totalSundays);
        }
        
        static void Problem20()
        {
            BigInteger bi = BigInteger.One;

            for (int i = 1; i <= 100; i++)
            {
                bi *= new BigInteger(i);
            }

            Console.WriteLine(bi.SumOfDigits().ToString());
        }

        static void Problem21()
        {
            const int START = 1;
            const int STOP = 10000;

            List<int> numbers = [];

            for (int a = START; a < STOP; a++)
            {
                int da = a.ProperDivisors().Sum();
                int dofda = da.ProperDivisors().Sum();

                if (dofda == a && a != da)
                {
                    Console.WriteLine("Found pair: a: {0}, b: {1}", a, da);

                    if (!numbers.Contains(a))
                        numbers.Add(a);

                    if (da < 10000 && !numbers.Contains(da))
                        numbers.Add(da);
                }
            }

            Console.WriteLine(numbers.Sum());
        }

        static void Problem22()
        {
            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Euler.Inputs.Problem22.txt") ?? throw new Exception("Resource not found: Problem22.txt");
            using StreamReader sr = new(mrs);

            string names = sr.ReadToEnd();
            
            // Remove quotes
            names = names.Replace("\"", "");

            // Parse
            List<string> parsed = names.ToList(',');
            Console.WriteLine("Found {0} names.", parsed.Count);

            // Sort
            parsed.Sort();

            long total = 0;

            for (int i = 1; i <= parsed.Count; i++)
            {
                string name = parsed[i - 1];
                int name_sum = 0;

                char[] chars = name.ToCharArray();
                foreach(char c in chars)
                {
                    int char_val = (int)c - 64;
                    name_sum += char_val;
                }
                Console.WriteLine("{0}: position: {1}, sum: {2}, position * sum: {3}", name, i, name_sum, i * name_sum);
                
                total += i * name_sum;
            }

            Console.WriteLine(total);
        }

        static void Problem23()
        {
            // Takes a while to execute. Would be good candidate for optimization and/or multithreading.

            const int LIMIT = 28124;

            List<int> abundant_nums = [];
            for (int i = 1; i < LIMIT; i++)
            {
                if (i.ProperDivisors().Sum() > i)
                    abundant_nums.Add(i);
            }

            int[] n = [.. abundant_nums];

            int sum = 0;
            for (int i = 1; i < LIMIT; i++)
            {
                bool summable = false;
                for (int j = 0; j < n.Length; j++)
                {
                    if (summable)
                        break;

                    for (int k = 0; k < n.Length; k++)
                    {
                        if (n[j] + n[k] == i)
                        {
                            summable = true;
                            break;
                        }
                    }
                }

                if (!summable)
                {
                    sum += i;
                }
            }

            Console.WriteLine(sum);
        }

        static void Problem24()
        {
            int[] list = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];

            Permutations<int> perms = new(list, GenerateOption.WithoutRepetition);

            int i = 0;
            foreach (List<int> p in perms.Cast<List<int>>())
            {
                i++;

                if (i == 1000000)
                {
                    int[] a = [.. p];
                    String pa = String.Join("", a);

                    Console.WriteLine(pa);
                }
            }
        }

        static void Problem25()
        {
            int digit_limit = 1000;

            BigInteger term = 0;
            BigInteger previous_value = 0;
            BigInteger current_value = 1;

            while (current_value.NumberOfDigits() < digit_limit)
            {
                term++;

                BigInteger temp_value = current_value;
                current_value += previous_value;
                previous_value = temp_value;
            }

            Console.WriteLine(term + 1);
        }

        static void Problem26()
        {
            int max = 0;
            int max_d = 3;

            for (int d = 3; d < 1000; d += 2)
            {
                if (d % 5 == 0)
                    continue;
                
                int x = 10 % d;
                int y = 1;

                while (x > 1)
                {
                    y++;
                    x = (x * 10) % d;
                }

                if (y > max)
                {
                    max = y;
                    max_d = d;
                }
            }

            Console.WriteLine(max_d);
        }

        static void Problem27()
        {
            int cap = 1000;

            int maxFailA = (-1 * cap) - 1;
            int maxFailB = (-1 * cap) - 1;
            int maxFailCount = 0;

            for (int a = (-1 * cap); a < cap; a++)
            {
                for (int b = (-1 * cap); b < cap; b++)
                {
                    bool prime = true;
                    int n = 0;
                    int counter = 0;

                    while (prime)
                    {
                        counter++;

                        int c = (n * n) + (a * n) + (b);

                        if (!c.IsPrime())
                        {
                            if (counter > maxFailCount)
                            {
                                maxFailA = a;
                                maxFailB = b;
                                maxFailCount = counter;
                                Console.WriteLine("a: {0}, b:{1}, new max fail #: {2}", maxFailA, maxFailB, maxFailCount);
                            }
                            break;
                        }

                        n++;
                    }
                }
            }

            Console.WriteLine("a: {0}, b:{1}, overall max fail #: {2}", maxFailA, maxFailB, maxFailCount);
            Console.WriteLine("a * b = {0}", maxFailA * maxFailB);
        }

        static void Problem28()
        {
            const int SQUARE_WIDTH = 1001;

            // The upper right diagonal consists of odd squares. Skipping the center, sum them up e.g. 3^2, 5^2, 7^2.
            // While doing this, sum up the even numbers. e.g. 2, 4, 6

            int sum_ur = 0;
            int sum_evens = 0;
            int even = 0;

            for (int i = 3; i <= SQUARE_WIDTH; i += 2)
            {
                sum_ur += i * i;
                even += 2;
                sum_evens += even;
            }

            // The upper left diagonal sum is the upper right diagonal sum minus sum_evens.
            int sum_ul = sum_ur - sum_evens;
            // Ditto for lower left and lower right.
            int sum_ll = sum_ul - sum_evens;
            int sum_lr = sum_ll - sum_evens;

            // The sum is all the diagonals plus 1.
            int sum_ttl = sum_ur + sum_ul + sum_ll + sum_lr + 1;
            Console.WriteLine("sum_ttl: {0}", sum_ttl);
        }

        static void Problem29()
        {
            const long CAP = 100;

            List<BigInteger> results = [];

            for (BigInteger a = 2; a <= CAP; a++)
            {
                for (int b = 2; b <= CAP; b++)
                {
                    BigInteger c = BigInteger.Pow(a, b);

                    // Add unique result (ignore dupes).
                    if (!results.Contains(c))
                        results.Add(c);
                }
            }

            results.Sort();

            foreach (BigInteger i in results)
                Console.WriteLine(i);

            Console.WriteLine("Distinct terms: {0}", results.Count);
        }

        static void Problem30()
        {
            double exp = 5;
            double grand_total = 0;
            double limit = 1000000;

            for (int n = 10; n < limit; n++)
            {
                double sum = 0;

                // Raise each digit to the 4th power and add it to the sum.

                for (int i = 0; i < n.ToString().Length; i++)
                    sum += Math.Pow(Convert.ToDouble(n.ToString().Substring(i, 1)), exp);

                if (sum == n)
                {
                    Console.WriteLine("n: {0}", n);
                    grand_total += sum;
                }
            }

            Console.WriteLine("Grand total: {0}", grand_total);
        }

        static void Problem31()
        {
            int count = 1; // Only 1 way to use 200p coin.

            // Use 0 to 2 100p coins.
            for (int i = 0; i <= 2; i++)
            {
                // Use 0 to 4 50p coins.
                for (int j = 0; j <= 4; j++)
                {
                    // Use 0 to 10 20p coins.
                    for (int k = 0; k <= 10; k++)
                    {
                        Console.WriteLine("Processing i, j, k: {0}, {1}, {2}", i, j, k);

                        // Use 0 to 20 10p coins.
                        for (int m = 0; m <= 20; m++)
                        {
                            // Use 0 to 40 5p coins.
                            for (int n = 0; n <= 40; n++)
                            {
                                // Use 0 to 100 2p coins.
                                for (int p = 0; p <= 100; p++)
                                {
                                    // Use 0 to 200 1p coins.
                                    for (int q = 0; q <= 200; q++)
                                    {
                                        int sum = 0;

                                        sum += i * 100;
                                        sum += j * 50;
                                        sum += k * 20;
                                        sum += m * 10;
                                        sum += n * 5;
                                        sum += p * 2;
                                        sum += q; // * 1

                                        if (sum == 200)
                                            count++;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Total count: {0}", count);
        }

        static void Problem32()
        {
            // Find the sum of all products whose multiplicand/multiplier/product identity can be written as a 1 through 9 pandigital.

            List<long> nums = [];

            for (int a = 1; a < 10000; a++)
            {
                for (int b = 1; b < 10000; b++)
                {
                    int c = a * b;

                    string s = String.Format("{0}{1}{2}", a, b, c);

                    if (s.Length == 9)
                    {
                        long n = Convert.ToInt64(s);
                        if (n.IsPandigital(1,9))
                        {
                            Console.WriteLine("{0} x {1} = {2}", a, b, c);

                            if (!nums.Contains(c))
                                nums.Add(c);
                        }
                    }
                }
            }

            Console.WriteLine("sum: {0}", nums.Sum());
        }

        static void Problem33()
        {
            List<int> numerators = [];
            List<int> denominators = [];

            for (int i = 10; i < 100; i++)
            {
                for (int j = 10; j < 100; j++)
                {
                    // The original value.
                    decimal m = (decimal)i / (decimal)j;

                    // Do the numerator + denominator share a common digit? If so, remove it from each, then divide and see if it matches the original value.
                    List<int> common = i.CommonDigits(j);

                    if (common.Count == 1 && j.RemoveDigit(common[0]) != 0)
                    {
                        int c = common[0];
                        decimal i1 = (decimal)i.RemoveDigit(c);
                        decimal j1 = (decimal)j.RemoveDigit(c);

                        decimal n = i1 / j1;

                        if (n > 0 && n < 1 && n == m && c != 0)
                        {
                            numerators.Add(i);
                            denominators.Add(j);

                            Console.WriteLine("i: {0}, j: {1}, i1: {2}, j1: {3}, m: {4}, n: {5}, c: {6}", i, j, i1, j1, m, n, c);
                        }
                    }
                }
            }

            Console.WriteLine("Found {0} matches.", numerators.Count);

            int prodNums = numerators.Product();
            int prodDenoms = denominators.Product();

            Console.WriteLine("num: {0}, den: {1}, reduced: {2}", prodNums, prodDenoms, prodDenoms / prodNums);
        }

        static void Problem34()
        {
            const int upperBound = 2540160;

            BigInteger sum = new(0);

            for (int i = 3; i <= upperBound; i++)
            {
                if (i % 1000 == 0)
                    Console.WriteLine("Calculating i = {0}. Current sum: {1}", i, sum);

                if (i.SumOfFactorialDigits() == new BigInteger(i))
                {
                    sum += i;
                }
            }

            Console.WriteLine(sum);
        }

        static void Problem35()
        {
            const int cap = 1000000;

            int count = 0;

            for (int n = 2; n < cap; n++)
            {
                List<int> rotations = n.ToRotations();

                if (rotations.ArePrime())
                {
                    Console.WriteLine(n);
                    count++;
                }
            }

            Console.WriteLine("Count: {0}", count);
        }

        static void Problem36()
        {
            long sum = 0;

            for (int i = 1; i < 1000000; i++)
            {
                if (i.IsPalindrome())
                {
                    string base2 = Convert.ToString(i, 2);

                    if (!base2.StartsWith('0'))
                    {
                        if (base2.IsPalindrome())
                        {
                            sum += i;
                        }
                    }
                }
            }

            Console.WriteLine(sum);
        }

        static void Problem37()
        {
            List<long> tps = [];

            long num = 10;

            while (tps.Count < 11)
            {
                num++;

                if (num.IsTruncatablePrime())
                {
                    Console.WriteLine("Found truncatable prime: {0}", num);
                    tps.Add(num);
                }
            }

            Console.WriteLine(tps.Sum());
        }

        static void Problem38()
        {
            long largest = 0;

            for (int n = 2; n <= 99999; n++)
            {
                if (n % 1000 == 0)
                    Console.WriteLine("Processing n = {0}", n);

                for (int i = 2; i <= 7; i++)
                {
                    // The array is 1 to i.
                    List<int> m = [];
                    for (int j = 1; j <= i; j++)
                    {
                        m.Add(j);
                    }

                    try
                    {
                        long p = n.PandigitalMultiple(m);

                        if (p.ToString().Length != 9)
                            continue;

                        if (p.IsPandigital(1, 9))
                        {
                            if (p > largest)
                            {
                                largest = p;
                                Console.WriteLine("Current largest: {0}, n = {1}, i = {2}", p, n, i);
                            }
                        }
                    }
                    catch (OverflowException)
                    {
                        continue;
                    }
                }
            }
        }

        static void Problem39()
        {
            int max_p;
            int max_solutions = 3;          // as given in example for p = 120

            const int P_LOWER_BOUND = 3;    // 3
            const int P_UPPER_BOUND = 1000; // 1000

            for (int p = P_LOWER_BOUND; p <= P_UPPER_BOUND; p++)
            {
                int solutionCount = 0;
                List<int[]> solutions = [];

                for (int a = 1; a < p / 2; a++)
                {
                    for (int b = 1; b < p / 2; b++)
                    {
                        for (int c = 1; c < p / 2; c++)
                        {
                            if (a * a + b * b == c * c && a + b + c == p)
                            {
                                int[] candidate1 = [a, b, c];
                                int[] candidate2 = [b, a, c];

                                bool exists = false;

                                foreach (int[] sol in solutions)
                                {
                                    if (sol[0] == candidate1[0] && sol[1] == candidate1[1] && sol[2] == candidate1[2])
                                        exists = true;
                                    if (sol[0] == candidate2[0] && sol[1] == candidate2[1] && sol[2] == candidate2[2])
                                        exists = true;
                                }

                                if (!exists)
                                {
                                    solutions.Add(candidate1);
                                    solutionCount++;
                                    Console.WriteLine("New solution: p({0}) = {1},{2},{3}", p, a, b, c);
                                }
                            }
                        }
                    }
                }

                if (solutionCount > max_solutions)
                {
                    max_p = p;
                    max_solutions = solutionCount;
                    Console.WriteLine("Found new max p: {0}, solutions: {1}", max_p, max_solutions);
                }
            }
        }

        static void Problem40()
        {
            StringBuilder sb = new();

            for (int i = 1; i <= 1000000; i++)
            {
                sb.Append(i);
            }

            // d1 × d10 × d100 × d1000 × d10000 × d100000 × d1000000

            string c = sb.ToString();

            int sum = 1;

            sum *= Convert.ToInt32(c.Substring(9, 1));
            sum *= Convert.ToInt32(c.Substring(99, 1));
            sum *= Convert.ToInt32(c.Substring(999, 1));
            sum *= Convert.ToInt32(c.Substring(9999, 1));
            sum *= Convert.ToInt32(c.Substring(99999, 1));
            sum *= Convert.ToInt32(c.Substring(999999, 1));

            Console.WriteLine(sum);
        }

        static void Problem41()
        {
            Console.WriteLine("2143 is pandigital: {0}", 2143L.IsPandigital(1, 4));

            for (long i = 9876543210; i > 0; i--)
            {
                if (i % 100000 == 0)
                    Console.WriteLine("Search down to {0}.", i);

                if (i.IsPandigital(1, i.ToString().Length) && i.IsPrime())
                {
                    Console.WriteLine(i);
                    break;
                }
            }
        }

        static void Problem42()
        {
            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Euler.Inputs.Problem42.txt") ?? throw new Exception("Resource not found: Problem42.txt");
            using StreamReader sr = new(mrs);

            string words = sr.ReadToEnd();

            // Remove quotes
            words = words.Replace("\"", "");

            // Parse
            List<string> parsed = words.ToList(',');
            Console.WriteLine("Found {0} words.", parsed.Count);

            List<int> triange_nums = [];

            for (int n = 1; n < 1000; n++)
            {
                triange_nums.Add(n * (n + 1) / 2);
            }

            int ttl = 0;

            foreach (string w in parsed)
            {
                int wv = w.WordValue();

                if (triange_nums.Contains(wv))
                    ttl++;
            }

            Console.WriteLine(ttl);
        }

        static void Problem43()
        {
            long sum = 0;
            long counter = 0;

            long loVal = 1023456789;
            long hiVal = 9876543210; // performance test value: 1033456789;

            long total = hiVal - loVal;

            DateTime dtStart = DateTime.Now;

            ParallelOptions options = new()
            {
                MaxDegreeOfParallelism = 4
            };

            Parallel.For(loVal, hiVal, options, i =>
            {
                counter++;

                if (i % 1000000 == 0)
                    Console.WriteLine("Searching at {0}. Current sum: {1}. Current thread: {2}. Processing {3} of {4}. {5} remaining.", i, sum, Environment.CurrentManagedThreadId,
                        counter, total, total - counter);

                //Let d1 be the 1st digit, d2 be the 2nd digit, and so on. In this way, we note the following:

                //d2d3d4 = 406 is divisible by 2
                //d3d4d5 = 063 is divisible by 3
                //d4d5d6 = 635 is divisible by 5
                //d5d6d7 = 357 is divisible by 7
                //d6d7d8 = 572 is divisible by 11
                //d7d8d9 = 728 is divisible by 13
                //d8d9d10 = 289 is divisible by 17

                if (i.IsPandigital(0, 9))
                {
                    if
                        (Convert.ToInt32(i.ToInt32(1, 1).ToString() + i.ToInt32(2, 1) + i.ToInt32(3, 1)) % 2 == 0 &&
                        Convert.ToInt32(i.ToInt32(2, 1).ToString() + i.ToInt32(3, 1) + i.ToInt32(4, 1)) % 3 == 0 &&
                        Convert.ToInt32(i.ToInt32(3, 1).ToString() + i.ToInt32(4, 1) + i.ToInt32(5, 1)) % 5 == 0 &&
                        Convert.ToInt32(i.ToInt32(4, 1).ToString() + i.ToInt32(5, 1) + i.ToInt32(6, 1)) % 7 == 0 &&
                        Convert.ToInt32(i.ToInt32(5, 1).ToString() + i.ToInt32(6, 1) + i.ToInt32(7, 1)) % 11 == 0 &&
                        Convert.ToInt32(i.ToInt32(6, 1).ToString() + i.ToInt32(7, 1) + i.ToInt32(8, 1)) % 13 == 0 &&
                        Convert.ToInt32(i.ToInt32(7, 1).ToString() + i.ToInt32(8, 1) + i.ToInt32(9, 1)) % 17 == 0
                    )
                    {
                        sum += i;
                        Console.WriteLine("Found solution: {0}. Current sum: {1}. Current thread: {2}.", i, sum, Environment.CurrentManagedThreadId);
                    }
                }
            });

            TimeSpan duration = DateTime.Now - dtStart;
            Console.WriteLine("Execution completed in {0} seconds.", duration.TotalSeconds);

            Console.WriteLine("Final sum: {0}.", sum);
        }

        static void Problem44()
        {
            ParallelOptions options = new()
            {
                MaxDegreeOfParallelism = 4
            };
            long counter = 0;

            List<long> pent_nums = [];
            for (long n = 1; n < 2500; n++)
            {
                pent_nums.Add(n * (3 * n - 1) / 2);
            }

            long lowestD = long.MaxValue;

            Parallel.For(0, pent_nums.Count, options, j =>
            {
                counter++;

                Console.WriteLine("Processing {0} of {1}.", counter, pent_nums.Count + 1);

                for (int k = 0; k < pent_nums.Count; k++)
                {
                    long s = pent_nums[j] + pent_nums[k];
                    long d = pent_nums[k] - pent_nums[j];

                    if (pent_nums.Contains(s) && pent_nums.Contains(d))
                    {
                        long D = pent_nums[k] - pent_nums[j];
                        if (D < 0)
                            D *= -1;

                        if (D < lowestD)
                            lowestD = D;
                    }
                }
            });

            Console.WriteLine(lowestD);
        }

        static void Problem45()
        {
            // Triangle Tn = n(n + 1) / 2     1, 3, 6, 10, 15, ...
            // Pentagonal Pn = n(3n−1) / 2    1, 5, 12, 22, 35, ...
            // Hexagonal Hn = n(2n−1)        1, 6, 15, 28, 45, ...
            
            List<long> t_nums = [];
            List<long> p_nums = [];
            List<long> h_nums = [];

            for (int n = 1; n <= 1000000; n++)
            {
                t_nums.Add(n * ((long)n + 1) / 2);
                p_nums.Add(n * (3 * (long)n - 1) / 2);
                h_nums.Add(n * (2 * (long)n - 1));
            }

            for (int n = 286; n < 1000000; n++)
            {
                if (n % 10000 == 0)
                    Console.WriteLine("Processing batch {0}.", n);

                if (p_nums.Contains(t_nums[n]) && h_nums.Contains(t_nums[n]))
                {
                    Console.WriteLine("n: {0}, Tn: {1}", n + 1, t_nums[n]);
                    break;
                }
            }
        }

        static void Problem46()
        {
            long n = 9;

            while (true)
            {
                if (n % 1000 == 0)
                    Console.WriteLine("Searching at n = {0}.", n);

                // Only consider composites.
                if (n.IsPrime())
                {
                    n += 2;
                    continue;
                }

                bool found = false;

                // For each prime less than n.
                for (long p = n - 2; p > 1; p--)
                {
                    if (p.IsPrime())
                    {
                        long q = n - p;

                        // Is q twice a square?
                        q /= 2;

                        // Now is q a square?
                        double r = Math.Sqrt(q);

                        if (r % 1 == 0)
                        {
                            found = true;
                            break;
                        }
                    }
                }

                if (!found)
                {
                    Console.WriteLine("n: {0}", n);
                    break;
                }

                n += 2;

                if (n > 1000000)
                {
                    Console.WriteLine("Overflow condition reached. Halting.");
                    break;
                }
            }
        }

        static void Problem47()
        {
            for (long p1 = 2; p1 < 1000000; p1++)
            {
                if (p1 % 1000 == 0)
                    Console.WriteLine("Searching p1 = {0}.", p1);

                long p2 = p1 + 1;
                long p3 = p2 + 1;
                long p4 = p3 + 1;

                List<long> uniqueFactors1 = p1.Factor().Distinct().ToList();
                List<long> uniqueFactors2 = p2.Factor().Distinct().ToList();
                List<long> uniqueFactors3 = p3.Factor().Distinct().ToList();
                List<long> uniqueFactors4 = p4.Factor().Distinct().ToList();

                if (uniqueFactors1.Count == 4 && uniqueFactors2.Count == 4 && uniqueFactors3.Count == 4 && uniqueFactors4.Count == 4)
                {
                    Console.WriteLine("p1: {0}", p1);
                    break;
                }
            }
        }

        static void Problem48()
        {
            BigInteger sum = new(0);

            for (int i = 1; i <= 1000; i++)
            {
                BigInteger b = new(i);

                sum += BigInteger.Pow(b, i);
            }

            string num = sum.ToString();

            Console.WriteLine("Full num: {0}", num);

            // Get last 10 digits.
            num = num[^10..];
            if (num.Length != 10)
                throw new Exception("Invalid length.");

            Console.WriteLine("Last 10 digits: {0}", num);
        }

        static void Problem49()
        {
            List<int> primes = [];

            for (int n = 1000; n < 10000; n++)
            {
                if (n.IsPrime())
                {
                    primes.Add(n);
                }
            }

            for (int x = 0; x < primes.Count; x++)
                for (int y = 0; y < primes.Count; y++)
                    for (int z = 0; z < primes.Count; z++)
                    {
                        // Only check different sets of numbers.
                        if (primes[x] == primes[y] || primes[y] == primes[z] || primes[x] == primes[z])
                            continue;

                        if (primes[y] != primes[x] + 3330 || primes[z] != primes[y] + 3330)
                            continue;

                        // Are x, y, and z permutations of each other?
                        if (primes[x].HasSameDigits(primes[y]) && primes[x].HasSameDigits(primes[z]))
                        {
                            StringBuilder sb = new();
                            sb.Append(primes[x]);
                            sb.Append(primes[y]);
                            sb.Append(primes[z]);

                            Console.WriteLine("{0}, {1}, {2}, cat: {3}", primes[x], primes[y], primes[z], sb.ToString());
                        }
                    }
        }

        static void Problem50()
        {
            int n = 1000000;

            // First, get all the primes less than n.
            Console.WriteLine("Building prime cache...");
            List<int> primes = [];
            for (int i = 2; i < n; i++)
            {
                if (i.IsPrime())
                {
                    primes.Add(i);
                }
            }

            Console.WriteLine("Found {0} primes.", primes.Count);
            // For n = 100, the list now contains 25 primes, so the longest possible sum has 25 terms.
            // To check terms of length 24, count terms 1-24, 2-25.
            // To check terms of length 23, count terms 1-23, 2-24, 3-25.
            // To check terms of length 22, count terms 1-22, 2-23, 3-24, 4-25.
            // Etc.

            int len = primes.Count - 1;
            int j = 2;

            while (len >= 2)
            {
                if ((len > 1000 && len % 100 == 0) || len <= 1000)
                {
                    Console.WriteLine("Checking terms of length {0}.", len);
                }

                int k = len;

                for (int i = 1; i <= j; i++)
                {
                    int sum = 0;
                    for (int m = k - 1; m >= i - 1; m--) // Count them largest to smallest, so we can bail once the sum hits n and we can eliminate unneccessary calculations.
                    {
                        sum += primes[m];

                        if (sum >= n)
                        {
                            break;
                        }
                    }

                    // Do these terms sum up to a prime number that is less than n? If so, this is the result.
                    if (sum < n && sum.IsPrime())
                    {
                        Console.WriteLine("Counting terms {0}-{1}, sum: {2}, prime: {3}", i, k, sum, sum.IsPrime());
                        return;
                    }

                    k++;
                }

                j++;
                len--;
            }
        }

        static void Problem51()
        {
            const int familyCountToFind = 8;
            int[] inputSet = [0, 1, 2, 3, 4, 5];
            int len = inputSet.Length;
            int lowerBound = Convert.ToInt32("1".PadRight(len, '0'));
            int upperBound = Convert.ToInt32("1".PadRight(len + 1, '0'));

            List<int> primes = [];
            Console.WriteLine("Generating prime table.");
            for (int i = lowerBound; i < upperBound; i++)
            {
                if (i.IsPrime())
                {
                    primes.Add(i);
                }
            }

            Console.WriteLine("Found {0} primes.", primes.Count);

            int count = 0;

            List<int> results = [];

            // Check each prime.
            foreach (int p in primes)
            {
                count++;

                // Check each number of stars in each prime.
                for (int starCount = inputSet.Length - 1; starCount > 0; starCount--)
                {
                    if (count % 100 == 0 || p >= 120383)
                    {
                        Console.WriteLine("Checking prime {0}, number {1} of {2} with {3} stars.", p, count, primes.Count, starCount);
                    }

                    Combinations<int> combos = new(inputSet, starCount, GenerateOption.WithoutRepetition);

                    // Check each combination.
                    foreach (IList<int> combo in combos.Cast<IList<int>>())
                    {
                        int[] array = combo.Cast<int>().ToArray();

                        int familyCount = 0;
                        List<int> familyMembers = [];

                        for (int i = 0; i < 10; i++)
                        {
                            int pTest = p;

                            foreach (int index in array)
                            {
                                if (index >= pTest.ToString().Length)
                                    continue;
                                pTest = pTest.ReplaceDigit(index, i);
                            }

                            if (pTest.ToString().Length != p.ToString().Length)
                                continue;

                            if (primes.Contains(pTest))
                            {
                                familyCount++;
                                familyMembers.Add(pTest);
                            }

                            if (familyCount == familyCountToFind)
                            {
                                Console.WriteLine("Found matching family count for prime {0}: {1}!", p, familyCount);
                                results.Add(p);

                                familyMembers.Sort();
                                Console.WriteLine("Family members:");
                                foreach (int m in familyMembers)
                                    Console.WriteLine(m);

                                return;
                            }
                        }
                    }
                }
            }

            if (results.Count == 0)
                Console.WriteLine("Matching family count not found.");
            else
            {
                results.Sort();
                for (int i = 0; i < results.Count; i++)
                {
                    Console.WriteLine("Match: {0}", results[0]);
                }
            }
        }

        static void Problem52()
        {
            int inclusiveLowerBound = 100001;
            int exclusiveUpperBound = 10000000;

            for (int x = inclusiveLowerBound; x < exclusiveUpperBound; x++)
            {
                List<int> list = [];

                int x2 = 2 * x;

                list.Add(3 * x);
                list.Add(4 * x);
                list.Add(5 * x);
                list.Add(6 * x);

                bool match = x2.SameDigits(list);

                if (match)
                {
                    Console.WriteLine("Found solution. x = {0}", x);
                    return;
                }
            }

            Console.WriteLine("No solution found from {0} to {1}", inclusiveLowerBound, exclusiveUpperBound);
        }

        static void Problem53()
        {
            int count = 0;

            for (int n = 1; n <= 100; n++)
            {
                for (int r = 1; r <= n; r++)
                {
                    Console.WriteLine("n: {0}, r: {1}", n, r);

                    BigInteger valN = new(n);
                    BigInteger valR = new(r);


                    BigInteger val = valN.Factorial() / ((valR.Factorial() * (valN - valR).Factorial()));

                    if (val > 1000000)
                        count++;
                }
            }

            Console.WriteLine("Count: {0}", count);
        }

        static void Problem54()
        {
            //Deck deck = new Deck();
            //deck.Shuffle();

            //PokerHand pokerHand = deck.DealPokerHand();

            //PokerHand pokerHand = new PokerHand(new string[] { "5H", "5C", "6S", "7S", "KD" });

            //Console.WriteLine("Hand:\n{0}", pokerHand);

            //List<Card> cardsRanked;

            //cardsRanked = pokerHand.OnePair();
            //Console.WriteLine("One Pair:\n{0}", PokerHand.CardsToString(cardsRanked));

            //cardsRanked = pokerHand.TwoPair();
            //Console.WriteLine("Two Pair:\n{0}", PokerHand.CardsToString(cardsRanked));

            //cardsRanked = pokerHand.ThreeOfAKind();
            //Console.WriteLine("Three of a Kind:\n{0}", PokerHand.CardsToString(cardsRanked));

            //cardsRanked = pokerHand.Straight();
            //Console.WriteLine("Straight:\n{0}", PokerHand.CardsToString(cardsRanked));

            //cardsRanked = pokerHand.Flush();
            //Console.WriteLine("Flush:\n{0}", PokerHand.CardsToString(cardsRanked));

            //cardsRanked = pokerHand.FullHouse();
            //Console.WriteLine("Full House:\n{0}", PokerHand.CardsToString(cardsRanked));

            //cardsRanked = pokerHand.FourOfAKind();
            //Console.WriteLine("Four of a Kind:\n{0}", PokerHand.CardsToString(cardsRanked));

            //cardsRanked = pokerHand.StraightFlush();
            //Console.WriteLine("Straight Flush:\n{0}", PokerHand.CardsToString(cardsRanked));

            //cardsRanked = pokerHand.RoyalFlush();
            //Console.WriteLine("Royal Flush:\n{0}", PokerHand.CardsToString(cardsRanked));

            //Console.WriteLine("Rank: {0}", pokerHand.GetRank());

            //PokerHand hand1 = new PokerHand(new string[] { "2H", "2D", "4C", "4D", "4S" });
            //PokerHand hand2 = new PokerHand(new string[] { "3C", "3D", "3S", "9S", "9D" });

            //Console.WriteLine("Hand 1:\n{0}", hand1);
            //Console.WriteLine("Hand 2:\n{0}", hand2);

            //PokerHand winner = PokerHand.FindWinner(hand1, hand2);

            //if (winner == null)
            //    Console.WriteLine("Winner: Draw");
            //else if (winner.Equals(hand1))
            //    Console.WriteLine("Winner: Hand 1");
            //else if (winner.Equals(hand2))
            //    Console.WriteLine("Winner: Hand 2");

            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Euler.Inputs.Problem54.txt") ?? throw new Exception("Resource not found: Problem54.txt");
            using StreamReader sr = new(mrs);

            List<string> input = [];

            while (!sr.EndOfStream)
            {
                string? hand = sr.ReadLine();
                if (hand != null)
                    input.Add(hand);
            }
            
            int player1Wins = 0;

            for (int handNum = 1; handNum <= input.Count; handNum++)
            {
                string game = input[handNum - 1];

                StringBuilder sb = new();

                sb.Append(String.Format("Game: {0}, Cards: {1}", handNum, game));

                string[] cards = game.Split(' ');

                PokerHand hand1 = new([cards[0], cards[1], cards[2], cards[3], cards[4]]);
                PokerHand hand2 = new([cards[5], cards[6], cards[7], cards[8], cards[9]]);

                PokerHand? winner = PokerHand.FindWinner(hand1, hand2);

                if (winner == null)
                    sb.Append(", Winner: Draw");
                else if (winner.Equals(hand1))
                {
                    sb.Append(", Winner: Player 1");
                    player1Wins++;
                }
                else if (winner.Equals(hand2))
                    sb.Append(", Winner: Player 2");

                Console.WriteLine(sb.ToString());
            }

            Console.WriteLine("Total Player 1 Wins: {0}", player1Wins);
        }

        static void Problem55()
        {
            int count = 0;

            // A number that never forms a palindrome through the reverse and add process is called a Lychrel number.

            for (BigInteger b = 0; b < 10000; b++)
            {
                Console.WriteLine("Checking {0}.", b);
                BigInteger bBase = b;
                bool isLychrelNum = true;

                for (int i = 0; i < 50; i++)
                {
                    BigInteger bRev = bBase.Reverse();
                    BigInteger sum = bBase + bRev;

                    if (sum.IsPalindrome())
                    {
                        isLychrelNum = false;
                        break;
                    }
                    else
                    {
                        bBase = sum;
                    }
                }

                if (isLychrelNum)
                    count++;
            }

            Console.WriteLine("Count: {0}", count);
        }

        static void Problem56()
        {
            int maxDigits = 1;

            for(BigInteger a = 2; a < 100; a++)
            {
                for(int b = 2; b < 100; b++)
                {
                    BigInteger c = BigInteger.Pow(a, b);

                    int digits = c.SumOfDigits();

                    Console.WriteLine("{0}^{1}={2}, sum of digits: {3}", a, b, c, digits);

                    if (digits > maxDigits)
                    {
                        maxDigits = digits;
                    }
                }
            }

            Console.WriteLine("maxDigits: {0}", maxDigits);
        }

        static void Problem57()
        {
            long ttl = 0;

            BigInteger numerator = 3;
            BigInteger denominator = 2;

            for (int i = 2; i <= 1000; i++)
            {
                BigInteger sum = (numerator + denominator);
                numerator = sum + denominator;
                denominator = sum;
                
                if ((int)BigInteger.Log10(numerator) > (int)BigInteger.Log10(denominator))
                    ttl++;
            }

            Console.WriteLine(ttl);
        }

        static void Problem58()
        {
            // We're only interested in the diagonal values, so all other numbers can be skipped.

            const int TOTAL_LAYERS = 100000;    // The size of the square to be analyzed.
            int skip = 2;                       // The distance between corners.
            int step = 0;                       // Cycles from 1-4 since a square has 4 corners.
            int layer = 1;                      // A complete layer of the square.
            int primes = 0;                     // The number of primes in a given layer.
            int corners = 0;                    // The number of all corners. This is always going to be the number of layers times 4.
            
            // It is interesting to note that the odd squares lie along the bottom right diagonal, but what is more interesting is that out of the
            // numbers lying along both diagonals are prime; that is, a ratio of 8/13 ~= 62%.

            // If one complete new layer is wrapped around the spiral above, a square spiral with side length
            // will be formed.If this process is continued, what is the side length of the square spiral for which the ratio of primes along both diagonals first falls below 10%?

            for (int i = 1; i <= int.MaxValue; i += skip)
            {
                //Console.WriteLine("step {0}, layer {1}: {2}", step, layer, i);

                // 1 is a special case since it starts at the center of the spiral.
                if (step == 0 && layer == 1)
                    layer++;

                // Count all i that are prime. Ratio is prime(i) / i. E.g.. 
                if (i.IsPrime())
                    primes++;
                corners++;

                if (step == 4)
                {
                    // Lower right corner. This marks the end of a layer.
                    double ratio = (double)primes / (double)corners * 100d;

                    Console.WriteLine("Layer completed (lower right corner reached): step: {0}, layer: {1}, i: {2}, primes: {3}, corners: {4}, ratio: {5:0.00}", 
                        step, layer, i, primes, corners, ratio);

                    // We are done with the last layer.
                    if (layer == TOTAL_LAYERS || ratio <= 10d)
                    {
                        // The side length is the square root of the final lower right corner.
                        Console.WriteLine("Side length: {0}", Math.Sqrt((double)i));

                        break;
                    }
                }

                step++;
                if (step > 4)
                {
                    step = 1;
                    skip += 2;
                    layer++;
                }
            }
        }

        static void Problem59()
        {
            // ******************************

            // XOR encoding: https://en.wikipedia.org/wiki/XOR_cipher
            // Character encoding: https://learn.microsoft.com/en-us/dotnet/standard/base-types/character-encoding

            /* This is the easy way to do it... */
            // From https://www.geeksforgeeks.org/program-to-find-the-xor-of-ascii-values-of-characters-in-a-string/
            //string str = "Geeks";

            //int result = str[0];
            //Console.WriteLine(str[0]);
            //for (int i = 1; i < str.Length; i++)
            //{
            //    Console.WriteLine(str[i]);
            //    result = result ^ str[i];
            //}

            //Console.WriteLine(result);

            // ******************************

            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Euler.Inputs.Problem59.txt") ?? throw new Exception("Resource not found: Problem59.txt");
            using StreamReader sr = new(mrs);

            int[] ascii_vals = sr.ReadToEnd().ToIntArray(',');

            // Lowercase ascii values are 97-122
            int iter = 0;

            for (int m = 97; m <= 122; m++)
            {
                for(int n = 97; n <= 122; n++)
                {
                    for (int o = 97; o <= 122; o++)
                    {
                        string decrypted = String.Empty;

                        for (int i = 0; i < ascii_vals.Length; i++)
                        {
                            int result = -1;

                            if (iter == 0)
                            {
                                result = m ^ ascii_vals[i];
                                iter++;
                            }
                            else if (iter == 1)
                            {
                                result = n ^ ascii_vals[i];
                                iter++;
                            }
                            else
                            {
                                result = o ^ ascii_vals[i];
                                iter = 0;
                            }

                            decrypted += (char)result;
                        }

                        if (decrypted.Contains("and") && decrypted.Contains("the") && decrypted.Contains("that"))
                        {
                            Console.WriteLine(decrypted);

                            int sum = 0;

                            for (int i = 0; i < decrypted.Length; i++)
                            {
                                sum += decrypted[i];
                            }

                            Console.WriteLine("Sum of ascii vals: {0}", sum);

                            return;
                        }
                    }
                }
            }
        }

        static void Problem60()
        {
            // Find the lowest sum for a set of five primes for which any two primes concatenate to produce another prime.

            const int MAX_PRIME_SIZE = 100000000;
            const int MAX_ROOT = 1500;
            List<int> primes = [];
            HashSet<int> set = [];

            Console.WriteLine("Searching for primes less than {0}.", MAX_PRIME_SIZE);

            bool[] prime = new bool[MAX_PRIME_SIZE + 1];

            for (int i = 0; i <= MAX_PRIME_SIZE; i++)
                prime[i] = true;

            for (int p = 2; p * p <= MAX_PRIME_SIZE; p++)
            {
                if (prime[p] == true)
                {
                    for (int i = p * p; i <= MAX_PRIME_SIZE; i += p)
                        prime[i] = false;
                }
            }
            
            for (int i = 2; i <= MAX_PRIME_SIZE; i++)
            {
                if (prime[i] == true)
                {
                    primes.Add(i);
                    set.Add(i);
                }
            }
  
            for (int a = 0; a < MAX_ROOT; a++)
            {
                for (int b = a; b < MAX_ROOT; b++)
                {
                    Console.WriteLine("a: {0}, b: {1}", primes[a], primes[b]);
                    if (set.Contains(primes[a].Concatenate(primes[b])) && 
                        set.Contains(primes[b].Concatenate(primes[a])))
                    {
                        for(int c = b; c < MAX_ROOT; c++)
                        {
                            if (set.Contains(primes[a].Concatenate(primes[c])) && set.Contains(primes[b].Concatenate(primes[c])) &&
                                set.Contains(primes[c].Concatenate(primes[a])) && set.Contains(primes[c].Concatenate(primes[b])))
                            {
                                for (int d = c; d < MAX_ROOT; d++)
                                {
                                    if (set.Contains(primes[a].Concatenate(primes[d])) && set.Contains(primes[b].Concatenate(primes[d])) && set.Contains(primes[c].Concatenate(primes[d])) &&
                                        set.Contains(primes[d].Concatenate(primes[a])) && set.Contains(primes[d].Concatenate(primes[b])) && set.Contains(primes[d].Concatenate(primes[c])))
                                    {
                                        for (int e = d; e < MAX_ROOT; e++)
                                        {
                                            if (set.Contains(primes[a].Concatenate(primes[e])) && set.Contains(primes[b].Concatenate(primes[e])) && set.Contains(primes[c].Concatenate(primes[e])) && set.Contains(primes[d].Concatenate(primes[e])) &&
                                                set.Contains(primes[e].Concatenate(primes[a])) && set.Contains(primes[e].Concatenate(primes[b])) && set.Contains(primes[e].Concatenate(primes[c])) && set.Contains(primes[e].Concatenate(primes[d])))
                                            {
                                                Console.WriteLine("Solution found: {0}: {1} + {2} + {3} + {4} + {5}", primes[a] + primes[b] + primes[c] + primes[d] + primes[e],
                                                    primes[a], primes[b], primes[c], primes[d], primes[e]);
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
            Console.WriteLine("Solution not found.");
        }

        static void Problem61()
        {
            // Get a list of all members of each type.
            // Find ordered set that uses one of each type and is cyclical.

            List<FigurateNumber> nums = [];

            int i = 1;
            FigurateNumber fn = new();
            int count = 0;

            while (fn.Number < 10000)
            {
                i++;
                fn = FigurateCalculator.Triangle(i);
                if (fn.Number >= 1000 && fn.Number < 10000)
                {
                    nums.Add(fn);
                    count++;
                }
            }
            Console.WriteLine("Total triangle numbers: {0}", count);
            
            i = 1;
            fn = new();
            count = 0;
            
            while (fn.Number < 10000)
            {
                i++;
                fn = FigurateCalculator.Square(i);
                if (fn.Number >= 1000 && fn.Number < 10000)
                {
                    nums.Add(fn);
                    count++;
                }
            }
            Console.WriteLine("Total square numbers: {0}", count);

            i = 1;
            fn = new();
            count = 0;
            
            while (fn.Number < 10000)
            {
                i++;
                fn = FigurateCalculator.Pentagonal(i);
                if (fn.Number >= 1000 && fn.Number < 10000)
                {
                    nums.Add(fn);
                    count++;
                }
            }
            Console.WriteLine("Total pent numbers: {0}", count);

            i = 1;
            fn = new();
            count = 0;
            
            while (fn.Number < 10000)
            {
                i++;
                fn = FigurateCalculator.Hexagonal(i);
                if (fn.Number >= 1000 && fn.Number < 10000)
                {
                    nums.Add(fn);
                    count++;
                }
            }
            Console.WriteLine("Total hex numbers: {0}", count);

            i = 1;
            fn = new();
            count = 0;
            
            while (fn.Number < 10000)
            {
                i++;
                fn = FigurateCalculator.Heptagonal(i);
                if (fn.Number >= 1000 && fn.Number < 10000)
                {
                    nums.Add(fn);
                    count++;
                }
            }
            Console.WriteLine("Total hep numbers: {0}", count);

            i = 1;
            fn = new();
            count = 0;
            
            while (fn.Number < 10000)
            {
                i++;
                fn = FigurateCalculator.Octagonal(i);
                if (fn.Number >= 1000 && fn.Number < 10000)
                {
                    nums.Add(fn);
                    count++;
                }
            }
            Console.WriteLine("Total oct numbers: {0}", count);

            count = 0;
            foreach (FigurateNumber triangle in nums.Where(i => i.Type == FigurateType.Triangle))
            {
                count++;
                Console.WriteLine(count);

                foreach (FigurateNumber square in nums.Where(i => i.Type == FigurateType.Square))
                {
                    foreach (FigurateNumber pent in nums.Where(i => i.Type == FigurateType.Pentagonal))
                    {
                        foreach (FigurateNumber hex in nums.Where(i => i.Type == FigurateType.Hexagonal))
                        {
                            foreach (FigurateNumber hep in nums.Where(i => i.Type == FigurateType.Heptagonal))
                            {
                                foreach (FigurateNumber oct in nums.Where(i => i.Type == FigurateType.Octagonal))
                                {
                                    List<int> set = [triangle.Number, square.Number, pent.Number, hex.Number, hep.Number, oct.Number];

                                    // Need to iterate over all orders of the set
                                    // See https://www.codeproject.com/Articles/26050/Permutations-Combinations-and-Variations-using-C-G

                                    Permutations<int> perms = new(set, GenerateOption.WithoutRepetition);

                                    foreach (List<int> p in perms.Cast<List<int>>())
                                    {
                                        if (p.IsCyclicSet(2))
                                        {
                                            Console.WriteLine("Found set: {0}", p.PrettyPrint());
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        static void Problem62()
        {
            // Find the smallest cube for which exactly five permutations of its digits are cube.

            // Calculate cubes from 1 to n and store the result in a list.
            // For each new cube, count how many in the list are permutation.

            long i = 0;
            List<long> cubes = [];
            
            while (true)
            {
                i++;
                long cube = i * i * i;

                var matches = cubes.Where(j => j.IsPermutation(cube));
                
                cubes.Add(cube);

                if (matches.Count() >= 5)
                {
                    Console.WriteLine("{0}: {1}, First match: {2}", i, matches.Count(), matches.First());
                    break;
                }
            }
        }

        static void Problem63()
        {
            // Range is small enough to brute force.
            
            long count = 0;

            for (int i = 1; i <= 30; i++)
            {
                BigInteger p = 0;
                BigInteger b = 1;

                while (p.ToString().Length < (i + 1))
                {
                    p = BigInteger.Pow(b, i);

                    if (p.ToString().Length == i)
                    {
                        Console.WriteLine("{0}^{1}: {2}", b, i, p);
                        count++;
                    }

                    b++;
                }
            }

            Console.WriteLine("count: {0}", count);
        }

        static void Problem64()
        {
            int odd_periods = 0;

            for (int i = 2; i <= 13; i++)
            {
                //int root = i.ContinuedFraction(out List<int> repeat);
                //Console.WriteLine("{0}: {1}; {2}", i, root, String.Join(", ", repeat));
                //Console.WriteLine("{0}: {1}", i, root);

                //if (repeat.Count % 2 == 1)
                //    odd_periods++;
            }

            Console.WriteLine("Total odd periods: {0}", odd_periods);
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

            var plt = new ScottPlot.Plot();
            plt.Add.Scatter(dataX, dataY);
            plt.SavePng("d:\\temp\\Misc4.png", 1200, 800);
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

        static void Misc8()
        {
            // Apocalyptic numbers are of the form 2^n and contain '666' somewhere in the expansion.
            // https://www.youtube.com/watch?v=0LkBwCSMsX4

            // Unproven conjecture: 29784 is the last known non-apocalyptic power of two.

            // Search for counter-examples...
            // This takes a while to execute - would be a good candidate for multi-threaded or distributed computation.
            // Other number theory experiments to consider:
            // What about 3^n, 4^n, etc.
            // What about other strings besides '666'? What does the frequency histogram look like?
            for (int n = 40000; n <= 100000; n++)
            {
                if (n % 1000 == 0)
                    Console.WriteLine("Checking n = {0}...", n);

                BigInteger result = BigInteger.Pow(2, n);

                if (!result.ToString().Contains("666"))
                {
                    Console.WriteLine("Found counter-example: {0} is NOT an apocalyptic number.", n);
                    break;
                }
            }
        }

        static void Misc9()
        {
            string s = "Problem123";

            string[] parts = s.Split("Problem");
            Console.WriteLine("0: {0}, 1: {1}", parts[0], parts[1]);

            Type t = typeof(Program);

            foreach(MethodInfo mi in t.GetMethods(BindingFlags.Static | BindingFlags.NonPublic))
            {
                Console.WriteLine(mi.Name);
            }

            MethodInfo? method = typeof(Program).GetMethod("Problem55", BindingFlags.Static | BindingFlags.NonPublic);
            method?.Invoke(null, null);
        }

        static void Misc10()
        {
            Console.WriteLine("Reflection test.");
        }

        static void Misc11()
        {
            for(int i = 0; i <= 100; i++)
            {
                Console.WriteLine("{0} is prime: {1}, is probably prime: {2}. Same? {3}", i, i.IsPrime(), i.IsProbablyPrime(), i.IsPrime() == i.IsProbablyPrime());
            }
        }

        static void Misc12()
        {
            // From Recreations in the Theory of Numbers.
            // Page 1, problem 1.

            // Find the divisors of 16000001.

            long n = 16000001;

            Console.WriteLine(n.ProperDivisors().PrettyPrint());
        }

        #endregion

        #pragma warning restore IDE0051 // Remove unused private members
    }
}