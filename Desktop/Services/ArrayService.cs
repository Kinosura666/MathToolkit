using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Desktop.Services
{
    public static class ArrayService
    {
        public static int[] GenerateIntArray(int size, int min = 0, int max = 100)
        {
            Random rnd = new();
            return Enumerable.Range(0, size)
                .Select(_ => rnd.Next(min, max))
                .ToArray();
        }

        public static double[] GenerateDoubleArray(int size, double min = 0, double max = 100)
        {
            Random rnd = new();
            return Enumerable.Range(0, size)
                .Select(_ => Math.Round(rnd.NextDouble() * (max - min) + min, 2))
                .ToArray();
        }

        public static void Shuffle<T>(T[] array)
        {
            Random rnd = new();
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }

        public static string FormatArray<T>(T[] array, string format = "0.##")
        {
            return string.Join(" ", array.Select(x =>
                x is IFormattable f
                    ? f.ToString(format, CultureInfo.InvariantCulture)
                    : x?.ToString() ?? "null"));
        }
    }
}
