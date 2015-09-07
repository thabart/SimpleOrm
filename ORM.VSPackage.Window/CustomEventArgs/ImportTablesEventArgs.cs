using ORM.VSPackage.ImportWindowSqlServer.Models;
using System;
using System.Collections.Generic;

namespace ORM.VSPackage.ImportWindowSqlServer.CustomEventArgs
{
    public class ImportTablesEventArgs : EventArgs
    {
        private readonly List<TableDefinition> _tableDefinitions;

        public ImportTablesEventArgs(List<TableDefinition> tableDefinition)
        {
            _tableDefinitions = tableDefinition;
        }

        public List<TableDefinition> TableDefinitions
        {
            get
            {
                return _tableDefinitions;
            }
        }
    }
}
