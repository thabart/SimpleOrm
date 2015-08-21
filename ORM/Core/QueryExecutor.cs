using System;
using System.Collections.Generic;

using ORM.Mappings;

namespace ORM.Core
{
    /// <summary>
    /// Provides functions to execute the query based on the mapping rules.
    /// </summary>
    public class QueryExecutor : IQueryExecutor, IDisposable
    {
        private readonly List<BaseMapping<object>> _mappingRules;

        private readonly ConnectionManager _connectionManager;

        private bool _isDisposed;

        public QueryExecutor()
        {
            _isDisposed = false;
        }

        /// <summary>
        /// Create a new instance of query executor
        /// </summary>
        /// <param name="connectionString"></param>
        public QueryExecutor(string connectionString)
        {
            _mappingRules = new List<BaseMapping<object>>();
            _connectionManager = new ConnectionManager(connectionString);
        }

        public void ExecuteSelect(string sqlScript)
        {
            
        }

        public void Dispose()
        {
            Dispose(_isDisposed);
        }

        private void Dispose(bool isDisposed)
        {
            if (!isDisposed)
            {
                _connectionManager.Dispose();
            }

            _isDisposed = true;
        }
    }
}
