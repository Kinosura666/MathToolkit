using MathCore.Interfaces;
using MathCore.Libraries.SortingCore;
using Web.Interfaces;

namespace Web.Services
{
    public class SortFactory : ISortFactory
    {
        public ISortAlgorithm<T> Resolve<T>(string algorithm) where T : IComparable<T>
        {
            return algorithm.ToLower() switch
            {
                "bubble" when typeof(T) == typeof(int) || typeof(T) == typeof(double) 
                    => new BubbleSort<T>(),
                "insertion" when typeof(T) == typeof(int) || typeof(T) == typeof(double)
                    => new InsertionSort<T>(),
                "quick" when typeof(T) == typeof(int) || typeof(T) == typeof(double)
                    => new QuickSort<T>(),
                "selection" when typeof(T) == typeof(int) || typeof(T) == typeof(double)
                    => new SelectionSort<T>(),
                "counting" when typeof(T) == typeof(int)
                    => (ISortAlgorithm<T>)(object) new CountingSort(),
                "shell" when typeof(T) == typeof(int) || typeof(T) == typeof(double)
                    => new ShellSort<T>(),
                "merge" when typeof(T) == typeof(int) || typeof(T) == typeof(double)
                    => new MergeSort<T>(),
                "heap" when typeof(T) == typeof(int) || typeof(T) == typeof(double)
                    => new HeapSort<T>(),
                "smooth" when typeof(T) == typeof(int) || typeof(T) == typeof(double)
                    => new SmoothSort<T>(),
                "lsdradix" when typeof(T) == typeof(int)
                    => (ISortAlgorithm<T>)(object) new LsdRadixSort(),
                "msdradix" when typeof(T) == typeof(int) 
                    => (ISortAlgorithm<T>)(object) new MsdRadixSort(),
                "bucket" when typeof(T) == typeof(double)
                    => (ISortAlgorithm<T>)(object) new BucketSort(),
                _ => throw new ArgumentException($"Unsupported algorithm '{algorithm}' for type {typeof(T).Name}")
            };
        }
    }
}
