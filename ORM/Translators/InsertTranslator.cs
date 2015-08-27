using ORM.Core;
using ORM.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ORM.Translators
{
    public class InsertTranslator : ExpressionVisitor
    {
        private readonly IMappingRuleTranslator _mappingRuleTranslator;

        private StringBuilder _builder;

        private Type _genericType;

        public InsertTranslator(IMappingRuleTranslator mappingRuleTranslator)
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
            switch (nodeType)
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
        /// Visit the method call expression and fill in the T-SQL script
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            var parameter = expression.Arguments[1];
            if (parameter == null)
            {
                // TODO : Throw the appropriate exception.
            }

            _genericType = ExpressionHelper.GetFirstGenericTypeArgumentOfMethodCallExpression(expression);
            var tableName = _mappingRuleTranslator.GetTableName(_genericType);

            _builder.Append(string.Format("INSERT INTO {0} ", tableName));
            Visit(parameter);

            return expression;
        }

        /// <summary>
        /// Visit the constant expression and fill in the T-SQL script.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression expression)
        {
            return expression;
        }
    }
}
