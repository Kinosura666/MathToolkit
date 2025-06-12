using MathCore.Interfaces;
using MathCore.Libraries;

namespace MathCore.Mappers
{
    public class MatrixMapper : IMatrixMapper
    {
        public Matrix FromJagged(double[][] data)
        {
            int rows = data.Length;
            int cols = data[0].Length;

            var result = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                if (data[i].Length != cols)
                    throw new ArgumentException("All rows must have the same number of columns.");

                for (int j = 0; j < cols; j++)
                    result[i, j] = data[i][j];
            }

            return new Matrix(result);
        }

        public double[][] ToJagged(Matrix matrix)
        {
            int rows = matrix.Rows;
            int cols = matrix.Columns;
            var data = matrix.Data;

            var jagged = new double[rows][];
            for (int i = 0; i < rows; i++)
            {
                jagged[i] = new double[cols];
                for (int j = 0; j < cols; j++)
                    jagged[i][j] = data[i, j];
            }

            return jagged;
        }
    }
}
