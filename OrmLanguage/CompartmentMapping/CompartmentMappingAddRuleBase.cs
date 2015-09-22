/*  (c) 2008 Benjamin Schroeter
 *  This file is part of JaDAL - Just another DSL-Tools Addon Library
 *  and licensed under the New BSD License
 *  For more information please visit http://www.codeplex.com/JaDAL */
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace CompartmentMapping
{
    /// <summary>
    /// Base class for all compartment connection add rules.
    /// 
    /// This rule changes the endpoints of e new connection to correspond to the entries of the compartment shapes.
    /// </summary>
    /// <remarks>
    /// You need not to use this class directly (e.g. create implementors). 
    /// The code generator will create one derived class for your compartment connection
    /// </remarks>
    public abstract class CompartmentMappingAddRuleBase : AddRule
    {
        /// <summary>
        /// This method is called when the rule is fired, that is when a new connection is added to the model.
        /// </summary>
        /// <param name="e">the ElementAddedEventArgs</param>
        public override void ElementAdded(ElementAddedEventArgs e)
        {
            BinaryLinkShape c = e.ModelElement as BinaryLinkShape;
            if (c == null)
                return;

            CompartmentMappingUtil.RerouteCompartmentMappings(c);
        }
    }
}
