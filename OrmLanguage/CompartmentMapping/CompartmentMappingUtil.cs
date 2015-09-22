/*  (c) 2008 Benjamin Schroeter
 *  This file is part of JaDAL - Just another DSL-Tools Addon Library
 *  and licensed under the New BSD License
 *  For more information please visit http://www.codeplex.com/JaDAL */
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace CompartmentMapping
{
    /// <summary>
    /// Some static utility methods to use with compartment mappings
    /// </summary>
    public static class CompartmentMappingUtil
    {
        #region Rerouting
        /// <summary>
        /// Reroutes all compartment connection link shape of a given diagram.
        /// </summary>
        /// <remarks>
        /// The start and endpoints of the link will recalculated.
        /// </remarks>
        /// <param name="diagram">The Diagram</param>
        public static void RerouteCompartmentMappings(Diagram diagram)
        {
            // fill the cache
            FindAllCompartmentMappingRouter(diagram.GetType().Assembly);

            using (Transaction t = diagram.Store.TransactionManager.BeginTransaction("reroute all compartment links on diagram"))
            {
                // look for all links
                DiagramItem x = diagram.FindFirstChild(false);
                while (x != null)
                {
                    if (x.Shape is BinaryLinkShape)
                        DoRerouteCompartmentMappings((BinaryLinkShape)x.Shape);

                    x = diagram.FindNextChild(x, false);
                }
                t.Commit();
            }
        }

        /// <summary>
        /// Reroutes all compartment connection link shape connected to a given NodeShape.
        /// </summary>
        /// <remarks>
        /// The start and endpoints of the link will recalculated.
        /// </remarks>
        /// <param name="shape">The NodeShape</param>
        public static void RerouteCompartmentMappings(NodeShape shape)
        {
            // fill the cache
            FindAllCompartmentMappingRouter(shape.GetType().Assembly);

            using (Transaction t = shape.Store.TransactionManager.BeginTransaction("reroute all compartment links of a shape"))
            {
                foreach (LinkShape linkShape in shape.Link)
                    DoRerouteCompartmentMappings(linkShape as BinaryLinkShape);

                t.Commit();
            }
        }

        /// <summary>
        /// Reroutes a compartment connection link shape.
        /// </summary>
        /// <remarks>
        /// The start and endpoints of the link will recalculated.
        /// </remarks>
        /// <param name="link">The BinaryLinkShape</param>
        public static void RerouteCompartmentMappings(BinaryLinkShape link)
        {
            // fill the cache
            FindAllCompartmentMappingRouter(link.GetType().Assembly);

            using (Transaction t = link.Store.TransactionManager.BeginTransaction("reroute compartment links"))
            {
                DoRerouteCompartmentMappings(link);
                t.Commit();
            }
        }

        /// <summary>
        /// Reroutes a compartment connection link shape.
        /// </summary>
        /// <remarks>
        /// To call this method you have to fill the cache and start a transaction!
        /// </remarks>
        /// <param name="link">The BinaryLinkShape</param>
        private static void DoRerouteCompartmentMappings(BinaryLinkShape link)
        {
            if (link == null || link.ModelElement == null)
                return;

            Type connectionType = link.ModelElement.GetType();
            //do we have a ICompartmentMappingRouter instance for this type of connection?
            if (allCompartmentMappingRouter.ContainsKey(connectionType))
                allCompartmentMappingRouter[connectionType].CorrectBinaryLinkShapeEndPoints(link);
        }

        private static Dictionary<Type, ICompartmentMappingRouter> allCompartmentMappingRouter = null;
        /// <summary>
        /// Search for all classes based on CompartmentMappingRouterBase
        /// </summary>
        /// <remarks>
        /// Only direct subclasses of CompartmentMappingRouterBase will be found,
        /// but since the code generator creates this classes it is not a problem.
        /// The generated code must be live in the Dsl Porject or the rules may be not found.
        /// </remarks>
        /// <param name="assembly">The assembly to search for the Rules</param>
        private static void FindAllCompartmentMappingRouter(Assembly assembly)
        {
            if (allCompartmentMappingRouter != null)
                return;

            var routerTypes = from t in assembly.GetTypes()
                              where !t.IsAbstract
                                      && t.BaseType.Name == typeof(CompartmentMappingRouterBase<,,,,>).Name
                              select t;

            allCompartmentMappingRouter = new Dictionary<Type, ICompartmentMappingRouter>();
            foreach (Type t in routerTypes)
            {
                ICompartmentMappingRouter router = t.GetConstructor(new Type[] { }).Invoke(null) as ICompartmentMappingRouter;
                Type linkType = t.BaseType.GetGenericArguments()[2];
                if (router != null)
                    allCompartmentMappingRouter.Add(linkType, router);
            }
        }
        #endregion

        #region RemoveRerouteCommand
        public static IList<MenuCommand> RemoveRerouteCommand(IList<MenuCommand> baseList)
        {
            MenuCommand rerouteCommand = baseList.First(c => c.CommandID == Microsoft.VisualStudio.Modeling.Shell.CommonModelingCommands.RerouteLine);
            if (rerouteCommand != null)
                baseList.Remove(rerouteCommand);

            return baseList;
        }
        #endregion

        #region AllCompartmentMappingRules()
        /// <summary>
        /// Gets all rules defined by the compartment mappings to register them by the dsl framework.
        /// </summary>
        /// <remarks>
        /// You should override the GetCustomDomainModelTypes() method in a partial class of your DomainModel.
        /// </remarks>
        /// <example>
        /// partial class CompMapTestDslDomainModel
        /// {
        ///     protected override Type[] GetCustomDomainModelTypes()
        ///     {
        ///         return CompartmentMappingUtil.AllCompartmentMappingRules(this);
        ///     }
        /// }
        /// </example>
        /// <param name="model">The model to get the rules for.</param>
        /// <returns>The list of the types of all rules</returns>
        public static Type[] AllCompartmentMappingRules(DomainModel model)
        {
            // fill the cache
            Assembly assembly = model.GetType().Assembly;
            return FindAllCompartmentRules(assembly);
        }

        /// <summary>
        /// Gets all rules defined by the compartment mappings to register them by the dsl framework.
        /// </summary>
        /// <remarks>
        /// You should override the GetCustomDomainModelTypes() method in a partial class of your DomainModel.
        /// </remarks>
        /// <example>
        /// partial class CompMapTestDslDomainModel
        /// {
        ///     protected override Type[] GetCustomDomainModelTypes()
        ///     {
        ///         return CompartmentMappingUtil.AllCompartmentMappingRules(this, new[] {typeof(someOtherRule)});
        ///     }
        /// }
        /// </example>
        /// <param name="model">The model to get the rules for.</param>
        /// <param name="baseElementsForList">Some other rules to add to the result list.</param>
        /// <returns>The list of the types of all rules</returns>
        public static Type[] AllCompartmentMappingRules(DomainModel model, Type[] baseElementsForList)
        {
            // fill the cache
            Assembly assembly = model.GetType().Assembly;
            Type[] ruleTypes = FindAllCompartmentRules(assembly);

            // create a new (empty) array of needed length
            Type[] t = new Type[baseElementsForList.Length + ruleTypes.Count()];

            // copy the baseElementsForList to the array
            Array.Copy(baseElementsForList, t, baseElementsForList.Length);

            // copy the rule types
            Array.Copy(ruleTypes, 0, t, baseElementsForList.Length, ruleTypes.Length);

            return t;
        }

        /// <summary>
        /// Search for all Rules based on CompartmentMappingAddRuleBase or CompartmentEntryDeletingRuleBase
        /// </summary>
        /// <remarks>
        /// Only direct subclasses of CompartmentEntryDeletingRuleBase will be found,
        /// but since the code generator creates this classes it is not a problem.
        /// The generated code must be live in the Dsl Porject or the rules may be not found.</remarks>
        /// <param name="assembly">The assembly to search for the Rules</param>
        private static Type[] FindAllCompartmentRules(Assembly assembly)
        {
            // look for all subclasses of CompartmentMappingAddRuleBase in this assembly
            var rules = from t in assembly.GetTypes()
                        where !t.IsAbstract
                                && (t.BaseType == typeof(CompartmentMappingAddRuleBase)
                                     || t.BaseType.Name == typeof(CompartmentEntryDeletingRuleBase<,,,,>).Name)

                        select t;

            return rules.ToArray();  // TODO: to array
        }
        #endregion
    }
}