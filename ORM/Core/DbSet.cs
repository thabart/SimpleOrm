using System;

using System.Linq.Expressions;

namespace ORM.Core
{
    public interface IDbSet<TSource> where TSource : class
    {
        CommandBuilder<TSource, TTarget> Select<TTarget>(Expression<Func<TSource, TTarget>> callback = null);
    }

    public class DbSet<TSource> : IDbSet<TSource> where TSource : class
    {
        private readonly ConnectionManager _connectionManager;

        private BaseEntity<TSource> _instanciateEntity;

        public DbSet(ConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
            LoadEntity();
        }

        /// <summary>
        /// Select the data.
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="callback"></param>
        /// <returns></returns>
        public CommandBuilder<TSource, TTarget> Select<TTarget>(Expression<Func<TSource, TTarget>> callback)
        {
            // TODO : Interprete the select.
            var commandBuilder = new CommandBuilder<TSource, TTarget>(_instanciateEntity, _connectionManager);
            commandBuilder.Select(callback);
            return commandBuilder;
        }

        /// <summary>
        /// Load the entity definition.
        /// </summary>
        private void LoadEntity()
        {
            var type = typeof(TSource);
            var inheritFromBaseEntity = typeof(BaseEntity<TSource>).IsAssignableFrom(type);
            if (!inheritFromBaseEntity)
            {
                return;
            }
            
            _instanciateEntity = (BaseEntity<TSource>)Activator.CreateInstance(type);
            _instanciateEntity.Mappings();
        }
    }
}
