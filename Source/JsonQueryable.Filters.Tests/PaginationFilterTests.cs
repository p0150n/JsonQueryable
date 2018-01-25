using System.Collections.Generic;
using System.Linq;
using JsonQueryable.Contracts;
using JsonQueryable.Factories;
using JsonQueryable.Filters.Tests.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace JsonQueryable.Filters.Tests
{
    public class PaginationFilterTests
    {
        [Fact]
        public void PaginationFilter_ShouldReturn_CorrectPageData()
        {
            var queryable = new[]
            {
                new ObjectWithProperty<int> {Property = 3},
                new ObjectWithProperty<int> {Property = 1},
                new ObjectWithProperty<int> {Property = 5},
                new ObjectWithProperty<int> {Property = 2},
                new ObjectWithProperty<int> {Property = 4},
                new ObjectWithProperty<int> {Property = 6},
                new ObjectWithProperty<int> {Property = 0},
            }.AsQueryable();

            IFilterFactory<ObjectWithProperty<int>> filterFactory = new FilterFactory<ObjectWithProperty<int>>();
            FilterData orderByFilterModel = JsonConvert.DeserializeObject<FilterData>(@"
            {
            ""name"": ""OrderBy"",
            ""data"": ""Property""
            }");
            JToken orderByFilterModelData = orderByFilterModel.Data;
            IFilter<ObjectWithProperty<int>> orderByFilter = filterFactory.Create<OrderByFilter<ObjectWithProperty<int>>>(queryable, orderByFilterModelData);

            var orderedQueryable = orderByFilter.Apply();

            FilterData paginationFilterModel = JsonConvert.DeserializeObject<FilterData>(@"
            {
            ""name"": ""Pagination"",
            ""data"": {
                        ""pageSize"": 2,
                        ""pageNumber"": 2
                      }
            }");
            JToken paginationFilterModelData = paginationFilterModel.Data;
            IFilter<ObjectWithProperty<int>> paginationFilter = filterFactory.Create<PaginationFilter<ObjectWithProperty<int>>>(orderedQueryable, paginationFilterModelData);

            var pagedQueryable = paginationFilter.Apply();

            IEnumerable<ObjectWithProperty<int>> materializedResult = pagedQueryable.ToList();

            Assert.Equal(2, materializedResult.Count());
            Assert.Equal(2, materializedResult.First().Property);
            Assert.Equal(3, materializedResult.Last().Property);
        }
    }
}
