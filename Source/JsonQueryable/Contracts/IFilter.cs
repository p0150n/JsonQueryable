using System.Linq;

namespace JsonQueryable.Contracts
{
    public interface IFilter<out T>
    {
        IQueryable<T> Apply();
    }
}