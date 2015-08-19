using ORM.Exceptions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ORM.Translators
{
    public class QueryTranslator : ExpressionVisitor
    {
        private readonly SelectTranslator _selectTranslator;

        private readonly WhereTranslator _whereTranslator;

        private string _translatedSelect;

        private string _translatedWhere;

        private StringBuilder _builder;

        public QueryTranslator()
        {
            _selectTranslator = new SelectTranslator();

            _translatedSelect = string.Empty;
            _translatedWhere = string.Empty;
        }
        
        /// <summary>
        /// Translate an expression and returns the SQL Script
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public string Translate(Expression expression)
        {
            Visit(expression);
            return GenerateSql();
        }

        /// <summary>
        /// Visit on node of the expression tree.
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
                case ExpressionType.Quote:
                    break;
            }
            return expression;
        }

        /// <summary>
        /// Visit the expression tree node which makes a call to a method.
        /// </summary>
        /// <param name="expression"></param>   
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            var declaringType = expression.Type;
            if (!typeof(IQueryable).IsAssignableFrom(declaringType))
            {
                throw new OrmInternalException("The type of the query is not IQueryable");
            }
            
            var arguments = expression.Arguments;
            foreach (var argument in arguments)
            {
                if (argument.NodeType == ExpressionType.Call)
                {
                    Visit(argument);
                }
            }

            var methodName = expression.Method.Name;
            switch(methodName)
            {
                case "Select":
                    var translated = _selectTranslator.Translate(expression);
                    _translatedSelect = translated;
                    break;
                case "Where":

                    break;
            }

            return expression;
        }

        /// <summary>
        /// When the class has finished to read the expression tree this function is called to generate the corresponding sql script.
        /// </summary>
        /// <returns></returns>
        private string GenerateSql()
        {
            return _translatedSelect;
        }
    }
}
