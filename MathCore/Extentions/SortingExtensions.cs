using MathCore.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathCore.Extentions
{
    public static class SortingExtensions
    {
        public static int Compare<T>(T a, T b, SortDirection direction) where T : IComparable<T>
            => direction == SortDirection.Ascending ? a.CompareTo(b) : b.CompareTo(a);

        public static void Swap<T>(T[] array, int i, int j)
        {
            T temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
}
