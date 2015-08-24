using System;
using System.Collections.Generic;

using ORM.Mappings;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Collections;

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
        /// <param name="entityMappingDefinition"></param>
        /// <returns></returns>
        public object ExecuteText(string sqlScript, EntityMappingDefinition entityMappingDefinition)
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
                    var instance = Activator.CreateInstance(entityMappingDefinition.EntityType);
                    for (var i = 0; i < columnNames.Count; i++)
                    {
                        var columnName = columnNames[i];
                        var columnDefinition = entityMappingDefinition.ColumnDefinitions.FirstOrDefault(c => c.ColumnName == columnName);
                        var getPropertyInfoType = GetPropertyInfoType(instance, columnDefinition.PropertyName);
                        if(getPropertyInfoType == typeof(int))
                        {
                            var recordValue = reader.GetInt32(i);
                            SetPropertyValue(instance, columnDefinition.PropertyName, recordValue);
                            continue;
                        }
                        else if (getPropertyInfoType == typeof(double))
                        {
                            var recordValue = reader.GetDouble(i);
                            SetPropertyValue(instance, columnDefinition.PropertyName, recordValue);
                            continue;
                        }
                        else if(getPropertyInfoType == typeof(Guid))
                        {
                            var recordValue = reader.GetGuid(i);
                            SetPropertyValue(instance, columnDefinition.PropertyName, recordValue);
                            continue;
                        }
                        else
                        {
                            var recordValue = reader.GetString(i);
                            SetPropertyValue(instance, columnDefinition.PropertyName, recordValue);
                            continue;
                        }
                    }

                    result.Add(instance);
                }
            }

            reader.Close();
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

        private static void SetPropertyValue<TProperty>(object instance, string propertyName, TProperty propertyValue)
        {
            var type = instance.GetType();
            var propertyInfo = type.GetProperty(propertyName,
                BindingFlags.Instance | BindingFlags.Public);
            if (propertyInfo != null && propertyInfo.CanWrite)
            {
                propertyInfo.SetValue(instance, propertyValue);
            }
        }

        private static Type GetPropertyInfoType(object instance, string propertyName)
        {
            var type = instance.GetType();
            var propertyInfo = type.GetProperty(propertyName,
                BindingFlags.Instance | BindingFlags.Public);
            if (propertyInfo == null)
            {
                // TODO : throw the appropriate exception.
            }

            return propertyInfo.PropertyType;
        }
    }
}
