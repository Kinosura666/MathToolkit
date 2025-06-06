using MathCore.Models;

class Program
{
    static void Main()
    {
        var a = Matrix.Generate(2, 3);
        var b = Matrix.Generate(2, 3);
        var c = Matrix.Generate(3, 2);
        var sq = Matrix.Generate(3, 3);

        Console.WriteLine("Матриця A:");
        Console.WriteLine(a);

        Console.WriteLine("Матриця B:");
        Console.WriteLine(b);

        Console.WriteLine("Матриця С:");
        Console.WriteLine(c);

        var sum = a.Add(b);
        Console.WriteLine("A + B:");
        Console.WriteLine(sum);

        var diff = a.Subtract(b);
        Console.WriteLine("A - B:");
        Console.WriteLine(diff);

        var prod = a.Multiply(c);
        Console.WriteLine("A * C:");
        Console.WriteLine(prod);

        var trans = a.Transpose();
        Console.WriteLine("Транспонована A:");
        Console.WriteLine(trans);

        var matrix = Matrix.Generate(3, 3, -5, 5);
        Console.WriteLine("🧮 Матриця A:");
        Console.WriteLine(matrix);
        Console.WriteLine($"📐 Детермінант: {matrix.Determinant():F4}");

        var swap = sq.Inverse();
        Console.WriteLine("Inversed");
        Console.WriteLine(sq);

    }
}
