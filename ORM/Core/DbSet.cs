namespace ORM.Core
{
    public interface IDbSet<T> where T : class
    {
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

        
    }
}
