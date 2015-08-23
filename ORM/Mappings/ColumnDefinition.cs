using System;

namespace ORM.Mappings
{
    public class ColumnDefinition
    {
        private readonly string _propertyName;

        private string _columnName;

        private int _maxLength;

        public ColumnDefinition(string propertyName)
        {
            _propertyName = propertyName;
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

        public ColumnDefinition HasColumnName(string columnName)
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
