#region Copyright (c) 2006-2009 Widgetsphere LLC, All Rights Reserved
//--------------------------------------------------------------------- *
//                          Widgetsphere  LLC                           *
//             Copyright (c) 2006-2009 All Rights reserved              *
//                                                                      *
//                                                                      *
//This file and its contents are protected by United States and         *
//International copyright laws.  Unauthorized reproduction and/or       *
//distribution of all or any portion of the code contained herein       *
//is strictly prohibited and will result in severe civil and criminal   *
//penalties.  Any violations of this copyright will be prosecuted       *
//to the fullest extent possible under law.                             *
//                                                                      *
//THE SOURCE CODE CONTAINED HEREIN AND IN RELATED FILES IS PROVIDED     *
//TO THE REGISTERED DEVELOPER FOR THE PURPOSES OF EDUCATION AND         *
//TROUBLESHOOTING. UNDER NO CIRCUMSTANCES MAY ANY PORTION OF THE SOURCE *
//CODE BE DISTRIBUTED, DISCLOSED OR OTHERWISE MADE AVAILABLE TO ANY     *
//THIRD PARTY WITHOUT THE EXPRESS WRITTEN CONSENT OF WIDGETSPHERE LLC   *
//                                                                      *
//UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *
//PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *
//SUBSTANTIALLY THE SAME, FUNCTIONALITY AS ANY WIDGETSPHERE PRODUCT.    *
//                                                                      *
//THE REGISTERED DEVELOPER ACKNOWLEDGES THAT THIS SOURCE CODE           *
//CONTAINS VALUABLE AND PROPRIETARY TRADE SECRETS OF WIDGETSPHERE,      *
//INC.  THE REGISTERED DEVELOPER AGREES TO EXPEND EVERY EFFORT TO       *
//INSURE ITS CONFIDENTIALITY.                                           *
//                                                                      *
//THE END USER LICENSE AGREEMENT (EULA) ACCOMPANYING THE PRODUCT        *
//PERMITS THE REGISTERED DEVELOPER TO REDISTRIBUTE THE PRODUCT IN       *
//EXECUTABLE FORM ONLY IN SUPPORT OF APPLICATIONS WRITTEN USING         *
//THE PRODUCT.  IT DOES NOT PROVIDE ANY RIGHTS REGARDING THE            *
//SOURCE CODE CONTAINED HEREIN.                                         *
//                                                                      *
//THIS COPYRIGHT NOTICE MAY NOT BE REMOVED FROM THIS FILE.              *
//--------------------------------------------------------------------- *
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.Common.Util;

namespace Widgetsphere.Generator.ProjectItemGenerators.DataService.DataServiceStaticJScripts
{
	class DataServiceStaticJScriptsTemplate : BaseDataTransferServiceTemplate
	{
		private StringBuilder sb = new StringBuilder();		

		#region Constructors
		public DataServiceStaticJScriptsTemplate(ModelRoot model)
		{
			_model = model;
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
			get { return "objectcore.js"; }
		}

		#endregion

		#region GenerateContent
		private void GenerateContent()
		{
			try
			{
				this.AppendClass();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region namespace / objects

		public void AppendClass()
		{
			sb.AppendLine("var viewServiceProxy = new serviceProxy(\"Rest.svc/\");");
			sb.AppendLine();
			sb.AppendLine("function OnPageError(xhr, errorMsg, thrown) {");
			sb.AppendLine();
			sb.AppendLine("    var errorString;");
			sb.AppendLine("    if (typeof xhr == \"string\") {");
			sb.AppendLine("        alert(xhr);");
			sb.AppendLine("        return;");
			sb.AppendLine("    }");
			sb.AppendLine();
			sb.AppendLine("    if (errorMsg && errorMsg != \"error\")");
			sb.AppendLine("        alert(\"errormsg: \" + errorMsg);");
			sb.AppendLine();
			sb.AppendLine("    else if (typeof (xhr.responseText) == \"string\" && xhr.responseText != \"\") {");
			sb.AppendLine("        var err = JSON2.parse(xhr.responseText);");
			sb.AppendLine("        if (typeof (err) == \"object\")");
			sb.AppendLine("            alert(err.Message);");
			sb.AppendLine("        else");
			sb.AppendLine("            alert(\"Unknown server error.\");");
			sb.AppendLine("    }");
			sb.AppendLine("    else");
			sb.AppendLine("        alert(\"Unknown error occurred in callback.\");");
			sb.AppendLine("}");
			sb.AppendLine();
			sb.AppendLine("function IsNullOrEmpty(value) {");
			sb.AppendLine("    if (value == null || value == '')");
			sb.AppendLine("        return true;");
			sb.AppendLine("    else");
			sb.AppendLine("        return false;");
			sb.AppendLine("}");
		}

		#endregion

	}
}