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

namespace Widgetsphere.Generator.WCFService.ProjectItemGenerators.ServiceInterface
{
	class ServiceInterfaceTemplate : ServiceBaseTemplate
	{
		private readonly StringBuilder sb = new StringBuilder();		

		#region Constructors
		public ServiceInterfaceTemplate(ModelRoot model)
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
			get { return "IDataService.cs"; }
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
			sb.AppendLine("using Widgetsphere.Core.Util;");
			sb.AppendLine();
		}

		public void AppendClass()
		{
			sb.AppendLine("	// NOTE: If you change the interface name \"IDataService\" here, you must also update the reference to \"IDataService\" in Web.config.");
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// ");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	[ServiceContract]");
			sb.AppendLine("	public interface IDataService");
			sb.AppendLine("	{");
			sb.AppendLine();

			foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable).OrderBy(x => x.Name))
			{
				if (table.Generated)
				{
					#region Select All
					sb.AppendLine("		#region " + table.PascalName + " Select All");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Get a list of all " + table.PascalName + " DTO items");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		[OperationContract]");
					sb.AppendLine("		List<" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + "> Get" + table.PascalName + "List();");
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
					sb.AppendLine("		[OperationContract]");
					sb.Append("		" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + " Get" + table.PascalName + "(");

					var ii = 0;
					foreach (var column in table.PrimaryKeyColumns.OrderBy(x => x.Name))
					{
						sb.Append(column.GetCodeType() + " " + column.CamelName);
						if (ii < table.PrimaryKeyColumns.Count - 1)
							sb.Append(", ");
						ii++;
					}

					sb.AppendLine(");");
					sb.AppendLine();
					sb.AppendLine("		#endregion");
					sb.AppendLine();
					#endregion

					#region Select Paged
					sb.AppendLine("		#region " + table.PascalName + " select paged");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Get a paginated list of " + table.PascalName + " DTO items");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		[OperationContract]");
					sb.AppendLine("		" + this.DefaultNamespace + ".DataTransfer.PagedQueryResults<" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + "> Get" + table.PascalName + "ListPaged(int pageIndex, int pageSize, bool sortOrder, string sortColumn, string linq);");
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
					sb.AppendLine("		[OperationContract]");
					sb.AppendLine("		List<" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + "> Get" + table.PascalName + "ListByLINQ(string linq);");
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
						sb.AppendLine("		[OperationContract]");
						sb.AppendLine("		List<" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + "> Get" + table.PascalName + "By" + column.PascalName + "Field(" + column.GetCodeType(true) + " " + column.CamelName + ");");
						sb.AppendLine();
					}

					sb.AppendLine("		#endregion");
					sb.AppendLine();
					#endregion

					#region Select By FK
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
							var childTable = relation.ChildTable;
							//Handle relations by inheritance
							if (table.IsInheritedFrom(relation.ChildTable))
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
								sb.AppendLine("		[OperationContract]");
								sb.Append("		List<" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + "> Get" + table.PascalName + "By" + relation.PascalRoleName + relation.ParentTable.PascalName + "(");

								var index = 0;
								var columnList = relation.ColumnRelationships.Select(x => x.ParentColumnRef.Object as Column);
								foreach (var column in columnList.OrderBy(x => x.Name))
								{
									sb.Append(column.GetCodeType(true) + " " + column.CamelName);
									if (index < columnList.Count() - 1)
										sb.Append(", ");
									index++;
								}

								sb.AppendLine(");");
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
						sb.AppendLine("		#region " + table.PascalName + " Persist");
						sb.AppendLine();
						sb.AppendLine("		/// <summary>");
						sb.AppendLine("		/// Persist a single " + table.PascalName + " DTO item");
						sb.AppendLine("		/// </summary>");
						sb.AppendLine("		[OperationContract]");
						sb.AppendLine("		" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + " Persist" + table.PascalName + "(" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + " item);");
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
						sb.AppendLine("		/// Delete a single " + table.PascalName + " DTO item");
						sb.AppendLine("		/// </summary>");
						sb.AppendLine("		[OperationContract]");
						sb.AppendLine("		bool Delete" + table.PascalName + "(" + this.DefaultNamespace + ".DataTransfer." + table.PascalName + " item);");
						sb.AppendLine();
						sb.AppendLine("		#endregion");
						sb.AppendLine();
					}
					#endregion

				}
			}
			
			sb.AppendLine("	}");
			sb.AppendLine();
		}

		#endregion
		
	}
}