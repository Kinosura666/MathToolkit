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
    public class SingularValuesMethod : IMatrixMethod
    {
        public string Name => "Singular Values";

        public string Execute(MatrixModel A)
        {
            var singularValues = MatrixStats.GetSingularValues(A);
            var sb = new StringBuilder();
            sb.AppendLine("Singular Values:");

            for (int i = 0; i < singularValues.Length; i++)
                sb.AppendLine($"σ{i + 1} ≈ {singularValues[i]:F6}");

            return sb.ToString();
        }
    }
}
