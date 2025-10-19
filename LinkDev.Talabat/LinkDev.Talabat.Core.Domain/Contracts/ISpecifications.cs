using System.Linq.Expressions;
using System.Reflection.PortableExecutable;

namespace LinkDev.Talabat.Core.Domain.Contracts
{
    public interface ISpecifications<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public Expression<Func<TEntity, bool>>? Criteria { get; set; } // for (where) operator ==> Takes TEntity and returns boolean
        public List<Expression<Func<TEntity, object>>> Includes { get; set; }
        // for (include) operator
        // i made the func take object and not BaseEntity<TKey>, bec i am not sure if only one entity will be returned or icollection


        public Expression<Func<TEntity, object>>? OrderBy { get; set; }
        public Expression<Func<TEntity, object>>? OrderByDesc { get; set; }

        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }

    }

}
