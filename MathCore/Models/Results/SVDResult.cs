using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCore.Models.Results
{
    public class SVDResult
    {
        public double[][] U { get; set; }
        public double[][] S { get; set; }
        public double[][] VT { get; set; }
    }
}
