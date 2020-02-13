using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nHydrate.Generator.Common.Util;

namespace nHydrate.DataImport
{
	public class StoredProc : SQLObject
	{
		public StoredProc()
		: base()
		{
			this.FieldList = new List<Field>();
			this.ParameterList = new List<Parameter>();
			this.Schema = string.Empty;
			this.Collate = string.Empty;
		}

		public string Schema { get; set; }
		public string Collate { get; set; }
		public override	 List<Field> FieldList { get; internal set; }
		public override List<Parameter> ParameterList { get; internal set; }

		public override string ObjectType
		{
			get { return "Stored Procedure"; }
		}

		public override string ToString()
		{
			return this.Name;
		}

		/// <summary>
		/// Determine if there was an error reading column information
		/// </summary>
		public bool ColumnFailure { get; set; }

		public string CorePropertiesHash
		{
			get
			{
				var schema = this.Schema;
				if (string.IsNullOrEmpty(schema))
					schema = "dbo";

				var prehash =
					this.Name + "|" +
					this.Collate + "|" +
					schema + "|" +
					this.SQL + "|";
				return HashHelper.Hash(prehash);
				//return prehash;
			}
		}

	}
}

