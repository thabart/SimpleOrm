namespace ORM.Mappings
{
    public class ColumnDefinition
    {
        private string _columnName;

        private int _maxLength;

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
