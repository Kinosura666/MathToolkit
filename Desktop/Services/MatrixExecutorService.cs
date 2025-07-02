using MathCore.Models;
using MathCore.Interfaces;
using System.Collections.Generic;
using MathCore.Libraries.MatrixCore;
using MathCore.Libraries.MatrixCore.Executes.Decompositions;
using MathCore.Libraries.MatrixCore.Executes.EigenValues;
using MathCore.Libraries.MatrixCore.Executes.Stats;
using MathCore.Libraries.MatrixCore.Executes.BasicOperations;
using MathCore.Libraries.MatrixCore.Executes.Polynomial;
namespace Desktop.Services
{
    public static class MatrixExecutorService
    {
        private static readonly Dictionary<string, IMatrixMethod> _methods = new()
        {
            //Basic Operations
            { "Add Matrices", new AddMatricesMethod() },
            { "Subtract Matrices", new SubtractMatricesMethod() },
            { "Multiply Matrices", new MultiplyMatricesMethod() },
            { "Pseudo-Inverse", new PseudoInverseMethod() },
            { "Determinant", new DeterminantMethod() },
            { "Inverse Matrix", new InverseMatrixMethod() },
            { "Transpose Matrix", new TransposeMatrixMethod() },
            { "Symmetrize Matrix", new SymmetrizeMatrixMethod() },
            { "Matrix Rank", new MatrixRankMethod() },
            // Decompositions
            { "LU Decomposition", new LUDecompositionMethod() },
            { "QR Decomposition", new QRDecompositionMethod() },
            { "Cholesky Decomposition", new CholeskyDecompositionMethod() },
            { "SVD Decomposition", new SvdDecompositionMethod() },
            //Eigenvalue
            { "Power Iteration", new PowerIterationMethod() },
            { "Inverse Iteration", new InverseIterationMethod() },
            { "Rayleigh Quotient Iteration", new RayleighMethod() },
            { "Jacobi Method", new JacobiMethod() },
            { "QR Method", new MathCore.Libraries.MatrixCore.Executes.EigenValues.QRMethod() },
            { "LR Method", new LREigenvaluesMethod() },
            //Polynomial
            { "Leverrier-Faddeev", new LeverrierFaddeevMethod() },
            { "Krylov Method", new KrylovMethod() },
            //Stats
            { "Matrix Norms", new MatrixNormsMethod() },
            { "Condition Number", new ConditionNumberMethod() },
            { "Gershgorin Discs", new GershgorinDiscsMethod() },
            { "Singular Values", new SingularValuesMethod() },
        };

        public static bool TryExecute(string methodName, MatrixModel A, MatrixModel B, out string result)
        {
            if (!_methods.TryGetValue(methodName, out var method))
            {
                result = $"Method \"{methodName}\" not found.";
                return false;
            }

            try
            {
                string singleResult = _lastMatrixTag == "B"
                    ? method.Execute(B)
                    : method.Execute(A);

                if (singleResult == "This method does not support two matrices.")
                    result = method.Execute(A, B);
                else
                    result = singleResult;

                return true;
            }
            catch (Exception ex)
            {
                result = $"Execution error: {ex.Message}";
                return false;
            }
        }

        private static string _lastMatrixTag = "A";

        public static void SetActiveMatrixTag(string tag)
        {
            _lastMatrixTag = tag;
        }

    }
}