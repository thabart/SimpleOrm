using ORM.Mappings;

using System;

namespace ORM.Core
{
    public class MappingRuleTranslator : IMappingRuleTranslator
    {
        private IEntityMappingContainer _entityMappingContainer;

        public MappingRuleTranslator(IEntityMappingContainer entityMappingContainer)
        {
            _entityMappingContainer = entityMappingContainer;
        }

        public string GetTableName(Type type)
        {
            var entityMapping = _entityMappingContainer.GetEntityMappingDefinition(type);
            return entityMapping.TableName;
        }

        public string GetColumnName(Type type, string propertyName)
        {
            var entityMapping = _entityMappingContainer.GetEntityMappingDefinition(type);
            foreach(var columnDefinition in entityMapping.ColumnDefinitions)
            {
                var sameProperty = propertyName.Equals(columnDefinition.PropertyName, StringComparison.OrdinalIgnoreCase);
                if (sameProperty)
                {
                    return columnDefinition.ColumnName;
                }
            }

            return propertyName;
        }
    }
}
