using MathCore.Extentions;
using MathCore.Models;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCore.Libraries.MatrixCore
{
    public static class MatrixDecompositions
    {
        public static (MatrixModel L, MatrixModel U) LUDecomposition(MatrixModel model)
        {
            if (model.Rows != model.Columns)
                throw new InvalidOperationException("LU decomposition requires a square matrix.");

            int n = model.Rows;
            var L = new double[n, n];
            var U = new double[n, n];
            var A = model.Data;

            for (int i = 0; i < n; i++)
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

            return (new MatrixModel(L), new MatrixModel(U));
        }

        public static (MatrixModel Q, MatrixModel R) QRDecomposition(MatrixModel model)
        {
            if (model.Rows != model.Columns)
                throw new InvalidOperationException("QR decomposition requires a square matrix.");

            var A = Matrix<double>.Build.DenseOfArray(model.Data);
            var qr = A.QR();

            var Q = qr.Q.ToCore();
            var R = qr.R.ToCore();

            return (Q, R);
        }

        public static (MatrixModel L, MatrixModel LT) CholeskyDecomposition(MatrixModel model)
        {
            if (model.Rows != model.Columns)
                throw new InvalidOperationException("Cholesky decomposition requires a square matrix.");

            int n = model.Rows;
            var A = model.Data;
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
                        double value = A[i, i] - sum;
                        if (value <= 0)
                            throw new InvalidOperationException("Matrix is not positive definite.");

                        L[i, j] = Math.Sqrt(value);
                    }
                    else
                    {
                        L[i, j] = (A[i, j] - sum) / L[j, j];
                    }
                }
            }

            var LT = new double[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    LT[i, j] = L[j, i];

            return (new MatrixModel(L), new MatrixModel(LT));
        }

        public static (MatrixModel U, MatrixModel S, MatrixModel VT) SVD(MatrixModel model)
        {
            var A = Matrix<double>.Build.DenseOfArray(model.Data);
            var svd = A.Svd();

            var U = svd.U.ToCore();
            var S = svd.W.ToCore();
            var VT = svd.VT.ToCore();

            return (U, S, VT);
        }
    }
}
