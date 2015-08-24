using System;
using System.Collections.Generic;

namespace ORM.Mappings
{
    public class EntityMappingDefinition
    {
        private List<ColumnDefinition> _columnDefinitions;

        public EntityMappingDefinition(Type entityType)
        {
            _columnDefinitions = new List<ColumnDefinition>();
            EntityType = entityType;
        }

        public string TableName { get; set; }

        public Type EntityType { get; private set; }

        public List<ColumnDefinition> ColumnDefinitions
        {
            get
            {
                return _columnDefinitions;
            }
        }
    }
}
