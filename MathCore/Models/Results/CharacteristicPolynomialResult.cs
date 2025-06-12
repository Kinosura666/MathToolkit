using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCore.Models.Results
{
    public class CharacteristicPolynomialResult
    {
        public double[] Coefficients { get; set; } = [];
        public int Degree => Coefficients.Length - 1;
    }

}
