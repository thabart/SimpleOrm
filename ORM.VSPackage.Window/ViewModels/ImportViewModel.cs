using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;

using System;
using System.Data.SqlClient;
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

        public ICommand TestConnectionCommand { get; private set; }

        public string DataSource { private get; set; }

        public string UserName { private get; set; }

        public string Password { private get; set; }

        public string Catalog { private get; set; }

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
            TestConnectionCommand = new DelegateCommand(TestConnectionExecute);
        }

        private void EnableWindowsAuthenticationExecute()
        {
            IsWindowsAuthenticationEnabled = !IsWindowsAuthenticationEnabled;
        }

        private void TestConnectionExecute()
        {
            var connectionString = CreateConnectionString();
            var connectionStringValid = IsConnectionStringValid(connectionString);

        }

        private string CreateConnectionString()
        {
            var connectionString = string.Format("Data Srouce={0};Initial Catalog={1};", DataSource, Catalog);
            if (IsWindowsAuthenticationEnabled)
            {
                connectionString += "Integrated Security=True;";
            }
            else
            {
                connectionString += string.Format("User Id={0};Password={1};", UserName, Password);
            }

            return connectionString;
        }

        private bool IsConnectionStringValid(string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
