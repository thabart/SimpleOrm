using EnvDTE;

using ORM.VSPackage.ImportWindowSqlServer.Models;

namespace ORM.VSPackage.Helper
{
    public static class ColumnDefinitionHelper
    {
        public static vsCMTypeRef GetRefTypeOfColumnDefinition(ColumnDefinition columnDefinition)
        {
            var type = vsCMTypeRef.vsCMTypeRefString;
            switch (columnDefinition.ColumnType)
            {
                case "varchar":
                case "uniqueidentifier":
                    type = vsCMTypeRef.vsCMTypeRefString;
                    break;
            }

            return type;
        }

        public static string GetPropertyName(ColumnDefinition columnDefinition)
        {
            return "_" + columnDefinition.ColumnName.ToLower();
        }
    }
}
