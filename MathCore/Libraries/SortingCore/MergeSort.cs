using System;
using System.Collections.Generic;
using System.Diagnostics;
using MathCore.Common;
using MathCore.Extentions;
using MathCore.Interfaces;
using MathCore.Models.SortResults;

namespace MathCore.Libraries.SortingCore
{
    public class MergeSort<T> : ISortAlgorithm<T> where T : IComparable<T>
    {
        public SortResult<T> Sort(T[] input, bool logSteps = false, SortDirection direction = SortDirection.Ascending)
        {
            T[] array = (T[])input.Clone();
            List<string> steps = logSteps ? new List<string>() : null;
            int comparisons = 0, swaps = 0;

            var stopwatch = Stopwatch.StartNew();

            void MergeSortRecursive(T[] arr, int left, int right)
            {
                if (left < right)
                {
                    int mid = (left + right) / 2;
                    MergeSortRecursive(arr, left, mid);
                    MergeSortRecursive(arr, mid + 1, right);
                    Merge(arr, left, mid, right);
                }
            }

            void Merge(T[] arr, int left, int mid, int right)
            {
                int n1 = mid - left + 1;
                int n2 = right - mid;

                T[] L = new T[n1];
                T[] R = new T[n2];

                Array.Copy(arr, left, L, 0, n1);
                Array.Copy(arr, mid + 1, R, 0, n2);

                int i = 0, j = 0, k = left;

                while (i < n1 && j < n2)
                {
                    comparisons++;
                    if (SortingExtensions.Compare(L[i], R[j], direction) <= 0)
                    {
                        arr[k] = L[i];
                        i++;
                    }
                    else
                    {
                        arr[k] = R[j];
                        j++;
                    }
                    swaps++;
                    if (logSteps)
                        steps.Add($"Merged index {k} ← {arr[k]} → [{string.Join(", ", arr)}]");
                    k++;
                }

                while (i < n1)
                {
                    arr[k] = L[i++];
                    k++;
                    swaps++;
                    if (logSteps)
                        steps.Add($"Copied leftover {arr[k - 1]} from L → [{string.Join(", ", arr)}]");
                }

                while (j < n2)
                {
                    arr[k] = R[j++];
                    k++;
                    swaps++;
                    if (logSteps)
                        steps.Add($"Copied leftover {arr[k - 1]} from R → [{string.Join(", ", arr)}]");
                }
            }

            MergeSortRecursive(array, 0, array.Length - 1);
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
