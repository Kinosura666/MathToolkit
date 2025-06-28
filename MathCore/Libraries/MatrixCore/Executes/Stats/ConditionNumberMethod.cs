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
    public class ConditionNumberMethod : IMatrixMethod
    {
        public string Name => "Condition Number";

        public string Execute(MatrixModel A)
        {
            var cond = MatrixStats.ConditionNumber2(A);
            return $"Condition number \ncond₂(A) ≈ {cond:F5}";
        }
    }

}
