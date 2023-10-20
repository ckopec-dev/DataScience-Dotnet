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
    }
}