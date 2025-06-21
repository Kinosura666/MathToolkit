using MathCore.Models;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathCore.Extentions;

namespace MathCore.Libraries.MatrixCore
{
    public class MatrixStats
    {
        public static double FrobeniusNorm(MatrixModel model)
        {
            double sum = 0.0;
            var data = model.Data;
            for (int i = 0; i < model.Rows; i++)
                for (int j = 0; j < model.Columns; j++)
                    sum += data[i, j] * data[i, j];
            return Math.Sqrt(sum);
        }

        public static double OneNorm(MatrixModel model)
        {
            double max = 0.0;
            var data = model.Data;
            for (int j = 0; j < model.Columns; j++)
            {
                double colSum = 0.0;
                for (int i = 0; i < model.Rows; i++)
                    colSum += Math.Abs(data[i, j]);
                if (colSum > max) max = colSum;
            }
            return max;
        }

        public static double InfinityNorm(MatrixModel model)
        {
            double max = 0.0;
            var data = model.Data;
            for (int i = 0; i < model.Rows; i++)
            {
                double rowSum = 0.0;
                for (int j = 0; j < model.Columns; j++)
                    rowSum += Math.Abs(data[i, j]);
                if (rowSum > max) max = rowSum;
            }
            return max;
        }

        public static double TwoNorm(MatrixModel model)
        {
            var A = model.ToMathNet();
            var AtA = A.TransposeThisAndMultiply(A);
            var evd = AtA.Evd();
            return Math.Sqrt(evd.EigenValues.Real().Maximum());
        }

        public static double ConditionNumber2(MatrixModel model)
        {
            if (model.Rows != model.Columns)
                throw new InvalidOperationException("Condition number is defined for square matrices only.");

            var A = model.ToMathNet();
            var AtA = A.TransposeThisAndMultiply(A);
            var evd = AtA.Evd();

            var eigenvalues = evd.EigenValues
                .Select(x => x.Real)
                .Where(x => x > 1e-12)
                .ToArray();

            if (eigenvalues.Length == 0)
                throw new InvalidOperationException("Matrix appears to be singular or nearly singular.");

            double max = eigenvalues.Max();
            double min = eigenvalues.Min();

            return Math.Sqrt(max / min);
        }

        public static double[] GetSingularValues(MatrixModel model)
        {
            var A = Matrix<double>.Build.DenseOfArray(model.Data);
            var svd = A.Svd();
            return svd.S.ToArray();
        }
    }
}
