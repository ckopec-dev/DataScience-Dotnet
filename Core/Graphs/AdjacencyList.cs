using System.Text;

namespace Core.Graphs
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
            StringBuilder sb = new();

            for (int i = 0; i < graph.Count; i++)
            {
                sb.Append(i + ": ");

                foreach (int j in graph[i])
                {
                    sb.Append(j + " ");
                }

                sb.AppendLine();
            }
            
            return sb.ToString();
        }

        public void AddEdge(int vertexA, int vertexB)
        {
            graph[vertexA].Add(vertexB);
        }

        #endregion
    }
}
