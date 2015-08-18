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
        private readonly IQueryProvider _queryProvider;

        private readonly Expression _expression;
        
        public Queryable(IQueryProvider queryProvider)
        {
            _queryProvider = queryProvider;
            _expression = Expression.Constant(this);
        }

        public Queryable(IQueryProvider queryProvider, Expression expression)
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
            return ((IEnumerable<TSource>)_queryProvider.Execute(_expression)).GetEnumerator();
        }

        public override string ToString()
        {
            // Returns the SQL script.
            return string.Empty;
        }
    }
}
