
namespace Core
{
    public class Edge(int vertexA, int vertexB, int? weight = null)
    {
        public int VertexA { get; set; } = vertexA;
        public int VertexB { get; set; } = vertexB;
        public int? Weight { get; set; } = weight;

        public override string ToString()
        {
            return String.Format("{0} {1} {2}", VertexA, VertexB, Weight).Trim();
        }
    }
}
