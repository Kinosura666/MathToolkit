using System;
using System.Collections.Generic;
using System.Diagnostics;
using MathCore.Common;
using MathCore.Extentions;
using MathCore.Interfaces;
using MathCore.Models.SortResults;

namespace MathCore.Libraries.SortingCore
{
    public class HeapSort<T> : ISortAlgorithm<T> where T : IComparable<T>
    {
        public SortResult<T> Sort(T[] input, bool logSteps = false, SortDirection direction = SortDirection.Ascending)
        {
            T[] array = (T[])input.Clone();
            var steps = logSteps ? new List<string>() : null;
            int comparisons = 0, swaps = 0;

            var stopwatch = Stopwatch.StartNew();

            int n = array.Length;

            for (int i = n / 2 - 1; i >= 0; i--)
                Heapify(array, n, i);

            for (int i = n - 1; i > 0; i--)
            {
                SortingExtensions.Swap(array, 0, i);
                swaps++;
                if (logSteps)
                    steps?.Add($"Swapped root with index {i} → [{string.Join(", ", array)}]");

                Heapify(array, i, 0);
            }

            stopwatch.Stop();

            if (direction == SortDirection.Descending)
                Array.Reverse(array);

            return new SortResult<T>
            {
                SortedArray = array,
                Steps = steps ?? new List<string>(),
                ComparisonCount = comparisons,
                SwapCount = swaps,
                Duration = stopwatch.Elapsed
            };

            void Heapify(T[] arr, int heapSize, int root)
            {
                int largest = root;
                int left = 2 * root + 1;
                int right = 2 * root + 2;

                if (left < heapSize && SortingExtensions.Compare(arr[left], arr[largest], SortDirection.Ascending) > 0)
                {
                    largest = left;
                    comparisons++;
                }

                if (right < heapSize && SortingExtensions.Compare(arr[right], arr[largest], SortDirection.Ascending) > 0)
                {
                    largest = right;
                    comparisons++;
                }

                if (largest != root)
                {
                    SortingExtensions.Swap(arr, root, largest);
                    swaps++;
                    if (logSteps)
                        steps?.Add($"Heapified index {root} with child {largest} → [{string.Join(", ", arr)}]");

                    Heapify(arr, heapSize, largest);
                }
            }
        }
    }
}
