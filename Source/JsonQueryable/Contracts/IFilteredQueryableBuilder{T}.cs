using System.Collections.Generic;
using System.Linq;
using JsonQueryable.Models;

namespace JsonQueryable.Contracts
{
    public interface IFilteredQueryableBuilder<T>
    {
        IFilteredQueryableBuilder<T> AddFilter<TFilter>()
            where TFilter : IFilter<T>;

        IQueryable<T> Apply(IEnumerable<FilterData> filterDatas);
    }
}