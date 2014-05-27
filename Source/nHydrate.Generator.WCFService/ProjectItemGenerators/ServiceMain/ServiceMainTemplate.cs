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

namespace Widgetsphere.Generator.WCFService.ProjectItemGenerators.ServiceMain
{
	class ServiceMainTemplate : ServiceBaseTemplate
	{
		private readonly StringBuilder sb = new StringBuilder();

		#region Constructors
		public ServiceMainTemplate(ModelRoot model)
		{
			_model = model;
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
			get { return "DataService.svc.cs"; }
		}

		public string ParentItemName
		{
			get { return "DataService.svc"; }
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
			sb.AppendLine("using System.Runtime.Serialization;");
			sb.AppendLine("using System.ServiceModel;");
			sb.AppendLine("using System.Text;");
			//sb.AppendLine("using " + this.DefaultNamespace + ".DataTransfer;");
			sb.AppendLine("using " + this.DefaultNamespace + ".DALProxy.Extensions;");
			sb.AppendLine("using Widgetsphere.Core.Util;");
			sb.AppendLine();
		}

		public void AppendClass()
		{
			sb.AppendLine("	// NOTE: If you change the class name \"DataService\" here, you must also update the reference to \"DataService\" in Web.config and in the associated .svc file.");
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// This WCF service allows you to interact with all generated objects");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	public class DataService : IDataService");
			sb.AppendLine("	{");

			foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable).OrderBy(x => x.Name))
			{
				if (table.Generated)
				{
					sb.AppendLine("		#region " + table.PascalName);

					#region Select All
					sb.AppendLine("		#region " + table.PascalName + " Select All");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Get a list of all " + table.PascalName + " DTO items");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public List<" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + "> Get" + table.PascalName + "List()");
					sb.AppendLine("		{");
					sb.AppendLine("			var retval = new List<" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + ">();");
					sb.AppendLine("			retval.RunSelect();");
					sb.AppendLine("			return retval;");
					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		#endregion");
					sb.AppendLine();
					#endregion

					#region Select By PK
					sb.AppendLine("		#region " + table.PascalName + " Select By Primary Key");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Get a single " + table.PascalName + " DTO item by primary key");
					sb.AppendLine("		/// </summary>");
					sb.Append("		public " + this.DefaultNamespace + ".DataTransfer." + table.PascalName + " Get" + table.PascalName + "(");

					var ii = 0;
					foreach (var column in table.PrimaryKeyColumns.OrderBy(x => x.Name))
					{
						sb.Append(column.GetCodeType() + " " + column.CamelName);
						if (ii < table.PrimaryKeyColumns.Count - 1)
							sb.Append(", ");
						ii++;
					}

					sb.AppendLine(")");
					sb.AppendLine("		{");
					sb.AppendLine("			var retVal = new " + this.DefaultNamespace + ".DataTransfer." + table.PascalName + "();");
					sb.Append("			retVal.SelectUsingPK(");

					ii = 0;
					foreach (var column in table.PrimaryKeyColumns.OrderBy(x => x.Name))
					{
						sb.Append(column.CamelName);
						if (ii < table.PrimaryKeyColumns.Count - 1)
							sb.Append(", ");
						ii++;
					}

					sb.AppendLine(");");
					sb.AppendLine("			return retVal;");
					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		#endregion");
					sb.AppendLine();
					#endregion

					#region Paged
					sb.AppendLine("		#region " + table.PascalName + " select paged");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Get a paginated list of " + table.PascalName + " DTO items using LINQ");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public " + this.DefaultNamespace + ".DataTransfer.PagedQueryResults<" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + "> Get" + table.PascalName + "ListPaged(int pageIndex, int pageSize, bool sortOrder, string sortColumn, string linq)");
					sb.AppendLine("		{");
					sb.AppendLine("			var dtoList = new List<" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + ">();");
					sb.AppendLine("			return dtoList.RunSelect(pageSize.ToString(), pageIndex.ToString(), sortOrder.ToString().ToLower(), sortColumn, linq);");
					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		#endregion");
					sb.AppendLine();
					#endregion

					#region Linq non-paged
					sb.AppendLine("		#region " + table.PascalName + " Select by LINQ");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Get a list of " + table.PascalName + " DTO items using LINQ");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public List<" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + "> Get" + table.PascalName + "ListByLINQ(string linq)");
					sb.AppendLine("		{");
					sb.AppendLine("			var dtoList = new List<" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + ">();");
					sb.AppendLine("			dtoList.RunSelect(linq);");
					sb.AppendLine("			return dtoList;");
					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		#endregion");
					sb.AppendLine();
					#endregion

					#region Select By Column
					sb.AppendLine("		#region " + table.PascalName + " Select by Column");
					sb.AppendLine();

					foreach (var column in table.GetColumnsSearchable(true))
					{
						sb.AppendLine("		/// <summary>");
						sb.AppendLine("		/// Get a list of " + table.PascalName + " DTO items by the " + column.PascalName + " field");
						sb.AppendLine("		/// </summary>");
						sb.AppendLine("		public List<" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + "> Get" + table.PascalName + "By" + column.PascalName + "Field(" + column.GetCodeType(true) + " " + column.CamelName + ")");
						sb.AppendLine("		{");
						sb.AppendLine("			var retval = new List<" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + ">();");
						sb.AppendLine("			retval.SelectBy" + column.PascalName + "Field(" + column.CamelName + ");");
						sb.AppendLine("			return retval;");
						sb.AppendLine("		}");
						sb.AppendLine();
					}

					sb.AppendLine("		#endregion");
					sb.AppendLine();
					#endregion

					#region Select by FK
					sb.AppendLine("		#region " + table.PascalName + " Select By Foreign Key");
					sb.AppendLine();

					var allRelations = table.GetRelationsFullHierarchy();
					foreach (var relation in allRelations)
					{
						if (!relation.ParentTable.Generated || !relation.ChildTable.Generated)
						{
							//Do Nothing
						}
						else
						{
							//Handle relations by inheritance
							var childTable = relation.ChildTable;
							if (table.IsInheritedFrom(childTable))
								childTable = table;

							if (childTable.IsInheritedFrom(relation.ParentTable))
							{
								//Do Nothing
							}
							else if (childTable == table)
							{
								sb.AppendLine("		/// <summary>");
								sb.AppendLine("		/// Get a list of " + table.PascalName + " DTO items by the " + relation.ParentTable.PascalName + " foreign key");
								sb.AppendLine("		/// </summary>");
								sb.Append("		public List<" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + "> Get" + table.PascalName + "By" + relation.PascalRoleName + relation.ParentTable.PascalName + "(");

								var columnList = new List<Column>();
								foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
								{
									columnList.Add(columnRelationship.ParentColumnRef.Object as Column);
								}

								var index = 0;
								foreach (var column in columnList.OrderBy(x => x.Name))
								{
									sb.Append(column.GetCodeType(true) + " " + column.CamelName);
									if (index < columnList.Count - 1)
										sb.Append(", ");
									index++;
								}

								sb.AppendLine(")");
								sb.AppendLine("		{");
								sb.AppendLine("			var retval = new List<" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + ">();");
								sb.Append("			retval.SelectBy" + relation.PascalRoleName + relation.ParentTable.PascalName + "(");

								index = 0;
								foreach (var column in columnList.OrderBy(x => x.Name))
								{
									sb.Append(column.CamelName);
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

					sb.AppendLine("		#endregion");
					sb.AppendLine();
					#endregion

					#region Persist
					if (!table.Immutable)
					{
						sb.AppendLine("		#region " + table.PascalName + " Add Item");
						sb.AppendLine();
						sb.AppendLine("		/// <summary>");
						sb.AppendLine("		/// Persist a single " + table.PascalName + " DTO item");
						sb.AppendLine("		/// </summary>");
						sb.AppendLine("		public " + this.DefaultNamespace + ".DataTransfer." + table.PascalName + " Persist" + table.PascalName + "(" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + " item)");
						sb.AppendLine("		{");
						sb.AppendLine("			return item.Persist();");
						sb.AppendLine("		}");
						sb.AppendLine();
						sb.AppendLine("		#endregion");
						sb.AppendLine();
					}
					#endregion

					#region Delete
					if (!table.Immutable)
					{
						sb.AppendLine("		#region " + table.PascalName + " Delete");
						sb.AppendLine();
						sb.AppendLine("		/// <summary>");
						sb.AppendLine("		/// Delete a single " + table.PascalName + " item");
						sb.AppendLine("		/// </summary>");
						sb.AppendLine("		public bool Delete" + table.PascalName + "(" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + " item)");
						sb.AppendLine("		{");
						sb.AppendLine("			return item.Delete();");
						sb.AppendLine("		}");
						sb.AppendLine();
						sb.AppendLine("		#endregion");
					}
					#endregion

					sb.AppendLine("		#endregion");
					sb.AppendLine();

				}
			}

			sb.AppendLine("	}");
			sb.AppendLine();
		}

		#endregion

	}
}