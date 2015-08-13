using System;

namespace ORM.Exceptions
{
    public class OrmConfigurationException : BaseOrmException
    {
        public OrmConfigurationException(string message) : base(message)
        {
        }

        public OrmConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
