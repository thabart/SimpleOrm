using System;

namespace ORM.Mappings
{
    public class BaseMapping<TSource> where TSource : class
    {
        private string _tableName;

        protected void ToTable(string tableName)
        {
            _tableName = tableName;
        }

        protected ColumnDefinition Property<TProperty>(Func<TSource, TProperty> callback)
        {
            var columnDefinition = new ColumnDefinition();
            return columnDefinition;
        }
    }
}
