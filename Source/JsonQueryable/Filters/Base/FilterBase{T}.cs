using System;
using System.Linq;
using JsonQueryable.Attributes;
using JsonQueryable.Contracts;
using JsonQueryable.Enums;

namespace JsonQueryable.Filters.Base
{
    [FilterKind(FilterKinds.Default)]
    public abstract class FilterBase<T> : IFilter<T>
    {
        protected readonly IQueryable<T> queryable;

        protected FilterBase(IQueryable<T> queryable)
        {
            this.queryable = queryable ?? throw new ArgumentNullException(nameof(queryable));
        }

        public abstract IQueryable<T> Apply();
    }
}