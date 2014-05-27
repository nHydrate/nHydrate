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
using System.Collections;

namespace Widgetsphere.Generator.ProjectItemGenerators.SQLStoredProcedureAll
{
	class SQLSelectBusinessObjectByRelationTemplate : ISQLGenerate
	{
		private ModelRoot _model;
		private Table _currentTable;
		private Relation _currentRelation;

		#region Constructors

		public SQLSelectBusinessObjectByRelationTemplate(ModelRoot model, Table currentTable, Relation relation)
		{
			_model = model;
			_currentTable = currentTable;
			_currentRelation = relation;
		}

		#endregion

		#region GenerateContent
		public void GenerateContent(StringBuilder sb)
		{
			if (_model.Database.AllowZeroTouch) return;
			try
			{
				this.AppendFullTemplate(sb);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		protected ArrayList GetValidColumns()
		{
			try
			{
				ArrayList validColumns = new ArrayList();
				foreach (Column dc in _currentTable.GetColumnsFullHierarchy())
				{
					if (!(dc.DataType == System.Data.SqlDbType.Binary ||
						dc.DataType == System.Data.SqlDbType.Image ||
						dc.DataType == System.Data.SqlDbType.NText ||
						dc.DataType == System.Data.SqlDbType.Text ||
						dc.DataType == System.Data.SqlDbType.Timestamp ||
						dc.DataType == System.Data.SqlDbType.Udt ||
						dc.DataType == System.Data.SqlDbType.VarBinary ||
						dc.DataType == System.Data.SqlDbType.Variant ||
					dc.DataType == System.Data.SqlDbType.Money))
					{
						validColumns.Add(dc);
					}
				}
				return validColumns;
			}
			catch (Exception ex)
			{
				throw new Exception(_currentTable.DatabaseName + ": Failed on generation of paging select statement", ex);
			}
		}

		private void AppendFullTemplate(StringBuilder sb)
		{
			try
			{
				string objectName = "gen_" + _currentTable.PascalName + "SelectBy" + _currentRelation.PascalRoleName + "" + ((Table)_currentRelation.ChildTableRef.Object).PascalName + "RelationCommand";
				sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + objectName + "]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
				sb.AppendLine("	drop procedure [dbo].[" + objectName + "]");
				sb.AppendLine("GO");
				sb.AppendLine();
				sb.AppendLine("SET QUOTED_IDENTIFIER ON ");
				sb.AppendLine("GO");
				sb.AppendLine("SET ANSI_NULLS ON ");
				sb.AppendLine("GO");
				sb.AppendLine();
				sb.AppendLine("CREATE PROCEDURE [dbo].[" + objectName + "]");
				sb.AppendLine("(");

				foreach (ColumnRelationship columnRelationship in _currentRelation.ColumnRelationships)
				{
					Column parentColumn = ((Column)columnRelationship.ParentColumnRef.Object); 
					Column childColumn = ((Column)columnRelationship.ChildColumnRef.Object);
					sb.Append("	@" + parentColumn.PascalName + " " + parentColumn.DatabaseType);
					if (_currentRelation.ColumnRelationships.IndexOf(columnRelationship) < _currentRelation.ColumnRelationships.Count - 1)
						sb.AppendLine(",");
					else
						sb.AppendLine();
				}

				sb.AppendLine(")");
				sb.AppendLine("AS");
				sb.AppendLine();
				sb.AppendLine("SET NOCOUNT ON;");
				sb.AppendLine();
				sb.AppendLine("SELECT * FROM [" + _currentTable.DatabaseName + "]");
				sb.AppendLine("WHERE");
				
				foreach (ColumnRelationship columnRelationship in _currentRelation.ColumnRelationships)
				{
					Column parentColumn = ((Column)columnRelationship.ParentColumnRef.Object);
					Column childColumn = ((Column)columnRelationship.ChildColumnRef.Object);
					sb.AppendLine("[" + _currentTable.DatabaseName + "].[" + parentColumn.DatabaseName + "] = @" + parentColumn.PascalName);
				}

				sb.AppendLine();
				sb.AppendLine("GO");
				sb.AppendLine("SET QUOTED_IDENTIFIER OFF ");
				sb.AppendLine("GO");
				sb.AppendLine("SET ANSI_NULLS ON ");
				sb.AppendLine("GO");
			}
			catch (Exception ex)
			{
				throw;
			}

		}

	}
}