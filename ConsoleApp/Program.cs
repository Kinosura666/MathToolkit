using MathCore.Models;

class Program
{
    static void Main()
    {
        var A = new Matrix(new double[,] {
            { 4, 1 },
            { 1, 3 }
        });

        var (λ, vectors) = A.JacobiEigenSolver();

        Console.WriteLine("Eigenvalues:");
        foreach (var x in λ)
            Console.WriteLine($"{x:F5}");

        Console.WriteLine("Eigenvectors (columns):");
        for (int i = 0; i < vectors.GetLength(0); i++)
        {
            for (int j = 0; j < vectors.GetLength(1); j++)
                Console.Write($"{vectors[i, j],10:F5}");
            Console.WriteLine();
        }

    }
}
