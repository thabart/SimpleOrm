using ORM.Mappings;

namespace ORM.Core
{
    public interface IQueryExecutor
    {
        object ExecuteReaderAndReturnList(string sqlScript, EntityMappingDefinition mappingDefinition);

        object ExecuteCommandAndReturnObject(string sqlScript);
    }
}
