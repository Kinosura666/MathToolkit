using Desktop.Services;
using Desktop.ViewModels;
using MathCore.Interfaces;
using MathCore.Libraries.SortingCore;

public static class SortingExecutorService
{
    public static bool TryExecuteSort(string methodName, SortViewModel vm)
    {
        if (vm == null || string.IsNullOrWhiteSpace(vm.SortInput))
        {
            vm.SetResultText?.Invoke("Input is empty or context is invalid.");
            return false;
        }

        switch (methodName)
        {
            case "Bubble Sort":
                return Run(vm, () => new BubbleSort<int>(), () => new BubbleSort<double>());
            case "Insertion Sort":
                return Run(vm, () => new InsertionSort<int>(), () => new InsertionSort<double>());
            case "Selection Sort":
                return Run(vm, () => new SelectionSort<int>(), () => new SelectionSort<double>());
            case "Quick Sort":
                return Run(vm, () => new QuickSort<int>(), () => new QuickSort<double>());
            case "Counting Sort":
                return Run(vm, () => new CountingSort(), null); // only int
            case "Shell Sort":
                return Run(vm, () => new ShellSort<int>(), () => new ShellSort<double>());
            case "Merge Sort":
                return Run(vm, () => new MergeSort<int>(), () => new MergeSort<double>());
            case "Heap Sort":
                return Run(vm, () => new HeapSort<int>(), () => new HeapSort<double>());
            case "Smooth Sort":
                return Run(vm, () => new SmoothSort<int>(), null); // only int
            case "LSD Radix Sort":
                return Run(vm, () => new LsdRadixSort(), null); // only int
            case "MSD Radix Sort":
                return Run(vm, () => new MsdRadixSort(), null); // only int
            case "Bucket Sort":
                return Run(vm, null, () => new BucketSort()); // only double

            default:
                vm.SetResultText?.Invoke($"Method \"{methodName}\" not recognized.");
                return false;
        }
    }

    private static bool Run(
        SortViewModel vm,
        Func<ISortAlgorithm<int>>? intFactory,
        Func<ISortAlgorithm<double>>? doubleFactory)
    {
        if (SortService.IsAllInt(vm.SortInput))
            return RunInt(vm, intFactory);
        else if (SortService.IsAllDouble(vm.SortInput))
            return RunDouble(vm, doubleFactory);
        else
        {
            vm.SetResultText?.Invoke("Enter only int or double values (not mixed).");
            return false;
        }
    }

    private static bool RunInt(SortViewModel vm, Func<ISortAlgorithm<int>>? factory)
    {
        if (factory == null)
        {
            vm.SetResultText?.Invoke("This sorting method does not support int.");
            return false;
        }

        var result = SortService.RunSort(vm.SortInput, factory);
        vm.SetResultText?.Invoke(SortService.FormatSortResult(result));
        return true;
    }

    private static bool RunDouble(SortViewModel vm, Func<ISortAlgorithm<double>>? factory)
    {
        if (factory == null)
        {
            vm.SetResultText?.Invoke("This sorting method does not support double.");
            return false;
        }

        var result = SortService.RunSort(vm.SortInput, factory);
        vm.SetResultText?.Invoke(SortService.FormatSortResult(result));
        return true;
    }
}
