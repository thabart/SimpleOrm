//------------------------------------------------------------------------------
// <copyright file="SimpleOrmMappingWindow.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

using Microsoft.VisualStudio.Shell;

namespace Company.OrmLanguage
{

    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("00954eb4-c9c9-42e4-a81a-cb09b617534a")]
    public class SimpleOrmMappingWindow : ToolWindowPane
    {
        private readonly SimpleOrmMappingWindowControl _simpleOrmMappingWindowControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleOrmMappingWindow"/> class.
        /// </summary>
        public SimpleOrmMappingWindow() : base(null)
        {
            _simpleOrmMappingWindowControl = new SimpleOrmMappingWindowControl();

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            Content = _simpleOrmMappingWindowControl;
        }

        public EntityElement EntityElement
        {
            set
            {
                Caption = string.Format("Mapping rules : {0}", value.Name);
                _simpleOrmMappingWindowControl.EntityElement = value;
            }
        }
    }
}
