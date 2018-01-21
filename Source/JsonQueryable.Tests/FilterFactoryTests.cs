using System.Linq;
using JsonQueryable.Contracts;
using JsonQueryable.Factories;
using JsonQueryable.Filters;
using JsonQueryable.Models;
using JsonQueryable.Tests.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace JsonQueryable.Tests
{
    public class FilterFactoryTests
    {
        [Fact]
        public void FilterFactory_ShouldCreate_CorrectOrderByFilterType()
        {
            var queryable = new[]
            {
                new ObjectWithProperty<int> {Property = 3}
            }.AsQueryable();
            IFilterFactory<ObjectWithProperty<int>> filterFactory = new FilterFactory<ObjectWithProperty<int>>();
            FilterData filterModel = JsonConvert.DeserializeObject<FilterData>(@"
            {
            ""name"": ""SomeFilterName"",
            ""data"": ""Property""
            }");
            JToken filterData = filterModel.Data;

            IFilter<ObjectWithProperty<int>> filter = filterFactory.Create<OrderByFilter<ObjectWithProperty<int>>>(queryable, filterData);

            Assert.True(filter.GetType() == typeof(OrderByFilter<ObjectWithProperty<int>>));
        }

        [Fact]
        public void FilterFactory_ShouldCreate_CorrectPaginationFilterType()
        {
            var queryable = new[]
            {
                new ObjectWithProperty<int> {Property = 3}
            }.AsQueryable();
            IFilterFactory<ObjectWithProperty<int>> filterFactory = new FilterFactory<ObjectWithProperty<int>>();
            FilterData filterModel = JsonConvert.DeserializeObject<FilterData>(@"
            {
            ""name"": ""SomeFilterName"",
            ""data"": {
                        ""PageSize"": 2,
                        ""PageNumber"": 2
                      }
            }");
            JToken filterData = filterModel.Data;

            IFilter<ObjectWithProperty<int>> paginationFilter = filterFactory.Create<PaginationFilter<ObjectWithProperty<int>>>(queryable, filterData);

            Assert.True(paginationFilter.GetType() == typeof(PaginationFilter<ObjectWithProperty<int>>));
        }
    }
}