using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Company.OrmLanguage.Window
{
    public sealed class SimpleOrmMappingWindowViewModel : INotifyPropertyChanged
    {
        private EntityElement _entityElement;

        private string _entityName;

        private string _tableName;

        private ObservableCollection<PropertyMappingViewModel> _propertyMappingViewModels;

        public event PropertyChangedEventHandler PropertyChanged;

        public string EntityName
        {
            get { return _entityName; }
            set
            {
                _entityName = value;
                NotifyPropertyChanged("EntityName");
            }
        }

        public string TableName
        {
            get { return _tableName; }
            set
            {
                _tableName = value;
                using (var transaction = _entityElement.Store.TransactionManager.BeginTransaction())
                {
                    _entityElement.TableName = _tableName;
                    transaction.Commit();
                }

                NotifyPropertyChanged("TableName");
            }
        }

        public ObservableCollection<PropertyMappingViewModel> PropertyMappings
        {
            get { return _propertyMappingViewModels; }
            set
            {
                _propertyMappingViewModels = value;
                NotifyPropertyChanged("PropertyMappings");
            }
        }

        public void Update(EntityElement entityElement)
        {
            PropertyMappings = new ObservableCollection<PropertyMappingViewModel>();
            _entityElement = entityElement;
            EntityName = entityElement.Name;
            TableName = entityElement.TableName;
            foreach (var property in entityElement.Properties)
            {
                var mappingRule = new PropertyMappingViewModel(property)
                {
                    PropertyName = property.Name,
                    ColumnName = property.ColumnName
                };

                PropertyMappings.Add(mappingRule);
            }
        }

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
