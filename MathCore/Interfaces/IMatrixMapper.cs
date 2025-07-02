using MathCore.Libraries;
using MathCore.Models;

namespace MathCore.Interfaces
{
    public interface IMatrixMapper
    {
        MatrixModel FromJagged(double[][] matrix);

        double[][] ToJagged(MatrixModel matrix);
    }
}
