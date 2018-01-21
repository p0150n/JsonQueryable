using System.Collections.Generic;
using System.Linq;
using JsonQueryable.Extensions;
using JsonQueryable.Filters;
using JsonQueryable.Models;
using JsonQueryable.Tests.Models;
using Newtonsoft.Json;
using Xunit;

namespace JsonQueryable.Tests
{
    public class MultyFiltersTests
    {
        [Fact]
        public void AllFilters_ShouldReturn_CorrectData()
        {
            IQueryable<Person> persons = new List<Person>()
            {
                new Person()
                {
                    FirstName = "Sylvester",
                    LastName = "Stallone",
                    Age = 54
                },
                new Person()
                {
                    FirstName = "Johnny",
                    LastName = "Depp",
                    Age = 54
                },
                new Person()
                {
                    FirstName = "Jason",
                    LastName = "Statham",
                    Age = 50
                },
                new Person()
                {
                    FirstName = "Bruce",
                    LastName = "Willis",
                    Age = 54
                },
                new Person()
                {
                    FirstName = "Chuck",
                    LastName = "Norris",
                    Age = 54
                }
            }.AsQueryable();

            IEnumerable<FilterData> filterData = JsonConvert.DeserializeObject<IEnumerable<FilterData>>(@"
            [
                {
                ""name"": ""Equals"",
                ""data"": {
                            ""PropertyName"": ""Age"",
                            ""Value"": 54
                          }
                },
                {
                ""name"": ""OrderBy"",
                ""data"": ""FirstName""
                },
                {
                ""name"": ""Pagination"",
                ""data"": {
                            ""pageSize"": 2,
                            ""pageNumber"": 2
                          }
                }
            ]");

            List<Person> result = persons.WithFilters()
                .AddFilter<EqualsFilter<Person>>()
                .AddFilter<OrderByFilter<Person>>()
                .AddFilter<PaginationFilter<Person>>()
                .ApplyByFilterOrder(filterData)
                .ToList();

            Assert.Equal(2, result.Count());
            Assert.Equal("Johnny", result.First().FirstName);
            Assert.Equal("Sylvester", result.Last().FirstName);
        }
    }
}
