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

        /// <summary>
        /// Execute the sql script and returns the result.
        /// </summary>
        /// <param name="sqlScript"></param>
        /// <returns></returns>
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
                var columnNames = GetColumnNames(reader);
                while (reader.Read())
                {
                    Console.WriteLine(reader.GetString(0));
                }
            }

            reader.Close();
            _connectionManager.Close();

            return new List<string>();
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

        private static List<string> GetColumnNames(SqlDataReader reader)
        {
            var columns = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                columns.Add(reader.GetName(i));
            }

            return columns;
        }
    }
}
