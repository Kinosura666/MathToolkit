using MathCore.Interfaces;
using MathCore.Libraries.MatrixCore.DefaultLogic;
using MathCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCore.Libraries.MatrixCore.Executes.Stats
{
    public class MatrixRankMethod : IMatrixMethod
    {
        public string Name => "Matrix Rank";

        public string Execute(MatrixModel A)
        {
            try
            {
                int rank = MatrixOperations.Rank(A);
                return "Matrix Rank\nrank(A) = " + rank;
            }
            catch (Exception ex)
            {
                return "Rank calculation failed:\n" + ex.Message;
            }
        }
    }

}
