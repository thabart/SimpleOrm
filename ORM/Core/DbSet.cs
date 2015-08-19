using ORM.LinqToSql;
using System.Linq;

namespace ORM.Core
{
    public interface IDbSet<TSource> : IQueryable<TSource> where TSource : class
    {
    }

    public class DbSet<TSource> : Queryable<TSource>, IDbSet<TSource> where TSource : class
    {
        public DbSet(QueryProvider queryProvider) : base(queryProvider)
        {
        }
    }
}
