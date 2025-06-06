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
        var (λ2, v2) = A.InversePowerIteration();

        Console.WriteLine($"Largest eigenvalue (power): {λ1:F5}");
        Console.WriteLine("Corresponding eigenvector:");
        foreach (var x in v1)
            Console.WriteLine($"{x:F5}");

        Console.WriteLine($"Smallest eigenvalue (inverse): {λ2:F5}");
        Console.WriteLine("Corresponding eigenvector:");
        foreach (var y in v2)
            Console.WriteLine($"{y:F5}");



    }
}
