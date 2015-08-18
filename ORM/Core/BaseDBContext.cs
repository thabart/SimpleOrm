using ORM.Exceptions;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace ORM.Core
{
    public class BaseDBContext : IDisposable
    {
        private ConnectionManager _connectionManager;

        private bool _isDisposed;

        /// <summary>
        /// Configure the base db context via a connection string name
        /// </summary>
        /// <param name="connectionStringName">Name of the connection string</param>
        public BaseDBContext(string connectionStringName)
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
        public BaseDBContext(string dataSource, string initialCatalog)
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
                _connectionManager.Dispose();
            }

            _isDisposed = true;
        }

        /// <summary>
        /// Initialize the DbContext
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        private void InitializeDbContext(string connectionString)
        {
            _connectionManager = new ConnectionManager(connectionString);

            _isDisposed = false;
            InitializeDbSets();
        }

        /// <summary>
        /// Initialize the DBSet properties by type reflection.
        /// </summary>
        private void InitializeDbSets()
        {
            var type = GetType();
            var dbSetType = typeof(DbSet<>);
            var properties = type.GetRuntimeProperties()
                .Where(p => p.PropertyType.GetInterfaces().Any(i => i.GetGenericTypeDefinition() == typeof(IDbSet<>)))
                .Select(p => new
                {
                    p,
                    p.PropertyType
                });
            if (properties != null && properties.Any())
            {
                foreach(var property in properties)
                {
                    var dbSet = Activator.CreateInstance(property.PropertyType, new[] { _connectionManager });
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
    }
}
