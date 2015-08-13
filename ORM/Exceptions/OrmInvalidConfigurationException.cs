using System;

namespace ORM.Exceptions
{
    public class OrmInvalidConfigurationException : OrmConfigurationException
    {
        public OrmInvalidConfigurationException(string message) : base(message)
        {
        }

        public OrmInvalidConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
