using MathCore.Interfaces;
using MathCore.Models.SortResults;
using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Desktop.Services
{
    public static class SortService
    {
        public static bool IsAllInt(string input) =>
            input.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                 .All(s => int.TryParse(s, out _));

        public static bool IsAllDouble(string input) =>
            input.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                 .All(s => double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out _));

        public static T[] ParseArray<T>(string input) where T : IComparable<T>
        {
            string[] parts = input
                .Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            return parts.Select(s =>
                (T)Convert.ChangeType(
                    typeof(T) == typeof(double)
                        ? double.Parse(s, CultureInfo.InvariantCulture)
                        : int.Parse(s),
                    typeof(T)
                )).ToArray();
        }

        public static string FormatSortResult<T>(SortResult<T> result)
        {
            var output = new StringBuilder();
            var culture = CultureInfo.InvariantCulture;

            string formattedArray = string.Join(", ",
                result.SortedArray.Select(x =>
                    x is IFormattable f
                        ? f.ToString("G", culture)
                        : x?.ToString() ?? "null"
                ));

            output.AppendLine($"Sorted Array: [{formattedArray}]");
            output.AppendLine($"Comparisons: {result.ComparisonCount}");
            output.AppendLine($"Swaps: {result.SwapCount}");
            output.AppendLine($"Time: {result.Duration.TotalMilliseconds.ToString("F2", culture)} ms");

            if (result.Steps?.Any() == true)
            {
                output.AppendLine("\nSteps:");

                foreach (var step in result.Steps)
                {
                    string formattedStep = step;
                    if (typeof(T) == typeof(double))
                    {
                        formattedStep = FormatDoublesInText(step);
                    }
                    output.AppendLine("‣ " + formattedStep);
                }
            }

            return output.ToString();
        }

        private static string FormatDoublesInText(string input)
        {
            var cultureInput = new CultureInfo("uk-UA");
            var cultureOutput = CultureInfo.InvariantCulture;

            return System.Text.RegularExpressions.Regex.Replace(
                input,
                @"-?\d{1,3}(?:\.\d{3})*(?:,\d+)?|-?\d+,\d+",
                m =>
                {
                    try
                    {
                        double parsed = double.Parse(m.Value, cultureInput);
                        return parsed.ToString("G", cultureOutput);
                    }
                    catch
                    {
                        return m.Value;
                    }
                });
        }

        public static SortResult<T> RunSort<T>(string input, Func<ISortAlgorithm<T>> sorterFactory)
            where T : IComparable<T>
        {
            var array = ParseArray<T>(input);
            var sorter = sorterFactory();
            return sorter.Sort(array, logSteps: true);
        }
    }
}
