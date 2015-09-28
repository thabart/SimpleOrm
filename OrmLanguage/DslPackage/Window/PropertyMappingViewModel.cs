using System.ComponentModel;

namespace Company.OrmLanguage.Window
{
    public class PropertyMappingViewModel : INotifyPropertyChanged
    {
        private readonly Property _property;

        private string _columnName;

        public PropertyMappingViewModel(Property property)
        {
            _property = property;
        }

        public string PropertyName { get; set; }

        public string ColumnName
        {
            get { return _columnName; }
            set
            {
                _columnName = value;
                using (var transaction = _property.Store.TransactionManager.BeginTransaction())
                {
                    _property.ColumnName = value;
                    transaction.Commit();
                }

                NotifyPropertyChanged("ColumnName");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
