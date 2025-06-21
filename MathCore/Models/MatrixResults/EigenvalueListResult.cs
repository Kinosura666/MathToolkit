using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCore.Models.MatrixResults
{
    public class EigenvalueListResult
    {
        public double[] Eigenvalues { get; set; }
        public int Iterations { get; set; }
        public bool Converged { get; set; }
    }

}
