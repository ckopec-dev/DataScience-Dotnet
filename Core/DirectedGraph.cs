
namespace Core
{
    public class DirectedGraph
    {
        public int VertexCount { get; set; }
        public int EdgeCount { get; set; }
        public List<int> Origins = [];
        public List<int> Destinations = [];
        //public List<DirectedGraphPath> Paths = [];

        public DirectedGraph()
        {
        }

        public DirectedGraph(StreamReader sr)
        {
            string? line = sr.ReadLine() ?? throw new InvalidInputException();
            string[] header = line.Split(' ');
            VertexCount = int.Parse(header[0]);
            EdgeCount = int.Parse(header[1]);

            while(!sr.EndOfStream)
            {
                line = sr.ReadLine() ?? throw new InvalidInputException();

                string[] split = line.Split(' ');
                Origins.Add(Convert.ToInt32(split[0])); 
                Destinations.Add(Convert.ToInt32(split[1]));
            }
        }



        //public List<int> PathsFrom(int destinationVertexNumber, int originVertexNumber)
        //{
        //    return PathsFrom(destinationVertexNumber, originVertexNumber, 1, null);
        //}

        //public List<int> PathsFrom(int destinationVertexNumber, int originVertexNumber, int depth, string? fullPath)
        //{
        //    List<int> originIndexes = [];
        //    int newDepth = depth + 1;
            
        //    for (int i = 0; i < Destinations.Count; i++)
        //    {
        //        if (Destinations[i] == destinationVertexNumber)
        //        {
        //            string currentPath = String.Format(
        //                "Destination: {0}, Origin: {1}, Depth: {2}", 
        //                destinationVertexNumber, Origins[i], depth);

        //            Paths.Add(new(Origins[i], Destinations[i], depth));
                    
        //            if (fullPath != null)
        //            {
        //                currentPath = fullPath + " -> " + currentPath;
        //            }

        //            originIndexes.Add(i);

        //            if (Destinations[i] == originVertexNumber ||
        //                Origins[i] != originVertexNumber)
        //            {
        //                originIndexes.AddRange(PathsFrom(Origins[i], originVertexNumber, newDepth, currentPath));
        //            }
        //            else
        //            {
        //                Console.WriteLine("path ended");
        //            }
        //        }
        //    }

        //    return originIndexes;
        //}
    }
}
