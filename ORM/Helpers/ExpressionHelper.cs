using ORM.Exceptions;

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ORM.Helpers
{
    public static class ExpressionHelper
    {
        /// <summary>
        /// Returns first generic type argument of a type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>First generic type argument.</returns>
        public static Type GetFirstGenericTypeArgumentOfType(MethodInfo methodInfo)
        {
            if (!methodInfo.IsGenericMethod)
            {
                throw new OrmInternalException("The type of the query is not generic");
            }

            return methodInfo.GetGenericArguments().First();
        }
    }
}
