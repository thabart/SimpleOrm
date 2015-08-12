using System;

using System.Linq.Expressions;

namespace ORM.Core
{
    public interface IDbSet<TSource> where TSource : class
    {
        CommandBuilder<TSource> Select<TTarget>(Expression<Func<TSource, TTarget>> callback = null);
    }

    public class DbSet<TSource> : IDbSet<TSource> where TSource : class
    {
        private readonly ConnectionManager _connectionManager;

        private readonly BaseDBContext _dbContext;

        private BaseEntity<TSource> _instanciateEntity;

        public DbSet(BaseDBContext dbContext)
        {
            _dbContext = dbContext;
            LoadEntity();
        }

        /// <summary>
        /// Select the data.
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="callback"></param>
        /// <returns></returns>
        public CommandBuilder<TSource> Select<TTarget>(Expression<Func<TSource, TTarget>> callback)
        {
            // TODO : Interprete the select.
            var commandBuilder = new CommandBuilder<TSource>(_instanciateEntity);
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
