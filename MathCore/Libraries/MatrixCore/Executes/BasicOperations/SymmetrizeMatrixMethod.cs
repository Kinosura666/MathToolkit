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
    public class SymmetrizeMatrixMethod : IMatrixMethod
    {
        public string Name => "Symmetrize Matrix";

        public string Execute(MatrixModel A)
        {
            try
            {
                var sym = MatrixOperations.Symmetrize(A);
                return "Symmetrized Matrix\nA_sym = (A + Aᵗ) / 2 =\n" + sym.ToFormattedString();
            }
            catch (Exception ex)
            {
                return "Symmetrization failed:\n" + ex.Message;
            }
        }
    }

}
