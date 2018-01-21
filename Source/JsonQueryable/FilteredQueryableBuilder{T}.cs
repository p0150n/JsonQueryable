using System;
using System.Collections.Generic;
using System.Linq;
using JsonQueryable.Contracts;
using JsonQueryable.Extensions;
using JsonQueryable.Models;

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

        public IQueryable<T> Apply(IEnumerable<FilterData> filterDatas)
        {
            return this.queryable.ApplyFilters(this.filterTypes, filterDatas);
        }
    }
}
