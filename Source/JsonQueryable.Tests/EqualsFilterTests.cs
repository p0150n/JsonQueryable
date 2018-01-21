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
    public class EqualsFilterTests
    {
        [Fact]
        public void EqualsFilter_ShouldReturn_CorrectData()
        {
            var persons = new List<Person>()
            {
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
                }
            }.AsQueryable();

            var filterData = JsonConvert.DeserializeObject<IEnumerable<FilterData>>(@"
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
                }
            ]");

            var result = persons.WithFilters()
                .AddFilter<EqualsFilter<Person>>()
                .AddFilter<OrderByFilter<Person>>()
                .ApplyByFilterOrder(filterData)
                .ToList();

            Assert.Equal(2, result.Count());
            Assert.Equal("Bruce", result.First().FirstName);
            Assert.Equal("Johnny", result.Last().FirstName);
        }
    }
}
