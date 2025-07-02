using System;
using System.Collections.Generic;
using System.Diagnostics;
using MathCore.Common;
using MathCore.Extentions;
using MathCore.Interfaces;
using MathCore.Models.SortResults;

namespace MathCore.Libraries.SortingCore
{
    public class BucketSort : ISortAlgorithm<double>
    {
        public SortResult<double> Sort(double[] input, bool logSteps = false, SortDirection direction = SortDirection.Ascending)
        {
            if (input == null || input.Length == 0)
                throw new ArgumentException("Input array is null or empty.");

            double[] array = (double[])input.Clone();
            List<string> steps = logSteps ? new List<string>() : null;
            int comparisons = 0, swaps = 0;

            var stopwatch = Stopwatch.StartNew();

            int n = array.Length;
            var buckets = new List<double>[n];
            for (int i = 0; i < n; i++)
                buckets[i] = new List<double>();

            for (int i = 0; i < n; i++)
            {
                int index = (int)(array[i] * n);
                index = Math.Min(index, n - 1); 
                buckets[index].Add(array[i]);
                if (logSteps)
                    steps.Add($"Placed {array[i]:F4} in bucket {index}");
            }

            for (int i = 0; i < n; i++)
            {
                buckets[i].Sort(); 
                comparisons += buckets[i].Count * (buckets[i].Count - 1) / 2;
                swaps += buckets[i].Count; 
            }

            int idx = 0;
            if (direction == SortDirection.Ascending)
            {
                for (int i = 0; i < n; i++)
                {
                    foreach (var val in buckets[i])
                    {
                        array[idx++] = val;
                        if (logSteps)
                            steps.Add($"Collected {val:F4} → [{string.Join(", ", array)}]");
                    }
                }
            }
            else
            {
                for (int i = n - 1; i >= 0; i--)
                {
                    foreach (var val in buckets[i])
                    {
                        array[idx++] = val;
                        if (logSteps)
                            steps.Add($"Collected {val:F4} (descending) → [{string.Join(", ", array)}]");
                    }
                }
            }

            stopwatch.Stop();

            return new SortResult<double>
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
