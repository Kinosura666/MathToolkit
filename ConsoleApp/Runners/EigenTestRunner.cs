using MathCore.Models;

namespace Runners
{
    public static class EigenTestRunner
    {
        public static void RunPower(Matrix A)
        {
            Console.WriteLine("=== Power Iteration ===");
            var (lambda, v) = A.PowerIteration();
            Console.WriteLine($"lambda = {lambda:F10}");
            PrintVector(v);
        }

        public static void RunInverse(Matrix A)
        {
            Console.WriteLine("=== Inverse Power Iteration ===");
            var (lambda, v) = A.InversePowerIteration();
            Console.WriteLine($"lambda = {lambda:F10}");
            PrintVector(v);
        }

        public static void RunRayleigh(Matrix A)
        {
            Console.WriteLine("=== Rayleigh Quotient Iteration ===");
            var (lambda, v) = A.RayleighQuotientIteration();
            Console.WriteLine($"lambda = {lambda:F10}");
            PrintVector(v);
        }

        public static void RunJacobi(Matrix A)
        {
            Console.WriteLine("=== Jacobi Method ===");
            var (lambdas, vectors) = A.JacobiEigenSolver();
            for (int i = 0; i < lambdas.Length; i++)
            {
                Console.WriteLine($"lambda{i + 1} = {lambdas[i]:F10}");
                Console.Write("v = (");
                for (int j = 0; j < vectors.GetLength(0); j++)
                {
                    Console.Write($"{vectors[j, i]:F5}");
                    if (j < vectors.GetLength(0) - 1)
                        Console.Write(", ");
                }
                Console.WriteLine(")");
            }
        }

        public static void RunQR(Matrix A)
        {
            Console.WriteLine("=== QR Method ===");
            var lambdas = A.QREigenValues();
            for (int i = 0; i < lambdas.Length; i++)
                Console.WriteLine($"lambda{i + 1} = {lambdas[i]:F10}");
        }

        public static void RunGershgorin(Matrix A)
        {
            Console.WriteLine("=== Gershgorin Discs ===");
            var (discs, min, max) = A.GershgorinDiscs();
            for (int i = 0; i < discs.Length; i++)
                Console.WriteLine($"Disc {i + 1}: center = {discs[i].Center:F5}, radius = {discs[i].Radius:F5}");
            Console.WriteLine($"Estimated eigenvalue range: [{min:F5}, {max:F5}]");
        }

        private static void PrintVector(double[] v)
        {
            Console.WriteLine("v = (");
            foreach (var x in v)
                Console.WriteLine($"  {x:F5}");
            Console.WriteLine(")");
        }

        public static void RunLR(Matrix A)
        {
            Console.WriteLine("=== LR Method ===");
            var λs = A.LREigenValues();
            for (int i = 0; i < λs.Length; i++)
                Console.WriteLine($"lambda{i + 1} = {λs[i]:F10}");
        }


    }
}
