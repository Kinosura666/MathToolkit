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
    public class PseudoInverseMethod : IMatrixMethod
    {
        public string Name => "Pseudo-Inverse";

        public string Execute(MatrixModel A)
        {
            var pseudo = MatrixOperations.PseudoInverse(A);
            return "Pseudo-Inverse:\n" + pseudo.ToFormattedString();
        }
    }
}
