using System.Linq;
using System.Linq.Dynamic.Core;
using JsonQueryable.Attributes;

namespace JsonQueryable.Filters
{
    [FilterName("OrderBy")]
    public class OrderByFilter<T> : ParameterizedFilterBase<T, string>
    {
        public OrderByFilter(IQueryable<T> queryable, string parameters) : base(queryable, parameters)
        {
        }

        public override IQueryable<T> Apply()
        {
            return this.queryable.OrderBy(this.parameters);
        }
    }
}