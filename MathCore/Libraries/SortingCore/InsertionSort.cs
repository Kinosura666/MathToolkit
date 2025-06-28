using MathCore.Common;
using MathCore.Extentions;
using MathCore.Interfaces;
using MathCore.Models.SortResults;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MathCore.Libraries.SortingCore
{
    public class InsertionSort<T> : ISortAlgorithm<T> where T : IComparable<T>
    {
        public SortResult<T> Sort(T[] input, bool logSteps = false, SortDirection direction = SortDirection.Ascending)
        {
            T[] array = (T[])input.Clone();
            List<string> steps = logSteps ? new List<string>() : null;

            int swaps = 0, comparisons = 0;
            var stopwatch = Stopwatch.StartNew();

            for (int i = 1; i < array.Length; i++)
            {
                T key = array[i];
                int j = i - 1;
                bool moved = false;

                while (j >= 0 && SortingExtensions.Compare(array[j], key, direction) > 0)
                {
                    comparisons++;
                    T movedValue = array[j];
                    int toIndex = j + 1;

                    if (logSteps)
                        steps.Add($"Moved {movedValue} from index {j} to {toIndex} → [{string.Join(", ", array)}]");

                    array[toIndex] = movedValue;
                    j--;
                    swaps++;
                    moved = true;
                }

                if (j >= 0) comparisons++;

                array[j + 1] = key;

                if (logSteps && moved)
                    steps.Add($"Inserted {key} at position {j + 1} → [{string.Join(", ", array)}]");
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
