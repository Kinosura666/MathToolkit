using System;
using System.Collections.Generic;
using System.Diagnostics;
using MathCore.Common;
using MathCore.Extentions;
using MathCore.Interfaces;
using MathCore.Models.SortResults;

namespace MathCore.Libraries.SortingCore
{
    public class LsdRadixSort : ISortAlgorithm<int>
    {
        public SortResult<int> Sort(int[] input, bool logSteps = false, SortDirection direction = SortDirection.Ascending)
        {
            if (input == null || input.Length == 0)
                throw new ArgumentException("Input array is null or empty.");

            int[] array = (int[])input.Clone();
            List<string> steps = logSteps ? new List<string>() : null;
            int swaps = 0, comparisons = 0;

            var stopwatch = Stopwatch.StartNew();

            int max = Math.Max(Math.Abs(array[0]), 1);
            foreach (var val in array)
                max = Math.Max(max, Math.Abs(val));

            int exp = 1; 

            while (max / exp > 0)
            {
                CountingSortByDigit(array, exp, direction, steps, ref swaps);
                exp *= 10;
            }

            stopwatch.Stop();

            return new SortResult<int>
            {
                SortedArray = array,
                Steps = steps ?? new List<string>(),
                ComparisonCount = comparisons,
                SwapCount = swaps,
                Duration = stopwatch.Elapsed
            };
        }

        private void CountingSortByDigit(int[] array, int exp, SortDirection direction,
            List<string> steps, ref int swaps)
        {
            int n = array.Length;
            int[] output = new int[n];
            int[] count = new int[19];

            for (int i = 0; i < n; i++)
            {
                int digit = (array[i] / exp) % 10 + 9;
                count[digit]++;
            }

            if (direction == SortDirection.Ascending)
            {
                for (int i = 1; i < 19; i++)
                    count[i] += count[i - 1];
            }
            else
            {
                for (int i = 17; i >= 0; i--)
                    count[i] += count[i + 1];
            }

            for (int i = n - 1; i >= 0; i--)
            {
                int digit = (array[i] / exp) % 10 + 9;
                int pos = --count[digit];
                output[pos] = array[i];
                swaps++;

                if (steps != null)
                    steps.Add($"Digit {exp}, placed {array[i]} at index {pos} → [{string.Join(", ", output)}]");
            }

            Array.Copy(output, array, n);
        }
    }
}
