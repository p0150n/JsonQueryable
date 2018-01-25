using System;
using System.Collections.Generic;
using System.Linq;
using JsonQueryable.Contracts;
using JsonQueryable.Filters.Tests.Models;
using Newtonsoft.Json;
using Xunit;

namespace JsonQueryable.Filters.Tests
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

            IEnumerable<ObjectWithProperty<int>> materializedResult = queryable.ApplyFiltersByFilterOrder(filterTypes, filterDatas).ToList();

            Assert.Equal(2, materializedResult.Count());
            Assert.Equal(2, materializedResult.First().Property);
            Assert.Equal(3, materializedResult.Last().Property);
        }

        [Fact]
        public void FiltersExecutedWithFiltersQueryableExtension_ShouldReturn_CorrectPageData()
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

            IFilteredQueryableBuilder<ObjectWithProperty<int>> filterBuilderQuery = queryable
                .WithFilters()
                .AddFilter<OrderByFilter<ObjectWithProperty<int>>>()
                .AddFilter<PaginationFilter<ObjectWithProperty<int>>>();

            IEnumerable<ObjectWithProperty<int>> filteredByFilterDataOrder = filterBuilderQuery
                .ApplyByFilterDataOrder(filterDatas)
                .ToList();

            Assert.Equal(2, filteredByFilterDataOrder.Count());
            Assert.Equal(4, filteredByFilterDataOrder.First().Property);
            Assert.Equal(5, filteredByFilterDataOrder.Last().Property);

            IEnumerable<ObjectWithProperty<int>> filteredByFilterOrder = filterBuilderQuery
                .ApplyByFilterOrder(filterDatas)
                .ToList();

            Assert.Equal(2, filteredByFilterOrder.Count());
            Assert.Equal(4, filteredByFilterOrder.First().Property);
            Assert.Equal(5, filteredByFilterOrder.Last().Property);
        }

        [Fact]
        public void FiltersExecutedWithFiltersDataOrderQueryableExtension_ShouldReturn_CorrectPageData()
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
                ""name"": ""Pagination"",
                ""data"": {
                            ""pageSize"": 2,
                            ""pageNumber"": 2
                          }
                },
                {
                ""name"": ""OrderBy"",
                ""data"": ""Property""
                }
            ]");

            IFilteredQueryableBuilder<ObjectWithProperty<int>> filterBuilderQuery = queryable
                .WithFilters()
                .AddFilter<OrderByFilter<ObjectWithProperty<int>>>()
                .AddFilter<PaginationFilter<ObjectWithProperty<int>>>();

            IEnumerable<ObjectWithProperty<int>> filteredByFilterOrder = filterBuilderQuery
                .ApplyByFilterDataOrder(filterDatas)
                .ToList();

            Assert.Equal(2, filteredByFilterOrder.Count());
            Assert.Equal(2, filteredByFilterOrder.First().Property);
            Assert.Equal(5, filteredByFilterOrder.Last().Property);
        }
        
        [Fact]
        public void FiltersExecutedWithFiltersOrderQueryableExtension_ShouldReturn_CorrectPageData()
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
                ""name"": ""Pagination"",
                ""data"": {
                            ""pageSize"": 2,
                            ""pageNumber"": 2
                          }
                },
                {
                ""name"": ""OrderBy"",
                ""data"": ""Property""
                }
            ]");

            IFilteredQueryableBuilder<ObjectWithProperty<int>> filterBuilderQuery = queryable
                .WithFilters()
                .AddFilter<OrderByFilter<ObjectWithProperty<int>>>()
                .AddFilter<PaginationFilter<ObjectWithProperty<int>>>();

            IEnumerable<ObjectWithProperty<int>> filteredByFilterOrder = filterBuilderQuery
                .ApplyByFilterOrder(filterDatas)
                .ToList();

            Assert.Equal(2, filteredByFilterOrder.Count());
            Assert.Equal(2, filteredByFilterOrder.First().Property);
            Assert.Equal(3, filteredByFilterOrder.Last().Property);
        }
    }
}
