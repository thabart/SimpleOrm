using ORM.Core;
using ORM.Translators;

using System;
using System.Linq;
using System.Linq.Expressions;

namespace ORM.LinqToSql
{
    public class QueryProvider : IQueryProvider
    {
        private readonly IConnectionManager _connectionManager;

        public QueryProvider(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new Queryable<TElement>(this, expression);
        }

        /// <summary>
        /// Transform the expression tree into SQL script, execute it and returns the result.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public object Execute(Expression expression)
        {
            var queryTranslator = new QueryTranslator();
            var query = queryTranslator.Translate(expression);

            return null;
        }

        /// <summary>
        /// Transform the expression tree into T-SQL script and execute it.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TResult Execute<TResult>(Expression expression)
        {
            var queryTranslator = new QueryTranslator();
            throw new NotImplementedException();
        }

        /// <summary>
        /// Translate the expression tree into T-SQL and return the value.
        /// </summary>
        /// <returns>Sql script</returns>
        public string TranslateToSql(Expression expression)
        {
            var queryTranslator = new QueryTranslator();
            return queryTranslator.Translate(expression);
        }
    }
}
