using MathCore.Common;
using MathCore.Interfaces;
using MathCore.Models.SortResults;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MathCore.Libraries.SortingCore
{
    public class CountingSort : ISortAlgorithm<int>
    {
        public SortResult<int> Sort(int[] input, bool logSteps = false, SortDirection direction = SortDirection.Ascending)
        {
            if (input == null || input.Length == 0)
                throw new ArgumentException("Input array is null or empty.");

            var array = (int[])input.Clone();
            var steps = logSteps ? new List<string>() : null;
            var stopwatch = Stopwatch.StartNew();

            int min = array[0], max = array[0];
            foreach (var val in array)
            {
                if (val < min) min = val;
                if (val > max) max = val;
            }

            int[] count = new int[max - min + 1];

            foreach (var val in array)
                count[val - min]++;

            int index = 0;
            int comparisons = array.Length; 
            int swaps = 0;

            if (direction == SortDirection.Ascending)
            {
                for (int i = 0; i < count.Length; i++)
                {
                    while (count[i]-- > 0)
                    {
                        array[index++] = i + min;
                        swaps++;
                        if (logSteps)
                            steps.Add($"Placed {i + min} at index {index - 1} → [{string.Join(", ", array)}]");
                    }
                }
            }
            else 
            {
                for (int i = count.Length - 1; i >= 0; i--)
                {
                    while (count[i]-- > 0)
                    {
                        array[index++] = i + min;
                        swaps++;
                        if (logSteps)
                            steps.Add($"Placed {i + min} at index {index - 1} → [{string.Join(", ", array)}]");
                    }
                }
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
    }
}
