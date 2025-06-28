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
    public class SvdDecompositionMethod : IMatrixMethod
    {
        public string Name => "SVD Decomposition";

        public string Execute(MatrixModel A)
        {
            var (U, S, VT) = MatrixDecompositions.SVD(A);
            var singularValues = MatrixStats.GetSingularValues(A);
            var sb = new StringBuilder();

            sb.AppendLine("SVD Decomposition");

            sb.AppendLine("\nSingular values (diagonal of Σ):");
            for (int i = 0; i < singularValues.Length; i++)
                sb.AppendLine($"σ{i + 1} ≈ {singularValues[i]:F6}");

            sb.AppendLine("Matrix U:");
            sb.AppendLine(U.ToFormattedString());

            sb.AppendLine("Matrix Σ:");
            sb.AppendLine(S.ToFormattedString());

            sb.AppendLine("Matrix V^T:");
            sb.AppendLine(VT.ToFormattedString());

            return sb.ToString();
        }
    }

}
