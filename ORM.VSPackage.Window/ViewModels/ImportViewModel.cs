using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;

using ORM.VSPackage.ImportWindowSqlServer.CustomEventArgs;
using ORM.VSPackage.ImportWindowSqlServer.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ORM.VSPackage.ImportWindowSqlServer.ViewModels
{
    public class ImportViewModel : BindableBase
    {
        private bool _isWindowsAuthenticationEnabled;

        private bool _isConnectionSuccessful;

        private bool _databasesAreRetrieved;

        public ImportViewModel()
        {
            _isWindowsAuthenticationEnabled = false;
            _isConnectionSuccessful = false;
            _databasesAreRetrieved = false;
            Tables = new ObservableCollection<SelectedTable>();
            Catalogs = new ObservableCollection<string>();

            RegisterCommands();
        }

        public delegate void ImportTablesHandler(object sender, ImportTablesEventArgs args);

        public event ImportTablesHandler ImportTablesEvent;

        public ICommand TestConnectionCommand { get; private set; }

        public ICommand GenerateTablesCommand { get; private set; }

        public ICommand DeployCatalogCommand { get; private set; }

        public ObservableCollection<string> Catalogs { get; private set; } 

        public string DataSource { private get; set; }

        public string UserName { private get; set; }

        public string Password { private get; set; }

        public string Catalog { private get; set; }

        public bool IsWindowsAuthenticationEnabled
        {
            get
            {
                return _isWindowsAuthenticationEnabled;
            }
            set
            {
                SetProperty(ref _isWindowsAuthenticationEnabled, value);
            }
        }

        public bool IsConnectionSuccessful
        {
            get
            {
                return _isConnectionSuccessful;
            } 
            set
            {
                SetProperty(ref _isConnectionSuccessful, value);
            }
        }

        public ObservableCollection<SelectedTable> Tables { get; set; }

        private void RegisterCommands()
        {
            TestConnectionCommand = new DelegateCommand(TestConnectionCommandExecute);
            GenerateTablesCommand = new DelegateCommand(GenerateTablesCommandExecute);
            DeployCatalogCommand = new DelegateCommand(DeployCatalogCommandExecute);
        }

        /// <summary>
        /// Check the connection string is correct.
        /// </summary>
        private async void TestConnectionCommandExecute()
        {
            Tables.Clear();
            var connectionString = CreateConnectionString();
            var connectionStringValid = await IsConnectionStringValid(connectionString);
            if (!connectionStringValid) {
                MessageBox.Show("Cannot establish the connection");
                return;
            }

            MessageBox.Show("Connection successful");
            IsConnectionSuccessful = connectionStringValid;
            var tables = await GetTables(connectionString);
            foreach(var table in  tables)
            {
                Tables.Add(table);
            }
        }

        private void GenerateTablesCommandExecute()
        {
            if (ImportTablesEvent != null)
            {
                var connectionString = CreateConnectionString();
                var selectedTables = Tables.Where(t => t.IsSelected).Select(t => t.TableDefinition).ToList();
                var argument = new ImportTablesEventArgs(selectedTables, connectionString);
                ImportTablesEvent(this, argument);
            }
        }

        /// <summary>
        /// This callback is called when the catalog combobox is deployed.
        /// </summary>
        private void DeployCatalogCommandExecute()
        {
            if (_databasesAreRetrieved)
            {
                return;
            }

            var context = TaskScheduler.FromCurrentSynchronizationContext();
            Catalogs.Clear();
            Catalogs.Add("(loading databases ...)");
            try
            {
                var connectionString = CreateConnectionStringWithoutCatalog();
                GetDatabases(connectionString).ContinueWith(taskResult =>
                {
                    var databases = taskResult.Result;
                    Catalogs.Clear();
                    databases.ForEach(s => Catalogs.Add(s));
                    _databasesAreRetrieved = true;
                }, context);
            }
            catch (Exception)
            {
                Trace.WriteLine("Cannot retrieve the databases");
            }
        }

        /// <summary>
        /// Create and returns the connection string.
        /// </summary>
        /// <returns></returns>
        private string CreateConnectionString()
        {
            var connectionString = new StringBuilder(CreateConnectionStringWithoutCatalog());

            if (!string.IsNullOrWhiteSpace(Catalog))
            {
                connectionString.Append(string.Format("Initial Catalog={0};", Catalog));
            }

            return connectionString.ToString();
        }

        /// <summary>
        /// Create and returns the connection string without catalog name.
        /// </summary>
        /// <returns></returns>
        private string CreateConnectionStringWithoutCatalog()
        {
            var connectionString = new StringBuilder(string.Format("Data Source={0};", DataSource));
            if (IsWindowsAuthenticationEnabled)
            {
                connectionString.Append("Integrated Security=True;");
            }
            else
            {
                connectionString.Append(string.Format("User Id={0};Password={1};", UserName, Password));
            }

            return connectionString.ToString();
        }

        private async Task<bool> IsConnectionStringValid(string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<List<SelectedTable>> GetTables(string connectionString)
        {
            var result = new List<SelectedTable>();
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT TABLE_SCHEMA,TABLE_NAME FROM information_schema.tables", connection);
                var reader = await command.ExecuteReaderAsync();
                while(reader.Read())
                {
                    var record = new SelectedTable
                    {
                        IsSelected = true
                    };

                    var tableDefinition = new TableDefinition
                    {
                        TableSchema = reader.GetString(0),
                        TableName = reader.GetString(1),
                        ColumnDefinitions = new List<ColumnDefinition>()
                    };

                    await GetColumnDefinitions(tableDefinition.TableName, connectionString, tableDefinition.ColumnDefinitions);

                    record.TableDefinition = tableDefinition;
                    result.Add(record);
                }


                reader.Close();
            }

            return result;
        }

        /// <summary>
        /// Returns the databases name.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private async Task<List<string>> GetDatabases(string connectionString)
        {
            var result = new List<string>();
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var command = new SqlCommand("SELECT name FROM sys.databases", connection);
                var reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    var databaseName = reader.GetString(0);
                    result.Add(databaseName);
                }

                reader.Close();
            }

            return result;
        }

        private async Task GetColumnDefinitions(string tableName, string connectionString, List<ColumnDefinition> columnDefinitions)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                var tableStructureCommand = new SqlCommand(string.Format("SELECT COLUMN_NAME,DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}'", tableName), connection);
                var tableStructureReader = await tableStructureCommand.ExecuteReaderAsync();
                while (tableStructureReader.Read())
                {
                    var columnDefinition = new ColumnDefinition
                    {
                        ColumnName = tableStructureReader.GetString(0),
                        ColumnType = tableStructureReader.GetString(1)
                    };

                    columnDefinitions.Add(columnDefinition);
                }

                tableStructureReader.Close();
                connection.Close();
            }
        }
    }
}
