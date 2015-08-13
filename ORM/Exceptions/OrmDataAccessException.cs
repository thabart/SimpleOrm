using System;

namespace ORM.Exceptions
{
    public class OrmDataAccessException : BaseOrmException
    {
        public OrmDataAccessException(string message) : base(message)
        {
        }

        public OrmDataAccessException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
