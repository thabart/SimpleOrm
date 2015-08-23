using System;

namespace ORM.Core
{
    public interface IMappingRuleTranslator
    {
        string GetTableName(Type type);

        string GetColumnName(Type type, string propertyName);
    }
}
