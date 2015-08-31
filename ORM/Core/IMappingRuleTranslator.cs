using ORM.Mappings;

using System;

namespace ORM.Core
{
    public interface IMappingRuleTranslator
    {
        string GetTableName(Type type);

        string GetColumnName(Type type, string propertyName);

        EntityMappingDefinition GetMappingDefinition(Type type);

        ColumnDefinition GetColumnDefinition(Type type, string columnName);
    }
}
