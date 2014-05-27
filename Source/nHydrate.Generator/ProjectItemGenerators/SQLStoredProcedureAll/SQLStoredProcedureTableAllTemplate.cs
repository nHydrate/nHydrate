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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.Common.Util;

namespace Widgetsphere.Generator.ProjectItemGenerators.SQLStoredProcedureAll
{
	class SQLStoredProcedureTableAllTemplate : BaseDbScriptTemplate
	{
		private StringBuilder sb = new StringBuilder();
		private Table _table;

		#region Constructors
		public SQLStoredProcedureTableAllTemplate(ModelRoot model, Table table)
		{
			_model = model;
			_table = table;
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
			get { return string.Format("{0}.sql", _table.PascalName); }
		}

		#endregion

		#region GenerateContent
		private void GenerateContent()
		{
			try
			{
				ISQLGenerate generator = null;

				sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
				sb.AppendLine("--Data Schema For Version " + _model.Version);
				ValidationHelper.AppendCopyrightInSQL(sb, _model);

				generator = new SQLDeleteBusinessObjectTemplate(_model, _table);
				generator.GenerateContent(sb);

				generator = new SQLSelectBusinessObjectByPrimaryKeyTemplate(_model, _table);
				generator.GenerateContent(sb);

				generator = new SQLInsertBusinessObjectTemplate(_model, _table);
				generator.GenerateContent(sb);

				generator = new SQLPagedSelectBusinessObjectTemplate(_model, _table);
				generator.GenerateContent(sb);

				generator = new SQLSelectBusinessObjectByAndTemplate(_model, _table);
				generator.GenerateContent(sb);

				generator = new SQLSelectBusinessObjectByFieldTemplate(_model, _table);
				generator.GenerateContent(sb);

				generator = new SQLUpdateBusinessObjectTemplate(_model, _table);
				generator.GenerateContent(sb);

				if (_table.AllowCreateAudit)
				{
					generator = new SQLSelectBusinessObjectByCreatedDateTemplate(_model, _table);
					generator.GenerateContent(sb);
				}

				if (_table.AllowModifiedAudit)
				{
					generator = new SQLSelectBusinessObjectByModifiedDateTemplate(_model, _table);
					generator.GenerateContent(sb);
				}

				generator = new SQLSelectBusinessObjectByOrTemplate(_model, _table);
				generator.GenerateContent(sb);

				generator = new SQLSelectBusinessObjectTemplate(_model, _table);
				generator.GenerateContent(sb);


				//All parent relationships
				foreach (Relation relation in _table.ParentRoleRelations.Where(x => x.IsGenerated))
				{
					if (!relation.IsPrimaryKeyRelation())
					{
						generator = new SQLSelectBusinessObjectByRelationTemplate(_model, _table, relation);
						generator.GenerateContent(sb);
					}
				}

				foreach (Relation relation in _table.GetChildRoleRelationsFullHierarchy().Where(x => x.IsGenerated))
				{
					generator = new SQLSelectBusinessObjectByForeignKeyTemplate(_model, relation, _table);
					generator.GenerateContent(sb);
				}

				//All Components for this table
				foreach (TableComponent component in _table.ComponentList)
				{
					generator = new SQLPagedSelectComponentTemplate(_model, component);
					generator.GenerateContent(sb);

					generator = new SQLSelectComponentByPrimaryKeyTemplate(_model, component);
					generator.GenerateContent(sb);

					generator = new SQLSelectComponentByFieldTemplate(_model, component);
					generator.GenerateContent(sb);

					if (component.Parent.AllowCreateAudit)
					{
						generator = new SQLSelectComponentByCreatedDateTemplate(_model, component);
						generator.GenerateContent(sb);
					}

					if (component.Parent.AllowModifiedAudit)
					{
						generator = new SQLSelectComponentByModifiedDateTemplate(_model, component);
						generator.GenerateContent(sb);
					}

					generator = new SqlSelectComponentTemplate(_model, component);
					generator.GenerateContent(sb);

					generator = new SQLUpdateComponentTemplate(_model, component);
					generator.GenerateContent(sb);
				}


				foreach (CustomRetrieveRule rule in _model.Database.CustomRetrieveRules)
				{
					Table table = (Table)rule.ParentTableRef.Object;
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