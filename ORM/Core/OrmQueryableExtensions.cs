using System;
using System.Linq;
using System.Linq.Expressions;

namespace ORM.Core
{
    public static class OrmQueryableExtensions
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate)
        {
            return null;
        }
    }
}
