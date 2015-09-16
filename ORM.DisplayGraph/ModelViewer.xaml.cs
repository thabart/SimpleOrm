using ORM.DisplayGraph.Components.Entity.ViewModel;
using System.Collections.Generic;

namespace ORM.DisplayGraph
{
    /// <summary>
    /// Interaction logic for ModelViewer.xaml
    /// </summary>
    public partial class ModelViewer
    {
        public ModelViewer()
        {
            InitializeComponent();

            ColumnDefinitions = new List<ColumnDefinition>
            {
                new ColumnDefinition
                {
                    ColumnName = "Column1"
                }
            };

            DataContext = this;
        }

        public List<ColumnDefinition> ColumnDefinitions { get; set; }
    }
}
