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
    public class PowerIterationMethod : IMatrixMethod
    {
        public string Name => "Power Iteration";

        public string Execute(MatrixModel A)
        {
            var result = MatrixEigen.PowerIteration(A);
            return $"Power Iteration\n" +
                   $"\u03bb ≈ {result.Eigenvalue:F6}\n" +
                   $"Iterations: {result.Iterations}\n" +
                   $"Converged: {(result.Converged ? "Yes" : "No")}\n\n" +
                   "Eigenvector:\n" +
                   string.Join("\n", result.Eigenvector.Select(x => $"{x:F5}"));
        }
    }
}
