using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        public int RelationCount { get { return this.Entities.SelectMany(x => x.Relations).Count(); } }
    }

    public class EntityYaml
    {
        public string Name { get; set; }

        public Guid Id { get; set; }

        public bool AllowCreateAudit { get; set; } = true;

        public bool AllowModifyAudit { get; set; } = true;

        public bool AllowTimestamp { get; set; } = true;

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string CodeFacade { get; set; } = string.Empty;

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool Immutable { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(TypedTableConstants.None)]
        public TypedTableConstants TypedTable { get; set; } = TypedTableConstants.None;

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool IsAssociative { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string Schema { get; set; } = string.Empty;

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string Summary { get; set; } = string.Empty;

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

        public Guid Id { get; set; }

        public Utilities.DataTypeConstants Datatype { get; set; } = Utilities.DataTypeConstants.VarChar;

        public bool Nullable { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(IdentityTypeConstants.None)]
        public IdentityTypeConstants Identity { get; set; } = IdentityTypeConstants.None;

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string CodeFacade { get; set; } = string.Empty;

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string DataFormatString { get; set; } = string.Empty;

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string Default { get; set; } = string.Empty;

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool DefaultIsFunc { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string Formula { get; set; } = string.Empty;

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

        public int Length { get; set; } = 50;

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(0)]
        public int Scale { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool IsReadonly { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string Summary { get; set; } = string.Empty;

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool Obsolete { get; set; }

        public override string ToString() => this.Name;
    }

    public class RelationYaml
    {
        public string ForeignEntityName { get; set; }

        public Guid ForeignEntityId { get; set; }

        //public Guid Id { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(true)]
        public bool IsEnforced { get; set; } = true;

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(DeleteActionConstants.NoAction)]
        public DeleteActionConstants DeleteAction { get; set; } = DeleteActionConstants.NoAction;

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string RoleName { get; set; } = string.Empty;

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string Summary { get; set; } = string.Empty;

        public List<RelationFieldYaml> Fields { get; set; } = new List<RelationFieldYaml>();
    }

    public class RelationFieldYaml
    {
        public string PrimaryFieldName { get; set; }
        public string ForeignFieldName { get; set; }
        //public Guid Id { get; set; }
        public Guid PrimaryFieldId { get; set; }
        public Guid ForeignFieldId { get; set; }
    }

    public class IndexYaml
    {
        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool Clustered { get; set; }

        //public Guid Id { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string ImportedName { get; set; } = string.Empty;

        public Utilities.IndexTypeConstants IndexType { get; set; } = Utilities.IndexTypeConstants.IsIndexed;

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool IsUnique { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string Summary { get; set; } = string.Empty;

        public List<IndexFieldYaml> Fields { get; set; } = new List<IndexFieldYaml>();
    }

    public class IndexFieldYaml
    {
        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(true)]
        public bool Ascending { get; set; } = true;

        public Guid FieldId { get; set; }

        public string FieldName { get; set; } = string.Empty;

        //public Guid Id { get; set; }
    }

    public class StaticDataYaml
    {
        public Guid ColumnId { get; set; }
        public string Value { get; set; }
        public int SortOrder { get; set; }
        public override string ToString() => this.ColumnId.ToString();
    }

    public class ViewYaml
    {
        public string Name { get; set; }

        public Guid Id { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string CodeFacade { get; set; } = string.Empty;

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string Schema { get; set; } = string.Empty;

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string Summary { get; set; } = string.Empty;

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

        public Guid Id { get; set; }

        public Utilities.DataTypeConstants Datatype { get; set; } = Utilities.DataTypeConstants.VarChar;

        public bool Nullable { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string CodeFacade { get; set; } = string.Empty;

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool IsPrimaryKey { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string Default { get; set; } = string.Empty;

        public int Length { get; set; } = 50;

        public int Scale { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string Summary { get; set; } = string.Empty;

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
