using Core;
using Microsoft.Data.SqlClient.DataClassification;
using System.Threading.Tasks.Sources;

namespace Test
{
    /// <summary>
    /// Eventually to be turned into actual unit tests.
    /// </summary>
    internal class Program
    {
        static void Main()
        {
            PrimeTest();
            ProperDivisorsTest();
            BubbleSortTest();
            EasterTest();
            MagicSquareTest();
            DayNumberTest();
            HistogramTest();
            EulerMascheroniTest();
        }

        static void PrimeTest()
        {
            for(int i = 1; i <= 10; i++) 
            {
                Console.WriteLine("{0} is prime: {1}", i, Core.MathHelper.IsPrime(i));
            }
        }

        static void ProperDivisorsTest()
        {
            Console.WriteLine("Proper divisors of 220: {0}", String.Join(", ", Core.MathHelper.ProperDivisors(220)));
        }

        static void BubbleSortTest()
        {
            int[] ints = { 5, 3, 7, 1 };

            Core.SortHelper.BubbleSort(ints);

            Console.WriteLine("Bubble sorted: {0}", String.Join(", ", ints));
        }

        static void EasterTest()
        {
            // To verify, see https://www.census.gov/data/software/x13as/genhol/easter-dates.html 
            Console.WriteLine("Easter in 2008 fall on: {0}", Core.PhysicsHelper.Easter(2008));
        }

        static void MagicSquareTest()
        {
            // Test of magic square helper function.

            long[,] a = new long[3, 3] {
                {2, 7, 6},
                {9, 5, 1},
                {4, 3, 8}
            };

            Console.WriteLine("Is a magic square: {0}", MathHelper.Is3x3MagicSquare(a));
        }

        static void DayNumberTest()
        {
            DateTime dt = new(1985, 2, 17);
            Console.WriteLine("The day number of 2/17/1985 is {0}", dt.DayNumber());
        }

        static void HistogramTest()
        {
            Random r = new();
            const uint dataPoints = 5;
            Histogram<uint> hist = new();

            for (uint i = 0; i < dataPoints; i++)
            {
                uint iterations = (uint)r.Next(0, 11);

                hist.Increment(iterations);
            }

            foreach (KeyValuePair<uint, uint> histEntry in hist.AsEnumerable())
            {
                Console.WriteLine("{0} occurred {1} times", histEntry.Key, histEntry.Value);
            }
        }

        static void EulerMascheroniTest()
        {
            // From Projects in Scientific Computation, Project 1.1.1.

            double sum = 0;
            int i = 0;

            while (!sum.ToString().StartsWith("0.5772"))  //for (int i = 1; i <= 10000; i++)
            {
                i++;
                sum = 0;                

                for(int j = 1; j <= i; j++)
                {
                    sum += 1d / j;
                }

                sum -= Math.Log(i);

                Console.WriteLine("{0}: {1}", i, sum);
            }
        }
    }
}