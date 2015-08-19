using ORM.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ORM.Translators
{
    public class SelectTranslator : ExpressionVisitor
    {
        private StringBuilder _builder;

        public string Translate(Expression expression)
        {
            _builder = new StringBuilder();
            Visit(expression);
            return _builder.ToString();
        }

        public override Expression Visit(Expression expression)
        {
            var nodeType = expression.NodeType;
            switch(nodeType)
            {
                case ExpressionType.Call:
                    VisitMethodCall((MethodCallExpression)expression);
                    break;
                case ExpressionType.Constant:
                    VisitConstant((ConstantExpression)expression);
                    break;
                case ExpressionType.MemberAccess:
                    VisitMember((MemberExpression)expression);
                    break;
                case ExpressionType.New:
                    VisitNew((NewExpression)expression);
                    break;
            }

            return expression;
        }

        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            var declaringType = expression.Type;
            if (!typeof(IQueryable).IsAssignableFrom(declaringType))
            {
                throw new OrmInternalException("The type of the query is not IQueryable");
            }

            _builder.Append("SELECT ");
            var secondArgument = expression.Arguments[1];

            var lambdaExpression = (LambdaExpression)QueryHelper.StripQuotes(secondArgument);
            var bodyLambdaExpression = lambdaExpression.Body;
            var nodeType = bodyLambdaExpression.NodeType;

            Visit(bodyLambdaExpression);

            var genericArgument = declaringType.GetGenericArguments().First();
            var table = genericArgument.Name;
            _builder.Append(" FROM ");
            _builder.Append(table);

            return expression;
        }

        protected override Expression VisitConstant(ConstantExpression expression)
        {
            var queryable = expression.Value as IQueryable;
            if (queryable != null)
            {
                _builder.Append(queryable.ElementType.Name);
            }

            return expression;
        }

        protected override Expression VisitMember(MemberExpression expression)
        {
            if (expression.Expression != null && expression.Expression.NodeType == ExpressionType.Parameter)
            {
                _builder.Append(expression.Member.Name);
            }

            return expression;
        }

        /// <summary>
        /// Visits an expression which is making a call to the constructor.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected override Expression VisitNew(NewExpression expression)
        {
            var members = expression.Members;
            var columnNames = new List<string>();
            foreach(var member in members)
            {
                columnNames.Add(member.Name);
            }

            _builder.Append(string.Join(",", columnNames));
            return expression;
        }
    }
}
