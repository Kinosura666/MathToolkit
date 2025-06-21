using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathCore.Mappers;
using MathCore.Models;
using MathCore.Extentions;

namespace MathCore.Libraries.MatrixCore
{
    public static class MatrixOperations
    {
        public static MatrixModel Add(MatrixModel A, MatrixModel B)
        {
            if (A.Rows != B.Rows || A.Columns != B.Columns)
                throw new InvalidOperationException("Matrix dimensions must match for addition.");

            return A.ToMathNet().Add(B.ToMathNet()).ToCore();
        }

        public static MatrixModel Subtract(MatrixModel A, MatrixModel B)
        {
            if (A.Rows != B.Rows || A.Columns != B.Columns)
                throw new InvalidOperationException("Matrix dimensions must match for subtraction.");

            return A.ToMathNet().Subtract(B.ToMathNet()).ToCore();
        }

        public static MatrixModel Multiply(MatrixModel A, MatrixModel B)
        {
            if (A.Columns != B.Rows)
                throw new InvalidOperationException($"Matrix dimensions do not match for multiplication: A({A.Rows}x{A.Columns}) * B({B.Rows}x{B.Columns})");

            return A.ToMathNet().Multiply(B.ToMathNet()).ToCore();
        }

        public static MatrixModel Transpose(MatrixModel A) =>
            A.ToMathNet().Transpose().ToCore();

        public static MatrixModel Inverse(MatrixModel A)
        {
            if (A.Rows != A.Columns)
                throw new InvalidOperationException("Inverse is only defined for square matrices.");

            var m = A.ToMathNet();
            if (Math.Abs(m.Determinant()) < 1e-12)
                throw new InvalidOperationException("Matrix is singular and cannot be inverted.");

            return m.Inverse().ToCore();
        }

        public static double Determinant(MatrixModel A)
        {
            if (A.Rows != A.Columns)
                throw new InvalidOperationException("Determinant is only defined for square matrices.");

            return A.ToMathNet().Determinant();
        }

        public static MatrixModel PseudoInverse(MatrixModel A)
        {
            return A.ToMathNet().PseudoInverse().ToCore();
        }

        public static MatrixModel Power(MatrixModel A, int exponent)
        {
            if (A.Rows != A.Columns)
                throw new InvalidOperationException("Only square matrices can be raised to a power.");
            if (exponent < 0)
                throw new ArgumentException("Negative exponents are not supported.");

            if (exponent == 0)
                return Matrix<double>.Build.DenseIdentity(A.Rows).ToCore();

            var baseMatrix = A.ToMathNet();
            var result = baseMatrix.Clone();

            for (int i = 1; i < exponent; i++)
                result *= baseMatrix;

            return result.ToCore();
        }

        public static MatrixModel Symmetrize(MatrixModel A)
        {
            if (A.Rows != A.Columns)
                throw new InvalidOperationException("Symmetrization is defined only for square matrices.");

            var M = A.ToMathNet();
            return ((M + M.Transpose()) * 0.5).ToCore();
        }

        public static int Rank(MatrixModel A)
        {
            return A.ToMathNet().Rank();
        }
    }
}
