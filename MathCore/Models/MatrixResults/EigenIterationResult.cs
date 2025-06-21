using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCore.Models.MatrixResults
{
    public class EigenIterationResult
    {
        public double Eigenvalue { get; set; }
        public double[] Eigenvector { get; set; }
        public int Iterations { get; set; }
        public bool Converged { get; set; }
    }

}
