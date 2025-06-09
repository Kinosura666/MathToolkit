using MathCore.Extentions;
using MathNet.Numerics.LinearAlgebra;
using System.Data;
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

        public Matrix PseudoInverse()
        {
            var A = this.ToMathNet();
            var pinv = A.PseudoInverse();
            return pinv.ToCore();
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

        public (double Eigenvalue, double[] Eigenvector) InversePowerIteration(
            double eps = 1e-6, int maxIterations = 1000)
        {
            if (Rows != Columns)
                throw new InvalidOperationException("Matrix must be square.");

            int n = Rows;
            var A = this.ToMathNet();
            var solver = A.LU();

            var x = Vector<double>.Build.Dense(n, 1.0).Normalize(2);
            Vector<double> y;
            double lambdaOld = 0, lambdaNew = 0;

            for (int iter = 0; iter < maxIterations; iter++)
            {
                y = solver.Solve(x);
                y = y.Normalize(2);

                lambdaNew = y.DotProduct(A * y);

                if (Math.Abs(lambdaNew - lambdaOld) < eps)
                    break;

                lambdaOld = lambdaNew;
                x = y;
            }

            return (lambdaNew, x.ToArray());
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

        public double[] QREigenValues(int maxIterations = 1000, double tolerance = 1e-10)
        {
            if (Rows != Columns)
                throw new InvalidOperationException("QR algorithm requires a square matrix.");

            int n = Rows;
            var A = Matrix<double>.Build.DenseOfArray(Data); 

            for (int iter = 0; iter < maxIterations; iter++)
            {
                var qr = A.QR();
                var Q = qr.Q;
                var R = qr.R;

                A = R * Q; 

                bool converged = true;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (Math.Abs(A[i, j]) > tolerance)
                        {
                            converged = false;
                            break;
                        }
                    }
                }

                if (converged)
                    break;
            }

            var eigenvalues = new double[n];
            for (int i = 0; i < n; i++)
                eigenvalues[i] = A[i, i];

            return eigenvalues;
        }

        public double[] LREigenValues(int maxIterations = 1000, double epsilon = 1e-6)
        {
            if (Rows != Columns)
                throw new InvalidOperationException("Matrix must be square.");

            int n = Rows;
            double[,] A_k = (double[,])Data.Clone();
            double[,] A_next = new double[n, n];
            double[,] L = new double[n, n];
            double[,] R = new double[n, n];

            for (int iter = 0; iter < maxIterations; iter++)
            {
                Array.Clear(L, 0, L.Length);
                Array.Clear(R, 0, R.Length);

                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        R[i, j] = A_k[i, j];

                for (int i = 0; i < n; i++)
                    L[i, i] = 1.0;

                for (int k = 0; k < n - 1; k++)
                {
                    for (int i = k + 1; i < n; i++)
                    {
                        double factor = R[i, k] / R[k, k];
                        L[i, k] = factor;
                        for (int j = k; j < n; j++)
                        {
                            R[i, j] -= factor * R[k, j];
                        }
                    }
                }

                A_next = new double[n, n];
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        for (int k = 0; k < n; k++)
                            A_next[i, j] += R[i, k] * L[k, j];

                double norm = 0.0;
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                    {
                        double diff = A_k[i, j] - A_next[i, j];
                        norm += diff * diff;
                    }

                if (Math.Sqrt(norm) < epsilon)
                    break;

                Array.Copy(A_next, A_k, A_k.Length);
            }

            var eigenvalues = new double[n];
            for (int i = 0; i < n; i++)
                eigenvalues[i] = A_k[i, i];

            return eigenvalues;
        }

        public double[] LeverrierFaddeev()
        {
            if (Rows != Columns)
                throw new InvalidOperationException("Matrix must be square.");

            int n = Rows;
            var A = this.ToMathNet();
            var I = Matrix<double>.Build.DenseIdentity(n);

            var B = I.Clone();
            double[] coeffs = new double[n + 1];
            coeffs[0] = 1.0;

            for(int k = 1;k <= n; k++)
            {
                double trace = (A*B).Trace();
                double a_k = -trace / k;
                coeffs[k] = a_k;

                if (k < n)
                {
                    B = A * B + a_k * I;
                }
            }

            return coeffs;
        }

        public double[] KrylovCharacteristicPolynomial()
        {
            if (Rows != Columns)
                throw new InvalidOperationException("Matrix must be square.");

            int n = Rows;
            var A = this.ToMathNet();

            var v = Vector<double>.Build.Dense(n);
            v[n - 1] = 1.0;

            var krylovMatrix = Matrix<double>.Build.Dense(n, n);
            var current = v.Clone();
            for (int i = 0; i < n; i++)
            {
                krylovMatrix.SetColumn(i, current);
                current = A * current;
            }

            var rhs = -current;

            var coeffsReversed = krylovMatrix.Solve(rhs);

            var coeffs = new double[n + 1];
            coeffs[0] = 1.0; 
            for (int i = 0; i < n; i++)
                coeffs[i + 1] = coeffsReversed[n - 1 - i];

            return coeffs;
        }

        #endregion

        #region Matrix Stats (norm, cond)

        public double FrobeniusNorm()
        {
            double sum = 0.0;
            for (int i = 0; i < Rows; i++)
                for (int j = 0; j < Columns; j++)
                    sum += Data[i, j] * Data[i, j];
            return Math.Sqrt(sum);
        }

        public double InfinityNorm()
        {
            double max = 0.0;
            for (int i = 0; i < Rows; i++)
            {
                double rowSum = 0.0;
                for (int j = 0; j < Columns; j++)
                    rowSum += Math.Abs(Data[i, j]);
                if (rowSum > max) max = rowSum;
            }
            return max;
        }

        public double OneNorm()
        {
            double max = 0.0;
            for (int j = 0; j < Columns; j++)
            {
                double colSum = 0.0;
                for (int i = 0; i < Rows; i++)
                    colSum += Math.Abs(Data[i, j]);
                if (colSum > max) max = colSum;
            }
            return max;
        }

        public double TwoNorm()
        {
            var A = this.ToMathNet();
            var AtA = A.TransposeThisAndMultiply(A);
            var eigen = AtA.Evd(); 
            return Math.Sqrt(eigen.EigenValues.Real().Maximum());
        } 

        public double ConditionNumber2()
        {
            if (Rows != Columns)
                throw new InvalidOperationException("Condition number is defined for square matrices only.");

            var A = this.ToMathNet();
            var AtA = A.TransposeThisAndMultiply(A);
            var evd = AtA.Evd();

            var eigenvalues = evd.EigenValues
                                 .Select(c => c.Real)
                                 .Where(x => x > 1e-12) 
                                 .ToArray();

            if (eigenvalues.Length == 0)
                throw new InvalidOperationException("Matrix appears to be singular or nearly singular.");

            double max = eigenvalues.Max();
            double min = eigenvalues.Min();

            return Math.Sqrt(max / min);
        }

        public double[] GetSingularValues()
        {
            var A = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.OfArray(this.Data);
            var svd = A.Svd();
            return svd.S.ToArray();
        }

        #endregion

        #region Decompositions

        public (Matrix L, Matrix U) LUDecomposition()
        {
            if (Rows != Columns) 
                throw new InvalidOperationException("LU decomposition requires a square matrix.");
            
            int n = Rows;
            var L = new double[n,n];
            var U = new double[n,n];
            var A = this.Data;

            for(int i = 0; i < n; i++)
            {
                for (int k = i; k < n; k++)
                {
                    double sum = 0;
                    for (int j = 0; j < i; j++)
                        sum += L[i, j] * U[j, k];

                    U[i, k] = A[i, k] - sum;
                }

                for (int k = i; k < n; k++)
                {
                    if (i == k)
                        L[i, i] = 1.0; 
                    else
                    {
                        double sum = 0;
                        for (int j = 0; j < i; j++)
                            sum += L[k, j] * U[j, i];

                        if (U[i, i] == 0)
                            throw new InvalidOperationException("Zero pivot encountered. LU decomposition without pivoting failed.");

                        L[k, i] = (A[k, i] - sum) / U[i, i];
                    }
                }
            }
            return (new Matrix(L), new Matrix(U));
        }

        public (Matrix Q, Matrix R) QRDecomposition()
        {
            if (Rows != Columns)
                throw new InvalidOperationException("QR decomposition requires a square matrix.");

            var A = Matrix<double>.Build.DenseOfArray(Data);
            var qr = A.QR();

            var Q = new Matrix(qr.Q.ToArray());
            var R = new Matrix(qr.R.ToArray());

            return (Q, R);
        }

        public (Matrix L, Matrix LT) CholeskyDecomposition()
        {
            if (Rows != Columns)
                throw new InvalidOperationException("Cholesky decomposition requires a square matrix.");

            int n = Rows;
            var L = new double[n, n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    double sum = 0.0;

                    for (int k = 0; k < j; k++)
                        sum += L[i, k] * L[j, k];

                    if (i == j)
                    {
                        double value = Data[i, i] - sum;
                        if (value <= 0)
                            throw new InvalidOperationException("Matrix is not positive definite.");
                        L[i, j] = Math.Sqrt(value);
                    }
                    else
                    {
                        L[i, j] = (Data[i, j] - sum) / L[j, j];
                    }
                }
            }

            var LT = new double[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    LT[i, j] = L[j, i];

            return (new Matrix(L), new Matrix(LT));
        }

        public (Matrix U, Matrix S, Matrix VT) SVD()
        {
            var A = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.OfArray(this.Data);
            var svd = A.Svd();

            var U = new Matrix(svd.U.ToArray());
            var S = new Matrix(svd.W.ToArray());   
            var VT = new Matrix(svd.VT.ToArray());

            return (U, S, VT);
        }

        #endregion
    }
}
