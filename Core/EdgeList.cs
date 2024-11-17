
namespace Core
{
    /// <summary>
    /// https://rosalind.info/glossary/algo-edge-list-format/
    /// </summary>
    public class EdgeList
    {
        // The first line contains two numbers, the number of vertices n and the number of edges m.
        // Each of the following m lines contains an edge given by two vertices. The format can be used to specify directed and undirected graphs as well as weighted and unweighted graphs.

        /* Example:
        
        6 7
        1 2
        2 3
        6 3
        5 6
        2 5
        2 4
        4 1

        */

        // To specify a weighted graph each edge should be followed by its weight.

        /* Example:
          
        6 7
        1 2 4
        2 3 1
        6 3 3
        5 6 2
        2 5 -2
        2 4 5
        4 1 1

        */

        public int VertexCount { get; set; }
        public int EdgeCount { get; set; }
        public List<Edge> Edges { get; set; }
        
        public EdgeList()
        {
            VertexCount = 0;
            EdgeCount = 0;
            Edges = [];
        }

        public EdgeList(int vertexCount, int edgeCount, List<Edge> edges)
        {
            VertexCount = vertexCount;
            EdgeCount = edgeCount;
            Edges = edges;
        }

        public EdgeList(List<string> list)
        {
            VertexCount = list[0][0];
            EdgeCount = list[0][1];
            Edges = [];

            for (int i = 2; i < list.Count; i++)
            {
                int[] vals = list[i].ToIntArray(' ');
                if (vals.Length == 2)
                    Edges.Add(new Edge(vals[0], vals[1]));
                if (vals.Length == 3)
                    Edges.Add(new Edge(vals[0], vals[1], vals[2]));
            }
        }

        public override string? ToString()
        {
            throw new NotImplementedException();
        }
    }
}
