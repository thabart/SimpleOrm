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
            throw new NotImplementedException();
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
            var dbSetType = typeof(DbSet<>);
            var properties = GetType().GetProperties();
            foreach(var property in properties)
            {
                var propertyInfo = property.PropertyType
                    .GetInterfaces()
                    .FirstOrDefault(i => i.GetGenericTypeDefinition() == typeof(IDbSet<>));
                if (propertyInfo != null)
                {

                }
            }

            string s = "zouzou";
        }
    }
}
