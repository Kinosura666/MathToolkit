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
    public class AddMatricesMethod : IMatrixMethod
    {
        public string Name => "Add Matrices";

        public string Execute(MatrixModel A)
        {
            throw new InvalidOperationException("Addition requires two matrices.");
        }

        public string Execute(MatrixModel A, MatrixModel B)
        {
            if (A.Rows != B.Rows || A.Columns != B.Columns)
                return "Addition requires matrices of same size.";

            var C = MatrixOperations.Add(A, B);
            return "A + B:\n" + C.ToFormattedString();
        }
    }
}
