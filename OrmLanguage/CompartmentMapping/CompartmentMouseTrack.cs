/*  (c) 2008 Benjamin Schroeter
 *  This file is part of JaDAL - Just another DSL-Tools Addon Library
 *  and licensed under the New BSD License
 *  For more information please visit http://www.codeplex.com/JaDAL */
using System.Diagnostics;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace CompartmentMapping
{
    /// <summary>
    /// When associated with a CompartmentShape this class keps track of mouse movement above the shape.
    /// This is needed by the compartment mappings to determinate the entry below the mouse cursor.
    /// </summary>
    public class CompartmentMouseTrack
    {
        /// <summary>
        /// The part of the compartment shape currently below the mouse cursor
        /// </summary>
        public SelectedCompartmentPart entryNowHoveringAbove = new SelectedCompartmentPart();
        /// <summary>
        /// The part of the compartment shape that was below the mouse curse when the mouse button was pessed the last time.
        /// This is used to find the startpoint if a connection.
        /// </summary>
        public SelectedCompartmentPart entryBelowMouseDown = new SelectedCompartmentPart();

        private bool eventsRegisterd = false;

        public void ShapeInserted<T>(T shape) where T : CompartmentShape, ICompartmentMouseActionTrackable
        {
            Debug.Assert(!eventsRegisterd, "The events should only register once.");
            if (eventsRegisterd)
                return;

            // register events to the shape
            shape.MouseMove += Shape_MouseMove;
            shape.MouseDown += Shape_MouseDown;

            foreach (DiagramItem x in shape.Children())
                if (x.Shape is ElementListCompartment)
                {
                    // and events to the compartment lists
                    ElementListCompartment elc = (ElementListCompartment)x.Shape;
                    elc.MouseMove += ElementListCompartment_MouseMove;
                }

            eventsRegisterd = true;
        }

        /// <summary>
        /// Eventhandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Shape_MouseDown(object sender, DiagramMouseEventArgs e)
        {
            entryBelowMouseDown = entryNowHoveringAbove.Clone();
        }

        /// <summary>
        /// Eventhandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Shape_MouseMove(object sender, DiagramMouseEventArgs e)
        {
            // this event is only fired when moving over the shape itself. 
            // moving over compartment entries or compartment headers are handled by ElementListCompartment_MouseMove()
            entryNowHoveringAbove.Type = SelectedCompartmentPartType.Head;
            entryNowHoveringAbove.Element = null;
        }

        /// <summary>
        /// Eventhandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ElementListCompartment_MouseMove(object sender, DiagramMouseEventArgs e)
        {
            entryNowHoveringAbove.Type = SelectedCompartmentPartType.None;
            entryNowHoveringAbove.Element = null;

            foreach (object o in e.HitDiagramItem.RepresentedElements)
                if (o is ModelElement)
                {
                    entryNowHoveringAbove.Type = SelectedCompartmentPartType.Element;
                    entryNowHoveringAbove.Element = (ModelElement)o;
                    break;
                }
        }
    }
}