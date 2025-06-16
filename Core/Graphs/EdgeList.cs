
namespace Core.Graphs
{
    /// <summary>
    /// https://rosalind.info/glossary/algo-edge-list-format/
    /// </summary>
    /// <remarks>
    /// The first line contains two numbers, the number of vertices n and the number of edges m.
    /// Each of the following m lines contains an edge given by two vertices. The format can be used to specify directed and undirected graphs as well as weighted and unweighted graphs.
    ///
    /// Example:
    ///
    /// 6 7
    /// 1 2
    /// 2 3
    /// 6 3
    /// 5 6
    /// 2 5
    /// 2 4
    /// 4 1
    ///
    /// To specify a weighted graph each edge should be followed by its weight.
    ///
    /// Example:
    ///
    /// 6 7
    /// 1 2 4
    /// 2 3 1
    /// 6 3 3
    /// 5 6 2
    /// 2 5 -2
    /// 2 4 5
    /// 4 1 1
    /// 
    /// </remarks>
    public class EdgeList
    {
        public static UndirectedGraph GetUndirectedGraph(List<string> lines)
        {
            int[] header = lines[0].ToIntArray(' ');

            UndirectedGraph graph = new(header[0], header[1]);
            

            throw new NotImplementedException();
        }

        public static DirectedGraph GetDirectedGraph(List<string> lines)
        {
            int[] header = lines[0].ToIntArray(' ');

            DirectedGraph graph = new(header[0], header[1]);


            throw new NotImplementedException();
        }
    }
}
