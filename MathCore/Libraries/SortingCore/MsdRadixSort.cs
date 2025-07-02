using System;
using System.Collections.Generic;
using System.Diagnostics;
using MathCore.Common;
using MathCore.Extentions;
using MathCore.Interfaces;
using MathCore.Models.SortResults;

namespace MathCore.Libraries.SortingCore
{
    public class MsdRadixSort : ISortAlgorithm<int>
    {
        public SortResult<int> Sort(int[] input, bool logSteps = false, SortDirection direction = SortDirection.Ascending)
        {
            if (input == null || input.Length == 0)
                throw new ArgumentException("Input array is null or empty.");

            int[] array = (int[])input.Clone();
            List<string> steps = logSteps ? new List<string>() : null;
            int swaps = 0;

            var stopwatch = Stopwatch.StartNew();

            int max = Math.Max(Math.Abs(array[0]), 1);
            foreach (var val in array)
                max = Math.Max(max, Math.Abs(val));

            int maxExp = 1;
            while (max / maxExp >= 10)
                maxExp *= 10;

            MSD(array, 0, array.Length, maxExp, direction, steps, ref swaps);

            stopwatch.Stop();

            return new SortResult<int>
            {
                SortedArray = array,
                Steps = steps ?? new List<string>(),
                ComparisonCount = 0,
                SwapCount = swaps,
                Duration = stopwatch.Elapsed
            };
        }

        private void MSD(int[] array, int left, int right, int exp, SortDirection direction, List<string> steps, ref int swaps)
        {
            if (right - left <= 1 || exp == 0)
                return;

            int[] count = new int[19];
            int[] aux = new int[right - left];

            for (int i = left; i < right; i++)
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

            for (int i = right - 1; i >= left; i--)
            {
                int digit = (array[i] / exp) % 10 + 9;
                int pos = --count[digit];
                aux[pos] = array[i];
                swaps++;
                if (steps != null)
                    steps.Add($"Exp {exp}, placed {array[i]} at local {pos} → [{string.Join(", ", aux)}]");
            }

            for (int i = 0; i < aux.Length; i++)
                array[left + i] = aux[i];

            int start = 0;
            for (int d = 0; d < 19; d++)
            {
                int size = (d == 0 ? count[d] : count[d] - count[d - 1]);
                if (size > 1)
                    MSD(array, left + start, left + start + size, exp / 10, direction, steps, ref swaps);
                start += size;
            }
        }
    }
}
