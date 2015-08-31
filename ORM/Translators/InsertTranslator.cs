using ORM.Core;
using ORM.Helpers;
using System;
using System.Collections.Generic;
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

            if (!expression.Method.IsGenericMethod)
            {
                // TODO : Throw the appropriate exception.
            }

            var firstParameter = expression.Method.GetGenericArguments().FirstOrDefault();
            _genericType = firstParameter;
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
            var value = expression.Value;
            var type = expression.Type;
            var properties = type.GetProperties();
            var columnNames = new List<string>();
            var columnValues = new List<string>();
            foreach(var property in properties)
            {
                var columnName = _mappingRuleTranslator.GetColumnName(type, property.Name);
                var columnValue = property.GetValue(value);
                var columnType = _mappingRuleTranslator.GetColumnDefinition(type, property.Name).Type;
                columnNames.Add(columnName);
                columnValues.Add(GenerateSqlScriptHelper.ConvertConstantIntoSqlScript(columnType, columnValue));
            }

            var sqlRequest = string.Format("({0}) VALUES ({1})", string.Join(",", columnNames), string.Join(",", columnValues));
            _builder.Append(sqlRequest);
            return expression;
        }
    }
}
