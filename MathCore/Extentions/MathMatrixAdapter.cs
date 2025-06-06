using MathCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System.Runtime.CompilerServices;

namespace MathCore.Extentions
{
    public static class MathMatrixAdapter
    {
        public static Matrix<double> ToMathNet(this Models.Matrix matrix)
        {
            return DenseMatrix.OfArray(matrix.Data);
        }

        public static Models.Matrix ToCore(this Matrix<double> matrix)
        {
            return new Models.Matrix(matrix.ToArray());
        }

    }
}
