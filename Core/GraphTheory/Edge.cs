
namespace Core.GraphTheory
{
    /// <summary>
    /// Represents an edge in the graph with weight support
    /// </summary>
    public class Edge<T>(T from, T to, double weight = 1.0, bool isDirected = false) where T : IComparable<T>
    {
        public T From { get; set; } = from;
        public T To { get; set; } = to;
        public double Weight { get; set; } = weight;
        public bool IsDirected { get; set; } = isDirected;

        public override string ToString()
        {
            string arrow = IsDirected ? " -> " : " -- ";
            return $"{From}{arrow}{To} (Weight: {Weight})";
        }

        public override bool Equals(object? obj)
        {
            if (obj is Edge<T> edge)
            {
                return From.Equals(edge.From) && To.Equals(edge.To) &&
                       Math.Abs(Weight - edge.Weight) < 1e-10 &&
                       IsDirected == edge.IsDirected;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(From, To, Weight, IsDirected);
        }
    }
}
