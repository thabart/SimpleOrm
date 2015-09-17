using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using VM = ORM.DisplayGraph.Components.Entity.ViewModel;

namespace ORM.DisplayGraph.Components.Entity
{
    [TemplateVisualState(Name = "IsExpand", GroupName = "Expand")]
    [TemplateVisualState(Name = "IsNotExpand", GroupName = "Expand")]
    public class TableDefinitionControl : ContentControl
    {
        #region Fields

        private Button _expandButtonTableDefinition;

        private ListBox _columnDefinitionsListBox;

        #endregion

        #region Constructor

        static TableDefinitionControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TableDefinitionControl), new FrameworkPropertyMetadata(typeof(TableDefinitionControl)));
        }

        #endregion

        #region Properties

        public string EntityName
        {
            get
            {
                return (string)GetValue(EntityNameProperty);
            }
            set
            {
                SetValue(EntityNameProperty, value);
            }
        }
        
        public List<VM.PropertyDefinition> Properties
        {
            get
            {
                return (List<VM.PropertyDefinition>)GetValue(PropertiesProperty);
            } set
            {
                SetValue(PropertiesProperty, value);
            }
        }

        public bool IsExpand
        {
            get
            {
                return (bool)GetValue(IsExpandProperty);
            } set
            {
                SetValue(IsExpandProperty, value);
            }
        }

        #endregion

        #region Dependency properties

        public static readonly DependencyProperty EntityNameProperty = DependencyProperty.Register(
            "EntityName",
            typeof(string),
            typeof(TableDefinitionControl),
            new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty IsExpandProperty = DependencyProperty.Register(
            "IsExpand",
            typeof(bool),
            typeof(TableDefinitionControl),
            new PropertyMetadata(true, IsExpandChanged));

        public static readonly DependencyProperty PropertiesProperty = DependencyProperty.Register(
            "Properties",
            typeof(List<VM.PropertyDefinition>),
            typeof(TableDefinitionControl),
            new PropertyMetadata(new List<VM.PropertyDefinition>()));

        #endregion

        #region Private methods

        private void RegisterEvents()
        {
            _expandButtonTableDefinition.Click += ExpandButtonOnClick;
        }

        private void ExpandButtonOnClick(object sender, RoutedEventArgs e)
        {
            IsExpand = !IsExpand;
        }

        private void Initialize()
        {
            IsExpand = true;
            UpdateExpandProperty(true);
        }

        private void UpdateExpandProperty(bool isExpand)
        {
            if (isExpand)
            {
                VisualStateManager.GoToState(_expandButtonTableDefinition, "IsExpand", false);
                VisualStateManager.GoToState(this, "IsExpand", false);
            }
            else
            {
                VisualStateManager.GoToState(_expandButtonTableDefinition, "IsNotExpand", false);
                VisualStateManager.GoToState(this, "IsNotExpand", false);
            }
        }

        #endregion

        #region private static methods

        private static void IsExpandChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var sender = (TableDefinitionControl)obj;
            var isExpand = (bool)args.NewValue;
            sender.UpdateExpandProperty(isExpand);
        }

        #endregion

        #region Public methods

        public override void OnApplyTemplate()
        {
            _expandButtonTableDefinition = Template.FindName("PART_ExpandButton", this) as Button;
            _columnDefinitionsListBox = Template.FindName("PART_ColumnDefinitionsListBox", this) as ListBox;

            RegisterEvents();
            Initialize();

            base.OnApplyTemplate();
        }

        #endregion
    }
}
