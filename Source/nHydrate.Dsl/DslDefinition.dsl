<?xml version="1.0" encoding="utf-8"?>
<Dsl xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="7a314716-48c9-4371-8978-062be635f9b4" Description="This is the nHydrate Visual Modeler" Name="nHydrate" DisplayName="nHydrate ORM Modeler" Namespace="nHydrate.Dsl" MajorVersion="7" Build="1" Revision="234" ProductName="nHydrate ORM Modeler" CompanyName="nHydrate.org" PackageGuid="36220dab-63c7-4daa-860c-fc548bf4d5d3" PackageNamespace="nHydrate.DslPackage" xmlns="http://schemas.microsoft.com/VisualStudio/2005/DslTools/DslDefinitionModel">
  <Notes>This integrated VS.NET component creates strongly-typed, extendable classes inside of a framework based on Entity Framework.</Notes>
  <Classes>
    <DomainClass Id="77b5fe81-853a-4b74-8ce5-98612544852f" Description="" Name="nHydrateModel" DisplayName="nHydrate Model" Namespace="nHydrate.Dsl" HasCustomConstructor="true" GeneratesDoubleDerived="true">
      <Properties>
        <DomainProperty Id="e4a7289c-e61c-440e-b881-5b06950fd6f0" Description="Specifies the company name that will be used to build namespaces" Name="CompanyName" DisplayName="Company Name" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="878ea856-ee97-4991-bb04-941062bbea33" Description="Determines the default namespace and base project names of all generated projects. Leave blank for the default value of CompanyName.ProjectName" Name="DefaultNamespace" DisplayName="Default Namespace" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="6aed7802-72a5-4336-9dd3-fad46ec05759" Description="Specifies the name of the generated assembly" Name="ProjectName" DisplayName="Project Name" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="5be892eb-5e4f-4065-ad4a-ee6b1c65c8d4" Description="Specifies whether UTC or local time is used for the created and modified audits" Name="UseUTCTime" DisplayName="Use UTCTime" DefaultValue="false" Category="Definition" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="0d7940fc-1697-4d8a-957a-6479d9710a87" Description="Specifies the version number of the generated assembly" Name="Version" DisplayName="Version" DefaultValue="0.0.0.0" Category="Definition">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(nHydrate.Dsl.Design.Converters.VersionConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="efe34335-275d-4e98-aa77-2c72387ca5f2" Description="Determines the name of the created by field" Name="CreatedByColumnName" DisplayName="Created By Column Name" DefaultValue="CreatedBy" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b33e6b20-f095-403e-b0a4-72deb6a014d3" Description="Determines the name of the created date field" Name="CreatedDateColumnName" DisplayName="Created Date Column Name" DefaultValue="CreatedDate" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="c9898612-ec74-4afc-904a-fab3e0b7a132" Description="Determines the name of the modified date field" Name="ModifiedDateColumnName" DisplayName="Modified Date Column Name" DefaultValue="ModifiedDate" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="c3940874-169f-44ff-926e-680febb2c8f1" Description="Determines the name of the modified by field" Name="ModifiedByColumnName" DisplayName="Modified By Column Name" DefaultValue="ModifiedBy" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="eaf7be21-e695-48b8-96b3-e1ace1139aab" Description="Determines the name of the timestamp column" Name="TimestampColumnName" DisplayName="Timestamp Column Name" DefaultValue="Timestamp" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="04550abb-3bfc-4787-9064-85bfbe9cd48e" Description="Determines the database user to grant access permissions to for database objects" Name="GrantUser" DisplayName="Grant User" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="6fbe9eae-bebb-4ede-b0df-3f8ea4153633" Description="Determines if model objects are duplicated on disk for easy editing" Name="ModelToDisk" DisplayName="Model To Disk" DefaultValue="false" Category="Behavior" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="cfac3b29-311d-4da5-8d93-bb442283b51f" Description="Determines the version of the model. Used for tracking changes and provides an upgrade path for older models" Name="ModelVersion" DisplayName="Model Version" DefaultValue="" Category="Definition" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="be71ee12-7759-4612-930e-e53430febfb8" Description="The target location for generated projects" Name="OutputTarget" DisplayName="Output Target" Category="Definition" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="8d907fa0-9318-41e3-8fcf-d9e154112597" Description="Determines the prefix for generated views that map to tenant tables" Name="TenantPrefix" DisplayName="Tenant Prefix" DefaultValue="__vw_tenant" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="fd3902ce-3044-47e2-a379-885c8e1a1a4d" Description="Determines the name of the column to hold tenant information for tenant tables" Name="TenantColumnName" DisplayName="Tenant Column Name" DefaultValue="__tenant_user" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="f348a2a7-4cf4-440c-b0b5-75be15bf6dde" Description="Determines if normalization safety scripts are emitted into the installer" Name="EmitSafetyScripts" DisplayName="Emit Safety Scripts" DefaultValue="true" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="bca19c13-0e3b-4ecf-9627-a50367e1aee4" Description="Determines if change events are generated around all entity property setters" Name="EmitChangeScripts" DisplayName="Emit Change Scripts" DefaultValue="true" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Notes>Creates an embedding link when an element is dropped onto a model. </Notes>
          <Index>
            <DomainClassMoniker Name="Entity" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>nHydrateModelHasEntities.Entities</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="View" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>nHydrateModelHasViews.Views</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective UsesCustomAccept="true">
          <Index>
            <DomainClassMoniker Name="RelationField" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>nHydrateModelHasRelationFields.RelationFields</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="99e3c3f3-d9f6-4226-b914-b50a93434e4f" Description="This object represents a database table" Name="Entity" DisplayName="Entity" Namespace="nHydrate.Dsl" GeneratesDoubleDerived="true">
      <Properties>
        <DomainProperty Id="767a392d-c773-430a-abbb-e321993fc5de" Description="Determines the name of this entity" Name="Name" DisplayName="Name" DefaultValue="" Category="Definition" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="542415d5-c314-471a-a484-1186ae40cd25" Description="A summary of the entity" Name="Summary" DisplayName="Summary" Category="Documentation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e56a3e68-5513-499e-a933-5110057d2b1f" Description="Determines if the fields 'CreatedBy' and 'CreateDate' are created" Name="AllowCreateAudit" DisplayName="Allow Create Audit" DefaultValue="true" Category="Audit">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="50a4b7c2-6a8e-44f3-bb0b-fcbf1f51f855" Description="Determines if the fields 'ModifiedBy' and 'ModifiedDate' are created" Name="AllowModifyAudit" DisplayName="Allow Modify Audit" DefaultValue="true" Category="Audit">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="9fc4b9f7-6d29-431c-8f71-6ac6a74da370" Description="Determines if this table will have a timestamp field created and used for synchronization" Name="AllowTimestamp" DisplayName="Allow Timestamp" DefaultValue="true" Category="Audit">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="06d09cfc-31af-465a-aa16-f6eae978dc92" Description="Determines if this is an intermediary entity between two other entities" Name="IsAssociative" DisplayName="Is Associative" DefaultValue="false" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="7b18441c-79a6-4b6e-9d2a-b155eb75dbe5" Description="Determines if this entity can be changed" Name="Immutable" DisplayName="Immutable" DefaultValue="false" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b6cd65f4-36aa-4ac5-b627-43c85f047626" Description="The database schema in which this entity lives" Name="Schema" DisplayName="Schema" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="df8974d7-0da4-49ab-b208-1e5aca96cf6b" Description="Determines the object name used in the API. If this property is blank the 'Name' property is used in the API. This property can be used to mask the database identifier." Name="CodeFacade" DisplayName="Code Facade" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="a597dfa1-fc68-43e9-856f-2ef82291ea30" Description="If True, will generate both a base class with all functionality and a partial class to support customization through overrides" Name="GeneratesDoubleDerived" DisplayName="Double Derived" DefaultValue="false" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="4ecb38e6-dacf-439a-8471-72634105dc80" Description="Determines if this is a typed entity" Name="TypedEntity" DisplayName="Typed Entity" DefaultValue="None" Category="Definition">
          <Type>
            <DomainEnumerationMoniker Name="TypedEntityConstants" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="ec18a590-98a8-4f16-a4c0-adac3377eb56" Description="Determines if the table is tenant based" Name="IsTenant" DisplayName="Is Tenant" DefaultValue="false" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e3468fb2-910f-48e4-8ad7-7dbe952c4c5b" Description="Description for nHydrate.Dsl.Entity.Copy State Info" Name="CopyStateInfo" DisplayName="Copy State Info" Kind="CustomStorage" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Field" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>EntityHasFields.Fields</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective UsesCustomAccept="true">
          <Index>
            <DomainClassMoniker Name="StaticData" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>EntityHasStaticDatum.StaticDatum</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Index" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>EntityHasIndexes.Indexes</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="6e74a55a-de35-41e1-b542-908a20112a5d" Description="This is a field in a database table" Name="Field" DisplayName="Field" Namespace="nHydrate.Dsl" HasCustomConstructor="true" GeneratesDoubleDerived="true">
      <Properties>
        <DomainProperty Id="f7eaaafb-addf-4cae-8926-b8da16de5e62" Description="Determines the name of this field" Name="Name" DisplayName="Name" DefaultValue="" Category="Definition" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="f990fadc-0b79-46b2-8afc-b83182fbe1cc" Description="Determines if this item allows null values" Name="Nullable" DisplayName="Nullable" DefaultValue="true" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="9c86b99a-81ab-4d2d-80f5-dfcba41f42be" Description="Determines if this field is based on a calculated database column" Name="IsCalculated" DisplayName="Is Calculated" DefaultValue="false" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="accd89d9-66da-4ff0-8d2e-a2e8d7ff7a6c" Description="Determines the data type of this field" Name="DataType" DisplayName="Datatype" DefaultValue="VarChar" Category="Definition">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(nHydrate.Dsl.Design.Converters.DatatypeConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <DomainEnumerationMoniker Name="DataTypeConstants" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="78416a86-37eb-4d7a-a39e-e2311c32e145" Description="Determines the default value of this field" Name="Default" DisplayName="Default" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="ac63ac37-d989-4ca7-a4cd-93d7bd06fb26" Description="Determines summary text were applicable" Name="Summary" DisplayName="Summary" Category="Documentation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="3dcfa810-4d71-4d08-af7d-e3f523d8fea6" Description="The formula for a computed field" Name="Formula" DisplayName="Formula" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="29e9a228-046e-468d-930d-ebcaac1c0418" Description="Determines the type of identity for this column" Name="Identity" DisplayName="Identity" DefaultValue="None" Category="Definition">
          <Type>
            <DomainEnumerationMoniker Name="IdentityTypeConstants" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="d5ad0098-1d8a-4873-a370-e260a204af50" Description="Determines if this field has an associated database index" Name="IsIndexed" DisplayName="Is Indexed" DefaultValue="false" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="d99371ef-dd34-4b20-b5ba-de159603982c" Description="Determines if this field is marked as unique" Name="IsUnique" DisplayName="Is Unique" DefaultValue="false" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="84851d5c-4f27-4464-8a10-2219088608e2" Description="Determines the size of this column in bytes" Name="Length" DisplayName="Length" DefaultValue="50" Category="Definition">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(nHydrate.Dsl.Design.Converters.TextLengthConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/Int32" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="658312f1-4067-4791-9c8e-818d086c108e" Description="Determine if this field is the entity primary key" Name="IsPrimaryKey" DisplayName="Is Primary Key" DefaultValue="false" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="6ebe23fb-3b81-433b-a878-0cfd9d4aa4a5" Description="Determines the scale of some data types" Name="Scale" DisplayName="Scale" Category="Definition">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(nHydrate.Dsl.Design.Converters.TextDecimalScaleConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/Int32" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="6bb3c3d0-f62d-46cb-90c6-9e64709cb13a" Description="Determines the object name used in the API. If this property is blank the 'Name' property is used in the API. This property can be used to mask the database identifier." Name="CodeFacade" DisplayName="Code Facade" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="232598d5-1d67-46f0-8f5f-6357b90eaa38" Description="Determines if the property can be set in code" Name="IsReadOnly" DisplayName="Is Read Only" DefaultValue="false" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="2078ed80-ab07-48a9-826b-bd2fe6942a70" Description="Order Entered/Database order" Name="SortOrder" DisplayName="Sort Order" DefaultValue="0" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Int32" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="a9a73844-38f5-4261-bd5c-aa8b863bab7b" Description="Identifies the format string for data input and presentation" Name="DataFormatString" DisplayName="Data Format String" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="46c0c3a4-3a50-41c2-8493-c852ec9a9e41" Description="Determines if the default value is a function" Name="DefaultIsFunc" DisplayName="Default Is Func" DefaultValue="false" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="3122c386-bdd3-49ff-a1ef-40aac8b4803d" Description="The imported database default name" Name="ImportedDefaultName" DisplayName="Imported Default Name" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e798fdc7-2666-435a-bf56-5f4d1613974e" Description="Determines if this property should create a compiler warning" Name="Obsolete" DisplayName="Obsolete" DefaultValue="false" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="fcb76b2a-6488-4578-a99b-451bd16ff1b8" Description="This is a custom database view" Name="View" DisplayName="View" Namespace="nHydrate.Dsl" GeneratesDoubleDerived="true">
      <Properties>
        <DomainProperty Id="1b837a3a-d7a9-4fe4-897b-f0b69b89f64e" Description="Determines SQL statement used to create the database view object" Name="SQL" DisplayName="SQL" Category="Definition">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(nHydrate.Dsl.Design.Editors.SQLEditor), typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="270471bd-00cc-4dae-b6d2-0253f42708a4" Description="Determines the name of this object" Name="Name" DisplayName="Name" Category="Definition" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b955fb42-785a-4b07-80fd-9b5dc816cb14" Description="Determines the object name used in the API. If this property is blank the 'Name' property is used in the API. This property can be used to mask the database identifier." Name="CodeFacade" DisplayName="Code Facade" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="794adbbd-5a98-4ee5-b22c-77c516856761" Description="Determines the summary of this object" Name="Summary" DisplayName="Summary" Category="Documentation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="63f73103-5ced-4d51-9c6c-148806711162" Description="Determines the parent schema for this object" Name="Schema" DisplayName="Schema" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="dfeb167d-e6f0-4275-a826-83ac314d814f" Description="If True, will generate both a base class with all functionality and a partial class to support customization through overrides" Name="GeneratesDoubleDerived" DisplayName="Generates Double Derived" DefaultValue="false" Category="Code" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="ViewField" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>ViewHasFields.Fields</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="84ea79b9-c696-4742-873b-6d31ce2ae2fb" Description="" Name="ViewField" DisplayName="Field" Namespace="nHydrate.Dsl" HasCustomConstructor="true" GeneratesDoubleDerived="true">
      <Properties>
        <DomainProperty Id="893f6809-7dc3-47ab-8afd-8f3b0b0df7aa" Description="Determines the name of this object" Name="Name" DisplayName="Name" DefaultValue="" Category="Definition" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="93151f66-2435-4f5a-8b8b-0b57ea65e12e" Description="Determines if this item allows null values" Name="Nullable" DisplayName="Nullable" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="afc810ea-82b0-4d4c-ba44-4fdc3c2e28c4" Description="Determines the data type of this field" Name="DataType" DisplayName="Datatype" DefaultValue="VarChar" Category="Definition">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(nHydrate.Dsl.Design.Converters.DatatypeConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <DomainEnumerationMoniker Name="DataTypeConstants" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="53ad9f4d-d0c1-4aaa-b549-0b73a47dd983" Description="Determines the default value of this object" Name="Default" DisplayName="Default" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="da986d57-440b-4fbb-af35-cb5c1038f69b" Description="Determines the summary of this object" Name="Summary" DisplayName="Summary" Category="Documentation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="6ab91e23-be5f-4ff9-b197-edc5ca1ea970" Description="Determines the size of this field in bytes" Name="Length" DisplayName="Length" DefaultValue="50" Category="Definition">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(nHydrate.Dsl.Design.Converters.TextLengthConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/Int32" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="2ea4b05a-3fa9-4cee-895e-57cdf00579f7" Description="Determines the scale of some data types" Name="Scale" DisplayName="Scale" Category="Definition">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(nHydrate.Dsl.Design.Converters.TextDecimalScaleConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/Int32" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="7afe7e0d-867c-4db3-aae3-adb3f4bd848e" Description="Determines the object name used in the API. If this property is blank the 'Name' property is used in the API. This property can be used to mask the database identifier." Name="CodeFacade" DisplayName="Code Facade" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="19ed1bb4-66f1-40a0-9395-53218e4dbe17" Description="Determine if this field is the entity primary key" Name="IsPrimaryKey" DisplayName="Is Primary Key" DefaultValue="false" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="fddfb133-1223-4aed-8861-2d4c0553e79a" Description="" Name="RelationField" DisplayName="Relation Field" Namespace="nHydrate.Dsl">
      <Properties>
        <DomainProperty Id="babfbaaa-ebc3-4b53-9ed8-5e7b5a2cd6a0" Description="" Name="SourceFieldId" DisplayName="Source Field Id" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Guid" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="34a6d2cf-fc80-4e78-9599-bd82161752c1" Description="" Name="TargetFieldId" DisplayName="Target Field Id" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Guid" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="dae4f7d7-b925-4215-9dec-d81182a744a4" Description="" Name="RelationID" DisplayName="Relation ID" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Guid" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="30d62f0d-4b1e-4c95-b75b-c2b775dd98a3" Description="" Name="StaticData" DisplayName="Static Data" Namespace="nHydrate.Dsl">
      <Properties>
        <DomainProperty Id="268d4928-ffa6-46ca-9558-b2a28b04f8c0" Description="The column identifier for the field value" Name="ColumnKey" DisplayName="Column Key" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Guid" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="510965ae-aa88-4c1f-a80b-bab422003178" Description="The value for this column in the current record" Name="Value" DisplayName="Value" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b57e8a0c-dc79-41ef-8835-9115b5cb66f0" Description="The record number [1..N]" Name="OrderKey" DisplayName="Order Key" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Int32" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="ced4ceba-e07d-4396-b2fe-a981c60933d0" Description="A defined index for an entity" Name="Index" DisplayName="Index" Namespace="nHydrate.Dsl" HasCustomConstructor="true" GeneratesDoubleDerived="true">
      <Properties>
        <DomainProperty Id="a8689c72-62db-4c31-8f4e-a65b2ed97cea" Description="" Name="ParentEntityID" DisplayName="Parent Entity" Category="Definition" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Guid" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="9aa19f49-a18d-4c03-9b2c-88a91505f4be" Description="Determines if this index is unique" Name="IsUnique" DisplayName="Is Unique" DefaultValue="false" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="9d97280c-8b06-435d-9a01-e7e34c72cf2a" Description="Determines the summary of this object" Name="Summary" DisplayName="Summary" Category="Documentation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e48ef865-b8c9-461e-8974-4fb86a0f7284" Description="The columns in the index" Name="Definition" DisplayName="Definition" Category="Definition" SetterAccessModifier="Private" IsBrowsable="false" IsUIReadOnly="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="071e2916-81d3-4102-a4ab-cee01881889b" Description="" Name="IndexType" DisplayName="Index Type" DefaultValue="User" IsBrowsable="false">
          <Type>
            <DomainEnumerationMoniker Name="IndexTypeConstants" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b581adbf-990b-4bdf-86a8-0560d26064c0" Description="The original name imported from database" Name="ImportedName" DisplayName="Imported Name" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="075c5cd6-2db6-4112-914b-645de3f7507a" Description="Create this a a clustered index" Name="Clustered" DisplayName="Clustered" DefaultValue="false" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="IndexColumn" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>IndexHasIndexColumns.IndexColumns</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="06e94c72-d866-4c92-b5a9-624fdbebdf9e" Description="" Name="IndexColumn" DisplayName="Index Column" Namespace="nHydrate.Dsl" HasCustomConstructor="true" GeneratesDoubleDerived="true">
      <Properties>
        <DomainProperty Id="a34ec5ca-d4d4-4fc6-8938-473c73de4998" Description="The referenced entity field" Name="FieldID" DisplayName="Field" Category="Definition">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(nHydrate.Dsl.Design.Editors.EntityFieldEditor), typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(nHydrate.Dsl.Design.Converters.EntityFieldConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/Guid" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="d203b5ed-1826-4ce0-ab66-0b9c0b65d0fb" Description="Determines if this column is indexed in ascending order" Name="Ascending" DisplayName="Ascending" DefaultValue="true" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="390d4ff4-e3d9-4ca5-92a4-c410f4808bbb" Description="" Name="Definition" DisplayName="Definition" SetterAccessModifier="Private" IsBrowsable="false" IsUIReadOnly="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="87394d79-86a1-4e18-935c-29793df45f39" Description="" Name="SortOrder" DisplayName="Sort Order" DefaultValue="0" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Int32" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
  </Classes>
  <Relationships>
    <DomainRelationship Id="cd23608d-a10a-4e2e-9ff7-0897d5701618" Description="" Name="nHydrateModelHasEntities" DisplayName="nHydrate Model Has Elements" Namespace="nHydrate.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="acfa23ed-c266-409f-afcc-9cd96d6f8ffc" Description="" Name="nHydrateModel" DisplayName="nHydrate Model" PropertyName="Entities" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Entities">
          <RolePlayer>
            <DomainClassMoniker Name="nHydrateModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="88e9e580-082f-4a9f-a8f5-b29421d8d56a" Description="" Name="Entity" DisplayName="Entity" PropertyName="nHydrateModel" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="nHydrate Model">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="3ee482ac-9232-4953-b839-df2253d00079" Description="Association relationship between entities" Name="EntityHasEntities" DisplayName="Entity Has Entities" Namespace="nHydrate.Dsl" HasCustomConstructor="true" GeneratesDoubleDerived="true" AllowsDuplicates="true">
      <Properties>
        <DomainProperty Id="1d1d755e-c8eb-4e62-81dc-d60cd0a128db" Description="Determines the multiplicity of this relationship" Name="Multiplicity" DisplayName="Multiplicity" DefaultValue="OneToMany" Category="Definition" IsBrowsable="false">
          <Type>
            <DomainEnumerationMoniker Name="RelationshipTypeConstants" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="9b6d5ca2-a1ad-47dc-a1db-889888cb6a0e" Description="The named relation necessary when there is more than one relation between two entities" Name="RoleName" DisplayName="Role Name" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="483720f5-89eb-40a2-8d50-19e59a9c0eb2" Description="Determines if this relationship is enfored in the database or just in code" Name="IsEnforced" DisplayName="Is Enforced" DefaultValue="true" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="0f07c726-427f-4fe7-81c3-9cc64c8f4d92" Description="Internal data to track imports" Name="ImportData" DisplayName="Import Data" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="4faca3f2-9fc5-48e1-bfb5-dc374da9800d" Description="Determines summary text were applicable" Name="Summary" DisplayName="Summary" Category="Documentation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(System.ComponentModel.Design.MultilineStringEditor), typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="12b5d330-0160-43eb-b1ca-94f84781fa3c" Description="The imported database constraint name" Name="ImportedConstraintName" DisplayName="Imported Constraint Name" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="64d1611c-a22b-40c1-a3b0-9b55d59149f6" Description="Determines the action on chld objects when principal entity is deleted" Name="DeleteAction" DisplayName="Delete Action" DefaultValue="NoAction">
          <Type>
            <DomainEnumerationMoniker Name="DeleteActionConstants" />
          </Type>
        </DomainProperty>
      </Properties>
      <Source>
        <DomainRole Id="6b449ed7-d557-4185-aca4-44921368b041" Description="" Name="ParentEntity" DisplayName="Parent Entity" PropertyName="ChildEntities" PropertyDisplayName="Child Entities">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="c4467b71-2045-467c-b568-0b5344ff06da" Description="" Name="ChildEntity" DisplayName="Child Entity" PropertyName="ParentEntity" PropertyDisplayName="Parent Entity">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="a66a58d9-2d27-44e9-8e41-4bc640a0452e" Description="" Name="EntityHasFields" DisplayName="Entity Has Fields" Namespace="nHydrate.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="cccabbdc-2d1f-4d73-b812-7bf88b24374d" Description="" Name="Entity" DisplayName="Entity" PropertyName="Fields" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Fields">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="f276b3f4-bdf3-42e2-ad29-f6774b6e3f25" Description="" Name="Field" DisplayName="Field" PropertyName="Entity" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Entity">
          <RolePlayer>
            <DomainClassMoniker Name="Field" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="4d1b0533-9248-40f1-aadc-09d8af388a29" Description="" Name="nHydrateModelHasViews" DisplayName="NHydrate Model Has Views" Namespace="nHydrate.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="e99ffbdf-da6c-46a3-ab93-f02ac9018d5c" Description="" Name="nHydrateModel" DisplayName="nHydrate Model" PropertyName="Views" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Views">
          <RolePlayer>
            <DomainClassMoniker Name="nHydrateModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="b16fb4a4-410e-4d49-bcea-0880eae5fba0" Description="" Name="View" DisplayName="View" PropertyName="nHydrateModel" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="nHydrate Model">
          <RolePlayer>
            <DomainClassMoniker Name="View" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="d33ecf57-1d21-4b06-8b2b-9e9d686e2be2" Description="" Name="ViewHasFields" DisplayName="View Has Fields" Namespace="nHydrate.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="5788dafc-4131-4e61-babf-c9130479fdc7" Description="" Name="View" DisplayName="View" PropertyName="Fields" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Fields">
          <RolePlayer>
            <DomainClassMoniker Name="View" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="133d0662-8715-4686-832e-84739f6d918a" Description="" Name="ViewField" DisplayName="View Field" PropertyName="View" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="View">
          <RolePlayer>
            <DomainClassMoniker Name="ViewField" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="72b736b5-132f-4d2c-89b6-23b3d3acd5b3" Description="" Name="nHydrateModelHasRelationFields" DisplayName="nHydrate Model Has Relation Fields" Namespace="nHydrate.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="48bbb21d-2233-4bbe-a0f3-7979ba27122d" Description="" Name="nHydrateModel" DisplayName="nHydrate Model" PropertyName="RelationFields" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" IsPropertyBrowsable="false" PropertyDisplayName="Relation Fields">
          <RolePlayer>
            <DomainClassMoniker Name="nHydrateModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="74e8ccb4-6cc4-4874-99be-1f93f17ecdc1" Description="" Name="RelationField" DisplayName="Relation Field" PropertyName="nHydrateModel" Multiplicity="One" PropagatesDelete="true" IsPropertyBrowsable="false" PropertyDisplayName="nHydrate Model">
          <RolePlayer>
            <DomainClassMoniker Name="RelationField" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="b1472bb7-1e34-4d07-b001-d36410e89265" Description="" Name="EntityHasStaticDatum" DisplayName="Entity Has Static Datum" Namespace="nHydrate.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="8f45174c-512a-489a-8402-13b6eb560be3" Description="" Name="Entity" DisplayName="Entity" PropertyName="StaticDatum" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" IsPropertyBrowsable="false" PropertyDisplayName="Static Datum">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="5c59db95-cf44-4553-a296-84ce045296c5" Description="" Name="StaticData" DisplayName="Static Data" PropertyName="Entity" Multiplicity="One" PropagatesDelete="true" IsPropertyBrowsable="false" PropertyDisplayName="Entity">
          <RolePlayer>
            <DomainClassMoniker Name="StaticData" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="1caea208-6608-47d1-a106-efb54a86e7c4" Description="" Name="EntityHasIndexes" DisplayName="Entity Has Indexes" Namespace="nHydrate.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="7c69ce01-ab6b-4507-bedd-fa784b25a493" Description="" Name="Entity" DisplayName="Entity" PropertyName="Indexes" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Indexes">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="c33e52e0-17e7-44c7-898b-75263962edf6" Description="" Name="Index" DisplayName="Index" PropertyName="Entity" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Entity">
          <RolePlayer>
            <DomainClassMoniker Name="Index" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="f50821c9-7d23-4ae4-a1f0-1051f9ae551b" Description="Description for nHydrate.Dsl.IndexHasIndexColumns" Name="IndexHasIndexColumns" DisplayName="Index Has Index Columns" Namespace="nHydrate.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="48eb9827-ed57-4196-814f-efe6045bd213" Description="Description for nHydrate.Dsl.IndexHasIndexColumns.Index" Name="Index" DisplayName="Index" PropertyName="IndexColumns" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Index Columns">
          <RolePlayer>
            <DomainClassMoniker Name="Index" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="2312ab11-6be2-4b81-8c39-7e788ee1fdcb" Description="Description for nHydrate.Dsl.IndexHasIndexColumns.IndexColumn" Name="IndexColumn" DisplayName="Index Column" PropertyName="Index" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Index">
          <RolePlayer>
            <DomainClassMoniker Name="IndexColumn" />
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
    <DomainEnumeration Name="DataTypeConstants" Namespace="nHydrate.Dsl" Description="">
      <Literals>
        <EnumerationLiteral Description="" Name="BigInt" Value="" />
        <EnumerationLiteral Description="" Name="Binary" Value="" />
        <EnumerationLiteral Description="" Name="Bit" Value="" />
        <EnumerationLiteral Description="" Name="Char" Value="" />
        <EnumerationLiteral Description="" Name="Date" Value="" />
        <EnumerationLiteral Description="" Name="DateTime" Value="" />
        <EnumerationLiteral Description="" Name="DateTime2" Value="" />
        <EnumerationLiteral Description="" Name="DateTimeOffset" Value="" />
        <EnumerationLiteral Description="" Name="Decimal" Value="" />
        <EnumerationLiteral Description="" Name="Float" Value="" />
        <EnumerationLiteral Description="" Name="Image" Value="" />
        <EnumerationLiteral Description="" Name="Int" Value="" />
        <EnumerationLiteral Description="" Name="Money" Value="" />
        <EnumerationLiteral Description="" Name="NChar" Value="" />
        <EnumerationLiteral Description="" Name="NText" Value="" />
        <EnumerationLiteral Description="" Name="NVarChar" Value="" />
        <EnumerationLiteral Description="" Name="Real" Value="" />
        <EnumerationLiteral Description="" Name="SmallDateTime" Value="" />
        <EnumerationLiteral Description="" Name="SmallInt" Value="" />
        <EnumerationLiteral Description="" Name="SmallMoney" Value="" />
        <EnumerationLiteral Description="" Name="Structured" Value="" />
        <EnumerationLiteral Description="" Name="Text" Value="" />
        <EnumerationLiteral Description="" Name="Time" Value="" />
        <EnumerationLiteral Description="" Name="Timestamp" Value="" />
        <EnumerationLiteral Description="" Name="TinyInt" Value="" />
        <EnumerationLiteral Description="" Name="Udt" Value="" />
        <EnumerationLiteral Description="" Name="UniqueIdentifier" Value="" />
        <EnumerationLiteral Description="" Name="VarBinary" Value="" />
        <EnumerationLiteral Description="" Name="VarChar" Value="" />
        <EnumerationLiteral Description="" Name="Variant" Value="" />
        <EnumerationLiteral Description="" Name="Xml" Value="" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="RelationshipTypeConstants" Namespace="nHydrate.Dsl" Description="">
      <Literals>
        <EnumerationLiteral Description="" Name="OneToOne" Value="" />
        <EnumerationLiteral Description="" Name="OneToMany" Value="" />
        <EnumerationLiteral Description="" Name="ManyToMany" Value="" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="IdentityTypeConstants" Namespace="nHydrate.Dsl" Description="">
      <Literals>
        <EnumerationLiteral Description="" Name="None" Value="" />
        <EnumerationLiteral Description="" Name="Database" Value="" />
        <EnumerationLiteral Description="" Name="Code" Value="" />
      </Literals>
    </DomainEnumeration>
    <ExternalType Name="Color" Namespace="System.Drawing" />
    <ExternalType Name="DashStyle" Namespace="System.Drawing.Drawing2D" />
    <ExternalType Name="LinearGradientMode" Namespace="System.Drawing.Drawing2D" />
    <DomainEnumeration Name="TypedEntityConstants" Namespace="nHydrate.Dsl" Description="Description for nHydrate.Dsl.TypedEntityConstants">
      <Literals>
        <EnumerationLiteral Description="This is not a typed entity" Name="None" Value="" />
        <EnumerationLiteral Description="The typed entity has a backing database table" Name="DatabaseTable" Value="" />
        <EnumerationLiteral Description="The typed entity is a code-only enumeration" Name="EnumOnly" Value="" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="IndexTypeConstants" Namespace="nHydrate.Dsl" Description="">
      <Literals>
        <EnumerationLiteral Description="" Name="PrimaryKey" Value="" />
        <EnumerationLiteral Description="" Name="IsIndexed" Value="" />
        <EnumerationLiteral Description="" Name="User" Value="" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="DeleteActionConstants" Namespace="nHydrate.Dsl" Description="Description for nHydrate.Dsl.DeleteActionConstants">
      <Literals>
        <EnumerationLiteral Description="Take no action on principal entity deletion" Name="NoAction" Value="" />
        <EnumerationLiteral Description="Case delete of child objects" Name="Cascade" Value="" />
        <EnumerationLiteral Description="On principal entity deletion, set foreign key to null" Name="SetNull" Value="" />
      </Literals>
    </DomainEnumeration>
  </Types>
  <Shapes>
    <CompartmentShape Id="88c528eb-f5e8-45fd-a37f-3f092cf148d8" Description="" Name="EntityShape" DisplayName="Entity Shape" Namespace="nHydrate.Dsl" HasCustomConstructor="true" GeneratesDoubleDerived="true" TooltipType="Variable" FixedTooltipText="Entity Shape" TextColor="White" ExposesTextColor="true" FillColor="0, 122, 204" OutlineColor="Gray" InitialWidth="2" InitialHeight="0.3" OutlineThickness="0.01" FillGradientMode="None" ExposesOutlineColorAsProperty="true" ExposesFillColorAsProperty="true" ExposesOutlineDashStyleAsProperty="true" Geometry="Rectangle">
      <Properties>
        <DomainProperty Id="cfd42386-824b-4a70-9efd-9a54245b0ee8" Description="Description for nHydrate.Dsl.EntityShape.Fill Color" Name="FillColor" DisplayName="Fill Color" Kind="CustomStorage" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System.Drawing/Color" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="81760680-cfae-451c-ae35-23e612940beb" Description="Description for nHydrate.Dsl.EntityShape.Text Color" Name="TextColor" DisplayName="Text Color" Kind="CustomStorage" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System.Drawing/Color" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="a6ff4219-bd76-46d3-9f39-823dcc537a00" Description="Description for nHydrate.Dsl.EntityShape.Outline Color" Name="OutlineColor" DisplayName="Outline Color" Kind="CustomStorage" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System.Drawing/Color" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="9d7fe012-b646-4568-b7c0-550bbd46422e" Description="Description for nHydrate.Dsl.EntityShape.Outline Dash Style" Name="OutlineDashStyle" DisplayName="Outline Dash Style" Kind="CustomStorage" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System.Drawing.Drawing2D/DashStyle" />
          </Type>
        </DomainProperty>
      </Properties>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.25" VerticalOffset="0">
        <TextDecorator Name="EntityTextDecorator" DisplayName="Entity Text Decorator" DefaultText="" FontStyle="Bold" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopRight" HorizontalOffset="0" VerticalOffset="0">
        <ExpandCollapseDecorator Name="EntityExpandDecorator" DisplayName="Entity Expand Decorator" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.05" VerticalOffset="0">
        <IconDecorator Name="EntityIconDecorator" DisplayName="Entity Icon Decorator" DefaultIcon="Resources\Entity.png" />
      </ShapeHasDecorators>
      <Compartment FillColor="WhiteSmoke" TitleFillColor="Gainsboro" Name="EntityFieldCompartment" Title="Fields" />
    </CompartmentShape>
    <CompartmentShape Id="9cd4f727-289f-4f8c-be4b-6caf68e48408" Description="" Name="ViewShape" DisplayName="View Shape" Namespace="nHydrate.Dsl" GeneratesDoubleDerived="true" TooltipType="Variable" FixedTooltipText="View Shape" FillColor="255, 255, 192" OutlineColor="255, 128, 255" InitialWidth="2" InitialHeight="0.3" OutlineThickness="0.01" FillGradientMode="None" Geometry="Rectangle">
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.25" VerticalOffset="0">
        <TextDecorator Name="ViewTextDecorator" DisplayName="" DefaultText="" FontStyle="Bold" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopRight" HorizontalOffset="0" VerticalOffset="0">
        <ExpandCollapseDecorator Name="ViewExpandDecorator" DisplayName="" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.05" VerticalOffset="0">
        <IconDecorator Name="ViewIconDecorator" DisplayName="View Icon Decorator" DefaultIcon="Resources\view.png" />
      </ShapeHasDecorators>
      <Compartment FillColor="WhiteSmoke" TitleFillColor="Gainsboro" Name="ViewFieldCompartment" Title="Fields" />
    </CompartmentShape>
  </Shapes>
  <Connectors>
    <Connector Id="00d9c38d-d3b6-458e-b0c2-1b9603825d3d" Description="Connect two entities" Name="EntityAssociationConnector" DisplayName="Entity Association Connector" Namespace="nHydrate.Dsl" HasCustomConstructor="true" GeneratesDoubleDerived="true" TooltipType="Variable" FixedTooltipText="Connect two entities" Color="DimGray" SourceEndStyle="EmptyDiamond" TargetEndStyle="FilledArrow" Thickness="0.01">
      <ConnectorHasDecorators Position="SourceTop" OffsetFromShape="0" OffsetFromLine="0" isMoveable="true">
        <TextDecorator Name="SourceEntityRelationTextDecorator" DisplayName="SOURCE" DefaultText="" FontStyle="Italic" />
      </ConnectorHasDecorators>
      <ConnectorHasDecorators Position="TargetTop" OffsetFromShape="0" OffsetFromLine="0" isMoveable="true">
        <TextDecorator Name="DestEntityRelationTextDecorator" DisplayName="DESTINATION" DefaultText="" FontStyle="Italic" />
      </ConnectorHasDecorators>
    </Connector>
  </Connectors>
  <XmlSerializationBehavior Name="nHydrateSerializationBehavior" Namespace="nHydrate.Dsl">
    <ClassData>
      <XmlClassData TypeName="NHydrateModel" MonikerAttributeName="" SerializeId="true" MonikerElementName="nHydrateModelMoniker" ElementName="nHydrateModel" MonikerTypeName="NHydrateModelMoniker">
        <DomainClassMoniker Name="nHydrateModel" />
        <ElementData>
          <XmlRelationshipData RoleElementName="entities">
            <DomainRelationshipMoniker Name="nHydrateModelHasEntities" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="views">
            <DomainRelationshipMoniker Name="nHydrateModelHasViews" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="companyName">
            <DomainPropertyMoniker Name="nHydrateModel/CompanyName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="defaultNamespace">
            <DomainPropertyMoniker Name="nHydrateModel/DefaultNamespace" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="projectName">
            <DomainPropertyMoniker Name="nHydrateModel/ProjectName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="useUTCTime">
            <DomainPropertyMoniker Name="nHydrateModel/UseUTCTime" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="version">
            <DomainPropertyMoniker Name="nHydrateModel/Version" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="createdByColumnName">
            <DomainPropertyMoniker Name="nHydrateModel/CreatedByColumnName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="createdDateColumnName">
            <DomainPropertyMoniker Name="nHydrateModel/CreatedDateColumnName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="modifiedDateColumnName">
            <DomainPropertyMoniker Name="nHydrateModel/ModifiedDateColumnName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="modifiedByColumnName">
            <DomainPropertyMoniker Name="nHydrateModel/ModifiedByColumnName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="timestampColumnName">
            <DomainPropertyMoniker Name="nHydrateModel/TimestampColumnName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="grantUser">
            <DomainPropertyMoniker Name="nHydrateModel/GrantUser" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="relationFields">
            <DomainRelationshipMoniker Name="nHydrateModelHasRelationFields" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="modelToDisk">
            <DomainPropertyMoniker Name="nHydrateModel/ModelToDisk" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="modelVersion">
            <DomainPropertyMoniker Name="nHydrateModel/ModelVersion" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="outputTarget">
            <DomainPropertyMoniker Name="nHydrateModel/OutputTarget" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="tenantPrefix">
            <DomainPropertyMoniker Name="nHydrateModel/TenantPrefix" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="tenantColumnName">
            <DomainPropertyMoniker Name="nHydrateModel/TenantColumnName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="emitSafetyScripts">
            <DomainPropertyMoniker Name="nHydrateModel/EmitSafetyScripts" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="emitChangeScripts">
            <DomainPropertyMoniker Name="nHydrateModel/EmitChangeScripts" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="Entity" MonikerAttributeName="name" SerializeId="true" MonikerElementName="entityMoniker" ElementName="entity" MonikerTypeName="EntityMoniker">
        <DomainClassMoniker Name="Entity" />
        <ElementData>
          <XmlPropertyData XmlName="name" IsMonikerKey="true">
            <DomainPropertyMoniker Name="Entity/Name" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="childEntities">
            <DomainRelationshipMoniker Name="EntityHasEntities" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="fields">
            <DomainRelationshipMoniker Name="EntityHasFields" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="Entity/Summary" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="allowCreateAudit">
            <DomainPropertyMoniker Name="Entity/AllowCreateAudit" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="allowModifyAudit">
            <DomainPropertyMoniker Name="Entity/AllowModifyAudit" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="allowTimestamp">
            <DomainPropertyMoniker Name="Entity/AllowTimestamp" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isAssociative">
            <DomainPropertyMoniker Name="Entity/IsAssociative" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="immutable">
            <DomainPropertyMoniker Name="Entity/Immutable" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="schema">
            <DomainPropertyMoniker Name="Entity/Schema" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="codeFacade">
            <DomainPropertyMoniker Name="Entity/CodeFacade" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="staticDatum">
            <DomainRelationshipMoniker Name="EntityHasStaticDatum" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="generatesDoubleDerived">
            <DomainPropertyMoniker Name="Entity/GeneratesDoubleDerived" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="typedEntity">
            <DomainPropertyMoniker Name="Entity/TypedEntity" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="indexes">
            <DomainRelationshipMoniker Name="EntityHasIndexes" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="isTenant">
            <DomainPropertyMoniker Name="Entity/IsTenant" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="copyStateInfo">
            <DomainPropertyMoniker Name="Entity/CopyStateInfo" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="NHydrateModelHasEntities" MonikerAttributeName="" SerializeId="true" MonikerElementName="nHydrateModelHasEntitiesMoniker" ElementName="nHydrateModelHasEntities" MonikerTypeName="NHydrateModelHasEntitiesMoniker">
        <DomainRelationshipMoniker Name="nHydrateModelHasEntities" />
      </XmlClassData>
      <XmlClassData TypeName="EntityHasEntities" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityHasEntitiesMoniker" ElementName="entityHasEntities" MonikerTypeName="EntityHasEntitiesMoniker">
        <DomainRelationshipMoniker Name="EntityHasEntities" />
        <ElementData>
          <XmlPropertyData XmlName="multiplicity">
            <DomainPropertyMoniker Name="EntityHasEntities/Multiplicity" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="roleName">
            <DomainPropertyMoniker Name="EntityHasEntities/RoleName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isEnforced">
            <DomainPropertyMoniker Name="EntityHasEntities/IsEnforced" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="importData">
            <DomainPropertyMoniker Name="EntityHasEntities/ImportData" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="EntityHasEntities/Summary" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="importedConstraintName">
            <DomainPropertyMoniker Name="EntityHasEntities/ImportedConstraintName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="deleteAction">
            <DomainPropertyMoniker Name="EntityHasEntities/DeleteAction" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EntityAssociationConnector" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityAssociationConnectorMoniker" ElementName="entityAssociationConnector" MonikerTypeName="EntityAssociationConnectorMoniker">
        <ConnectorMoniker Name="EntityAssociationConnector" />
      </XmlClassData>
      <XmlClassData TypeName="NHydrateDiagram" MonikerAttributeName="" SerializeId="true" MonikerElementName="nHydrateDiagramMoniker" ElementName="nHydrateDiagram" MonikerTypeName="NHydrateDiagramMoniker">
        <DiagramMoniker Name="nHydrateDiagram" />
        <ElementData>
          <XmlPropertyData XmlName="displayType">
            <DomainPropertyMoniker Name="nHydrateDiagram/DisplayType" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EntityHasFields" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityHasFieldsMoniker" ElementName="entityHasFields" MonikerTypeName="EntityHasFieldsMoniker">
        <DomainRelationshipMoniker Name="EntityHasFields" />
      </XmlClassData>
      <XmlClassData TypeName="Field" MonikerAttributeName="" SerializeId="true" MonikerElementName="fieldMoniker" ElementName="field" MonikerTypeName="FieldMoniker">
        <DomainClassMoniker Name="Field" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="Field/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="nullable">
            <DomainPropertyMoniker Name="Field/Nullable" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isCalculated">
            <DomainPropertyMoniker Name="Field/IsCalculated" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="dataType">
            <DomainPropertyMoniker Name="Field/DataType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="default">
            <DomainPropertyMoniker Name="Field/Default" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="Field/Summary" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="formula">
            <DomainPropertyMoniker Name="Field/Formula" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="identity">
            <DomainPropertyMoniker Name="Field/Identity" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isIndexed">
            <DomainPropertyMoniker Name="Field/IsIndexed" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isUnique">
            <DomainPropertyMoniker Name="Field/IsUnique" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="length">
            <DomainPropertyMoniker Name="Field/Length" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isPrimaryKey">
            <DomainPropertyMoniker Name="Field/IsPrimaryKey" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="scale">
            <DomainPropertyMoniker Name="Field/Scale" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="codeFacade">
            <DomainPropertyMoniker Name="Field/CodeFacade" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isReadOnly">
            <DomainPropertyMoniker Name="Field/IsReadOnly" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="sortOrder">
            <DomainPropertyMoniker Name="Field/SortOrder" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="dataFormatString">
            <DomainPropertyMoniker Name="Field/DataFormatString" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="defaultIsFunc">
            <DomainPropertyMoniker Name="Field/DefaultIsFunc" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="importedDefaultName">
            <DomainPropertyMoniker Name="Field/ImportedDefaultName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="obsolete">
            <DomainPropertyMoniker Name="Field/Obsolete" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EntityShape" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityShapeMoniker" ElementName="entityShape" MonikerTypeName="EntityShapeMoniker">
        <CompartmentShapeMoniker Name="EntityShape" />
        <ElementData>
          <XmlPropertyData XmlName="fillColor">
            <DomainPropertyMoniker Name="EntityShape/FillColor" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="textColor">
            <DomainPropertyMoniker Name="EntityShape/TextColor" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="outlineColor">
            <DomainPropertyMoniker Name="EntityShape/OutlineColor" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="outlineDashStyle">
            <DomainPropertyMoniker Name="EntityShape/OutlineDashStyle" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="NHydrateModelHasViews" MonikerAttributeName="" SerializeId="true" MonikerElementName="nHydrateModelHasViewsMoniker" ElementName="nHydrateModelHasViews" MonikerTypeName="NHydrateModelHasViewsMoniker">
        <DomainRelationshipMoniker Name="nHydrateModelHasViews" />
      </XmlClassData>
      <XmlClassData TypeName="View" MonikerAttributeName="" SerializeId="true" MonikerElementName="viewMoniker" ElementName="view" MonikerTypeName="ViewMoniker">
        <DomainClassMoniker Name="View" />
        <ElementData>
          <XmlPropertyData XmlName="sQL">
            <DomainPropertyMoniker Name="View/SQL" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="View/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="codeFacade">
            <DomainPropertyMoniker Name="View/CodeFacade" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="View/Summary" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="schema">
            <DomainPropertyMoniker Name="View/Schema" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="fields">
            <DomainRelationshipMoniker Name="ViewHasFields" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="generatesDoubleDerived">
            <DomainPropertyMoniker Name="View/GeneratesDoubleDerived" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ViewHasFields" MonikerAttributeName="" SerializeId="true" MonikerElementName="viewHasFieldsMoniker" ElementName="viewHasFields" MonikerTypeName="ViewHasFieldsMoniker">
        <DomainRelationshipMoniker Name="ViewHasFields" />
      </XmlClassData>
      <XmlClassData TypeName="ViewField" MonikerAttributeName="" SerializeId="true" MonikerElementName="viewFieldMoniker" ElementName="viewField" MonikerTypeName="ViewFieldMoniker">
        <DomainClassMoniker Name="ViewField" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="ViewField/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="nullable">
            <DomainPropertyMoniker Name="ViewField/Nullable" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="dataType">
            <DomainPropertyMoniker Name="ViewField/DataType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="default">
            <DomainPropertyMoniker Name="ViewField/Default" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="ViewField/Summary" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="length">
            <DomainPropertyMoniker Name="ViewField/Length" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="scale">
            <DomainPropertyMoniker Name="ViewField/Scale" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="codeFacade">
            <DomainPropertyMoniker Name="ViewField/CodeFacade" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isPrimaryKey">
            <DomainPropertyMoniker Name="ViewField/IsPrimaryKey" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ViewShape" MonikerAttributeName="" SerializeId="true" MonikerElementName="viewShapeMoniker" ElementName="viewShape" MonikerTypeName="ViewShapeMoniker">
        <CompartmentShapeMoniker Name="ViewShape" />
      </XmlClassData>
      <XmlClassData TypeName="NHydrateModelHasRelationFields" MonikerAttributeName="" SerializeId="true" MonikerElementName="nHydrateModelHasRelationFieldsMoniker" ElementName="nHydrateModelHasRelationFields" MonikerTypeName="NHydrateModelHasRelationFieldsMoniker">
        <DomainRelationshipMoniker Name="nHydrateModelHasRelationFields" />
      </XmlClassData>
      <XmlClassData TypeName="RelationField" MonikerAttributeName="" SerializeId="true" MonikerElementName="relationFieldMoniker" ElementName="relationField" MonikerTypeName="RelationFieldMoniker">
        <DomainClassMoniker Name="RelationField" />
        <ElementData>
          <XmlPropertyData XmlName="sourceFieldId">
            <DomainPropertyMoniker Name="RelationField/SourceFieldId" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="targetFieldId">
            <DomainPropertyMoniker Name="RelationField/TargetFieldId" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="relationID">
            <DomainPropertyMoniker Name="RelationField/RelationID" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EntityHasStaticDatum" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityHasStaticDatumMoniker" ElementName="entityHasStaticDatum" MonikerTypeName="EntityHasStaticDatumMoniker">
        <DomainRelationshipMoniker Name="EntityHasStaticDatum" />
      </XmlClassData>
      <XmlClassData TypeName="StaticData" MonikerAttributeName="" SerializeId="true" MonikerElementName="staticDataMoniker" ElementName="staticData" MonikerTypeName="StaticDataMoniker">
        <DomainClassMoniker Name="StaticData" />
        <ElementData>
          <XmlPropertyData XmlName="columnKey">
            <DomainPropertyMoniker Name="StaticData/ColumnKey" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="value">
            <DomainPropertyMoniker Name="StaticData/Value" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="orderKey">
            <DomainPropertyMoniker Name="StaticData/OrderKey" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EntityHasIndexes" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityHasIndexesMoniker" ElementName="entityHasIndexes" MonikerTypeName="EntityHasIndexesMoniker">
        <DomainRelationshipMoniker Name="EntityHasIndexes" />
      </XmlClassData>
      <XmlClassData TypeName="Index" MonikerAttributeName="" SerializeId="true" MonikerElementName="indexMoniker" ElementName="index" MonikerTypeName="IndexMoniker">
        <DomainClassMoniker Name="Index" />
        <ElementData>
          <XmlPropertyData XmlName="parentEntityID">
            <DomainPropertyMoniker Name="Index/ParentEntityID" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isUnique">
            <DomainPropertyMoniker Name="Index/IsUnique" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="Index/Summary" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="indexColumns">
            <DomainRelationshipMoniker Name="IndexHasIndexColumns" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="definition" Representation="Ignore">
            <DomainPropertyMoniker Name="Index/Definition" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="indexType">
            <DomainPropertyMoniker Name="Index/IndexType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="importedName">
            <DomainPropertyMoniker Name="Index/ImportedName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="clustered">
            <DomainPropertyMoniker Name="Index/Clustered" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="IndexHasIndexColumns" MonikerAttributeName="" SerializeId="true" MonikerElementName="indexHasIndexColumnsMoniker" ElementName="indexHasIndexColumns" MonikerTypeName="IndexHasIndexColumnsMoniker">
        <DomainRelationshipMoniker Name="IndexHasIndexColumns" />
      </XmlClassData>
      <XmlClassData TypeName="IndexColumn" MonikerAttributeName="" SerializeId="true" MonikerElementName="indexColumnMoniker" ElementName="indexColumn" MonikerTypeName="IndexColumnMoniker">
        <DomainClassMoniker Name="IndexColumn" />
        <ElementData>
          <XmlPropertyData XmlName="fieldID">
            <DomainPropertyMoniker Name="IndexColumn/FieldID" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="ascending">
            <DomainPropertyMoniker Name="IndexColumn/Ascending" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="definition" Representation="Ignore">
            <DomainPropertyMoniker Name="IndexColumn/Definition" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="sortOrder">
            <DomainPropertyMoniker Name="IndexColumn/SortOrder" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
    </ClassData>
  </XmlSerializationBehavior>
  <ExplorerBehavior Name="nHydrateExplorer">
    <CustomNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\Entity.png">
        <Class>
          <DomainClassMoniker Name="Entity" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\field.png">
        <Class>
          <DomainClassMoniker Name="Field" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\view.png">
        <Class>
          <DomainClassMoniker Name="View" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\field.png">
        <Class>
          <DomainClassMoniker Name="ViewField" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\nhydrate.png">
        <Class>
          <DomainClassMoniker Name="nHydrateModel" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\index.png">
        <Class>
          <DomainClassMoniker Name="Index" />
        </Class>
        <PropertyDisplayed>
          <PropertyPath>
            <DomainPropertyMoniker Name="Index/Definition" />
            <DomainPath />
          </PropertyPath>
        </PropertyDisplayed>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\field.png">
        <Class>
          <DomainClassMoniker Name="IndexColumn" />
        </Class>
        <PropertyDisplayed>
          <PropertyPath>
            <DomainPropertyMoniker Name="IndexColumn/Definition" />
            <DomainPath />
          </PropertyPath>
        </PropertyDisplayed>
      </ExplorerNodeSettings>
    </CustomNodeSettings>
    <HiddenNodes>
      <DomainPath>EntityHasStaticDatum.StaticDatum</DomainPath>
      <DomainPath>EntityHasEntities.ChildEntities</DomainPath>
      <DomainPath>EntityHasEntities.ParentEntity</DomainPath>
      <DomainPath>nHydrateModelHasRelationFields.RelationFields</DomainPath>
      <DomainPath>EntityHasEntities.ChildEntities</DomainPath>
      <DomainPath>EntityHasEntities.ParentEntity</DomainPath>
    </HiddenNodes>
  </ExplorerBehavior>
  <ConnectionBuilders>
    <ConnectionBuilder Name="EntityHasEntitiesBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="EntityHasEntities" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Entity" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Entity" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
  </ConnectionBuilders>
  <Diagram Id="baed03c1-2130-477d-ba88-9e9cb86956de" Description="" Name="nHydrateDiagram" DisplayName="nHydrate Diagram" Namespace="nHydrate.Dsl" HasCustomConstructor="true" GeneratesDoubleDerived="true" FillColor="">
    <Properties>
      <DomainProperty Id="292cd8ce-c52a-49d8-8cbc-e3c3b29d8f51" Description="" Name="DisplayType" DisplayName="Display Type" DefaultValue="false" IsBrowsable="false">
        <Type>
          <ExternalTypeMoniker Name="/System/Boolean" />
        </Type>
      </DomainProperty>
    </Properties>
    <Class>
      <DomainClassMoniker Name="nHydrateModel" />
    </Class>
    <ShapeMaps>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="Entity" />
        <ParentElementPath>
          <DomainPath>nHydrateModelHasEntities.nHydrateModel/!nHydrateModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="EntityShape/EntityTextDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Entity/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="EntityShape" />
        <CompartmentMap>
          <CompartmentMoniker Name="EntityShape/EntityFieldCompartment" />
          <ElementsDisplayed>
            <DomainPath>EntityHasFields.Fields/!Field</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Field/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
      </CompartmentShapeMap>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="View" />
        <ParentElementPath>
          <DomainPath>nHydrateModelHasViews.nHydrateModel/!nHydrateModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="ViewShape/ViewTextDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="View/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="ViewShape" />
        <CompartmentMap>
          <CompartmentMoniker Name="ViewShape/ViewFieldCompartment" />
          <ElementsDisplayed>
            <DomainPath>ViewHasFields.Fields/!ViewField</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="ViewField/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
      </CompartmentShapeMap>
    </ShapeMaps>
    <ConnectorMaps>
      <ConnectorMap>
        <ConnectorMoniker Name="EntityAssociationConnector" />
        <DomainRelationshipMoniker Name="EntityHasEntities" />
        <DecoratorMap>
          <TextDecoratorMoniker Name="EntityAssociationConnector/SourceEntityRelationTextDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Entity/Name" />
              <DomainPath>EntityHasEntities!ChildEntity</DomainPath>
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <DecoratorMap>
          <TextDecoratorMoniker Name="EntityAssociationConnector/DestEntityRelationTextDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Entity/Name" />
              <DomainPath>EntityHasEntities!ParentEntity</DomainPath>
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
      </ConnectorMap>
    </ConnectorMaps>
  </Diagram>
  <Designer CopyPasteGeneration="CopyPasteOnly" FileExtension="nhydrate" EditorGuid="29e4af38-a8e4-4323-a69b-b69b971fb6cd">
    <RootClass>
      <DomainClassMoniker Name="nHydrateModel" />
    </RootClass>
    <XmlSerializationDefinition CustomPostLoad="true">
      <XmlSerializationBehaviorMoniker Name="nHydrateSerializationBehavior" />
    </XmlSerializationDefinition>
    <ToolboxTab TabText="nHydrate">
      <ElementTool Name="Entity" ToolboxIcon="Resources\entity.bmp" Caption="Entity" Tooltip="Create an Entity" HelpKeyword="Entity">
        <DomainClassMoniker Name="Entity" />
      </ElementTool>
      <ConnectionTool Name="Association" ToolboxIcon="resources\exampleconnectortoolbitmap.bmp" Caption="Association" Tooltip="Create a relation between two entities" HelpKeyword="">
        <ConnectionBuilderMoniker Name="nHydrate/EntityHasEntitiesBuilder" />
      </ConnectionTool>
      <ElementTool Name="View" ToolboxIcon="Resources\view.bmp" Caption="View" Tooltip="Create a View" HelpKeyword="View">
        <DomainClassMoniker Name="View" />
      </ElementTool>
    </ToolboxTab>
    <Validation UsesMenu="false" UsesOpen="false" UsesSave="false" UsesCustom="true" UsesLoad="false" />
    <DiagramMoniker Name="nHydrateDiagram" />
  </Designer>
  <Explorer ExplorerGuid="383b4f2d-0240-4258-beba-da8a6a0c3ab6" Title="nHydrate Explorer">
    <ExplorerBehaviorMoniker Name="nHydrate/nHydrateExplorer" />
  </Explorer>
</Dsl>