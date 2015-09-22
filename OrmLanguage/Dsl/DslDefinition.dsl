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
            <DomainClassMoniker Name="Property" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>EntityHasProperties.Properties</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="2e887c94-2215-44d4-82cb-d0a069dff0bf" Description="Description for Company.OrmLanguage.Property" Name="Property" DisplayName="Property" Namespace="Company.OrmLanguage">
      <BaseClass>
        <DomainClassMoniker Name="Entry" />
      </BaseClass>
    </DomainClass>
    <DomainClass Id="b4d2b7dc-de9b-4d6d-bc48-2ceae846f01d" Description="Description for Company.OrmLanguage.Reference" Name="Reference" DisplayName="Reference" Namespace="Company.OrmLanguage">
      <BaseClass>
        <DomainClassMoniker Name="Entry" />
      </BaseClass>
    </DomainClass>
    <DomainClass Id="003edb12-1ee2-400d-8723-7b6c57d9a42c" Description="Description for Company.OrmLanguage.Entry" Name="Entry" DisplayName="Entry" Namespace="Company.OrmLanguage">
      <Properties>
        <DomainProperty Id="d9d4256b-da15-420a-8a1a-152cc074b61b" Description="Description for Company.OrmLanguage.Entry.Name" Name="Name" DisplayName="Name" DefaultValue="New Property">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="2e67e649-d5b8-4e23-8e6f-e8996b0e0b8d" Description="Description for Company.OrmLanguage.Entry.Guid" Name="Guid" DisplayName="Guid">
          <Type>
            <ExternalTypeMoniker Name="/System/Guid" />
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
        <DomainRole Id="1a9b2d73-c741-43a6-b451-d5a99fd9c470" Description="Description for Company.OrmLanguage.EntityHasProperties.Property" Name="Property" DisplayName="Property" PropertyName="EntityElement" Multiplicity="ZeroOne" PropagatesDelete="true" PropertyDisplayName="Entity Element">
          <RolePlayer>
            <DomainClassMoniker Name="Property" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="33832f8d-6221-4dd5-a704-ceaa7c397002" Description="Description for Company.OrmLanguage.EntityHasRelationShips" Name="EntityHasRelationShips" DisplayName="Entity Has Relation Ships" Namespace="Company.OrmLanguage">
      <Properties>
        <DomainProperty Id="2d5672af-c4c7-4508-b19f-2bfb13321664" Description="Description for Company.OrmLanguage.EntityHasRelationShips.From Property" Name="fromProperty" DisplayName="From Property">
          <Type>
            <ExternalTypeMoniker Name="/System/Guid" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="681e85b9-dd7b-4970-985a-2eca85a5d449" Description="Description for Company.OrmLanguage.EntityHasRelationShips.To Property" Name="toProperty" DisplayName="To Property">
          <Type>
            <ExternalTypeMoniker Name="/System/Guid" />
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
        <DomainRole Id="41ce1527-7ae6-4c4a-a392-6cc58e7b5f3f" Description="Description for Company.OrmLanguage.EntityHasRelationShips.TargetEntityElement" Name="TargetEntityElement" DisplayName="Target Entity Element" PropertyName="SourceEntityElement" Multiplicity="ZeroOne" PropertyDisplayName="Source Entity Element">
          <RolePlayer>
            <DomainClassMoniker Name="EntityElement" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="e16ebd6e-090f-418b-9d1f-845d24718934" Description="Description for Company.OrmLanguage.EntityHasReferences" Name="EntityHasReferences" DisplayName="Entity Has References" Namespace="Company.OrmLanguage">
      <Source>
        <DomainRole Id="a028db9b-ed0f-4735-a877-75ea8ea52bd1" Description="Description for Company.OrmLanguage.EntityHasReferences.EntityElement" Name="EntityElement" DisplayName="Entity Element" PropertyName="References" PropertyDisplayName="References">
          <RolePlayer>
            <DomainClassMoniker Name="EntityElement" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="995c88e4-65cc-4a0f-bbf0-10e0385b7ef0" Description="Description for Company.OrmLanguage.EntityHasReferences.Reference" Name="Reference" DisplayName="Reference" PropertyName="EntityElement" Multiplicity="ZeroOne" PropertyDisplayName="Entity Element">
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
  </Types>
  <Shapes>
    <CompartmentShape Id="c4e83e27-330b-483c-9a73-b40e9dbef110" Description="Description for Company.OrmLanguage.EntityShape" Name="EntityShape" DisplayName="Entity Shape" Namespace="Company.OrmLanguage" FixedTooltipText="Entity Shape" InitialHeight="1" Geometry="RoundedRectangle">
      <ShapeHasDecorators Position="InnerTopCenter" HorizontalOffset="0" VerticalOffset="0">
        <TextDecorator Name="Name" DisplayName="Name" DefaultText="Name" />
      </ShapeHasDecorators>
      <Compartment Name="Properties" Title="Properties" />
      <Compartment Name="References" Title="References" />
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
      <XmlClassData TypeName="Property" MonikerAttributeName="" SerializeId="true" MonikerElementName="propertyMoniker" ElementName="property" MonikerTypeName="PropertyMoniker">
        <DomainClassMoniker Name="Property" />
      </XmlClassData>
      <XmlClassData TypeName="EntityHasProperties" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityHasPropertiesMoniker" ElementName="entityHasProperties" MonikerTypeName="EntityHasPropertiesMoniker">
        <DomainRelationshipMoniker Name="EntityHasProperties" />
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
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EntityHasRelationShipsConnector" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityHasRelationShipsConnectorMoniker" ElementName="entityHasRelationShipsConnector" MonikerTypeName="EntityHasRelationShipsConnectorMoniker">
        <ConnectorMoniker Name="EntityHasRelationShipsConnector" />
      </XmlClassData>
      <XmlClassData TypeName="Reference" MonikerAttributeName="" SerializeId="true" MonikerElementName="referenceMoniker" ElementName="reference" MonikerTypeName="ReferenceMoniker">
        <DomainClassMoniker Name="Reference" />
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
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EntityHasReferences" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityHasReferencesMoniker" ElementName="entityHasReferences" MonikerTypeName="EntityHasReferencesMoniker">
        <DomainRelationshipMoniker Name="EntityHasReferences" />
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
    <ConnectionBuilder Name="EntityHasReferencesBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="EntityHasReferences" />
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
              <DomainClassMoniker Name="Reference" />
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
        <CompartmentMap>
          <CompartmentMoniker Name="EntityShape/References" />
          <ElementsDisplayed>
            <DomainPath>EntityHasReferences.References/!Reference</DomainPath>
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