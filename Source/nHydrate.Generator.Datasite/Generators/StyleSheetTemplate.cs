#pragma warning disable 0168
using System;
using System.Text;
using nHydrate.Generator.Models;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Datasite
{
	class StyleSheetTemplate : BaseScriptTemplate
	{
		private StringBuilder sb = new StringBuilder();
		private string _templateLocation = string.Empty;

		#region Constructors
		public StyleSheetTemplate(ModelRoot model, string templateLocation)
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
			get { return string.Format("styles.css"); }
		}
		#endregion

		#region GenerateContent
		private void GenerateContent()
		{
			try
			{
				var fileContent = Helpers.GetFileContent(new EmbeddedResourceName(_templateLocation + ".datasite-style.css"));
				sb.Append(fileContent);
			}
			catch (Exception ex)
			{
				throw;
			}
		}
		#endregion
	}
}
