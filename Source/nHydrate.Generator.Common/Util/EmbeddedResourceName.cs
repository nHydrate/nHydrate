namespace nHydrate.Generator.Common.Util
{
	public class EmbeddedResourceName
	{
		#region members
		private string _fullName = string.Empty;
		private string _fileName = string.Empty;
		private string _asmLocation = string.Empty;
		#endregion

		#region construction
		public EmbeddedResourceName()
		{
		}

		public EmbeddedResourceName(string resourceName)
		{
			var splitResourceName = resourceName.Split('.');
			for(var ii = 0; ii < splitResourceName.Length -2; ii++)
			{
				_asmLocation += splitResourceName[ii];
				if (ii < splitResourceName.Length - 3)
				{
					_asmLocation += ".";
				}
			}
			_fullName = resourceName;
			_fileName = splitResourceName[splitResourceName.Length - 2] + "." + splitResourceName[splitResourceName.Length - 1];
		}
		#endregion

		#region properties
		public string FullName
		{
			get { return _fullName; }
			set { _fullName = value; }
		}

		public string FileName
		{
			get { return _fileName; }
			set { _fileName = value; }
		}

		public string AsmLocation
		{
			get { return _asmLocation; }
			set { _asmLocation = value; }
		}
		#endregion
	}
}

