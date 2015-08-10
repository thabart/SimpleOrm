using System;

namespace ORM.Exceptions
{
    public class BaseOrmException : Exception
    {
        public BaseOrmException(string message) : base(message)
        {
        }

        public BaseOrmException(string message, Exception innerException): base(message, innerException)
        {
        }
    }
}
