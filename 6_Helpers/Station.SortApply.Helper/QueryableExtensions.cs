﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
namespace Station.SortApply.Helper
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(
            this IQueryable<T> source,
            string orderBy,
            Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (mappingDictionary == null)
                throw new ArgumentNullException(nameof(mappingDictionary));
            if (string.IsNullOrWhiteSpace(orderBy))
                return source;

            var orderByAfterSplit = orderBy.Split(',');
            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                var trimmedOrderByClause = orderByClause.Trim();
                var orderByDescending = trimmedOrderByClause.EndsWith(" desc");
                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ", StringComparison.Ordinal);

                var propertyName = indexOfFirstSpace == -1
                    ? trimmedOrderByClause
                    : trimmedOrderByClause.Remove(indexOfFirstSpace);

                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentException($"没有找到key为{propertyName}的映射");
                }

                var propertyMappingValue = mappingDictionary[propertyName];
                if (propertyMappingValue == null)
                {
                    throw new ArgumentNullException(nameof(propertyMappingValue));
                }

                foreach (var destinationProperty in propertyMappingValue.DestinationProperties)
                {
                    if (propertyMappingValue.Revert)
                    {
                        orderByDescending = !orderByDescending;
                    }

                    source = source.OrderBy(destinationProperty + (orderByDescending ? " descending" : " ascending"));
                }
            }

            return source;
        }
    }
}