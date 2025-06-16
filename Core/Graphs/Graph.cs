
namespace Core.Graphs
{
    /// <summary>
    /// See https://rosalind.info/problems/deg/
    /// </summary>
    public abstract class Graph
    {
        public List<Vertex> Vertices { get; set; }
        public List<Edge> Edges { get; set; }

        public Graph(int vertexCount, int edgeCount)
        {
            Vertices = [];
            for (int i = 0; i < vertexCount; i++)
            {
                Vertices.Add(new());
            }

            Edges = [];
            for(int i = 0; i < edgeCount; i++)
            {
                Edges.Add(new());
            }
        }
    }
}
