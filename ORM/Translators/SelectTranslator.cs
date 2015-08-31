using ORM.Core;
using ORM.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ORM.Translators
{
    public class SelectTranslator : ExpressionVisitor
    {
        private readonly IMappingRuleTranslator _mappingRuleTranslator;

        private StringBuilder _builder;

        private Type _genericType;

        public SelectTranslator(IMappingRuleTranslator mappingRuleTranslator)
        {
            _mappingRuleTranslator = mappingRuleTranslator;
        }

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
            _genericType = ExpressionHelper.GetFirstGenericTypeArgumentOfType(expression.Method);

            _builder.Append("SELECT ");
            var secondArgument = expression.Arguments[1];

            var lambdaExpression = (LambdaExpression)QueryHelper.StripQuotes(secondArgument);
            var bodyLambdaExpression = lambdaExpression.Body;

            Visit(bodyLambdaExpression);

            _builder.Append(" FROM ");

            var tableName = _mappingRuleTranslator.GetTableName(_genericType);
            _builder.Append(tableName);

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
            var type = expression.Type;
            if (expression.Expression != null && expression.Expression.NodeType == ExpressionType.Parameter)
            {
                var memberName = expression.Member.Name;
                var columnName = _mappingRuleTranslator.GetColumnName(_genericType, memberName);
                _builder.Append(columnName);
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
                var columnName = _mappingRuleTranslator.GetColumnName(_genericType, member.Name);
                columnNames.Add(columnName);
            }

            _builder.Append(string.Join(",", columnNames));
            return expression;
        }
    }
}
