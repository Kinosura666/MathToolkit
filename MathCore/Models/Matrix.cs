using MathCore.Extentions;
using MathNet.Numerics.LinearAlgebra;
using System.Text;

namespace MathCore.Models
{
    public class Matrix
    {
        public double[,] Data { get; }

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

        #region Basic Operations
        public override string ToString()
        {
            var sb = new StringBuilder(Rows * Columns * 8);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
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

        #endregion

        #region Eigenvalue Methods
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

            return (eigenvalue, v.ToArray());
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

        public (double Eigenvalue, double[] Eigenvector) RayleighQuotientIteration(
            int maxIterations = 100,
            double tolerance = 1e-10,
            double? initialGuess = null)
        {
            if (Rows != Columns)
                throw new InvalidOperationException("Rayleigh quotient iteration requires a square matrix.");

            var A = this.ToMathNet();
            int n = Rows;

            var v = Vector<double>.Build.Dense(n, 1.0).Normalize(2);

            double lambda = initialGuess ?? v.DotProduct(A * v);

            for (int iter = 0; iter < maxIterations; iter++)
            {
                var identity = Matrix<double>.Build.DenseIdentity(n);
                var shifted = A - identity.Multiply(lambda);

                Vector<double> x;
                try
                {
                    var solver = shifted.LU();
                    x = solver.Solve(v);
                }
                catch
                {
                    throw new InvalidOperationException("Matrix (A - λI) is singular or near-singular.");
                }

                var newV = x.Normalize(2);
                var newLambda = newV.DotProduct(A * newV);

                if (Math.Abs(newLambda - lambda) < tolerance)
                    return (newLambda, newV.ToArray());

                lambda = newLambda;
                v = newV;
            }

            return (lambda, v.ToArray());
        }

        public ((double Center, double Radius)[] Discs, double Min, double Max) GershgorinDiscs()
        {
            var discs = new (double Center, double Radius)[Rows];
            var globalMin = double.PositiveInfinity;
            var globalMax = double.NegativeInfinity;


            for (int i = 0; i < Rows; i++)
            {
                double center = Data[i, i];
                double radius = 0;

                for (int j = 0; j < Columns; j++)
                {
                    if (i != j)
                        radius += Math.Abs(Data[i, j]);
                }

                discs[i] = (center, radius);
                double left = center - radius;
                double right = center + radius;

                if (left < globalMin) globalMin = left;
                if (right > globalMax) globalMax = right;
            }
            return (discs, globalMin, globalMax);
        }

        public (double[] Eigenvalues, double[,] Eigenvectors) JacobiEigenSolver(double tolerance = 1e-10, int maxIterations = 100)
        {
            if (Rows != Columns)
                throw new InvalidOperationException("Jacobi method requires a square matrix.");

            int n = Rows;
            double[,] A = (double[,])Data.Clone();
            double[,] V = new double[n, n];

            for (int i = 0; i < n; i++)
                V[i, i] = 1.0;

            for (int iter = 0; iter < maxIterations; iter++)
            {
                int p = 0, q = 1;
                double max = Math.Abs(A[p, q]);

                for (int i = 0; i < n; i++)
                {
                    for (int j = i + 1; j < n; j++)
                    {
                        if (Math.Abs(A[i, j]) > max)
                        {
                            max = Math.Abs(A[i, j]);
                            p = i; q = j;
                        }
                    }
                }
                if (max < tolerance)
                    break;

                double θ = 0.5 * Math.Atan2(2 * A[p, q], A[q, q] - A[p, p]);
                double cos = Math.Cos(θ);
                double sin = Math.Sin(θ);

                double[,] Anew = (double[,])A.Clone();

                for (int i = 0; i < n; i++)
                {
                    if (i != p && i != q)
                    {
                        Anew[i, p] = Anew[p, i] = cos * A[i, p] - sin * A[i, q];
                        Anew[i, q] = Anew[q, i] = sin * A[i, p] + cos * A[i, q];
                    }
                }

                double app = cos * cos * A[p, p] - 2 * sin * cos * A[p, q] + sin * sin * A[q, q];
                double aqq = sin * sin * A[p, p] + 2 * sin * cos * A[p, q] + cos * cos * A[q, q];

                Anew[p, p] = app;
                Anew[q, q] = aqq;
                Anew[p, q] = Anew[q, p] = 0.0;

                A = Anew;

                for (int i = 0; i < n; i++)
                {
                    double vip = V[i, p];
                    double viq = V[i, q];
                    V[i, p] = cos * vip - sin * viq;
                    V[i, q] = sin * vip + cos * viq;
                }

            }

            var eigenvalues = new double[n];
            for (int i = 0; i < n; i++)
                eigenvalues[i] = A[i, i];

            return (eigenvalues, V);
        }
    }


    #endregion
}
