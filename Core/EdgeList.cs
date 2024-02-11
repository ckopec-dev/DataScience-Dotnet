
namespace Core
{
    /// <summary>
    /// https://rosalind.info/glossary/algo-edge-list-format/
    /// </summary>
    public class EdgeList
    {
        // The first line contains two numbers, the number of vertices n and the number of edges m.
        // Each of the following m lines contains an edge given by two vertices. The format can be used to specify directed and undirected graphs as well as weighted and unweighted graphs.

        public int VertexCount { get; set; }
        public int EdgeCount { get; set; }

        //public

        public EdgeList(List<int[]> lines)
        {
            VertexCount = lines[0][0];
            EdgeCount = lines[0][1];

            //throw new NotImplementedException();
        }

        public override string? ToString()
        {
            throw new NotImplementedException();
        }
    }
}
