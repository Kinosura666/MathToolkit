using Web.Models;

namespace Web.Interfaces
{
    public interface ISortService
    {
        SortResult Sort(SortRequest request);
    }
}
