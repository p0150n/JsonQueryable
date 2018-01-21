using System;
using System.Linq;
using JsonQueryable.Attributes;
using JsonQueryable.Filters.Base;
using JsonQueryable.Models.Filter;

namespace JsonQueryable.Filters
{
    [FilterName("Pagination")]
    public class PaginationFilter<T> : ParameterizedFilterBase<T, PaginationModel>
    {
        public PaginationFilter(IQueryable<T> queryable, PaginationModel parameters) : base(queryable, parameters)
        {
        }

        public override IQueryable<T> Apply()
        {
            int zeroBasePageNumber = this.parameters.PageNumber - 1;

            if (zeroBasePageNumber < 0)
            {
                throw new ArgumentException($"{nameof(this.parameters.PageNumber)} must be greater or equal to 1, but {nameof(this.parameters.PageNumber)} is: {this.parameters.PageNumber}.");
            }

            return this.queryable.Skip(this.parameters.PageSize * zeroBasePageNumber)
                                 .Take(this.parameters.PageSize);
        }
    }
}
