#region Copyright (c) 2006-2011 Widgetsphere LLC, All Rights Reserved
//--------------------------------------------------------------------- *
//                          Widgetsphere  LLC                           *
//             Copyright (c) 2006-2011 All Rights reserved              *
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
using System.Text;
using Widgetsphere.Generator.Models;

namespace Widgetsphere.Generator.MockProxy.ProjectItemGenerators.ObjectExtension
{
	class ObjectServiceExtenderTemplate : BaseMockProxyTemplate
	{
		private readonly StringBuilder sb = new StringBuilder();
		private readonly Table _currentTable = null;

		#region Constructors
		public ObjectServiceExtenderTemplate(ModelRoot model, Table table)
		{
			_model = model;
			_currentTable = table;
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
			get { return _currentTable.PascalName + "Service.cs"; }
		}

		#endregion

		#region GenerateContent

		private void GenerateContent()
		{
			try
			{
				Widgetsphere.Generator.GenerationHelper.AppendCopyrightInCode(sb, _model);
				this.AppendUsingStatements();
				sb.AppendLine("namespace " + this.GetLocalNamespace());
				sb.AppendLine("{");
				this.AppendClass();
				sb.AppendLine("}");
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region namespace / objects

		public void AppendUsingStatements()
		{
			sb.AppendLine("using System;");
			sb.AppendLine("using System.Collections.Generic;");
			sb.AppendLine("using System.Linq;");
			sb.AppendLine("using System.Text;");
			sb.AppendLine("using " + this.DefaultNamespace + ".DataTransfer;");
			sb.AppendLine("using " + this.DefaultNamespace + ".Service.Interfaces;");
			sb.AppendLine();
		}

		public void AppendClass()
		{
			sb.AppendLine("	partial class " + _currentTable.PascalName + "Service : I" + _currentTable.PascalName + "Service");
			sb.AppendLine("	{");
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Hook the relevant events in the constructor for mocking");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + _currentTable.PascalName + "Service()");
			sb.AppendLine("		{");
			sb.AppendLine("			//ADD EVENT HANDLERS HERE");
			sb.AppendLine("			//EXAMPLE HOOK EVENT");
			sb.AppendLine("			//this.Mock" + _currentTable.PascalName + "RunSelect += new EventHandler<" + _currentTable.PascalName + "RunSelectEventArgs>(" + _currentTable.PascalName + "Service_Mock" + _currentTable.PascalName + "RunSelect);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		//EXAMPLE EVENT HANDLER");
			sb.AppendLine("		//private void " + _currentTable.PascalName + "Service_Mock" + _currentTable.PascalName + "RunSelect(object sender, " + _currentTable.PascalName + "RunSelectEventArgs e)");
			sb.AppendLine("		//{");
			sb.AppendLine("		//  //Add code here for the the RunSelect method");
			sb.AppendLine("		//  e.ReturnValue = new List<" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + ">();");
			sb.AppendLine("		//  " + _currentTable.PascalName + "DTO newItem = new " + _currentTable.PascalName + "DTO();");
			sb.AppendLine("		//  //newItem.PropertyName = some value...;");
			sb.AppendLine("		//  e.ReturnValue.Add(newItem);");
			sb.AppendLine("		//}");
			sb.AppendLine();
			sb.AppendLine("	}");
		}

		#endregion

	}
}