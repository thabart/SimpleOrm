/*  (c) 2008 Benjamin Schroeter
 *  This file is part of JaDAL - Just another DSL-Tools Addon Library
 *  and licensed under the New BSD License
 *  For more information please visit http://www.codeplex.com/JaDAL */
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace CompartmentMapping
{
    /// <summary>
    /// Base class for all compartment connection builders
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
    public abstract class CompartmentMappingBuilderBase<SOURCE_ELEMENT, SOURCE_COMPARTMENT_ENTRY, CONNECTION, TARGET_ELEMENT, TARGET_COMPARTMENT_ENTRY>
        where SOURCE_ELEMENT : ModelElement
        where SOURCE_COMPARTMENT_ENTRY : class
        where CONNECTION : ElementLink
        where TARGET_ELEMENT : ModelElement
        where TARGET_COMPARTMENT_ENTRY : class
    {

        private SOURCE_ELEMENT rememberedSourceShape = null;
        private SelectedCompartmentPart rememberedSourceEntry = null;

        private TARGET_ELEMENT rememberedTargetShape = null;
        private SelectedCompartmentPart rememberedTargetEntry = null;

        private readonly bool allowHeadAsSource;
        private readonly bool allowHeadAsTarget;
        private readonly bool allowSelfReference;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="allowHeadAsSource">The head of the source CompartmentShape is a valid connection source.</param>
        /// <param name="allowHeadAsTarget">The head of the target CompartmentShape is a valid connection target.</param>
        /// <param name="allowSelfReference">A connection with source shape == target shape is allowed.</param>
        protected CompartmentMappingBuilderBase(bool allowHeadAsSource, bool allowHeadAsTarget, bool allowSelfReference)
        {
            this.allowHeadAsSource = allowHeadAsSource;
            this.allowHeadAsTarget = allowHeadAsTarget;
            this.allowSelfReference = allowSelfReference;
        }

        #region extensibility points
        /// <summary>
        /// Checks if the given connection has the entry as source
        /// </summary>
        /// <remarks>You have to implement this method, because only you can know how to store this information in your connection class.</remarks>
        /// <param name="entry">the entry if a compartment shape</param>
        /// <param name="connection">the connection</param>
        /// <returns>true, if the connection has the entry as source</returns>
        public abstract bool IsEntryConnectionSource(SOURCE_COMPARTMENT_ENTRY entry, CONNECTION connection);

        /// <summary>
        /// Checks if the given connection has the entry as target
        /// </summary>
        /// <remarks>You have to implement this method, because only you can know how to store this information in your connection class.</remarks>
        /// <param name="entry">the entry if a compartment shape</param>
        /// <param name="connection">the connection</param>
        /// <returns>true, if the connection has the entry as target</returns>
        public abstract bool IsEntryConnectionTarget(TARGET_COMPARTMENT_ENTRY entry, CONNECTION connection);

        #region CanAcceptAs..
        /// <summary>
        /// Test whether a given model element is acceptable to this ConnectionBuilder as the source of a connection.
        /// </summary>
        /// <param name="candidate">The model element to test.</param>
        /// <param name="partType">The type of the source. (head or entry)</param>
        /// <param name="candidateEntry">The model element to use as the source entry of the conneciton</param>
        /// <returns>Whether the element can be used as the source of a connection.</returns>
        protected virtual bool CanAcceptAsCompartmentSource(SOURCE_ELEMENT candidate, SelectedCompartmentPartType partType, SOURCE_COMPARTMENT_ENTRY candidateEntry)
        {
            return true;
        }

        /// <summary>
        /// Test whether a given model element is acceptable to this ConnectionBuilder as the target of a connection
        /// </summary>
        /// <param name="targetElement">The model element to use as the target of the connection</param>
        /// <param name="targetPartType">The type of the target. (head or entry)</param>
        /// <param name="targetEntry">The model elment to use as the target entry of the connection</param>
        /// <returns>Whether the elements can be used as the source and target of a connection</returns>
        protected virtual bool CanAcceptAsCompartmentTarget(TARGET_ELEMENT candidate, SelectedCompartmentPartType partType, TARGET_COMPARTMENT_ENTRY candidateEntry)
        {
            return true;
        }

        /// <summary>
        /// Test whether a given pair of model elements are acceptable to this ConnectionBuilder as the source and target of a connection
        /// </summary>
        /// <remarks>This method is only called if both, source and targent, are compartment shapes.</remarks>
        /// <param name="sourceElement">The model element to use as the source of the connection</param>
        /// <param name="sourcePartType">The type of the source. (head or entry)</param>
        /// <param name="sourceEntry">The model element to use as the source entry of the conneciton</param>
        /// <param name="targetElement">The model element to use as the target of the connection</param>
        /// <param name="targetPartType">The type of the target. (head or entry)</param>
        /// <param name="targetEntry">The model elment to use as the target entry of the connection</param>
        /// <returns>Whether the elements can be used as the source and target of a connection</returns>
        protected virtual bool CanAcceptAsCompartmentSourceAndTarget(SOURCE_ELEMENT sourceElement, SelectedCompartmentPartType sourcePartType, SOURCE_COMPARTMENT_ENTRY sourceEntry, TARGET_ELEMENT targetElement, SelectedCompartmentPartType targetPartType, TARGET_COMPARTMENT_ENTRY targetEntry)
        {
            return true;
        }
        #endregion

        #region CreateElementLink()

        /// <summary>
        /// Make a connection between the given pair of source and target elements
        /// </summary>
        /// <returns>A link representing the created connection</returns>        
        /// <remarks>This method is only called if both, source and targent, are compartment shapes.</remarks>
        /// <param name="source">The model element to use as the source of the connection</param>
        /// <param name="sourcePartType">The type of the source. (head or entry)</param>
        /// <param name="sourceEntry">The model element to use as the source entry of the conneciton</param>
        /// <param name="target">The model element to use as the target of the connection</param>
        /// <param name="targetPartType">The type of the target. (head or entry)</param>
        /// <param name="targetEntry">The model elment to use as the target entry of the connection</param>
        /// <returns>A link representing the created connection</returns>
        protected abstract ElementLink CreateElementLink(SOURCE_ELEMENT source,
                                                         SelectedCompartmentPartType sourcePartType,
                                                         SOURCE_COMPARTMENT_ENTRY sourceEntry, TARGET_ELEMENT target,
                                                         SelectedCompartmentPartType targetPartType,
                                                         TARGET_COMPARTMENT_ENTRY targetEntry);
        #endregion
        #endregion

        /// <summary>
        /// Test whether a given model element is acceptable to this ConnectionBuilder as the source of a connection.
        /// </summary>
        /// <param name="candidate">The model element to test.</param>
        /// <param name="forSourceAndTarget">false: this method is called to query only the source of a new connection, true: after this call the <see cref="CanAcceptAsSourceAndTarget"/> method will be called.</param>
        /// <returns>Whether the element can be used as the source of a connection.</returns>
        private bool CanAcceptAsSource(SOURCE_ELEMENT candidate, bool forSourceAndTarget)
        {
            ICompartmentMouseActionTrackable candidateShape = GetFirstCompartmentShapeForModelElement(candidate);

            if (candidateShape == null) // must be a regular shap
            {
                rememberedSourceShape = candidate;
                rememberedSourceEntry = new SelectedCompartmentPart() { IsRegularShape = true };
                return CanAcceptAsCompartmentSource(rememberedSourceShape, SelectedCompartmentPartType.None, null);
            }
            else
            {
                rememberedSourceShape = candidate;
                SelectedCompartmentPart entry =
                    (forSourceAndTarget
                         ? candidateShape.MouseTrack.entryBelowMouseDown
                         : candidateShape.MouseTrack.entryNowHoveringAbove).Clone();

                // the Compartment Title is never valid as a source
                if (entry.Element is ElementListCompartment)
                    return false;

                if (entry.Type == SelectedCompartmentPartType.None
                    || (entry.Type == SelectedCompartmentPartType.Head && !allowHeadAsSource)
                    || (entry.Type == SelectedCompartmentPartType.Element && !(entry.Element is SOURCE_COMPARTMENT_ENTRY)))
                    return false;

                if (!forSourceAndTarget)
                {
                    rememberedSourceEntry = entry;
                    // everything fine for me. now ask the user to decide
                    return CanAcceptAsCompartmentSource(rememberedSourceShape, rememberedSourceEntry.Type,
                                                        rememberedSourceEntry.Element as SOURCE_COMPARTMENT_ENTRY);
                }

                return true;
            }
        }

        /// <summary>
        /// Test whether a given pair of model elements are acceptable to this ConnectionBuilder as the source and target of a connection
        /// </summary>
        /// <param name="candidateSource">The model element to test as a source</param>
        /// <param name="candidateTarget">The model element to test as a target</param>
        /// <returns>Whether the elements can be used as the source and target of a connection</returns>
        private bool CanAcceptAsSourceAndTarget(SOURCE_ELEMENT candidateSource, TARGET_ELEMENT candidateTarget)
        {
            if (!allowSelfReference && candidateSource == candidateTarget)
                return false;

            if (candidateSource != rememberedSourceShape)
                return false;

            ICompartmentMouseActionTrackable targetShape = GetFirstCompartmentShapeForModelElement(candidateTarget);

            if (targetShape == null) // it is a regular shape
            {
                rememberedTargetShape = candidateTarget;
                rememberedTargetEntry = new SelectedCompartmentPart() { IsRegularShape = true };

                return CanAcceptAsCompartmentTarget(rememberedTargetShape,
                                                    SelectedCompartmentPartType.None,
                                                    null)
                       && CanAcceptAsCompartmentSourceAndTarget(rememberedSourceShape,
                                                                       rememberedSourceEntry.Type,
                                                                       rememberedSourceEntry.Element as SOURCE_COMPARTMENT_ENTRY,
                                                                       rememberedTargetShape,
                                                                       SelectedCompartmentPartType.None,
                                                                       null);
            }
            else
            {
                rememberedTargetShape = candidateTarget;
                rememberedTargetEntry = targetShape.MouseTrack.entryNowHoveringAbove.Clone();

                // the Compartment Title is never valid as a target
                if (rememberedTargetEntry.Element is ElementListCompartment)
                    return false;

                if (rememberedTargetEntry.Type == SelectedCompartmentPartType.None
                    || (rememberedTargetEntry.Type == SelectedCompartmentPartType.Head && !allowHeadAsTarget)
                    || (rememberedTargetEntry.Type == SelectedCompartmentPartType.Element && !(rememberedTargetEntry.Element is TARGET_COMPARTMENT_ENTRY)))
                    return false;

                // check some standart cases for source is compartment shape
                if (!rememberedSourceEntry.IsRegularShape)
                {
                    if (rememberedSourceEntry.Type == SelectedCompartmentPartType.None
                        || (rememberedSourceEntry.Type == SelectedCompartmentPartType.Head && !allowHeadAsSource)
                        || (rememberedSourceEntry.Type == SelectedCompartmentPartType.Element && !(rememberedSourceEntry.Element is SOURCE_COMPARTMENT_ENTRY)))
                        return false;
                }

                // everything fine for me. now ask the user to decide
                if (!CanAcceptAsCompartmentTarget(rememberedTargetShape, rememberedTargetEntry.Type, rememberedTargetEntry.Element as TARGET_COMPARTMENT_ENTRY))
                    return false;

                if (rememberedSourceEntry.IsRegularShape)
                {
                    return CanAcceptAsCompartmentSourceAndTarget(rememberedSourceShape,
                                                                    SelectedCompartmentPartType.None,
                                                                 null,
                                                                 rememberedTargetShape,
                                                                 rememberedTargetEntry.Type,
                                                                 rememberedTargetEntry.Element as TARGET_COMPARTMENT_ENTRY);
                }
                else
                {
                    return CanAcceptAsCompartmentSourceAndTarget(rememberedSourceShape,
                                                                 rememberedSourceEntry.Type,
                                                                 rememberedSourceEntry.Element as SOURCE_COMPARTMENT_ENTRY,
                                                                 rememberedTargetShape,
                                                                 rememberedTargetEntry.Type,
                                                                 rememberedTargetEntry.Element as TARGET_COMPARTMENT_ENTRY);
                }
            }
        }

        #region formerly generated code
        // but a little bit changes: CanAcceptSource() got one additional parameter and an overload call
        // without this parameter. The usages in this file of CanAcceptSource() are consequently changed.
        // some bits of Connect() are changed also
        // the original code was generated by the DSL-Tools so I think the copyright belongs to Microsoft...

        #region Accept Connection Methods
        /// <summary>
        /// Test whether a given model element is acceptable to this ConnectionBuilder as the source of a connection.
        /// </summary>
        /// <param name="candidate">The model element to test.</param>
        /// <param name="forSourceAndTarget">false: this method is called to query only the source of ne new connection, true: after this call the <see cref="CanAcceptAsSourceAndTarget"/> method will be called.</param>
        /// <returns>Whether the element can be used as the source of a connection.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        public bool CanAcceptSource(ModelElement candidate, bool forSourceAndTarget)
        {
            if (candidate == null) return false;
            else if (candidate is SOURCE_ELEMENT)
            {
                // Add a custom method to your code with the following signature:
                // private static bool CanAcceptAsSource(SOURCE_SHAPE candidate)
                // {
                // }
                return CanAcceptAsSource((SOURCE_ELEMENT)candidate, forSourceAndTarget);
            }
            else
                return false;
        }
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        public bool CanAcceptSource(ModelElement candidate)
        {
            return CanAcceptSource(candidate, false);
        }

        /// <summary>
        /// Test whether a given model element is acceptable to this ConnectionBuilder as the target of a connection.
        /// </summary>
        /// <param name="candidate">The model element to test.</param>
        /// <returns>Whether the element can be used as the target of a connection.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        public bool CanAcceptTarget(ModelElement candidate)
        {
            if (candidate == null) return false;
            else if (candidate is TARGET_ELEMENT)
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Test whether a given pair of model elements are acceptable to this ConnectionBuilder as the source and target of a connection
        /// </summary>
        /// <param name="candidateSource">The model element to test as a source</param>
        /// <param name="candidateTarget">The model element to test as a target</param>
        /// <returns>Whether the elements can be used as the source and target of a connection</returns>
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Generated code.")]
        public bool CanAcceptSourceAndTarget(ModelElement candidateSource, ModelElement candidateTarget)
        {
            // Accepts null, null; source, null; source, target but NOT null, target
            if (candidateSource == null)
            {
                if (candidateTarget != null)
                {
                    throw new ArgumentNullException("candidateSource");
                }
                else // Both null
                {
                    return false;
                }
            }
            bool acceptSource = CanAcceptSource(candidateSource, true);
            // If the source wasn't accepted then there's no point checking targets.
            // If there is no target then the source controls the accept.
            if (!acceptSource || candidateTarget == null)
            {
                return acceptSource;
            }
            else // Check combinations
            {
                if (candidateSource is SOURCE_ELEMENT)
                {
                    if (candidateTarget is TARGET_ELEMENT)
                    {
                        SOURCE_ELEMENT sourceElement = (SOURCE_ELEMENT)candidateSource;
                        TARGET_ELEMENT targetElement = (TARGET_ELEMENT)candidateTarget;
                        return CanAcceptAsSourceAndTarget(sourceElement, targetElement);
                    }
                }

            }
            return false;
        }
        #endregion

        #region Connection Methods
        /// <summary>
        /// Make a connection between the given pair of source and target elements
        /// </summary>
        /// <param name="source">The model element to use as the source of the connection</param>
        /// <param name="target">The model element to use as the target of the connection</param>
        /// <returns>A link representing the created connection</returns>
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Generated code.")]
        public ElementLink Connect(ModelElement source, ModelElement target)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            // TODO bei fehler mit base in svn vergleichen ;)
            if (source != rememberedSourceShape || target != rememberedTargetShape)
            {
                Debug.Fail("Cannot create Connection for other then the rememberd ones.");
                throw new InvalidOperationException();
            }

            if (CanAcceptSourceAndTarget(source, target))
            {
                if (source is SOURCE_ELEMENT)
                {
                    if (target is TARGET_ELEMENT)
                    {
                        SOURCE_ELEMENT sourceAccepted = (SOURCE_ELEMENT)source;
                        TARGET_ELEMENT targetAccepted = (TARGET_ELEMENT)target;

                        ElementLink result;
                        if (rememberedSourceEntry.IsRegularShape)
                        {
                            result = CreateElementLink(sourceAccepted,
                                                        SelectedCompartmentPartType.None,
                                                        null,
                                                                   targetAccepted,
                                                                   rememberedTargetEntry.Type,
                                                                   rememberedTargetEntry.Element as TARGET_COMPARTMENT_ENTRY);
                        }
                        else if (rememberedTargetEntry.IsRegularShape)
                        {
                            result = CreateElementLink(sourceAccepted,
                                                                   rememberedSourceEntry.Type,
                                                                   rememberedSourceEntry.Element as SOURCE_COMPARTMENT_ENTRY,
                                                                   targetAccepted,
                                                                   SelectedCompartmentPartType.None,
                                                                   null);
                        }
                        else
                        {
                            result = CreateElementLink(sourceAccepted,
                                                                   rememberedSourceEntry.Type,
                                                                   rememberedSourceEntry.Element as SOURCE_COMPARTMENT_ENTRY,
                                                                   targetAccepted,
                                                                   rememberedTargetEntry.Type,
                                                                   rememberedTargetEntry.Element as TARGET_COMPARTMENT_ENTRY);
                        }

                        if (DomainClassInfo.HasNameProperty(result))
                            DomainClassInfo.SetUniqueName(result);

                        rememberedSourceEntry = null;
                        rememberedTargetEntry = null;
                        rememberedSourceShape = null;
                        rememberedTargetShape = null;

                        return result;
                    }
                }

            }
            Debug.Fail("Having agreed that the connection can be accepted we should never fail to make one.");
            throw new InvalidOperationException();
        }

        #endregion
        #endregion

        /// <summary>
        /// Looks for a shape that represents a model element
        /// </summary>
        /// <param name="modelElement">a model element that is represented by a compartment shape</param>
        /// <returns>the shape</returns>
        private static ICompartmentMouseActionTrackable GetFirstCompartmentShapeForModelElement(ModelElement modelElement)
        {
            LinkedElementCollection<PresentationElement> presentations = PresentationViewsSubject.GetPresentation(modelElement);
            foreach (PresentationElement element in presentations)
            {
                ICompartmentMouseActionTrackable shape = element as ICompartmentMouseActionTrackable;
                if (shape != null)
                    return shape;
            }

            return null;
        }

        // used for error messages
        protected const string ERROR_MESSAGE_WRONG_METHOD_CALLED_CODEGEN_SHOULD_PREVENT = "This mehtod should never be called on this class. The codegen should generate one of the methods and the other two will never be called unless something bad happens.";
        protected const string ERROR_MESSAGE_WRONG_METHOD_CALLED_CODEGEN_SHOULD_PREVENT_OR_OVERRIDE = "This mehtod should never be called on this class or you have to overide it. The codegen should generate one of the methods in the commentblock at the top of the file and the other two will never be called unless something bad happens.";
    }
}
