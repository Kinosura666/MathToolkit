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
    public class TransposeMatrixMethod : IMatrixMethod
    {
        public string Name => "Transpose Matrix";

        public string Execute(MatrixModel A)
        {
            var T = MatrixOperations.Transpose(A);
            return "Transpose Matrix\nAᵗ =\n" + T.ToFormattedString();
        }
    }

}
