using System;

namespace ORM.Exceptions
{
    public class OrmInternalException : BaseOrmException
    {
        public OrmInternalException(string message) : base(message)
        {
        }

        public OrmInternalException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
