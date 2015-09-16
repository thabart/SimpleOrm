using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ORM.DisplayGraph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private Point _anchorPoint;

        private Point _currentPoint;

        private double _xCoordinate;

        private double _yCoordinate;

        private bool _isInDrag;

        public MainWindow()
        {
            InitializeComponent();

            _isInDrag = false;
            _xCoordinate = 0;
            _yCoordinate = 0;

            DataContext = this;

            RenderTransform = new TranslateTransform(0, 0);

            Loaded += OnLoaded;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public double XCoordinate
        {
            get { return _xCoordinate; }
            set
            {
                _xCoordinate = value;
                RaisePropertyChange("XCoordinate");
            }
        }

        public double YCoordinate
        {
            get { return _yCoordinate; }
            set
            {
                _yCoordinate = value;
                RaisePropertyChange("YCoordinate");
            }
        }
        
        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
        }

        private void UIElement_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element == null)
            {
                return;
            }

            _anchorPoint = e.GetPosition(Canvas);
            element.CaptureMouse();
            _isInDrag = true;
            e.Handled = true;
        }

        private void UIElement_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!_isInDrag)
            {
                return;
            }

            _currentPoint = e.GetPosition(Canvas);
            XCoordinate += _currentPoint.X - _anchorPoint.X;
            YCoordinate += _currentPoint.Y - _anchorPoint.Y;

            _anchorPoint = _currentPoint;
        }

        private void UIElement_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!_isInDrag)
            {
                return;
            }

            var element = sender as FrameworkElement;
            if (element != null)
            {
                element.ReleaseMouseCapture();
            }

            _isInDrag = false;
            e.Handled = true;
        }
        
        private void RaisePropertyChange(string prop)
        {
           if( PropertyChanged != null )
           {
              PropertyChanged(this, new PropertyChangedEventArgs(prop));
           }
        }
    }
}
