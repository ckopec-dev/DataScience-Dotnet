using Core;
using System.Reflection;

namespace Rosalind
{
    public class AlgorithmicHeights
    {
        #region Problems

        public static void ProblemFIBO()
        {
            // https://rosalind.info/problems/fibo/
            // Given: A positive integer n <= 25.
            // Return: The value of F(n).

            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.bins.txt") ?? throw new Exception("Resource not found: bins.txt");
            using StreamReader sr = new(mrs);

            int n = Convert.ToInt32(sr.ReadToEnd().Trim());

            Console.WriteLine("FinonacciRecursive({0}): {1}", n, Core.MathHelper.FibonacciRecursive(n));

            Console.WriteLine("Finonacci({0}): {1}", n, Core.MathHelper.Fibonacci(n));
        }

        public static void ProblemBINS()
        {
            // https://rosalind.info/problems/bins/
            // Given: Two positive integers n <= 10^5 and m <= 10^5, a sorted array A[1..n] of integers from −10^5 to 10^5 and a list of m integers −10%5 <= k1,k2,…,km<=10%5.
            // Return: For each ki, output an index 1<=j≤n s.t.A[j] = ki or "-1" if there is no such index.

            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.bins.txt") ?? throw new Exception("Resource not found: bins.txt");
            using StreamReader sr = new(mrs);

            List<double[]> input = sr.ReadToEnd().Trim().ToDoubleListArray('\n', ' ');

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

            Console.WriteLine(String.Join(" ", output));
        }

        public static void ProblemDEG()
        {
            // https://rosalind.info/problems/deg/
            // Given: A simple graph with n <= 10^3 vertices in the edge list format.
            // Return: An array D[1..n] where D[i] is the degree of vertex i.

            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.deg.txt") ?? throw new Exception("Resource not found: deg.txt");
            using StreamReader sr = new(mrs);

            List<double[]> input = sr.ReadToEnd().Trim().ToDoubleListArray('\n', ' ');
            SortedDictionary<double, int> dic = [];

            for(int i = 1; i < input.Count; i++)
            {
                // How many edges on this vertex?
                
                if (dic.TryGetValue(input[i][0], out int value))
                {
                    dic[input[i][0]] = ++value;
                }
                else
                {
                    dic.Add(input[i][0], 1);
                }

                if (dic.TryGetValue(input[i][1], out int value2))
                {
                    dic[input[i][1]] = ++value2;
                }
                else
                {
                    dic.Add(input[i][1], 1);
                }
            }

            Console.WriteLine(String.Join(" ", dic.Values));
        }
        
        public static void ProblemINS()
        {
            // https://rosalind.info/problems/ins/
            // Given: A positive integer n <= 10^3 and an array A[1..n] of integers.
            // Return: The number of swaps performed by insertion sort algorithm on A[1..n].
            // NOTE: Manually strip out irrelevant first line of input provided.

            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.ins.txt") ?? throw new Exception("Resource not found: ins.txt");
            using StreamReader sr = new(mrs);

            int[] input = sr.ReadToEnd().Trim().ToIntArray(' ');

            int swaps = 0;

            int j = input.Length;
            for (int i = 1; i < j; ++i)
            {
                int sort = input[i];
                int k = i - 1;

                while (k >= 0 && input[k] > sort)
                {
                    input[k + 1] = input[k];
                    k--;
                    swaps++;
                }
                input[k + 1] = sort;
            }

            Console.WriteLine(swaps.ToString());
        }

        public static void ProblemDDEG()
        {
            // Given: A simple graph with n <= 10^3 vertices in the edge list format.
            // Return: An array D[1..n] where D[i] is the sum of the degrees of i's neighbors.

            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.ddeg.txt") ?? throw new Exception("Resource not found: ddeg.txt");
            using StreamReader sr = new(mrs);

            #pragma warning disable IDE0305 // Simplify collection initialization
            List<string> lst = sr.ReadToEnd().ToList();
#pragma warning restore IDE0305 // Simplify collection initialization
            
            EdgeList e = new(lst);

            Console.WriteLine(e);
            
            //throw new NotImplementedException();
        }

        #endregion

        #region Helpers

        #endregion
    }
}
