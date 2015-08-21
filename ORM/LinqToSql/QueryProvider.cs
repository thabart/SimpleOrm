using System.Data;
using System.Data.SqlClient;
using ORM.Core;
using ORM.Translators;

using System;
using System.Linq;
using System.Linq.Expressions;

namespace ORM.LinqToSql
{
    public class QueryProvider : IQueryProvider
    {
        private readonly IQueryExecutor _queryExecutor;

        public QueryProvider(IQueryExecutor queryExecutor)
        {
            _queryExecutor = queryExecutor;
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
            /*
            
            _connectionManager.Open();
            
            var command = new SqlCommand();
            command.CommandText = query;
            command.CommandType = CommandType.Text;
            command.Connection = _connectionManager.Connection;

            var reader = command.ExecuteReader();

            reader.Close();
            _connectionManager.Close();
            */

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
