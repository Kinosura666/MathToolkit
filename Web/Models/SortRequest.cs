namespace Web.Models
{
    public class SortRequest
    {
        public List<double> array { get; set; }
        public string algorithm { get; set; }
        public string direction { get; set; } = "Ascending";
        public bool logSteps { get; set; } = false;
    }
}
