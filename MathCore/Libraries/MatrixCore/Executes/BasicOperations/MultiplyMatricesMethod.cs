using MathCore.Extentions;
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
    public class MultiplyMatricesMethod : IMatrixMethod
    {
        public string Name => "Multiply Matrices";

        public string Execute(MatrixModel A)
        {
            throw new InvalidOperationException("Multiplication requires two matrices.");
        }

        public string Execute(MatrixModel A, MatrixModel B)
        {
            if (A.Columns != B.Rows)
                return $"Multiplication not possible: A is {A.Rows}×{A.Columns}, B is {B.Rows}×{B.Columns}";

            var C = MatrixOperations.Multiply(A, B);
            return "A × B:\n" + C.ToFormattedString();
        }
    }
}
