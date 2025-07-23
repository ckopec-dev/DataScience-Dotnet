
namespace Core.GraphTheory.v2
{
    /// <summary>
    /// Edges have no direction.
    /// </summary>
    /// <param name="vertexCount">The number of vertices.</param>
    public class UndirectedGraph(int vertexCount) : IGraph
    {
        #region Fields

        private readonly int _VertexCount = vertexCount;

        #endregion

        #region Properties

        public int[,] AdjacencyMatrix = new int[vertexCount, vertexCount];

        #endregion
        
        #region Ctors/Dtors

        #endregion

        #region Methods

        public void AddEdge(int i, int j)
        {
            AdjacencyMatrix[i, j] = 1;
            AdjacencyMatrix[j, i] = 1; // since it's undirected
        }

        public void DisplayMatrix()
        {
            if (AdjacencyMatrix == null)
                return;

            int V = AdjacencyMatrix.GetLength(0);
            for (int i = 0; i < V; i++)
            {
                for (int j = 0; j < V; j++)
                {
                    Console.Write(AdjacencyMatrix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        #endregion
    }
}
