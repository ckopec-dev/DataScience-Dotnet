
namespace Core
{
    public class DirectedGraphQueueItem(int origin, int destination, int hopCount)
    {
        public int Origin { get; set; } = origin;
        public int Destination { get; set; } = destination;
        public int HopCount { get; set; } = hopCount;
    }
}
