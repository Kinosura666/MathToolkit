using System;
using System.Collections.Generic;
using System.Diagnostics;
using MathCore.Common;
using MathCore.Extentions;
using MathCore.Interfaces;
using MathCore.Models.SortResults;

namespace MathCore.Libraries.SortingCore
{
    public class BubbleSort<T> : ISortAlgorithm<T> where T : IComparable<T>
    {
        public SortResult<T> Sort(T[] input, bool logSteps = false, SortDirection direction = SortDirection.Ascending)
        {
            T[] array = (T[])input.Clone();
            var steps = logSteps ? new List<string>() : null;
            int comparisons = 0, swaps = 0;
            bool swapped;
            var sw = Stopwatch.StartNew();

            for (int i = 0; i < array.Length - 1; i++)
            {
                swapped = false;
                for (int j = 0; j < array.Length - i - 1; j++)
                {
                    comparisons++;
                    if (SortingExtensions.Compare(array[j], array[j + 1], direction) > 0)
                    {
                        SortingExtensions.Swap(array, j, j + 1);
                        swaps++;
                        swapped = true;

                        if (logSteps)
                            steps!.Add($"Swapped {array[j + 1]} and {array[j]} → [{string.Join(", ", array)}]");
                    }
                }

                if (!swapped)
                    break;
            }

            sw.Stop();
            return new SortResult<T>
            {
                SortedArray = array,
                Steps = steps ?? new List<string>(),
                ComparisonCount = comparisons,
                SwapCount = swaps,
                Duration = sw.Elapsed
            };
        }
    }
}
