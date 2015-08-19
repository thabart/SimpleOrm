using System;
using System.Data.SqlClient;

namespace ORM.Core
{
    public interface IConnectionManager : IDisposable
    {
        SqlConnection Connection { get; }

        /// <summary>
        /// Open the connection.
        /// </summary>
        void Open();

        /// <summary>
        /// Close the connection
        /// </summary>
        void Close();
    }
}
