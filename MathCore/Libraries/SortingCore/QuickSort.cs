using MathCore.Common;
using MathCore.Extentions;
using MathCore.Interfaces;
using MathCore.Models.SortResults;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MathCore.Libraries.SortingCore
{
    public class QuickSort<T> : ISortAlgorithm<T> where T : IComparable<T>
    {
        public SortResult<T> Sort(T[] input, bool logSteps = false, SortDirection direction = SortDirection.Ascending)
        {
            if (input == null || input.Length == 0)
                throw new ArgumentException("Input array is null or empty.");

            var array = (T[])input.Clone();
            var stopwatch = Stopwatch.StartNew();

            var steps = logSteps ? new List<string>() : null;
            var pivots = logSteps ? new List<T>() : null;
            var pivotIndices = logSteps ? new List<int>() : null;

            int comparisons = 0;
            int swaps = 0;

            void QuickSortRecursive(int low, int high)
            {
                if (low < high)
                {
                    int pi = Partition(low, high);
                    QuickSortRecursive(low, pi - 1);
                    QuickSortRecursive(pi + 1, high);
                }
            }

            int Partition(int low, int high)
            {
                T pivot = array[high];
                int i = low - 1;

                if (logSteps)
                {
                    pivots.Add(pivot);
                    pivotIndices.Add(high);
                }

                for (int j = low; j < high; j++)
                {
                    comparisons++;
                    bool condition = SortingExtensions.Compare(array[j], pivot, direction) <= 0;

                    if (condition)
                    {
                        i++;
                        if (i != j)
                        {
                            SortingExtensions.Swap(array, i, j);
                            swaps++;
                            if (logSteps)
                                steps.Add($"Swap {i} <-> {j}: [{string.Join(", ", array)}]");
                        }
                    }
                }

                if ((i + 1) != high)
                {
                    SortingExtensions.Swap(array, i + 1, high);
                    swaps++;
                    if (logSteps)
                        steps.Add($"Move {pivot} (pivot) to index {i + 1}: [{string.Join(", ", array)}]");
                }

                return i + 1;
            }

            QuickSortRecursive(0, array.Length - 1);
            stopwatch.Stop();

            return new SortResult<T>
            {
                SortedArray = array,
                ComparisonCount = comparisons,
                SwapCount = swaps,
                Duration = stopwatch.Elapsed,
                Steps = steps ?? new List<string>(),
                Pivots = pivots ?? new List<T>(),
                PivotIndices = pivotIndices ?? new List<int>()
            };
        }
    }
}
