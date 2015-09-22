/*  (c) 2008 Benjamin Schroeter
 *  This file is part of JaDAL - Just another DSL-Tools Addon Library
 *  and licensed under the New BSD License
 *  For more information please visit http://www.codeplex.com/JaDAL */
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;

namespace CompartmentMapping
{
    /// <summary>
    /// Base class for all compartment connection aeleting rules.
    /// 
    /// If a compartment entry is deleting this rule deletes corresponding compartment connection, too.
    /// </summary>
    /// <remarks>
    /// You need not to use this class directly (e.g. create implementors). 
    /// The code generator will create one derived class for your compartment connection
    /// </remarks>
    /// <typeparam name="SOURCE_ELEMENT">DomainClass mapped to compartment shape of source</typeparam>
    /// <typeparam name="SOURCE_COMPARTMENT_ENTRY">DomainClass mapped to element inside of compartment shape of source</typeparam>
    /// <typeparam name="CONNECTION">DomainRelationship used for this compartment connection</typeparam>
    /// <typeparam name="TARGET_ELEMENT">DomainClass mapped to compartment shape of target</typeparam>
    /// <typeparam name="TARGET_COMPARTMENT_ENTRY">DomainClass mapped to element inside of compartment shape of target</typeparam>
    public abstract class CompartmentEntryDeletingRuleBase<SOURCE_ELEMENT, SOURCE_COMPARTMENT_ENTRY, CONNECTION, TARGET_ELEMENT, TARGET_COMPARTMENT_ENTRY> : DeletingRule
        where SOURCE_ELEMENT : ModelElement
        where SOURCE_COMPARTMENT_ENTRY : class
        where CONNECTION : ElementLink
        where TARGET_ELEMENT : ModelElement
        where TARGET_COMPARTMENT_ENTRY : class
    {
        /// <summary>
        /// This method is called when the rule is fired, that is when a compartment entry is deleting from the model.
        /// </summary>
        /// <param name="e">the ElementDeletingEventArgs</param>
        public override void ElementDeleting(ElementDeletingEventArgs e)
        {
            // it is not easy to find the compartment parent of a compartment entry
            if (e.ModelElement != null)
            {
                foreach (ElementLink link in DomainRoleInfo.GetAllElementLinks(e.ModelElement))
                {
                    // I have to find the ElementLink where the compartment entry is the target
                    if (DomainRoleInfo.GetTargetRolePlayer(link) == e.ModelElement)
                    {
                        // and check if this ElementLink is annotated with the DomainRelationshipAttribute
                        object[] attrs = link.GetType().GetCustomAttributes(typeof(DomainRelationshipAttribute), true);
                        if (attrs.Length > 0)
                        {
                            DomainRelationshipAttribute attr = attrs[0] as DomainRelationshipAttribute;
                            if (attr != null)
                            {
                                // and only if the DomainRelationshipAttribute is set to IsEmbedding == true
                                if (attr.IsEmbedding)  // follow only embedded links - there is only one embedded link with a concret class (the child) as target
                                {
                                    // finally we found the parent of the compartment entry
                                    ModelElement parent = DomainRoleInfo.GetSourceRolePlayer(link);

                                    // check all links from parent if anyone uses this child as source or target
                                    DeleteReferencesFromParentToChild(parent, e.ModelElement);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Looks for compartment connections of Type CONNECTION on the compartment parent that have the 
        /// compartment element as source or target and deletes this compartment connection from the model.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="element"></param>
        private void DeleteReferencesFromParentToChild(ModelElement parent, ModelElement element)
        {
            // i don't want to change my list inside the foreach loop
            List<ElementLink> toDelete = new List<ElementLink>();

            foreach (ElementLink link in DomainRoleInfo.GetAllElementLinks(parent))
                if (link is CONNECTION)
                {
                    if ((DomainRoleInfo.GetSourceRolePlayer(link) == parent
                             && element is SOURCE_COMPARTMENT_ENTRY
                             && GetBuilderInstance().IsEntryConnectionSource(element as SOURCE_COMPARTMENT_ENTRY, (CONNECTION)link))
                        || (DomainRoleInfo.GetTargetRolePlayer(link) == parent
                             && element is TARGET_COMPARTMENT_ENTRY
                             && GetBuilderInstance().IsEntryConnectionTarget(element as TARGET_COMPARTMENT_ENTRY, (CONNECTION)link)))
                        toDelete.Add(link);
                }

            // now do the deleting
            foreach (ElementLink link in toDelete)
                link.Delete();
        }

        /// <summary>
        /// This method is used to access the IsEntryConnectionSource() and IsEntryConnectionTarget() methods 
        /// via the CompartmentMappingBuilderBase instance generated by the code generator.
        /// </summary>
        /// <remarks>This method is implemented by the code generator.</remarks>
        /// <returns>a CompartmentMappingBuilderBase for this connection</returns>
        protected abstract CompartmentMappingBuilderBase<SOURCE_ELEMENT, SOURCE_COMPARTMENT_ENTRY, CONNECTION, TARGET_ELEMENT, TARGET_COMPARTMENT_ENTRY> GetBuilderInstance();
    }
}
