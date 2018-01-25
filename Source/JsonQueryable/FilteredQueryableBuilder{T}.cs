using System;
using System.Collections.Generic;
using System.Linq;
using JsonQueryable.Contracts;
using JsonQueryable.Extensions;

namespace JsonQueryable
{
    public class FilteredQueryableBuilder<T> : IFilteredQueryableBuilder<T>
    {
        private readonly IQueryable<T> queryable;
        private readonly List<Type> filterTypes;

        public FilteredQueryableBuilder(IQueryable<T> queryable)
        {
            this.queryable = queryable;
            this.filterTypes = new List<Type>();
        }

        public IFilteredQueryableBuilder<T> AddFilter<TFilter>() where TFilter : IFilter<T>
        {
            this.filterTypes.Add(typeof(TFilter));

            return this;
        }

        public IQueryable<T> ApplyByFilterOrder(IEnumerable<FilterData> filterDatas)
        {
            return this.queryable.ApplyFiltersByFilterOrder(this.filterTypes, filterDatas);
        }

        public IQueryable<T> ApplyByFilterDataOrder(IEnumerable<FilterData> filterDatas)
        {
            return this.queryable.ApplyFiltersByFilterDataOrder(this.filterTypes, filterDatas);
        }
    }
}
