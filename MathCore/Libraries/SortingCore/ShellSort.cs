using System;
using System.Collections.Generic;
using System.Diagnostics;
using MathCore.Common;
using MathCore.Extentions;
using MathCore.Interfaces;
using MathCore.Models.SortResults;

namespace MathCore.Libraries.SortingCore
{
    public class ShellSort<T> : ISortAlgorithm<T> where T : IComparable<T>
    {
        public SortResult<T> Sort(T[] input, bool logSteps = false, SortDirection direction = SortDirection.Ascending)
        {
            T[] array = (T[])input.Clone();
            var steps = logSteps ? new List<string>() : null;
            int comparisons = 0, swaps = 0;
            var stopwatch = Stopwatch.StartNew();

            int n = array.Length;
            int gap = n / 2;

            while (gap > 0)
            {
                for (int i = gap; i < n; i++)
                {
                    T temp = array[i];
                    int j = i;

                    while (j >= gap && SortingExtensions.Compare(array[j - gap], temp, direction) > 0)
                    {
                        comparisons++;
                        array[j] = array[j - gap];
                        swaps++;

                        if (logSteps)
                            steps?.Add($"Moved {array[j - gap]} to index {j} → [{string.Join(", ", array)}]");

                        j -= gap;
                    }

                    if (j >= gap) comparisons++;

                    array[j] = temp;
                    if (logSteps && j != i)
                        steps?.Add($"Inserted {temp} at index {j} → [{string.Join(", ", array)}]");
                }

                gap /= 2;
            }

            stopwatch.Stop();
            return new SortResult<T>
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
