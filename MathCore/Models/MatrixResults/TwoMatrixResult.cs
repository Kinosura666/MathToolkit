using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCore.Models.MatrixResults
{
    public class TwoMatrixResult
    {
        public string Matrix1Name { get; set; }
        public string Matrix2Name { get; set; }
        public double[][] Matrix1 { get; set; }
        public double[][] Matrix2 { get; set; }
    }
}
