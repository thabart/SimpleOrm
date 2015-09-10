using ORM.VSPackage.ImportWindowSqlServer.Models;

namespace ORM.VSPackage.Helper
{
    public static class TableDefinitionHelper
    {
        public static string GetModelFileName(TableDefinition tableDefinition)
        {
            return tableDefinition.TableName;
        }

        public static string GetMappingFileName(TableDefinition tableDefinition)
        {
            return tableDefinition.TableName + "Mapping";
        }
    }
}
