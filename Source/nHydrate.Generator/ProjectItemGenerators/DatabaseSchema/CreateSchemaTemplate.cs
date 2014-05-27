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
	class CreateSchemaTemplate : BaseDbScriptTemplate
	{
		private StringBuilder sb = new StringBuilder();

		#region Constructors
		public CreateSchemaTemplate(ModelRoot model)
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
			get
			{
				return string.Format("CreateSchema.sql");
			}
		}
		#endregion

		#region GenerateContent
		private void GenerateContent()
		{
			try
			{
				sb = new StringBuilder();
				sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
				sb.AppendLine("--Data Schema For Version " + _model.Version);
				sb.AppendLine("--Generated on " + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss"));
				sb.AppendLine();

				this.AppendCreateTable();
				this.AppendAuditTracking();
				this.AppendCreateAudit();
				this.AppendCreatePrimaryKey();
				this.AppendCreateUniqueKey();
				this.AppendCreateForeignKey();				
				this.AppendCreateIndexes();
				this.AppendCreateDefaults();
				this.AppendViews();
				this.AppendAuditTriggers();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#region Append CreateTable
		private void AppendCreateTable()
		{
			foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			{
				sb.AppendLine("--CREATE TABLE [" + table.DatabaseName + "]");
				sb.AppendLine(SQLGeneration.GetSQLCreateTable(_model, table));
				sb.AppendLine("GO");
				sb.AppendLine();
			}

			////Drop all descriptions
			//foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			//{
			//  sb.AppendLine("--DROP ALL COLUMN DESCRIPTIONS FOR TABLE [" + table.DatabaseName + "]");
			//  foreach (Column column in table.GetColumns())
			//  {
			//    sb.AppendLine("EXEC sys.sp_dropextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + table.DatabaseName + "', @level2type=N'COLUMN',@level2name=N'" + column.DatabaseName + "'");
			//  }
			//  sb.AppendLine();
			//}

			////Add all descriptions
			//foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			//{
			//  sb.AppendLine("--ADD ALL COLUMN DESCRIPTIONS FOR TABLE [" + table.DatabaseName + "]");
			//  foreach (Column column in table.GetColumns())
			//  {
			//    if (column.Description != "")
			//    {
			//      sb.AppendLine("EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'" + column.Description + "', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'" + table.DatabaseName + "', @level2type=N'COLUMN',@level2name=N'" + column.DatabaseName + "'");
			//    }
			//  }
			//  sb.AppendLine();
			//}

		}
		#endregion

		#region Append AuditTracking
		private void AppendAuditTracking()
		{
			foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			{
				if (table.AllowAuditTracking)
				{
					sb.AppendLine("--CREATE AUDIT TABLE FOR [" + table.DatabaseName + "]");
					sb.AppendLine(SQLGeneration.GetSQLCreateAuditTable(_model, table));
					sb.AppendLine("GO");
					sb.AppendLine();
				}
			}
		}
		#endregion

		#region Append Primary Key

		private void AppendCreatePrimaryKey()
		{
			foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			{
				string tableName = Globals.GetTableDatabaseName(_model, table);
				string indexName = "PK_" + tableName;
				indexName = indexName.ToUpper();

				if (table.EnforcePrimaryKey)
				{
					sb.AppendLine("--PRIMARY KEY FOR TABLE [" + tableName + "]");
					sb.AppendLine("if not exists(select * from sysobjects where name = '" + indexName + "' and xtype = 'PK')");
					sb.AppendLine("ALTER TABLE [dbo].[" + tableName + "] WITH NOCHECK ADD ");
					sb.AppendLine("CONSTRAINT [" + indexName + "] PRIMARY KEY CLUSTERED ");
					sb.AppendLine("(");
					for (int ii = 0; ii < table.PrimaryKeyColumns.Count; ii++)
					{
						sb.Append("	[" + ((Column)table.PrimaryKeyColumns[ii]).DatabaseName + "]");
						if (ii < table.PrimaryKeyColumns.Count - 1)
							sb.Append(",");
						sb.AppendLine();

					}
					sb.AppendLine(") ON [PRIMARY] ");
					sb.AppendLine("GO");
					sb.AppendLine();
				}
				else
				{
					//Drop the PK if necessary
					sb.AppendLine("--PRIMARY KEY FOR TABLE [" + tableName + "]");
					sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'" + indexName + "'))");
					sb.AppendLine("ALTER TABLE [" + table.DatabaseName + "] DROP CONSTRAINT [" + indexName + "]");
					sb.AppendLine("GO");
					sb.AppendLine();
				}
			}
		}

		#endregion

		#region Append Foreign Keys

		private void AppendCreateForeignKey()
		{
			foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			{
				string tableName = Globals.GetTableDatabaseName(_model, table);
				IList<Relation> childRoleRelations = table.ChildRoleRelations;
				if (childRoleRelations.Count > 0)
				{
					for (int ii = 0; ii < childRoleRelations.Count; ii++)
					{
						Relation relation = (Relation)childRoleRelations[ii];
						Table childTable = (Table)relation.ChildTableRef.Object;
						Table parentTable = (Table)relation.ParentTableRef.Object;
						string indexName = "FK_" + relation.DatabaseRoleName + "_" + Globals.GetTableDatabaseName(_model, childTable) + "_" + Globals.GetTableDatabaseName(_model, parentTable) + "";
						indexName = indexName.ToUpper();
						sb.AppendLine("--FOREIGN KEY RELATIONSHIP [" + parentTable.DatabaseName + "] -> [" + childTable.DatabaseName + "] (" + GetFieldNames(relation) + ")");
						sb.AppendLine("if not exists(select * from sysobjects where name = '" + indexName + "' and xtype = 'F')");
						sb.AppendLine("ALTER TABLE [dbo].[" + tableName + "] ADD ");
						sb.AppendLine("CONSTRAINT [" + indexName + "] FOREIGN KEY ");
						sb.AppendLine("(");
						AppendChildTableColumns(relation);
						sb.AppendLine(") REFERENCES [dbo].[" + Globals.GetTableDatabaseName(_model, parentTable) + "] (" );
						AppendParentTableColumns(relation);
						sb.AppendLine(")");
                        sb.AppendLine("GO");
						sb.AppendLine();                        
					}					
					sb.AppendLine();
				}
			}
		}

		private string GetFieldNames(Relation relation)
		{
			StringBuilder retval = new StringBuilder();
			for (int kk = 0; kk < relation.ColumnRelationships.Count; kk++)
			{
				ColumnRelationship columnRelationship = relation.ColumnRelationships[kk];
				Column parentColumn = ((Column)columnRelationship.ParentColumnRef.Object);
				Column childColumn = ((Column)columnRelationship.ChildColumnRef.Object);
				Table parentTable = (Table)parentColumn.ParentTableRef.Object;
				Table childTable = (Table)childColumn.ParentTableRef.Object;
				retval.Append("[" + parentTable.DatabaseName + "].[" + parentColumn.DatabaseName + "] -> ");
				retval.Append("[" + childTable.DatabaseName + "].[" + childColumn.DatabaseName + "]");
				if (kk < relation.ColumnRelationships.Count - 1)
				{
					retval.Append(", ");
				}
			}
			return retval.ToString();
		}

		private void AppendChildTableColumns(Relation relation)
		{
			for (int kk = 0; kk < relation.ColumnRelationships.Count; kk++)
			{
				ColumnRelationship columnRelationship = relation.ColumnRelationships[kk];
				sb.Append("		[" + ((Column)columnRelationship.ChildColumnRef.Object).DatabaseName + "]");
				if (kk < relation.ColumnRelationships.Count - 1)
				{
					sb.Append(",");
				}
				sb.AppendLine();
			}
		}

		private void AppendParentTableColumns(Relation relation)
		{
			for (int jj = 0; jj < relation.ColumnRelationships.Count; jj++)
			{
				ColumnRelationship columnRelationship = relation.ColumnRelationships[jj];
				sb.Append("		[" + ((Column)columnRelationship.ParentColumnRef.Object).DatabaseName + "]");
				if (jj < relation.ColumnRelationships.Count - 1)
				{
					sb.Append(",");
				}
				sb.AppendLine();
			}
		}

		#endregion

		#region AppendCreateIndexes

		private void AppendCreateIndexes()
		{
			foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			{
				string tableName = Globals.GetTableDatabaseName(_model, table);
				foreach (Reference reference in table.Columns)
				{
					Column column = (Column)reference.Object;
					if (column.IsIndexed && !column.IsUnique)
					{
						//Make sure that the index name is the same each time
						string indexName = "IDX_" + table.Name.Replace("-", "") + "_" + column.Name.Replace("-", "");
						indexName = indexName.ToUpper();

						sb.AppendLine("--INDEX FOR TABLE [" + ((Table)column.ParentTableRef.Object).DatabaseName + "] COLUMN [" + column.DatabaseName + "]");
						sb.AppendLine("if not exists(select * from sysindexes where name = '" + indexName + "')");
						sb.AppendLine("CREATE INDEX [" + indexName + "] ON [dbo].[" + tableName + "]([" + column.DatabaseName + "]) ON [PRIMARY]");
						sb.AppendLine("GO");
						sb.AppendLine();
					}
				}

			}
		}

		#endregion

		#region AppendCreateUniqueKey

		private void AppendCreateUniqueKey()
		{
			foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			{
				string tableName = Globals.GetTableDatabaseName(_model, table);
				foreach (Reference reference in table.Columns)
				{
					//If this is a non-key column that is unqiue then create the SQL KEY
					Column column = (Column)reference.Object;
					if (column.IsUnique && !table.PrimaryKeyColumns.Contains(column))
					{
						//Make sure that the index name is the same each time
						string indexName = "IX_" + table.Name.Replace("-", "") + "_" + column.Name.Replace("-", "");
						indexName = indexName.ToUpper();
						
						sb.AppendLine("--UNIQUE COLUMN TABLE [" + tableName + "].[" + column.DatabaseName + "] (NON-PRIMARY KEY)");
						sb.AppendLine("if not exists(select * from sysindexes where name = '" + indexName + "')");						
						sb.AppendLine("ALTER TABLE [" + tableName + "] ADD CONSTRAINT [" + indexName + "] UNIQUE ([" + column.DatabaseName + "]) ");
						sb.AppendLine("GO");
						sb.AppendLine();
					}
				}
			}
		}

		#endregion

		#region AppendCreateDefaults

		private void AppendCreateDefaults()
		{
			foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			{
				bool used = false;

				//Add Defaults
				foreach (Reference columnRef in table.Columns)
				{
					Column column = (Column)columnRef.Object;
					string defaultText = SQLGeneration.AppendColumnDefaultSQL(column);
					if (defaultText != "")
					{
						if (!used) sb.AppendLine("--DEFAULTS FOR TABLE [" + ((Table)column.ParentTableRef.Object).DatabaseName + "]");
						used = true;
						sb.Append(defaultText);
					}
				}

				if (used)
				{
					sb.AppendLine("GO");
					sb.AppendLine();
				}

			}
		}

		#endregion

		#region AppendViews

		private void AppendViews()
		{
			try
			{
				foreach (CustomView customView in _model.Database.CustomViews)
				{
					sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
					sb.AppendLine("--Data Schema For Version " + _model.Version);
					ValidationHelper.AppendCopyrightInSQL(sb, _model);
					sb.AppendLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[view_" + customView.DatabaseName + "]') and OBJECTPROPERTY(id, N'IsView') = 1)");
					sb.AppendLine("drop view [dbo].[view_" + customView.DatabaseName + "]");
					sb.AppendLine("GO");
					sb.AppendLine();
					sb.AppendLine("SET QUOTED_IDENTIFIER ON ");
					sb.AppendLine("GO");
					sb.AppendLine("SET ANSI_NULLS ON ");
					sb.AppendLine("GO");
					sb.AppendLine();
					sb.AppendLine("CREATE VIEW dbo.view_" + customView.DatabaseName);
					sb.AppendLine("AS");
					sb.AppendLine(customView.SQL);
					sb.AppendLine("GO");
					sb.AppendLine("SET QUOTED_IDENTIFIER OFF ");
					sb.AppendLine("GO");
					sb.AppendLine("SET ANSI_NULLS ON ");
					sb.AppendLine("GO");
					sb.AppendLine();
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

		#region AppendCreateAudit

		private void AppendCreateAudit()
		{
			foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			{
				if (table.AllowCreateAudit)
				{
					sb.AppendLine("--APPEND AUDIT TRAIL CREATE for Table [" + table.DatabaseName + "]");
					sb.AppendLine("if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + _model.Database.CreatedByColumnName + "' and o.name = '" + table.DatabaseName + "')");
					sb.AppendLine("ALTER TABLE [" + table.DatabaseName + "] ADD [" + _model.Database.CreatedByColumnName + "] [varchar] (50) NULL");
					string dfName = "DF__" + table.DatabaseName + "_" + _model.Database.CreatedDateColumnName;
					dfName = dfName.ToUpper();
					sb.AppendLine("if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + _model.Database.CreatedDateColumnName + "' and o.name = '" + table.DatabaseName + "')");
					sb.AppendLine("ALTER TABLE [" + table.DatabaseName + "] ADD [" + _model.Database.CreatedDateColumnName + "] [datetime] CONSTRAINT [" + dfName + "] DEFAULT " + (_model.UseUTCTime ? "GetUTCDate()" : "GetDate()") + " NULL");
					sb.AppendLine();
				}

				if (table.AllowModifiedAudit)
				{
					sb.AppendLine("--APPEND AUDIT TRAIL MODIFY for Table [" + table.DatabaseName + "]");
					sb.AppendLine("if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + _model.Database.ModifiedByColumnName + "' and o.name = '" + table.DatabaseName + "')");
					sb.AppendLine("ALTER TABLE [" + table.DatabaseName + "] ADD [" + _model.Database.ModifiedByColumnName + "] [varchar] (50) NULL");
					string dfName = "DF__" + table.DatabaseName + "_" + _model.Database.ModifiedDateColumnName;
					dfName = dfName.ToUpper();
					sb.AppendLine("if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + _model.Database.ModifiedDateColumnName + "' and o.name = '" + table.DatabaseName + "')");
					sb.AppendLine("ALTER TABLE [" + table.DatabaseName + "] ADD [" + _model.Database.ModifiedDateColumnName + "] [datetime] CONSTRAINT [" + dfName + "] DEFAULT " + (_model.UseUTCTime ? "GetUTCDate()" : "GetDate()") + " NULL");
					sb.AppendLine();
				}

				if (table.AllowTimestamp)
				{
					sb.AppendLine("--APPEND AUDIT TRAIL TIMESTAMP for Table [" + table.DatabaseName + "]");
					sb.AppendLine("if not exists (select * from syscolumns c inner join sysobjects o on c.id = o.id where c.name = '" + _model.Database.TimestampColumnName + "' and o.name = '" + table.DatabaseName + "')");
					sb.AppendLine("ALTER TABLE [" + table.DatabaseName + "] ADD [" + _model.Database.TimestampColumnName + "] [timestamp] NOT NULL");
					sb.AppendLine();
				}

			}
		}

		#endregion

		#region 

		private void AppendAuditTriggers()
		{
			foreach (Table table in (from x in _model.Database.Tables where x.Generated orderby x.Name select x))
			{
				sb.AppendLine("--DROP ANY AUDIT TRIGGERS FOR [" + table.DatabaseName + "]");
				sb.AppendLine("if exists(select * from sysobjects where name = '__TR_" + table.DatabaseName + "__INSERT' AND xtype = 'TR')");
				sb.AppendLine("DROP TRIGGER [__TR_" + table.DatabaseName + "__INSERT]");
				sb.AppendLine("GO");
				sb.AppendLine("if exists(select * from sysobjects where name = '__TR_" + table.DatabaseName + "__UPDATE' AND xtype = 'TR')");
				sb.AppendLine("DROP TRIGGER [__TR_" + table.DatabaseName + "__UPDATE]");
				sb.AppendLine("GO");
				sb.AppendLine("if exists(select * from sysobjects where name = '__TR_" + table.DatabaseName + "__DELETE' AND xtype = 'TR')");
				sb.AppendLine("DROP TRIGGER [__TR_" + table.DatabaseName + "__DELETE]");
				sb.AppendLine("GO");
				sb.AppendLine();

				if (table.AllowAuditTracking)
				{
					ColumnCollection columnList = table.GetColumns();
					string columnText = "";
					foreach (Column column in table.GetColumns())
					{
						columnText += "[" + column.DatabaseName + "]";
						if (columnList.IndexOf(column) < columnList.Count - 1) columnText += ",";
					}

					sb.AppendLine("--CREATE TRIGGER INSERT FOR [" + table.DatabaseName + "]");					
					sb.AppendLine("CREATE TRIGGER [__TR_" + table.DatabaseName + "__INSERT]");
					sb.AppendLine("ON [" + table.DatabaseName + "]");
					sb.AppendLine("FOR INSERT AS");
					sb.AppendLine("INSERT INTO [__AUDIT__" + table.DatabaseName + "] ([__action]," + columnText + ")");
					sb.AppendLine("SELECT 1, " + columnText + " FROM [inserted]");
					sb.AppendLine("GO");
					sb.AppendLine();
					sb.AppendLine("--CREATE TRIGGER UPDATE FOR [" + table.DatabaseName + "]");
					sb.AppendLine("CREATE TRIGGER [__TR_" + table.DatabaseName + "__UPDATE]");
					sb.AppendLine("ON [" + table.DatabaseName + "]");
					sb.AppendLine("FOR UPDATE AS");
					sb.AppendLine("INSERT INTO [__AUDIT__" + table.DatabaseName + "] ([__action]," + columnText + ")");
					sb.AppendLine("SELECT 2, " + columnText + " FROM [inserted]");
					sb.AppendLine("GO");
					sb.AppendLine();
					sb.AppendLine("--CREATE TRIGGER DELETE FOR [" + table.DatabaseName + "]");
					sb.AppendLine("CREATE TRIGGER [__TR_" + table.DatabaseName + "__DELETE]");
					sb.AppendLine("ON [" + table.DatabaseName + "]");
					sb.AppendLine("FOR DELETE AS");
					sb.AppendLine("INSERT INTO [__AUDIT__" + table.DatabaseName + "] ([__action]," + columnText + ")");
					sb.AppendLine("SELECT 3, " + columnText + " FROM [deleted]");
					sb.AppendLine("GO");
					sb.AppendLine();
				}

			}
		}

		#endregion

		#endregion
	}
}