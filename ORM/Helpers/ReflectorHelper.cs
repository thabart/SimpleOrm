using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ORM.Helpers
{
    public class ReflectorHelper
    {
        public string GetMemberName<TSource, TTarget>(Expression<Func<TSource, TTarget>> callback = null)
        {
            var propertyLambda = callback.Body as MemberExpression;
            var propertyName = propertyLambda.Member.Name;
            return propertyName;
        }

        public MemberInfo GetMemberInfo<TSource, TTarget>(Expression<Func<TSource, TTarget>> callback = null)
        {
            var propertyLambda = callback.Body as MemberExpression;
            return propertyLambda.Member;
        }
    }
}
