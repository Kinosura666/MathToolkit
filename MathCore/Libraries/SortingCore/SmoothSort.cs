using System;
using System.Collections.Generic;
using System.Diagnostics;
using MathCore.Common;
using MathCore.Extentions;
using MathCore.Interfaces;
using MathCore.Models.SortResults;

namespace MathCore.Libraries.SortingCore
{
    public class SmoothSort<T> : ISortAlgorithm<T> where T : IComparable<T>
    {
        private static readonly List<int> LeonardoNumbers = GenerateLeonardoNumbers(64);

        private static List<int> GenerateLeonardoNumbers(int max)
        {
            var list = new List<int> { 1, 1 };
            while (true)
            {
                int next = list[^1] + list[^2] + 1;
                if (next > max) break;
                list.Add(next);
            }
            return list;
        }

        public SortResult<T> Sort(T[] input, bool logSteps = false, SortDirection direction = SortDirection.Ascending)
        {
            T[] array = (T[])input.Clone();
            List<string> steps = logSteps ? new List<string>() : null;
            int comparisons = 0, swaps = 0;
            var stopwatch = Stopwatch.StartNew();

            int n = array.Length;
            var heaps = new List<int>(); 

            for (int i = 0; i < n; i++)
            {
                AddHeap(heaps);
                Sift(array, i, heaps, ref comparisons, ref swaps, direction, steps);
                Trinkle(array, i, heaps, ref comparisons, ref swaps, direction, steps);
            }

            for (int i = n - 1; i >= 0; i--)
            {
                if (heaps.Count == 0) break;

                int order = heaps[^1];
                heaps.RemoveAt(heaps.Count - 1);

                if (order >= 2)
                {
                    int right = i - 1;
                    int left = i - 1 - LeonardoNumbers[order - 2];
                    heaps.Add(order - 1);
                    Trinkle(array, left, heaps, ref comparisons, ref swaps, direction, steps);
                    heaps.Add(order - 2);
                    Trinkle(array, right, heaps, ref comparisons, ref swaps, direction, steps);
                }
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
        }

        private void AddHeap(List<int> heaps)
        {
            int count = heaps.Count;
            if (count >= 2 &&
                heaps[count - 1] == heaps[count - 2] + 1)
            {
                int newOrder = heaps[count - 1] + 1;
                heaps.RemoveAt(count - 1);
                heaps[count - 2] = newOrder;
            }
            else if (heaps.Count > 0 && heaps[^1] == 1)
                heaps.Add(0);
            else
                heaps.Add(1);
        }

        private void Sift(T[] array, int end, List<int> heaps, ref int comparisons, ref int swaps,
                          SortDirection direction, List<string> steps)
        {
            int order = heaps[^1];
            while (order >= 2)
            {
                int r = end;
                int l = end - 1 - LeonardoNumbers[order - 2];
                int m = end - 1;

                int max = r;

                if (SortingExtensions.Compare(array[l], array[max], direction) > 0)
                    max = l;

                if (SortingExtensions.Compare(array[m], array[max], direction) > 0)
                    max = m;

                comparisons += 2;
                if (max == r) break;

                SortingExtensions.Swap(array, r, max);
                swaps++;
                if (steps != null)
                    steps.Add($"Sifted {array[max]} up to index {r} → [{string.Join(", ", array)}]");

                end = max;
                order = order == 1 ? 0 : order - (max == l ? 1 : 2);
            }
        }

        private void Trinkle(T[] array, int end, List<int> heaps, ref int comparisons, ref int swaps,
                             SortDirection direction, List<string> steps)
        {
            int order = heaps[^1];
            while (heaps.Count > 1)
            {
                int h2 = heaps[^2];
                int parent = end - LeonardoNumbers[order];

                if (SortingExtensions.Compare(array[parent], array[end], direction) <= 0)
                    break;

                SortingExtensions.Swap(array, parent, end);
                swaps++;
                if (steps != null)
                    steps.Add($"Trinkle swapped {array[end]} with parent at {parent} → [{string.Join(", ", array)}]");

                end = parent;
                order = h2;
                heaps.RemoveAt(heaps.Count - 1);
            }

            Sift(array, end, heaps, ref comparisons, ref swaps, direction, steps);
        }
    }
}
