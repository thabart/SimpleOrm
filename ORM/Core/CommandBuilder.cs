using ORM.Helpers;
using ORM.Query;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ORM.Core
{
    public class CommandBuilder<TSource> where TSource : class
    {
        private const string SelectStar = "*";

        private readonly ReflectorHelper _reflectorHelper;

        private readonly BaseEntity<TSource> _entity;

        private SelectTableCommand _command;

        public CommandBuilder(BaseEntity<TSource> entity)
        {
            _reflectorHelper = new ReflectorHelper();
            _entity = entity;
        }

        public CommandBuilder<TSource> Select<TTarget>(Expression<Func<TSource, TTarget>> callback)
        {
            var column = SelectStar;
            if (callback != null)
            {
                var propertyName = _reflectorHelper.GetPropertyName(callback);
                var keyPair = _entity.MappingRules.FirstOrDefault(d => d.Value.ToUpperInvariant() == propertyName.ToUpperInvariant());
                if (!string.IsNullOrEmpty(keyPair.Key))
                {
                    column = keyPair.Key;
                }
            }

            if (_command == null)
            {
                _command = new SelectTableCommand
                {
                    TableName = _entity.TableName,
                    Columns = new List<string> { column }
                };

                return this;
            }
            
            if (column != SelectStar)
            {
                _command.Columns.Add(column);
            }            

            return this;
        }

        public CommandBuilder<TSource> Where()
        {
            return this;
        }

        public string GetSqlCommand()
        {
            return _command.GetSqlCommand();
        }
        
        public List<TSource> Execute()
        {
            return null;
        }
    }
}
