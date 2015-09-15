using System;

namespace ORM.Mappings
{
    public class ColumnDefinition
    {
        private readonly string _propertyName;

        private readonly Type _type;

        private string _columnName;

        private int _maxLength;

        public ColumnDefinition(string propertyName, Type type)
        {
            _propertyName = propertyName;
            _type = type;
        }

        public string ColumnName
        {
            get
            {
                return _columnName;
            }
        }

        public int MaxLength
        {
            get
            {
                return _maxLength;
            }
        }

        public string PropertyName
        {
            get
            {
                return _propertyName;
            }        
        }

        public Type Type
        {
            get
            {
                return _type;
            }
        }

        public ColumnDefinition ToColumn(string columnName)
        {
            _columnName = columnName;
            return this;
        }

        public ColumnDefinition HasMaxLength(int maxLength)
        {
            _maxLength = maxLength;
            return this;
        }
    }
}
