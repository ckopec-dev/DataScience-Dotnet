using QuikGraph;
using System.Security.Cryptography;
using System.Text;

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

        #region Fields

        public int VertexCount { get; set; }
        public int EdgeCount { get; set; }
        public List<Edge> Edges { get; set; }

        #endregion

        #region Properties

        public List<Vertex> Vertices 
        {
            get
            {
                List<Vertex> list = [];

                for (int i = 0; i < VertexCount; i++)
                {
                    list.Add(new Vertex()); 
                }

                return list;
            }
        }

        #endregion

        #region Ctors/Dtors

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
            int[] firstRow = list[0].ToIntArray(' ');
            VertexCount = firstRow[0];
            EdgeCount = firstRow[1];
            Edges = [];

            for (int i = 1; i < list.Count; i++)
            {
                int[] vals = list[i].ToIntArray(' ');
                if (vals.Length == 2)
                    Edges.Add(new Edge(vals[0], vals[1]));
                if (vals.Length == 3)
                    Edges.Add(new Edge(vals[0], vals[1], vals[2]));
            }
        }

        #endregion

        #region Public methods

        public override string? ToString()
        {
            StringBuilder sb = new();

            sb.AppendLine(String.Format("{0} {1}", VertexCount, EdgeCount));

            foreach(Edge e in Edges)
            {
                sb.AppendLine(String.Format("{0} {1} {2}", e.VertexA, e.VertexB, e.Weight).Trim());
            }

            return sb.ToString();
        }

        public AdjacencyGraph<int, Edge<int>> ToGraph()
        {
            
            AdjacencyGraph<int, Edge<int>> graph = new();

            for (int i = 1; i <= VertexCount; i++)
            {
                graph.AddVertex(i);
            }

            foreach(Edge e in Edges)
            {
                graph.AddEdge(new Edge<int>(e.VertexA, e.VertexB));
            }

            return graph;
        }

        #endregion
    }
}
