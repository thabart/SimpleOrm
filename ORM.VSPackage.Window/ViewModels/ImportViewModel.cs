using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System.Windows.Input;

namespace ORM.VSPackage.ImportWindowSqlServer.ViewModels
{
    public class ImportViewModel : BindableBase
    {
        private bool _isWindowsAuthenticationEnabled;

        public ImportViewModel()
        {
            _isWindowsAuthenticationEnabled = false;
            RegisterCommands();
        }

        public ICommand EnableWindowsAuthenticationCommand { get; private set; }

        public bool IsWindowsAuthenticationEnabled
        {
            get
            {
                return _isWindowsAuthenticationEnabled;
            } set
            {
                SetProperty(ref _isWindowsAuthenticationEnabled, value);
            }
        }

        private void RegisterCommands()
        {
            EnableWindowsAuthenticationCommand = new DelegateCommand(EnableWindowsAuthenticationExecute);
        }

        private void EnableWindowsAuthenticationExecute()
        {
            IsWindowsAuthenticationEnabled = !IsWindowsAuthenticationEnabled;
        }
    }
}
