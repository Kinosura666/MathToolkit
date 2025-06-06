using MathCore.Models;

class Program
{
    static void Main()
    {
        var A = new Matrix(new double[,]
        {
            { 4, 1 },
            { 2, 3 }
        });

        var (λ, v) = A.PowerIteration();

        Console.WriteLine($"Largest eigenvalue: {λ:F5}");
        Console.WriteLine("Corresponding eigenvector:");
        foreach (var x in v)
            Console.WriteLine($"{x:F5}");


    }
}
