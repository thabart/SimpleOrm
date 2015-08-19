using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ORM.LinqToSql
{
    public static class QueryableExtensions
    {
        public static IQueryable<TSource> Select<TSource, TTarget>(this IQueryable<TSource> source, Expression<Func<TSource, TTarget>> expression)
        {
            var expressionTree = Expression.Call(
            null,
            ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[]
            {
                typeof(TSource),
                typeof(TTarget)
            }),
            new Expression[]
            {
                source.Expression,
                Expression.Quote(expression)
            });
            return source.Provider.CreateQuery<TSource>(expressionTree);
        }

        public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> expression)
        {
            var expressionTree = Expression.Call(
                null,
                ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[]
                {
                    typeof(TSource)
                }),
                new Expression[]
                {
                    source.Expression,
                    Expression.Quote(expression)
                });
            return source.Provider.CreateQuery<TSource>(expressionTree);
        }
    }
}
