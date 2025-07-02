using MathCore.Interfaces;
using MathCore.Libraries.MatrixCore.DefaultLogic;
using MathCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCore.Libraries.MatrixCore.Executes.Polynomial
{
    public class KrylovMethod : IMatrixMethod
    {
        public string Name => "Krylov Method";

        public string Execute(MatrixModel A)
        {
            var result = MatrixPolynomial.KrylovCharacteristicPolynomial(A);
            var coeffs = result.Coefficients;
            int degree = result.Degree;

            var sb = new StringBuilder();
            sb.AppendLine("Krylov\nCharacteristic polynomial:\n");

            for (int i = 0; i < coeffs.Length; i++)
            {
                int power = degree - i;
                sb.AppendLine($"a{power} = {coeffs[i]:F6}");
            }

            sb.AppendLine("\np(λ) = ");
            for (int i = 0; i < coeffs.Length; i++)
            {
                int power = degree - i;
                double coeff = coeffs[i];
                string sign = coeff >= 0 && i > 0 ? " + " : i > 0 ? " - " : "";
                sb.Append($"{sign}{Math.Abs(coeff):F4}");

                if (power > 1) sb.Append($"λ^{power}");
                else if (power == 1) sb.Append("λ");
            }

            return sb.ToString();
        }
    }

}
