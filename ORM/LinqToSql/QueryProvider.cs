using ORM.Core;
using ORM.Helpers;
using ORM.Translators;

using System;
using System.Linq;
using System.Linq.Expressions;

namespace ORM.LinqToSql
{
    public class QueryProvider : IQueryProvider
    {
        private readonly IQueryExecutor _queryExecutor;

        private readonly IMappingRuleTranslator _mappingRuleTranslator;

        private readonly IQueryTracker _queryTracker;

        public QueryProvider(
            IQueryExecutor queryExecutor,
            IMappingRuleTranslator mappingRuleTranslator,
            IQueryTracker queryTracker)
        {
            _queryExecutor = queryExecutor;
            _mappingRuleTranslator = mappingRuleTranslator;
            _queryTracker = queryTracker;
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
        /// Transforms the expression tree into SQL script, executes it and returns the result.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public object Execute(Expression expression)
        {
            var methodExpression = (MethodCallExpression)expression;
            var genericType = ExpressionHelper.GetFirstGenericTypeArgumentOfType(methodExpression.Method);
            var isACommand = IsACommand(methodExpression);
            var queryTranslator = new QueryTranslator(_mappingRuleTranslator);
            var query = queryTranslator.Translate(expression);
            var mappingDefinition = _mappingRuleTranslator.GetMappingDefinition(genericType);
            object result = null;
            if (isACommand)
            {
                result = query;
                _queryTracker.AddQuery(() => _queryExecutor.ExecuteCommandAndReturnObject(query));
            }
            else
            {
                result = _queryExecutor.ExecuteReaderAndReturnList(query, mappingDefinition);
            }

            return result;
        }

        /// <summary>
        /// Transform the expression tree into T-SQL script and execute it.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TResult Execute<TResult>(Expression expression)
        {
            return (TResult)Execute(expression);
        }

        /// <summary>
        /// Translate the expression tree into T-SQL and return the value.
        /// </summary>
        /// <returns>Sql script</returns>
        public string TranslateToSql(Expression expression)
        {
            var queryTranslator = new QueryTranslator(_mappingRuleTranslator);
            return queryTranslator.Translate(expression);
        }

        private bool IsACommand(MethodCallExpression methodExpression)
        {
            var methodName = methodExpression.Method.Name;
            return methodName == "Add" || methodName == "Remove";
        }
    }
}
