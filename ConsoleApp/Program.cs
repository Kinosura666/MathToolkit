using MathCore.Models;

class Program
{
    static void Main()
    {
        var A = new Matrix(new double[,] {
            { 2, 1 },
            { 1, 2 }
        });

        var (λ1, v1) = A.PowerIteration();
        var (λ2, v2) = A.RayleighQuotientIteration(initialGuess: λ1);

        Console.WriteLine($"Refined eigenvalue: {λ2:F10}");

    }
}
