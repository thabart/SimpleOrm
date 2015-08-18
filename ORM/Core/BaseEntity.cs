using ORM.Helpers;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ORM.Core
{
    public abstract class BaseEntity<TSource> where TSource : class
    {
        private readonly Dictionary<string, Action<TSource, object>> _mappingRules;

        private readonly ReflectorHelper _reflectorHelper; 

        private string _tableName;

        protected BaseEntity()
        {
            _mappingRules = new Dictionary<string, Action<TSource, object>>(StringComparer.OrdinalIgnoreCase);
            _reflectorHelper = new ReflectorHelper();
        }

        public Dictionary<string, Action<TSource, object>> MappingRules
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
            var compiledExpression = property.Compile();
            var memberInfo = (PropertyInfo)_reflectorHelper.GetMemberInfo(property);
            Action<TSource, object> callback = (s,p) =>
            {
                memberInfo.SetValue(s, p);
            };

            _mappingRules.Add(columnName, callback);
        }

        protected void ToTable(string tableName)
        {
            _tableName = tableName;
        }
    }
}
