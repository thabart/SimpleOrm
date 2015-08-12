using System;
using System.Linq.Expressions;

namespace ORM.Helpers
{
    public class ReflectorHelper
    {
        public string GetPropertyName<TSource, TTarget>(Expression<Func<TSource, TTarget>> callback = null)
        {
            var propertyLambda = callback.Body as MemberExpression;
            var propertyName = propertyLambda.Member.Name;
            return propertyName;
        }
    }
}
