using ORM.Exceptions;

using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ORM.Translators
{
    public class QueryTranslator : ExpressionVisitor
    {
        private readonly SelectTranslator _selectTranslator;

        private StringBuilder _builder;

        public QueryTranslator()
        {
            _selectTranslator = new SelectTranslator();
        }
        
        public string Translate(Expression expression)
        {
            _builder = new StringBuilder();
            Visit(expression);
            return _builder.ToString();
        }

        /// <summary>
        /// Visit an expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
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
                case ExpressionType.Constant:
                    VisitConstant((ConstantExpression)expression);
                    break;
            }
            return expression;
        }

        /// <summary>
        /// Visit a call to a method.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression expression)
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
                    var translated = _selectTranslator.Translate(expression);
                    _builder.Append(translated);
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
    }
}
