using System.Text;

namespace Core.GraphTheory
{
    /// <summary>
    /// Comprehensive graph data structure supporting both directed and undirected graphs
    /// </summary>
    public class Graph<T>(bool isDirected = false) where T : IComparable<T>
    {
        private readonly Dictionary<T, HashSet<Edge<T>>> _adjacencyList = [];
        private readonly HashSet<T> _vertices = [];
        private readonly HashSet<Edge<T>> _edges = [];
        private readonly bool _isDirected = isDirected;

        #region Properties
        public bool IsDirected => _isDirected;
        public int VertexCount => _vertices.Count;
        public int EdgeCount => _edges.Count;
        public IEnumerable<T> Vertices => _vertices.AsEnumerable();
        public IEnumerable<Edge<T>> Edges => _edges.AsEnumerable();
        #endregion

        #region Basic Operations
        /// <summary>
        /// Adds a vertex to the graph
        /// </summary>
        public bool AddVertex(T vertex)
        {
            if (_vertices.Contains(vertex))
                return false;

            _vertices.Add(vertex);
            _adjacencyList[vertex] = [];
            return true;
        }

        /// <summary>
        /// Removes a vertex and all its edges from the graph
        /// </summary>
        public bool RemoveVertex(T vertex)
        {
            if (!_vertices.Contains(vertex))
                return false;

            // Remove all edges connected to this vertex
            var edgesToRemove = _edges.Where(e => e.From.Equals(vertex) || e.To.Equals(vertex)).ToList();
            foreach (var edge in edgesToRemove)
            {
                RemoveEdge(edge.From, edge.To);
            }

            _vertices.Remove(vertex);
            _adjacencyList.Remove(vertex);
            return true;
        }

        /// <summary>
        /// Adds an edge between two vertices
        /// </summary>
        public bool AddEdge(T from, T to, double weight = 1.0)
        {
            // Ensure both vertices exist
            AddVertex(from);
            AddVertex(to);

            var edge = new Edge<T>(from, to, weight, _isDirected);

            if (_edges.Contains(edge))
                return false;

            _edges.Add(edge);
            _adjacencyList[from].Add(edge);

            // For undirected graphs, add reverse edge
            if (!_isDirected && !from.Equals(to))
            {
                var reverseEdge = new Edge<T>(to, from, weight, _isDirected);
                _adjacencyList[to].Add(reverseEdge);
            }

            return true;
        }

        /// <summary>
        /// Removes an edge between two vertices
        /// </summary>
        public bool RemoveEdge(T from, T to)
        {
            if (!_vertices.Contains(from) || !_vertices.Contains(to))
                return false;

            var edgeToRemove = _adjacencyList[from].FirstOrDefault(e => e.To.Equals(to));
            if (edgeToRemove == null)
                return false;

            _adjacencyList[from].Remove(edgeToRemove);
            _edges.Remove(edgeToRemove);

            // For undirected graphs, remove reverse edge
            if (!_isDirected && !from.Equals(to))
            {
                var reverseEdge = _adjacencyList[to].FirstOrDefault(e => e.To.Equals(from));
                if (reverseEdge != null)
                {
                    _adjacencyList[to].Remove(reverseEdge);
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if an edge exists between two vertices
        /// </summary>
        public bool HasEdge(T from, T to)
        {
            return _vertices.Contains(from) &&
                   _adjacencyList[from].Any(e => e.To.Equals(to));
        }

        /// <summary>
        /// Gets the weight of an edge between two vertices
        /// </summary>
        public double GetEdgeWeight(T from, T to)
        {
            if (!HasEdge(from, to))
                throw new ArgumentException("Edge does not exist");

            return _adjacencyList[from].First(e => e.To.Equals(to)).Weight;
        }

        /// <summary>
        /// Gets all neighbors of a vertex
        /// </summary>
        public IEnumerable<T> GetNeighbors(T vertex)
        {
            if (!_vertices.Contains(vertex))
                return [];

            return _adjacencyList[vertex].Select(e => e.To);
        }

        /// <summary>
        /// Gets the degree of a vertex (number of edges)
        /// </summary>
        public int GetDegree(T vertex)
        {
            if (!_vertices.Contains(vertex))
                return 0;

            if (_isDirected)
                return GetInDegree(vertex) + GetOutDegree(vertex);

            return _adjacencyList[vertex].Count;
        }

        /// <summary>
        /// Gets the in-degree of a vertex (for directed graphs)
        /// </summary>
        public int GetInDegree(T vertex)
        {
            if (!_vertices.Contains(vertex))
                return 0;

            return _edges.Count(e => e.To.Equals(vertex));
        }

        /// <summary>
        /// Gets the out-degree of a vertex (for directed graphs)
        /// </summary>
        public int GetOutDegree(T vertex)
        {
            if (!_vertices.Contains(vertex))
                return 0;

            return _adjacencyList[vertex].Count;
        }
        #endregion

        #region Graph Algorithms
        /// <summary>
        /// Performs Depth-First Search starting from a given vertex
        /// </summary>
        public List<T> DepthFirstSearch(T startVertex)
        {
            if (!_vertices.Contains(startVertex))
                throw new ArgumentException("Start vertex does not exist in graph");

            var visited = new HashSet<T>();
            var result = new List<T>();
            var stack = new Stack<T>();

            stack.Push(startVertex);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                if (visited.Contains(current))
                {
                    continue;
                }
                visited.Add(current);
                result.Add(current);

                // Add neighbors in reverse order to maintain left-to-right traversal
                var neighbors = GetNeighbors(current).Reverse();
                foreach (var neighbor in neighbors)
                {
                    if (!visited.Contains(neighbor))
                    {
                        stack.Push(neighbor);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Performs Breadth-First Search starting from a given vertex
        /// </summary>
        public List<T> BreadthFirstSearch(T startVertex)
        {
            if (!_vertices.Contains(startVertex))
                throw new ArgumentException("Start vertex does not exist in graph");

            var visited = new HashSet<T>();
            var result = new List<T>();
            var queue = new Queue<T>();

            queue.Enqueue(startVertex);
            visited.Add(startVertex);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                result.Add(current);

                foreach (var neighbor in GetNeighbors(current))
                {
                    if (visited.Contains(neighbor))
                    {
                        continue;
                    }
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }

            return result;
        }

        /// <summary>
        /// Finds the shortest path between two vertices using Dijkstra's algorithm
        /// </summary>
        public List<T> FindShortestPath(T start, T end)
        {
            if (!_vertices.Contains(start) || !_vertices.Contains(end))
                throw new ArgumentException("Start or end vertex does not exist");

            var distances = new Dictionary<T, double>();
            var previous = new Dictionary<T, T>();
            var unvisited = new HashSet<T>(_vertices);

            // Initialize distances
            foreach (var vertex in _vertices)
            {
                distances[vertex] = double.MaxValue;
            }
            distances[start] = 0;

            while (unvisited.Count > 0)
            {
                // Find unvisited vertex with minimum distance
                var current = unvisited.OrderBy(v => distances[v]).First();
                unvisited.Remove(current);

                if (current.Equals(end))
                    break;

                if (distances[current] == double.MaxValue)
                    break;

                // Update distances to neighbors
                foreach (var edge in _adjacencyList[current])
                {
                    if (unvisited.Contains(edge.To))
                    {
                        var newDistance = distances[current] + edge.Weight;
                        if (newDistance < distances[edge.To])
                        {
                            distances[edge.To] = newDistance;
                            previous[edge.To] = current;
                        }
                    }
                }
            }

            // Reconstruct path
            var path = new List<T>();
            var pathVertex = end;

            while (previous.ContainsKey(pathVertex))
            {
                path.Add(pathVertex);
                pathVertex = previous[pathVertex];
            }
            path.Add(start);
            path.Reverse();

            return path.Count == 1 && !path[0].Equals(end) ? [] : path;
        }

        /// <summary>
        /// Checks if the graph is connected (for undirected) or strongly connected (for directed)
        /// </summary>
        public bool IsConnected()
        {
            if (_vertices.Count == 0)
                return true;

            var visited = new HashSet<T>();
            var startVertex = _vertices.First();

            // DFS from first vertex
            DfsHelper(startVertex, visited);

            // For undirected graphs, check if all vertices are reachable
            if (!_isDirected)
            {
                return visited.Count == _vertices.Count;
            }

            // For directed graphs, need to check strong connectivity
            // This is a simplified check - for full strong connectivity, use Kosaraju's algorithm
            return visited.Count == _vertices.Count && CheckReverseConnectivity();
        }

        private void DfsHelper(T vertex, HashSet<T> visited)
        {
            visited.Add(vertex);
            foreach (var neighbor in GetNeighbors(vertex))
            {
                if (!visited.Contains(neighbor))
                {
                    DfsHelper(neighbor, visited);
                }
            }
        }

        private bool CheckReverseConnectivity()
        {
            // Create reverse graph and check connectivity
            var reverseGraph = new Graph<T>(true);

            foreach (var vertex in _vertices)
            {
                reverseGraph.AddVertex(vertex);
            }

            foreach (var edge in _edges)
            {
                reverseGraph.AddEdge(edge.To, edge.From, edge.Weight);
            }

            var visited = new HashSet<T>();
            var startVertex = _vertices.First();
            reverseGraph.DfsHelper(startVertex, visited);

            return visited.Count == _vertices.Count;
        }

        /// <summary>
        /// Detects if the graph has a cycle
        /// </summary>
        public bool HasCycle()
        {
            var visited = new HashSet<T>();
            var recursionStack = new HashSet<T>();

            foreach (var vertex in _vertices)
            {
                if (!visited.Contains(vertex))
                {
                    if (HasCycleHelper(vertex, visited, recursionStack))
                        return true;
                }
            }

            return false;
        }

        private bool HasCycleHelper(T vertex, HashSet<T> visited, HashSet<T> recursionStack)
        {
            visited.Add(vertex);
            recursionStack.Add(vertex);

            foreach (var neighbor in GetNeighbors(vertex))
            {
                if (!visited.Contains(neighbor))
                {
                    if (HasCycleHelper(neighbor, visited, recursionStack))
                        return true;
                }
                else if (recursionStack.Contains(neighbor))
                {
                    return true;
                }
            }

            recursionStack.Remove(vertex);
            return false;
        }

        /// <summary>
        /// Performs topological sort (for DAGs only)
        /// </summary>
        public List<T> TopologicalSort()
        {
            if (!_isDirected)
                throw new InvalidOperationException("Topological sort is only valid for directed graphs");

            if (HasCycle())
                throw new InvalidOperationException("Graph contains a cycle - topological sort not possible");

            var visited = new HashSet<T>();
            var stack = new Stack<T>();

            foreach (var vertex in _vertices)
            {
                if (!visited.Contains(vertex))
                {
                    TopologicalSortHelper(vertex, visited, stack);
                }
            }

            return [.. stack];
        }

        private void TopologicalSortHelper(T vertex, HashSet<T> visited, Stack<T> stack)
        {
            visited.Add(vertex);

            foreach (var neighbor in GetNeighbors(vertex))
            {
                if (!visited.Contains(neighbor))
                {
                    TopologicalSortHelper(neighbor, visited, stack);
                }
            }

            stack.Push(vertex);
        }
        #endregion

        #region Utility Methods
        /// <summary>
        /// Clears all vertices and edges from the graph
        /// </summary>
        public void Clear()
        {
            _vertices.Clear();
            _adjacencyList.Clear();
            _edges.Clear();
        }

        /// <summary>
        /// Creates a copy of the graph
        /// </summary>
        public Graph<T> Clone()
        {
            var clone = new Graph<T>(_isDirected);

            foreach (var vertex in _vertices)
            {
                clone.AddVertex(vertex);
            }

            foreach (var edge in _edges)
            {
                clone.AddEdge(edge.From, edge.To, edge.Weight);
            }

            return clone;
        }

        /// <summary>
        /// Returns a string representation of the graph
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Graph ({(_isDirected ? "Directed" : "Undirected")}):");
            sb.AppendLine($"Vertices: {VertexCount}, Edges: {EdgeCount}");

            foreach (var vertex in _vertices.OrderBy(v => v))
            {
                sb.AppendLine($"{vertex}: [{string.Join(", ", GetNeighbors(vertex).OrderBy(n => n))}]");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Exports graph to DOT format for visualization
        /// </summary>
        public string ToDotFormat()
        {
            var sb = new StringBuilder();
            string graphType = _isDirected ? "digraph" : "graph";
            string edgeSymbol = _isDirected ? " -> " : " -- ";

            sb.AppendLine($"{graphType} G {{");

            foreach (var edge in _edges)
            {
                sb.AppendLine($"    {edge.From}{edgeSymbol}{edge.To} [label=\"{edge.Weight}\"];");
            }

            sb.AppendLine("}");
            return sb.ToString();
        }
        #endregion
    }

}
