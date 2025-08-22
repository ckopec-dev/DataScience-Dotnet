using System.Text;

namespace Core.LinearAlgebra
{
    // Matrix class for m×n matrices
    public class Matrix
    {
        private readonly double[,] _elements;

        public int Rows { get; }
        public int Cols { get; }
        public bool IsSquare => Rows == Cols;

        // Constructors
        public Matrix(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            _elements = new double[rows, cols];
        }

        public Matrix(double[,] elements)
        {
            Rows = elements.GetLength(0);
            Cols = elements.GetLength(1);
            _elements = (double[,])elements.Clone();
        }

        // Indexer
        public double this[int row, int col]
        {
            get => _elements[row, col];
            set => _elements[row, col] = value;
        }

        // Static factory methods
        public static Matrix Zero(int rows, int cols) => new(rows, cols);
        public static Matrix Identity(int size)
        {
            var m = new Matrix(size, size);
            for (int i = 0; i < size; i++)
                m[i, i] = 1.0;
            return m;
        }

        public static Matrix Ones(int rows, int cols)
        {
            var m = new Matrix(rows, cols);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    m[i, j] = 1.0;
            return m;
        }

        public static Matrix Random(int rows, int cols, Random rng = null!)
        {
            rng ??= new Random();
            var m = new Matrix(rows, cols);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    m[i, j] = rng.NextDouble() * 2 - 1; // Range [-1, 1]
            return m;
        }

        // Matrix operations
        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.Rows != b.Rows || a.Cols != b.Cols)
                throw new LinearAlgebraException("Matrix dimensions must match");

            var result = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < a.Cols; j++)
                    result[i, j] = a[i, j] + b[i, j];
            return result;
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a.Rows != b.Rows || a.Cols != b.Cols)
                throw new LinearAlgebraException("Matrix dimensions must match");

            var result = new Matrix(a.Rows, a.Cols);
            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < a.Cols; j++)
                    result[i, j] = a[i, j] - b[i, j];
            return result;
        }

        public static Matrix operator *(double scalar, Matrix m)
        {
            var result = new Matrix(m.Rows, m.Cols);
            for (int i = 0; i < m.Rows; i++)
                for (int j = 0; j < m.Cols; j++)
                    result[i, j] = scalar * m[i, j];
            return result;
        }

        public static Matrix operator *(Matrix m, double scalar) => scalar * m;

        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.Cols != b.Rows)
                throw new LinearAlgebraException($"Cannot multiply {a.Rows}×{a.Cols} matrix by {b.Rows}×{b.Cols} matrix");

            var result = new Matrix(a.Rows, b.Cols);
            for (int i = 0; i < a.Rows; i++)
                for (int j = 0; j < b.Cols; j++)
                    for (int k = 0; k < a.Cols; k++)
                        result[i, j] += a[i, k] * b[k, j];
            return result;
        }

        public static Vector operator *(Matrix m, Vector v)
        {
            if (m.Cols != v.Size)
                throw new LinearAlgebraException($"Cannot multiply {m.Rows}×{m.Cols} matrix by vector of size {v.Size}");

            var result = new Vector(m.Rows);
            for (int i = 0; i < m.Rows; i++)
                for (int j = 0; j < m.Cols; j++)
                    result[i] += m[i, j] * v[j];
            return result;
        }

        // Matrix properties and operations
        public Matrix Transpose()
        {
            var result = new Matrix(Cols, Rows);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    result[j, i] = _elements[i, j];
            return result;
        }

        public double Trace()
        {
            if (!IsSquare) throw new LinearAlgebraException("Trace is only defined for square matrices");
            double sum = 0;
            for (int i = 0; i < Rows; i++)
                sum += _elements[i, i];
            return sum;
        }

        public double Determinant()
        {
            if (!IsSquare) throw new LinearAlgebraException("Determinant is only defined for square matrices");
            return ComputeDeterminant(_elements);
        }

        private static double ComputeDeterminant(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            if (n == 1) return matrix[0, 0];
            if (n == 2) return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];

            double det = 0;
            for (int j = 0; j < n; j++)
            {
                det += matrix[0, j] * Math.Pow(-1, j) * ComputeDeterminant(GetMinor(matrix, 0, j));
            }
            return det;
        }

        private static double[,] GetMinor(double[,] matrix, int row, int col)
        {
            int n = matrix.GetLength(0);
            var minor = new double[n - 1, n - 1];
            int minorRow = 0;
            for (int i = 0; i < n; i++)
            {
                if (i == row) continue;
                int minorCol = 0;
                for (int j = 0; j < n; j++)
                {
                    if (j == col) continue;
                    minor[minorRow, minorCol] = matrix[i, j];
                    minorCol++;
                }
                minorRow++;
            }
            return minor;
        }

        // Matrix decompositions and advanced operations
        public Matrix Inverse()
        {
            if (!IsSquare) throw new LinearAlgebraException("Inverse is only defined for square matrices");

            double det = Determinant();
            if (Math.Abs(det) < double.Epsilon) throw new LinearAlgebraException("Matrix is singular (not invertible)");

            return (1.0 / det) * Adjugate();
        }

        public Matrix Adjugate()
        {
            if (!IsSquare) throw new LinearAlgebraException("Adjugate is only defined for square matrices");

            var adj = new Matrix(Rows, Cols);
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    adj[i, j] = Math.Pow(-1, i + j) * ComputeDeterminant(GetMinor(_elements, i, j));

            return adj.Transpose();
        }

        // LU Decomposition
        public (Matrix L, Matrix U) LUDecomposition()
        {
            if (!IsSquare) throw new LinearAlgebraException("LU decomposition requires a square matrix");

            int n = Rows;
            var L = Identity(n);
            var U = new Matrix(n, n);

            // Copy original matrix to U
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    U[i, j] = _elements[i, j];

            // Perform decomposition
            for (int i = 0; i < n; i++)
            {
                for (int k = i + 1; k < n; k++)
                {
                    if (Math.Abs(U[i, i]) < double.Epsilon)
                        throw new LinearAlgebraException("Matrix is singular");

                    L[k, i] = U[k, i] / U[i, i];
                    for (int j = i; j < n; j++)
                        U[k, j] -= L[k, i] * U[i, j];
                }
            }

            return (L, U);
        }

        // QR Decomposition using Gram-Schmidt process
        public (Matrix Q, Matrix R) QRDecomposition()
        {
            var Q = new Matrix(Rows, Cols);
            var R = new Matrix(Cols, Cols);

            // Extract columns as vectors
            var columns = new Vector[Cols];
            for (int j = 0; j < Cols; j++)
            {
                columns[j] = new Vector(Rows);
                for (int i = 0; i < Rows; i++)
                    columns[j][i] = _elements[i, j];
            }

            // Gram-Schmidt orthogonalization
            var orthogonal = new Vector[Cols];
            for (int j = 0; j < Cols; j++)
            {
                orthogonal[j] = new Vector(columns[j].Elements);

                for (int k = 0; k < j; k++)
                {
                    R[k, j] = orthogonal[k].Dot(columns[j]);
                    orthogonal[j] = orthogonal[j] - R[k, j] * orthogonal[k];
                }

                R[j, j] = orthogonal[j].Magnitude;
                if (R[j, j] < double.Epsilon)
                    throw new LinearAlgebraException("Matrix columns are linearly dependent");

                orthogonal[j] = orthogonal[j].Normalize();
            }

            // Fill Q matrix
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    Q[i, j] = orthogonal[j][i];

            return (Q, R);
        }

        // Get row as vector
        public Vector GetRow(int row)
        {
            var result = new Vector(Cols);
            for (int j = 0; j < Cols; j++)
                result[j] = _elements[row, j];
            return result;
        }

        // Get column as vector
        public Vector GetColumn(int col)
        {
            var result = new Vector(Rows);
            for (int i = 0; i < Rows; i++)
                result[i] = _elements[i, col];
            return result;
        }

        // Set row from vector
        public void SetRow(int row, Vector v)
        {
            if (v.Size != Cols) throw new LinearAlgebraException("Vector size must match number of columns");
            for (int j = 0; j < Cols; j++)
                _elements[row, j] = v[j];
        }

        // Set column from vector
        public void SetColumn(int col, Vector v)
        {
            if (v.Size != Rows) throw new LinearAlgebraException("Vector size must match number of rows");
            for (int i = 0; i < Rows; i++)
                _elements[i, col] = v[i];
        }

        // Frobenius norm
        public double FrobeniusNorm()
        {
            double sum = 0;
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Cols; j++)
                    sum += _elements[i, j] * _elements[i, j];
            return Math.Sqrt(sum);
        }

        // String representation
        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < Rows; i++)
            {
                sb.Append('[');
                for (int j = 0; j < Cols; j++)
                {
                    sb.Append(_elements[i, j].ToString("F3"));
                    if (j < Cols - 1) sb.Append(", ");
                }
                sb.AppendLine("]");
            }
            return sb.ToString();
        }
    }
}
