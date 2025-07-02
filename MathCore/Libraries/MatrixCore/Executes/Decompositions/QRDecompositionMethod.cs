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
    public class QRDecompositionMethod : IMatrixMethod
    {
        public string Name => "QR Decomposition";

        public string Execute(MatrixModel A)
        {
            try
            {
                var (Q, R) = MatrixDecompositions.QRDecomposition(A);
                var sb = new StringBuilder();

                sb.AppendLine("QR Decomposition:\n");
                sb.AppendLine("Q (Orthogonal):");
                sb.AppendLine(Q.ToFormattedString());

                sb.AppendLine("R (Upper Triangular):");
                sb.AppendLine(R.ToFormattedString());

                return sb.ToString();
            }
            catch (Exception ex)
            {
                return $"QR decomposition failed: {ex.Message}";
            }
        }
    }

}
