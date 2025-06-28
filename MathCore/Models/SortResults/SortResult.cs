using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCore.Models.SortResults
{
    public class SortResult<T>
    {
        public T[] SortedArray { get; set; }
        public List<string> Steps { get; set; }
        public int ComparisonCount { get; set; }
        public int SwapCount { get; set; }
        public TimeSpan Duration { get; set; }
        public List<T> Pivots { get; set; }
        public List<int> PivotIndices { get; set; }
    }
}
