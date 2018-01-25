# JsonQueryable
JsonQueryable provide easy way to filter IQueryable with set of filters and JSON with filters data.

## Project is available as NuGet packages

- [NuGet JsonQueryable](https://www.nuget.org/packages/JsonQueryable)
- [NuGet JsonQueryable.Filters](https://www.nuget.org/packages/JsonQueryable.Filters)

JsonQueryable is core implementation and you can implement custom filters.  
JsonQueryable.Filters provides collection of filters and depends on JsonQueryable.

## Usage Example
```csharp

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
```

## How to implement custom filter

1. Create new class and inherit **ParameterizedFilterBase<T, TParameters>** (json property "**data**" is deserialized to TParameters).
If your filter do not need parameters to do its job, you can inherit **FilterBase< T >**.
2. Annotate you filter with **FilterNameAttribute** and specify you filter name, this is filter identifier.
3. Filter constructor must be **public** and signature must match to base constructor signature.
4. Implement your filtering logic in **Apply** method body.

### Custom filter example

```csharp
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
```
