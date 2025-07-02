using MathCore.Interfaces;

namespace Web.Interfaces
{
    public interface ISortFactory
    {
        ISortAlgorithm<T> Resolve<T>(string algorithm) where T : IComparable<T>;
    }
}
