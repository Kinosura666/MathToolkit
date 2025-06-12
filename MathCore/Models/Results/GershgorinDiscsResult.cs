using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCore.Models.Results
{
    public class GershgorinDiscsResult
    {
        public (double Center, double Radius)[] Discs { get; set; }
        public double MinBound { get; set; }
        public double MaxBound { get; set; }
    }

}
