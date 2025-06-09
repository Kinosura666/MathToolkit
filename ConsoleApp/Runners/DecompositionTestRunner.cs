using System;
using MathCore.Extentions;
using MathCore.Models;

namespace Runners
{
    public static class DecompositionTestRunner
    {
        public static void RunLUDecomposition(Matrix A)
        {
            Console.WriteLine("=== LU Decomposition ===");
            Console.WriteLine("Matrix A:");
            Console.WriteLine(A.ToFormattedString());

            try
            {
                var (L, U) = A.LUDecomposition();

                Console.WriteLine("\nL (Lower Triangular):");
                Console.WriteLine(L.ToFormattedString());

                Console.WriteLine("U (Upper Triangular):");
                Console.WriteLine(U.ToFormattedString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LU decomposition failed: {ex.Message}");
            }
        }

    }
}
