using System.Text;

namespace ORM.Query
{
    public class SelectTableCommand : BaseSelectCommand
    {
        public string TableName { get; set; }

        public string GetSqlCommand()
        {
            var stringBuilder = new StringBuilder();
            var selectPattern = "SELECT {0} FROM {1}";
            var columnsBuilder = string.Join(",", Columns.ToArray());

            var result = string.Format(selectPattern, columnsBuilder, TableName);
            return result;
        }
    }
}
