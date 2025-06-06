using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathCore.Extentions;
using MathNet.Numerics.LinearAlgebra;

namespace MathCore.Models
{
    public class Matrix
    {
        public double[,] Data {  get; }

        public int Rows => Data.GetLength(0);
        public int Columns => Data.GetLength(1);

        private static readonly Random _random = new();

        public Matrix(double[,] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (data.GetLength(0) == 0 || data.GetLength(1) == 0)
                throw new ArgumentException("Matrix cannot have zero rows or columns.");

            Data = data;
        }

        public override string ToString()
        {
            var sb = new StringBuilder(Rows * Columns * 8);
            for (int i = 0; i < Rows;i++)
            {
                for(int j = 0; j < Columns; j++)
                {
                    sb.Append($"{Data[i, j],8:F2}");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public static Matrix Generate(int rows, int cols, double min = 0, double max = 10)
        {
            if (rows <= 0 || cols <= 0)
                throw new ArgumentException("Matrix dimensions must be greater than zero.");

            var data = new double[rows, cols];
            double range = max - min;

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    data[i, j] = _random.NextDouble() * range + min;

            return new Matrix(data);
        }

        public Matrix Add(Matrix other) =>
            this.ToMathNet()
                .Add(other
                .ToMathNet())
                .ToCore();

        public Matrix Multiply(Matrix other) => 
            this.ToMathNet()
                .Multiply(other
                .ToMathNet())
                .ToCore();

        public Matrix Subtract(Matrix other) => 
            this.ToMathNet()
                .Subtract(other
                .ToMathNet())
                .ToCore();

        public Matrix Transpose() => 
            this.ToMathNet()
                .Transpose()
                .ToCore();


        public double Determinant()
        {
            if (Rows != Columns)
                throw new InvalidOperationException("Детермінант визначається лише для квадратних матриць.");

            var mathnetMatrix = MathMatrixAdapter.ToMathNet(this);
            return mathnetMatrix.Determinant();
        }

    }
}
