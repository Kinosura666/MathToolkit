using MathCore.Interfaces;
using MathCore.Libraries.MatrixCore.DefaultLogic;
using MathCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCore.Libraries.MatrixCore.Executes.Stats
{
    public class GershgorinDiscsMethod : IMatrixMethod
    {
        public string Name => "Gershgorin Discs";

        public string Execute(MatrixModel A)
        {
            var result = MatrixGershgorin.GershgorinDiscs(A);
            var sb = new StringBuilder();

            sb.AppendLine("Gershgorin discs (center ± radius):\n");

            for (int i = 0; i < result.Discs.Length; i++)
            {
                var disc = result.Discs[i];
                sb.AppendLine($"D{i + 1}: {disc.Center:F5} ± {disc.Radius:F5}");
            }

            sb.AppendLine($"\nApproximate eigenvalue range: [{result.MinBound:F5}; {result.MaxBound:F5}]");
            return sb.ToString();
        }
    }

}
