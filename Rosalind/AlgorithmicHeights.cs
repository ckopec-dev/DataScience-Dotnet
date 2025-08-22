using Core;
using QuikGraph;
using QuikGraph.Algorithms;
using QuikGraph.Graphviz;
using ScottPlot.Triangulation;
using SkiaSharp;
using System.Diagnostics.SymbolStore;
using System.Reflection;
using System.Text;

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

            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.bins.txt") ?? throw new ResourceNotFoundException();
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

            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.bins.txt") ?? throw new ResourceNotFoundException();
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

            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.deg.txt") ?? throw new ResourceNotFoundException();
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

            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.ins.txt") ?? throw new ResourceNotFoundException();
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

            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.ddeg.txt") ?? throw new ResourceNotFoundException();
            using StreamReader sr = new(mrs);

            throw new NotImplementedException();
            //#pragma warning disable IDE0305 // Simplify collection initialization
            //List<string> lst = sr.ReadToEnd().ToList();
            //#pragma warning restore IDE0305 // Simplify collection initialization

            //EdgeList e = new(lst);
            //var graph = e.ToAdjacencyGraph();

            //StringBuilder sb = new();

            //// For each vertex, count the number of edges of each connected vertex and print out the results.
            //for (int i = 1; i <= graph.Vertices.Count(); ++i)
            //{
            //    //Console.WriteLine("Vertex {0}", i);

            //    int sum = 0;

            //    foreach (var ed in graph.Edges.Where(j => j.Source == i || j.Target == i))
            //    {
            //        // Each result is a neighbor. 
            //        // Sum the neighbor's edge count.

            //        //Console.WriteLine("\t{0}, {1}", ed.Source, ed.Target);

            //        if (ed.Source == i)
            //        {
            //            // The target is the neighbor.
            //            sum += graph.Edges.Where(k => k.Source == ed.Target || k.Target == ed.Target).Count();
            //        }
            //        else
            //        {
            //            // The source is the neighbor.
            //            sum += graph.Edges.Where(k => k.Source == ed.Source || k.Target == ed.Source).Count();

            //        }
            //    }

            //    sb.Append(sum + " ");

            //}

            //Console.WriteLine(sb.ToString().Trim());
        }

        public static void ProblemMAJ()
        {
            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.maj.txt") ?? throw new ResourceNotFoundException();
            using StreamReader sr = new(mrs);

            string? header = sr.ReadLine();

            if (header == null)
                return;

            int[] headerParts = header.ToIntArray(' ');
            StringBuilder sb = new();

            for (int k = 0; k < headerParts[0]; k++)
            {
                string? line = sr.ReadLine();
                
                if (line == null)
                    break;

                //Console.WriteLine(line);

                int[] array = line.ToIntArray(' ');
                bool found = false;

                foreach (int i in array)
                {
                    int total = array.Count(c => c == i);

                    if (total > headerParts[1] / 2)
                    {
                        sb.Append(i + " ");
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    sb.Append("-1 ");
                }
            }

            Console.Write(sb.ToString().Trim());
        }

        public static void ProblemMER()
        {
            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.mer.txt") ?? throw new ResourceNotFoundException();
            using StreamReader sr = new(mrs);

            sr.ReadLine();
            string? line = sr.ReadLine() ?? throw new InvalidInputException();
            int[] a = line.ToIntArray(' ');
            sr.ReadLine();
            line = sr.ReadLine();
            if (line == null) throw new InvalidInputException();
            int[] b = line.ToIntArray(' ');

            List<int> list = [];
            foreach (int i in a)
                list.Add(i);
            foreach (int i in b)
                list.Add(i);
            list.Sort();

            Console.WriteLine(list.PrettyPrint().Replace(",", ""));
        }

        public static void Problem2SUM()
        {
            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.2sum.txt") ?? throw new ResourceNotFoundException();
            using StreamReader sr = new(mrs);

            string? header = sr.ReadLine() ?? throw new InvalidInputException();
            int[] header_vals = header.ToIntArray(' ');
            int n = header_vals[1];
            
            for(int k = 0; k < header_vals[0]; k++)
            {
                string? line = sr.ReadLine() ?? throw new InvalidInputException();
                
                int[] arr = line.ToIntArray(' ');
                string? answer = null;

                for(int p = 1; p < n; p++)
                {
                    for(int q = p + 1; q <= n; q++)
                    {
                        if (!(q <= n))
                        {
                            continue;
                        }

                        if (arr[p -1] == -arr[q - 1])
                        {
                            answer = String.Format("{0} {1}", p, q);
                            p = n;
                            break;
                        }
                    }
                }

                if (answer == null)
                    Console.WriteLine("-1");
                else
                    Console.WriteLine(answer);
            }
        }

        public static void ProblemBFS()
        {
            // See https://rosalind.info/problems/deg/

            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.bfs.txt") ?? throw new ResourceNotFoundException();
            using StreamReader sr = new(mrs);

            throw new NotImplementedException();
            //#pragma warning disable IDE0305 // Simplify collection initialization
            //List<string> lst = sr.ReadToEnd().ToList();
            //#pragma warning restore IDE0305 // Simplify collection initialization

            //Graph graph = Graph.FromEdgeList(lst);

            //int[] shortestDistances = graph.ShortestDestinations(1);

            //// Print the shortest distances
            //for (int i = 1; i <= graph.VertexCount; i++)
            //{
            //    Console.Write(shortestDistances[i] + " ");
            //}
        }

        public static void ProblemCC()
        {
            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.cc.txt") ?? throw new ResourceNotFoundException();
            using StreamReader sr = new(mrs);

            string? line = sr.ReadLine() ?? throw new InvalidInputException();
            var nm = line.Split();
            int n = int.Parse(nm[0]);
            int m = int.Parse(nm[1]);

            // Create adjacency list
            var adjList = new Dictionary<int, List<int>>();
            for (int i = 1; i <= n; i++)
            {
                adjList[i] = [];
            }

            // Read edges
            for (int i = 0; i < m; i++)
            {
                line = sr.ReadLine();
                if (line == null) throw new InvalidInputException();
                var edge = line.Split();
                int u = int.Parse(edge[0]);
                int v = int.Parse(edge[1]);
                adjList[u].Add(v);
                adjList[v].Add(u);
            }

            var visited = new bool[n + 1]; // index 0 unused
            int components = 0;

            for (int i = 1; i <= n; i++)
            {
                if (!visited[i])
                {
                    DFS_CC(i, adjList, visited);
                    components++;
                }
            }

            Console.WriteLine(components);
        }

        public static void ProblemHEA()
        {
            Stream? mrs = Assembly.GetExecutingAssembly().GetManifestResourceStream("Rosalind.Inputs.hea.txt") ?? throw new ResourceNotFoundException();
            using StreamReader sr = new(mrs);

            int n = int.Parse(sr.ReadLine()!);
            int[] arr = Array.ConvertAll(sr.ReadLine()!.Split(), int.Parse);

            List<(int, int)> swaps = BuildMaxHeap_HEA(arr);

            Console.WriteLine(string.Join(" ", arr));
        }

        #endregion

        #region Helpers

        static void DFS_CC(int node, Dictionary<int, List<int>> adjList, bool[] visited)
        {
            var stack = new Stack<int>();
            stack.Push(node);

            while (stack.Count > 0)
            {
                int current = stack.Pop();
                if (!visited[current])
                {
                    visited[current] = true;
                    foreach (int neighbor in adjList[current])
                    {
                        if (!visited[neighbor])
                            stack.Push(neighbor);
                    }
                }
            }
        }

        static List<(int, int)> BuildMaxHeap_HEA(int[] arr)
        {
            List<(int, int)> swaps = [];
            int n = arr.Length;

            // Start from last non-leaf node and heapify down
            for (int i = n / 2 - 1; i >= 0; i--)
            {
                MaxHeapify_HEA(arr, n, i, swaps);
            }

            return swaps;
        }

        static void MaxHeapify_HEA(int[] arr, int heapSize, int i, List<(int, int)> swaps)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            // Find largest among root, left child and right child
            if (left < heapSize && arr[left] > arr[largest])
                largest = left;

            if (right < heapSize && arr[right] > arr[largest])
                largest = right;

            // If largest is not root, swap and continue heapifying
            if (largest != i)
            {
                // Record the swap (1-indexed for output)
                swaps.Add((i + 1, largest + 1));

                // Swap elements
                (arr[largest], arr[i]) = (arr[i], arr[largest]);

                // Recursively heapify the affected subtree
                MaxHeapify_HEA(arr, heapSize, largest, swaps);
            }
        }

        #endregion
    }
}
