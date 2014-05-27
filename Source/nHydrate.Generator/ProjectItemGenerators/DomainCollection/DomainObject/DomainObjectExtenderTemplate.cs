using System;
using System.Collections.Generic;
using System.Text;
using Widgetsphere.Generator.Models;

namespace Widgetsphere.Generator.ProjectItemGenerators.DomainObject
{
	class DomainObjectExtenderTemplate : BaseClassTemplate
	{
		private StringBuilder sb = new StringBuilder();
		private Table _currentTable;

		#region construction
		public DomainObjectExtenderTemplate(ModelRoot model, Table currentTable)
    {
      _model = model;
      _currentTable = currentTable;
		}
		#endregion 

		#region BaseClassTemplate overrides
		public override string FileContent
		{
			get 
			{
				GenerateContent();
				return sb.ToString();
			}
		}

		public override string FileName
		{
			get 
			{
				return string.Format("Domain{0}.cs", _currentTable.PascalName);
			}
		}
		#endregion

		#region GenerateContent
		private void GenerateContent()
		{
      try
      {
				ValidationHelper.AppendCopyrightInCode(sb, _model); 
				this.AppendUsingStatements();        
        sb.AppendLine("namespace " + DefaultNamespace + ".Domain.Objects" );
        sb.AppendLine("{" );
        this.AppendClass();
        sb.AppendLine("}" );
      }
      catch(Exception ex)
      {
        throw;
      }
		}

		#endregion

		#region namespace / objects
		public void AppendUsingStatements()
		{
		
		}
		public void AppendClass()
		{
      sb.AppendLine("	internal partial class " + "Domain" + _currentTable.PascalName);
			sb.AppendLine("	{");
			sb.AppendLine("	}");
		}
		#endregion

		#region append regions
		#endregion

		#region append member variables
		#endregion

		#region append constructors
		#endregion

		#region append properties
		#endregion

		#region append methods
		#endregion

		#region append operator overloads
		#endregion

	}
}
