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

        public static void RunQRDecomposition(Matrix A)
        {
            Console.WriteLine("=== QR Decomposition ===");
            Console.WriteLine("Matrix A:");
            Console.WriteLine(A.ToFormattedString());

            try
            {
                var (Q, R) = A.QRDecomposition();

                Console.WriteLine("\nQ (Orthogonal):");
                Console.WriteLine(Q.ToFormattedString());

                Console.WriteLine("R (Upper Triangular):");
                Console.WriteLine(R.ToFormattedString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"QR decomposition failed: {ex.Message}");
            }
        }
        public static void RunCholeskyDecomposition(Matrix A)
        {
            Console.WriteLine("=== Cholesky Decomposition ===");
            Console.WriteLine("Matrix A:");
            Console.WriteLine(A.ToFormattedString());

            try
            {
                var (L, LT) = A.CholeskyDecomposition();

                Console.WriteLine("\nL (Lower Triangular):");
                Console.WriteLine(L.ToFormattedString());

                Console.WriteLine("L (Transposed):");
                Console.WriteLine(LT.ToFormattedString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cholesky decomposition failed: {ex.Message}");
            }
        }

        public static void RunSVD(Matrix A)
        {
            Console.WriteLine("=== SVD Decomposition ===");
            Console.WriteLine("Matrix A:");
            Console.WriteLine(A.ToFormattedString());

            var (U, S, VT) = A.SVD();

            Console.WriteLine("\nU (Left orthogonal):");
            Console.WriteLine(U.ToFormattedString());

            Console.WriteLine("S (Singular values - diagonal):");
            Console.WriteLine(S.ToFormattedString());

            Console.WriteLine("V (Right orthogonal transpose):");
            Console.WriteLine(VT.ToFormattedString());
        }

    }
}
