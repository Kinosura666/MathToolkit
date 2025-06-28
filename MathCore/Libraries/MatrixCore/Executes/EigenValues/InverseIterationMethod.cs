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
    public class InverseIterationMethod : IMatrixMethod
    {
        public string Name => "Inverse Iteration";

        public string Execute(MatrixModel A)
        {
            var result = MatrixEigen.InversePowerIteration(A);
            var sb = new StringBuilder();

            sb.AppendLine("Inverse Iteration");
            sb.AppendLine($"λ ≈ {result.Eigenvalue:F6}");
            sb.AppendLine($"Iterations: {result.Iterations}");
            sb.AppendLine($"Converged: {(result.Converged ? "Yes" : "No")}");
            sb.AppendLine("\nEigenvector:");
            foreach (var x in result.Eigenvector)
                sb.AppendLine($"{x:F5}");

            return sb.ToString();
        }
    }

}
