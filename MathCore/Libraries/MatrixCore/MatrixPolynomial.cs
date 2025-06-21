using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathCore.Mappers;
using MathCore.Models;
using MathCore.Extentions;
using MathCore.Models.MatrixResults;

namespace MathCore.Libraries.MatrixCore
{
    public class MatrixPolynomial
    {
        public static CharacteristicPolynomialResult LeverrierFaddeev(MatrixModel model)
        {
            if (model.Rows != model.Columns)
                throw new InvalidOperationException("Matrix must be square.");

            int n = model.Rows;
            var A = model.ToMathNet();
            var I = Matrix<double>.Build.DenseIdentity(n);

            var B = I.Clone();
            double[] coeffs = new double[n + 1];
            coeffs[0] = 1.0;

            for (int k = 1; k <= n; k++)
            {
                double trace = (A * B).Trace();
                double a_k = -trace / k;
                coeffs[k] = a_k;

                if (k < n)
                {
                    B = A * B + a_k * I;
                }
            }

            return new CharacteristicPolynomialResult
            {
                Coefficients = coeffs
            };
        }

        public static CharacteristicPolynomialResult KrylovCharacteristicPolynomial(MatrixModel model)
        {
            if (model.Rows != model.Columns)
                throw new InvalidOperationException("Matrix must be square.");

            int n = model.Rows;
            var A = model.ToMathNet();

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

            return new CharacteristicPolynomialResult
            {
                Coefficients = coeffs
            };
        }
    }
}
