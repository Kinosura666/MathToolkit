using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathCore.Common;
using MathCore.Models.SortResults;

namespace MathCore.Interfaces
{
    public interface ISortAlgorithm<T> where T : IComparable<T>
    {
        SortResult<T> Sort(T[] input, bool logSteps = false, SortDirection direction = SortDirection.Ascending);
    }
}
