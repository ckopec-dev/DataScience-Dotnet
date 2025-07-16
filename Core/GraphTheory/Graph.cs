
namespace Core.GraphTheory
{
    public class Graph(int VertexCount)
    {
        #region Fields

        private readonly int vertexCount = VertexCount;

        #endregion
    
        #region Properties

        public readonly Dictionary<int, List<int>> AdjacencyList = [];
        
        public int VertexCount
        {
            get { return vertexCount; }
        }

        #endregion

        #region Ctors/Dtors 

        #endregion

        #region Methods

        public void AddEdge(int u, int v)
        {
            if (!AdjacencyList.TryGetValue(u, out List<int>? value))
            {
                value = [];
                AdjacencyList[u] = value;
            }

            value.Add(v);
        }

        public static Graph FromEdgeList(List<string> Lines)
        {
            string[] header = Lines[0].Split(' ');
            int vertexCount = Convert.ToInt32(header[0]);
            int edgeCount = Convert.ToInt32(header[1]);

            Graph graph = new(vertexCount);

            for (int i = 1; i <= edgeCount; i++)
            {
                string[] line = Lines[i].Split(' ');
                int vertexA = Convert.ToInt32(line[0]);
                int vertexB = Convert.ToInt32(line[1]);

                graph.AddEdge(vertexA, vertexB);

                if (line.Length > 2)
                {
                    // Todo: add support for weighted graphs.
                    throw new NotImplementedException();
                }
            }

            return graph;
        }

        public int[] ShortestDestinations(int startNode)
        {
            // Using a Breadth First Search, returns the
            // shortest distances from start node to
            // every destination node.

            // Initialize distances with -1
            int[] distances = new int[VertexCount + 1]; 
            for (int i = 0; i <= VertexCount; i++)
            {
                distances[i] = -1;
            }

            Queue<int> queue = [];
            queue.Enqueue(startNode);

            // Distance from source to itself is 0
            distances[startNode] = 0; 

            while (queue.Count > 0)
            {
                int currentNode = queue.Dequeue();

                if (AdjacencyList.TryGetValue(currentNode, out List<int>? value))
                {
                    foreach (int neighbor in value)
                    {
                        // If neighbor not visited
                        if (distances[neighbor] == -1) 
                        {
                            distances[neighbor] = distances[currentNode] + 1;
                            queue.Enqueue(neighbor);
                        }
                    }
                }
            }

            return distances;
        }

        #endregion
    }
}
