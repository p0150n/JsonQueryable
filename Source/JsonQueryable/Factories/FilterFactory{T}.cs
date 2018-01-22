using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using JsonQueryable.Attributes;
using JsonQueryable.Contracts;
using JsonQueryable.Enums;
using JsonQueryable.Exceptions;
using JsonQueryable.Extensions;
using JsonQueryable.Filters.Base;
using Newtonsoft.Json.Linq;

namespace JsonQueryable.Factories
{
    public class FilterFactory<T> : IFilterFactory<T>
    {
        private readonly IReadOnlyDictionary<FilterKinds, Func<Type, IQueryable<T>, JToken, IFilter<T>>> factoryStrategies;

        public FilterFactory()
        {
            var factoryStrategies = new ConcurrentDictionary<FilterKinds, Func<Type, IQueryable<T>, JToken, IFilter<T>>>();

            factoryStrategies.TryAdd(FilterKinds.Default, this.CreateDefaultFilter);
            factoryStrategies.TryAdd(FilterKinds.Parameterized, this.CreateParameterizedFilter);

            this.factoryStrategies = factoryStrategies;
        }

        public IFilter<T> Create<TFilter>(IQueryable<T> queryable, JToken filterData)
            where TFilter : IFilter<T>
        {
            return this.Create(typeof(TFilter), queryable, filterData);
        }

        public IFilter<T> Create(Type filterType, IQueryable<T> queryable, JToken filterData)
        {
            if (filterType == null)
            {
                throw new ArgumentNullException(nameof(filterType));
            }

            if (queryable == null)
            {
                throw new ArgumentNullException(nameof(queryable));
            }

            Type iFilterClosedGenericType = typeof(IFilter<T>);
            if (!iFilterClosedGenericType.IsAssignableFrom(filterType))
            {
                throw new ArgumentException($"{filterType.FullName} must be assignable to {iFilterClosedGenericType.FullName}");
            }

            FilterKindAttribute filterKindAttribute = filterType.GetCustomAttributes(true)
                .Where(a => a is FilterKindAttribute)
                .Cast<FilterKindAttribute>()
                .FirstOrDefault();

            if (filterKindAttribute == null)
            {
                throw new FilterAttributeException<FilterKindAttribute>();
            }

            FilterKinds filterKind = filterKindAttribute.FilterKind;

            return this.factoryStrategies[filterKind](filterType, queryable, filterData);
        }

        private IFilter<T> CreateDefaultFilter(Type filterType, IQueryable<T> queryable, JToken filterData)
        {
            return (IFilter<T>)Activator.CreateInstance(filterType, queryable);
        }

        private IFilter<T> CreateParameterizedFilter(Type filterType, IQueryable<T> queryable, JToken filterData)
        {
            if (filterData == null)
            {
                throw new ArgumentNullException(nameof(filterData));
            }

            // TParameters is second generic parameter
            Type parametersType = filterType.FindBaseType(t => t.GetGenericTypeDefinition() == typeof(ParameterizedFilterBase<,>)).GetGenericArguments()[1];

            object parameters = filterData.ToObject(parametersType);

            return (IFilter<T>)Activator.CreateInstance(filterType, queryable, parameters);
        }
    }
}