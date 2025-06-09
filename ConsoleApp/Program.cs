using MathCore.Models;
using Runners;

class Program
{
    static void Main()
    {
        var symA = new Matrix(new double[,] {
            { 0.5, 1.2, 1.0, 0.7 },
            { 1.2, 2.0, 0.5, 1.2 },
            { 1.0, 0.5, 1.0, 1.0 },
            { 0.7, 1.2, 1.0, 2.2 },
        });

        var A = new Matrix(new double[,] {
            { 0.5, 1.2, 1.0, 0.9 },
            { 1.2, 2.0, 0.5, 1.2 },
            { 1.0, 0.5, 1.0, 1.0 },
            { 0.5, 1.2, 1.0, 2.2 },
        });

        var B = new Matrix(new double[,]
        {
            {2.6, 1.0, 1.6 },
            {1.0, 3.1, 1.6 },
            {1.6, 1.6, 3.6 }
        });

        var m16 = new Matrix(new double[,]
        {
            { 1.8, 1.6, 1.7, 1.8},
            { 1.6, 2.8, 1.5, 1.3},
            { 1.7, 1.5, 3.8, 4.8},
            { 1.8, 1.3, 1.4, 4.8}
        });
        var Chol = new Matrix(new double[,]
        {
            { 4, 2, 2 },
            { 2, 10, 4 },
            { 2, 4, 9 }
        });
        //EigenTestRunner.RunGershgorin(A);
        //EigenTestRunner.RunPower(A);
        //EigenTestRunner.RunInverse(A);
        //EigenTestRunner.RunRayleigh(A);
        //EigenTestRunner.RunQR(symA);
        //EigenTestRunner.RunJacobi(A);
        //EigenTestRunner.RunLR(A);
        //EigenTestRunner.RunLeverrier(B);
        //EigenTestRunner.RunKrylov(B);
        //EigenTestRunner.RunFrobeniusNorm(A);
        //EigenTestRunner.RunInfinityNorm(A);
        //EigenTestRunner.RunOneNorm(A);
        //EigenTestRunner.RunTwoNorm(A);
        //EigenTestRunner.RunConditionNumber(m16);
        //DecompositionTestRunner.RunLUDecomposition(A);
        //DecompositionTestRunner.RunQRDecomposition(A);
        DecompositionTestRunner.RunCholeskyDecomposition(Chol);
    }
}
