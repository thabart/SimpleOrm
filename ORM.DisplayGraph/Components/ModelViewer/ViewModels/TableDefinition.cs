using ORM.DisplayGraph.Components.Entity.ViewModel;
using System.Collections.Generic;

namespace ORM.DisplayGraph.Components.ModelViewer.ViewModels
{
    public class TableDefinition
    {
        public string TableName { get; set; }

        public List<ColumnDefinition> ColumnDefinitions { get; set; }
    }
}
