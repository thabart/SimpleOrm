/*  (c) 2008 Benjamin Schroeter
 *  This file is part of JaDAL - Just another DSL-Tools Addon Library
 *  and licensed under the New BSD License
 *  For more information please visit http://www.codeplex.com/JaDAL */
using Microsoft.VisualStudio.Modeling;

namespace CompartmentMapping
{
    /// <summary>
    /// Different parts of a compartment shape.
    /// </summary>
    public enum SelectedCompartmentPartType
    {
        None = 0,
        Head = 1,
        Element = 2
    }

    /// <summary>
    /// This class represents a part of a compartment shape.
    /// </summary>
    /// <remarks>
    /// This part can be the head or an compartment element
    /// </remarks>
    public class SelectedCompartmentPart
    {
        public bool IsRegularShape = false;
        public SelectedCompartmentPartType Type = SelectedCompartmentPartType.None;
        public ModelElement Element;

        public SelectedCompartmentPart Clone()
        {
            return new SelectedCompartmentPart { Type = Type, Element = Element, IsRegularShape = IsRegularShape };
        }
    }
}