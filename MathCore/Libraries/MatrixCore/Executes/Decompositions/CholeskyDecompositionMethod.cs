using MathCore.Extentions;
using MathCore.Interfaces;
using MathCore.Libraries.MatrixCore.DefaultLogic;
using MathCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCore.Libraries.MatrixCore.Executes.Decompositions
{
    public class CholeskyDecompositionMethod : IMatrixMethod
    {
        public string Name => "Cholesky Decomposition";

        public string Execute(MatrixModel A)
        {
            try
            {
                var (L, LT) = MatrixDecompositions.CholeskyDecomposition(A);
                var sb = new StringBuilder();

                sb.AppendLine("Cholesky Decomposition:\n");
                sb.AppendLine("L (Lower Triangular):");
                sb.AppendLine(L.ToFormattedString());

                sb.AppendLine("L^T (Transposed):");
                sb.AppendLine(LT.ToFormattedString());

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return $"Cholesky decomposition failed: {ex.Message}";
            }
        }
    }

}
