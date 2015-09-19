using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using ORM.DisplayGraph.Components.Entity;
using ORM.DisplayGraph.Components.ModelViewer.ViewModels;

namespace ORM.DisplayGraph.Components.ModelViewer
{
    public class DragAndDropTableDefinitionControl
    {
        private readonly TableDefinitionControl _tableDefinitionControl;

        private readonly Dictionary<TableDefinition, TableDefinitionControl> _tableDefinitionControls;

        private readonly Dictionary<LinkDefinition, Line> _linkDefinitionControls; 

        private readonly ObservableCollection<LinkDefinition> _linkDefinitions;

        private readonly Canvas _modelViewerContainer;

        private bool _isInDrag;

        private Point _anchorPoint;

        private Point _currentPoint;

        private double _originalLeft;

        private double _originalTop;

        private DragAndDropTableDefinitionControl(
            TableDefinitionControl tableDefinitionControl,
            Dictionary<TableDefinition, TableDefinitionControl> tableDefinitionControls,
            ObservableCollection<LinkDefinition> linkDefinitions,
            Canvas modelViewerContainer,
            Dictionary<LinkDefinition, Line> linkDefinitionControls)
        {
            _isInDrag = false;
            _tableDefinitionControls = tableDefinitionControls;
            _linkDefinitions = linkDefinitions;
            _tableDefinitionControl = tableDefinitionControl;
            _modelViewerContainer = modelViewerContainer;
            _linkDefinitionControls = linkDefinitionControls;
            var tableDefinitionHeader = tableDefinitionControl.Template.FindName("PART_TableDefinitionHeader", tableDefinitionControl) as Grid;
            
            tableDefinitionHeader.MouseLeftButtonDown += TableDefinitionControlOnMouseLeftButtonDown;
            tableDefinitionHeader.MouseMove += TableDefinitionControlOnMouseMove;
            tableDefinitionHeader.MouseLeftButtonUp += TableDefinitionControlOnMouseLeftButtonUp;
        }

        public static void InitializeDragAndDrop(
            TableDefinitionControl tableDefinitionControl,
            Dictionary<TableDefinition, TableDefinitionControl> tableDefinitionControls,
            ObservableCollection<LinkDefinition> linkDefinitions,
            Canvas modelViewerContainer,
            Dictionary<LinkDefinition, Line> linkDefinitionControls)
        {
            new DragAndDropTableDefinitionControl(
                tableDefinitionControl,
                tableDefinitionControls, 
                linkDefinitions, 
                modelViewerContainer,
                linkDefinitionControls);
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

            // Move the table definition control
            Canvas.SetLeft(_tableDefinitionControl, xCoordinate);
            Canvas.SetTop(_tableDefinitionControl, yCoordinate);

            // Move all the links.
            var keyPairValue = _tableDefinitionControls.FirstOrDefault(t => t.Value.Equals(_tableDefinitionControl));
            var tableDefinition = keyPairValue.Key;
            var links = GetAllLinkedLinks(tableDefinition);
            foreach (var link in links)
            {
                var line = _linkDefinitionControls[link];
                UpdateRelationShips.RefreshRelationShipPosition(
                    link,
                    _tableDefinitionControls,
                    _modelViewerContainer,
                    line);
            }
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


        private IEnumerable<LinkDefinition> GetAllLinkedLinks(TableDefinition tableDefinition)
        {
            return _linkDefinitions.Where(
                l => l.Source.TableDefinition == tableDefinition || l.Target.TableDefinition == tableDefinition).ToList();
        }
    }
}
