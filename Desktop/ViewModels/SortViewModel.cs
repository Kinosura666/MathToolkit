using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MathCore.Libraries.SortingCore;

namespace Desktop.ViewModels
{
    public class SortViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Action<string>? SetResultText { get; set; }

        private string _sortInput = string.Empty;
        public string SortInput
        {
            get => _sortInput;
            set { _sortInput = value; OnPropertyChanged(); }
        }

        private int _minValue = 0;
        public int MinValue
        {
            get => _minValue;
            set { _minValue = value; OnPropertyChanged(); }
        }

        private int _maxValue = 100;
        public int MaxValue
        {
            get => _maxValue;
            set { _maxValue = value; OnPropertyChanged(); }
        }

        public int SortSize { get; set; } = 10;
        public int PartiallySortedSize { get; set; } = 10;
        public int SortedPercent { get; set; } = 10;
        public int DuplicateArraySize { get; set; } = 10;
        public int DuplicatePercent { get; set; } = 50;
        public bool UseDouble { get; set; } = false;

        public ICommand GenerateCommand { get; }
        public ICommand ClearSortCommand { get; }
        public ICommand GeneratePartiallySortedCommand { get; }
        public ICommand GenerateWithDuplicatesCommand { get; }

        public SortViewModel()
        {
            GenerateCommand = new RelayCommand(_ => Generate());
            ClearSortCommand = new RelayCommand(_ => ClearSort());
            GeneratePartiallySortedCommand = new RelayCommand(_ => GeneratePartiallySorted());
            GenerateWithDuplicatesCommand = new RelayCommand(_ => GenerateWithDuplicates());
        }

        private void Generate()
        {
            if (MinValue >= MaxValue)
            {
                SetResultText?.Invoke("Invalid range: Min should be less than Max.");
                return;
            }

            Random rnd = new Random();
            if (UseDouble)
            {
                double[] array = Enumerable.Range(0, SortSize)
                    .Select(_ => Math.Round(rnd.NextDouble() * (MaxValue - MinValue) + MinValue, 2))
                    .ToArray();
                SortInput = string.Join(" ", array);
                SetResultText?.Invoke($"Generated double array ({SortSize} elements):\n{SortInput}");
            }
            else
            {
                int[] array = Enumerable.Range(0, SortSize)
                    .Select(_ => rnd.Next(MinValue, MaxValue + 1))
                    .ToArray();
                SortInput = string.Join(" ", array);
                SetResultText?.Invoke($"Generated int array ({SortSize} elements):\n{SortInput}");
            }
        }

        private void GeneratePartiallySorted()
        {
            if (MinValue >= MaxValue)
            {
                SetResultText?.Invoke("Invalid range: Min should be less than Max.");
                return;
            }
            Random rnd = new Random();
            int sortedCount = PartiallySortedSize * SortedPercent / 100;

            if (UseDouble)
            {
                double[] full = Enumerable.Range(0, PartiallySortedSize)
                    .Select(_ => Math.Round(rnd.NextDouble() * (MaxValue - MinValue) + MinValue, 2))
                    .ToArray();
                Array.Sort(full, 0, sortedCount);
                SortInput = string.Join(" ", full.Select(x => x.ToString("0.##", CultureInfo.InvariantCulture)));
                SetResultText?.Invoke($"Generated partially sorted double array ({SortedPercent}% sorted):\n{SortInput}");
            }
            else
            {
                int[] full = Enumerable.Range(0, PartiallySortedSize)
                    .Select(_ => rnd.Next(MinValue, MaxValue + 1))
                    .ToArray();
                Array.Sort(full, 0, sortedCount);
                SortInput = string.Join(" ", full);
                SetResultText?.Invoke($"Generated partially sorted int array ({SortedPercent}% sorted):\n{SortInput}");
            }
        }

        private void GenerateWithDuplicates()
        {
            if (MinValue >= MaxValue)
            {
                SetResultText?.Invoke("Invalid range: Min should be less than Max.");
                return;
            }
            Random rnd = new Random();
            int targetDuplicates = DuplicateArraySize * DuplicatePercent / 100;

            if (UseDouble)
            {
                GenerateDuplicates<double>(DuplicateArraySize, targetDuplicates,
                    () => Math.Round(rnd.NextDouble() * (MaxValue - MinValue) + MinValue, 2),
                    x => x.ToString("0.##", CultureInfo.InvariantCulture));
            }
            else
            {
                GenerateDuplicates<int>(DuplicateArraySize, targetDuplicates,
                    () => rnd.Next(MinValue, MaxValue + 1),
                    x => x.ToString());
            }
        }

        private void GenerateDuplicates<T>(int size, int targetDuplicates, Func<T> generator, Func<T, string> formatter)
            where T : notnull
        {
            Random rnd = new Random();
            var uniqueSet = new HashSet<T>();
            while (uniqueSet.Count < size)
                uniqueSet.Add(generator());

            var array = uniqueSet.ToList();
            var counts = array.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
            int currentDuplicates = counts.Values.Sum(c => c - 1);

            while (currentDuplicates < targetDuplicates)
            {
                var uniques = array.Select((val, i) => new { val, i })
                                   .GroupBy(x => x.val)
                                   .Where(g => g.Count() == 1)
                                   .Select(g => g.First().i)
                                   .ToList();

                if (uniques.Count == 0) break;

                int replaceIndex = uniques[rnd.Next(uniques.Count)];
                T oldValue = array[replaceIndex];
                var candidates = counts.Keys.OrderBy(_ => rnd.Next()).ToList();
                bool replaced = false;

                foreach (var valToRepeat in candidates)
                {
                    int added = counts.ContainsKey(valToRepeat) && counts[valToRepeat] >= 1 ? 1 : 0;
                    if (currentDuplicates + added > targetDuplicates) continue;

                    array[replaceIndex] = valToRepeat;

                    counts[oldValue]--;
                    if (counts[oldValue] == 0) counts.Remove(oldValue);
                    if (!counts.ContainsKey(valToRepeat)) counts[valToRepeat] = 0;
                    counts[valToRepeat]++;

                    currentDuplicates += added;
                    replaced = true;
                    break;
                }

                if (!replaced) break;
            }

            array = array.OrderBy(_ => rnd.Next()).ToList();
            SortInput = string.Join(" ", array.Select(formatter));
            SetResultText?.Invoke($"Generated {(typeof(T) == typeof(double) ? "double" : "int")} array with approx. {DuplicatePercent}% duplicated values ({currentDuplicates} duplicates out of {size}):\n{SortInput}");
        }

        private void ClearSort()
        {
            SortInput = string.Empty;
            SetResultText?.Invoke(string.Empty);
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
