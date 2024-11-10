
namespace Core
{
    public class Edge(int vertexA, int vertexB)
    {
        public int VertexA { get; set; } = vertexA;
        public int VertexB { get; set; } = vertexB;

        public override string ToString()
        {
            return String.Format("{0} {1}", VertexA, VertexB);
        }
    }
}
