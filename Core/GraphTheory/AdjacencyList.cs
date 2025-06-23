using System.Text;

namespace Core.GraphTheory
{
    public class AdjacencyList
    {
        #region Members 

        private readonly int vertexCount;
        private readonly bool isDirected;
        private readonly List<List<int>> graph;

        #endregion

        #region Properties

        public int VertexCount
        {
            get { return vertexCount; }
        }

        public bool IsDirected 
        { 
            get 
            { 
                return isDirected; 
            } 
        }

        #endregion

        #region Ctors/Dtors

        public AdjacencyList(bool IsDirected, int VertexCount)
        {
            graph = [];
            for(int i = 0; i < VertexCount; i++)
            {
                graph.Add([]);
            }

            isDirected = IsDirected;
            vertexCount = VertexCount;
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return this.ToString(true);
        }

        public string ToString(bool zeroBased)
        {
            // A 0-based representation is the default, and corresponds to
            // the internal representation of the data.

            // If 1-based is preferred, the vertices and edges are 
            // incremented by 1.

            StringBuilder sb = new();

            for (int i = 0; i < graph.Count; i++)
            {
                int i_base = i;
                if (!zeroBased)
                {
                    i_base++;
                }

                sb.Append(i_base + ": ");

                foreach (int j in graph[i])
                {
                    int j_base = j;
                    if (!zeroBased)
                    {
                        j_base++;
                    }

                    sb.Append(j_base + " ");
                }

                sb.Append('\n');
            }

            return sb.ToString().Trim();
        }

        public void AddEdge(int VertexA, int VertexB)
        {
            graph[VertexA].Add(VertexB);

            if (!isDirected)
            {
                graph[VertexB].Add(VertexA);
            }
        }

        public static AdjacencyList FromEdgeList(
            bool IsDirected, 
            bool ZeroBased,
            List<string> Lines)
        {
            string[] header = Lines[0].Split(' ');
            int vertexCount = Convert.ToInt32(header[0]);
            int edgeCount = Convert.ToInt32(header[1]); 

            AdjacencyList adj = new(IsDirected, vertexCount);

            for(int i = 1; i <= edgeCount; i++)
            {
                string[] line = Lines[i].Split(' ');
                int vertexA = Convert.ToInt32(line[0]);
                int vertexB = Convert.ToInt32(line[1]);

                if (!ZeroBased)
                {
                    // Input is 1-based, so subtract 1 from each vertex
                    // since AdjacencyList is 0-based.

                    vertexA--;
                    vertexB--;
                }
                
                //Console.WriteLine("i: {0}, a: {1}, b: {2}", i, vertexA, vertexB);
                
                adj.AddEdge(vertexA, vertexB);

                if (line.Length > 2)
                {
                    // Todo: add support for weighted graphs.
                    throw new NotImplementedException();
                }
            }

            return adj;
        }

        public List<int> BreadthFirstSearch(int originVertex)
        {
            // Returns list of vertices reachable from the origin.

            if (IsDirected)
            {
                throw new NotImplementedException("BFS does not support directed graphs.");
            }

            List<int> destinations = [];
            Queue<int> q = new();

            bool[] visited = new bool[vertexCount];

            visited[originVertex] = true;
            q.Enqueue(originVertex);

            while(q.Count > 0)
            {
                int currentVertex = q.Dequeue();
                destinations.Add(currentVertex);

                foreach (int x in graph[currentVertex])
                {
                    if (!visited[x])
                    {
                        visited[x] = true;
                        q.Enqueue(x);
                    }
                }
            }

            return destinations;
        }



        #endregion
    }
}
