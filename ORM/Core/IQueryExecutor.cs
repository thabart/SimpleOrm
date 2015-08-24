using ORM.Mappings;

namespace ORM.Core
{
    public interface IQueryExecutor
    {
        object ExecuteText(string sqlScript, EntityMappingDefinition mappingDefinition);
    }
}
