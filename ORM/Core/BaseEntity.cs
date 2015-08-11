using System;
using System.Collections.Generic;

namespace ORM.Core
{
    public abstract class BaseEntity
    {
        private readonly Dictionary<string, Func<object>> _mappingRules;

        protected BaseEntity()
        {
            _mappingRules = new Dictionary<string, Func<object>>();
        }

        public abstract void Mappings();

        /// <summary>
        /// Insert mapping rule between column name & property
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <param name="property">Property</param>
        protected void AddColumnMapping(string columnName, Func<object> property)
        {
            _mappingRules.Add(columnName, property);
        }
    }
}
