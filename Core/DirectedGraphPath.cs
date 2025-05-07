
namespace Core
{
    public class DirectedGraphPath(int origin, int destination, int hop)
    {
        public int Origin { get; set; } = origin;
        public int Destination { get; set; } = destination;
        public int Hop { get; set; } = hop;

        public override string ToString()
        {
            return String.Format("{0}, {1}, {2}", Origin, Destination, Hop);
        }
    }
}
