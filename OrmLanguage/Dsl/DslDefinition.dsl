<?xml version="1.0" encoding="utf-8"?>
<Dsl xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="3418fb30-d41c-496d-9cb7-5c76ba66620c" Description="Description for Company.OrmLanguage.OrmLanguage" Name="OrmLanguage" DisplayName="OrmLanguage" Namespace="Company.OrmLanguage" ProductName="OrmLanguage" CompanyName="Company" PackageGuid="673435e5-6a48-4569-b3b6-7a7ca61bb1d2" PackageNamespace="Company.OrmLanguage" xmlns="http://schemas.microsoft.com/VisualStudio/2005/DslTools/DslDefinitionModel">
  <Classes>
    <DomainClass Id="583ad1fe-b130-4778-a99a-ddfa88829f94" Description="The root in which all other elements are embedded. Appears as a diagram." Name="SampleOrmModel" DisplayName="Sample Orm Model" Namespace="Company.OrmLanguage">
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Notes>Creates an embedding link when an element is dropped onto a model. </Notes>
          <Index>
            <DomainClassMoniker Name="EntityElement" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>SampleOrmModelHasElements.Elements</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="10f6f6a8-e10f-47b5-b83e-153c8cfb0322" Description="Elements embedded in the model. Appear as boxes on the diagram." Name="EntityElement" DisplayName="Entity Element" Namespace="Company.OrmLanguage">
      <Properties>
        <DomainProperty Id="3d2a9e27-1fbf-4b35-82f5-aaab3979ccec" Description="Description for Company.OrmLanguage.EntityElement.Name" Name="Name" DisplayName="Name" DefaultValue="New Entity" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Property" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>EntityHasProperties.Properties</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Reference" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>EntityHasReferences.References</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="434fcd74-129b-4049-ba26-b5e60d519a1a" Description="Description for Company.OrmLanguage.Entry" Name="Entry" DisplayName="Entry" InheritanceModifier="Abstract" Namespace="Company.OrmLanguage" HasCustomConstructor="true">
      <Properties>
        <DomainProperty Id="360c5f50-ebef-479d-9b9b-e927a959f892" Description="Description for Company.OrmLanguage.Entry.Name" Name="Name" DisplayName="Name" DefaultValue="New property">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="a0755a71-ef60-4c60-bf65-5bf0d85ef7ec" Description="Description for Company.OrmLanguage.Entry.Guid" Name="Guid" DisplayName="Guid" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Guid" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="33ad3cdc-58fd-4d0d-9681-e2f1ddf55dba" Description="Description for Company.OrmLanguage.Entry.Type" Name="Type" DisplayName="Type">
          <Type>
            <ExternalTypeMoniker Name="/System/TypeCode" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="b9437070-9bf3-4fa0-a37d-0e63d94b2e8e" Description="Description for Company.OrmLanguage.Property" Name="Property" DisplayName="Property" InheritanceModifier="Sealed" Namespace="Company.OrmLanguage">
      <BaseClass>
        <DomainClassMoniker Name="Entry" />
      </BaseClass>
    </DomainClass>
    <DomainClass Id="35492d24-c52d-488e-a334-d32c0850ea5f" Description="Description for Company.OrmLanguage.Reference" Name="Reference" DisplayName="Reference" InheritanceModifier="Sealed" Namespace="Company.OrmLanguage">
      <BaseClass>
        <DomainClassMoniker Name="Entry" />
      </BaseClass>
    </DomainClass>
  </Classes>
  <Relationships>
    <DomainRelationship Id="3a158a50-ceab-48e4-b9f1-5828c849ff59" Description="Embedding relationship between the Model and Elements" Name="SampleOrmModelHasElements" DisplayName="Sample Orm Model Has Elements" Namespace="Company.OrmLanguage" IsEmbedding="true">
      <Source>
        <DomainRole Id="c8a68f81-36d8-4122-bc46-66672074e765" Description="" Name="SampleOrmModel" DisplayName="Sample Orm Model" PropertyName="Elements" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Elements">
          <RolePlayer>
            <DomainClassMoniker Name="SampleOrmModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="ea1dfb1c-6090-4ea8-a509-0a2ada23da8f" Description="" Name="Element" DisplayName="Element" PropertyName="SampleOrmModel" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Sample Orm Model">
          <RolePlayer>
            <DomainClassMoniker Name="EntityElement" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="33832f8d-6221-4dd5-a704-ceaa7c397002" Description="Description for Company.OrmLanguage.EntityHasRelationShips" Name="EntityHasRelationShips" DisplayName="Entity Has Relation Ships" Namespace="Company.OrmLanguage">
      <Properties>
        <DomainProperty Id="2d5672af-c4c7-4508-b19f-2bfb13321664" Description="Description for Company.OrmLanguage.EntityHasRelationShips.From Property" Name="fromProperty" DisplayName="From Property" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Guid" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="681e85b9-dd7b-4970-985a-2eca85a5d449" Description="Description for Company.OrmLanguage.EntityHasRelationShips.To Property" Name="toProperty" DisplayName="To Property" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Guid" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="ede80518-fefd-479b-b5a5-8e4a3896b206" Description="Description for Company.OrmLanguage.EntityHasRelationShips.From Property Name" Name="fromPropertyName" DisplayName="From Property Name">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="4c475f09-94de-4a4b-a23b-8592afecccd4" Description="Description for Company.OrmLanguage.EntityHasRelationShips.To Property Name" Name="toPropertyName" DisplayName="To Property Name">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="abe224f7-799d-4ffe-b68b-f6d51c0e2409" Description="Description for Company.OrmLanguage.EntityHasRelationShips.SourceEntityElement" Name="SourceEntityElement" DisplayName="Source Entity Element" PropertyName="EntityElements" PropertyDisplayName="Entity Elements">
          <RolePlayer>
            <DomainClassMoniker Name="EntityElement" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="41ce1527-7ae6-4c4a-a392-6cc58e7b5f3f" Description="Description for Company.OrmLanguage.EntityHasRelationShips.TargetEntityElement" Name="TargetEntityElement" DisplayName="Target Entity Element" PropertyName="SourceEntityElements" PropertyDisplayName="Source Entity Elements">
          <RolePlayer>
            <DomainClassMoniker Name="EntityElement" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="40169f63-50fa-4880-9004-59db7ce716b7" Description="Description for Company.OrmLanguage.EntityHasProperties" Name="EntityHasProperties" DisplayName="Entity Has Properties" Namespace="Company.OrmLanguage" IsEmbedding="true">
      <Source>
        <DomainRole Id="5b884564-9a0a-4b29-a1ae-6e2c607d60d6" Description="Description for Company.OrmLanguage.EntityHasProperties.EntityElement" Name="EntityElement" DisplayName="Entity Element" PropertyName="Properties" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Properties">
          <RolePlayer>
            <DomainClassMoniker Name="EntityElement" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="18c6b76a-ab2e-4a66-88dc-34a6ba414f74" Description="Description for Company.OrmLanguage.EntityHasProperties.Property" Name="Property" DisplayName="Property" PropertyName="EntityElement" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Entity Element">
          <RolePlayer>
            <DomainClassMoniker Name="Property" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="c9d0517b-47b7-4cc9-abfa-e201641c96c2" Description="Description for Company.OrmLanguage.EntityHasReferences" Name="EntityHasReferences" DisplayName="Entity Has References" Namespace="Company.OrmLanguage" IsEmbedding="true">
      <Source>
        <DomainRole Id="26cc55e6-577e-4505-8882-2bb9babcafd6" Description="Description for Company.OrmLanguage.EntityHasReferences.EntityElement" Name="EntityElement" DisplayName="Entity Element" PropertyName="References" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="References">
          <RolePlayer>
            <DomainClassMoniker Name="EntityElement" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="e02a7c7b-9baf-4115-a5eb-c9c713cc984d" Description="Description for Company.OrmLanguage.EntityHasReferences.Reference" Name="Reference" DisplayName="Reference" PropertyName="EntityElement" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Entity Element">
          <RolePlayer>
            <DomainClassMoniker Name="Reference" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
  </Relationships>
  <Types>
    <ExternalType Name="DateTime" Namespace="System" />
    <ExternalType Name="String" Namespace="System" />
    <ExternalType Name="Int16" Namespace="System" />
    <ExternalType Name="Int32" Namespace="System" />
    <ExternalType Name="Int64" Namespace="System" />
    <ExternalType Name="UInt16" Namespace="System" />
    <ExternalType Name="UInt32" Namespace="System" />
    <ExternalType Name="UInt64" Namespace="System" />
    <ExternalType Name="SByte" Namespace="System" />
    <ExternalType Name="Byte" Namespace="System" />
    <ExternalType Name="Double" Namespace="System" />
    <ExternalType Name="Single" Namespace="System" />
    <ExternalType Name="Guid" Namespace="System" />
    <ExternalType Name="Boolean" Namespace="System" />
    <ExternalType Name="Char" Namespace="System" />
    <ExternalType Name="TypeCode" Namespace="System" />
  </Types>
  <Shapes>
    <CompartmentShape Id="c4e83e27-330b-483c-9a73-b40e9dbef110" Description="Description for Company.OrmLanguage.EntityShape" Name="EntityShape" DisplayName="Entity Shape" Namespace="Company.OrmLanguage" FixedTooltipText="Entity Shape" InitialHeight="1" Geometry="RoundedRectangle">
      <ShapeHasDecorators Position="InnerTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="Name" DisplayName="Name" DefaultText="Name" />
      </ShapeHasDecorators>
      <Compartment Name="Properties" Title="Properties" />
    </CompartmentShape>
  </Shapes>
  <Connectors>
    <Connector Id="f6e2759d-26b8-4d8b-81d3-f33fd8d3086f" Description="Description for Company.OrmLanguage.EntityHasRelationShipsConnector" Name="EntityHasRelationShipsConnector" DisplayName="Entity Has Relation Ships Connector" Namespace="Company.OrmLanguage" FixedTooltipText="Entity Has Relation Ships Connector" DashStyle="Dash" TargetEndStyle="HollowArrow" />
  </Connectors>
  <XmlSerializationBehavior Name="OrmLanguageSerializationBehavior" Namespace="Company.OrmLanguage">
    <ClassData>
      <XmlClassData TypeName="SampleOrmModel" MonikerAttributeName="" SerializeId="true" MonikerElementName="sampleOrmModelMoniker" ElementName="sampleOrmModel" MonikerTypeName="SampleOrmModelMoniker">
        <DomainClassMoniker Name="SampleOrmModel" />
        <ElementData>
          <XmlRelationshipData RoleElementName="elements">
            <DomainRelationshipMoniker Name="SampleOrmModelHasElements" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EntityElement" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityElementMoniker" ElementName="entityElement" MonikerTypeName="EntityElementMoniker">
        <DomainClassMoniker Name="EntityElement" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="EntityElement/Name" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="entityElements">
            <DomainRelationshipMoniker Name="EntityHasRelationShips" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="properties">
            <DomainRelationshipMoniker Name="EntityHasProperties" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="references">
            <DomainRelationshipMoniker Name="EntityHasReferences" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="SampleOrmModelHasElements" MonikerAttributeName="" SerializeId="true" MonikerElementName="sampleOrmModelHasElementsMoniker" ElementName="sampleOrmModelHasElements" MonikerTypeName="SampleOrmModelHasElementsMoniker">
        <DomainRelationshipMoniker Name="SampleOrmModelHasElements" />
      </XmlClassData>
      <XmlClassData TypeName="OrmLanguageDiagram" MonikerAttributeName="" SerializeId="true" MonikerElementName="ormLanguageDiagramMoniker" ElementName="ormLanguageDiagram" MonikerTypeName="OrmLanguageDiagramMoniker">
        <DiagramMoniker Name="OrmLanguageDiagram" />
      </XmlClassData>
      <XmlClassData TypeName="EntityShape" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityShapeMoniker" ElementName="entityShape" MonikerTypeName="EntityShapeMoniker">
        <CompartmentShapeMoniker Name="EntityShape" />
      </XmlClassData>
      <XmlClassData TypeName="EntityHasRelationShips" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityHasRelationShipsMoniker" ElementName="entityHasRelationShips" MonikerTypeName="EntityHasRelationShipsMoniker">
        <DomainRelationshipMoniker Name="EntityHasRelationShips" />
        <ElementData>
          <XmlPropertyData XmlName="fromProperty">
            <DomainPropertyMoniker Name="EntityHasRelationShips/fromProperty" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="toProperty">
            <DomainPropertyMoniker Name="EntityHasRelationShips/toProperty" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="fromPropertyName">
            <DomainPropertyMoniker Name="EntityHasRelationShips/fromPropertyName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="toPropertyName">
            <DomainPropertyMoniker Name="EntityHasRelationShips/toPropertyName" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EntityHasRelationShipsConnector" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityHasRelationShipsConnectorMoniker" ElementName="entityHasRelationShipsConnector" MonikerTypeName="EntityHasRelationShipsConnectorMoniker">
        <ConnectorMoniker Name="EntityHasRelationShipsConnector" />
      </XmlClassData>
      <XmlClassData TypeName="Entry" MonikerAttributeName="" SerializeId="true" MonikerElementName="entryMoniker" ElementName="entry" MonikerTypeName="EntryMoniker">
        <DomainClassMoniker Name="Entry" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="Entry/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="guid">
            <DomainPropertyMoniker Name="Entry/Guid" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="type">
            <DomainPropertyMoniker Name="Entry/Type" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="Property" MonikerAttributeName="" SerializeId="true" MonikerElementName="propertyMoniker" ElementName="property" MonikerTypeName="PropertyMoniker">
        <DomainClassMoniker Name="Property" />
      </XmlClassData>
      <XmlClassData TypeName="Reference" MonikerAttributeName="" SerializeId="true" MonikerElementName="referenceMoniker" ElementName="reference" MonikerTypeName="ReferenceMoniker">
        <DomainClassMoniker Name="Reference" />
      </XmlClassData>
      <XmlClassData TypeName="EntityHasProperties" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityHasPropertiesMoniker" ElementName="entityHasProperties" MonikerTypeName="EntityHasPropertiesMoniker">
        <DomainRelationshipMoniker Name="EntityHasProperties" />
      </XmlClassData>
      <XmlClassData TypeName="EntityHasReferences" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityHasReferencesMoniker" ElementName="entityHasReferences" MonikerTypeName="EntityHasReferencesMoniker">
        <DomainRelationshipMoniker Name="EntityHasReferences" />
      </XmlClassData>
    </ClassData>
  </XmlSerializationBehavior>
  <ExplorerBehavior Name="OrmLanguageExplorer" />
  <ConnectionBuilders>
    <ConnectionBuilder Name="EntityHasRelationShipsBuilder" IsCustom="true">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="EntityHasRelationShips" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="EntityElement" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="EntityElement" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
  </ConnectionBuilders>
  <Diagram Id="c3303623-5860-4dd8-9d94-f791d26e5e29" Description="Description for Company.OrmLanguage.OrmLanguageDiagram" Name="OrmLanguageDiagram" DisplayName="Minimal Language Diagram" Namespace="Company.OrmLanguage">
    <Class>
      <DomainClassMoniker Name="SampleOrmModel" />
    </Class>
    <ShapeMaps>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="EntityElement" />
        <ParentElementPath>
          <DomainPath>SampleOrmModelHasElements.SampleOrmModel/!SampleOrmModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="EntityShape/Name" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="EntityElement/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="EntityShape" />
        <CompartmentMap>
          <CompartmentMoniker Name="EntityShape/Properties" />
          <ElementsDisplayed>
            <DomainPath>EntityHasProperties.Properties/!Property</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Entry/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
      </CompartmentShapeMap>
    </ShapeMaps>
    <ConnectorMaps>
      <ConnectorMap>
        <ConnectorMoniker Name="EntityHasRelationShipsConnector" />
        <DomainRelationshipMoniker Name="EntityHasRelationShips" />
      </ConnectorMap>
    </ConnectorMaps>
  </Diagram>
  <Designer CopyPasteGeneration="CopyPasteOnly" FileExtension="simpleorm" EditorGuid="b7394ff9-2081-4b2d-bcaf-43e465f31314">
    <RootClass>
      <DomainClassMoniker Name="SampleOrmModel" />
    </RootClass>
    <XmlSerializationDefinition CustomPostLoad="false">
      <XmlSerializationBehaviorMoniker Name="OrmLanguageSerializationBehavior" />
    </XmlSerializationDefinition>
    <ToolboxTab TabText="OrmLanguage">
      <ElementTool Name="Entity" ToolboxIcon="resources\exampleshapetoolbitmap.bmp" Caption="Entity" Tooltip="Create an ExampleElement" HelpKeyword="CreateExampleClassF1Keyword">
        <DomainClassMoniker Name="EntityElement" />
      </ElementTool>
      <ConnectionTool Name="EntityRelationShip" ToolboxIcon="resources\exampleconnectortoolbitmap.bmp" Caption="EntityRelationShip" Tooltip="Entity Relation Ship" HelpKeyword="EntityRelationShip">
        <ConnectionBuilderMoniker Name="OrmLanguage/EntityHasRelationShipsBuilder" />
      </ConnectionTool>
    </ToolboxTab>
    <Validation UsesMenu="false" UsesOpen="false" UsesSave="false" UsesLoad="false" />
    <DiagramMoniker Name="OrmLanguageDiagram" />
  </Designer>
  <Explorer ExplorerGuid="fc64633f-a0d4-45dc-a5c2-924fcfb0e9e1" Title="OrmLanguage Explorer">
    <ExplorerBehaviorMoniker Name="OrmLanguage/OrmLanguageExplorer" />
  </Explorer>
</Dsl>