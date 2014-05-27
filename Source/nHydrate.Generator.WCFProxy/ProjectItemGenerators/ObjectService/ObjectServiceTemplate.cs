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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.ProjectItemGenerators;

namespace Widgetsphere.Generator.IOCExtensions.ProjectItemGenerators.ObjectService
{
	class ObjectServiceTemplate : BaseWCFProxyTemplate
	{
		private readonly StringBuilder sb = new StringBuilder();
		private readonly Table _currentTable = null;

		#region Constructors
		public ObjectServiceTemplate(ModelRoot model, Table table)
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
			get { return _currentTable.PascalName + "Service.Generated.cs"; }
		}

		public string ParentItemName
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
			sb.AppendLine("using System.Linq.Expressions;");
			sb.AppendLine();
		}

		public void AppendClass()
		{
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// The service implementation for " + _currentTable.PascalName + " object");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	public partial class " + _currentTable.PascalName + "Service : " + this.DefaultNamespace + ".Service.Interfaces.I" + _currentTable.PascalName + "Service");
			sb.AppendLine("	{");
			sb.AppendLine("		#region I" + _currentTable.PascalName + "Service Members");
			sb.AppendLine();
			this.AppendMethodSelectByPK();
			this.AppendMethodPersist();
			this.AppendMethodRunSelect();
			this.AppendMethodSelectByRelation();
			this.AppendMethodSelectByField();
			this.AppendMethodDelete();
			sb.AppendLine("		#endregion");
			sb.AppendLine("	}");
		}

		private void AppendMethodRunSelect()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Run a selection using LINQ");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public List<" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + "> RunSelect(string linq)");
			sb.AppendLine("		{");
			sb.AppendLine("			var retVal = new List<" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + ">();");
			sb.AppendLine("			retVal.AddRange(DataService.Instance.Get" + _currentTable.PascalName + "ListByLINQ(linq));");
			sb.AppendLine("			return retVal;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Run a paginated selection using LINQ");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + this.DefaultNamespace + ".DataTransfer.PagedQueryResults<" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + "> RunSelect(int pageIndex, int pageSize, bool ascending, string sortColumn, string linq)");
			sb.AppendLine("		{");
			sb.AppendLine("			return DataService.Instance.Get" + _currentTable.PascalName + "ListPaged(pageIndex, pageSize, ascending, sortColumn, linq);");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Run a selection for all objects");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public List<" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + "> RunSelect()");
			sb.AppendLine("		{");
			sb.AppendLine("			var retVal = new List<" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + ">();");
			sb.AppendLine("			var ds = DataService.Instance;");
			sb.AppendLine("			retVal.AddRange(ds.Get" + _currentTable.PascalName + "List());");
			sb.AppendLine("			return retVal;");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendMethodPersist()
		{
			if (!_currentTable.Immutable)
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Run the persist for a list");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public bool Persist(List<" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + "> list)");
				sb.AppendLine("		{");
				sb.AppendLine("			var retval = true;");
				sb.AppendLine("			foreach (" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + " " + _currentTable.CamelName + " in list)");
				sb.AppendLine("			{");
				sb.AppendLine("				retval &= this.Persist(" + _currentTable.CamelName + ");");
				sb.AppendLine("			}");
				sb.AppendLine("			return retval;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Run the persist for an object");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public bool Persist(" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + " dto)");
				sb.AppendLine("		{");
				sb.AppendLine("			return (DataService.Instance.Persist" + _currentTable.PascalName + "(dto) != null);");
				sb.AppendLine("		}");
				sb.AppendLine();
			}
		}

		private void AppendMethodSelectByPK()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Run a selection by the primary key");
			sb.AppendLine("		/// </summary>");
			sb.Append("		public " + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + " SelectByPrimaryKey(");

			var ii = 0;
			foreach (var column in _currentTable.PrimaryKeyColumns.OrderBy(x => x.Name))
			{
				sb.Append(column.GetCodeType() + " " + column.CamelName);
				if (ii < _currentTable.PrimaryKeyColumns.Count - 1)
					sb.Append(", ");
				ii++;
			}
			sb.AppendLine(")");
			sb.AppendLine("		{");
			sb.Append("			return DataService.Instance.Get" + _currentTable.PascalName + "(");

			ii = 0;
			foreach (var column in _currentTable.PrimaryKeyColumns.OrderBy(x => x.Name))
			{
				sb.Append(column.CamelName);
				if (ii < _currentTable.PrimaryKeyColumns.Count - 1)
					sb.Append(", ");
				ii++;
			}
			sb.AppendLine(");");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendMethodSelectByRelation()
		{
			//Get by parent relations
			foreach (var relation in _currentTable.GetRelations().Where(x => x.IsGenerated))
			{
				//Do not render relations from inheritance
				if (!relation.ChildTable.IsInheritedFrom(relation.ParentTable) && relation.ChildTable.Generated && relation.IsOneToOne)
				{
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Return a related " + relation.ChildTable.PascalName + " object");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public " + this.DefaultNamespace + ".DataTransfer." + relation.ChildTable.PascalName + " Get" + relation.PascalRoleName + relation.ChildTable.PascalName + "Item(" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + " item)");
					sb.AppendLine("		{");
					sb.AppendLine("			var retval = new List<" + this.DefaultNamespace + ".DataTransfer." + relation.ChildTable.PascalName + ">();");
					sb.AppendLine("			var ds = DataService.Instance;");

					sb.AppendLine("			string s = string.Empty;");
					foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
					{
						var parentColumn = columnRelationship.ParentColumnRef.Object as Column;
						var childColumn = columnRelationship.ChildColumnRef.Object as Column;

						var v = "			s += \"x." + childColumn.PascalName + " = ";
						if (parentColumn.IsTextType) v += ((char)92).ToString() + ((char)34).ToString();
						v += "\" + item.";
						v += parentColumn.PascalName + " + \"";
						if (parentColumn.IsTextType) v += ((char)92).ToString() + ((char)34).ToString();
						v += "\"";

						if (relation.ColumnRelationships.IndexOf(columnRelationship) < relation.ColumnRelationships.Count - 1)
							v += " + \" && \"";

						sb.AppendLine(v + ";");
					}				
					sb.AppendLine("			retval.AddRange(ds.Get" + relation.ChildTable.PascalName + "ListByLINQ(\"x => \" + s));");

					sb.AppendLine("			return retval.FirstOrDefault();");
					sb.AppendLine("		}");
					sb.AppendLine();
				}
				else if (!relation.ChildTable.IsInheritedFrom(relation.ParentTable) && relation.ChildTable.Generated && relation.IsManyToMany)
				{
					//Relationship M:N
					var otherTable = relation.GetSecondaryAssociativeTable();

					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Return a list of related " + relation.ChildTable.PascalName + " objects");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public List<" + this.DefaultNamespace + ".DataTransfer." + otherTable.PascalName + "> Get" + relation.PascalRoleName + otherTable.PascalName + "List(" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + " item)");
					sb.AppendLine("		{");
					sb.AppendLine("			var retval = new List<" + this.DefaultNamespace + ".DataTransfer." + otherTable.PascalName + ">();");
					sb.AppendLine("			var ds = DataService.Instance;");

					sb.AppendLine("			string s = string.Empty;");
					foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
					{
						var parentColumn = columnRelationship.ParentColumnRef.Object as Column;
						var childColumn = columnRelationship.ChildColumnRef.Object as Column;

						var v = "			s += \"x." + childColumn.PascalName + " = ";
						if (parentColumn.IsTextType) v += ((char)92).ToString() + ((char)34).ToString();
						v += "\" + item.";
						v += parentColumn.PascalName + " + \"";
						if (parentColumn.IsTextType) v += ((char)92).ToString() + ((char)34).ToString();
						v += "\"";

						if (relation.ColumnRelationships.IndexOf(columnRelationship) < relation.ColumnRelationships.Count - 1)
							v += " + \" && \"";

						sb.AppendLine(v + ";");
					}
					sb.AppendLine("			retval.AddRange(ds.Get" + otherTable.PascalName + "ListByLINQ(\"x => \" + s));");

					sb.AppendLine("			return retval;");
					sb.AppendLine("		}");
					sb.AppendLine();
				}
				else if (!relation.ChildTable.IsInheritedFrom(relation.ParentTable) && relation.ChildTable.Generated)
				{
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Return a list of related " + relation.ChildTable.PascalName + " objects");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public List<" + this.DefaultNamespace + ".DataTransfer." + relation.ChildTable.PascalName + "> Get" + relation.PascalRoleName + relation.ChildTable.PascalName + "List(" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + " item)");
					sb.AppendLine("		{");
					sb.AppendLine("			var retval = new List<" + this.DefaultNamespace + ".DataTransfer." + relation.ChildTable.PascalName + ">();");
					sb.AppendLine("			var ds = DataService.Instance;");

					sb.AppendLine("			string s = string.Empty;");
					foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
					{
						var parentColumn = columnRelationship.ParentColumnRef.Object as Column;
						var childColumn = columnRelationship.ChildColumnRef.Object as Column;

						var v = "			s += \"x." + childColumn.PascalName + " = ";
						if (parentColumn.IsTextType) v += ((char)92).ToString() + ((char)34).ToString();
						v += "\" + item.";
						v += parentColumn.PascalName + " + \"";
						if (parentColumn.IsTextType) v += ((char)92).ToString() + ((char)34).ToString();
						v += "\"";

						if (relation.ColumnRelationships.IndexOf(columnRelationship) < relation.ColumnRelationships.Count - 1)
							v += " + \" && \"";

						sb.AppendLine(v + ";");
					}				
					sb.AppendLine("			retval.AddRange(ds.Get" + relation.ChildTable.PascalName + "ListByLINQ(\"x => \" + s));");

					sb.AppendLine("			return retval;");
					sb.AppendLine("		}");
					sb.AppendLine();
				}

			}

			//Get by child relations
			foreach (var relation in _currentTable.GetChildRoleRelationsFullHierarchy().Where(x => x.IsGenerated))
			{
				if (!relation.ParentTable.Generated || !relation.ChildTable.Generated)
				{
					//Do Nothing
				}

				//Do not render relations from inheritance
				else if (!relation.ChildTable.IsInheritedFrom(relation.ParentTable))
				{
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Return the related " + relation.ParentTable.PascalName + " item");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public " + this.DefaultNamespace + ".DataTransfer." + relation.ParentTable.PascalName + " Get" + relation.PascalRoleName + relation.ParentTable.PascalName + "Item(" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + " item)");
					sb.AppendLine("		{");
					sb.AppendLine("			var retval = new " + this.DefaultNamespace + ".DataTransfer." + relation.ParentTable.PascalName + "();");

					#region Check for Nulls
					var checkCount = 0;
					var sbChild = new StringBuilder();
					sbChild.AppendLine("			if (false ||");
					foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
					{
						var parentColumn = columnRelationship.ParentColumnRef.Object as Column;
						var childColumn = columnRelationship.ChildColumnRef.Object as Column;
						if (childColumn.AllowNull)
						{
							checkCount++;
							sbChild.AppendLine("				(item." + childColumn.PascalName + " == null) ||");
						}
					}
					sbChild.AppendLine("				false");
					sbChild.AppendLine("			)");
					sbChild.AppendLine("			{");
					sbChild.AppendLine("				//The primary key cannot be null, so there is no object");
					sbChild.AppendLine("				return null;");
					sbChild.AppendLine("			}");
					if (checkCount > 0) sb.Append(sbChild.ToString());
					#endregion

					sb.AppendLine("			var ds = DataService.Instance;");
					sb.Append("			retval = ds.Get" + relation.ParentTable.PascalName + "(");

					var columnList = new SortedDictionary<string, Column>();
					foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
					{
						var parentColumn = columnRelationship.ParentColumnRef.Object as Column;
						var childColumn = columnRelationship.ChildColumnRef.Object as Column;
						columnList.Add(parentColumn.PascalName, childColumn);
					}

					var index = 0;
					foreach (var pair in columnList)
					{
						var childColumn = pair.Value;
						sb.Append("item." + childColumn.PascalName);
						if (childColumn.AllowNull && !childColumn.IsTextType)
							sb.Append(".Value");
						if (index < columnList.Count - 1)
							sb.Append(", ");
						index++;
					}
					sb.AppendLine(");");
					sb.AppendLine("			return retval;");
					sb.AppendLine("		}");
					sb.AppendLine();
				}
			}

		}

		private void AppendMethodSelectByField()
		{
			foreach (var column in _currentTable.GetColumnsFullHierarchy().Where(x => x.Generated && x.IsSearchable))
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Return a list of items by the " + column.PascalName + " field");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public List<" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + "> SelectBy" + column.PascalName + "(" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + " item, " + column.GetCodeType() + " " + column.CamelName + ")");
				sb.AppendLine("		{");
				sb.AppendLine("			var retval = new List<" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + ">();");
				sb.AppendLine("			var ds = DataService.Instance;");
				sb.AppendLine("			retval.AddRange(ds.Get" + _currentTable.PascalName + "By" + column.PascalName + "Field(" + column.CamelName + "));");
				sb.AppendLine("			return retval;");	
				sb.AppendLine("		}");
				sb.AppendLine();
			}
		}

		private void AppendMethodDelete()
		{
			if (!_currentTable.Immutable)
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Delete the specified item");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public bool Delete(" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + " item)");
				sb.AppendLine("		{");
				sb.AppendLine("			var ds = DataService.Instance;");
				sb.AppendLine("			return ds.Delete" + _currentTable.PascalName + "(item);");
				sb.AppendLine("		}");
				sb.AppendLine();
			}
		}

		#endregion

	}
}