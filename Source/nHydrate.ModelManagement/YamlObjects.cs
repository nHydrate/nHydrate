using System;
using System.Collections.Generic;
using System.ComponentModel;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace nHydrate.ModelManagement
{
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
        [DefaultValue(false)]
        public bool IsAssociative { get; set; }

        public string Typedentity { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string Schema { get; set; }
   
        public string Type { get; set; }

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
    }

    public class EntityFieldYaml
    {
        public string Id { get; set; }
        
        public string Name { get; set; }
        
        public bool Nullable { get; set; }
        
        public string Datatype { get; set; }
        
        public string Identity { get; set; }

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
        [DefaultValue(0)]
        public int SortOrder { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool IsReadonly { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string Summary { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool Obsolete { get; set; }
    }

    public class RelationYaml
    {
        public string ChildEntity { get; set; }
        
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
        public string Id { get; set; }
        
        public string SourceFieldId { get; set; }
        
        public string SourceFieldName { get; set; }
        
        public string TargetFieldId { get; set; }
        
        public string TargetFieldName { get; set; }
    }

    public class IndexYaml
    {
        public string Id { get; set; }
        public string Type { get; set; }

        public List<IndexFieldYaml> Fields { get; set; }
    }

    public class IndexFieldYaml
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
        public string IndexType { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue(false)]
        public bool IsUnique { get; set; }

        [YamlDotNet.Serialization.YamlMember(DefaultValuesHandling = DefaultValuesHandling.OmitDefaults)]
        [DefaultValue("")]
        public string Summary { get; set; }
    }

    public class SystemTypeTypeConverter : IYamlTypeConverter
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
