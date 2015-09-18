using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using ORM.DisplayGraph.Components.Entity;

namespace ORM.DisplayGraph.Components.ModelViewer
{
    public class DragAndDropTableDefinitionControl
    {
        private readonly TableDefinitionControl _tableDefinitionControl;

        private readonly Canvas _modelViewerContainer;

        private bool _isInDrag;

        private Point _anchorPoint;

        private Point _currentPoint;

        private double _originalLeft;

        private double _originalTop;

        private DragAndDropTableDefinitionControl(
            TableDefinitionControl tableDefinitionControl,
            Canvas modelViewerContainer)
        {
            _isInDrag = false;
            _tableDefinitionControl = tableDefinitionControl;
            _modelViewerContainer = modelViewerContainer;
            var tableDefinitionHeader = tableDefinitionControl.Template.FindName("PART_TableDefinitionHeader", tableDefinitionControl) as Grid;
            
            tableDefinitionHeader.MouseLeftButtonDown += TableDefinitionControlOnMouseLeftButtonDown;
            tableDefinitionHeader.MouseMove += TableDefinitionControlOnMouseMove;
            tableDefinitionHeader.MouseLeftButtonUp += TableDefinitionControlOnMouseLeftButtonUp;
        }

        public static void InitializeDragAndDrop(
            TableDefinitionControl tableDefinitionControl,
            Canvas modelViewerContainer)
        {
            new DragAndDropTableDefinitionControl(tableDefinitionControl, modelViewerContainer);
        }

        private void TableDefinitionControlOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element == null)
            {
                return;
            }

            _anchorPoint = e.GetPosition(_modelViewerContainer);
            _originalLeft = Canvas.GetLeft(_tableDefinitionControl);
            _originalTop = Canvas.GetTop(_tableDefinitionControl);
            element.CaptureMouse();
            _isInDrag = true;
            e.Handled = true;
        }

        private void TableDefinitionControlOnMouseMove(object sender, MouseEventArgs e)
        {
            if (!_isInDrag)
            {
                return;
            }

            _currentPoint = e.GetPosition(_modelViewerContainer);

            var xCoordinate = _originalLeft + _currentPoint.X - _anchorPoint.X;
            var yCoordinate = _originalTop + _currentPoint.Y - _anchorPoint.Y;
            Canvas.SetLeft(_tableDefinitionControl, xCoordinate);
            Canvas.SetTop(_tableDefinitionControl, yCoordinate);
        }

        private void TableDefinitionControlOnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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
    }
}
