using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Shapes;
using ORM.DisplayGraph.Components.Entity;
using ORM.DisplayGraph.Components.Entity.ViewModel;
using ORM.DisplayGraph.Components.ModelViewer.ViewModels;

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace ORM.DisplayGraph.Components.ModelViewer
{
    public class ModelViewerControl : ContentControl
    {
        #region Fields

        private Canvas _modelViewerContainer;

        private readonly Dictionary<TableDefinition, TableDefinitionControl> _tableDefinitionControls;

        private readonly Dictionary<LinkDefinition, Line> _linkDefinitionControls; 

        #endregion

        #region Constructor

        static ModelViewerControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModelViewerControl), new FrameworkPropertyMetadata(typeof(ModelViewerControl)));
        }

        public ModelViewerControl()
        {
            _tableDefinitionControls = new Dictionary<TableDefinition, TableDefinitionControl>();
            _linkDefinitionControls = new Dictionary<LinkDefinition, Line>();
        }

        #endregion

        #region Properties

        public ObservableCollection<TableDefinition> TableDefinitions
        {
            get
            {
                return (ObservableCollection<TableDefinition>)GetValue(TableDefinitionsProperty);
            }
            set
            {
                SetValue(TableDefinitionsProperty, value);
            }
        }

        public ObservableCollection<LinkDefinition> LinkDefinitions
        {
            get
            {
                return (ObservableCollection<LinkDefinition>)GetValue(LinkDefinitionsProperty);
            } set
            {
                SetValue(LinkDefinitionsProperty, value);
            }
        }

        #endregion

        #region Dependency properties

        public static readonly DependencyProperty TableDefinitionsProperty = DependencyProperty.Register(
            "TableDefinitions",
            typeof(ObservableCollection<TableDefinition>),
            typeof(ModelViewerControl),
            new FrameworkPropertyMetadata(null, OnTableDefinitionsChanged));

        public static readonly DependencyProperty LinkDefinitionsProperty = DependencyProperty.Register(
            "LinkDefinitions",
            typeof(ObservableCollection<LinkDefinition>),
            typeof(ModelViewerControl),
            new FrameworkPropertyMetadata(null, OnLinkDefinitionsChanged));

        #endregion

        #region Public methods

        public override void OnApplyTemplate()
        {
            _modelViewerContainer = Template.FindName("PART_ModelViewerContainer", this) as Canvas;
            _modelViewerContainer.SizeChanged += OnModelViewerContainerSizeChanged;
            base.OnApplyTemplate();
        }

        #endregion

        #region Private methods

        private void OnTableDefinitionsChanged(object sender,
            NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var addedItems = e.NewItems;
                foreach (var addItem in addedItems)
                {
                    var tableDefinition = addItem as TableDefinition;
                    if (tableDefinition == null)
                    {
                        continue;
                    }

                    var control = CreateTableDefinitionControl(tableDefinition);
                    control.Loaded +=
                        (obj, args) => DragAndDropTableDefinitionControl.InitializeDragAndDrop(control, _tableDefinitionControls, LinkDefinitions, _modelViewerContainer, _linkDefinitionControls);
                    _modelViewerContainer.Children.Add(control);
                    Canvas.SetLeft(control, 0);
                    Canvas.SetTop(control, 0);

                    _tableDefinitionControls.Add(tableDefinition, control);
                }
            }
        }

        private void OnLinkDefinitionsChanged(object sender,
            NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var addedItems = e.NewItems;
                foreach (var addedItem in addedItems)
                {
                    var linkDefinition = addedItem as LinkDefinition;
                    if (linkDefinition == null)
                    {
                        continue;
                    }

                    var line = new Line
                    {
                        StrokeThickness = 2,
                        Stroke = Brushes.Black
                    };

                    _modelViewerContainer.Children.Add(line);

                    _linkDefinitionControls.Add(linkDefinition, line);
                    UpdateRelationShips.RefreshRelationShipPosition(
                        linkDefinition, 
                        _tableDefinitionControls,
                        _modelViewerContainer,
                        line);
                }
            }
        }

        private void OnModelViewerContainerSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var oldSize = e.PreviousSize;
            var newSize = e.NewSize;
            var diffWidth = newSize.Width - oldSize.Width;
            // Trace.WriteLine(diffWidth);
        }

        private Matrix GetMatrixTransform()
        {
            return ((MatrixTransform) _modelViewerContainer.RenderTransform).Matrix;
        }

        #endregion

        #region Private static methods

        private static void OnTableDefinitionsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldTableDefinitions = args.OldValue;
            var newTableDefinitions = args.NewValue;
            var modelViewer = (ModelViewerControl)obj;
            if (oldTableDefinitions != null)
            {
                var oldCastedTableDefinitions = (ObservableCollection<TableDefinition>)oldTableDefinitions;
                oldCastedTableDefinitions.CollectionChanged -= modelViewer.OnTableDefinitionsChanged;
            }

            if (newTableDefinitions != null)
            {
                var newCastedTableDefinitions = (ObservableCollection<TableDefinition>)newTableDefinitions;
                newCastedTableDefinitions.CollectionChanged += modelViewer.OnTableDefinitionsChanged;
            }
        }

        private static void OnLinkDefinitionsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldLinkDefinitions = args.OldValue;
            var newLinkDefinitions = args.NewValue;
            var modelViewer = (ModelViewerControl) obj;
            if (oldLinkDefinitions != null)
            {
                var oldCastedLinkDefinitions = (ObservableCollection<LinkDefinition>) oldLinkDefinitions;
                oldCastedLinkDefinitions.CollectionChanged -= modelViewer.OnLinkDefinitionsChanged;
            }

            if (newLinkDefinitions != null)
            {
                var newCastedLinkDefinitions = (ObservableCollection<LinkDefinition>) newLinkDefinitions;
                newCastedLinkDefinitions.CollectionChanged += modelViewer.OnLinkDefinitionsChanged;
            }
        }

        private static TableDefinitionControl CreateTableDefinitionControl(TableDefinition tableDefinition)
        {
            var control = new TableDefinitionControl
            {
                EntityName = tableDefinition.TableName
            };

            if (tableDefinition.ColumnDefinitions != null && tableDefinition.ColumnDefinitions.Any())
            {
                foreach (var columnDefinition in tableDefinition.ColumnDefinitions)
                {
                    var property = new PropertyDefinition
                    {
                        Name = columnDefinition.Name,
                        Type = columnDefinition.Type,
                        IsPrimaryKey = columnDefinition.IsPrimaryKey
                    };

                    control.Properties.Add(property);
                }
            }

            return control;
        }

        #endregion
    }
}
