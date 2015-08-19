using System.Linq.Expressions;

namespace ORM.Translators
{
    public class WhereTranslator : ExpressionVisitor
    {
        public string Translate(Expression expression)
        {
            return string.Empty;
        }
    }
}
