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
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Widgetsphere.Generator;
using Widgetsphere.Generator.Models;
using System.Collections;
using Widgetsphere.Generator.Common.Util;

namespace Widgetsphere.Generator.ProjectItemGenerators.DatabaseSchema
{
	class CreateDataTemplate : BaseDbScriptTemplate
	{
		private StringBuilder sb = new StringBuilder();

		#region Constructors
		public CreateDataTemplate(ModelRoot model)
    {
      _model = model;
		}
		#endregion 

		#region BaseClassTemplate overrides
		public override string FileContent
		{
			get 
			{
				try
				{
					GenerateContent();
					return sb.ToString();

				}
				catch (Exception ex)
				{					
					throw;
				}
			}
		}

		public override string FileName
		{
			get 
			{
				return string.Format("CreateData.sql");
			}
		}
		#endregion

		#region GenerateContent
		private void GenerateContent()
		{
			try
			{
				sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
				sb.AppendLine("--Static Data For Version " + _model.Version);
				sb.AppendLine("--Generated on " + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss"));
				sb.AppendLine();

				//Turn OFF CONSTRAINTS
				sb.AppendLine("exec sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");
				sb.AppendLine();

				#region Add Static Data
				foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
				{
					sb.Append(SQLGeneration.GetSQLInsertStaticData(table));
				}
				#endregion

				//Turn ON CONSTRAINTS
				sb.AppendLine("exec sp_MSforeachtable 'ALTER TABLE ? CHECK CONSTRAINT ALL'");
				sb.AppendLine();

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion
	}
}
