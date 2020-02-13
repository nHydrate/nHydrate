#pragma warning disable 0168
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator;
using nHydrate.Generator.Models;
using System.Collections;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.ProjectItemGenerators;

namespace nHydrate.Generator.Datasite
{
	class DatasiteJQueryTemplate : BaseScriptTemplate
	{
		private StringBuilder sb = new StringBuilder();
		private string _templateLocation = string.Empty;

		#region Constructors
		public DatasiteJQueryTemplate(ModelRoot model, string templateLocation)
			: base(model)
		{
			_templateLocation = templateLocation;
		}
		#endregion

		#region BaseClassTemplate overrides
		public override string FileContent
		{
			get
			{
				this.GenerateContent();
				return sb.ToString();
			}
		}

		public override string FileName
		{
			get { return string.Format("jquery-1.9.0.min.js"); }
		}
		#endregion

		#region GenerateContent
		private void GenerateContent()
		{
			try
			{
				sb.Append(Helpers.GetFileContent(new EmbeddedResourceName(_templateLocation + ".jquery-1.9.0.min.js")));
			}
			catch (Exception ex)
			{
				throw;
			}
		}
		#endregion
	}
}
