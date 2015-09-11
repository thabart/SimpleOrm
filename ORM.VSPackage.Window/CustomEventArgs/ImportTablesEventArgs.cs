using ORM.VSPackage.ImportWindowSqlServer.Models;
using System;
using System.Collections.Generic;

namespace ORM.VSPackage.ImportWindowSqlServer.CustomEventArgs
{
    public class ImportTablesEventArgs : EventArgs
    {
        private readonly List<TableDefinition> _tableDefinitions;

        private readonly string _connectionString;

        public ImportTablesEventArgs(
            List<TableDefinition> tableDefinition, 
            string connectionString)
        {
            _tableDefinitions = tableDefinition;
            _connectionString = connectionString;
        }

        public List<TableDefinition> TableDefinitions
        {
            get
            {
                return _tableDefinitions;
            }
        }

        public string ConnectionString
        {
            get { return _connectionString; }
        }
    }
}
