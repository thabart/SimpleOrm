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
        <DomainProperty Id="3d2a9e27-1fbf-4b35-82f5-aaab3979ccec" Description="Description for Company.OrmLanguage.EntityElement.Name" Name="Name" DisplayName="Name" DefaultValue="New Entity">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="EntityProperty" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>EntityHasProperties.Properties</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="2e887c94-2215-44d4-82cb-d0a069dff0bf" Description="Description for Company.OrmLanguage.EntityProperty" Name="EntityProperty" DisplayName="Entity Property" Namespace="Company.OrmLanguage">
      <Properties>
        <DomainProperty Id="a438b50e-3af8-4af6-8cb3-b69e27870767" Description="Description for Company.OrmLanguage.EntityProperty.Type" Name="Type" DisplayName="Type">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="8a56a53b-6bfc-4bb5-98d9-10a9ebe9e78a" Description="Description for Company.OrmLanguage.EntityProperty.Name" Name="Name" DisplayName="Name" DefaultValue="NewProperty">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
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
    <DomainRelationship Id="bde149c0-a73b-4848-848e-fe0ee2178c3b" Description="Description for Company.OrmLanguage.EntityHasProperties" Name="EntityHasProperties" DisplayName="Entity Has Properties" Namespace="Company.OrmLanguage" IsEmbedding="true">
      <Source>
        <DomainRole Id="2debbe78-c609-44af-b513-f283c93d6d2f" Description="Description for Company.OrmLanguage.EntityHasProperties.EntityElement" Name="EntityElement" DisplayName="Entity Element" PropertyName="Properties" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Properties">
          <RolePlayer>
            <DomainClassMoniker Name="EntityElement" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="1a9b2d73-c741-43a6-b451-d5a99fd9c470" Description="Description for Company.OrmLanguage.EntityHasProperties.EntityProperty" Name="EntityProperty" DisplayName="Entity Property" PropertyName="EntityElement" Multiplicity="ZeroOne" PropagatesDelete="true" PropertyDisplayName="Entity Element">
          <RolePlayer>
            <DomainClassMoniker Name="EntityProperty" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="33832f8d-6221-4dd5-a704-ceaa7c397002" Description="Description for Company.OrmLanguage.EntityHasRelationShips" Name="EntityHasRelationShips" DisplayName="Entity Has Relation Ships" Namespace="Company.OrmLanguage">
      <Source>
        <DomainRole Id="abe224f7-799d-4ffe-b68b-f6d51c0e2409" Description="Description for Company.OrmLanguage.EntityHasRelationShips.SourceEntityElement" Name="SourceEntityElement" DisplayName="Source Entity Element" PropertyName="EntityElements" PropertyDisplayName="Entity Elements">
          <RolePlayer>
            <DomainClassMoniker Name="EntityElement" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="41ce1527-7ae6-4c4a-a392-6cc58e7b5f3f" Description="Description for Company.OrmLanguage.EntityHasRelationShips.TargetEntityElement" Name="TargetEntityElement" DisplayName="Target Entity Element" PropertyName="SourceEntityElement" Multiplicity="ZeroOne" PropertyDisplayName="Source Entity Element">
          <RolePlayer>
            <DomainClassMoniker Name="EntityElement" />
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
          <XmlRelationshipData UseFullForm="true" RoleElementName="properties">
            <DomainRelationshipMoniker Name="EntityHasProperties" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="EntityElement/Name" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="entityElements">
            <DomainRelationshipMoniker Name="EntityHasRelationShips" />
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
      <XmlClassData TypeName="EntityProperty" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityPropertyMoniker" ElementName="entityProperty" MonikerTypeName="EntityPropertyMoniker">
        <DomainClassMoniker Name="EntityProperty" />
        <ElementData>
          <XmlPropertyData XmlName="type">
            <DomainPropertyMoniker Name="EntityProperty/Type" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="EntityProperty/Name" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EntityHasProperties" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityHasPropertiesMoniker" ElementName="entityHasProperties" MonikerTypeName="EntityHasPropertiesMoniker">
        <DomainRelationshipMoniker Name="EntityHasProperties" />
      </XmlClassData>
      <XmlClassData TypeName="EntityHasRelationShips" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityHasRelationShipsMoniker" ElementName="entityHasRelationShips" MonikerTypeName="EntityHasRelationShipsMoniker">
        <DomainRelationshipMoniker Name="EntityHasRelationShips" />
      </XmlClassData>
      <XmlClassData TypeName="EntityHasRelationShipsConnector" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityHasRelationShipsConnectorMoniker" ElementName="entityHasRelationShipsConnector" MonikerTypeName="EntityHasRelationShipsConnectorMoniker">
        <ConnectorMoniker Name="EntityHasRelationShipsConnector" />
      </XmlClassData>
    </ClassData>
  </XmlSerializationBehavior>
  <ExplorerBehavior Name="OrmLanguageExplorer" />
  <ConnectionBuilders>
    <ConnectionBuilder Name="EntityHasRelationShipsBuilder">
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
            <DomainPath>EntityHasProperties.Properties/!EntityProperty</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="EntityProperty/Name" />
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