using LinkDev.Talabat.Core.Domain.Common;
using LinkDev.Talabat.Core.Domain.Contracts;

namespace LinkDev.Talabat.Infrastructure.Persistence.Generic_Repository
{
    internal static class SpecificationsEvaluator<TEntity, TKey>
           where TEntity : BaseEntity<TKey>
           where TKey : IEquatable<TKey>
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity, TKey> spec)
        {
            var query = inputQuery; //_dbContext.Set<Product>();

            if (spec.Criteria is not null) // p => p.ID.equals(1)
                query = query.Where(spec.Criteria);

            // _dbContext.Set<Product>().where( p => p.ID.equals(1))

            if(spec.OrderByDesc is not null)
                query = query.OrderByDescending(spec.OrderByDesc);
            else if(spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);



            if (spec.IsPaginationEnabled)
                query = query.Skip(spec.Skip).Take(spec.Take); // if IsPaginationEnabled is false, pagination will still work bec skip = 0, take = 5 (defaults)

            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            

            return query;
        }
    }

}




















//internal static class SpecificationsEvaluator<TEntity, TKey>
//       where TEntity : BaseEntity<TKey>
//       where TKey : IEquatable<TKey>
//{
//    public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity, TKey> spec)
//    {
//        var query = inputQuery; //_dbContext.Set<Product>();

//        if (spec.Criteria is not null) // p => p.ID.equals(1)
//            query = query.Where(spec.Criteria);

//        // _dbContext.Set<Product>().where( p => p.ID.equals(1))

//        query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

//        return query;
//    }
//}
