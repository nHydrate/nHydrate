using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nHydrate.DataImport
{
	public class Parameter : DatabaseBaseObject
	{
		public Parameter()
		{
			this.DataType = System.Data.SqlDbType.VarChar;
			this.DefaultValue = string.Empty;
			this.Collate = string.Empty;
			this.SortOrder = 0;
		}

		public System.Data.SqlDbType DataType { get; set; }
		public string Collate { get; set; }
		public string DefaultValue { get; set; }
		public bool PrimaryKey { get; set; }
		public bool Nullable { get; set; }
		public int Length { get; set; }
		public int Scale { get; set; }
		public bool IsOutputParameter { get; set; }
		public int SortOrder { get; set; }

		public override string ObjectType
		{
			get { return "Parameter"; }
		}

		public override string ToString()
		{
			return this.Name;
		}

		public override bool Equals(object obj)
		{
			var o = obj as Parameter;
			if (o == null)
				return false;

			if (this.Name != o.Name) return false;
			if (this.Collate != o.Collate) return false;
			if (this.DefaultValue != o.DefaultValue) return false;
			if (this.PrimaryKey != o.PrimaryKey) return false;
			if (this.Nullable != o.Nullable) return false;
			if (this.Length != o.Length) return false;
			if (this.Scale != o.Scale) return false;
			if (this.DataType != o.DataType) return false;

			return true;

		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

	}
}

