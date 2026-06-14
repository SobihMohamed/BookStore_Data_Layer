using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Domain.Contracts.SpecificationPattern
{
    public abstract class BaseSpecifications<TEntity, TKey>
        : ISpecifications<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        public List<Expression<Func<TEntity, bool>>> Criterias { get; private set; } = new();
        public List<Expression<Func<TEntity, object>>> Includes { get; private set; } = new();
        public List<string> IncludeStrings { get; private set; } = new();
        public List<OrderExpressionInfo<TEntity>> OrderExpressionInfo { get; private set; } = new();
        public Expression<Func<TEntity, object>> GroupBy { get; private set; }
        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPagenationEnabled { get; private set; } = false;
        public bool IsNoTracking { get; private set; } = false;

        protected BaseSpecifications() { }

        // IF want to use the constructor with criteria, you can use it like this:
        protected BaseSpecifications(Expression<Func<TEntity, bool>> criteria)
        {
            Criterias.Add(criteria);
        }

        #region Methods To Build the Specification

        protected void AddCriteria(Expression<Func<TEntity, bool>> criteriaExpression)
            => Criterias.Add(criteriaExpression);

        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
            => Includes.Add(includeExpression);

        protected void AddInclude(string includeString)
            => IncludeStrings.Add(includeString);

        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression, bool isDescending = false)
            => OrderExpressionInfo.Add(new OrderExpressionInfo<TEntity>
            {
                OrderExpression = orderByExpression,
                IsDescending = isDescending
            });

        protected void ApplyGroupBy(Expression<Func<TEntity, object>> groupByExpression)
            => GroupBy = groupByExpression;

        protected void ApplyPagenation(int pageSize, int pageIndex)
        {
            Skip = (pageIndex - 1) * pageSize;
            Take = pageSize;
            IsPagenationEnabled = true;
        }

        protected void ApplyNoTracking()
            => IsNoTracking = true;

        #endregion
    }
}