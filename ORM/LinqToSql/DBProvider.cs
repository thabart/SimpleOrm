using System;
using System.Linq;


namespace ORM.LinqToSql
{
    public class DBProvider
    {
        public DBProvider(string connectionString)
        {

        }

        public DBProvider(string userName, string password)
        {

        }

        public IQueryable<TSource> GetTable<TSource>()
        {
            var type = typeof(Queryable<TSource>);
            var queryProvider = new QueryProvider(null);
            var instance = (IQueryable<TSource>)Activator.CreateInstance(type, queryProvider);
            return instance;
        }
    }
}
