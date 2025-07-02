using MathCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCore.Interfaces
{
    public interface IMatrixMethod
    {
        string Name { get; }
        string Execute(MatrixModel A);

        string Execute(MatrixModel A, MatrixModel B) => "This method does not support two matrices.";
    }
}
