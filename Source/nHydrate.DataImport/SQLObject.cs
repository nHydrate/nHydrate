using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nHydrate.DataImport
{
	public abstract class SQLObject : DatabaseBaseObject
	{
		public SQLObject()
		{
			this.InError = false;
		}

		public string SQL { get; set; }
		public abstract List<Field> FieldList { get; internal set; }

		/// <summary>
		/// Determines if error so cannot import
		/// </summary>
		public bool InError { get; set; }
	}
}
