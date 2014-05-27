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

namespace Widgetsphere.Generator.ProjectItemGenerators.DataService.DataServiceWebConfigMainServiceASMX
{
	class DataServiceWebConfigMainServiceASMXTemplate : BaseDataTransferServiceTemplate
	{
		private StringBuilder sb = new StringBuilder();

		#region Constructors
		public DataServiceWebConfigMainServiceASMXTemplate(ModelRoot model)
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
			get { return "Rest.svc"; }
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
			sb.AppendLine("<%@ ServiceHost Language=\"C#\" Debug=\"true\" Service=\"" + this.DefaultNamespace + ".Rest\" Factory=\"" + this.DefaultNamespace + ".AppServiceHostFactory\"%>");
			sb.AppendLine("using System;");
			sb.AppendLine("using System.ServiceModel;");
			sb.AppendLine("using System.ServiceModel.Activation;");
			sb.AppendLine("using Microsoft.ServiceModel.Web;");
			sb.AppendLine();
			sb.AppendLine("namespace " + this.DefaultNamespace);
			sb.AppendLine("{");
			sb.AppendLine("	class AppServiceHostFactory : ServiceHostFactory");
			sb.AppendLine("	{");
			sb.AppendLine("		protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)");
			sb.AppendLine("		{");
			sb.AppendLine("			return new WebServiceHost2(serviceType, true, baseAddresses);");
			sb.AppendLine("		}");
			sb.AppendLine("	}");
			sb.AppendLine("}	");
		}

		#endregion

	}
}