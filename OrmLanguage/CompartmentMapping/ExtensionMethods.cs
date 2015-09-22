/*  (c) 2008 Benjamin Schroeter
 *  This file is part of JaDAL - Just another DSL-Tools Addon Library
 *  and licensed under the New BSD License
 *  For more information please visit http://www.codeplex.com/JaDAL */
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace CompartmentMapping
{
    /// <summary>
    /// Some extention methods
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Findes the first represented element of a DiagramItem
        /// </summary>
        /// <param name="item">a DiagramItem</param>
        /// <returns>the first represented element or null if there is no represented element</returns>
        public static object FirstRepresentedElement(this DiagramItem item)
        {
            foreach (object o in item.RepresentedElements)
                return o;

            return null;
        }

        /// <summary>
        /// Gets all children of a ShapeElement as a List
        /// </summary>
        /// <remarks>This is a wrapper for easy use of the FindFirstChild() and FindNextChild() methodes</remarks>
        /// <param name="shape">a ShapeElement</param>
        /// <returns>the children of the given ShapeElement</returns>
        public static IList<DiagramItem> Children(this ShapeElement shape)
        {
            List<DiagramItem> list = new List<DiagramItem>();
            DiagramItem x = shape.FindFirstChild(false);
            while (x != null)
            {
                list.Add(x);
                x = shape.FindNextChild(x, false);
            }

            return list;
        }
    }
}