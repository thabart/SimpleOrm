using ORM.Mappings;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ORM.Core
{
    /// <summary>
    /// Provides functions to execute the query based on the mapping rules.
    /// </summary>
    public class QueryExecutor : IQueryExecutor, IDisposable
    {
        private readonly ConnectionManager _connectionManager;

        private readonly IRecordReader _recordReader;

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
            _recordReader = new RecordReader();
        }

        /// <summary>
        /// Execute the sql script and returns the list
        /// </summary>
        /// <param name="sqlScript"></param>
        /// <param name="entityMappingDefinition"></param>
        /// <returns></returns>
        public object ExecuteReaderAndReturnList(string sqlScript, EntityMappingDefinition entityMappingDefinition)
        {
            _connectionManager.Open();

            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(entityMappingDefinition.EntityType);
            IList result = (IList)Activator.CreateInstance(constructedListType);
            
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
                    var instance = _recordReader.MapToEntity(reader, entityMappingDefinition, columnNames);

                    result.Add(instance);
                }
            }

            reader.Close();
            _connectionManager.Close();

            return result;
        }

        /// <summary>
        /// Execute the sql script and returns the object.
        /// </summary>
        /// <param name="sqlScript"></param>
        /// <param name="entityMappingDefinition"></param>
        /// <returns></returns>
        public object ExecuteCommandAndReturnObject(string sqlScript)
        {
            _connectionManager.Open();

            var command = new SqlCommand();
            command.CommandText = sqlScript;
            command.CommandType = CommandType.Text;
            command.Connection = _connectionManager.Connection;

            var result = command.ExecuteNonQuery();
            _connectionManager.Close();
            return result;
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
