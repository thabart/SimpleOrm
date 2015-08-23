using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;

namespace ORM.LinqToSql
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public class Queryable<TSource> : IQueryable<TSource>
    {
        private readonly QueryProvider _queryProvider;

        private readonly Expression _expression;
        
        public Queryable(QueryProvider queryProvider)
        {
            _queryProvider = queryProvider;
            _expression = Expression.Constant(this);
        }

        public Queryable(QueryProvider queryProvider, Expression expression)
        {
            _queryProvider = queryProvider;
            _expression = expression;
        }

        public Type ElementType
        {
            get
            {
                return typeof(TSource);
            }
        }

        public Expression Expression
        {
            get
            {
                return _expression;
            }
        }

        public IQueryProvider Provider
        {
            get
            {
                return _queryProvider;
            }
        }

        public IEnumerator<TSource> GetEnumerator()
        {
            return ((IEnumerable<TSource>)_queryProvider.Execute(_expression)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_queryProvider.Execute(_expression)).GetEnumerator();
        }

        public override string ToString()
        {
            var sql = _queryProvider.TranslateToSql(Expression);
            return sql;
        }
    }
}
