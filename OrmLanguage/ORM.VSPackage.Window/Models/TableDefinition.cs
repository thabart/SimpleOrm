using Microsoft.Practices.Prism.Mvvm;
using System.Collections.Generic;

namespace ORM.VSPackage.ImportWindowSqlServer.Models
{
    public class TableDefinition : BindableBase
    {
        public string TableName { get; set; }

        public string TableSchema { get; set; }

        public List<ColumnDefinition> ColumnDefinitions { get; set; }
    }
}
