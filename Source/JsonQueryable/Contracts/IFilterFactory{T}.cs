using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace JsonQueryable.Contracts
{
    public interface IFilterFactory<T>
    {
        IFilter<T> Create<TFilter>(IQueryable<T> queryable, JToken filterData)
            where TFilter : IFilter<T>;

        IFilter<T> Create(Type filterType, IQueryable<T> queryable, JToken filterData);
    }
}