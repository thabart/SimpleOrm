using ORM.DisplayGraph.Components.ModelViewer.ViewModels;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace ORM.DisplayGraph.ViewModels
{
    public class TestModelViewerViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TableDefinition> _tableDefinitions;

        public TestModelViewerViewModel()
        {
            _tableDefinitions = new ObservableCollection<TableDefinition>();

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

        public ICommand AddEntityCommand { get; set; }

        public ICommand ClearEntitiesCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RegisterCommands()
        {
            AddEntityCommand = new DelegateCommand(AddEntityExecute);
            ClearEntitiesCommand = new DelegateCommand(ClearEntitiesExecute);
        }

        private void AddEntityExecute()
        {
            var record = new TableDefinition
            {
                TableName = "table_generated"
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
    }
}
