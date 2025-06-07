using MathCore.Models;

class Program
{
    static void Main()
    {
        var A = new Matrix(new double[,] {
            { 4, -1, 0 },
            { -1, 3, -1 },
            { 0, -1, 2 }
        });

        var (discs, min, max) = A.GershgorinDiscs();

        Console.WriteLine("Gershgorin Discs:");
        foreach (var (center, radius) in discs)
            Console.WriteLine($"Center = {center:F2}, Radius = {radius:F2} -> [{center - radius:F2}, {center + radius:F2}]");

        Console.WriteLine($"\nEstimated eigenvalue range: [{min:F2}, {max:F2}]");



    }
}
