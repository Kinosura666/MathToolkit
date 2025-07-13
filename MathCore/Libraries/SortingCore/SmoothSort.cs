using System;
using System.Collections.Generic;
using MathCore.Common;
using MathCore.Interfaces;
using MathCore.Models.SortResults;

namespace MathCore.Libraries.SortingCore 
{ 
    public sealed class SmoothSort<T> : ISortAlgorithm<T> where T : IComparable<T>
    {
        public SortResult<T> Sort(T[] input, bool logSteps = false, SortDirection direction = SortDirection.Ascending)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));

            var a = (T[])input.Clone();

            List<string>? steps = logSteps ? new() : null;
            int compares = 0, swaps = 0;

            var started = DateTime.UtcNow;
            var heapSizes = new List<int>();

            for (int i = 0; i < a.Length; i++)
            {
                heapSizes.Add(1);

                if (heapSizes.Count >= 2 &&
                    heapSizes[^1] == heapSizes[^2] + 1)
                {
                    heapSizes[^2] = heapSizes[^2] + 1;
                    heapSizes.RemoveAt(heapSizes.Count - 1);
                }

                Trinkle(a, i, heapSizes, direction, ref compares, ref swaps, steps);
            }

            for (int i = a.Length - 1; i >= 0; i--)
            {
                int size = heapSizes[^1];
                heapSizes.RemoveAt(heapSizes.Count - 1);

                if (size > 1)
                {
                    heapSizes.Add(size - 2);
                    heapSizes.Add(size - 1);

                    Trinkle(a, i - 1, heapSizes, direction, ref compares, ref swaps, steps);
                    Trinkle(a, i - 1 - Leonardo(size - 2), heapSizes,
                            direction, ref compares, ref swaps, steps);
                }
            }
            if (direction == SortDirection.Ascending)
                Array.Reverse(a);

            return new SortResult<T>
            {
                SortedArray = a,
                Steps = steps ?? new(),
                ComparisonCount = compares,
                SwapCount = swaps,
                Duration = DateTime.UtcNow - started,
                Pivots = new(), 
                PivotIndices = new()
            };
        }

        private static readonly int[] _leos = BuildLeonardoTable(46);

        private static int Leonardo(int k) => _leos[k];

        private static int Compare(T x, T y, SortDirection dir, ref int cmp)
        {
            cmp++;
            return dir == SortDirection.Ascending
                ? x.CompareTo(y)
                : y.CompareTo(x);
        }

        private static void Swap(T[] a, int i, int j, ref int swp,
                                 List<string>? steps)
        {
            (a[i], a[j]) = (a[j], a[i]);
            swp++;
            if (steps is not null)
                steps.Add($"Swapped {i} with {j} → [{string.Join(", ", a)}]");
        }

        
        private static void Sift(T[] a, int root, int order,
                                 SortDirection dir, ref int cmp, ref int swp,
                                 List<string>? steps)
        {
            while (order > 1) 
            {
                int rChild = root - 1; 
                int lChild = root - 1 - Leonardo(order - 2);
                int larger = Compare(a[lChild], a[rChild], dir, ref cmp) > 0 ? lChild : rChild;

                if (Compare(a[root], a[larger], dir, ref cmp) >= 0) break;

                Swap(a, root, larger, ref swp, steps);
                root = larger;
                order = larger == rChild ? order - 1 : order - 2;
            }
        }

        private static void Trinkle(T[] a, int index, IList<int> heapSizes,
                                    SortDirection dir, ref int cmp, ref int swp,
                                    List<string>? steps)
        {
            int cur = index;
            int hIdx = heapSizes.Count - 1;   
            int order = heapSizes[hIdx];

            while (hIdx > 0)
            {
                int parent = cur - Leonardo(order);
                if (Compare(a[parent], a[cur], dir, ref cmp) >= 0) break;

                Swap(a, cur, parent, ref swp, steps);
                cur = parent;
                hIdx -= 1;
                order = heapSizes[hIdx];
            }

            Sift(a, cur, order, dir, ref cmp, ref swp, steps);
        }

        private static int[] BuildLeonardoTable(int n)
        {
            var arr = new int[n];
            arr[0] = arr[1] = 1;
            for (int k = 2; k < n; k++)
                arr[k] = arr[k - 1] + arr[k - 2] + 1;
            return arr;
        }
    }
}