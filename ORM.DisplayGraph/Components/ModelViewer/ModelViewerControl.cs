﻿using System;
using System.Windows.Forms;
using System.Windows.Media;
using ORM.DisplayGraph.Components.Entity;
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

        #endregion

        #region Constructor

        static ModelViewerControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModelViewerControl), new FrameworkPropertyMetadata(typeof(ModelViewerControl)));
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

        public ObservableCollection<Link> Links
        {
            get
            {
                return (ObservableCollection<Link>)GetValue(LinksProperty);
            } set
            {
                SetValue(LinksProperty, value);
            }
        }

        #endregion

        #region Dependency properties

        public static readonly DependencyProperty TableDefinitionsProperty = DependencyProperty.Register(
            "TableDefinitions",
            typeof(ObservableCollection<TableDefinition>),
            typeof(ModelViewerControl),
            new FrameworkPropertyMetadata(null, OnTableDefinitionsChanged));

        public static readonly DependencyProperty LinksProperty = DependencyProperty.Register(
            "Links",
            typeof(ObservableCollection<Link>),
            typeof(ModelViewerControl),
            new FrameworkPropertyMetadata(null, OnTableDefinitionsChanged));

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
                        (obj, args) =>
                            DragAndDropTableDefinitionControl.InitializeDragAndDrop(control, _modelViewerContainer);
                    _modelViewerContainer.Children.Add(control);
                    Canvas.SetLeft(control, 0);
                    Canvas.SetTop(control, 0);
                }
            }
        }

        private void OnModelViewerContainerSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var oldfit = e.PreviousSize;

        }

        private double GetCurrentXOffset()
        {
            return ((MatrixTransform)_modelViewerContainer.RenderTransform).Matrix.OffsetX;
        }


        private double GetCurrentYOffset()
        {
            return ((MatrixTransform)_modelViewerContainer.RenderTransform).Matrix.OffsetY;
        }

        private double GetCurrentScale()
        {
            return ((MatrixTransform)_modelViewerContainer.RenderTransform).Matrix.M11;
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

        private static void OnLinksChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {

        }

        private static TableDefinitionControl CreateTableDefinitionControl(TableDefinition tableDefinition)
        {
            var control = new TableDefinitionControl();
            control.EntityName = tableDefinition.TableName;
            return control;
        }

        #endregion
    }
}