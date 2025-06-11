using MathCore.Models;

namespace Web.Interfaces
{
    public interface IMatrixMapper
    {
        Matrix FromJagged(double[][] matrix);

        double[][] ToJagged(Matrix matrix);
    }
}
