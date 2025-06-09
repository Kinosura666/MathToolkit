using MathCore.Extentions;
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
            var lambdas = A.LREigenValues();
            for (int i = 0; i < lambdas.Length; i++)
                Console.WriteLine($"lambda{i + 1} = {lambdas[i]:F10}");
        }

        public static void RunLeverrier(Matrix A)
        {
            Console.WriteLine("=== Leverrier-Faddeev Method ===");
            var coeffs = A.LeverrierFaddeev();

            int degree = coeffs.Length - 1;

            Console.WriteLine("Characteristic polynomial coefficients (from lambda^n to lambda^0):");
            for (int i = 0; i < coeffs.Length; i++)
                Console.WriteLine($"a_{degree - i} = {coeffs[i]:F10}");

            Console.WriteLine("\nPolynomial:");
            Console.Write("p(lambda) = ");
            for (int i = 0; i < coeffs.Length; i++)
            {
                int power = degree - i;
                double coeff = coeffs[i];
                string sign = (coeff >= 0 && i > 0) ? " + " : (i > 0 ? " - " : "");

                Console.Write($"{sign}{Math.Abs(coeff):F4}");

                if (power > 1)
                    Console.Write($"lambda^{power}");
                else if (power == 1)
                    Console.Write($"lambda");
            }
            Console.WriteLine();
        }

        public static void RunKrylov(Matrix A)
        {
            Console.WriteLine("=== Krylov Method ===");
            var coeffs = A.KrylovCharacteristicPolynomial();

            int degree = coeffs.Length - 1;

            Console.WriteLine("Characteristic polynomial coefficients (from lambda^n to lambda^0):");
            for (int i = 0; i < coeffs.Length; i++)
                Console.WriteLine($"a_{degree - i} = {coeffs[i]:F10}");

            Console.Write("\nPolynomial: p(lambda) = ");
            for (int i = 0; i < coeffs.Length; i++)
            {
                int power = degree - i;
                string sign = (coeffs[i] >= 0 && i > 0) ? " + " : (i > 0 ? " - " : "");
                Console.Write($"{sign}{Math.Abs(coeffs[i]):F4}");

                if (power > 1) Console.Write($"lambda^{power}");
                else if (power == 1) Console.Write("lambda");
            }
            Console.WriteLine();
        }

        public static void RunFrobeniusNorm(Matrix A)
        {
            Console.WriteLine("=== Matrix Frobenius Norm ===");
            Console.WriteLine($"Frobenius norm: {A.FrobeniusNorm():F6}");
        }

        public static void RunInfinityNorm(Matrix A)
        {
            Console.WriteLine("=== Matrix Infinity Norm ===");
            Console.WriteLine($"Infinity norm: {A.InfinityNorm():F6}");
        }

        public static void RunOneNorm(Matrix A)
        {
            Console.WriteLine("=== Matrix 1-Norm ===");
            Console.WriteLine($"One norm: {A.OneNorm():F6}");
        }

        public static void RunTwoNorm(Matrix A)
        {
            Console.WriteLine("=== Matrix 2-Norm ===");
            Console.WriteLine($"||A||2 = {A.TwoNorm():F6}");
        }

        public static void RunConditionNumber(Matrix A)
        {
            Console.WriteLine("=== Matrix Condition Number ===");
            Console.WriteLine($"cond2(A) = {A.ConditionNumber2():F6}");
        }

        public static void RunSingularValues(Matrix A)
        {
            var singularValues = A.GetSingularValues();

            Console.WriteLine("=== Singular Values ===");
            for (int i = 0; i < singularValues.Length; i++)
                Console.WriteLine($"singularity{i + 1} = {singularValues[i]:F6}");

            Console.WriteLine($"Max singular value (Spectral norm): {singularValues[0]:F6}");
        }

        public static void RunPseudoInverse(Matrix A)
        {
            Console.WriteLine("=== Pseudo-Inverse (Moore–Penrose) ===");
            Console.WriteLine("Matrix A:");
            Console.WriteLine(A.ToFormattedString());

            try
            {
                var pinv = A.PseudoInverse();
                Console.WriteLine("\nPseudo-Inverse A⁺:");
                Console.WriteLine(pinv.ToFormattedString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Pseudo-inverse failed: {ex.Message}");
            }
        }

        public static void RunPower(Matrix A, int k)
        {
            Console.WriteLine($"=== A^{k} ===");
            Console.WriteLine("Matrix A:");
            Console.WriteLine(A.ToFormattedString());

            try
            {
                var powered = A.Power(k);
                Console.WriteLine($"\nA^{k}:");
                Console.WriteLine(powered.ToFormattedString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Matrix power failed: {ex.Message}");
            }
        }

        public static void RunSymmetrize(Matrix A)
        {
            Console.WriteLine("=== Symmetrized Matrix ===");
            Console.WriteLine("Original A:");
            Console.WriteLine(A.ToFormattedString());

            try
            {
                var symA = A.Symmetrize();
                Console.WriteLine("\n(A + Aᵗ) / 2:");
                Console.WriteLine(symA.ToFormattedString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Symmetrization failed: {ex.Message}");
            }
        }

        public static void RunRank(Matrix A)
        {
            Console.WriteLine("=== Matrix Rank ===");
            Console.WriteLine("Matrix A:");
            Console.WriteLine(A.ToFormattedString());

            try
            {
                int rank = A.Rank();
                Console.WriteLine($"\nRank(A) = {rank}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Rank calculation failed: {ex.Message}");
            }
        }

    }
}
