using ORM.Mappings;

using System.Collections.Generic;
using System.Data.SqlClient;

namespace ORM.Core
{
    public interface IRecordReader
    {
        object MapToEntity(SqlDataReader reader,
            EntityMappingDefinition entityMappingDefinition,
            List<string> columnNames);
    }
}
