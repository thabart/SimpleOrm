namespace ORM.Core
{
    public interface IDbSet<T> where T : class
    {
    }

    public class DbSet<T> : IDbSet<T> where T : class
    {
        private readonly BaseDBContext _dbContext;

        public DbSet(BaseDBContext dbContext)
        {
            _dbContext = dbContext;

            var type = typeof(T);
            string s = "coco";
        }
    }
}
