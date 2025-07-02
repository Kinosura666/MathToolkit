using MathCore.Common;
using MathCore.Interfaces;
using MathCore.Models.SortResults;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using MathCore.Extentions;

namespace MathCore.Libraries.SortingCore
{
    public class SelectionSort<T> : ISortAlgorithm<T> where T : IComparable<T>
    {
        public SortResult<T> Sort(T[] input, bool logSteps = false, SortDirection direction = SortDirection.Ascending)
        {
            T[] array = (T[])input.Clone();
            List<string> steps = logSteps ? new List<string>() : null;
            int comparisons = 0, swaps = 0;

            var stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < array.Length - 1; i++)
            {
                int selectedIndex = i;

                for (int j = i + 1; j < array.Length; j++)
                {
                    comparisons++;
                    if (SortingExtensions.Compare(array[j], array[selectedIndex], direction) < 0)
                    {
                        selectedIndex = j;
                    }
                }

                if (selectedIndex != i)
                {
                    SortingExtensions.Swap(array, i, selectedIndex);
                    swaps++;

                    if (logSteps)
                        steps.Add($"Swapped index {i} and {selectedIndex} → [{string.Join(", ", array)}]");
                }
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
