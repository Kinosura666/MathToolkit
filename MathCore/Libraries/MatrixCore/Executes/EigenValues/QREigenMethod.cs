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
    public class QRMethod : IMatrixMethod
    {
        public string Name => "QR Method";

        public string Execute(MatrixModel A)
        {
            var result = MatrixEigen.QREigenValues(A);
            var sb = new StringBuilder();

            sb.AppendLine("QR Method");
            sb.AppendLine($"Iterations: {result.Iterations}");
            sb.AppendLine($"Converged: {(result.Converged ? "Yes" : "No")}");
            sb.AppendLine("\nEigenvalues:");

            for (int i = 0; i < result.Eigenvalues.Length; i++)
                sb.AppendLine($"λ{i + 1} ≈ {result.Eigenvalues[i]:F6}");

            return sb.ToString();
        }
    }

}
