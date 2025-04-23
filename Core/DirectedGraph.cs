
using System.Diagnostics.CodeAnalysis;

namespace Core
{
    public class DirectedGraph
    {
        public int Vertices { get; set; }
        public int Edges { get; set; }
        public List<int> Origins = [];
        public List<int> Destinations = [];

        public DirectedGraph()
        {
        }

        public DirectedGraph(StreamReader sr)
        {
            string? line = sr.ReadLine() ?? throw new InvalidInputException();
            string[] header = line.Split(' ');
            Vertices = int.Parse(header[0]);
            Edges = int.Parse(header[1]);

            while(!sr.EndOfStream)
            {
                line = sr.ReadLine() ?? throw new InvalidInputException();

                string[] split = line.Split(' ');
                Origins.Add(Convert.ToInt32(split[0])); 
                Destinations.Add(Convert.ToInt32(split[1]));
            }
        }

        public List<int> PathsFrom(int destinationVertexNumber, int depth)
        {
            // e.g. vertextNumber 5. 
            // Find all destinations with that value.

            List<int> originIndexes = [];
            int newDepth = depth + 1;

            for (int i = 0; i < Destinations.Count; i++)
            {
                if (Destinations[i] == destinationVertexNumber)
                {
                    // this is a path. find next path.
                    Console.WriteLine("Destination: {0}, Origin: {1}, Depth: {2}", destinationVertexNumber, Origins[i], depth);
                    originIndexes.Add(i);

                    originIndexes.AddRange(PathsFrom(Origins[i], newDepth));
                }
            }

            return originIndexes;
        }
    }
}
