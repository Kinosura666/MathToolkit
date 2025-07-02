using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCore.Models
{
    public class MatrixModel 
    {
        public double[,] Data { get; }

        public int Rows => Data.GetLength(0);
        public int Columns => Data.GetLength(1);

        public MatrixModel(double[,] data)
        {
            Data = data;
        }
    }
}
