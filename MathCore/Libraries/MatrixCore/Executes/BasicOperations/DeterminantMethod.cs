using MathCore.Interfaces;
using MathCore.Libraries.MatrixCore.DefaultLogic;
using MathCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCore.Libraries.MatrixCore.Executes.BasicOperations
{
    public class DeterminantMethod : IMatrixMethod
    {
        public string Name => "Determinant";

        public string Execute(MatrixModel A)
        {
            var det = MatrixOperations.Determinant(A);
            return "Determinant\n" + $"det(A) ≈ {det:F6}";
        }
    }
}
