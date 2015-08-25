using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ORM.Core;
using ORM.Exceptions;

namespace ORM.Translators
{
    public class WhereTranslator : ExpressionVisitor
    {
        private readonly IMappingRuleTranslator _mappingRuleTranslator;

        private StringBuilder _builder;

        private Type _genericType;

        public WhereTranslator(IMappingRuleTranslator mappingRuleTranslator)
        {
            _mappingRuleTranslator = mappingRuleTranslator;
        }

        /// <summary>
        /// Translate the expression into sql script.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public string Translate(Expression expression)
        {
            _builder = new StringBuilder();
            Visit(expression);
            return _builder.ToString();
        }

        public override Expression Visit(Expression expression)
        {
            var nodeType = expression.NodeType;
            switch (nodeType)
            {
                case ExpressionType.Call:
                    VisitMethodCall((MethodCallExpression) expression);
                    break;
            }
            Trace.WriteLine("test");

            return expression;
        }

        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            var declaringType = expression.Type;
            if (!typeof(IQueryable).IsAssignableFrom(declaringType))
            {
                throw new OrmInternalException("The type of the query is not IQueryable");
            }

            if (!declaringType.IsGenericType)
            {
                throw new OrmInternalException("The type of the query is not generic");
            }

            if (declaringType.GetGenericArguments().Count() > 1)
            {
                throw new OrmInternalException("The type of the query is generic but contains more than one argument");
            }

            _genericType = declaringType.GetGenericArguments().First();

            _builder.Append("WHERE ");
            var secondArgument = expression.Arguments[1];
            var lambdaExpression = (LambdaExpression)QueryHelper.StripQuotes(secondArgument);
            var bodyLambdaExpression = lambdaExpression.Body;

            var binaryExpression = (BinaryExpression)bodyLambdaExpression;

            return expression;
        }
    }
}
