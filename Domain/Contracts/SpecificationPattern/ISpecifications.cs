using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Domain.Contracts.SpecificationPattern
{
    public interface ISpecifications<TEntity, TKey> where TEntity : IEntity<TKey>
    {
        List<Expression<Func<TEntity, bool>>> Criterias { get; }

        List<Expression<Func<TEntity, object>>> Includes { get; }
        List<string> IncludeStrings { get; }

        List<OrderExpressionInfo<TEntity>> OrderExpressionInfo { get; }

        Expression<Func<TEntity, object>> GroupBy { get; }

        int Take { get; }
        int Skip { get; }
        bool IsPagenationEnabled { get; }

        bool IsNoTracking { get; }
    }
}