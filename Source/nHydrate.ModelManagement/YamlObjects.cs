using System;
using System.Collections.Generic;
using System.ComponentModel;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using static nHydrate.ModelManagement.Utilities;

namespace nHydrate.ModelManagement
{
    public class DiskModelYaml
    {
        public List<EntityYaml> Entities { get; internal set; } = new List<EntityYaml>();
        public List<ViewYaml> Views { get; internal set; } = new List<ViewYaml>();
        public ModelProperties ModelProperties { get; set; } = new ModelProperties();
    }

    public class EntityYaml
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public bool AllowCreateAudit { get; set; }
        public bool AllowModifyAudit { get; set; }

        public bool AllowTimestamp { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string CodeFacade { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool Immutable { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(TypedTableConstants.None)]
        public TypedTableConstants TypedTable { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool IsAssociative { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string Schema { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string Summary { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool GeneratesDoubleDerived { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool IsTenant { get; set; }

        public List<EntityFieldYaml> Fields { get; set; } = new List<EntityFieldYaml>();

        public List<RelationYaml> Relations { get; set; } = new List<RelationYaml>();

        public List<IndexYaml> Indexes { get; set; } = new List<IndexYaml>();

        public List<StaticDataYaml> StaticData { get; set; } = new List<StaticDataYaml>();

        public override string ToString() => this.Name;
    }

    public class EntityFieldYaml
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public Utilities.DataTypeConstants Datatype { get; set; }

        public bool Nullable { get; set; }

        public IdentityTypeConstants Identity { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string CodeFacade { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string DataFormatString { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string Default { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool DefaultIsFunc { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string Formula { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool IsIndexed { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool IsPrimaryKey { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool IsCalculated { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool IsUnique { get; set; }

        public int Length { get; set; }

        public int Scale { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool IsReadonly { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string Summary { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool Obsolete { get; set; }

        public override string ToString() => this.Name;
    }

    public class RelationYaml
    {
        public string ChildEntity { get; set; }

        public string Id { get; set; }

        public string ChildId { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(true)]
        public bool IsEnforced { get; set; }

        public string DeleteAction { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string RoleName { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string Summary { get; set; }

        public List<RelationFieldYaml> Fields { get; set; } = new List<RelationFieldYaml>();
    }

    public class RelationFieldYaml
    {
        public string SourceFieldName { get; set; }
        public string TargetFieldName { get; set; }
        public string Id { get; set; }
        public string SourceFieldId { get; set; }
        public string TargetFieldId { get; set; }
    }

    public class IndexYaml
    {
        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool Clustered { get; set; }

        public string Id { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string ImportedName { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public Utilities.IndexTypeConstants IndexType { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool IsUnique { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string Summary { get; set; }

        public List<IndexFieldYaml> Fields { get; set; } = new List<IndexFieldYaml>();
    }

    public class IndexFieldYaml
    {
        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(true)]
        public bool Ascending { get; set; }

        public string FieldId { get; set; }

        public string Id { get; set; }
    }

    public class StaticDataYaml
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public override string ToString() => this.Key;
    }

    public class ViewYaml
    {
        public string Name { get; set; }

        public string Id { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string CodeFacade { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string Schema { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string Summary { get; set; }

        public string Sql { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool GeneratesDoubleDerived { get; set; }

        public List<ViewFieldYaml> Fields { get; set; } = new List<ViewFieldYaml>();

        public override string ToString() => this.Name;
    }

    public class ViewFieldYaml
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public Utilities.DataTypeConstants Datatype { get; set; }

        public bool Nullable { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string CodeFacade { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool IsPrimaryKey { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string Default { get; set; }

        public int Length { get; set; }

        public int Scale { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string Summary { get; set; }

        public override string ToString() => this.Name;
    }

    internal class SystemTypeTypeConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return typeof(Type).IsAssignableFrom(type);
        }

        public object ReadYaml(IParser parser, Type type)
        {
            var scalar = parser.Expect<Scalar>();
            return Type.GetType(scalar.Value);
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            var typeName = ((Type)value).AssemblyQualifiedName;
            emitter.Emit(new Scalar(typeName));
        }
    }
}
