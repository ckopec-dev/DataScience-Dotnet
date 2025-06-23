namespace Core.GraphTheory
{
    public class Edge(int vertexA, int vertexB, int? weight = null)
    {
        public int VertexA { get; set; } = vertexA;
        public int VertexB { get; set; } = vertexB;
        public int? Weight { get; set; } = weight;

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", VertexA, VertexB, Weight).Trim();
        }
    }
}
