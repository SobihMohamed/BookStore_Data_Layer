using Domain.Contracts;
using Domain.Contracts.SpecificationPattern;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Persistence.Evaluator
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> GenerateQuery<TEntity, TKey>
            (IQueryable<TEntity> BaseQuery, ISpecifications<TEntity, TKey> specification)
            where TEntity : class, IEntity<TKey>
        {
            var query = BaseQuery;

            // to make sure that the query is not tracked by the context, which can improve performance for read-only operations.
            if (specification.IsNoTracking)
                query = query.AsNoTracking();

            // Criterias List
            if (specification.Criterias is not null && specification.Criterias.Any())
            {
                query = specification.Criterias.Aggregate(query, (current, criteria) => current.Where(criteria));
            }

            // Includes 
            if (specification.Includes is not null && specification.Includes.Any())
                query = specification.Includes.Aggregate(query, (current, includeExpression) => current.Include(includeExpression));

            // Nested Includes String based
            if (specification.IncludeStrings is not null && specification.IncludeStrings.Any())
                query = specification.IncludeStrings.Aggregate(query, (current, includeString) => current.Include(includeString));

            // OrderBy & ThenBy
            if (specification.OrderExpressionInfo is not null && specification.OrderExpressionInfo.Any())
            {
                var firstOrder = specification.OrderExpressionInfo.First();

                IOrderedQueryable<TEntity> orderedQuery = firstOrder.IsDescending
                    ? query.OrderByDescending(firstOrder.OrderExpression!)
                    : query.OrderBy(firstOrder.OrderExpression!);

                for (int i = 1; i < specification.OrderExpressionInfo.Count; i++)
                {
                    var nextSort = specification.OrderExpressionInfo[i];

                    orderedQuery = nextSort.IsDescending
                        ? orderedQuery.ThenByDescending(nextSort.OrderExpression!)
                        : orderedQuery.ThenBy(nextSort.OrderExpression!);
                }

                query = orderedQuery;
            }

            // Pagination
            if (specification.IsPagenationEnabled)
                query = query.Skip(specification.Skip).Take(specification.Take);

            return query;
        }
    }
}