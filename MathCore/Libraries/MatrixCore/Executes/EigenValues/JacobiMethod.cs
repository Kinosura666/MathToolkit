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
    public class JacobiMethod : IMatrixMethod
    {
        public string Name => "Jacobi Method";

        public string Execute(MatrixModel A)
        {
            var result = MatrixEigen.JacobiEigenSolver(A);
            var sb = new StringBuilder();

            sb.AppendLine("Jacobi Method");
            sb.AppendLine($"Iterations: {result.Iterations}");
            sb.AppendLine($"Converged: {(result.Converged ? "Yes" : "No")}");
            sb.AppendLine("\nEigenvalues and corresponding eigenvectors:");

            for (int i = 0; i < result.Eigenvalues.Length; i++)
            {
                sb.AppendLine($"λ{i + 1} ≈ {result.Eigenvalues[i]:F6}");
                sb.Append("v = (");
                for (int j = 0; j < result.Eigenvectors.Length; j++)
                {
                    sb.Append($"{result.Eigenvectors[j][i]:F5}");
                    if (j < result.Eigenvectors.Length - 1)
                        sb.Append("; ");
                }
                sb.AppendLine(")");
            }

            return sb.ToString();
        }
    }

}
