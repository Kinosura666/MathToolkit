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
    public class InverseMatrixMethod : IMatrixMethod
    {
        public string Name => "Inverse Matrix";

        public string Execute(MatrixModel A)
        {
            try
            {
                var inv = MatrixOperations.Inverse(A);
                return "Inverse Matrix\nA⁻¹ =\n" + inv.ToFormattedString();
            }
            catch (Exception ex)
            {
                return "Inverse Matrix\nMatrix is not invertible.\n" + ex.Message;
            }
        }
    }
}
