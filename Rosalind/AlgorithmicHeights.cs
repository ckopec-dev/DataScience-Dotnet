
namespace Rosalind
{
    public class AlgorithmicHeights : ProblemDomain
    {
        #region Problems

        public static void ProblemFIBO()
        {
            // https://rosalind.info/problems/fibo/
            // Given: A positive integer n <= 25.
            // Return: The value of F(n).

            int n = LoadInt32();

            Console.WriteLine("FinonacciRecursive({0}): {1}", n, Core.MathHelper.FibonacciRecursive(n));

            Console.WriteLine("Finonacci({0}): {1}", n, Core.MathHelper.Fibonacci(n));
        }

        public static void ProblemBINS()
        {
            // https://rosalind.info/problems/bins/
            // Given: Two positive integers n <= 105 and m <= 105, a sorted array A[1..n] of integers from −105 to 105 and a list of m integers −105 <= k1,k2,…,km<=105.
            // Return: For each ki, output an index 1<=j≤n s.t.A[j] = ki or "-1" if there is no such index.

            throw new NotImplementedException();
        }

        #endregion

        #region Helpers

        private static int LoadInt32()
        {
            if (InputPath != null)
                return Convert.ToInt32(File.ReadAllText(InputPath));
            else
                throw new InvalidInputException();
        }

        #endregion
    }
}
