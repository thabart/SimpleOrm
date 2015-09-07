using Microsoft.Practices.Prism.Mvvm;

namespace ORM.VSPackage.ImportWindowSqlServer.ViewModels
{
    public class SelectedTable : BindableBase
    {
        public string TableName { get; set; }

        public string TableSchema { get; set; }

        public bool IsSelected { get; set; }
    }
}
