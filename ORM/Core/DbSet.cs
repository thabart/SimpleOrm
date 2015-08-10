namespace ORM.Core
{
    public interface IDbSet<T> where T : class
    {
    }

    public class DbSet<T> : IDbSet<T> where T : class
    {
        public DbSet()
        {

        }
    }
}
