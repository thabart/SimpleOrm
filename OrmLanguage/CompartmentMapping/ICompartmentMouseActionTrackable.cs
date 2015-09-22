/*  (c) 2008 Benjamin Schroeter
 *  This file is part of JaDAL - Just another DSL-Tools Addon Library
 *  and licensed under the New BSD License
 *  For more information please visit http://www.codeplex.com/JaDAL */
namespace CompartmentMapping
{
    /// <summary>
    /// This interface will be added to CompartmentShapes used with the compartment mapping.
    /// The code generater will create the code therefor, so dont worry
    /// </summary>
    public interface ICompartmentMouseActionTrackable
    {
        /// <summary>
        /// Gets the <see cref="CompartmentMouseTrack"/> element for this shape.
        /// </summary>
        CompartmentMouseTrack MouseTrack { get; }
    }
}