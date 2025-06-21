using MathCore.Models;
using MathCore.Models.MatrixResults;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathCore.Extentions;

namespace MathCore.Libraries.MatrixCore
{
    public static class MatrixEigen
    {
        public static EigenIterationResult PowerIteration(MatrixModel A, int maxIterations = 1000, double tolerance = 1e-10)
        {
            if (A.Rows != A.Columns)
                throw new InvalidOperationException("Power iteration requires a square matrix.");

            int n = A.Rows;
            var v = Vector<double>.Build.Dense(n, 1.0).Normalize(2);
            var matrix = A.ToMathNet();
            double eigenvalue = 0;

            for (int iter = 0; iter < maxIterations; iter++)
            {
                var Av = matrix * v;
                var newEigenvalue = Av.DotProduct(v);
                var newV = Av.Normalize(2);

                if (Math.Abs(newEigenvalue - eigenvalue) < tolerance)
                {
                    return new EigenIterationResult
                    {
                        Eigenvalue = newEigenvalue,
                        Eigenvector = newV.ToArray(),
                        Iterations = iter + 1,
                        Converged = true
                    };
                }

                v = newV;
                eigenvalue = newEigenvalue;
            }

            return new EigenIterationResult
            {
                Eigenvalue = eigenvalue,
                Eigenvector = v.ToArray(),
                Iterations = maxIterations,
                Converged = false
            };
        }

        public static EigenIterationResult InversePowerIteration(MatrixModel A, double tolerance = 1e-6, int maxIterations = 1000)
        {
            if (A.Rows != A.Columns)
                throw new InvalidOperationException("Matrix must be square.");

            int n = A.Rows;
            var matrix = A.ToMathNet();

            var solver = matrix.LU();
            var x = Vector<double>.Build.Dense(n, 1.0).Normalize(2);
            Vector<double> y;
            double lambdaOld = 0, lambdaNew = 0;

            for (int iter = 0; iter < maxIterations; iter++)
            {
                y = solver.Solve(x).Normalize(2);
                lambdaNew = y.DotProduct(matrix * y);

                if (Math.Abs(lambdaNew - lambdaOld) < tolerance)
                {
                    return new EigenIterationResult
                    {
                        Eigenvalue = lambdaNew,
                        Eigenvector = y.ToArray(),
                        Iterations = iter + 1,
                        Converged = true
                    };
                }

                lambdaOld = lambdaNew;
                x = y;
            }

            return new EigenIterationResult
            {
                Eigenvalue = lambdaNew,
                Eigenvector = x.ToArray(),
                Iterations = maxIterations,
                Converged = false
            };
        }

        public static EigenIterationResult RayleighQuotientIteration(MatrixModel A, int maxIterations = 100,
            double tolerance = 1e-10,
            double? initialGuess = null)
        {
            if (A.Rows != A.Columns)
                throw new InvalidOperationException("Rayleigh quotient iteration requires a square matrix.");

            var matrix = A.ToMathNet();
            int n = A.Rows;

            var v = Vector<double>.Build.Dense(n, 1.0).Normalize(2);

            double lambda = initialGuess ?? v.DotProduct(matrix * v);

            for (int iter = 0; iter < maxIterations; iter++)
            {
                var identity = Matrix<double>.Build.DenseIdentity(n);
                var shifted = matrix - identity.Multiply(lambda);

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
                var newLambda = newV.DotProduct(matrix * newV);

                if (Math.Abs(newLambda - lambda) < tolerance)
                {
                    return new EigenIterationResult
                    {
                        Eigenvalue = newLambda,
                        Eigenvector = newV.ToArray(),
                        Iterations = iter + 1,
                        Converged = true
                    };
                }

                lambda = newLambda;
                v = newV;
            }

            return new EigenIterationResult
            {
                Eigenvalue = lambda,
                Eigenvector = v.ToArray(),
                Iterations = maxIterations,
                Converged = false
            };
        }

        public static JacobiEigenResult JacobiEigenSolver(MatrixModel matrix, double tolerance = 1e-10, int maxIterations = 100)
        {
            if (matrix.Rows != matrix.Columns)
                throw new InvalidOperationException("Jacobi method requires a square matrix.");

            int n = matrix.Rows;
            double[,] A = (double[,])matrix.Data.Clone();
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
                {
                    return new JacobiEigenResult
                    {
                        Eigenvalues = Enumerable.Range(0, n).Select(i => A[i, i]).ToArray(),
                        Eigenvectors = Extentions.MatrixExtensions.ToJagged(V),
                        Iterations = iter + 1,
                        Converged = true
                    };
                }

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

            return new JacobiEigenResult
            {
                Eigenvalues = Enumerable.Range(0, n).Select(i => A[i, i]).ToArray(),
                Eigenvectors = Extentions.MatrixExtensions.ToJagged(V),
                Iterations = maxIterations,
                Converged = false
            };
        }

        public static EigenvalueListResult QREigenValues(MatrixModel model, int maxIterations = 1000, double tolerance = 1e-10)
        {
            if (model.Rows != model.Columns)
                throw new InvalidOperationException("QR algorithm requires a square matrix.");

            int n = model.Rows;
            var A = Matrix<double>.Build.DenseOfArray(model.Data);

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
                {
                    return new EigenvalueListResult
                    {
                        Eigenvalues = Enumerable.Range(0, n).Select(i => A[i, i]).ToArray(),
                        Iterations = iter + 1,
                        Converged = true
                    };
                }
            }

            return new EigenvalueListResult
            {
                Eigenvalues = Enumerable.Range(0, n).Select(i => A[i, i]).ToArray(),
                Iterations = maxIterations,
                Converged = false
            };
        }

        public static EigenvalueListResult LREigenValues(MatrixModel model, int maxIterations = 1000, double epsilon = 1e-6)
        {
            if (model.Rows != model.Columns)
                throw new InvalidOperationException("Matrix must be square.");

            int n = model.Rows;
            double[,] A_k = (double[,])model.Data.Clone();
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
                            R[i, j] -= factor * R[k, j];
                    }
                }

                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                    {
                        A_next[i, j] = 0;
                        for (int k = 0; k < n; k++)
                            A_next[i, j] += R[i, k] * L[k, j];
                    }

                double norm = 0.0;
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                    {
                        double diff = A_k[i, j] - A_next[i, j];
                        norm += diff * diff;
                    }

                if (Math.Sqrt(norm) < epsilon)
                {
                    return new EigenvalueListResult
                    {
                        Eigenvalues = Enumerable.Range(0, n).Select(i => A_next[i, i]).ToArray(),
                        Iterations = iter + 1,
                        Converged = true
                    };
                }

                Array.Copy(A_next, A_k, A_k.Length);
            }

            return new EigenvalueListResult
            {
                Eigenvalues = Enumerable.Range(0, n).Select(i => A_k[i, i]).ToArray(),
                Iterations = maxIterations,
                Converged = false
            };
        }

    }
}
