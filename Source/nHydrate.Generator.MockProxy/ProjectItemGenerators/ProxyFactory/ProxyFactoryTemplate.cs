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
using System.Linq;
using System.Text;
using Widgetsphere.Generator.Models;

namespace Widgetsphere.Generator.MockProxy.ProjectItemGenerators.ProxyFactory
{
	class ProxyFactoryTemplate : BaseMockProxyTemplate
	{
		private readonly StringBuilder sb = new StringBuilder();

		#region Constructors
		public ProxyFactoryTemplate(ModelRoot model)
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
			get { return "ProxyFactory.Generated.cs"; }
		}

		public string ParentItemName
		{
			get { return "ProxyFactory.cs"; }
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
			sb.AppendLine();
		}

		public void AppendClass()
		{			
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// Used to create and return an implementation of a DTO");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	/// <typeparam name=\"T\">The requested DTO</typeparam>");
			sb.AppendLine("	public partial class ProxyFactory<T> : " + this.DefaultNamespace + ".Service.Interfaces.IProxyFactory<T> where T : " + this.DefaultNamespace + ".DataTransfer.IDTO");
			sb.AppendLine("	{");
			sb.AppendLine("		#region IProxyFactory<T> Members");
			sb.AppendLine();

			#region GetReadOnlyProxy
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets a DTO implementation");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + this.DefaultNamespace + ".Service.Interfaces.IService<T> GetReadOnlyProxy()");
			sb.AppendLine("		{");

			sb.AppendLine("			if (false) {}");
			foreach (var table in (from x in _model.Database.Tables where x.Generated && !x.AssociativeTable orderby x.Name select x))
			{
				if (table.Immutable)
				{
					sb.AppendLine("			else if (typeof(T) == typeof(" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + "))");
					sb.AppendLine("			{");
					sb.AppendLine("				return (" + this.DefaultNamespace + ".Service.Interfaces.IService<T>) new " + table.PascalName + "Service();");
					sb.AppendLine("			}");
				}
			}
			sb.AppendLine("			else");
			sb.AppendLine("				return null;");
			sb.AppendLine("		}");
			sb.AppendLine();
			#endregion

			#region GetProxy
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Gets a DTO implementation");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + this.DefaultNamespace + ".Service.Interfaces.IPersistableService<T> GetProxy()");
			sb.AppendLine("		{");

			sb.AppendLine("			if (false) {}");
			foreach (var table in (from x in _model.Database.Tables where x.Generated && !x.AssociativeTable orderby x.Name select x))
			{
				if (!table.Immutable)
				{
					sb.AppendLine("			else if (typeof(T) == typeof(" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + "))");
					sb.AppendLine("			{");
					sb.AppendLine("				return (" + this.DefaultNamespace + ".Service.Interfaces.IPersistableService<T>) new " + table.PascalName + "Service();");
					sb.AppendLine("			}");
				}
			}
			sb.AppendLine("			else");
			sb.AppendLine("				return null;");
			sb.AppendLine("		}");
			sb.AppendLine();
			#endregion

			sb.AppendLine("		#endregion");
			sb.AppendLine("	}");
		}

		#endregion


	}
}