using System.Collections.Generic;

namespace ORM.Core
{
    public interface IDbSet<T> where T : class
    {
        List<T> All();
    }

    public class DbSet<T> : IDbSet<T> where T : class
    {
        private readonly BaseDBContext _dbContext;

        private readonly QueryBuilder _queryBuilder;

        public DbSet(BaseDBContext dbContext)
        {
            _dbContext = dbContext;
            _queryBuilder = new QueryBuilder();
        }

        public List<T> All()
        {
            // TODO : based on the type & mapping rules => create the query
            return null;
        }
    }
}
