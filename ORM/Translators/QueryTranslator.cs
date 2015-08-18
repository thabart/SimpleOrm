using ORM.Exceptions;

using System.Linq;
using System.Linq.Expressions;

namespace ORM.Translators
{
    public class QueryTranslator : ExpressionVisitor
    {
        private SelectTranslator _selectTranslator;

        public QueryTranslator()
        {
        }
        
        public override Expression Visit(Expression expression)
        {
            if (expression == null)
            {
                return expression;
            }

            var nodeType = expression.NodeType;
            switch(nodeType)
            {
                case ExpressionType.Call:
                    VisitMethodCall((MethodCallExpression)expression);
                    break;
            }
            return expression;
        }

        protected Expression VisitMethodCall(MethodCallExpression expression)
        {
            var declaringType = expression.Method.DeclaringType;
            if (typeof(IQueryable).IsAssignableFrom(declaringType))
            {
                throw new OrmInternalException("The type of the query is not IQueryable");
            }

            var methodName = expression.Method.Name;
            switch(methodName)
            {
                case "Select":
                    var argument = expression.Arguments;
                    var lambdaExpression = (LambdaExpression)StripQuotes(expression.Arguments[1]);
                    var nodeType = lambdaExpression.NodeType;
                    if (_selectTranslator != null)
                    {
                        throw new OrmInternalException("You cannot have more than one Select operator in this expression");
                    }
                    

                    break;
            }

            return expression;
        }

        private static Expression StripQuotes(Expression expression)
        {
            while(expression.NodeType == ExpressionType.Quote)
            {
                expression = ((UnaryExpression)expression).Operand;
            }

            return expression;
        }

        private void VisitSelect(Expression queryable, LambdaExpression predicate)
        {

        }
    }
}
