<?xml version="1.0" encoding="utf-8"?>
<Dsl xmlns:dm0="http://schemas.microsoft.com/VisualStudio/2008/DslTools/Core" dslVersion="1.0.0.0" Id="7a314716-48c9-4371-8978-062be635f9b4" Description="This is the nHydrate Visual Modeler" Name="nHydrate" DisplayName="nHydrate ORM Modeler" Namespace="nHydrate.Dsl" MajorVersion="6" Revision="222" ProductName="nHydrate ORM Modeler" CompanyName="nHydrate.org" PackageGuid="36220dab-63c7-4daa-860c-fc548bf4d5d3" PackageNamespace="nHydrate.DslPackage" xmlns="http://schemas.microsoft.com/VisualStudio/2005/DslTools/DslDefinitionModel">
  <Notes>This integrated VS.NET component creates strongly-typed, extendable classes inside of a framework based on Entity Framework.</Notes>
  <Classes>
    <DomainClass Id="77b5fe81-853a-4b74-8ce5-98612544852f" Description="" Name="nHydrateModel" DisplayName="nHydrate Model" Namespace="nHydrate.Dsl" HasCustomConstructor="true" GeneratesDoubleDerived="true">
      <Properties>
        <DomainProperty Id="e4a7289c-e61c-440e-b881-5b06950fd6f0" Description="Specifies the company name that will be used to build namespaces" Name="CompanyName" DisplayName="Company Name" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="4f568ed1-d9e1-4905-bee0-6e6111581ce5" Description="Determines copyright to add to each file" Name="Copyright" DisplayName="Copyright" Category="Documentation">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(nHydrate.Dsl.Design.Editors.CopyrightEditor), typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
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
        <DomainProperty Id="e309bd8d-ce4c-4995-a73e-b6aaeb070c3d" Description="Determines the target SQL Server version" Name="SQLServerType" DisplayName="SQLServer Type" DefaultValue="SQL2008" Category="Database">
          <Type>
            <DomainEnumerationMoniker Name="DatabaseTypeConstants" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="bc3b0b9e-6e90-4e4c-a859-e11747c420fc" Description="Determines the prefix for generated stored procedures" Name="StoredProcedurePrefix" DisplayName="Stored Procedure Prefix" DefaultValue="gen" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="5be892eb-5e4f-4065-ad4a-ee6b1c65c8d4" Description="Specifies whether UTC or local time is used for the created and modified audits" Name="UseUTCTime" DisplayName="Use UTCTime" DefaultValue="false" Category="Definition">
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
        <DomainProperty Id="f4344b62-7cd9-4937-b07a-7056d72fc1fa" Description="Determines if all tokens are transformed from a lower case/undescore format to title case tokens on generation" Name="TransformNames" DisplayName="Transform Names" DefaultValue="false" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="9e140ef2-050f-4fe9-b014-8d0da9f5d22e" Description="Determines if generation will be based on modules" Name="UseModules" DisplayName="Use Modules" DefaultValue="false" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="7eccc5be-839a-41a9-b187-0893eb0f3ea9" Description="Determines if views are visible on the diagram" Name="ShowViews" DisplayName="Show Views" DefaultValue="false" Category="Diagram" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="2a51cfa1-07fc-4ead-812a-9a7741b5cbde" Description="Determines if stored procedures are visible on the diagram" Name="ShowStoredProcedures" DisplayName="Show Stored Procedures" DefaultValue="false" Category="Diagram" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="9df51744-b095-46bc-840e-b9de79f31b36" Description="Determines if functions are visible on the diagram" Name="ShowFunctions" DisplayName="Show Functions" DefaultValue="false" Category="Diagram" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b677c102-9e9a-4944-81e2-58029dcf4667" Description="Determines the database collation" Name="Collate" DisplayName="Collate" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="6fbe9eae-bebb-4ede-b0df-3f8ea4153633" Description="Determines if model objects are duplicated on disk for easy editing" Name="ModelToDisk" DisplayName="Model To Disk" DefaultValue="false" Category="Behavior">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="d45a19f0-de09-44a9-840e-0a03202ac87e" Description="Determines the last used precedence for model objects used for installer generation" Name="MaxPrecedenceOrder" DisplayName="Max Precedence Order" DefaultValue="0" Category="Definition" IsBrowsable="false" IsUIReadOnly="true">
          <Type>
            <ExternalTypeMoniker Name="/System/Int32" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="cfac3b29-311d-4da5-8d93-bb442283b51f" Description="Determines the version of the model. Used for tracking changes and provides an upgrade path for older models" Name="ModelVersion" DisplayName="Model Version" DefaultValue="" Category="Definition" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e6f9583a-48f9-4747-95ef-6edaf297206b" Description="The URL of the company" Name="CompanyURL" DisplayName="Company URL" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="4bfc4208-7582-4e3d-a01a-7d20a1c3caaa" Description="The public phone numbner of the company" Name="CompanyPhone" DisplayName="Company Phone" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="4138a1c1-4bbf-4195-aba3-1c32aad51a0e" Description="Determines if generated stored procedures are used for the CRUD layer." Name="UseGeneratedCRUD" DisplayName="Use Generated CRUD" DefaultValue="" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="ceb0f015-ea20-4762-a324-e3c7fc567b0c" Description="Determines which objects are visible on the diagram" Name="DiagramVisibility" DisplayName="Diagram Visibility" DefaultValue="" Category="Diagram">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(nHydrate.Dsl.Design.Converters.VisibilityTypeEnumConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <DomainEnumerationMoniker Name="VisibilityTypeConstants" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="be71ee12-7759-4612-930e-e53430febfb8" Description="The target location for generated projects" Name="OutputTarget" DisplayName="Output Target" Category="Definition" IsBrowsable="false">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(nHydrate.Dsl.Design.Editors.OutputTargetEditor), typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
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
        <DomainProperty Id="faacbca6-ce50-4101-846f-3b6a55e61bce" Description="Determines the target Entity Framework version" Name="EFVersion" DisplayName="EF Version" DefaultValue="EF6" Category="Code">
          <Type>
            <DomainEnumerationMoniker Name="EFVersionConstants" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="f348a2a7-4cf4-440c-b0b5-75be15bf6dde" Description="Determines if normalization safety scripts are emitted into the installer" Name="EmitSafetyScripts" DisplayName="Emit Safety Scripts" DefaultValue="true" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="ef57a476-64d2-47a8-9e7f-562a41dd6be1" Description="Determines if mock objects are generated" Name="AllowMocks" DisplayName="Allow Mocks" DefaultValue="false" Category="Code">
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
            <DomainClassMoniker Name="StoredProcedure" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>nHydrateModelHasStoredProcedures.StoredProcedures</DomainPath>
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
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Function" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>nHydrateModelHasFunctions.Functions</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="Module" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>nHydrateModelHasModules.Modules</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective UsesCustomAccept="true">
          <Index>
            <DomainClassMoniker Name="RelationModule" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>nHydrateModelHasRelationModules.RelationModules</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="ModelMetadata" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>nHydrateModelHasModelMetadata.ModelMetadata</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective UsesCustomAccept="true">
          <Index>
            <DomainClassMoniker Name="IndexModule" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>nHydrateModelHasIndexModules.IndexModules</DomainPath>
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
        <DomainProperty Id="b01b66ac-5a6b-496b-92e9-a72d8bbe3b2d" Description="Determines if there is a full audit trail for this entity" Name="AllowAuditTracking" DisplayName="Allow Audit Tracking" DefaultValue="false" Category="Audit">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
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
        <DomainProperty Id="5ad0492a-2a29-4b84-9de9-a0c19112d3f6" Description="Determines if this primary key is enforced in the database" Name="EnforcePrimaryKey" DisplayName="Enforce Primary Key" DefaultValue="true" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="5044db17-fb85-4f77-b070-278f87b02f8e" Description="Determines if this item is used when generating" Name="IsGenerated" DisplayName="Is Generated" DefaultValue="true" Category="Definition">
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
        <DomainProperty Id="41752180-d536-4d6b-b5a0-45ae43f8c7a3" Description="Determines if this table has secured access" Name="Security" DisplayName="Security" DefaultValue="" Kind="CustomStorage" Category="Security">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(nHydrate.Dsl.Design.Editors.SecurityFunctionEditor), typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
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
        <ElementMergeDirective UsesCustomAccept="true">
          <Index>
            <DomainClassMoniker Name="Composite" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>EntityHasComposites.Composites</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="EntityMetadata" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>EntityHasMetadata.EntityMetadata</DomainPath>
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
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="SecurityFunction" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>EntityHasSecurityFunction.SecurityFunction</DomainPath>
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
        <DomainProperty Id="afae6e3a-195e-4510-b2d2-0fe9fd2738b7" Description="Determines a prompt that can be displayed in the UI" Name="FriendlyName" DisplayName="Friendly Name" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="f990fadc-0b79-46b2-8afc-b83182fbe1cc" Description="Determines if this item allows null values" Name="Nullable" DisplayName="Nullable" DefaultValue="true" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="893044a6-ae4a-4ec8-9e29-bde5c497f3e3" Description="Determines the field collation" Name="Collate" DisplayName="Collate" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
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
        <DomainProperty Id="76a59bb8-81b0-4459-82fe-e604387215bf" Description="Determines if this item is used when generating" Name="IsGenerated" DisplayName="Is Generated" DefaultValue="true" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
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
        <DomainProperty Id="1681175c-090d-404f-a776-086c6d444672" Description="Determines the minimum value for a int, long, float value" Name="Min" DisplayName="Min" DefaultValue="" Category="Definition">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(nHydrate.Dsl.Design.Converters.RangeMinConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/Double" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="7be3e16a-8034-41e2-872e-75ea86acbeb9" Description="Determines the maximum value for a int, long, float value" Name="Max" DisplayName="Max" DefaultValue="" Category="Definition">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(nHydrate.Dsl.Design.Converters.RangeMaxConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/Double" />
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
        <DomainProperty Id="69413108-0594-46d1-a223-61548de5dff6" Description="The validation pattern used for UI controls to validate this field value" Name="ValidationExpression" DisplayName="Validation Expression" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
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
        <DomainProperty Id="9a782fd3-4f83-4f26-b3a6-a5ba715342aa" Description="Determines if this property is browsable in the UI" Name="IsBrowsable" DisplayName="Is Browsable" DefaultValue="true" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="7ebd917a-67bb-45c4-93b7-066920658f80" Description="Determines the property grid category of this item" Name="Category" DisplayName="Category" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
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
        <DomainProperty Id="bd854f61-0c58-4072-bc5f-6fe9d40cd6a5" Description="Determines formatting information for the UI" Name="UIDataType" DisplayName="UI Datatype" DefaultValue="Custom" Category="Definition">
          <Type>
            <DomainEnumerationMoniker Name="UIDataTypeConstants" />
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
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="FieldMetadata" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>FieldHasMetadata.FieldMetadata</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="e32474f5-b350-4120-9e62-0790cd4b3f90" Description="This is a custom database stored procedure" Name="StoredProcedure" DisplayName="Stored Procedure" Namespace="nHydrate.Dsl" GeneratesDoubleDerived="true">
      <Properties>
        <DomainProperty Id="3a65566b-2e97-446b-abee-da73d0309a06" Description="Determines SQL statement used to create the database stored procedure object" Name="SQL" DisplayName="SQL" Category="Definition">
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
        <DomainProperty Id="045175eb-9128-4491-864d-00e8d1fdb107" Description="Determines the name of this object" Name="Name" DisplayName="Name" Category="Definition" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="41ba3b0e-81bc-4fa0-977e-519fb609a178" Description="Determines if this item is used when generating" Name="IsGenerated" DisplayName="Is Generated" DefaultValue="true" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="9252ed3a-0e26-4bef-b114-0ee1dd2b70ba" Description="Determines the object name used in the API. If this property is blank the 'Name' property is used in the API. This property can be used to mask the database identifier." Name="CodeFacade" DisplayName="Code Facade" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="61da569b-9110-46e4-99b9-1c0a7faee79e" Description="Determines the summary of this object" Name="Summary" DisplayName="Summary" Category="Documentation">
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
        <DomainProperty Id="1c8ece9a-182f-4210-a36a-344d7d0771b2" Description="Determines the parent schema for this object" Name="Schema" DisplayName="Schema" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="2083b2fd-c2b3-4fd5-8266-3572b57124a0" Description="Determines the this stored procedure is a pre-existing one and should not be overwritten." Name="IsExisting" DisplayName="Is Existing" DefaultValue="false" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="293cecfe-cfe0-462e-9903-538c43162eef" Description="Determines the name of this stored procedure in the database. Leave empty to auto-generate." Name="DatabaseObjectName" DisplayName="Database Object Name" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e12501b1-2015-49e6-a3ae-e6ac387f9cb3" Description="If True, will generate both a base class with all functionality and a partial class to support customization through overrides" Name="GeneratesDoubleDerived" DisplayName="Generates Double Derived" DefaultValue="false" Category="Code" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="5d5f712c-bc88-4720-895a-d550d32f5d61" Description="Determines the order generated scripts are run" Name="PrecedenceOrder" DisplayName="Precedence Order" DefaultValue="0" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Int32" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="StoredProcedureField" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>StoredProcedureHasFields.Fields</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="StoredProcedureParameter" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>StoredProcedureHasParameters.Parameters</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="09f86d91-8e2f-4a68-a738-8ab1855656d7" Description="" Name="StoredProcedureField" DisplayName="Field" Namespace="nHydrate.Dsl" HasCustomConstructor="true" GeneratesDoubleDerived="true">
      <Properties>
        <DomainProperty Id="27eac854-6501-4f02-8c85-113409246394" Description="Determines the name of this object" Name="Name" DisplayName="Name" DefaultValue="" Category="Definition" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="58043103-5d33-4dd8-88df-032ff798f22d" Description="Determines a friend name to display to users" Name="FriendlyName" DisplayName="Friendly Name" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="fe26f5f8-58d8-48fd-b644-9d77704ff855" Description="Determines the object name used in the API. If this property is blank the 'Name' property is used in the API. This property can be used to mask the database identifier." Name="CodeFacade" DisplayName="Code Facade" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="06f88333-b7c4-43cf-89e5-effd8bf94985" Description="Determines if this item allows null values" Name="Nullable" DisplayName="Nullable" DefaultValue="true" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="49c40309-92a6-4d7d-87bc-0cec2e05d7a5" Description="Determines the data type of this field" Name="DataType" DisplayName="Datatype" DefaultValue="VarChar" Category="Definition">
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
        <DomainProperty Id="bf7c1328-c7bf-480b-807e-c7fc6edd39fb" Description="Determines the summary of this object" Name="Summary" DisplayName="Summary" Category="Documentation">
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
        <DomainProperty Id="3601215e-78bc-4e3e-b298-b1805f5152a9" Description="Determines the default value of this object" Name="Default" DisplayName="Default" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="2a8e214f-140c-4483-ac4f-e79a2d4cf1cb" Description="Determines if this item is used when generating" Name="IsGenerated" DisplayName="Is Generated" DefaultValue="true" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="4aba4368-723c-47c8-80ba-dc03f2bfcc10" Description="Determines the size of this field in bytes" Name="Length" DisplayName="Length" DefaultValue="50" Category="Definition">
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
        <DomainProperty Id="85262f34-778c-441b-b6d2-e41ae84e9e97" Description="Determines the scale of some data types" Name="Scale" DisplayName="Scale" Category="Definition">
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
      </Properties>
    </DomainClass>
    <DomainClass Id="66aa7115-e9ae-425a-86fe-31630cf78b3a" Description="" Name="StoredProcedureParameter" DisplayName="Parameter" Namespace="nHydrate.Dsl" HasCustomConstructor="true" GeneratesDoubleDerived="true">
      <Properties>
        <DomainProperty Id="ec1e37ad-4fe1-47eb-a8cb-6df702660ba4" Description="Determines the name of this object" Name="Name" DisplayName="Name" DefaultValue="" Category="Definition" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="08f89366-ba85-4629-bc3b-3112150015cf" Description="Determines the object name used in the API. If this property is blank the 'Name' property is used in the API. This property can be used to mask the database identifier." Name="CodeFacade" DisplayName="Code Facade" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="76c1a180-9b5a-4415-acbe-b19bcdd7f491" Description="Determines if this item allows null values" Name="Nullable" DisplayName="Nullable" DefaultValue="true" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="f1c3aea3-a10c-4a29-a383-5013fafdf730" Description="Determines the data type of this field" Name="DataType" DisplayName="Datatype" DefaultValue="VarChar" Category="Definition">
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
        <DomainProperty Id="768fd231-e0eb-418f-a9b3-08ed27dd8baf" Description="Determines the summary of this object" Name="Summary" DisplayName="Summary" Category="Documentation">
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
        <DomainProperty Id="22beb39b-2b29-4dd1-94c1-bfef531ba545" Description="Determines the default value of this object" Name="Default" DisplayName="Default" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="68eb3263-0a8f-4793-b637-6d622cc7522f" Description="Determines if this item is used when generating" Name="IsGenerated" DisplayName="Is Generated" DefaultValue="true" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b106a032-1d57-4532-8fd6-8f370e281fe5" Description="Determines the size of this field in bytes" Name="Length" DisplayName="Length" DefaultValue="50" Category="Definition">
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
        <DomainProperty Id="53998728-8e93-4dc5-a4cc-32ae814f5dd5" Description="Determines if this is an output parameter for the mapped stored proc" Name="IsOutputParameter" DisplayName="Is Output Parameter" DefaultValue="false" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="a9a2eda9-591f-4f17-b19f-0f0ba495dda1" Description="Determines the scale of some data types" Name="Scale" DisplayName="Scale" Category="Definition">
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
        <DomainProperty Id="19673ffc-7726-4fbb-bde6-3ff406d4b3d8" Description="" Name="SortOrder" DisplayName="Sort Order" DefaultValue="0" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Int32" />
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
        <DomainProperty Id="a6ee4a71-3972-4a41-830d-dfe47ce7a807" Description="Determines if this item is used when generating" Name="IsGenerated" DisplayName="Is Generated" DefaultValue="true" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
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
        <DomainProperty Id="9f1807fb-1e99-4ce6-8052-44c94e072930" Description="Determines the order generated scripts are run" Name="PrecedenceOrder" DisplayName="Precedence Order" DefaultValue="0" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Int32" />
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
        <DomainProperty Id="7948a8f4-56b0-4e24-9f24-9d1bea973202" Description="Determines a friend name to display to users" Name="FriendlyName" DisplayName="Friendly Name" Category="Code">
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
        <DomainProperty Id="ee7d6ca9-883e-4d34-9444-18b3ea9c7c6b" Description="Determines if this item is used when generating" Name="IsGenerated" DisplayName="Is Generated" DefaultValue="true" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
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
    <DomainClass Id="a6bd893f-0e60-4354-a19e-64042cccbfb8" Description="" Name="Composite" DisplayName="Composite" Namespace="nHydrate.Dsl" GeneratesDoubleDerived="true">
      <Properties>
        <DomainProperty Id="56c0a23f-cc97-497c-bcab-c87899f954a1" Description="Determines the name of this object" Name="Name" DisplayName="Name" DefaultValue="" Category="Definition" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="6f15628a-77ab-4780-a712-d62c07506bb0" Description="Determines the object name used in the API. If this property is blank the 'Name' property is used in the API. This property can be used to mask the database identifier." Name="CodeFacade" DisplayName="Code Facade" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="7108a0f2-9ac1-43df-aea9-10d1785ad7b8" Description="Determines if this item is used when generating" Name="IsGenerated" DisplayName="Is Generated" DefaultValue="true" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="0e04de98-3432-420a-b615-613cddcf4524" Description="Determines the summary of this object" Name="Summary" DisplayName="Summary" Category="Documentation">
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
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective UsesCustomAccept="true">
          <Index>
            <DomainClassMoniker Name="CompositeField" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>CompositeHasFields.Fields</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="ae498b64-1182-495b-81d6-8972a32ecc67" Description="" Name="CompositeField" DisplayName="Composite Field" Namespace="nHydrate.Dsl">
      <Properties>
        <DomainProperty Id="24e3a97d-1b3b-4d4c-95ed-5064cd763d43" Description="" Name="FieldId" DisplayName="Field Id" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Guid" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="8022b63d-c856-4a0e-9881-c24888e718c5" Description="" Name="EntityMetadata" DisplayName="Metadata" Namespace="nHydrate.Dsl">
      <Properties>
        <DomainProperty Id="0202eeb2-cce2-4f28-9b61-d473348734db" Description="The unqiue key of this metadata" Name="Key" DisplayName="Key" Category="Data">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="8d770048-28f0-472d-8cff-e8d10ea7191b" Description="The value of this metadata" Name="Value" DisplayName="Value" Category="Data">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="ee86ada7-4a6b-498b-bd67-a9c3445b8a8b" Description="A summary of thsi object" Name="Summary" DisplayName="Summary" Category="Documentation">
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
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="EntityMetadata" />
          </Index>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="e4d98918-02cc-4cb0-94fc-2da89080f5be" Description="" Name="FieldMetadata" DisplayName="Metadata" Namespace="nHydrate.Dsl">
      <Properties>
        <DomainProperty Id="8a009da7-79ba-4f0b-9d2a-bbbd4c372231" Description="The unique key of this metadata" Name="Key" DisplayName="Key" Category="Data">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="f7fed8a2-943c-4c5f-90c2-f56bda23dfbc" Description="The value of this metadata" Name="Value" DisplayName="Value" Category="Data">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="4f1ef578-6f8a-422f-8542-3107675fa0e2" Description="A summary of thsi object" Name="Summary" DisplayName="Summary" Category="Documentation">
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
      </Properties>
    </DomainClass>
    <DomainClass Id="4940addd-d841-4fc6-8440-05351b351779" Description="This is a custom database function" Name="Function" DisplayName="Function" Namespace="nHydrate.Dsl" GeneratesDoubleDerived="true">
      <Properties>
        <DomainProperty Id="b6378f56-7e38-422b-a3ce-c777b6247d4c" Description="Determines SQL statement used to create the database function object" Name="SQL" DisplayName="SQL" Category="Definition">
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
        <DomainProperty Id="a40a2125-7278-4985-bb0b-ba43d43c2ce5" Description="Determines the name of this object" Name="Name" DisplayName="Name" Category="Definition" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="7a8be4b7-4cb3-4398-b64f-4c4eeb6384a8" Description="Determines if this item is used when generating" Name="IsGenerated" DisplayName="Is Generated" DefaultValue="true" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="5cb47a54-d2a1-4876-8a4e-76a37b08ba3c" Description="Determines the object name used in the API. If this property is blank the 'Name' property is used in the API. This property can be used to mask the database identifier." Name="CodeFacade" DisplayName="Code Facade" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="ce6796d2-b4b0-4a7d-a23b-63bb6bd2123e" Description="Determines the summary of this object" Name="Summary" DisplayName="Summary" Category="Documentation">
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
        <DomainProperty Id="bc5e57ab-995f-4c57-bb1d-7d6b981a817b" Description="Determines the parent schema for this object" Name="Schema" DisplayName="Schema" Category="Database">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="8863f742-c360-44f6-9031-e4d924cfbcb7" Description="Determines if this is a table function" Name="IsTable" DisplayName="Is Table" DefaultValue="false" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="31dfa463-ae1c-4f18-8150-865486249cae" Description="Optional variable name used in function declaration return type" Name="ReturnVariable" DisplayName="Return Variable" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="6b307dc7-05cb-4302-a521-622a47e64a2a" Description="Determines the order generated scripts are run" Name="PrecedenceOrder" DisplayName="Precedence Order" DefaultValue="0" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Int32" />
          </Type>
        </DomainProperty>
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="FunctionParameter" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>FunctionHasParameters.Parameters</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="FunctionField" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>FunctionHasFields.Fields</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="9a7fee83-c2d8-40e7-888d-e6c3ab881cac" Description="" Name="FunctionParameter" DisplayName="Parameter" Namespace="nHydrate.Dsl" HasCustomConstructor="true" GeneratesDoubleDerived="true">
      <Properties>
        <DomainProperty Id="838abb7a-9ad6-4495-922a-3b3607935fca" Description="Determines the name of this object" Name="Name" DisplayName="Name" DefaultValue="" Category="Definition" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="97fe9195-571f-4696-b57f-aadd4e3e6ec1" Description="Determines the object name used in the API. If this property is blank the 'Name' property is used in the API. This property can be used to mask the database identifier." Name="CodeFacade" DisplayName="Code Facade" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="58bdfc74-b1d2-4198-8506-52f89fbd0bf5" Description="Determines if this item allows null values" Name="Nullable" DisplayName="Nullable" DefaultValue="true" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e782b645-1554-4dfa-b017-53a9c5b8e779" Description="Determines the data type of this field" Name="DataType" DisplayName="Datatype" DefaultValue="VarChar" Category="Definition">
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
        <DomainProperty Id="b9f6c911-acf1-4fa6-9465-0626f66e34ce" Description="Determines the summary of this object" Name="Summary" DisplayName="Summary" Category="Documentation">
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
        <DomainProperty Id="c665e236-d7bd-455c-95bd-1e962ffa86e0" Description="Determines the default value of this object" Name="Default" DisplayName="Default" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="419b6652-e6b5-4b2a-86b0-f04d3f48deb5" Description="Determines if this item is used when generating" Name="IsGenerated" DisplayName="Is Generated" DefaultValue="true" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="0c316064-28a7-440f-a88a-3c6a94045dec" Description="Determines the size of this field in bytes" Name="Length" DisplayName="Length" DefaultValue="50" Category="Definition">
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
        <DomainProperty Id="e0c55ca8-d557-4f05-b668-5e6e96748934" Description="Determines the scale of some data types" Name="Scale" DisplayName="Scale" Category="Definition">
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
        <DomainProperty Id="22e6cd02-4cf1-4f1a-9316-994f96a0a0e6" Description="" Name="SortOrder" DisplayName="Sort Order" DefaultValue="0" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Int32" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="e203d7e8-2878-48e6-99e9-7c8a1daf0a53" Description="" Name="FunctionField" DisplayName="Field" Namespace="nHydrate.Dsl" HasCustomConstructor="true" GeneratesDoubleDerived="true">
      <Properties>
        <DomainProperty Id="159b2e68-0fad-402b-a809-9db545932201" Description="Determines the name of this object" Name="Name" DisplayName="Name" DefaultValue="" Category="Definition" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="8c20325a-def5-4428-8657-938f03756251" Description="Determines a friend name to display to users" Name="FriendlyName" DisplayName="Friendly Name" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="68ff06a6-34a5-4176-ae43-a97987c27e85" Description="Determines the object name used in the API. If this property is blank the 'Name' property is used in the API. This property can be used to mask the database identifier." Name="CodeFacade" DisplayName="Code Facade" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="6ee17bf0-1da2-47da-9b5f-d724da4518e1" Description="Determines if this item allows null values" Name="Nullable" DisplayName="Nullable" DefaultValue="true" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b51f07bd-34b2-4d6d-a12a-09f51a0d00e8" Description="Determines the data type of this field" Name="DataType" DisplayName="Datatype" DefaultValue="VarChar" Category="Definition">
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
        <DomainProperty Id="be737e4a-ec07-4834-93d3-d8a6e00c381f" Description="Determines the summary of this object" Name="Summary" DisplayName="Summary" Category="Documentation">
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
        <DomainProperty Id="4fe9787e-6a50-4060-8327-93dce4459689" Description="Determines the default value of this object" Name="Default" DisplayName="Default" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="767a56f3-75d1-4c8e-a767-a398641a0d8d" Description="Determines if this item is used when generating" Name="IsGenerated" DisplayName="Is Generated" DefaultValue="true" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="9b154c3b-1f7a-4d59-8912-e171efb9a6e7" Description="Determines the size of this field in bytes" Name="Length" DisplayName="Length" DefaultValue="50" Category="Definition">
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
        <DomainProperty Id="b06117d3-7bab-4b1a-bedc-c7620fb043ef" Description="Determines the scale of some data types" Name="Scale" DisplayName="Scale" Category="Definition">
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
      </Properties>
    </DomainClass>
    <DomainClass Id="f687b04b-616b-4974-8352-711274c9c4f2" Description="" Name="Module" DisplayName="Module" Namespace="nHydrate.Dsl" GeneratesDoubleDerived="true">
      <Properties>
        <DomainProperty Id="f2770b4c-a625-40ac-8e12-55b79189db0d" Description="Determines the name of this object" Name="Name" DisplayName="Name" DefaultValue="" Category="Definition" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="ba753fd5-2278-453f-b97a-567b28bf3733" Description="Determines summary text were applicable" Name="Summary" DisplayName="Summary" Category="Documentation">
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
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="ModuleRule" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>ModuleHasModuleRules.ModuleRules</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="07b9652b-9383-4cc4-b4c8-f2a54310cd11" Description="" Name="RelationModule" DisplayName="Relation Module" Namespace="nHydrate.Dsl">
      <Properties>
        <DomainProperty Id="2ffa9779-00ca-45f7-bf42-72119e743976" Description="" Name="RelationID" DisplayName="Relation ID" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Guid" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="14e732fe-8d5f-48c0-ad33-0108c3b2bfe9" Description="" Name="ModuleId" DisplayName="Module Id" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Guid" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="ab270741-6ed5-48af-b807-349665bd8e85" Description="Determines if this relation is included in the module" Name="Included" DisplayName="Included" DefaultValue="true">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="433141c2-8f16-43ab-bb8c-92b1ea954f92" Description="Determines if this relation is enforced in the database" Name="IsEnforced" DisplayName="Is Enforced" DefaultValue="true">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="66829b88-3323-4787-ac26-5aafe4dea717" Description="" Name="ModuleRule" DisplayName="Module Rule" Namespace="nHydrate.Dsl">
      <Properties>
        <DomainProperty Id="b262ea94-0839-4fab-9205-ae0caad1ee07" Description="The grouping for this rule" Name="Status" DisplayName="Status" DefaultValue="Subset" Category="Definition">
          <Type>
            <DomainEnumerationMoniker Name="ModuleRuleStatusTypeConstants" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="48dfe36f-e01d-4d61-a60a-761deb208079" Description="The module to which the status is applied" Name="DependentModule" DisplayName="Dependent Module" Category="Definition">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.Editor">
              <Parameters>
                <AttributeParameter Value="typeof(nHydrate.Dsl.Design.Editors.ModuleEditor), typeof(System.Drawing.Design.UITypeEditor)" />
              </Parameters>
            </ClrAttribute>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(nHydrate.Dsl.Design.Converters.ModuleConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/Guid" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="b38f468b-d7b6-4e03-8b6f-78fcac0d559b" Description="A summary of the rule" Name="Summary" DisplayName="Summary" Category="Documentation">
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
        <DomainProperty Id="e5291183-c926-4577-846e-1ed24b827e1d" Description="Determines the name of this rule" Name="Name" DisplayName="Name" DefaultValue="" Category="Definition" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="d7f3b90b-0732-4c32-bb77-54bf759ce9f9" Description="Determines the object types that are included by this rule" Name="Inclusion" DisplayName="Inclusion" DefaultValue="0" Category="Definition">
          <Attributes>
            <ClrAttribute Name="System.ComponentModel.TypeConverter">
              <Parameters>
                <AttributeParameter Value="typeof(nHydrate.Dsl.Design.Converters.ModuleRuleInclusionEnumConverter)" />
              </Parameters>
            </ClrAttribute>
          </Attributes>
          <Type>
            <ExternalTypeMoniker Name="/System/Int32" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="a66fcddc-6c16-4f84-b051-662ad3fcc5af" Description="Determines if this rule is enfored on validation" Name="Enforced" DisplayName="Enforced" DefaultValue="true" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
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
    <DomainClass Id="08609292-2eb3-4f13-a9a4-9455e9551a9e" Description="Description for nHydrate.Dsl.ModelMetadata" Name="ModelMetadata" DisplayName="Model Metadata" Namespace="nHydrate.Dsl">
      <Properties>
        <DomainProperty Id="2afda66a-2834-4ab9-944b-88dc25d84536" Description="The unqiue key of this metadata" Name="Key" DisplayName="Key" Category="Data">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="621eaec6-e972-4324-9236-6199be7b1612" Description="The value of this metadata" Name="Value" DisplayName="Value" Category="Data">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="776ac2d1-1c43-4b77-81f2-a4187e374f56" Description="" Name="IndexModule" DisplayName="Index Module" Namespace="nHydrate.Dsl">
      <Properties>
        <DomainProperty Id="b38a2405-6511-434e-ac57-70866a9ad29c" Description="" Name="IndexID" DisplayName="Index ID" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Guid" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="f4f123c3-878c-4add-8a3e-d7996be83978" Description="" Name="ModuleId" DisplayName="Module Id" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Guid" />
          </Type>
        </DomainProperty>
      </Properties>
    </DomainClass>
    <DomainClass Id="1e5bf9d9-6c1b-44eb-a238-0b88d669b189" Description="Description for nHydrate.Dsl.SecurityFunction" Name="SecurityFunction" DisplayName="Security Function" Namespace="nHydrate.Dsl">
      <Properties>
        <DomainProperty Id="442b861a-5046-480b-9d80-91526a63dacc" Description="Determines SQL statement used to create the database stored procedure object" Name="SQL" DisplayName="SQL" Category="Definition">
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
      </Properties>
      <ElementMergeDirectives>
        <ElementMergeDirective>
          <Index>
            <DomainClassMoniker Name="SecurityFunctionParameter" />
          </Index>
          <LinkCreationPaths>
            <DomainPath>SecurityFunctionHasSecurityFunctionParameters.SecurityFunctionParameters</DomainPath>
          </LinkCreationPaths>
        </ElementMergeDirective>
      </ElementMergeDirectives>
    </DomainClass>
    <DomainClass Id="c6986337-c218-4441-9154-556d20066753" Description="Description for nHydrate.Dsl.SecurityFunctionParameter" Name="SecurityFunctionParameter" DisplayName="Security Function Parameter" Namespace="nHydrate.Dsl">
      <Properties>
        <DomainProperty Id="69345b69-7253-46f3-b72f-f74b278bcf5d" Description="Determines the name of this object" Name="Name" DisplayName="Name" DefaultValue="" Category="Definition" IsElementName="true">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="c66fcb75-560d-4bca-9cbb-45f73275a180" Description="Determines if this item allows null values" Name="Nullable" DisplayName="Nullable" DefaultValue="true" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="39261e47-0ea7-4517-bf21-f6264543e58b" Description="Determines the data type of this field" Name="DataType" DisplayName="Datatype" DefaultValue="VarChar" Category="Definition">
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
        <DomainProperty Id="388dbdf1-95de-45e8-b478-1e1642961be8" Description="Determines the summary of this object" Name="Summary" DisplayName="Summary" Category="Documentation">
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
        <DomainProperty Id="d113df78-35f6-4d9b-89dd-57cafa29caf8" Description="Determines the default value of this object" Name="Default" DisplayName="Default" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="e0b39dab-197c-4449-b9be-664af9d39cd8" Description="Determines if this item is used when generating" Name="IsGenerated" DisplayName="Is Generated" DefaultValue="true" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="ff1f7ace-16f5-4e9c-8e8a-67d2fbdc5921" Description="Determines the size of this field in bytes" Name="Length" DisplayName="Length" DefaultValue="50" Category="Definition">
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
        <DomainProperty Id="f7bc69e4-a8b2-4a57-9870-e7eaf46d2e1f" Description="Determines the scale of some data types" Name="Scale" DisplayName="Scale" Category="Definition">
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
        <DomainProperty Id="10d9be44-ac7b-4e81-86a4-4363d7fdafcf" Description="" Name="SortOrder" DisplayName="Sort Order" DefaultValue="0" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/Int32" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="010816bf-2934-47e1-9323-a30aa9691250" Description="Determines the object name used in the API. If this property is blank the 'Name' property is used in the API. This property can be used to mask the database identifier." Name="CodeFacade" DisplayName="Code Facade" Category="Code">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
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
    <DomainRelationship Id="e17b4278-be93-4c91-a2ea-be5c39700492" Description="" Name="nHydrateModelHasStoredProcedures" DisplayName="NHydrate Model Has Stored Procedures" Namespace="nHydrate.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="a751f422-3d3f-4480-8f7b-be4454df83d3" Description="" Name="nHydrateModel" DisplayName="NHydrate Model" PropertyName="StoredProcedures" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Stored Procedures">
          <RolePlayer>
            <DomainClassMoniker Name="nHydrateModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="bc83dec7-cfbe-4bb9-9e42-128efd1e2e5e" Description="" Name="StoredProcedure" DisplayName="Stored Procedure" PropertyName="nHydrateModel" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="nHydrate Model">
          <RolePlayer>
            <DomainClassMoniker Name="StoredProcedure" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="4870cc73-c360-4c7c-9b2d-976e53f54dd7" Description="" Name="StoredProcedureHasFields" DisplayName="Stored Procedure Has Fields" Namespace="nHydrate.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="bdc7cdb9-a588-4e38-8498-35f33c6225a8" Description="" Name="StoredProcedure" DisplayName="Stored Procedure" PropertyName="Fields" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Fields">
          <RolePlayer>
            <DomainClassMoniker Name="StoredProcedure" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="4e93d360-be4b-4996-bff4-1493c41c94d0" Description="" Name="StoredProcedureField" DisplayName="Stored Procedure Field" PropertyName="StoredProcedure" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Stored Procedure">
          <RolePlayer>
            <DomainClassMoniker Name="StoredProcedureField" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="4fdc3e26-1253-4cea-89c5-cc6078cbfb7e" Description="" Name="StoredProcedureHasParameters" DisplayName="Stored Procedure Has Parameters" Namespace="nHydrate.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="021e0136-6b0e-4617-8308-fa63d66c8ff7" Description="" Name="StoredProcedure" DisplayName="Stored Procedure" PropertyName="Parameters" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Parameters">
          <RolePlayer>
            <DomainClassMoniker Name="StoredProcedure" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="d63e25f5-ce95-4369-bcd9-954362b9283f" Description="" Name="StoredProcedureParameter" DisplayName="Stored Procedure Parameter" PropertyName="StoredProcedure" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Stored Procedure">
          <RolePlayer>
            <DomainClassMoniker Name="StoredProcedureParameter" />
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
    <DomainRelationship Id="e18bfada-9d87-480c-bbc9-adb50d39ef74" Description="" Name="EntityInheritsEntity" DisplayName="Entity Inherits Entity" Namespace="nHydrate.Dsl" HasCustomConstructor="true" GeneratesDoubleDerived="true">
      <Properties>
        <DomainProperty Id="e1d48068-bba9-4ddc-a5cd-37f7a461bbde" Description="The named relation necessary when there is more than one relation between two entities" Name="RoleName" DisplayName="Role Name" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="1a54f2fc-fb88-49a9-b4dc-7c59f0a3c280" Description="Determines if this relationship is enfored in the database or just in code" Name="IsEnforced" DisplayName="Is Enforced" DefaultValue="true" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/Boolean" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="d7e7ea0c-5374-408b-9470-088bb8467c0a" Description="Determines summary text were applicable" Name="Summary" DisplayName="Summary" Category="Documentation">
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
      </Properties>
      <Source>
        <DomainRole Id="e28724cf-d273-4d1e-8292-4ef20e159afb" Description="" Name="ParentInheritedEntity" DisplayName="Parent Inherited Entity" PropertyName="ParentInheritedEntity" Multiplicity="ZeroOne" Category="Definition" PropertyDisplayName="Parent Inherited Entity">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="2095e234-5112-4a60-a3de-f6ad40712a46" Description="" Name="ChildDerivedEntities" DisplayName="Child Derived Entities" PropertyName="ChildDerivedEntities" Category="Definition" IsPropertyBrowsable="false" PropertyDisplayName="Child Derived Entities">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="196f7f0d-bf28-494a-916f-17f515856f7e" Description="" Name="EntityHasComposites" DisplayName="Entity Has Composites" Namespace="nHydrate.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="7c3a4c96-7c33-4e7c-80e1-905a7c15a248" Description="" Name="Entity" DisplayName="Entity" PropertyName="Composites" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Composites">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="906a19ba-d657-4536-87c5-9c49b84e95ef" Description="" Name="Composite" DisplayName="Composite" PropertyName="Entity" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Entity">
          <RolePlayer>
            <DomainClassMoniker Name="Composite" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="fcf0b894-a572-4b7c-b00b-b7ef50a0b5bc" Description="" Name="CompositeHasFields" DisplayName="Composite Has Fields" Namespace="nHydrate.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="d442f05a-b561-4029-acd7-d33501098183" Description="" Name="Composite" DisplayName="Composite" PropertyName="Fields" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Fields">
          <RolePlayer>
            <DomainClassMoniker Name="Composite" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="17b23d49-303f-471f-9d87-cf5e36c80a2a" Description="" Name="CompositeField" DisplayName="Composite Field" PropertyName="Composite" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Composite">
          <RolePlayer>
            <DomainClassMoniker Name="CompositeField" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="d8f52b3f-4006-4795-b93f-d47721cc13d3" Description="" Name="EntityHasMetadata" DisplayName="Entity Has Metadata" Namespace="nHydrate.Dsl" GeneratesDoubleDerived="true" IsEmbedding="true">
      <Source>
        <DomainRole Id="c3d56be9-0465-4928-992d-fbd0efdae2c4" Description="" Name="Entity" DisplayName="Entity" PropertyName="EntityMetadata" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Entity Metadata">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="1e3785e5-2c22-4ede-b596-24c8ff7f1d2e" Description="" Name="EntityMetadata" DisplayName="Metadata" PropertyName="Entity" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Entity">
          <RolePlayer>
            <DomainClassMoniker Name="EntityMetadata" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="c28d2f9b-cdfc-4855-b347-2282af8306ec" Description="" Name="FieldHasMetadata" DisplayName="Field Has Metadata" Namespace="nHydrate.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="b9da33f0-6f75-4c5e-b9c9-2b047ac164d4" Description="" Name="Field" DisplayName="Field" PropertyName="FieldMetadata" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Field Metadata">
          <RolePlayer>
            <DomainClassMoniker Name="Field" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="ce82ad42-2d12-445f-a7f0-45ce8629d304" Description="" Name="FieldMetadata" DisplayName="Metadata" PropertyName="Field" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Field">
          <RolePlayer>
            <DomainClassMoniker Name="FieldMetadata" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="49d512a7-4209-42e8-b803-65a006e4e87c" Description="" Name="nHydrateModelHasFunctions" DisplayName="NHydrate Model Has Functions" Namespace="nHydrate.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="842bef0d-faae-40f9-bfc0-ab1e3f20e4a4" Description="" Name="nHydrateModel" DisplayName="nHydrate Model" PropertyName="Functions" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Functions">
          <RolePlayer>
            <DomainClassMoniker Name="nHydrateModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="5b22120e-1b7a-41ed-a0ce-d955098d7ea8" Description="" Name="Function" DisplayName="Function" PropertyName="nHydrateModel" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="nHydrate Model">
          <RolePlayer>
            <DomainClassMoniker Name="Function" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="9a1cb63f-3055-4763-b2bf-0929d7853416" Description="" Name="FunctionHasParameters" DisplayName="Function Has Parameters" Namespace="nHydrate.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="88d7fe70-c77a-49af-b9de-512e58adeb79" Description="" Name="Function" DisplayName="Function" PropertyName="Parameters" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Parameters">
          <RolePlayer>
            <DomainClassMoniker Name="Function" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="01353901-6ff3-4a4a-b8cf-887b52e4c7c2" Description="" Name="FunctionParameter" DisplayName="Function Parameter" PropertyName="Function" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Function">
          <RolePlayer>
            <DomainClassMoniker Name="FunctionParameter" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="14ca3dd8-4973-439b-a145-0ec6525bb4e4" Description="" Name="FunctionHasFields" DisplayName="Function Has Fields" Namespace="nHydrate.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="451ee2f4-7d2a-4a33-aed9-4ed1dfd43396" Description="" Name="Function" DisplayName="Function" PropertyName="Fields" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Fields">
          <RolePlayer>
            <DomainClassMoniker Name="Function" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="a36eb8f4-c611-4a64-ad22-2db3c798cda4" Description="" Name="FunctionField" DisplayName="Function Field" PropertyName="Function" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Function">
          <RolePlayer>
            <DomainClassMoniker Name="FunctionField" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="7dc59c60-a41c-40cd-8089-23ede64cf6ba" Description="" Name="nHydrateModelHasModules" DisplayName="NHydrate Model Has Modules" Namespace="nHydrate.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="536fb7b9-3fc4-42e0-992a-ee28c53b75d6" Description="" Name="nHydrateModel" DisplayName="NHydrate Model" PropertyName="Modules" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Modules">
          <RolePlayer>
            <DomainClassMoniker Name="nHydrateModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="fc4998e0-9a90-49b4-956d-190bbd952e61" Description="" Name="Module" DisplayName="Module" PropertyName="nHydrateModel" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="nHydrate Model">
          <RolePlayer>
            <DomainClassMoniker Name="Module" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="f1cd3837-4ae7-4258-9092-f6af1931d0e6" Description="" Name="FunctionReferencesModules" DisplayName="Function References Modules" Namespace="nHydrate.Dsl">
      <Source>
        <DomainRole Id="bc343ccc-e73c-401a-b686-fe036ef819fe" Description="" Name="Function" DisplayName="Function" PropertyName="Modules" PropertyDisplayName="Modules">
          <RolePlayer>
            <DomainClassMoniker Name="Function" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="c9223f95-89ed-43b9-b4f1-18acf2b072e8" Description="" Name="Module" DisplayName="Module" PropertyName="Functions" PropertyDisplayName="Functions">
          <RolePlayer>
            <DomainClassMoniker Name="Module" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="864ca82b-4b9f-4789-aff6-8d09a35458be" Description="" Name="ViewReferencesModules" DisplayName="View References Modules" Namespace="nHydrate.Dsl">
      <Source>
        <DomainRole Id="e3badaaf-3c03-4591-bc64-3a24b52672be" Description="" Name="View" DisplayName="View" PropertyName="Modules" PropertyDisplayName="Modules">
          <RolePlayer>
            <DomainClassMoniker Name="View" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="e0aa72a9-f768-42d6-8cdd-761c0e29b5bf" Description="" Name="Module" DisplayName="Module" PropertyName="Views" PropertyDisplayName="Views">
          <RolePlayer>
            <DomainClassMoniker Name="Module" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="14f0b73a-8b9c-440f-8104-95dd375446e9" Description="" Name="StoredProcedureReferencesModules" DisplayName="Stored Procedure References Modules" Namespace="nHydrate.Dsl">
      <Source>
        <DomainRole Id="74dc297f-50d0-407d-865c-b02d1dbf55fd" Description="" Name="StoredProcedure" DisplayName="Stored Procedure" PropertyName="Modules" PropertyDisplayName="Modules">
          <RolePlayer>
            <DomainClassMoniker Name="StoredProcedure" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="974a57aa-20c4-4835-b4da-65adb984e565" Description="" Name="Module" DisplayName="Module" PropertyName="StoredProcedures" PropertyDisplayName="Stored Procedures">
          <RolePlayer>
            <DomainClassMoniker Name="Module" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="bc309d93-1b8b-491e-9e04-7114b1f3991c" Description="" Name="EntityReferencesModules" DisplayName="Entity References Modules" Namespace="nHydrate.Dsl">
      <Source>
        <DomainRole Id="9921890f-3210-4d22-a80b-ad60a04babf4" Description="" Name="Entity" DisplayName="Entity" PropertyName="Modules" PropertyDisplayName="Modules">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="634b3be3-b507-4353-93e7-1e35acb25a2d" Description="" Name="Module" DisplayName="Module" PropertyName="Entities" PropertyDisplayName="Entities">
          <RolePlayer>
            <DomainClassMoniker Name="Module" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="f07e8dd2-a74e-4e61-b7f7-b704a373d9c0" Description="" Name="FieldReferencesModules" DisplayName="Field References Modules" Namespace="nHydrate.Dsl">
      <Source>
        <DomainRole Id="ba2d3941-91ea-4ad6-9922-11adc3ea6303" Description="" Name="Field" DisplayName="Field" PropertyName="Modules" PropertyDisplayName="Modules">
          <RolePlayer>
            <DomainClassMoniker Name="Field" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="18791071-92e0-4629-b63f-9d80f106413c" Description="" Name="Module" DisplayName="Module" PropertyName="Fields" PropertyDisplayName="Fields">
          <RolePlayer>
            <DomainClassMoniker Name="Module" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="d6fcf125-9fc1-4537-a9fc-80b04ab84d22" Description="" Name="nHydrateModelHasRelationModules" DisplayName="nHydrate Model Has Relation Modules" Namespace="nHydrate.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="b398c97f-1a9d-40a2-b428-944678ec75eb" Description="" Name="nHydrateModel" DisplayName="nHydrate Model" PropertyName="RelationModules" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" IsPropertyBrowsable="false" PropertyDisplayName="Relation Modules">
          <RolePlayer>
            <DomainClassMoniker Name="nHydrateModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="5b5fe496-4fcb-47b7-a6f8-9c06961b103c" Description="" Name="RelationModule" DisplayName="Relation Module" PropertyName="nHydrateModel" Multiplicity="One" PropagatesDelete="true" IsPropertyBrowsable="false" PropertyDisplayName="nHydrate Model">
          <RolePlayer>
            <DomainClassMoniker Name="RelationModule" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="63aea98f-70cb-494c-8c26-8d263ba80627" Description="" Name="ModuleHasModuleRules" DisplayName="Module Has Module Rules" Namespace="nHydrate.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="c7332f35-4438-4f33-945e-6af7f43330cc" Description="" Name="Module" DisplayName="Module" PropertyName="ModuleRules" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Module Rules">
          <RolePlayer>
            <DomainClassMoniker Name="Module" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="249cd8e2-030c-4c5a-bcde-4c055b4d052e" Description="" Name="ModuleRule" DisplayName="Module Rule" PropertyName="Module" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Module">
          <RolePlayer>
            <DomainClassMoniker Name="ModuleRule" />
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
    <DomainRelationship Id="8b52efcf-2a19-449a-b104-9dd450281764" Description="Description for nHydrate.Dsl.nHydrateModelHasModelMetadata" Name="nHydrateModelHasModelMetadata" DisplayName="NHydrate Model Has Model Metadata" Namespace="nHydrate.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="fe8660eb-c0c9-4bab-ad2a-535afb8d2d9d" Description="Description for nHydrate.Dsl.nHydrateModelHasModelMetadata.nHydrateModel" Name="nHydrateModel" DisplayName="NHydrate Model" PropertyName="ModelMetadata" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Model Metadata">
          <RolePlayer>
            <DomainClassMoniker Name="nHydrateModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="edcb86d4-6b8a-470f-9ec0-2a98017c5c12" Description="Description for nHydrate.Dsl.nHydrateModelHasModelMetadata.ModelMetadata" Name="ModelMetadata" DisplayName="Model Metadata" PropertyName="nHydrateModel" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="NHydrate Model">
          <RolePlayer>
            <DomainClassMoniker Name="ModelMetadata" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="7ee38be2-ed4a-46a1-ae1a-3a61bd64e20f" Description="Association relationship between an entity and a view" Name="EntityHasViews" DisplayName="Entity Has Views" Namespace="nHydrate.Dsl" HasCustomConstructor="true" GeneratesDoubleDerived="true" AllowsDuplicates="true">
      <Properties>
        <DomainProperty Id="89ec5e65-db89-4578-8da3-2b6b453e72bb" Description="The named relation necessary when there is more than one relation between two entities" Name="RoleName" DisplayName="Role Name" Category="Definition">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="cfababba-9bc1-4ccc-93c6-2eb7a409850d" Description="Internal data to track imports" Name="ImportData" DisplayName="Import Data" IsBrowsable="false">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="7311f0de-9fed-4f35-abfa-b8b79f2b9ff4" Description="Determines summary text were applicable" Name="Summary" DisplayName="Summary" Category="Documentation">
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
      </Properties>
      <Source>
        <DomainRole Id="980aab21-bcfb-4594-bce6-e892e91039a1" Description="Description for nHydrate.Dsl.EntityHasViews.ParentEntity" Name="ParentEntity" DisplayName="Parent Entity" PropertyName="ChildViews" PropertyDisplayName="ChildViews">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="19b19e10-a398-468d-a504-aa47bd30287b" Description="Description for nHydrate.Dsl.EntityHasViews.ChildView" Name="ChildView" DisplayName="Child View" PropertyName="ParentEntity" PropertyDisplayName="Parent Entity">
          <RolePlayer>
            <DomainClassMoniker Name="View" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="4ce7e656-14e7-421e-9368-3091700c1eda" Description="Description for nHydrate.Dsl.nHydrateModelHasIndexModules" Name="nHydrateModelHasIndexModules" DisplayName="NHydrate Model Has Index Modules" Namespace="nHydrate.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="3b7768ac-9f51-4b4e-af67-3c2d59f179a1" Description="Description for nHydrate.Dsl.nHydrateModelHasIndexModules.nHydrateModel" Name="nHydrateModel" DisplayName="NHydrate Model" PropertyName="IndexModules" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Index Modules">
          <RolePlayer>
            <DomainClassMoniker Name="nHydrateModel" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="94543612-9ccf-4034-b59e-746d39d2d19e" Description="Description for nHydrate.Dsl.nHydrateModelHasIndexModules.IndexModule" Name="IndexModule" DisplayName="Index Module" PropertyName="nHydrateModel" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="NHydrate Model">
          <RolePlayer>
            <DomainClassMoniker Name="IndexModule" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="daa3bb78-a012-4074-b164-446cdf4e7c63" Description="Description for nHydrate.Dsl.EntityHasSecurityFunction" Name="EntityHasSecurityFunction" DisplayName="Entity Has Security Function" Namespace="nHydrate.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="43c74558-2e05-4067-bad8-9dd65eddeeaf" Description="Description for nHydrate.Dsl.EntityHasSecurityFunction.Entity" Name="Entity" DisplayName="Entity" PropertyName="SecurityFunction" Multiplicity="ZeroOne" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" Category="Security" PropertyDisplayName="Security Function">
          <RolePlayer>
            <DomainClassMoniker Name="Entity" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="96cc7081-dfc6-4acc-ac93-67c5cde791df" Description="Description for nHydrate.Dsl.EntityHasSecurityFunction.SecurityFunction" Name="SecurityFunction" DisplayName="Security Function" PropertyName="Entity" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Entity">
          <RolePlayer>
            <DomainClassMoniker Name="SecurityFunction" />
          </RolePlayer>
        </DomainRole>
      </Target>
    </DomainRelationship>
    <DomainRelationship Id="a49d2dab-f2d3-44cd-9573-468fa8928e4e" Description="Description for nHydrate.Dsl.SecurityFunctionHasSecurityFunctionParameters" Name="SecurityFunctionHasSecurityFunctionParameters" DisplayName="Security Function Has Security Function Parameters" Namespace="nHydrate.Dsl" IsEmbedding="true">
      <Source>
        <DomainRole Id="43336824-46a7-4b78-995a-bdd257c22603" Description="Description for nHydrate.Dsl.SecurityFunctionHasSecurityFunctionParameters.SecurityFunction" Name="SecurityFunction" DisplayName="Security Function" PropertyName="SecurityFunctionParameters" PropagatesCopy="PropagatesCopyToLinkAndOppositeRolePlayer" PropertyDisplayName="Security Function Parameters">
          <RolePlayer>
            <DomainClassMoniker Name="SecurityFunction" />
          </RolePlayer>
        </DomainRole>
      </Source>
      <Target>
        <DomainRole Id="20c53308-6b36-494f-9eba-ff83a9a74218" Description="Description for nHydrate.Dsl.SecurityFunctionHasSecurityFunctionParameters.SecurityFunctionParameter" Name="SecurityFunctionParameter" DisplayName="Security Function Parameter" PropertyName="SecurityFunction" Multiplicity="One" PropagatesDelete="true" PropertyDisplayName="Security Function">
          <RolePlayer>
            <DomainClassMoniker Name="SecurityFunctionParameter" />
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
    <DomainEnumeration Name="DatabaseTypeConstants" Namespace="nHydrate.Dsl" Description="">
      <Literals>
        <EnumerationLiteral Description="" Name="SQL2005" Value="" />
        <EnumerationLiteral Description="" Name="SQLAzure" Value="" />
        <EnumerationLiteral Description="" Name="SQL2008" Value="" />
      </Literals>
    </DomainEnumeration>
    <ExternalType Name="Color" Namespace="System.Drawing" />
    <ExternalType Name="DashStyle" Namespace="System.Drawing.Drawing2D" />
    <ExternalType Name="LinearGradientMode" Namespace="System.Drawing.Drawing2D" />
    <DomainEnumeration Name="ModuleRuleStatusTypeConstants" Namespace="nHydrate.Dsl" Description="">
      <Literals>
        <EnumerationLiteral Description="Enforces that all objects should be in common with the dependent module" Name="Subset" Value="" />
        <EnumerationLiteral Description="Enforces that no objects should be in common with the dependent module" Name="Outerset" Value="" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="TypedEntityConstants" Namespace="nHydrate.Dsl" Description="Description for nHydrate.Dsl.TypedEntityConstants">
      <Literals>
        <EnumerationLiteral Description="This is not a typed entity" Name="None" Value="" />
        <EnumerationLiteral Description="The typed entity has a backing database table" Name="DatabaseTable" Value="" />
        <EnumerationLiteral Description="The typed entity is a code-only enumeration" Name="EnumOnly" Value="" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="ModuleRuleInclusionTypeConstants" Namespace="nHydrate.Dsl" Description="A list of options for module rule inclusions">
      <Literals>
        <EnumerationLiteral Description="Include no objects in this rule validation" Name="None" Value="0" />
        <EnumerationLiteral Description="Include views in this rule validation" Name="View" Value="2" />
        <EnumerationLiteral Description="Include stored procedures in this rule validation" Name="StoredProcedure" Value="4" />
        <EnumerationLiteral Description="Include entities in this rule validation" Name="Entity" Value="1" />
        <EnumerationLiteral Description="Include functions in this rule validation" Name="Function" Value="8" />
      </Literals>
      <Attributes>
        <ClrAttribute Name="System.Flags" />
      </Attributes>
    </DomainEnumeration>
    <DomainEnumeration Name="IndexTypeConstants" Namespace="nHydrate.Dsl" Description="">
      <Literals>
        <EnumerationLiteral Description="" Name="PrimaryKey" Value="" />
        <EnumerationLiteral Description="" Name="IsIndexed" Value="" />
        <EnumerationLiteral Description="" Name="User" Value="" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="VisibilityTypeConstants" Namespace="nHydrate.Dsl" Description="A list of options for module rule inclusions">
      <Literals>
        <EnumerationLiteral Description="Show views on the diagram" Name="View" Value="4" />
        <EnumerationLiteral Description="Show stored procedures on the diagram" Name="StoredProcedure" Value="2" />
        <EnumerationLiteral Description="Show functions on the diagram" Name="Function" Value="1" />
        <EnumerationLiteral Description="" Name="None" Value="0" />
      </Literals>
      <Attributes>
        <ClrAttribute Name="System.Flags" />
      </Attributes>
    </DomainEnumeration>
    <DomainEnumeration Name="UIDataTypeConstants" Namespace="nHydrate.Dsl" Description="">
      <Literals>
        <EnumerationLiteral Description="" Name="Custom" Value="0" />
        <EnumerationLiteral Description="" Name="DateTime" Value="1" />
        <EnumerationLiteral Description="" Name="Date" Value="2" />
        <EnumerationLiteral Description="" Name="Time" Value="3" />
        <EnumerationLiteral Description="" Name="PhoneNumber" Value="5" />
        <EnumerationLiteral Description="" Name="Duration" Value="4" />
        <EnumerationLiteral Description="" Name="Currency" Value="6" />
        <EnumerationLiteral Description="" Name="Html" Value="8" />
        <EnumerationLiteral Description="" Name="Text" Value="7" />
        <EnumerationLiteral Description="" Name="MultilineText" Value="9" />
        <EnumerationLiteral Description="" Name="EmailAddress" Value="10" />
        <EnumerationLiteral Description="" Name="Password" Value="11" />
        <EnumerationLiteral Description="" Name="Url" Value="12" />
        <EnumerationLiteral Description="" Name="ImageUrl" Value="13" />
      </Literals>
    </DomainEnumeration>
    <DomainEnumeration Name="EFVersionConstants" Namespace="nHydrate.Dsl" Description="">
      <Literals>
        <EnumerationLiteral Description="EF 4" Name="EF4" Value="" />
        <EnumerationLiteral Description="" Name="EF6" Value="" />
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
    <CompartmentShape Id="07b75a7a-ef2b-4128-a9e3-7157cd97f6ed" Description="" Name="StoredProcedureShape" DisplayName="Stored Procedure Shape" Namespace="nHydrate.Dsl" HasCustomConstructor="true" GeneratesDoubleDerived="true" TooltipType="Variable" FixedTooltipText="Stored Procedure Shape" FillColor="255, 224, 192" OutlineColor="192, 255, 192" InitialWidth="2" InitialHeight="0.3" OutlineThickness="0.01" FillGradientMode="None" HasDefaultConnectionPoints="true" Geometry="Rectangle">
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.25" VerticalOffset="0">
        <TextDecorator Name="StoredProcedureTextDecorator" DisplayName="" DefaultText="" FontStyle="Bold" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopRight" HorizontalOffset="0" VerticalOffset="0">
        <ExpandCollapseDecorator Name="StoredProcedureExpandDecorator" DisplayName="" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.05" VerticalOffset="0">
        <IconDecorator Name="StoredProcedureIconDecorator" DisplayName="Stored Procedure Icon Decorator" DefaultIcon="Resources\storedproc.png" />
      </ShapeHasDecorators>
      <Compartment FillColor="WhiteSmoke" TitleFillColor="Gainsboro" Name="StoredProcedureFieldCompartment" Title="Fields" />
      <Compartment FillColor="WhiteSmoke" TitleFillColor="Gainsboro" Name="StoredProcedureParameterCompartment" Title="Parameters" />
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
    <CompartmentShape Id="cb67d1b7-d8ed-49d9-b750-9ac6129b2422" Description="" Name="EntityCompositeShape" DisplayName="Entity Composite Shape" Namespace="nHydrate.Dsl" GeneratesDoubleDerived="true" TooltipType="Variable" FixedTooltipText="Entity Composite Shape" FillColor="Gainsboro" InitialHeight="0.3" OutlineThickness="0.01" FillGradientMode="None" Geometry="Rectangle" IsSingleCompartmentHeaderVisible="false" DefaultExpandCollapseState="Collapsed">
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.25" VerticalOffset="0">
        <TextDecorator Name="EntityCompositeShapeTextDecorator" DisplayName="" DefaultText="" FontStyle="Bold" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.05" VerticalOffset="0">
        <IconDecorator Name="EntityCompositeShapeIconDecorator" DisplayName="" DefaultIcon="Resources\composite.png" />
      </ShapeHasDecorators>
    </CompartmentShape>
    <CompartmentShape Id="66621fb1-7d1f-4aef-a5f2-d1c5b92dbf17" Description="" Name="FunctionShape" DisplayName="Function Shape" Namespace="nHydrate.Dsl" HasCustomConstructor="true" GeneratesDoubleDerived="true" FixedTooltipText="Function Shape" FillColor="SandyBrown" OutlineColor="255, 128, 255" InitialWidth="2" InitialHeight="0.3" OutlineThickness="0.01" FillGradientMode="None" HasDefaultConnectionPoints="true" Geometry="Rectangle">
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.25" VerticalOffset="0">
        <TextDecorator Name="FunctionTextDecorator" DisplayName="Function Text Decorator" DefaultText="FunctionTextDecorator" FontStyle="Bold" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopLeft" HorizontalOffset="0.05" VerticalOffset="0">
        <IconDecorator Name="FunctionIconDecorator" DisplayName="Function Icon Decorator" DefaultIcon="Resources\function.png" />
      </ShapeHasDecorators>
      <ShapeHasDecorators Position="InnerTopRight" HorizontalOffset="0" VerticalOffset="0">
        <ExpandCollapseDecorator Name="FunctionExpandCollapseDecorator" DisplayName="Function Expand Collapse Decorator" />
      </ShapeHasDecorators>
      <Compartment FillColor="WhiteSmoke" TitleFillColor="Gainsboro" Name="ParameterCompartment" Title="Parameters" />
      <Compartment FillColor="WhiteSmoke" TitleFillColor="Gainsboro" Name="FieldCompartment" Title="Fields" />
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
    <Connector Id="1d2aa755-243b-474b-8208-5b11989ecba1" Description="Creates an inheritance relationship between two entities" Name="EntityInheritanceConnector" DisplayName="Entity Inheritance Connector" Namespace="nHydrate.Dsl" HasCustomConstructor="true" GeneratesDoubleDerived="true" TooltipType="Variable" FixedTooltipText="Creates an inheritance relationship between two entities" Color="DimGray" TargetEndStyle="HollowArrow" Thickness="0.01">
      <Properties>
        <DomainProperty Id="3837e412-7144-401b-8572-41adddba839f" Description="" Name="BaseType" DisplayName="Base Type" Kind="Calculated">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
        <DomainProperty Id="f3452069-aecf-4f34-9660-84941860f476" Description="" Name="DerivedType" DisplayName="Derived Type" Kind="Calculated">
          <Type>
            <ExternalTypeMoniker Name="/System/String" />
          </Type>
        </DomainProperty>
      </Properties>
    </Connector>
    <Connector Id="62c983a8-9996-45b6-8c2a-56a8f9b40e04" Description="" Name="EntityCompositeConnector" DisplayName="Composite Connector" Namespace="nHydrate.Dsl" GeneratesDoubleDerived="true" TooltipType="Variable" FixedTooltipText="" Color="DimGray" DashStyle="Dash" SourceEndStyle="FilledDiamond" Thickness="0.01" />
    <Connector Id="1715d1db-8274-453b-8248-a36e795c89b8" Description="Connect an entity and a view" Name="EntityViewAssociationConnector" DisplayName="Entity View Association Connector" Namespace="nHydrate.Dsl" TooltipType="Variable" FixedTooltipText="Entity View Association Connector" TextColor="DimGray" Color="DimGray" SourceEndStyle="EmptyDiamond" TargetEndStyle="EmptyArrow" Thickness="0.01" />
  </Connectors>
  <XmlSerializationBehavior Name="nHydrateSerializationBehavior" Namespace="nHydrate.Dsl">
    <ClassData>
      <XmlClassData TypeName="NHydrateModel" MonikerAttributeName="" SerializeId="true" MonikerElementName="nHydrateModelMoniker" ElementName="nHydrateModel" MonikerTypeName="NHydrateModelMoniker">
        <DomainClassMoniker Name="nHydrateModel" />
        <ElementData>
          <XmlRelationshipData RoleElementName="entities">
            <DomainRelationshipMoniker Name="nHydrateModelHasEntities" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="storedProcedures">
            <DomainRelationshipMoniker Name="nHydrateModelHasStoredProcedures" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="views">
            <DomainRelationshipMoniker Name="nHydrateModelHasViews" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="companyName">
            <DomainPropertyMoniker Name="nHydrateModel/CompanyName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="copyright">
            <DomainPropertyMoniker Name="nHydrateModel/Copyright" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="defaultNamespace">
            <DomainPropertyMoniker Name="nHydrateModel/DefaultNamespace" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="projectName">
            <DomainPropertyMoniker Name="nHydrateModel/ProjectName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="sQLServerType">
            <DomainPropertyMoniker Name="nHydrateModel/SQLServerType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="storedProcedurePrefix">
            <DomainPropertyMoniker Name="nHydrateModel/StoredProcedurePrefix" />
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
          <XmlPropertyData XmlName="transformNames">
            <DomainPropertyMoniker Name="nHydrateModel/TransformNames" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="functions">
            <DomainRelationshipMoniker Name="nHydrateModelHasFunctions" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="modules">
            <DomainRelationshipMoniker Name="nHydrateModelHasModules" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="useModules">
            <DomainPropertyMoniker Name="nHydrateModel/UseModules" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="relationModules">
            <DomainRelationshipMoniker Name="nHydrateModelHasRelationModules" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="showViews">
            <DomainPropertyMoniker Name="nHydrateModel/ShowViews" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="showStoredProcedures">
            <DomainPropertyMoniker Name="nHydrateModel/ShowStoredProcedures" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="showFunctions">
            <DomainPropertyMoniker Name="nHydrateModel/ShowFunctions" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="collate">
            <DomainPropertyMoniker Name="nHydrateModel/Collate" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="modelToDisk">
            <DomainPropertyMoniker Name="nHydrateModel/ModelToDisk" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="maxPrecedenceOrder">
            <DomainPropertyMoniker Name="nHydrateModel/MaxPrecedenceOrder" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="modelVersion">
            <DomainPropertyMoniker Name="nHydrateModel/ModelVersion" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="companyURL">
            <DomainPropertyMoniker Name="nHydrateModel/CompanyURL" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="companyPhone">
            <DomainPropertyMoniker Name="nHydrateModel/CompanyPhone" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="modelMetadata">
            <DomainRelationshipMoniker Name="nHydrateModelHasModelMetadata" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="useGeneratedCRUD">
            <DomainPropertyMoniker Name="nHydrateModel/UseGeneratedCRUD" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="diagramVisibility">
            <DomainPropertyMoniker Name="nHydrateModel/DiagramVisibility" />
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
          <XmlRelationshipData UseFullForm="true" RoleElementName="indexModules">
            <DomainRelationshipMoniker Name="nHydrateModelHasIndexModules" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="eFVersion">
            <DomainPropertyMoniker Name="nHydrateModel/EFVersion" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="emitSafetyScripts">
            <DomainPropertyMoniker Name="nHydrateModel/EmitSafetyScripts" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="allowMocks">
            <DomainPropertyMoniker Name="nHydrateModel/AllowMocks" />
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
          <XmlPropertyData XmlName="allowAuditTracking">
            <DomainPropertyMoniker Name="Entity/AllowAuditTracking" />
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
          <XmlPropertyData XmlName="enforcePrimaryKey">
            <DomainPropertyMoniker Name="Entity/EnforcePrimaryKey" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isGenerated">
            <DomainPropertyMoniker Name="Entity/IsGenerated" />
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
          <XmlRelationshipData UseFullForm="true" RoleElementName="parentInheritedEntity">
            <DomainRelationshipMoniker Name="EntityInheritsEntity" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="composites">
            <DomainRelationshipMoniker Name="EntityHasComposites" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="entityMetadata">
            <DomainRelationshipMoniker Name="EntityHasMetadata" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="modules">
            <DomainRelationshipMoniker Name="EntityReferencesModules" />
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
          <XmlRelationshipData UseFullForm="true" RoleElementName="childViews">
            <DomainRelationshipMoniker Name="EntityHasViews" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="isTenant">
            <DomainPropertyMoniker Name="Entity/IsTenant" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="securityFunction">
            <DomainRelationshipMoniker Name="EntityHasSecurityFunction" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="security">
            <DomainPropertyMoniker Name="Entity/Security" />
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
          <XmlPropertyData XmlName="friendlyName">
            <DomainPropertyMoniker Name="Field/FriendlyName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="nullable">
            <DomainPropertyMoniker Name="Field/Nullable" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="collate">
            <DomainPropertyMoniker Name="Field/Collate" />
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
          <XmlPropertyData XmlName="isGenerated">
            <DomainPropertyMoniker Name="Field/IsGenerated" />
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
          <XmlPropertyData XmlName="min">
            <DomainPropertyMoniker Name="Field/Min" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="max">
            <DomainPropertyMoniker Name="Field/Max" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isPrimaryKey">
            <DomainPropertyMoniker Name="Field/IsPrimaryKey" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="scale">
            <DomainPropertyMoniker Name="Field/Scale" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="validationExpression">
            <DomainPropertyMoniker Name="Field/ValidationExpression" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="codeFacade">
            <DomainPropertyMoniker Name="Field/CodeFacade" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="fieldMetadata">
            <DomainRelationshipMoniker Name="FieldHasMetadata" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="isReadOnly">
            <DomainPropertyMoniker Name="Field/IsReadOnly" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isBrowsable">
            <DomainPropertyMoniker Name="Field/IsBrowsable" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="modules">
            <DomainRelationshipMoniker Name="FieldReferencesModules" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="category">
            <DomainPropertyMoniker Name="Field/Category" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="sortOrder">
            <DomainPropertyMoniker Name="Field/SortOrder" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="dataFormatString">
            <DomainPropertyMoniker Name="Field/DataFormatString" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="uIDataType">
            <DomainPropertyMoniker Name="Field/UIDataType" />
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
      <XmlClassData TypeName="NHydrateModelHasStoredProcedures" MonikerAttributeName="" SerializeId="true" MonikerElementName="nHydrateModelHasStoredProceduresMoniker" ElementName="nHydrateModelHasStoredProcedures" MonikerTypeName="NHydrateModelHasStoredProceduresMoniker">
        <DomainRelationshipMoniker Name="nHydrateModelHasStoredProcedures" />
      </XmlClassData>
      <XmlClassData TypeName="StoredProcedure" MonikerAttributeName="" SerializeId="true" MonikerElementName="storedProcedureMoniker" ElementName="storedProcedure" MonikerTypeName="StoredProcedureMoniker">
        <DomainClassMoniker Name="StoredProcedure" />
        <ElementData>
          <XmlPropertyData XmlName="sQL">
            <DomainPropertyMoniker Name="StoredProcedure/SQL" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="fields">
            <DomainRelationshipMoniker Name="StoredProcedureHasFields" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="StoredProcedure/Name" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="parameters">
            <DomainRelationshipMoniker Name="StoredProcedureHasParameters" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="isGenerated">
            <DomainPropertyMoniker Name="StoredProcedure/IsGenerated" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="codeFacade">
            <DomainPropertyMoniker Name="StoredProcedure/CodeFacade" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="StoredProcedure/Summary" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="schema">
            <DomainPropertyMoniker Name="StoredProcedure/Schema" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isExisting">
            <DomainPropertyMoniker Name="StoredProcedure/IsExisting" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="databaseObjectName">
            <DomainPropertyMoniker Name="StoredProcedure/DatabaseObjectName" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="modules">
            <DomainRelationshipMoniker Name="StoredProcedureReferencesModules" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="generatesDoubleDerived">
            <DomainPropertyMoniker Name="StoredProcedure/GeneratesDoubleDerived" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="precedenceOrder">
            <DomainPropertyMoniker Name="StoredProcedure/PrecedenceOrder" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="StoredProcedureHasFields" MonikerAttributeName="" SerializeId="true" MonikerElementName="storedProcedureHasFieldsMoniker" ElementName="storedProcedureHasFields" MonikerTypeName="StoredProcedureHasFieldsMoniker">
        <DomainRelationshipMoniker Name="StoredProcedureHasFields" />
      </XmlClassData>
      <XmlClassData TypeName="StoredProcedureField" MonikerAttributeName="" SerializeId="true" MonikerElementName="storedProcedureFieldMoniker" ElementName="storedProcedureField" MonikerTypeName="StoredProcedureFieldMoniker">
        <DomainClassMoniker Name="StoredProcedureField" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="StoredProcedureField/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="friendlyName">
            <DomainPropertyMoniker Name="StoredProcedureField/FriendlyName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="codeFacade">
            <DomainPropertyMoniker Name="StoredProcedureField/CodeFacade" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="nullable">
            <DomainPropertyMoniker Name="StoredProcedureField/Nullable" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="dataType">
            <DomainPropertyMoniker Name="StoredProcedureField/DataType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="StoredProcedureField/Summary" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="default">
            <DomainPropertyMoniker Name="StoredProcedureField/Default" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isGenerated">
            <DomainPropertyMoniker Name="StoredProcedureField/IsGenerated" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="length">
            <DomainPropertyMoniker Name="StoredProcedureField/Length" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="scale">
            <DomainPropertyMoniker Name="StoredProcedureField/Scale" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="StoredProcedureShape" MonikerAttributeName="" SerializeId="true" MonikerElementName="storedProcedureShapeMoniker" ElementName="storedProcedureShape" MonikerTypeName="StoredProcedureShapeMoniker">
        <CompartmentShapeMoniker Name="StoredProcedureShape" />
      </XmlClassData>
      <XmlClassData TypeName="StoredProcedureHasParameters" MonikerAttributeName="" SerializeId="true" MonikerElementName="storedProcedureHasParametersMoniker" ElementName="storedProcedureHasParameters" MonikerTypeName="StoredProcedureHasParametersMoniker">
        <DomainRelationshipMoniker Name="StoredProcedureHasParameters" />
      </XmlClassData>
      <XmlClassData TypeName="StoredProcedureParameter" MonikerAttributeName="" SerializeId="true" MonikerElementName="storedProcedureParameterMoniker" ElementName="storedProcedureParameter" MonikerTypeName="StoredProcedureParameterMoniker">
        <DomainClassMoniker Name="StoredProcedureParameter" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="StoredProcedureParameter/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="codeFacade">
            <DomainPropertyMoniker Name="StoredProcedureParameter/CodeFacade" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="nullable">
            <DomainPropertyMoniker Name="StoredProcedureParameter/Nullable" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="dataType">
            <DomainPropertyMoniker Name="StoredProcedureParameter/DataType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="StoredProcedureParameter/Summary" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="default">
            <DomainPropertyMoniker Name="StoredProcedureParameter/Default" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isGenerated">
            <DomainPropertyMoniker Name="StoredProcedureParameter/IsGenerated" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="length">
            <DomainPropertyMoniker Name="StoredProcedureParameter/Length" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isOutputParameter">
            <DomainPropertyMoniker Name="StoredProcedureParameter/IsOutputParameter" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="scale">
            <DomainPropertyMoniker Name="StoredProcedureParameter/Scale" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="sortOrder">
            <DomainPropertyMoniker Name="StoredProcedureParameter/SortOrder" />
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
          <XmlPropertyData XmlName="isGenerated">
            <DomainPropertyMoniker Name="View/IsGenerated" />
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
          <XmlRelationshipData UseFullForm="true" RoleElementName="modules">
            <DomainRelationshipMoniker Name="ViewReferencesModules" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="generatesDoubleDerived">
            <DomainPropertyMoniker Name="View/GeneratesDoubleDerived" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="precedenceOrder">
            <DomainPropertyMoniker Name="View/PrecedenceOrder" />
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
          <XmlPropertyData XmlName="friendlyName">
            <DomainPropertyMoniker Name="ViewField/FriendlyName" />
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
          <XmlPropertyData XmlName="isGenerated">
            <DomainPropertyMoniker Name="ViewField/IsGenerated" />
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
      <XmlClassData TypeName="EntityInheritsEntity" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityInheritsEntityMoniker" ElementName="entityInheritsEntity" MonikerTypeName="EntityInheritsEntityMoniker">
        <DomainRelationshipMoniker Name="EntityInheritsEntity" />
        <ElementData>
          <XmlPropertyData XmlName="roleName">
            <DomainPropertyMoniker Name="EntityInheritsEntity/RoleName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isEnforced">
            <DomainPropertyMoniker Name="EntityInheritsEntity/IsEnforced" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="EntityInheritsEntity/Summary" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EntityInheritanceConnector" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityInheritanceConnectorMoniker" ElementName="entityInheritanceConnector" MonikerTypeName="EntityInheritanceConnectorMoniker">
        <ConnectorMoniker Name="EntityInheritanceConnector" />
        <ElementData>
          <XmlPropertyData XmlName="baseType" Representation="Ignore">
            <DomainPropertyMoniker Name="EntityInheritanceConnector/BaseType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="derivedType" Representation="Ignore">
            <DomainPropertyMoniker Name="EntityInheritanceConnector/DerivedType" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EntityHasComposites" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityHasCompositesMoniker" ElementName="entityHasComposites" MonikerTypeName="EntityHasCompositesMoniker">
        <DomainRelationshipMoniker Name="EntityHasComposites" />
      </XmlClassData>
      <XmlClassData TypeName="Composite" MonikerAttributeName="" SerializeId="true" MonikerElementName="compositeMoniker" ElementName="composite" MonikerTypeName="CompositeMoniker">
        <DomainClassMoniker Name="Composite" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="Composite/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="codeFacade">
            <DomainPropertyMoniker Name="Composite/CodeFacade" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isGenerated">
            <DomainPropertyMoniker Name="Composite/IsGenerated" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="Composite/Summary" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="fields">
            <DomainRelationshipMoniker Name="CompositeHasFields" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="CompositeHasFields" MonikerAttributeName="" SerializeId="true" MonikerElementName="compositeHasFieldsMoniker" ElementName="compositeHasFields" MonikerTypeName="CompositeHasFieldsMoniker">
        <DomainRelationshipMoniker Name="CompositeHasFields" />
      </XmlClassData>
      <XmlClassData TypeName="CompositeField" MonikerAttributeName="" SerializeId="true" MonikerElementName="compositeFieldMoniker" ElementName="compositeField" MonikerTypeName="CompositeFieldMoniker">
        <DomainClassMoniker Name="CompositeField" />
        <ElementData>
          <XmlPropertyData XmlName="fieldId">
            <DomainPropertyMoniker Name="CompositeField/FieldId" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EntityCompositeShape" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityCompositeShapeMoniker" ElementName="entityCompositeShape" MonikerTypeName="EntityCompositeShapeMoniker">
        <CompartmentShapeMoniker Name="EntityCompositeShape" />
      </XmlClassData>
      <XmlClassData TypeName="EntityCompositeConnector" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityCompositeConnectorMoniker" ElementName="entityCompositeConnector" MonikerTypeName="EntityCompositeConnectorMoniker">
        <ConnectorMoniker Name="EntityCompositeConnector" />
      </XmlClassData>
      <XmlClassData TypeName="EntityHasMetadata" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityHasMetadataMoniker" ElementName="entityHasMetadata" MonikerTypeName="EntityHasMetadataMoniker">
        <DomainRelationshipMoniker Name="EntityHasMetadata" />
      </XmlClassData>
      <XmlClassData TypeName="EntityMetadata" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityMetadataMoniker" ElementName="entityMetadata" MonikerTypeName="EntityMetadataMoniker">
        <DomainClassMoniker Name="EntityMetadata" />
        <ElementData>
          <XmlPropertyData XmlName="key">
            <DomainPropertyMoniker Name="EntityMetadata/Key" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="value">
            <DomainPropertyMoniker Name="EntityMetadata/Value" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="EntityMetadata/Summary" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="FieldHasMetadata" MonikerAttributeName="" SerializeId="true" MonikerElementName="fieldHasMetadataMoniker" ElementName="fieldHasMetadata" MonikerTypeName="FieldHasMetadataMoniker">
        <DomainRelationshipMoniker Name="FieldHasMetadata" />
      </XmlClassData>
      <XmlClassData TypeName="FieldMetadata" MonikerAttributeName="" SerializeId="true" MonikerElementName="fieldMetadataMoniker" ElementName="fieldMetadata" MonikerTypeName="FieldMetadataMoniker">
        <DomainClassMoniker Name="FieldMetadata" />
        <ElementData>
          <XmlPropertyData XmlName="key">
            <DomainPropertyMoniker Name="FieldMetadata/Key" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="value">
            <DomainPropertyMoniker Name="FieldMetadata/Value" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="FieldMetadata/Summary" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="NHydrateModelHasFunctions" MonikerAttributeName="" SerializeId="true" MonikerElementName="nHydrateModelHasFunctionsMoniker" ElementName="nHydrateModelHasFunctions" MonikerTypeName="NHydrateModelHasFunctionsMoniker">
        <DomainRelationshipMoniker Name="nHydrateModelHasFunctions" />
      </XmlClassData>
      <XmlClassData TypeName="Function" MonikerAttributeName="" SerializeId="true" MonikerElementName="functionMoniker" ElementName="function" MonikerTypeName="FunctionMoniker">
        <DomainClassMoniker Name="Function" />
        <ElementData>
          <XmlPropertyData XmlName="sQL">
            <DomainPropertyMoniker Name="Function/SQL" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="Function/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isGenerated">
            <DomainPropertyMoniker Name="Function/IsGenerated" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="codeFacade">
            <DomainPropertyMoniker Name="Function/CodeFacade" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="Function/Summary" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="schema">
            <DomainPropertyMoniker Name="Function/Schema" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="parameters">
            <DomainRelationshipMoniker Name="FunctionHasParameters" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="fields">
            <DomainRelationshipMoniker Name="FunctionHasFields" />
          </XmlRelationshipData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="modules">
            <DomainRelationshipMoniker Name="FunctionReferencesModules" />
          </XmlRelationshipData>
          <XmlPropertyData XmlName="isTable">
            <DomainPropertyMoniker Name="Function/IsTable" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="returnVariable">
            <DomainPropertyMoniker Name="Function/ReturnVariable" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="precedenceOrder">
            <DomainPropertyMoniker Name="Function/PrecedenceOrder" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="FunctionHasParameters" MonikerAttributeName="" SerializeId="true" MonikerElementName="functionHasParametersMoniker" ElementName="functionHasParameters" MonikerTypeName="FunctionHasParametersMoniker">
        <DomainRelationshipMoniker Name="FunctionHasParameters" />
      </XmlClassData>
      <XmlClassData TypeName="FunctionParameter" MonikerAttributeName="" SerializeId="true" MonikerElementName="functionParameterMoniker" ElementName="functionParameter" MonikerTypeName="FunctionParameterMoniker">
        <DomainClassMoniker Name="FunctionParameter" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="FunctionParameter/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="codeFacade">
            <DomainPropertyMoniker Name="FunctionParameter/CodeFacade" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="nullable">
            <DomainPropertyMoniker Name="FunctionParameter/Nullable" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="dataType">
            <DomainPropertyMoniker Name="FunctionParameter/DataType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="FunctionParameter/Summary" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="default">
            <DomainPropertyMoniker Name="FunctionParameter/Default" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isGenerated">
            <DomainPropertyMoniker Name="FunctionParameter/IsGenerated" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="length">
            <DomainPropertyMoniker Name="FunctionParameter/Length" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="scale">
            <DomainPropertyMoniker Name="FunctionParameter/Scale" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="sortOrder">
            <DomainPropertyMoniker Name="FunctionParameter/SortOrder" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="FunctionHasFields" MonikerAttributeName="" SerializeId="true" MonikerElementName="functionHasFieldsMoniker" ElementName="functionHasFields" MonikerTypeName="FunctionHasFieldsMoniker">
        <DomainRelationshipMoniker Name="FunctionHasFields" />
      </XmlClassData>
      <XmlClassData TypeName="FunctionField" MonikerAttributeName="" SerializeId="true" MonikerElementName="functionFieldMoniker" ElementName="functionField" MonikerTypeName="FunctionFieldMoniker">
        <DomainClassMoniker Name="FunctionField" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="FunctionField/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="friendlyName">
            <DomainPropertyMoniker Name="FunctionField/FriendlyName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="codeFacade">
            <DomainPropertyMoniker Name="FunctionField/CodeFacade" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="nullable">
            <DomainPropertyMoniker Name="FunctionField/Nullable" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="dataType">
            <DomainPropertyMoniker Name="FunctionField/DataType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="FunctionField/Summary" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="default">
            <DomainPropertyMoniker Name="FunctionField/Default" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isGenerated">
            <DomainPropertyMoniker Name="FunctionField/IsGenerated" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="length">
            <DomainPropertyMoniker Name="FunctionField/Length" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="scale">
            <DomainPropertyMoniker Name="FunctionField/Scale" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="FunctionShape" MonikerAttributeName="" SerializeId="true" MonikerElementName="functionShapeMoniker" ElementName="functionShape" MonikerTypeName="FunctionShapeMoniker">
        <CompartmentShapeMoniker Name="FunctionShape" />
      </XmlClassData>
      <XmlClassData TypeName="NHydrateModelHasModules" MonikerAttributeName="" SerializeId="true" MonikerElementName="nHydrateModelHasModulesMoniker" ElementName="nHydrateModelHasModules" MonikerTypeName="NHydrateModelHasModulesMoniker">
        <DomainRelationshipMoniker Name="nHydrateModelHasModules" />
      </XmlClassData>
      <XmlClassData TypeName="Module" MonikerAttributeName="" SerializeId="true" MonikerElementName="moduleMoniker" ElementName="module" MonikerTypeName="ModuleMoniker">
        <DomainClassMoniker Name="Module" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="Module/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="Module/Summary" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="moduleRules">
            <DomainRelationshipMoniker Name="ModuleHasModuleRules" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="FunctionReferencesModules" MonikerAttributeName="" SerializeId="true" MonikerElementName="functionReferencesModulesMoniker" ElementName="functionReferencesModules" MonikerTypeName="FunctionReferencesModulesMoniker">
        <DomainRelationshipMoniker Name="FunctionReferencesModules" />
      </XmlClassData>
      <XmlClassData TypeName="ViewReferencesModules" MonikerAttributeName="" SerializeId="true" MonikerElementName="viewReferencesModulesMoniker" ElementName="viewReferencesModules" MonikerTypeName="ViewReferencesModulesMoniker">
        <DomainRelationshipMoniker Name="ViewReferencesModules" />
      </XmlClassData>
      <XmlClassData TypeName="StoredProcedureReferencesModules" MonikerAttributeName="" SerializeId="true" MonikerElementName="storedProcedureReferencesModulesMoniker" ElementName="storedProcedureReferencesModules" MonikerTypeName="StoredProcedureReferencesModulesMoniker">
        <DomainRelationshipMoniker Name="StoredProcedureReferencesModules" />
      </XmlClassData>
      <XmlClassData TypeName="EntityReferencesModules" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityReferencesModulesMoniker" ElementName="entityReferencesModules" MonikerTypeName="EntityReferencesModulesMoniker">
        <DomainRelationshipMoniker Name="EntityReferencesModules" />
      </XmlClassData>
      <XmlClassData TypeName="FieldReferencesModules" MonikerAttributeName="" SerializeId="true" MonikerElementName="fieldReferencesModulesMoniker" ElementName="fieldReferencesModules" MonikerTypeName="FieldReferencesModulesMoniker">
        <DomainRelationshipMoniker Name="FieldReferencesModules" />
      </XmlClassData>
      <XmlClassData TypeName="NHydrateModelHasRelationModules" MonikerAttributeName="" SerializeId="true" MonikerElementName="nHydrateModelHasRelationModulesMoniker" ElementName="nHydrateModelHasRelationModules" MonikerTypeName="NHydrateModelHasRelationModulesMoniker">
        <DomainRelationshipMoniker Name="nHydrateModelHasRelationModules" />
      </XmlClassData>
      <XmlClassData TypeName="RelationModule" MonikerAttributeName="" SerializeId="true" MonikerElementName="relationModuleMoniker" ElementName="relationModule" MonikerTypeName="RelationModuleMoniker">
        <DomainClassMoniker Name="RelationModule" />
        <ElementData>
          <XmlPropertyData XmlName="relationID">
            <DomainPropertyMoniker Name="RelationModule/RelationID" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="moduleId">
            <DomainPropertyMoniker Name="RelationModule/ModuleId" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="included">
            <DomainPropertyMoniker Name="RelationModule/Included" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isEnforced">
            <DomainPropertyMoniker Name="RelationModule/IsEnforced" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="ModuleHasModuleRules" MonikerAttributeName="" SerializeId="true" MonikerElementName="moduleHasModuleRulesMoniker" ElementName="moduleHasModuleRules" MonikerTypeName="ModuleHasModuleRulesMoniker">
        <DomainRelationshipMoniker Name="ModuleHasModuleRules" />
      </XmlClassData>
      <XmlClassData TypeName="ModuleRule" MonikerAttributeName="" SerializeId="true" MonikerElementName="moduleRuleMoniker" ElementName="moduleRule" MonikerTypeName="ModuleRuleMoniker">
        <DomainClassMoniker Name="ModuleRule" />
        <ElementData>
          <XmlPropertyData XmlName="status">
            <DomainPropertyMoniker Name="ModuleRule/Status" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="dependentModule">
            <DomainPropertyMoniker Name="ModuleRule/DependentModule" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="ModuleRule/Summary" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="ModuleRule/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="inclusion">
            <DomainPropertyMoniker Name="ModuleRule/Inclusion" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="enforced">
            <DomainPropertyMoniker Name="ModuleRule/Enforced" />
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
      <XmlClassData TypeName="NHydrateModelHasModelMetadata" MonikerAttributeName="" SerializeId="true" MonikerElementName="nHydrateModelHasModelMetadataMoniker" ElementName="nHydrateModelHasModelMetadata" MonikerTypeName="NHydrateModelHasModelMetadataMoniker">
        <DomainRelationshipMoniker Name="nHydrateModelHasModelMetadata" />
      </XmlClassData>
      <XmlClassData TypeName="ModelMetadata" MonikerAttributeName="" SerializeId="true" MonikerElementName="modelMetadataMoniker" ElementName="modelMetadata" MonikerTypeName="ModelMetadataMoniker">
        <DomainClassMoniker Name="ModelMetadata" />
        <ElementData>
          <XmlPropertyData XmlName="key">
            <DomainPropertyMoniker Name="ModelMetadata/Key" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="value">
            <DomainPropertyMoniker Name="ModelMetadata/Value" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EntityHasViews" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityHasViewsMoniker" ElementName="entityHasViews" MonikerTypeName="EntityHasViewsMoniker">
        <DomainRelationshipMoniker Name="EntityHasViews" />
        <ElementData>
          <XmlPropertyData XmlName="roleName">
            <DomainPropertyMoniker Name="EntityHasViews/RoleName" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="importData">
            <DomainPropertyMoniker Name="EntityHasViews/ImportData" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="EntityHasViews/Summary" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EntityViewAssociationConnector" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityViewAssociationConnectorMoniker" ElementName="entityViewAssociationConnector" MonikerTypeName="EntityViewAssociationConnectorMoniker">
        <ConnectorMoniker Name="EntityViewAssociationConnector" />
      </XmlClassData>
      <XmlClassData TypeName="NHydrateModelHasIndexModules" MonikerAttributeName="" SerializeId="true" MonikerElementName="nHydrateModelHasIndexModulesMoniker" ElementName="nHydrateModelHasIndexModules" MonikerTypeName="NHydrateModelHasIndexModulesMoniker">
        <DomainRelationshipMoniker Name="nHydrateModelHasIndexModules" />
      </XmlClassData>
      <XmlClassData TypeName="IndexModule" MonikerAttributeName="" SerializeId="true" MonikerElementName="indexModuleMoniker" ElementName="indexModule" MonikerTypeName="IndexModuleMoniker">
        <DomainClassMoniker Name="IndexModule" />
        <ElementData>
          <XmlPropertyData XmlName="indexID">
            <DomainPropertyMoniker Name="IndexModule/IndexID" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="moduleId">
            <DomainPropertyMoniker Name="IndexModule/ModuleId" />
          </XmlPropertyData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="EntityHasSecurityFunction" MonikerAttributeName="" SerializeId="true" MonikerElementName="entityHasSecurityFunctionMoniker" ElementName="entityHasSecurityFunction" MonikerTypeName="EntityHasSecurityFunctionMoniker">
        <DomainRelationshipMoniker Name="EntityHasSecurityFunction" />
      </XmlClassData>
      <XmlClassData TypeName="SecurityFunction" MonikerAttributeName="" SerializeId="true" MonikerElementName="securityFunctionMoniker" ElementName="securityFunction" MonikerTypeName="SecurityFunctionMoniker">
        <DomainClassMoniker Name="SecurityFunction" />
        <ElementData>
          <XmlPropertyData XmlName="sQL">
            <DomainPropertyMoniker Name="SecurityFunction/SQL" />
          </XmlPropertyData>
          <XmlRelationshipData UseFullForm="true" RoleElementName="securityFunctionParameters">
            <DomainRelationshipMoniker Name="SecurityFunctionHasSecurityFunctionParameters" />
          </XmlRelationshipData>
        </ElementData>
      </XmlClassData>
      <XmlClassData TypeName="SecurityFunctionHasSecurityFunctionParameters" MonikerAttributeName="" SerializeId="true" MonikerElementName="securityFunctionHasSecurityFunctionParametersMoniker" ElementName="securityFunctionHasSecurityFunctionParameters" MonikerTypeName="SecurityFunctionHasSecurityFunctionParametersMoniker">
        <DomainRelationshipMoniker Name="SecurityFunctionHasSecurityFunctionParameters" />
      </XmlClassData>
      <XmlClassData TypeName="SecurityFunctionParameter" MonikerAttributeName="" SerializeId="true" MonikerElementName="securityFunctionParameterMoniker" ElementName="securityFunctionParameter" MonikerTypeName="SecurityFunctionParameterMoniker">
        <DomainClassMoniker Name="SecurityFunctionParameter" />
        <ElementData>
          <XmlPropertyData XmlName="name">
            <DomainPropertyMoniker Name="SecurityFunctionParameter/Name" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="nullable">
            <DomainPropertyMoniker Name="SecurityFunctionParameter/Nullable" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="dataType">
            <DomainPropertyMoniker Name="SecurityFunctionParameter/DataType" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="summary">
            <DomainPropertyMoniker Name="SecurityFunctionParameter/Summary" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="default">
            <DomainPropertyMoniker Name="SecurityFunctionParameter/Default" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="isGenerated">
            <DomainPropertyMoniker Name="SecurityFunctionParameter/IsGenerated" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="length">
            <DomainPropertyMoniker Name="SecurityFunctionParameter/Length" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="scale">
            <DomainPropertyMoniker Name="SecurityFunctionParameter/Scale" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="sortOrder">
            <DomainPropertyMoniker Name="SecurityFunctionParameter/SortOrder" />
          </XmlPropertyData>
          <XmlPropertyData XmlName="codeFacade">
            <DomainPropertyMoniker Name="SecurityFunctionParameter/CodeFacade" />
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
          <DomainClassMoniker Name="StoredProcedureField" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\storedproc.png">
        <Class>
          <DomainClassMoniker Name="StoredProcedure" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\parameter.png">
        <Class>
          <DomainClassMoniker Name="StoredProcedureParameter" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\field.png">
        <Class>
          <DomainClassMoniker Name="ViewField" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\composite.png">
        <Class>
          <DomainClassMoniker Name="Composite" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\data.png">
        <Class>
          <DomainClassMoniker Name="EntityMetadata" />
        </Class>
        <PropertyDisplayed>
          <PropertyPath>
            <DomainPropertyMoniker Name="EntityMetadata/Key" />
            <DomainPath />
          </PropertyPath>
        </PropertyDisplayed>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\data.png">
        <Class>
          <DomainClassMoniker Name="FieldMetadata" />
        </Class>
        <PropertyDisplayed>
          <PropertyPath>
            <DomainPropertyMoniker Name="FieldMetadata/Key" />
            <DomainPath />
          </PropertyPath>
        </PropertyDisplayed>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\nhydrate.png">
        <Class>
          <DomainClassMoniker Name="nHydrateModel" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\function.png">
        <Class>
          <DomainClassMoniker Name="Function" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\parameter.png">
        <Class>
          <DomainClassMoniker Name="FunctionParameter" />
        </Class>
      </ExplorerNodeSettings>
      <ExplorerNodeSettings IconToDisplay="Resources\field.png">
        <Class>
          <DomainClassMoniker Name="FunctionField" />
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
      <DomainPath>CompositeHasFields.Fields</DomainPath>
      <DomainPath>EntityReferencesModules.Modules</DomainPath>
      <DomainPath>nHydrateModelHasRelationModules.RelationModules</DomainPath>
      <DomainPath>EntityHasEntities.ChildEntities</DomainPath>
      <DomainPath>EntityHasEntities.ParentEntity</DomainPath>
      <DomainPath>nHydrateModelHasRelationModules.nHydrateModel</DomainPath>
      <DomainPath>EntityHasComposites.Composites</DomainPath>
      <DomainPath>nHydrateModelHasIndexModules.IndexModules</DomainPath>
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
    <ConnectionBuilder Name="EntityInheritsEntityBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="EntityInheritsEntity" />
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
    <ConnectionBuilder Name="FunctionReferencesModulesBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="FunctionReferencesModules" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Function" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Module" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
    <ConnectionBuilder Name="ViewReferencesModulesBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="ViewReferencesModules" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="View" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Module" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
    <ConnectionBuilder Name="StoredProcedureReferencesModulesBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="StoredProcedureReferencesModules" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="StoredProcedure" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Module" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
    <ConnectionBuilder Name="EntityReferencesModulesBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="EntityReferencesModules" />
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
              <DomainClassMoniker Name="Module" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
    <ConnectionBuilder Name="FieldReferencesModulesBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="FieldReferencesModules" />
        <SourceDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Field" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </SourceDirectives>
        <TargetDirectives>
          <RolePlayerConnectDirective>
            <AcceptingClass>
              <DomainClassMoniker Name="Module" />
            </AcceptingClass>
          </RolePlayerConnectDirective>
        </TargetDirectives>
      </LinkConnectDirective>
    </ConnectionBuilder>
    <ConnectionBuilder Name="EntityHasViewsBuilder">
      <LinkConnectDirective>
        <DomainRelationshipMoniker Name="EntityHasViews" />
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
              <DomainClassMoniker Name="View" />
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
        <DomainClassMoniker Name="Composite" />
        <ParentElementPath>
          <DomainPath>EntityHasComposites.Entity/!Entity/nHydrateModelHasEntities.nHydrateModel/!nHydrateModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="EntityCompositeShape/EntityCompositeShapeTextDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Composite/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="EntityCompositeShape" />
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
      <CompartmentShapeMap>
        <DomainClassMoniker Name="StoredProcedure" />
        <ParentElementPath>
          <DomainPath>nHydrateModelHasStoredProcedures.nHydrateModel/!nHydrateModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="StoredProcedureShape/StoredProcedureTextDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="StoredProcedure/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="StoredProcedureShape" />
        <CompartmentMap>
          <CompartmentMoniker Name="StoredProcedureShape/StoredProcedureFieldCompartment" />
          <ElementsDisplayed>
            <DomainPath>StoredProcedureHasFields.Fields/!StoredProcedureField</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="StoredProcedureField/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
        <CompartmentMap>
          <CompartmentMoniker Name="StoredProcedureShape/StoredProcedureParameterCompartment" />
          <ElementsDisplayed>
            <DomainPath>StoredProcedureHasParameters.Parameters/!StoredProcedureParameter</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="StoredProcedureParameter/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
      </CompartmentShapeMap>
      <CompartmentShapeMap>
        <DomainClassMoniker Name="Function" />
        <ParentElementPath>
          <DomainPath>nHydrateModelHasFunctions.nHydrateModel/!nHydrateModel</DomainPath>
        </ParentElementPath>
        <DecoratorMap>
          <TextDecoratorMoniker Name="FunctionShape/FunctionTextDecorator" />
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="Function/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </DecoratorMap>
        <CompartmentShapeMoniker Name="FunctionShape" />
        <CompartmentMap>
          <CompartmentMoniker Name="FunctionShape/FieldCompartment" />
          <ElementsDisplayed>
            <DomainPath>FunctionHasFields.Fields/!FunctionField</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="FunctionField/Name" />
            </PropertyPath>
          </PropertyDisplayed>
        </CompartmentMap>
        <CompartmentMap>
          <CompartmentMoniker Name="FunctionShape/ParameterCompartment" />
          <ElementsDisplayed>
            <DomainPath>FunctionHasParameters.Parameters/!FunctionParameter</DomainPath>
          </ElementsDisplayed>
          <PropertyDisplayed>
            <PropertyPath>
              <DomainPropertyMoniker Name="FunctionParameter/Name" />
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
      <ConnectorMap>
        <ConnectorMoniker Name="EntityInheritanceConnector" />
        <DomainRelationshipMoniker Name="EntityInheritsEntity" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="EntityCompositeConnector" />
        <DomainRelationshipMoniker Name="EntityHasComposites" />
      </ConnectorMap>
      <ConnectorMap>
        <ConnectorMoniker Name="EntityViewAssociationConnector" />
        <DomainRelationshipMoniker Name="EntityHasViews" />
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
      <ConnectionTool Name="Inheritance" ToolboxIcon="resources\exampleconnectortoolbitmap.bmp" Caption="Inheritance" Tooltip="Create an inheritance relation between two entities" HelpKeyword="">
        <ConnectionBuilderMoniker Name="nHydrate/EntityInheritsEntityBuilder" />
      </ConnectionTool>
      <ElementTool Name="View" ToolboxIcon="Resources\view.bmp" Caption="View" Tooltip="Create a View" HelpKeyword="View">
        <DomainClassMoniker Name="View" />
      </ElementTool>
      <ElementTool Name="Function" ToolboxIcon="Resources\function.bmp" Caption="Function" Tooltip="Create a Function" HelpKeyword="Function">
        <DomainClassMoniker Name="Function" />
      </ElementTool>
      <ElementTool Name="StoredProcedure" ToolboxIcon="Resources\storedproc.bmp" Caption="StoredProcedure" Tooltip="Create a Stored Procedure" HelpKeyword="StoredProcedure">
        <DomainClassMoniker Name="StoredProcedure" />
      </ElementTool>
      <ConnectionTool Name="ViewLink" ToolboxIcon="Resources\ExampleConnectorToolBitmap.bmp" Caption="View Link" Tooltip="Create a link between an entity and a view" HelpKeyword="">
        <ConnectionBuilderMoniker Name="nHydrate/EntityHasViewsBuilder" />
      </ConnectionTool>
    </ToolboxTab>
    <Validation UsesMenu="false" UsesOpen="false" UsesSave="false" UsesCustom="true" UsesLoad="false" />
    <DiagramMoniker Name="nHydrateDiagram" />
  </Designer>
  <Explorer ExplorerGuid="383b4f2d-0240-4258-beba-da8a6a0c3ab6" Title="nHydrate Explorer">
    <ExplorerBehaviorMoniker Name="nHydrate/nHydrateExplorer" />
  </Explorer>
</Dsl>