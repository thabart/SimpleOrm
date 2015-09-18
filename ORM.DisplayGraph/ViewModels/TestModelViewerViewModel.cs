using System.Collections.Generic;
using System.Linq;
using ORM.DisplayGraph.Components.ModelViewer.ViewModels;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace ORM.DisplayGraph.ViewModels
{
    public class TestModelViewerViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TableDefinition> _tableDefinitions;

        private ObservableCollection<LinkDefinition> _linkDefinitions;

        public TestModelViewerViewModel()
        {
            _tableDefinitions = new ObservableCollection<TableDefinition>();
            _linkDefinitions = new ObservableCollection<LinkDefinition>();

            RegisterCommands();
        }

        public ObservableCollection<TableDefinition> TableDefinitions
        {
            get
            {
                return _tableDefinitions;
            } set
            {
                _tableDefinitions = value;
                RaisePropertyChange("TableDefinitions");
            }
        }

        public ObservableCollection<LinkDefinition> LinkDefinitions
        {
            get { return _linkDefinitions; }
            set
            {
                _linkDefinitions = value;
                RaisePropertyChange("LinkDefinitions");
            }
        } 

        public ICommand AddEntityCommand { get; set; }

        public ICommand ClearEntitiesCommand { get; set; }

        public ICommand AddLinkCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RegisterCommands()
        {
            AddEntityCommand = new DelegateCommand(AddEntityExecute);
            ClearEntitiesCommand = new DelegateCommand(ClearEntitiesExecute);
            AddLinkCommand = new DelegateCommand(AddLinkCommandExecute);
        }

        private void AddEntityExecute()
        {
            var record = new TableDefinition
            {
                TableName = "table_generated",
                ColumnDefinitions = new List<ColumnDefinition>
                {
                    new ColumnDefinition
                    {
                        Name = "Column1",
                        IsPrimaryKey = false,
                        Type = "string"
                    }
                }
            };

            TableDefinitions.Add(record);
        }

        private void ClearEntitiesExecute()
        {
            TableDefinitions = new ObservableCollection<TableDefinition>();
        }

        private void RaisePropertyChange(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        private void AddLinkCommandExecute()
        {
            if (TableDefinitions.Count < 2)
            {
                return;
            }

            var link = new LinkDefinition
            {
                Source = new EndPointDefinition()
                {
                    ColumnDefinition = TableDefinitions.Last().ColumnDefinitions.First(),
                    TableDefinition = TableDefinitions.Last()
                },
                Target = new EndPointDefinition()
                {
                    ColumnDefinition = TableDefinitions[TableDefinitions.Count - 2].ColumnDefinitions.First(),
                    TableDefinition = TableDefinitions[TableDefinitions.Count - 2]
                }
            };

            _linkDefinitions.Add(link);
        }
    }
}
