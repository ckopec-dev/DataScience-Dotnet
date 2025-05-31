
namespace Core.Graphs
{
    public abstract class Graph
    {
        public int VertexCount { get; set; }
        public int EdgeCount { get; set; }

        public List<Vertex> Vertices { get; set; }
        public List<Edge> Edges { get; set; }

        public Graph()
        {
            Vertices = [];
            Edges = [];
        }
    }
}
