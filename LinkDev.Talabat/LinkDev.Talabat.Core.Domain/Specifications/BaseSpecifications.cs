using LinkDev.Talabat.Core.Domain.Contracts;
using System.Linq.Expressions;

namespace LinkDev.Talabat.Core.Domain.Specifications
{
    public abstract class BaseSpecifications<TEntity, TKey> : ISpecifications<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public Expression<Func<TEntity, bool>>? Criteria { get; set; } = null;
        public List<Expression<Func<TEntity, object>>> Includes { get; set; } = new List<Expression<Func<TEntity, object>>>();
        public Expression<Func<TEntity, object>>? OrderBy { get; set; } = null;
        public Expression<Func<TEntity, object>>? OrderByDesc { get; set; } = null;
        public int Skip { get; set; }
        public int Take { get; set ; }
        public bool IsPaginationEnabled { get; set ; }

        protected BaseSpecifications()
        {
            
        }

        public BaseSpecifications(Expression<Func<TEntity, bool>>? CriteriaExpression)
        {
            Criteria = CriteriaExpression;
        }

        public BaseSpecifications(TKey id)
        {
            Criteria = E => E.Id.Equals(id);

        }

        private protected virtual void AddOrderBy(Expression<Func<TEntity, object>> OrderByExpression)
        {
            OrderBy = OrderByExpression;
        }

        private protected virtual void AddOrderByDesc(Expression<Func<TEntity, object>> OrderByExpressionDesc)
        {
            OrderByDesc = OrderByExpressionDesc;
        }

        private protected void ApplyPagination(int skip, int take)
        {
            IsPaginationEnabled = true;
            Skip = skip;
            Take = take;
        }


        private protected virtual void AddIncludes()
        {

        }

    }
}
