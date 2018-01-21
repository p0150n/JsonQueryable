using System;
using System.Collections.Concurrent;
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

        public static IQueryable<T> ApplyFiltersByFilterOrder<T>(this IQueryable<T> queryable,
            IEnumerable<Type> filterTypes,
            IEnumerable<FilterData> filterDatas)
        {
            return ApplyFilters(queryable, filterTypes, filterDatas, ApplyFiltersByFilterOrder);
        }

        public static IQueryable<T> ApplyFiltersByFilterDataOrder<T>(this IQueryable<T> queryable,
            IEnumerable<Type> filterTypes,
            IEnumerable<FilterData> filterDatas)
        {
            return ApplyFilters(queryable, filterTypes, filterDatas, ApplyFiltersByFilterDataOrder);
        }

        private static IQueryable<T> ApplyFilters<T>(this IQueryable<T> queryable,
            IEnumerable<Type> filterTypes,
            IEnumerable<FilterData> filterDatas,
            Func<IQueryable<T>, ICollection<Type>, ICollection<FilterData>, IFilterFactory<T>, IQueryable<T>> applyFiltersStrategy)
        {
            if (filterDatas == null)
            {
                throw new ArgumentNullException(nameof(filterDatas));
            }

            var filterTypesList = filterTypes.ToList();
            var filterDatasList = filterDatas.ToList();

            IFilterFactory<T> filterFactory = new FilterFactory<T>();

            queryable = applyFiltersStrategy(queryable, filterTypesList, filterDatasList, filterFactory);

            return queryable;
        }

        private static IQueryable<T> ApplyFiltersByFilterOrder<T>(IQueryable<T> queryable, ICollection<Type> filterTypes, ICollection<FilterData> filterDatasList, IFilterFactory<T> filterFactory)
        {
            foreach (Type filterType in filterTypes)
            {
                string filterName = GetFilterName(filterType);

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

        private static IQueryable<T> ApplyFiltersByFilterDataOrder<T>(IQueryable<T> queryable, ICollection<Type> filterTypes, ICollection<FilterData> filterDatasList, IFilterFactory<T> filterFactory)
        {
            var filterNamesCache = new ConcurrentDictionary<Type, string>();

            foreach (FilterData filterData in filterDatasList)
            {
                foreach (Type filterType in filterTypes)
                {
                    string filterName = filterNamesCache.GetOrAdd(filterType, GetFilterName);
                    if (filterData.Name.Equals(filterName, StringComparison.InvariantCulture))
                    {
                        IFilter<T> filter = filterFactory.Create(filterType, queryable, filterData.Data);

                        queryable = filter.Apply();
                    }
                }
            }

            return queryable;
        }

        private static string GetFilterName(Type filterType)
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

            return filterName;
        }
    }
}
