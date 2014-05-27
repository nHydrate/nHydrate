using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.TestSite.Objects
{
	public class URLParameterCollection : List<URLParameter>
	{
		public URLParameter this[string name]
		{
			get
			{
				foreach (URLParameter item in this)
				{
					if (string.Compare(item.Name, name, true) == 0)
					{
						return item;
					}
				}
				return null;
			}
		}

		public bool Contains(string name)
		{
			foreach (URLParameter item in this)
			{
				if (string.Compare(item.Name, name, true) == 0)
				{
					return true;
				}
			}
			return false;
		}

		public void Remove(string name)
		{
			int index = 0;
			foreach (URLParameter item in this)
			{
				if (string.Compare(item.Name, name, true) == 0)
				{
					this.RemoveAt(index);
					return;
				}
				index++;
			}
		}

		public void SetValue(string name, string value)
		{
			if ((name == null) || (name == ""))
				return;
			
			if ((value == null) || (value == ""))
			{
				//If there is no value set then remove the parameter if it exists
				if (this.Contains(name))
					this.Remove(name);

			}
			else
			{
				if (this.Contains(name))
					this[name].Value = value;
				else
					this.Add(new URLParameter(name, value));
			}
			
		}

		public string GetValue(string name)
		{
			return this.GetValue(name, "");
		}

		public string GetValue(string name, string defaultValue)
		{
			if ((name == null) || (name == ""))
				return defaultValue;

			if (this.Contains(name))
				return this[name].Value;
			else
				return defaultValue;
		}

	}
}