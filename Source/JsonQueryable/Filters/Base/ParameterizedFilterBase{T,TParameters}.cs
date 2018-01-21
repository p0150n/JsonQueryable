using System;
using System.Linq;
using JsonQueryable.Attributes;
using JsonQueryable.Enums;

namespace JsonQueryable.Filters.Base
{
    [FilterKind(FilterKinds.Parameterized)]
    public abstract class ParameterizedFilterBase<T, TParameters> : FilterBase<T>
    {
        protected readonly TParameters parameters;

        protected ParameterizedFilterBase(IQueryable<T> queryable, TParameters parameters)
            : base(queryable)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            this.parameters = parameters;
        }
    }
}
