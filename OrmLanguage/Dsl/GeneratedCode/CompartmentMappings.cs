using System;
using CompartmentMapping;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;

namespace Company.OrmLanguage
{
	#region Code for ConnectionBuilder 'EntityHasRelationShips'
    [RuleOn(typeof(Property), FireTime = TimeToFire.LocalCommit)]
    public class SourceOrTargetOfEntityHasRelationShipsDeletingRule : CompartmentEntryDeletingRuleBase<EntityElement, Property, EntityHasRelationShips, EntityElement, Property>
    {
        protected override CompartmentMappingBuilderBase<EntityElement, Property, EntityHasRelationShips, EntityElement, Property> GetBuilderInstance()
        {
            return EntityHasRelationShipsBuilder.builder;
        }
    }

    public class EntityHasRelationShipsRouter : CompartmentMappingRouterBase<EntityElement, Property, EntityHasRelationShips, EntityElement, Property>
    {
        protected override CompartmentMappingBuilderBase<EntityElement, Property, EntityHasRelationShips, EntityElement, Property> GetBuilderInstance()
        {
            return EntityHasRelationShipsBuilder.builder;
        }
    }

    public partial class EntityHasRelationShipsBuilder
    {
        internal static readonly EntityHasRelationShipsBuilderInstance builder = new EntityHasRelationShipsBuilderInstance();

        public static bool CanAcceptSource(ModelElement candidate)
        {
            return builder.CanAcceptSource(candidate);
        }

        public static bool CanAcceptTarget(ModelElement candidate)
        {
            return builder.CanAcceptTarget(candidate);
        }

        public static bool CanAcceptSourceAndTarget(ModelElement candidateSource, ModelElement candidateTarget)
        {
            return builder.CanAcceptSourceAndTarget(candidateSource, candidateTarget);
        }

        public static ElementLink Connect(ModelElement source, ModelElement target)
        {
            return builder.Connect(source, target);
        }
    }

	// Create this partial class in another file and implement at least the abstract methods!
    public partial class EntityHasRelationShipsBuilderInstance : CompartmentMappingBuilderBase<EntityElement, Property, EntityHasRelationShips, EntityElement, Property>
    {
        internal EntityHasRelationShipsBuilderInstance()
            : base(
                false,		/* connection.Source.AllowHead */
                true,		/* connection.Target.AllowHead */
                true		/* connection.AllowSelfReference */		)
        {
        }
		
		public override bool IsEntryConnectionSource(Property entry, EntityHasRelationShips connection)
        {
            if (entry == null)
                return connection.fromProperty.Equals(Guid.Empty);

            return connection.fromProperty.Equals(entry.Guid);
        }

        public override bool IsEntryConnectionTarget(Property entry, EntityHasRelationShips connection)
        {
            if (entry == null)
            {
                return connection.toProperty.Equals(Guid.Empty);
            }

            return connection.toProperty.Equals(entry.Guid);
        }

        protected override ElementLink CreateElementLink(EntityElement source, SelectedCompartmentPartType sourcePartType, Property sourceEntry, EntityElement target, SelectedCompartmentPartType targetPartType, Property targetEntry)
        {
            EntityHasRelationShips result;
            result = new EntityHasRelationShips(source, target);
            if (sourcePartType == SelectedCompartmentPartType.Head)
            {
                result.fromProperty = Guid.Empty;
                result.fromPropertyName = string.Empty;
            }
            else
            {
                result.fromProperty = sourceEntry.Guid;
				result.fromPropertyName = sourceEntry.EntityElement.Name + "_" + sourceEntry.Name;
            }

            if (targetPartType == SelectedCompartmentPartType.Head)
            {
                result.toProperty = Guid.Empty;
				result.toPropertyName = string.Empty;
            }
            else
            {
                result.toProperty = targetEntry.Guid;
				result.toPropertyName = targetEntry.EntityElement.Name + "_" + targetEntry.Name;
            }

            return result;
        }
    }	
	#endregion

	#region Code for CompartmentShape 'EntityShape'
    partial class EntityShape : ICompartmentMouseActionTrackable
    {
        private readonly CompartmentMouseTrack mouseTrack = new CompartmentMouseTrack();

        public CompartmentMouseTrack MouseTrack
        {
            get { return mouseTrack; }
        }

        public override void OnShapeInserted()
        {
            mouseTrack.ShapeInserted(this);
            base.OnShapeInserted();
        }
        
        public override void OnAbsoluteBoundsChanged(Microsoft.VisualStudio.Modeling.Diagrams.AbsoluteBoundsChangedEventArgs e)
        {
            CompartmentMappingUtil.RerouteCompartmentMappings(this);
            base.OnAbsoluteBoundsChanged(e);
        }
    }
	#endregion


	#region Code for Connector 'EntityHasRelationShipsConnector'
    partial class EntityHasRelationShipsConnector
    {
        public override void OnInitialize()
        {
            this.FixedFrom = VGFixedCode.Caller;
            this.FixedTo = VGFixedCode.Caller;
            base.OnInitialize();
        }

        public override bool CanManuallyRoute
        {
            get
            {
                return false;
            }
        }
    }
    
    [RuleOn(typeof(EntityHasRelationShipsConnector), FireTime = TimeToFire.LocalCommit)]
    public class EntityHasRelationShipsConnectorAddRule : CompartmentMappingAddRuleBase
    {
    }
	#endregion
}
