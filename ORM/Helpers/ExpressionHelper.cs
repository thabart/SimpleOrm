using ORM.Exceptions;

using System;
using System.Linq;
using System.Linq.Expressions;

namespace ORM.Helpers
{
    public static class ExpressionHelper
    {
        /// <summary>
        /// Returns first generic type argument of a method call expression.
        /// </summary>
        /// <param name="node">Method call expression.</param>
        /// <returns>First generic type argument.</returns>
        public static Type GetFirstGenericTypeArgumentOfMethodCallExpression(Expression node)
        {
            var declaringType = node.Type;
            if (!typeof(IQueryable).IsAssignableFrom(declaringType))
            {
                throw new OrmInternalException("The type of the query is not IQueryable");
            }

            if (!declaringType.IsGenericType)
            {
                throw new OrmInternalException("The type of the query is not generic");
            }

            if (declaringType.GetGenericArguments().Count() > 1)
            {
                throw new OrmInternalException("The type of the query is generic but contains more than one argument");
            }

            return declaringType.GetGenericArguments().First();
        }
    }
}
