/*  (c) 2008 Benjamin Schroeter
 *  This file is part of JaDAL - Just another DSL-Tools Addon Library
 *  and licensed under the New BSD License
 *  For more information please visit http://www.codeplex.com/JaDAL */
using System;
using System.Linq;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;

namespace CompartmentMapping
{
    /// <summary>
    /// Interface to provide a function to correct the end points of a compartment connection to correspond to the entries of the compartmetn shapes.
    /// </summary>
    public interface ICompartmentMappingRouter
    {
        /// <summary>
        /// Correct the end points of a compartment connection to correspond to the entries of the compartmetn shapes.
        /// </summary>
        /// <param name="linkShape">the BinaryLinkShape</param>
        void CorrectBinaryLinkShapeEndPoints(BinaryLinkShape linkShape);
    }

    /// <summary>
    /// Base class for classes that determinate the routing (or better the start and endpoints) of compartment connections.
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
    public abstract class CompartmentMappingRouterBase<SOURCE_ELEMENT, SOURCE_COMPARTMENT_ENTRY, CONNECTION, TARGET_ELEMENT, TARGET_COMPARTMENT_ENTRY> : ICompartmentMappingRouter
        where CONNECTION : ElementLink
        where SOURCE_COMPARTMENT_ENTRY : class
        where TARGET_COMPARTMENT_ENTRY : class
        where SOURCE_ELEMENT : ModelElement
        where TARGET_ELEMENT : ModelElement
    {
        /// <summary>
        /// Correct the end points of a compartment connection to correspond to the entries of the compartmetn shapes.
        /// </summary>
        /// <param name="linkShape">the BinaryLinkShape</param>
        public void CorrectBinaryLinkShapeEndPoints(BinaryLinkShape linkShape)
        {
            CONNECTION connection = (CONNECTION)linkShape.ModelElement;
            if (connection == null)
                return;

            // source
            CompartmentShape fromShape = linkShape.FromShape as CompartmentShape;
            RectangleD fromRec = linkShape.FromShape.AbsoluteBoundingBox;
            PointD leftFrom;
            PointD rightFrom;
            if (fromShape != null)
            {
                // vertical offset on source
                Double fromOffset = GetVerticalOffset<SOURCE_COMPARTMENT_ENTRY>(fromShape, connection, GetBuilderInstance().IsEntryConnectionSource);
                leftFrom = new PointD(fromRec.Left, fromRec.Top + fromOffset);
                rightFrom = new PointD(fromRec.Right, fromRec.Top + fromOffset);
            }
            else
                leftFrom = rightFrom = fromRec.Center;

            // taget
            CompartmentShape toShape = linkShape.ToShape as CompartmentShape;
            RectangleD toRec = linkShape.ToShape.AbsoluteBoundingBox;
            PointD leftTarget;
            PointD rightTarget;
            if (toShape != null)
            {
                // vertical offset on source
                Double toOffset = GetVerticalOffset<TARGET_COMPARTMENT_ENTRY>(toShape, connection, GetBuilderInstance().IsEntryConnectionTarget);
                leftTarget = new PointD(toRec.Left, toRec.Top + toOffset);
                rightTarget = new PointD(toRec.Right, toRec.Top + toOffset);
            }
            else
                leftTarget = rightTarget = toRec.Center;

            // determinate the connection points
            ConnectionPoints points = GetConnectionPoints(leftFrom, rightFrom, leftTarget, rightTarget);

            // set point for source
            if (fromShape != null)
            {
                linkShape.FromEndPoint = points.Source;
                linkShape.FixedFrom = VGFixedCode.Caller;
            }

            // set point for taget
            if (toShape != null)
            {
                linkShape.ToEndPoint = points.Target;
                linkShape.FixedTo = VGFixedCode.Caller;
            }
        }

        protected delegate bool IsEntryConnectionSourceTarget<ENTRY>(ENTRY entry, CONNECTION connection);

        /// <summary>
        /// Determinates the vertical offset by searching the compartment entries connected by the given connection
        /// </summary>
        /// <typeparam name="T">The type of the compartment entry</typeparam>
        /// <param name="shape">the CompartmentShape</param>
        /// <param name="connection">the Connection</param>
        /// <param name="equaler">a delegate to the method to compare compartment entry with the connection</param>
        /// <returns></returns>
        protected virtual double GetVerticalOffset<T>(CompartmentShape shape, CONNECTION connection, IsEntryConnectionSourceTarget<T> equaler) where T : class
        {
            // the easy case: the header is meant or the shape is not expanded. so no entry must be searched
            if (!shape.IsExpanded || equaler(null, connection))
                return shape.DefaultSize.Height / 2; //the half of the height (the height of a CompartmentShape means only the height of the header)

            int elementListCompartmentCount = 0;
            bool entryFound = false;
            int entryPositionInListCompartment = -1;
            ElementListCompartment entryParent = null;

            // take a look at all children of the compartment shape
            foreach (DiagramItem item in shape.Children())
            {
                // some of the children are ElementListCompartments
                if (item.Shape is ElementListCompartment)
                {
                    // we have to count the ElementListCompartments
                    elementListCompartmentCount++;
                    // if we already found the entry and know the count of elementListCompartmentCount we can exit the foreach loop
                    // I only need the information elementListCompartmentCount is == 1 or > 1, the exact count is irrelevant
                    if (entryFound && elementListCompartmentCount > 1)
                        break;

                    ElementListCompartment compartmentList = (ElementListCompartment)item.Shape;

                    int entryInList = 0;
                    if (compartmentList.Items != null)
                    {
                        // now, take a look at the compartment entries of the compartment list
                        foreach (object compartmentEntry in compartmentList.Items)
                        {
                            // is this entry of our type?
                            if (compartmentEntry is T)
                            {
                                // finally check if this entry is targeted by the given connection
                                if (equaler((T)compartmentEntry, connection))
                                {
                                    // hey found... :-)
                                    entryFound = true;
                                    entryParent = compartmentList;
                                    entryPositionInListCompartment = entryInList;
                                    break; // there is no need to search in this compartmentList any longer (breaks only the inner foreach loop)
                                }

                                // not found: count and next
                                entryInList++;
                            }
                        }
                    }
                }
            }

            if (entryFound)
            {
                // from msdn: IsSingleCompartmentHeaderVisible: Gets the compartment shape and checks to see whether the its header is visible (if there is only one header in the compartment). 
                // to determinate if the compartment header is realy visible we need the IsSingleCompartmentHeaderVisible property and the number of compartment headers in this shape
                bool elementListCompartmentHeaderVisible = elementListCompartmentCount == 1 ? shape.IsSingleCompartmentHeaderVisible : true;
                int compartmentHeaderOffset = elementListCompartmentHeaderVisible ? 1 : 0;


                if (entryParent.IsExpanded)
                {
                    // the entry is visible
                    double singelElementRowHeight = entryParent.BoundingBox.Height / (entryParent.Items.Count + compartmentHeaderOffset);
                    return entryParent.BoundingBox.Top + singelElementRowHeight * (entryPositionInListCompartment + 0.6 + compartmentHeaderOffset);
                }
                else
                {
                    // the entry is not visible
                    // so we target the middel of the compartment header
                    return entryParent.BoundingBox.Top + entryParent.BoundingBox.Height * 0.5;
                }
            }

            // if we come to this point, we cannot find the entry. just return null. :-/
            return 0;
        }

        /// <summary>
        /// If I have to connect two CompartmentShapes I can calclulate two points for each Shape where the
        /// link should connect to the shape. One on the left and one on the right side.
        /// 
        /// This functions chooses the pair of points to draw the connectionline. The two points with the
        /// smallest distance will be choosen.
        /// </summary>
        /// <param name="firstCandidateSource">One point on the source shape</param>
        /// <param name="secondCandiateSource">Another point on the source shape</param>
        /// <param name="firstCandidateTarget">One point on the target shape</param>
        /// <param name="secondCandiateTarget">Another point on the target shape</param>
        /// <returns>The two points to use as connectionpoints</returns>
        private ConnectionPoints GetConnectionPoints(PointD firstCandidateSource, PointD secondCandiateSource,
                                                     PointD firstCandidateTarget, PointD secondCandiateTarget)
        {
            Double distanceS1T1 = GetDistance(firstCandidateSource, firstCandidateTarget);
            Double distanceS2T1 = GetDistance(secondCandiateSource, firstCandidateTarget);
            Double distanceS1T2 = GetDistance(firstCandidateSource, secondCandiateTarget);
            Double distanceS2T2 = GetDistance(secondCandiateSource, secondCandiateTarget);

            Double minDistance = (new Double[] { distanceS1T1, distanceS1T2, distanceS2T1, distanceS2T2 }).Min();

            if (minDistance == distanceS1T1)
                return new ConnectionPoints(firstCandidateSource, firstCandidateTarget);
            if (minDistance == distanceS2T1)
                return new ConnectionPoints(secondCandiateSource, firstCandidateTarget);
            if (minDistance == distanceS1T2)
                return new ConnectionPoints(firstCandidateSource, secondCandiateTarget);

            return new ConnectionPoints(secondCandiateSource, secondCandiateTarget);
        }

        /// <summary>
        /// Gets a value corresponding to the distance of two points.
        /// </summary>
        /// <remarks>Since i do not want to calculate the sqrt-function the return value is not realy the distance.</remarks>
        /// <param name="p1">One point</param>
        /// <param name="p2">Another point</param>
        /// <returns>A comparable meter for the distance of the two points</returns>
        private static double GetDistance(PointD p1, PointD p2)
        {
            double x = p1.X - p2.X;
            double y = p1.Y - p2.Y;

            return x * x + y * y;   // since we do not need the realy value we can obmitt the sqrt calculation
        }

        /// <summary>
        /// This method is used to access the IsEntryConnectionSource() and IsEntryConnectionTarget() methods 
        /// via the CompartmentMappingBuilderBase instance generated by the code generator.
        /// </summary>
        /// <remarks>This method is implemented by the code generator.</remarks>
        /// <returns>a CompartmentMappingBuilderBase for this connection</returns>
        protected abstract CompartmentMappingBuilderBase<SOURCE_ELEMENT, SOURCE_COMPARTMENT_ENTRY, CONNECTION, TARGET_ELEMENT, TARGET_COMPARTMENT_ENTRY> GetBuilderInstance();

        /// <summary>
        /// This struct is only used for the return value of <see cref="ConnectionPoints"/>.
        /// </summary>
        private struct ConnectionPoints
        {
            public readonly PointD Source;
            public readonly PointD Target;

            public ConnectionPoints(PointD source, PointD target)
            {
                Source = source;
                Target = target;
            }
        }

    }
}