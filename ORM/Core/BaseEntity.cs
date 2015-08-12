using ORM.Helpers;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ORM.Core
{
    public abstract class BaseEntity<TSource> where TSource : class
    {
        private readonly Dictionary<string, string> _mappingRules;

        private readonly ReflectorHelper _reflectorHelper; 

        private string _tableName;

        protected BaseEntity()
        {
            _mappingRules = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            _reflectorHelper = new ReflectorHelper();
        }

        public Dictionary<string, string> MappingRules
        {
            get
            {
                return _mappingRules;
            }
        }

        public string TableName
        {
            get
            {
                return _tableName;
            }
        }

        public abstract void Mappings();

        /// <summary>
        /// Insert mapping rule between column name & property
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <param name="property">Property</param>
        protected void AddColumnMapping<TTarget>(string columnName, Expression<Func<TSource, TTarget>> property)
        {
            var propertyName = _reflectorHelper.GetPropertyName(property);
            _mappingRules.Add(columnName, propertyName);
        }

        protected void LinkToTable(string tableName)
        {
            _tableName = tableName;
        }
    }
}
