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
using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.Common.Util;
using System.Collections;
using System.Collections.ObjectModel;

namespace Widgetsphere.Generator.ProjectItemGenerators.BusinessObjectLINQ
{
	class BusinessObjectLINQGeneratedTemplate : BaseClassTemplate
	{
		private StringBuilder sb = new StringBuilder();
		private Table _currentTable;

		public BusinessObjectLINQGeneratedTemplate(ModelRoot model, Table currentTable)
		{
			_model = model;
			_currentTable = currentTable;
		}

		#region BaseClassTemplate overrides
		public override string FileName
		{
			get { return string.Format("{0}.Generated.cs", _currentTable.PascalName); }
		}

		public string ParentItemName
		{
			get { return string.Format("{0}.cs", _currentTable.PascalName); }
		}

		public override string FileContent
		{
			get
			{
				GenerateContent();
				return sb.ToString();
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
				sb.AppendLine("namespace " + DefaultNamespace + ".Business.LINQ");
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
			sb.AppendLine("using System.Data;");
			sb.AppendLine("using System.Linq;");
			sb.AppendLine("using System.Data.Linq;");
			sb.AppendLine("using System.Linq.Expressions;");
			sb.AppendLine("using System.Data.Linq.Mapping;");
			sb.AppendLine("using System.Collections;");
			sb.AppendLine("using System.Collections.Generic;");
			sb.AppendLine("using Widgetsphere.Core.DataAccess;");
			sb.AppendLine();
		}

		private void AppendClass()
		{
			try
			{
				sb.AppendLine("	/// <summary>");
				sb.AppendLine("	/// This is a helper object for running LINQ queries on the " + _currentTable.PascalName + " collection.");
				sb.AppendLine("	/// </summary>");
				sb.AppendLine("	[Serializable()]");
				sb.AppendLine("	[Table(Name = \"" + _currentTable.DatabaseName + "\")]");
				sb.AppendLine("	public partial class " + _currentTable.PascalName + "Query");
				sb.AppendLine("	{");

				sb.AppendLine("		#region Properties");
				List<Table> allTables = _currentTable.GetTableHierarchy();

				foreach (Column c in _currentTable.GetColumnsFullHierarchy())
				{
					string description = c.Description.Trim();
					if (description != "") description += "\r\n		///";
					description += "(Maps to the '" + _currentTable.DatabaseName + "." + c.DatabaseName + "' database field)";

					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// " + description);
					sb.AppendLine("		/// </summary>");
					sb.Append("		[Column(");
					sb.Append("Name = \"" + c.DatabaseName + "\", ");
					sb.Append("DbType = \"" + c.DatabaseType + "\", ");
					sb.Append("CanBeNull = " + c.AllowNull.ToString().ToLower() + ", ");
					sb.Append("IsPrimaryKey = " + _currentTable.PrimaryKeyColumns.Contains(c).ToString().ToLower());
					sb.AppendLine(")]");
					sb.AppendLine("		public virtual " + c.GetCodeType(true) + " " + c.PascalName + " { get; set; }");
				}

				if (_currentTable.AllowCreateAudit)
				{
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// The date of creation");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		[Column(Name = \"" + _model.Database.CreatedDateColumnName + "\", DbType = \"DateTime\", CanBeNull = true)]");
					sb.AppendLine("		public virtual DateTime? " + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedDateColumnName) + " { get; set; }");

					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// The name of the creating entity");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		[Column(Name = \"" + _model.Database.CreatedByColumnName + "\", DbType = \"VarChar(100)\", CanBeNull = true)]");
					sb.AppendLine("		public virtual string " + StringHelper.DatabaseNameToPascalCase(_model.Database.CreatedByColumnName) + " { get; set; }");
				}

				if (_currentTable.AllowModifiedAudit)
				{
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// The date of last modification");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		[Column(Name = \"" + _model.Database.ModifiedDateColumnName + "\", DbType = \"DateTime\", CanBeNull = true)]");
					sb.AppendLine("		public virtual DateTime? " + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedDateColumnName) + " { get; set; }");

					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// The name of the last modifing entity");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		[Column(Name = \"" + _model.Database.ModifiedByColumnName + "\", DbType = \"VarChar(100)\", CanBeNull = true)]");
					sb.AppendLine("		public virtual string " + StringHelper.DatabaseNameToPascalCase(_model.Database.ModifiedByColumnName) + " { get; set; }");
				}

				if (_currentTable.AllowTimestamp)
				{
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// This is an internal field and is not to be used.");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		[Column(Name = \"" + _model.Database.TimestampColumnName + "\", DbType = \"Binary\", CanBeNull = false)]");
					sb.AppendLine("		public virtual byte[] " + StringHelper.DatabaseNameToPascalCase(_model.Database.TimestampColumnName) + " { get; set; }");
				}
				sb.AppendLine();

				//Add child relationships
				foreach (Relation relation in _model.Database.Relations.FindByParentTable(_currentTable, true).Where(x => x.IsGenerated))
				{
					//Relation relation = (Relation)reference.Object;
					Table childTable = (Table)relation.ChildTableRef.Object;
					//Column pkColumn = (Column)relation.ColumnRelationships[0].ChildColumnRef.Object;

					string thisKey = "";
					string otherKey = "";
					foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
					{
						thisKey += ((Column)columnRelationship.ParentColumnRef.Object).PascalName + ",";
						otherKey += ((Column)columnRelationship.ChildColumnRef.Object).PascalName + ",";
					}
					if (thisKey != "") thisKey = thisKey.Substring(0, thisKey.Length - 1);
					if (otherKey != "") otherKey = otherKey.Substring(0, otherKey.Length - 1);

					if ((childTable.Generated) && (!allTables.Contains(childTable)))
					{
						sb.AppendLine("		/// <summary>");
						sb.AppendLine("		/// This is a mapping of the relationship with the " + childTable.PascalName + " entity." + (relation.PascalRoleName == "" ? "" : " (Role: '" + relation.RoleName + "')"));
						sb.AppendLine("		/// </summary>");
						sb.AppendLine("		[Association(ThisKey = \"" + thisKey + "\", OtherKey = \"" + otherKey + "\")]");
						sb.AppendLine("		public " + childTable.PascalName + "Query " + relation.PascalRoleName + childTable.PascalName + "Entity { get; private set; }");
					}

				}

				//Add parent relationships
				foreach (Relation relation in _model.Database.Relations.FindByChildTable(_currentTable, true).Where(x => x.IsGenerated))
				{
					Table parentTable = (Table)relation.ParentTableRef.Object;

					//Do not process self-referencing relationships
					if (parentTable != _currentTable)
					{
						string thisKey = "";
						string otherKey = "";
						foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
						{
							thisKey += ((Column)columnRelationship.ChildColumnRef.Object).PascalName + ",";
							otherKey += ((Column)columnRelationship.ParentColumnRef.Object).PascalName + ",";
						}
						if (thisKey != "") thisKey = thisKey.Substring(0, thisKey.Length - 1);
						if (otherKey != "") otherKey = otherKey.Substring(0, otherKey.Length - 1);

						if ((parentTable.Generated) && (!allTables.Contains(parentTable)))
						{
							sb.AppendLine("		/// <summary>");
							sb.AppendLine("		/// This is a mapping of the relationship with the " + parentTable.PascalName + " entity." + (relation.PascalRoleName == "" ? "" : " (Role: '" + relation.RoleName + "')"));
							sb.AppendLine("		/// </summary>");
							sb.AppendLine("		[Association(ThisKey = \"" + thisKey + "\", OtherKey = \"" + otherKey + "\")]");
							sb.AppendLine("		public " + parentTable.PascalName + "Query " + relation.PascalRoleName + parentTable.PascalName + "Entity { get; private set; }");
						}

					}
				}

				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();
				sb.AppendLine("	}");
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