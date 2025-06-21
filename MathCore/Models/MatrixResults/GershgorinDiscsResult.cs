using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCore.Models.MatrixResults
{
    public class GershgorinDiscsResult
    {
        public Disc[] Discs { get; set; } = [];
        public double MinBound { get; set; }
        public double MaxBound { get; set; }
    }

    public class Disc
    {
        public double Center { get; set; }
        public double Radius { get; set; }
    }

}
