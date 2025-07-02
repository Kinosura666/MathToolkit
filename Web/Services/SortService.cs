using MathCore.Common;
using Web.Interfaces;
using Web.Models;

namespace Web.Services
{
    public class SortService : ISortService
    {
        private readonly ISortFactory _factory;

        public SortService(ISortFactory factory)
        {
            _factory = factory;
        }

        public SortResult Sort(SortRequest request)
        {
            if (request.array == null || request.array.Count == 0)
                throw new ArgumentException("Array is empty");

            var direction = request.direction.Equals("Descending", StringComparison.OrdinalIgnoreCase)
                ? SortDirection.Descending
                : SortDirection.Ascending;

            if (request.array.All(x => x % 1 == 0))
            {
                var ints = request.array.Select(x => (int)x).ToArray();
                var sorter = _factory.Resolve<int>(request.algorithm);
                var result = sorter.Sort(ints, request.logSteps, direction);

                return new SortResult
                {
                    sortedArray = result.SortedArray.Select(i => (double)i).ToList(),
                    steps = result.Steps,
                    comparisons = result.ComparisonCount,
                    swaps = result.SwapCount,
                    duration = result.Duration
                };
            }
            else
            {
                var doubles = request.array.ToArray();
                var sorter = _factory.Resolve<double>(request.algorithm);
                var result = sorter.Sort(doubles, request.logSteps, direction);

                return new SortResult
                {
                    sortedArray = result.SortedArray.ToList(),
                    steps = result.Steps,
                    comparisons = result.ComparisonCount,
                    swaps = result.SwapCount,
                    duration = result.Duration
                };
            }
        }
    }
}
