//------------------------------------------------------------------------------
// <copyright file="SimpleOrmMappingWindowControl.xaml.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Windows;
using Company.OrmLanguage.Window;

namespace Company.OrmLanguage
{

    /// <summary>
    /// Interaction logic for SimpleOrmMappingWindowControl.
    /// </summary>
    public partial class SimpleOrmMappingWindowControl
    {
        private readonly SimpleOrmMappingWindowViewModel _simpleOrmMappingWindowViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleOrmMappingWindowControl"/> class.
        /// </summary>
        public SimpleOrmMappingWindowControl()
        {
            _simpleOrmMappingWindowViewModel = new SimpleOrmMappingWindowViewModel();
            InitializeComponent();
            Loaded += OnLoaded;
        }

        public EntityElement EntityElement
        {
            set { _simpleOrmMappingWindowViewModel.Update(value); }
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            DataContext = _simpleOrmMappingWindowViewModel;
        }
    }
}