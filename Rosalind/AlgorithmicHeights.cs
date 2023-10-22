
namespace Rosalind
{
    public static class AlgorithmicHeights
    {
        #region Problems

        public static void ProblemFIBO()
        {
            int n = 21;

            Console.WriteLine("FinonacciRecursive({0}): {1}", n, Core.MathHelper.FibonacciRecursive(n));

            Console.WriteLine("Finonacci({0}): {1}", n, Core.MathHelper.Fibonacci(n));
        }

        public static void ProblemBINS()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Helpers

        #endregion
    }
}
