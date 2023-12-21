
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

            int n = ReadInputToInt32();

            Console.WriteLine("FinonacciRecursive({0}): {1}", n, Core.MathHelper.FibonacciRecursive(n));

            Console.WriteLine("Finonacci({0}): {1}", n, Core.MathHelper.Fibonacci(n));
        }

        public static void ProblemBINS()
        {
            // https://rosalind.info/problems/bins/
            // Given: Two positive integers n <= 10^5 and m <= 10^5, a sorted array A[1..n] of integers from −10^5 to 10^5 and a list of m integers −10%5 <= k1,k2,…,km<=10%5.
            // Return: For each ki, output an index 1<=j≤n s.t.A[j] = ki or "-1" if there is no such index.

            List<double[]> input = ReadInputToDoubleListArray();

            Console.WriteLine("Input line count: {0}", input.Count);
            foreach (double[] row in input)
            {
                Console.WriteLine(String.Join(", ", row));
            }

            double[] output = new double[input[3].Length];

            for(int i = 0; i < output.Length; i++)
            {
                // Where does input[3][i] appear in input[2]?

                output[i] = Array.FindIndex(input[2], j => j == input[3][i]);

                // One based output
                if (output[i] > -1) output[i]++;
            }

            WriteOutput(String.Join(" ", output));
        }

        public static void ProblemDEG()
        {
            // https://rosalind.info/problems/deg/
            // Given: A simple graph with n <= 10^3 vertices in the edge list format.
            // Return: An array D[1..n] where D[i] is the degree of vertex i.

            List<double[]> input = ReadInputToDoubleListArray();
            SortedDictionary<double, int> dic = new();

            for(int i = 1; i < input.Count; i++)
            {
                // How many edges on this vertex?
                
                if (dic.ContainsKey(input[i][0]))
                {
                    dic[input[i][0]]++;
                }
                else
                {
                    dic.Add(input[i][0], 1);
                }

                if (dic.ContainsKey(input[i][1]))
                {
                    dic[input[i][1]]++;
                }
                else
                {
                    dic.Add(input[i][1], 1);
                }
            }

            WriteOutput(String.Join(" ", dic.Values));
        }

        #endregion

        #region Helpers

        #endregion
    }
}
