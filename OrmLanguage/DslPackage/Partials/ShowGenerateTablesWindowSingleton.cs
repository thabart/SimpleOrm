using System;

using ORM.VSPackage.ImportWindowSqlServer;
using ORM.VSPackage.ImportWindowSqlServer.CustomEventArgs;
using ORM.VSPackage.ImportWindowSqlServer.ViewModels;

namespace Company.OrmLanguage.Partials
{
    public class ShowGenerateTablesWindowSingleton
    {
        private static ShowGenerateTablesWindowSingleton _showGenerateTablesWindowSingleton;

        private ImportView _importView;

        private bool _isClosed;

        private readonly Action<object, ImportTablesEventArgs> _callback;

        private ShowGenerateTablesWindowSingleton(Action<object, ImportTablesEventArgs> callback)
        {
            _isClosed = true;
            _callback = callback;
        }

        public static ShowGenerateTablesWindowSingleton Instanciate(Action<object, ImportTablesEventArgs> callback)
        {
            if (_showGenerateTablesWindowSingleton == null)
            {
                _showGenerateTablesWindowSingleton = new ShowGenerateTablesWindowSingleton(callback);
            }

            return _showGenerateTablesWindowSingleton;
        }

        public void Show()
        {
            if (_callback == null)
            {
                return;
            }

            if (_isClosed)
            {
                _importView = new ImportView();
                _importView.Loaded += (sender, args) =>
                {
                    _isClosed = false;
                    var importViewModel = (ImportViewModel)_importView.DataContext;
                    importViewModel.ImportTablesEvent += (o, eventArgs) => _callback(o, eventArgs);
                };

                _importView.Closed += (sender, args) => _isClosed = true;
                _importView.Show();
            }

            if (!_importView.IsVisible)
            {
                _importView.Focus();
            }
        }
    }
}
