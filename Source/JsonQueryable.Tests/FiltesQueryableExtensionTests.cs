using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JsonQueryable.Contracts;
using JsonQueryable.Extensions;
using JsonQueryable.Factories;
using JsonQueryable.Filters;
using JsonQueryable.Models;
using JsonQueryable.Tests.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace JsonQueryable.Tests
{
    public class FiltesQueryableExtensionTests
    {
        [Fact]
        public void FiltersExecutedWithApplyFiltesQueryableExtension_ShouldReturn_CorrectPageData()
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

            Type[] filterTypes = new[]
            {
                typeof(OrderByFilter<ObjectWithProperty<int>>),
                typeof(PaginationFilter<ObjectWithProperty<int>>)
            };

            FilterData[] filterDatas = JsonConvert.DeserializeObject<FilterData[]>(@"
            [
                {
                ""name"": ""OrderBy"",
                ""data"": ""Property""
                },
                {
                ""name"": ""Pagination"",
                ""data"": {
                            ""pageSize"": 2,
                            ""pageNumber"": 2
                          }
                }
            ]");

            IEnumerable<ObjectWithProperty<int>> materializedResult = queryable.ApplyFilters(filterTypes, filterDatas).ToList();

            Assert.Equal(2, materializedResult.Count());
            Assert.Equal(2, materializedResult.First().Property);
            Assert.Equal(3, materializedResult.Last().Property);
        }


        [Fact]
        public void FiltersExecutedWithFiltesQueryableExtension_ShouldReturn_CorrectPageData()
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

            FilterData[] filterDatas = JsonConvert.DeserializeObject<FilterData[]>(@"
            [
                {
                ""name"": ""OrderBy"",
                ""data"": ""Property""
                },
                {
                ""name"": ""Pagination"",
                ""data"": {
                            ""pageSize"": 2,
                            ""pageNumber"": 3
                          }
                }
            ]");

            IEnumerable<ObjectWithProperty<int>> materializedResult = queryable
                .WithFilters()
                .AddFilter<OrderByFilter<ObjectWithProperty<int>>>()
                .AddFilter<PaginationFilter<ObjectWithProperty<int>>>()
                .Apply(filterDatas)
                .ToList();

            Assert.Equal(2, materializedResult.Count());
            Assert.Equal(4, materializedResult.First().Property);
            Assert.Equal(5, materializedResult.Last().Property);
        }
    }
}
