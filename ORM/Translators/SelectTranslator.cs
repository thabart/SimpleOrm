using System.Linq.Expressions;

namespace ORM.Exceptions
{
    public class SelectTranslator : ExpressionVisitor
    {
        public override Expression Visit(Expression expression)
        {
            var nodeType = expression.NodeType;
            switch(nodeType)
            {
                case ExpressionType.Lambda:
                    VisitLambda((LambdaExpression)expression);
                    break;
            }

            return expression;
        }

        private Expression VisitLambda(LambdaExpression expression)
        {
            return expression;
        }
    }
}
