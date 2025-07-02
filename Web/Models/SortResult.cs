namespace Web.Models
{
    public class SortResult
    {
        public List<double> sortedArray { get; set; }
        public List<string> steps { get; set; }
        public int comparisons {  get; set; }
        public int swaps { get; set; }
        public TimeSpan duration { get; set; }
    }
}
