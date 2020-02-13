using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nHydrate.DataImport
{
	/// <summary>
	/// Used to compare strings ignoring case
	/// </summary>
	public class StringCompare : IEqualityComparer<string>
	{
		public bool Equals(string x, string y)
		{
			if ((x == null) && (y == null)) return true;
			if ((x != null) && (y == null)) return false;
			if ((x == null) && (y != null)) return false;
			if (x.ToLower() == y.ToLower()) return true;
			return false;
		}

		public int GetHashCode(string obj)
		{
			return base.GetHashCode();
		}

	}
}

