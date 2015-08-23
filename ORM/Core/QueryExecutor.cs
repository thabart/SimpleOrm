using System;
using System.Collections.Generic;

using ORM.Mappings;
using System.Data.SqlClient;
using System.Data;

namespace ORM.Core
{
    /// <summary>
    /// Provides functions to execute the query based on the mapping rules.
    /// </summary>
    public class QueryExecutor : IQueryExecutor, IDisposable
    {
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
            _connectionManager = new ConnectionManager(connectionString);
        }

        public object ExecuteText(string sqlScript)
        {
            _connectionManager.Open();

            var command = new SqlCommand();
            command.CommandText = sqlScript;
            command.CommandType = CommandType.Text;
            command.Connection = _connectionManager.Connection;

            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine(reader.GetString(0));
                }
            }

            reader.Close();
            _connectionManager.Close();

            return null;
        }

        public List<TResult> ExecuteText<TResult>(string sqlScript)
        {
            _connectionManager.Open();

            var command = new SqlCommand();
            command.CommandText = sqlScript;
            command.CommandType = CommandType.Text;
            command.Connection = _connectionManager.Connection;

            var reader = command.ExecuteReader();

            reader.Close();
            _connectionManager.Close();

            return new List<TResult>();
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
