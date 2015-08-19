using ORM.LinqToSql;
using System.Linq;

namespace ORM.Core
{
    public interface IDbSet<TSource> where TSource : class
    {
    }

    public class DbSet<TSource> : Queryable<TSource>, IDbSet<TSource> where TSource : class
    {
        public DbSet(IQueryProvider queryProvider) : base(queryProvider)
        {
        }
    }
}
