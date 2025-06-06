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

        public Matrix Add(Matrix other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (Rows != other.Rows || Columns != other.Columns)
                throw new InvalidOperationException("Matrix dimensions must match for addition.");
            
            return this.ToMathNet()
                        .Add(other.ToMathNet())
                        .ToCore();
        }

        public Matrix Subtract(Matrix other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (Rows != other.Rows || Columns != other.Columns)
                throw new InvalidOperationException("Matrix dimensions must match for subtraction.");

            return this.ToMathNet()
                       .Subtract(other.ToMathNet())
                       .ToCore();
        }

        public Matrix Multiply(Matrix other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (Columns != other.Rows)
                throw new InvalidOperationException(
                    $"Matrix dimensions do not match for multiplication: A({Rows}x{Columns}) * B({other.Rows}x{other.Columns})");

            return this.ToMathNet()
                       .Multiply(other.ToMathNet())
                       .ToCore();
        }

        public Matrix Transpose() =>
            this.ToMathNet()
                .Transpose()
                .ToCore();

        public Matrix Inverse()
        {
            if (Rows != Columns)
                throw new InvalidOperationException("Inverse is only defined for square matrices.");

            var mathNetMatrix = this.ToMathNet();

            if (Math.Abs(mathNetMatrix.Determinant()) < 1e-12)
                throw new InvalidOperationException("Matrix is singular and cannot be inverted.");

            return mathNetMatrix
                   .Inverse()
                   .ToCore();
        }

        public double Determinant()
        {
            if (Rows != Columns)
                throw new InvalidOperationException("Determinant is only defined for square matrices.");

            return this.ToMathNet().Determinant();
        }

        public (double Eigenvalue, double[] Eigenvector) PowerIteration(int maxIterations = 1000, double tolerance = 1e-10)
        {
            if (Rows != Columns)
                throw new InvalidOperationException("Power iteration requires a square matrix.");

            int n = Rows;

            var v = Vector<double>.Build.Dense(n, 1.0);
            v = v.Normalize(2);

            var A = this.ToMathNet();
            double eigenvalue = 0;

            for (int iter = 0; iter < maxIterations; iter++)
            {
                var Av = A * v;
                var newEigenvalue = Av.DotProduct(v);

                var newV = Av.Normalize(2);

                if (Math.Abs(newEigenvalue - eigenvalue) < tolerance)
                    return (newEigenvalue, newV.ToArray());

                v = newV;
                eigenvalue = newEigenvalue;
            }

            return (eigenvalue,v.ToArray());
        }

        public (double Eigenvalue, double[] Eigenvector) InversePowerIteration(int maxIterations = 1000, double tolerance = 1e-10)
        {
            if (Rows != Columns)
                throw new InvalidOperationException("Inverse power iteration requires a square matrix.");

            var A = this.ToMathNet();
            var lu = A.LU();
            int n = Rows;

            var v = Vector<double>.Build.Dense(n, 1.0).Normalize(2);
            double eigenvalue = 0;

            for (int iter = 0; iter < maxIterations; iter++)
            {
                var x = lu.Solve(v);
                var newV = x.Normalize(2);

                var newEigenvalue = 1.0 / newV.DotProduct(v);

                if (Math.Abs(newEigenvalue - eigenvalue) < tolerance)
                    return (newEigenvalue, newV.ToArray());

                v = newV;
                eigenvalue = newEigenvalue;
            }

            return (eigenvalue, v.ToArray());
        }


    }
}
