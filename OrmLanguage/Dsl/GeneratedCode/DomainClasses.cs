﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using DslModeling = global::Microsoft.VisualStudio.Modeling;
using DslDesign = global::Microsoft.VisualStudio.Modeling.Design;
namespace Company.OrmLanguage
{
	/// <summary>
	/// DomainClass SampleOrmModel
	/// The root in which all other elements are embedded. Appears as a diagram.
	/// </summary>
	[DslDesign::DisplayNameResource("Company.OrmLanguage.SampleOrmModel.DisplayName", typeof(global::Company.OrmLanguage.OrmLanguageDomainModel), "Company.OrmLanguage.GeneratedCode.DomainModelResx")]
	[DslDesign::DescriptionResource("Company.OrmLanguage.SampleOrmModel.Description", typeof(global::Company.OrmLanguage.OrmLanguageDomainModel), "Company.OrmLanguage.GeneratedCode.DomainModelResx")]
	[DslModeling::DomainModelOwner(typeof(global::Company.OrmLanguage.OrmLanguageDomainModel))]
	[global::System.CLSCompliant(true)]
	[DslModeling::DomainObjectId("583ad1fe-b130-4778-a99a-ddfa88829f94")]
	public partial class SampleOrmModel : DslModeling::ModelElement
	{
		#region Constructors, domain class Id
	
		/// <summary>
		/// SampleOrmModel domain class Id.
		/// </summary>
		public static readonly new global::System.Guid DomainClassId = new global::System.Guid(0x583ad1fe, 0xb130, 0x4778, 0xa9, 0x9a, 0xdd, 0xfa, 0x88, 0x82, 0x9f, 0x94);
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="store">Store where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public SampleOrmModel(DslModeling::Store store, params DslModeling::PropertyAssignment[] propertyAssignments)
			: this(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, propertyAssignments)
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="partition">Partition where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public SampleOrmModel(DslModeling::Partition partition, params DslModeling::PropertyAssignment[] propertyAssignments)
			: base(partition, propertyAssignments)
		{
		}
		#endregion
		#region Elements opposite domain role accessor
		
		/// <summary>
		/// Gets a list of Elements.
		/// </summary>
		public virtual DslModeling::LinkedElementCollection<EntityElement> Elements
		{
			[global::System.Diagnostics.DebuggerStepThrough]
			get
			{
				return GetRoleCollection<DslModeling::LinkedElementCollection<EntityElement>, EntityElement>(global::Company.OrmLanguage.SampleOrmModelHasElements.SampleOrmModelDomainRoleId);
			}
		}
		#endregion
		#region ElementGroupPrototype Merge methods
		/// <summary>
		/// Returns a value indicating whether the source element represented by the
		/// specified root ProtoElement can be added to this element.
		/// </summary>
		/// <param name="rootElement">
		/// The root ProtoElement representing a source element.  This can be null, 
		/// in which case the ElementGroupPrototype does not contain an ProtoElements
		/// and the code should inspect the ElementGroupPrototype context information.
		/// </param>
		/// <param name="elementGroupPrototype">The ElementGroupPrototype that contains the root ProtoElement.</param>
		/// <returns>true if the source element represented by the ProtoElement can be added to this target element.</returns>
		protected override bool CanMerge(DslModeling::ProtoElementBase rootElement, DslModeling::ElementGroupPrototype elementGroupPrototype)
		{
			if ( elementGroupPrototype == null ) throw new global::System.ArgumentNullException("elementGroupPrototype");
			
			if (rootElement != null)
			{
				DslModeling::DomainClassInfo rootElementDomainInfo = this.Partition.DomainDataDirectory.GetDomainClass(rootElement.DomainClassId);
				
				if (rootElementDomainInfo.IsDerivedFrom(global::Company.OrmLanguage.EntityElement.DomainClassId)) 
				{
					return true;
				}
			}
			return base.CanMerge(rootElement, elementGroupPrototype);
		}
		
		/// <summary>
		/// Called by the Merge process to create a relationship between 
		/// this target element and the specified source element. 
		/// Typically, a parent-child relationship is established
		/// between the target element (the parent) and the source element 
		/// (the child), but any relationship can be established.
		/// </summary>
		/// <param name="sourceElement">The element that is to be related to this model element.</param>
		/// <param name="elementGroup">The group of source ModelElements that have been rehydrated into the target store.</param>
		/// <remarks>
		/// This method is overriden to create the relationship between the target element and the specified source element.
		/// The base method does nothing.
		/// </remarks>
		protected override void MergeRelate(DslModeling::ModelElement sourceElement, DslModeling::ElementGroup elementGroup)
		{
			// In general, sourceElement is allowed to be null, meaning that the elementGroup must be parsed for special cases.
			// However this is not supported in generated code.  Use double-deriving on this class and then override MergeRelate completely if you 
			// need to support this case.
			if ( sourceElement == null ) throw new global::System.ArgumentNullException("sourceElement");
		
				
			global::Company.OrmLanguage.EntityElement sourceEntityElement1 = sourceElement as global::Company.OrmLanguage.EntityElement;
			if (sourceEntityElement1 != null)
			{
				// Create link for path SampleOrmModelHasElements.Elements
				this.Elements.Add(sourceEntityElement1);

				return;
			}
		
			// Sdk workaround to runtime bug #879350 (DSL: can't copy and paste a MEL that has a MEX). Avoid MergeRelate on ModelElementExtension
			// during a "Paste".
			if (sourceElement is DslModeling::ExtensionElement
				&& sourceElement.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.ContainsKey("{9DAFD42A-DC0E-4d78-8C3F-8266B2CF8B33}"))
			{
				return;
			}
		
			// Fall through to base class if this class hasn't handled the merge.
			base.MergeRelate(sourceElement, elementGroup);
		}
		
		/// <summary>
		/// Performs operation opposite to MergeRelate - i.e. disconnects a given
		/// element from the current one (removes links created by MergeRelate).
		/// </summary>
		/// <param name="sourceElement">Element to be unmerged/disconnected.</param>
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
		protected override void MergeDisconnect(DslModeling::ModelElement sourceElement)
		{
			if (sourceElement == null) throw new global::System.ArgumentNullException("sourceElement");
				
			global::Company.OrmLanguage.EntityElement sourceEntityElement1 = sourceElement as global::Company.OrmLanguage.EntityElement;
			if (sourceEntityElement1 != null)
			{
				// Delete link for path SampleOrmModelHasElements.Elements
				
				foreach (DslModeling::ElementLink link in global::Company.OrmLanguage.SampleOrmModelHasElements.GetLinks((global::Company.OrmLanguage.SampleOrmModel)this, sourceEntityElement1))
				{
					// Delete the link, but without possible delete propagation to the element since it's moving to a new location.
					link.Delete(global::Company.OrmLanguage.SampleOrmModelHasElements.SampleOrmModelDomainRoleId, global::Company.OrmLanguage.SampleOrmModelHasElements.ElementDomainRoleId);
				}

				return;
			}
			// Fall through to base class if this class hasn't handled the unmerge.
			base.MergeDisconnect(sourceElement);
		}
		#endregion
	}
}
namespace Company.OrmLanguage
{
	/// <summary>
	/// DomainClass EntityElement
	/// Elements embedded in the model. Appear as boxes on the diagram.
	/// </summary>
	[DslDesign::DisplayNameResource("Company.OrmLanguage.EntityElement.DisplayName", typeof(global::Company.OrmLanguage.OrmLanguageDomainModel), "Company.OrmLanguage.GeneratedCode.DomainModelResx")]
	[DslDesign::DescriptionResource("Company.OrmLanguage.EntityElement.Description", typeof(global::Company.OrmLanguage.OrmLanguageDomainModel), "Company.OrmLanguage.GeneratedCode.DomainModelResx")]
	[DslModeling::DomainModelOwner(typeof(global::Company.OrmLanguage.OrmLanguageDomainModel))]
	[global::System.CLSCompliant(true)]
	[DslModeling::DomainObjectId("10f6f6a8-e10f-47b5-b83e-153c8cfb0322")]
	public partial class EntityElement : DslModeling::ModelElement
	{
		#region Constructors, domain class Id
	
		/// <summary>
		/// EntityElement domain class Id.
		/// </summary>
		public static readonly new global::System.Guid DomainClassId = new global::System.Guid(0x10f6f6a8, 0xe10f, 0x47b5, 0xb8, 0x3e, 0x15, 0x3c, 0x8c, 0xfb, 0x03, 0x22);
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="store">Store where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public EntityElement(DslModeling::Store store, params DslModeling::PropertyAssignment[] propertyAssignments)
			: this(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, propertyAssignments)
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="partition">Partition where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public EntityElement(DslModeling::Partition partition, params DslModeling::PropertyAssignment[] propertyAssignments)
			: base(partition, propertyAssignments)
		{
		}
		#endregion
		#region Name domain property code
		
		/// <summary>
		/// Name domain property Id.
		/// </summary>
		public static readonly global::System.Guid NameDomainPropertyId = new global::System.Guid(0x3d2a9e27, 0x1fbf, 0x4b35, 0x82, 0xf5, 0xaa, 0xab, 0x39, 0x79, 0xcc, 0xec);
		
		/// <summary>
		/// Storage for Name
		/// </summary>
		private global::System.String namePropertyStorage = string.Empty;
		
		/// <summary>
		/// Gets or sets the value of Name domain property.
		/// Description for Company.OrmLanguage.EntityElement.Name
		/// </summary>
		[DslDesign::DisplayNameResource("Company.OrmLanguage.EntityElement/Name.DisplayName", typeof(global::Company.OrmLanguage.OrmLanguageDomainModel), "Company.OrmLanguage.GeneratedCode.DomainModelResx")]
		[DslDesign::DescriptionResource("Company.OrmLanguage.EntityElement/Name.Description", typeof(global::Company.OrmLanguage.OrmLanguageDomainModel), "Company.OrmLanguage.GeneratedCode.DomainModelResx")]
		[DslModeling::DomainObjectId("3d2a9e27-1fbf-4b35-82f5-aaab3979ccec")]
		public global::System.String Name
		{
			[global::System.Diagnostics.DebuggerStepThrough]
			get
			{
				return namePropertyStorage;
			}
			[global::System.Diagnostics.DebuggerStepThrough]
			set
			{
				NamePropertyHandler.Instance.SetValue(this, value);
			}
		}
		/// <summary>
		/// Value handler for the EntityElement.Name domain property.
		/// </summary>
		internal sealed partial class NamePropertyHandler : DslModeling::DomainPropertyValueHandler<EntityElement, global::System.String>
		{
			private NamePropertyHandler() { }
		
			/// <summary>
			/// Gets the singleton instance of the EntityElement.Name domain property value handler.
			/// </summary>
			public static readonly NamePropertyHandler Instance = new NamePropertyHandler();
		
			/// <summary>
			/// Gets the Id of the EntityElement.Name domain property.
			/// </summary>
			public sealed override global::System.Guid DomainPropertyId
			{
				[global::System.Diagnostics.DebuggerStepThrough]
				get
				{
					return NameDomainPropertyId;
				}
			}
			
			/// <summary>
			/// Gets a strongly-typed value of the property on specified element.
			/// </summary>
			/// <param name="element">Element which owns the property.</param>
			/// <returns>Property value.</returns>
			public override sealed global::System.String GetValue(EntityElement element)
			{
				if (element == null) throw new global::System.ArgumentNullException("element");
				return element.namePropertyStorage;
			}
		
			/// <summary>
			/// Sets property value on an element.
			/// </summary>
			/// <param name="element">Element which owns the property.</param>
			/// <param name="newValue">New property value.</param>
			public override sealed void SetValue(EntityElement element, global::System.String newValue)
			{
				if (element == null) throw new global::System.ArgumentNullException("element");
		
				global::System.String oldValue = GetValue(element);
				if (newValue != oldValue)
				{
					ValueChanging(element, oldValue, newValue);
					element.namePropertyStorage = newValue;
					ValueChanged(element, oldValue, newValue);
				}
			}
		}
		
		#endregion
		#region SampleOrmModel opposite domain role accessor
		/// <summary>
		/// Gets or sets SampleOrmModel.
		/// </summary>
		public virtual SampleOrmModel SampleOrmModel
		{
			[global::System.Diagnostics.DebuggerStepThrough]
			get
			{
				return DslModeling::DomainRoleInfo.GetLinkedElement(this, global::Company.OrmLanguage.SampleOrmModelHasElements.ElementDomainRoleId) as SampleOrmModel;
			}
			[global::System.Diagnostics.DebuggerStepThrough]
			set
			{
				DslModeling::DomainRoleInfo.SetLinkedElement(this, global::Company.OrmLanguage.SampleOrmModelHasElements.ElementDomainRoleId, value);
			}
		}
		#endregion
		#region Targets opposite domain role accessor
		
		/// <summary>
		/// Gets a list of Targets.
		/// Description for Company.OrmLanguage.ExampleRelationship.Target
		/// </summary>
		public virtual DslModeling::LinkedElementCollection<EntityElement> Targets
		{
			[global::System.Diagnostics.DebuggerStepThrough]
			get
			{
				return GetRoleCollection<DslModeling::LinkedElementCollection<EntityElement>, EntityElement>(global::Company.OrmLanguage.EntityElementReferencesTargets.SourceDomainRoleId);
			}
		}
		#endregion
		#region Sources opposite domain role accessor
		
		/// <summary>
		/// Gets a list of Sources.
		/// Description for Company.OrmLanguage.ExampleRelationship.Source
		/// </summary>
		public virtual DslModeling::LinkedElementCollection<EntityElement> Sources
		{
			[global::System.Diagnostics.DebuggerStepThrough]
			get
			{
				return GetRoleCollection<DslModeling::LinkedElementCollection<EntityElement>, EntityElement>(global::Company.OrmLanguage.EntityElementReferencesTargets.TargetDomainRoleId);
			}
		}
		#endregion
		#region Properties opposite domain role accessor
		
		/// <summary>
		/// Gets a list of Properties.
		/// Description for Company.OrmLanguage.EntityHasProperties.EntityElement
		/// </summary>
		public virtual DslModeling::LinkedElementCollection<EntityProperty> Properties
		{
			[global::System.Diagnostics.DebuggerStepThrough]
			get
			{
				return GetRoleCollection<DslModeling::LinkedElementCollection<EntityProperty>, EntityProperty>(global::Company.OrmLanguage.EntityHasProperties.EntityElementDomainRoleId);
			}
		}
		#endregion
		#region ElementGroupPrototype Merge methods
		/// <summary>
		/// Returns a value indicating whether the source element represented by the
		/// specified root ProtoElement can be added to this element.
		/// </summary>
		/// <param name="rootElement">
		/// The root ProtoElement representing a source element.  This can be null, 
		/// in which case the ElementGroupPrototype does not contain an ProtoElements
		/// and the code should inspect the ElementGroupPrototype context information.
		/// </param>
		/// <param name="elementGroupPrototype">The ElementGroupPrototype that contains the root ProtoElement.</param>
		/// <returns>true if the source element represented by the ProtoElement can be added to this target element.</returns>
		protected override bool CanMerge(DslModeling::ProtoElementBase rootElement, DslModeling::ElementGroupPrototype elementGroupPrototype)
		{
			if ( elementGroupPrototype == null ) throw new global::System.ArgumentNullException("elementGroupPrototype");
			
			if (rootElement != null)
			{
				DslModeling::DomainClassInfo rootElementDomainInfo = this.Partition.DomainDataDirectory.GetDomainClass(rootElement.DomainClassId);
				
				if (rootElementDomainInfo.IsDerivedFrom(global::Company.OrmLanguage.EntityProperty.DomainClassId)) 
				{
					return true;
				}
			}
			return base.CanMerge(rootElement, elementGroupPrototype);
		}
		
		/// <summary>
		/// Called by the Merge process to create a relationship between 
		/// this target element and the specified source element. 
		/// Typically, a parent-child relationship is established
		/// between the target element (the parent) and the source element 
		/// (the child), but any relationship can be established.
		/// </summary>
		/// <param name="sourceElement">The element that is to be related to this model element.</param>
		/// <param name="elementGroup">The group of source ModelElements that have been rehydrated into the target store.</param>
		/// <remarks>
		/// This method is overriden to create the relationship between the target element and the specified source element.
		/// The base method does nothing.
		/// </remarks>
		protected override void MergeRelate(DslModeling::ModelElement sourceElement, DslModeling::ElementGroup elementGroup)
		{
			// In general, sourceElement is allowed to be null, meaning that the elementGroup must be parsed for special cases.
			// However this is not supported in generated code.  Use double-deriving on this class and then override MergeRelate completely if you 
			// need to support this case.
			if ( sourceElement == null ) throw new global::System.ArgumentNullException("sourceElement");
		
				
			global::Company.OrmLanguage.EntityProperty sourceEntityProperty1 = sourceElement as global::Company.OrmLanguage.EntityProperty;
			if (sourceEntityProperty1 != null)
			{
				// Create link for path EntityHasProperties.Properties
				this.Properties.Add(sourceEntityProperty1);

				return;
			}
		
			// Sdk workaround to runtime bug #879350 (DSL: can't copy and paste a MEL that has a MEX). Avoid MergeRelate on ModelElementExtension
			// during a "Paste".
			if (sourceElement is DslModeling::ExtensionElement
				&& sourceElement.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.ContainsKey("{9DAFD42A-DC0E-4d78-8C3F-8266B2CF8B33}"))
			{
				return;
			}
		
			// Fall through to base class if this class hasn't handled the merge.
			base.MergeRelate(sourceElement, elementGroup);
		}
		
		/// <summary>
		/// Performs operation opposite to MergeRelate - i.e. disconnects a given
		/// element from the current one (removes links created by MergeRelate).
		/// </summary>
		/// <param name="sourceElement">Element to be unmerged/disconnected.</param>
		[global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
		protected override void MergeDisconnect(DslModeling::ModelElement sourceElement)
		{
			if (sourceElement == null) throw new global::System.ArgumentNullException("sourceElement");
				
			global::Company.OrmLanguage.EntityProperty sourceEntityProperty1 = sourceElement as global::Company.OrmLanguage.EntityProperty;
			if (sourceEntityProperty1 != null)
			{
				// Delete link for path EntityHasProperties.Properties
				
				foreach (DslModeling::ElementLink link in global::Company.OrmLanguage.EntityHasProperties.GetLinks((global::Company.OrmLanguage.EntityElement)this, sourceEntityProperty1))
				{
					// Delete the link, but without possible delete propagation to the element since it's moving to a new location.
					link.Delete(global::Company.OrmLanguage.EntityHasProperties.EntityElementDomainRoleId, global::Company.OrmLanguage.EntityHasProperties.EntityPropertyDomainRoleId);
				}

				return;
			}
			// Fall through to base class if this class hasn't handled the unmerge.
			base.MergeDisconnect(sourceElement);
		}
		#endregion
	}
}
namespace Company.OrmLanguage
{
	/// <summary>
	/// DomainClass EntityProperty
	/// Description for Company.OrmLanguage.EntityProperty
	/// </summary>
	[DslDesign::DisplayNameResource("Company.OrmLanguage.EntityProperty.DisplayName", typeof(global::Company.OrmLanguage.OrmLanguageDomainModel), "Company.OrmLanguage.GeneratedCode.DomainModelResx")]
	[DslDesign::DescriptionResource("Company.OrmLanguage.EntityProperty.Description", typeof(global::Company.OrmLanguage.OrmLanguageDomainModel), "Company.OrmLanguage.GeneratedCode.DomainModelResx")]
	[DslModeling::DomainModelOwner(typeof(global::Company.OrmLanguage.OrmLanguageDomainModel))]
	[global::System.CLSCompliant(true)]
	[DslModeling::DomainObjectId("2e887c94-2215-44d4-82cb-d0a069dff0bf")]
	public partial class EntityProperty : DslModeling::ModelElement
	{
		#region Constructors, domain class Id
	
		/// <summary>
		/// EntityProperty domain class Id.
		/// </summary>
		public static readonly new global::System.Guid DomainClassId = new global::System.Guid(0x2e887c94, 0x2215, 0x44d4, 0x82, 0xcb, 0xd0, 0xa0, 0x69, 0xdf, 0xf0, 0xbf);
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="store">Store where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public EntityProperty(DslModeling::Store store, params DslModeling::PropertyAssignment[] propertyAssignments)
			: this(store != null ? store.DefaultPartitionForClass(DomainClassId) : null, propertyAssignments)
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="partition">Partition where new element is to be created.</param>
		/// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
		public EntityProperty(DslModeling::Partition partition, params DslModeling::PropertyAssignment[] propertyAssignments)
			: base(partition, propertyAssignments)
		{
		}
		#endregion
		#region Type domain property code
		
		/// <summary>
		/// Type domain property Id.
		/// </summary>
		public static readonly global::System.Guid TypeDomainPropertyId = new global::System.Guid(0xa438b50e, 0x3af8, 0x4af6, 0x8c, 0xb3, 0xb6, 0x9e, 0x27, 0x87, 0x07, 0x67);
		
		/// <summary>
		/// Storage for Type
		/// </summary>
		private global::System.String typePropertyStorage = string.Empty;
		
		/// <summary>
		/// Gets or sets the value of Type domain property.
		/// Description for Company.OrmLanguage.EntityProperty.Type
		/// </summary>
		[DslDesign::DisplayNameResource("Company.OrmLanguage.EntityProperty/Type.DisplayName", typeof(global::Company.OrmLanguage.OrmLanguageDomainModel), "Company.OrmLanguage.GeneratedCode.DomainModelResx")]
		[DslDesign::DescriptionResource("Company.OrmLanguage.EntityProperty/Type.Description", typeof(global::Company.OrmLanguage.OrmLanguageDomainModel), "Company.OrmLanguage.GeneratedCode.DomainModelResx")]
		[DslModeling::DomainObjectId("a438b50e-3af8-4af6-8cb3-b69e27870767")]
		public global::System.String Type
		{
			[global::System.Diagnostics.DebuggerStepThrough]
			get
			{
				return typePropertyStorage;
			}
			[global::System.Diagnostics.DebuggerStepThrough]
			set
			{
				TypePropertyHandler.Instance.SetValue(this, value);
			}
		}
		/// <summary>
		/// Value handler for the EntityProperty.Type domain property.
		/// </summary>
		internal sealed partial class TypePropertyHandler : DslModeling::DomainPropertyValueHandler<EntityProperty, global::System.String>
		{
			private TypePropertyHandler() { }
		
			/// <summary>
			/// Gets the singleton instance of the EntityProperty.Type domain property value handler.
			/// </summary>
			public static readonly TypePropertyHandler Instance = new TypePropertyHandler();
		
			/// <summary>
			/// Gets the Id of the EntityProperty.Type domain property.
			/// </summary>
			public sealed override global::System.Guid DomainPropertyId
			{
				[global::System.Diagnostics.DebuggerStepThrough]
				get
				{
					return TypeDomainPropertyId;
				}
			}
			
			/// <summary>
			/// Gets a strongly-typed value of the property on specified element.
			/// </summary>
			/// <param name="element">Element which owns the property.</param>
			/// <returns>Property value.</returns>
			public override sealed global::System.String GetValue(EntityProperty element)
			{
				if (element == null) throw new global::System.ArgumentNullException("element");
				return element.typePropertyStorage;
			}
		
			/// <summary>
			/// Sets property value on an element.
			/// </summary>
			/// <param name="element">Element which owns the property.</param>
			/// <param name="newValue">New property value.</param>
			public override sealed void SetValue(EntityProperty element, global::System.String newValue)
			{
				if (element == null) throw new global::System.ArgumentNullException("element");
		
				global::System.String oldValue = GetValue(element);
				if (newValue != oldValue)
				{
					ValueChanging(element, oldValue, newValue);
					element.typePropertyStorage = newValue;
					ValueChanged(element, oldValue, newValue);
				}
			}
		}
		
		#endregion
		#region Name domain property code
		
		/// <summary>
		/// Name domain property Id.
		/// </summary>
		public static readonly global::System.Guid NameDomainPropertyId = new global::System.Guid(0x8a56a53b, 0x6bfc, 0x4bb5, 0x98, 0xd9, 0x10, 0xa9, 0xeb, 0xe9, 0xe7, 0x8a);
		
		/// <summary>
		/// Storage for Name
		/// </summary>
		private global::System.String namePropertyStorage = "NewProperty";
		
		/// <summary>
		/// Gets or sets the value of Name domain property.
		/// Description for Company.OrmLanguage.EntityProperty.Name
		/// </summary>
		[DslDesign::DisplayNameResource("Company.OrmLanguage.EntityProperty/Name.DisplayName", typeof(global::Company.OrmLanguage.OrmLanguageDomainModel), "Company.OrmLanguage.GeneratedCode.DomainModelResx")]
		[DslDesign::DescriptionResource("Company.OrmLanguage.EntityProperty/Name.Description", typeof(global::Company.OrmLanguage.OrmLanguageDomainModel), "Company.OrmLanguage.GeneratedCode.DomainModelResx")]
		[global::System.ComponentModel.DefaultValue("NewProperty")]
		[DslModeling::DomainObjectId("8a56a53b-6bfc-4bb5-98d9-10a9ebe9e78a")]
		public global::System.String Name
		{
			[global::System.Diagnostics.DebuggerStepThrough]
			get
			{
				return namePropertyStorage;
			}
			[global::System.Diagnostics.DebuggerStepThrough]
			set
			{
				NamePropertyHandler.Instance.SetValue(this, value);
			}
		}
		/// <summary>
		/// Value handler for the EntityProperty.Name domain property.
		/// </summary>
		internal sealed partial class NamePropertyHandler : DslModeling::DomainPropertyValueHandler<EntityProperty, global::System.String>
		{
			private NamePropertyHandler() { }
		
			/// <summary>
			/// Gets the singleton instance of the EntityProperty.Name domain property value handler.
			/// </summary>
			public static readonly NamePropertyHandler Instance = new NamePropertyHandler();
		
			/// <summary>
			/// Gets the Id of the EntityProperty.Name domain property.
			/// </summary>
			public sealed override global::System.Guid DomainPropertyId
			{
				[global::System.Diagnostics.DebuggerStepThrough]
				get
				{
					return NameDomainPropertyId;
				}
			}
			
			/// <summary>
			/// Gets a strongly-typed value of the property on specified element.
			/// </summary>
			/// <param name="element">Element which owns the property.</param>
			/// <returns>Property value.</returns>
			public override sealed global::System.String GetValue(EntityProperty element)
			{
				if (element == null) throw new global::System.ArgumentNullException("element");
				return element.namePropertyStorage;
			}
		
			/// <summary>
			/// Sets property value on an element.
			/// </summary>
			/// <param name="element">Element which owns the property.</param>
			/// <param name="newValue">New property value.</param>
			public override sealed void SetValue(EntityProperty element, global::System.String newValue)
			{
				if (element == null) throw new global::System.ArgumentNullException("element");
		
				global::System.String oldValue = GetValue(element);
				if (newValue != oldValue)
				{
					ValueChanging(element, oldValue, newValue);
					element.namePropertyStorage = newValue;
					ValueChanged(element, oldValue, newValue);
				}
			}
		}
		
		#endregion
		#region EntityElement opposite domain role accessor
		/// <summary>
		/// Gets or sets EntityElement.
		/// Description for Company.OrmLanguage.EntityHasProperties.EntityProperty
		/// </summary>
		public virtual EntityElement EntityElement
		{
			[global::System.Diagnostics.DebuggerStepThrough]
			get
			{
				return DslModeling::DomainRoleInfo.GetLinkedElement(this, global::Company.OrmLanguage.EntityHasProperties.EntityPropertyDomainRoleId) as EntityElement;
			}
			[global::System.Diagnostics.DebuggerStepThrough]
			set
			{
				DslModeling::DomainRoleInfo.SetLinkedElement(this, global::Company.OrmLanguage.EntityHasProperties.EntityPropertyDomainRoleId, value);
			}
		}
		#endregion
	}
}
