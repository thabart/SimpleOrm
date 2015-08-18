using System;
using System.Linq.Expressions;

namespace ORM.Expressions
{
    public class SelectExpression<T> : Expression
    {
        public bool IsByRef { get; }

        public string Name { get; }

        public sealed override ExpressionType NodeType { get; }

        public override Type Type { get; }
        
        protected override Expression Accept(ExpressionVisitor visitor)
        {
            return null;
        }
    }
}
