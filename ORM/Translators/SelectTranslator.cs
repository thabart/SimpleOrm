using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ORM.Exceptions
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
            }

            return expression;
        }

        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            _builder.Append("SELECT ");
            var secondArgument = expression.Arguments[1];
            var lambdaExpression = (LambdaExpression)StripQuotes(secondArgument);
            var bodyLambdaExpression = lambdaExpression.Body;
            Visit(bodyLambdaExpression);

            _builder.Append("FROM ");
            var firstArgument = expression.Arguments[0];
            Visit(firstArgument);

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
                _builder.Append(expression.Member.Name + " ");
            }

            return expression;
        }

        private static Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }

            return e;
        }
    }
}
