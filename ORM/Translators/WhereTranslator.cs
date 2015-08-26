using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

using ORM.Core;
using ORM.Helpers;

namespace ORM.Translators
{
    /// <summary>
    /// Translate a where method call expression into sql script.
    /// </summary>
    public class WhereTranslator : ExpressionVisitor
    {
        private readonly IMappingRuleTranslator _mappingRuleTranslator;

        private readonly List<Type> _listOfTypesWithQuotes;

        private StringBuilder _builder;

        private Type _genericType;

        /// <summary>
        /// Constructor <see cref="WhereTranslator"/>
        /// </summary>
        /// <param name="mappingRuleTranslator"></param>
        public WhereTranslator(IMappingRuleTranslator mappingRuleTranslator)
        {
            _mappingRuleTranslator = mappingRuleTranslator;
            _listOfTypesWithQuotes = new List<Type>()
            {
                typeof(string),
                typeof(Guid),
                typeof(char)
            };
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

        /// <summary>
        /// Visit any expression.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public override Expression Visit(Expression expression)
        {
            var nodeType = expression.NodeType;
            switch (nodeType)
            {
                case ExpressionType.Call:
                    VisitMethodCall((MethodCallExpression) expression);
                    break;
                case ExpressionType.Equal:
                case ExpressionType.GreaterThanOrEqual:
                case ExpressionType.LessThanOrEqual:
                case ExpressionType.NotEqual:
                case ExpressionType.LessThan:
                case ExpressionType.GreaterThan:
                case ExpressionType.AndAlso:
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    VisitBinary((BinaryExpression)expression);
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

        /// <summary>
        /// Visit a call to a method.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            _genericType = ExpressionHelper.GetFirstGenericTypeArgumentOfMethodCallExpression(expression);

            _builder.Append("WHERE ");

            var secondArgument = expression.Arguments[1];
            var lambdaExpression = (LambdaExpression)QueryHelper.StripQuotes(secondArgument);
            var bodyLambdaExpression = lambdaExpression.Body;
            var binaryExpression = (BinaryExpression)bodyLambdaExpression;
            Visit(binaryExpression);

            return expression;
        }

        /// <summary>
        /// Visit the binary expression and fill in the sql script.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression expression)
        {
            var left = expression.Left;
            var right = expression.Right;

            Visit(left);
            switch(expression.NodeType)
            {
                case ExpressionType.Equal:
                    _builder.Append(" = ");
                    break;
                case ExpressionType.AndAlso:
                    _builder.Append(" AND ");
                    break;
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    _builder.Append(" OR ");
                    break;
            }

            Visit(right);
            return expression;
        }

        /// <summary>
        /// Visit a constant expression and fill in the sql script.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression expression)
        {
            var result = "{0}";
            if (_listOfTypesWithQuotes.Contains(expression.Type))
            {
                result = "'{0}'";
            }
            
            _builder.Append(string.Format(result, expression.Value));
            return expression;
        }

        /// <summary>
        /// Visit a member access expression and fill in the sql script.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected override Expression VisitMember(MemberExpression expression)
        {
            var memberName = expression.Member.Name;
            var columnName = _mappingRuleTranslator.GetColumnName(_genericType, memberName);
            _builder.Append(columnName);
            return expression;
        }
    }
}
