using System;

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
    }
}
