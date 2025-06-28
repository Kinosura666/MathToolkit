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
    public class MatrixNormsMethod : IMatrixMethod
    {
        public string Name => "Matrix Norms";

        public string Execute(MatrixModel A)
        {
            var frob = MatrixStats.FrobeniusNorm(A);
            var norm1 = MatrixStats.OneNorm(A);
            var normInf = MatrixStats.InfinityNorm(A);
            var norm2 = MatrixStats.TwoNorm(A);

            return "Matrix norms:\n\n" +
                   $"‣ Frobenius (Euclid) norm ||A||_F  ≈ {frob:F5}\n" +
                   $"‣ 1-norm ||A||_1  ≈ {norm1:F5}\n" +
                   $"‣ ∞-norm ||A||_∞  ≈ {normInf:F5}\n" +
                   $"‣ 2-norm (spectral norm) ||A||_2  ≈ {norm2:F5}\n";
        }
    }

}
