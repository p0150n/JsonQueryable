using System.Linq;
using System.Linq.Dynamic.Core;
using JsonQueryable.Attributes;

namespace JsonQueryable.Filters
{
    [FilterName("Equals")]
    public class EqualsFilter<T> : ParameterizedFilterBase<T, Models.PropertyValueModel<object>>

    {
        public EqualsFilter(IQueryable<T> queryable, Models.PropertyValueModel<object> parameters) : base(queryable, parameters)
        {
        }

        public override IQueryable<T> Apply()
        {
            return this.queryable.Where($"{this.parameters.PropertyName} == @0", this.parameters.Value);
        }
    }
}
