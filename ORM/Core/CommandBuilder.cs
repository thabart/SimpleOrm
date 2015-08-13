using ORM.Helpers;
using ORM.Query;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace ORM.Core
{
    public class CommandBuilder<TSource, TTarget> where TSource : class
    {
        private readonly ReflectorHelper _reflectorHelper;

        private readonly BaseEntity<TSource> _entity;

        private readonly ConnectionManager _connectionManager;

        private KeyValuePair<SelectTableCommand, Func<TSource, TTarget>> _command;

        public CommandBuilder(BaseEntity<TSource> entity, ConnectionManager connectionManager)
        {
            _reflectorHelper = new ReflectorHelper();
            _entity = entity;
            _connectionManager = connectionManager;
        }

        public CommandBuilder<TSource, TTarget> Select(Expression<Func<TSource, TTarget>> callback)
        {   
            var selectCommand = new SelectTableCommand
            {
                TableName = _entity.TableName,
                Columns = new List<string> { "*" },
            
            };

            var compiledCallback = callback.Compile();
            Func<TSource, TTarget> setValueCallBack = (o) =>
            {
                return compiledCallback(o);
            };

            _command = new KeyValuePair<SelectTableCommand, Func<TSource, TTarget>>(selectCommand, setValueCallBack);
            return this;
        }

        public CommandBuilder<TSource, TTarget> Where()
        {
            return this;
        }

        public string GetSqlCommand()
        {
            return _command.Key.GetSqlCommand();
        }
        
        public List<TTarget> Execute()
        {
            _connectionManager.Open();
            var command = new SqlCommand();

            var result = new List<TTarget>();

            command.CommandText = GetSqlCommand();
            command.CommandType = CommandType.Text;
            command.Connection = _connectionManager.Connection;

            var reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                // Retrieve all the columns.
                var mappingColumnNameAndIndex = new Dictionary<int, string>();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    var columnName = reader.GetName(i);
                    mappingColumnNameAndIndex.Add(i, columnName);
                }

                // Fetch the records
                while (reader.Read())
                {
                    var record = Activator.CreateInstance<TSource>();
                    foreach(var mapping in mappingColumnNameAndIndex)
                    {
                        var columnName = mapping.Value;
                        var columnIndex = mapping.Key;
                        var value = reader.GetValue(columnIndex);
                        var r = _entity.MappingRules[columnName];
                        r(record, value);
                    }

                    var newRecord = _command.Value(record);
                    result.Add(newRecord);
                }
            }

            _connectionManager.Close();
            return result;
        }
    }
}
