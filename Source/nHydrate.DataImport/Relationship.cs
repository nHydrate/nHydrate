using System.Collections.Generic;
using System.Linq;

namespace nHydrate.DataImport
{
    public class Relationship : DatabaseBaseObject
    {
        public Relationship()
        {
            this.RelationshipColumnList = new List<RelationshipDetail>();
            this.ConstraintName = string.Empty;
        }

        public Entity SourceEntity { get; set; }
        public Entity TargetEntity { get; set; }
        public List<RelationshipDetail> RelationshipColumnList { get; set; }
        public string ConstraintName { get; set; }

        /// <summary>
        /// A related piece of data to track imports
        /// </summary>
        public string ImportData { get; set; }

        public string RoleName
        {
            get { return this.Name + string.Empty; }
            set { this.Name = value; }
        }

        public override string ObjectType => "Relation";

        public string CorePropertiesHash
        {
            get
            {
                var prehash =
                    this.SourceEntity.Name.ToLower() + "|" +
                    this.TargetEntity.Name.ToLower() + " | ";

                var columnList = this.RelationshipColumnList.OrderBy(x => x.ChildField.Name.ToLower()).ToList();
                prehash += string.Join("-|-", columnList.Select(x => x.ChildField.Name.ToLower())) + "~";
                prehash += string.Join("-|-", columnList.Select(x => x.ParentField.Name.ToLower())) + "~";

                return prehash;
            }
        }

    }
}

