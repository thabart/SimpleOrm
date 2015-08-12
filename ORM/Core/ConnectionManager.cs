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

        public void Dispose()
        {
            Dispose(_isDisposed);
        }

        private void EstablishConnection()
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                // TODO : Returns the appropriate exception
                throw new BaseOrmException("the connection string cannot be null or empty");
            }

            try
            {
                _connection = new SqlConnection(_connectionString);
            }
            catch (Exception ex)
            {
                throw new BaseOrmException("An exception occured when attempting to open the sql connection");
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
