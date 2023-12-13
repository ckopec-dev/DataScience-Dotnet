using Core;
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
    }
}