using System.Linq.Expressions;

namespace ORM.Translators
{
    public static class QueryHelper
    {
        public static Expression StripQuotes(Expression e)
        {
            while (e.NodeType == ExpressionType.Quote)
            {
                e = ((UnaryExpression)e).Operand;
            }

            return e;
        }
    }
}
