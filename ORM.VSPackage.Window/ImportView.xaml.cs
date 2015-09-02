using ORM.VSPackage.ImportWindowSqlServer.ViewModels;

namespace ORM.VSPackage.ImportWindowSqlServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ImportView
    {
        public ImportView()
        {
            InitializeComponent();
            Loaded += ImportView_Loaded;
        }

        private void ImportView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            DataContext = new ImportViewModel();
        }
    }
}
