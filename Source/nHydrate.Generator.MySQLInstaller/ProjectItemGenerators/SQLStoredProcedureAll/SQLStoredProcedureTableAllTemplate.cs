#region Copyright (c) 2006-2012 nHydrate.org, All Rights Reserved
//--------------------------------------------------------------------- *
//                          NHYDRATE.ORG                                *
//             Copyright (c) 2006-2012 All Rights reserved              *
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
//THIRD PARTY WITHOUT THE EXPRESS WRITTEN CONSENT OF THE NHYDRATE GROUP *
//                                                                      *
//UNDER NO CIRCUMSTANCES MAY THE SOURCE CODE BE USED IN WHOLE OR IN     *
//PART, AS THE BASIS FOR CREATING A PRODUCT THAT PROVIDES THE SAME, OR  *
//SUBSTANTIALLY THE SAME, FUNCTIONALITY AS THIS PRODUCT                 *
//                                                                      *
//THE REGISTERED DEVELOPER ACKNOWLEDGES THAT THIS SOURCE CODE           *
//CONTAINS VALUABLE AND PROPRIETARY TRADE SECRETS OF NHYDRATE,          *
//THE REGISTERED DEVELOPER AGREES TO EXPEND EVERY EFFORT TO             *
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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Models;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.ProjectItemGenerators;

namespace nHydrate.Generator.MySQLInstaller.ProjectItemGenerators.SQLStoredProcedureAll
{
	class SQLStoredProcedureTableAllTemplate : BaseDbScriptTemplate
	{
		private StringBuilder sb = new StringBuilder();
		private Table _table;
		private bool _useSingleFile = false;

		#region Constructors
		public SQLStoredProcedureTableAllTemplate(ModelRoot model, Table table, bool useSingleFile)
			: base(model)
		{
			_table = table;
			_useSingleFile = useSingleFile;
		}
		#endregion

		#region BaseClassTemplate overrides

		public override string FileContent
		{
			get
			{
				try
				{
					this.GenerateContent();
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
			get { return string.Format("{0}.sql", _table.PascalName); }
		}

		#endregion

		#region GenerateContent
		private void GenerateContent()
		{
			//if (_model.Database.AllowZeroTouch)
			//{
			//  //Add drop SP here
			//  return;
			//}

			try
			{
				ISQLGenerate generator = null;

				if (!_useSingleFile)
				{
					sb.AppendLine("#DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
					sb.AppendLine();
				}

				sb.AppendLine("#This SQL is generated for internal stored procedures for table [" + _table.DatabaseName + "]");
				sb.AppendLine();

				nHydrate.Generator.GenerationHelper.AppendCopyrightInSQL(sb, _model);

				generator = new SQLDeleteBusinessObjectTemplate(_model, _table);
				generator.GenerateContent(sb);

				generator = new SQLInsertBusinessObjectTemplate(_model, _table);
				generator.GenerateContent(sb);

				generator = new SQLSelectAuditBusinessObjectTemplate(_model, _table);
				generator.GenerateContent(sb);

				generator = new SQLUpdateBusinessObjectTemplate(_model, _table);
				generator.GenerateContent(sb);

				//All Components for this table
				foreach (TableComponent component in _table.ComponentList)
				{
					//generator = new SQLPagedSelectComponentTemplate(_model, component);
					//generator.GenerateContent(sb);

					//generator = new SQLSelectComponentByPrimaryKeyTemplate(_model, component);
					//generator.GenerateContent(sb);

					//generator = new SQLSelectComponentByFieldTemplate(_model, component);
					//generator.GenerateContent(sb);

					//if (component.Parent.AllowCreateAudit)
					//{
					//  generator = new SQLSelectComponentByCreatedDateTemplate(_model, component);
					//  generator.GenerateContent(sb);
					//}

					//if (component.Parent.AllowModifiedAudit)
					//{
					//  generator = new SQLSelectComponentByModifiedDateTemplate(_model, component);
					//  generator.GenerateContent(sb);
					//}

					//generator = new SqlSelectComponentTemplate(_model, component);
					//generator.GenerateContent(sb);

					generator = new SQLUpdateComponentTemplate(_model, component);
					generator.GenerateContent(sb);
				}


				foreach (var rule in _model.Database.CustomRetrieveRules.ToList())
				{
					var table = (Table)rule.ParentTableRef.Object;
					if (table == _table)
					{
						if (rule.Generated && table.Generated)
						{
							generator = new SQLSelectRetrieveRuleTemplate(_model, rule);
							generator.GenerateContent(sb);
						}
					}
				}

			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

	}
}