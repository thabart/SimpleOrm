using ORM.Exceptions;
using ORM.LinqToSql;
using ORM.Mappings;

using System;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace ORM.Core
{
    public class BaseDbContext : IDisposable
    {
        private QueryExecutor _queryExecutor;

        private IMappingRuleTranslator _mappingRuleTranslator;

        private IQueryTracker _queryTracker;
        
        private bool _isDisposed;
        
        /// <summary>
        /// Configure the base db context via a connection string name
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string</param>
        public BaseDbContext(string connectionStringName)
        {
            var connectionString = LoadConnectionStringFromConfigurationFile(connectionStringName);
            InitializeDbContext(connectionString);
        }

        /// <summary>
        /// DBContext of the constructor. The connection string is deduced based on the data source & catalog.
        /// The integrated security mode is enabled
        /// </summary>
        /// <param name="dataSource">Data source</param>
        /// <param name="initialCatalog">Catalog</param>
        public BaseDbContext(string dataSource, string initialCatalog)
        {
            var connectionString = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True;", dataSource, initialCatalog);
            InitializeDbContext(connectionString);
        }

        public void Dispose()
        {
            Dispose(_isDisposed);
        }

        public void Dispose(bool isDisposed)
        {
            if (!isDisposed)
            {
                _queryExecutor.Dispose();
            }

            _isDisposed = true;
        }

        public void SaveChanges()
        {
            _queryTracker.ExecuteQueries();
        }

        /// <summary>
        /// Initialize the DbContext and initialize the mapping roles.
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        private void InitializeDbContext(string connectionString)
        {
            _queryExecutor = new QueryExecutor(connectionString);
            _queryTracker = new QueryTracker();
            var entityMappingContainer = new EntityMappingContainer();
            _mappingRuleTranslator = new MappingRuleTranslator(entityMappingContainer);

            _isDisposed = false;

            InitializeDbSets();

            Mappings(entityMappingContainer);
        }

        /// <summary>
        /// Initialize the DBSet properties by type reflection.
        /// </summary>
        private void InitializeDbSets()
        {
            var type = GetType();
            var dbSetType = typeof(DbSet<>);
            var properties = type.GetRuntimeProperties()
                .Where(p => (p.PropertyType.GetInterfaces().Count() > 0 &&
                    p.PropertyType.GetInterfaces().Any(i => CheckPropertyInfoImplementsIDbSet(i)))
                    || (CheckPropertyInfoImplementsIDbSet(p.PropertyType)))
                .Select(p => new
                {
                    p,
                    p.PropertyType
                });
            
            if (properties != null && properties.Any())
            {
                foreach(var property in properties)
                {
                    var queryProvider = new QueryProvider(_queryExecutor, _mappingRuleTranslator, _queryTracker);
                    var genericArguments = property.p.PropertyType.GetGenericArguments();
                    var constructedType = dbSetType.MakeGenericType(genericArguments);
                    var dbSet = Activator.CreateInstance(constructedType, new[] { queryProvider });
                    property.p.SetValue(this, dbSet, null);
                }
            }
        }

        /// <summary>
        /// Returns the connection string from configuration file
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string</param>
        /// <returns>Connection string</returns>
        private static string LoadConnectionStringFromConfigurationFile(string connectionStringName)
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName];
                return connectionString.ConnectionString;
            }
            catch (ConfigurationErrorsException exception)
            {
                throw new OrmInvalidConfigurationException(
                    string.Format("The connection string {0} doesn't exist", connectionStringName),
                    exception);
            }
        }

        /// <summary>
        /// Check if the type implements the IDbSet interface.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static bool CheckPropertyInfoImplementsIDbSet(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDbSet<>);
        }

        /// <summary>
        /// Load the mapping roles.
        /// </summary>
        protected virtual void Mappings(IEntityMappingContainer entityMappingContainer)
        {
            ResolveMappingRolesByConvention(entityMappingContainer);
        }

        /// <summary>
        /// Resolve the mappings roles by convention.
        /// </summary>
        /// <param name="entityMappingContainer"></param>
        private static void ResolveMappingRolesByConvention(IEntityMappingContainer entityMappingContainer)
        {
            // TODO : Implement this method.
        }
    }
}
