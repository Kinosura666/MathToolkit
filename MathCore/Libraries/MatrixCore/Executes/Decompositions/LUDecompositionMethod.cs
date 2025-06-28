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
    public class LUDecompositionMethod : IMatrixMethod
    {
        public string Name => "LU Decomposition";

        public string Execute(MatrixModel A)
        {
            try
            {
                var (L, U) = MatrixDecompositions.LUDecomposition(A);
                var sb = new StringBuilder();

                sb.AppendLine("LU Decomposition:\n");
                sb.AppendLine("L (Lower Triangular):");
                sb.AppendLine(L.ToFormattedString());

                sb.AppendLine("U (Upper Triangular):");
                sb.AppendLine(U.ToFormattedString());

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return $"LU decomposition failed: {ex.Message}";
            }
        }
    }

}
