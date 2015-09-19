using ORM.DisplayGraph.ViewModels;

namespace ORM.DisplayGraph
{
    /// <summary>
    /// Interaction logic for ModelViewer.xaml
    /// </summary>
    public partial class TestModelViewer
    {
        public TestModelViewer()
        {
            InitializeComponent();
            DataContext = new TestModelViewerViewModel();
        }
    }
}
