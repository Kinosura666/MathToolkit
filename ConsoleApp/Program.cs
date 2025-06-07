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

        //EigenTestRunner.RunGershgorin(A);
        //EigenTestRunner.RunPower(A);
        //EigenTestRunner.RunInverse(A);
        //EigenTestRunner.RunRayleigh(A);
        //EigenTestRunner.RunQR(symA);
        //EigenTestRunner.RunJacobi(A);
        EigenTestRunner.RunLR(A);
    }
}
