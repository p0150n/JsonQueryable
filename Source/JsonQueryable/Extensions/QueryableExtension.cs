using System;
using System.Collections.Generic;
using System.Linq;
using JsonQueryable.Attributes;
using JsonQueryable.Contracts;
using JsonQueryable.Exceptions;
using JsonQueryable.Factories;
using JsonQueryable.Models;

namespace JsonQueryable.Extensions
{
    public static class QueryableExtension
    {
        public static IFilteredQueryableBuilder<T> WithFilters<T>(this IQueryable<T> queryable)
        {
            return new FilteredQueryableBuilder<T>(queryable);
        }

        public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> queryable,
            IEnumerable<Type> filterTypes,
            IEnumerable<FilterData> filterDatas)
        {
            if (filterDatas == null)
            {
                throw new ArgumentNullException(nameof(filterDatas));
            }

            var filterDatasList = filterDatas.ToList();

            IFilterFactory<T> filterFactory = new FilterFactory<T>();

            foreach (Type filterType in filterTypes)
            {
                FilterNameAttribute filterNameAttribute = filterType.GetCustomAttributes(false)
                    .Where(a => a is FilterNameAttribute)
                    .Cast<FilterNameAttribute>()
                    .FirstOrDefault();

                if (filterNameAttribute == null)
                {
                    throw new FilterAttributeException<FilterNameAttribute>();
                }

                string filterName = filterNameAttribute.Name;

                foreach (FilterData filterData in filterDatasList)
                {
                    if (filterData.Name.Equals(filterName, StringComparison.InvariantCulture))
                    {
                        IFilter<T> filter = filterFactory.Create(filterType, queryable, filterData.Data);

                        queryable = filter.Apply();
                    }
                }
            }

            return queryable;
        }
    }
}
