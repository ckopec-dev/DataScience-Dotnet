using System.Threading.Tasks.Sources;

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
                switch (args[0])
                {
                    case "/problem1": Problem1(); break;
                    case "/problem2": Problem2(); break;
                    case "/problem3": Problem3(); break;
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
                term = Core.MathHelper.Fibonacci(n);

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

            var factors = Core.MathHelper.PrimeFactors(600851475143);

            Console.WriteLine(factors.Last());
        }

        #endregion

        #region Misc experiments

        #endregion

        #region Helpers

        #endregion
    }
}