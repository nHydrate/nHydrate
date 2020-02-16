using System.Collections.Generic;

namespace nHydrate.DslPackage.Objects.Postgres
{
    public class PKModel : SchemaTableBaseModel
    {
        public string ConstraintName { get; set; }
        public int Position { get; set; }
        public string ColumnName { get; set; }

        public override string ToString()
        {
            return $"{this.SchemaName}.{this.TableName}.{this.ColumnName}";
        }

    }

    public class SchemaTableBaseModel
    {
        public string TableName { get; set; }
        public string SchemaName { get; set; }

        public override string ToString()
        {
            return $"{this.SchemaName}.{this.TableName}";
        }
    }

    public class TableModel : SchemaTableBaseModel
    {
        public List<ColumnModel> Columns { get; set; } = new List<ColumnModel>();
    }

    public class ColumnModel
    {
        public string ColumnName { get; set; }
        public string ColumnType { get; set; }
        public bool AllowNull { get; set; }
        public int? Length { get; set; }
        public int? Precision { get; set; }
        public bool IsIdentity { get; set; }

        public override string ToString()
        {
            return $"{this.ColumnName} / {this.ColumnType}";
        }
    }

    public class IndexModel : SchemaTableBaseModel
    {
        public string IndexName { get; set; }
        public List<IndexColumnModel> Columns { get; set; } = new List<IndexColumnModel>();

        public override string ToString()
        {
            return $"{this.SchemaName}.{this.TableName}.{this.IndexName}";
        }
    }

    public class IndexColumnModel
    {
        public string ColumnName { get; set; }

        public override string ToString()
        {
            return $"{this.ColumnName}";
        }
    }

    public class RelationModel : SchemaTableBaseModel
    {
        public string IndexName { get; set; }
        public string ColumnName { get; set; }
        public string FKTableName { get; set; }
        public string FKColumnName { get; set; }
    }
}
