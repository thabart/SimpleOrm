using System;
using System.Data.SqlClient;

using ORM.Exceptions;

namespace ORM.Core
{
    public class ConnectionManager : IDisposable
    {
        private readonly string _connectionString;

        private bool _isDisposed;

        private SqlConnection _connection;

        public ConnectionManager(string connectionString)
        {
            _connectionString = connectionString;
            _isDisposed = false;

            EstablishConnection();
        }

        public SqlConnection Connection
        {
            get
            {
                return _connection;
            }
        }

        ~ConnectionManager()
        {
        }

        public void Open()
        {
            _connection.Open();
        }

        public void Close()
        {
            _connection.Close();
        }

        public void Dispose()
        {
            Dispose(_isDisposed);
        }

        private void EstablishConnection()
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                // TODO : Returns the appropriate exception
                throw new OrmInvalidConfigurationException("It's not possible to establish a connection because the connection string is empty");
            }

            try
            {
                _connection = new SqlConnection(_connectionString);
            }
            catch (Exception ex)
            {
                throw new OrmDataAccessException("Cannot establish a connection to the SQLServer database", ex);
            }
        }

        private void Dispose(bool isDisposed)
        {
            if (!isDisposed)
            {
                _connection.Close();
                _connection.Dispose();
            }

            _isDisposed = true;
        }
    }
}
