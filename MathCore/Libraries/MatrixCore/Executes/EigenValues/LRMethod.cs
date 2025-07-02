using MathCore.Interfaces;
using MathCore.Libraries.MatrixCore.DefaultLogic;
using MathCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCore.Libraries.MatrixCore.Executes.EigenValues
{
    public class LREigenvaluesMethod : IMatrixMethod
    {
        public string Name => "LR Method";

        public string Execute(MatrixModel A)
        {
            var result = MatrixEigen.LREigenValues(A);

            var sb = new StringBuilder();
            sb.AppendLine("LR Method");
            sb.AppendLine($"Iterations: {result.Iterations}");
            sb.AppendLine($"Converged: {(result.Converged ? "Yes" : "No")}");
            sb.AppendLine();
            sb.AppendLine("Eigenvalues:");
            foreach (var λ in result.Eigenvalues)
                sb.AppendLine($"λ ≈ {λ:F6}");

            return sb.ToString();
        }
    }
}
