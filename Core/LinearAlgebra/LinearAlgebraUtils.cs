
namespace Core.LinearAlgebra
{
    // Utility class for common linear algebra operations
    public static class LinearAlgebraUtils
    {
        // Solve linear system Ax = b using Gaussian elimination
        public static Vector SolveLinearSystem(Matrix A, Vector b)
        {
            if (A.Rows != A.Cols) throw new LinearAlgebraException("Matrix must be square");
            if (A.Rows != b.Size) throw new LinearAlgebraException("Matrix rows must match vector size");

            // Create augmented matrix
            var augmented = new Matrix(A.Rows, A.Cols + 1);
            for (int i = 0; i < A.Rows; i++)
            {
                for (int j = 0; j < A.Cols; j++)
                    augmented[i, j] = A[i, j];
                augmented[i, A.Cols] = b[i];
            }

            // Forward elimination
            for (int i = 0; i < A.Rows - 1; i++)
            {
                // Find pivot
                int maxRow = i;
                for (int k = i + 1; k < A.Rows; k++)
                {
                    if (Math.Abs(augmented[k, i]) > Math.Abs(augmented[maxRow, i]))
                        maxRow = k;
                }

                // Swap rows
                for (int j = 0; j < A.Cols + 1; j++)
                {
                    (augmented[maxRow, j], augmented[i, j]) = (augmented[i, j], augmented[maxRow, j]);
                }

                // Eliminate
                for (int k = i + 1; k < A.Rows; k++)
                {
                    if (Math.Abs(augmented[i, i]) < double.Epsilon)
                        throw new LinearAlgebraException("Matrix is singular");

                    double factor = augmented[k, i] / augmented[i, i];
                    for (int j = i; j < A.Cols + 1; j++)
                        augmented[k, j] -= factor * augmented[i, j];
                }
            }

            // Back substitution
            var x = new Vector(A.Rows);
            for (int i = A.Rows - 1; i >= 0; i--)
            {
                x[i] = augmented[i, A.Cols];
                for (int j = i + 1; j < A.Cols; j++)
                    x[i] -= augmented[i, j] * x[j];

                if (Math.Abs(augmented[i, i]) < double.Epsilon)
                    throw new LinearAlgebraException("Matrix is singular");

                x[i] /= augmented[i, i];
            }

            return x;
        }

        // Compute eigenvalues using QR algorithm (simplified version)
        public static double[] ComputeEigenvalues(Matrix matrix, int maxIterations = 100)
        {
            if (!matrix.IsSquare) throw new LinearAlgebraException("Eigenvalues are only defined for square matrices");

            var A = new Matrix(matrix.Rows, matrix.Cols);
            for (int i = 0; i < matrix.Rows; i++)
                for (int j = 0; j < matrix.Cols; j++)
                    A[i, j] = matrix[i, j];

            // QR algorithm
            for (int iter = 0; iter < maxIterations; iter++)
            {
                var (Q, R) = A.QRDecomposition();
                A = R * Q;
            }

            // Extract eigenvalues from diagonal
            var eigenvalues = new double[matrix.Rows];
            for (int i = 0; i < matrix.Rows; i++)
                eigenvalues[i] = A[i, i];

            return eigenvalues;
        }

        // Compute matrix rank using row reduction
        public static int ComputeRank(Matrix matrix)
        {
            var temp = new Matrix(matrix.Rows, matrix.Cols);
            for (int i = 0; i < matrix.Rows; i++)
                for (int j = 0; j < matrix.Cols; j++)
                    temp[i, j] = matrix[i, j];

            int rank = 0;
            for (int col = 0, row = 0; col < matrix.Cols && row < matrix.Rows; col++)
            {
                // Find pivot
                int pivot = row;
                for (int i = row + 1; i < matrix.Rows; i++)
                {
                    if (Math.Abs(temp[i, col]) > Math.Abs(temp[pivot, col]))
                        pivot = i;
                }

                if (Math.Abs(temp[pivot, col]) < double.Epsilon)
                    continue;

                // Swap rows
                for (int j = 0; j < matrix.Cols; j++)
                {
                    (temp[pivot, j], temp[row, j]) = (temp[row, j], temp[pivot, j]);
                }

                // Eliminate
                for (int i = 0; i < matrix.Rows; i++)
                {
                    if (i != row)
                    {
                        double factor = temp[i, col] / temp[row, col];
                        for (int j = 0; j < matrix.Cols; j++)
                            temp[i, j] -= factor * temp[row, j];
                    }
                }

                rank++;
                row++;
            }

            return rank;
        }
    }
}
