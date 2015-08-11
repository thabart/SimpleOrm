using ORM.Models;
using System;
using System.Linq;
using System.Reflection;

namespace ORM.Core
{
    public class BaseDBContext : IDisposable
    {
        private readonly ConnectionManager _connectionManager;

        private bool _isDisposed;

        public BaseDBContext(string connectionString)
        {
            _connectionManager = new ConnectionManager(connectionString);

            _isDisposed = false;
            InitializeDbSets();
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

        private void InitializeDbSets()
        {
            var type = GetType();
            var dbSetType = typeof(DbSet<>);
            var properties = type.GetProperties();     
            foreach(PropertyInfo property in properties)
            {
                var implementInterface = property.PropertyType
                    .GetInterfaces()
                    .Any(i => i.GetGenericTypeDefinition() == typeof(IDbSet<>));
                if (implementInterface)
                {
                    var dbSet = Activator.CreateInstance(property.PropertyType, new[] { this });
                    property.SetValue(this, dbSet, null);
                }
            }            
        }
    }
}
