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

namespace Widgetsphere.Generator.MockProxy.ProjectItemGenerators.ObjectExtension
{
	class ObjectServiceTemplate : BaseMockProxyTemplate
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
			sb.AppendLine("using " + this.DefaultNamespace + ".MockProxy.EventArgs;");
			sb.AppendLine();
		}

		public void AppendClass()
		{
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// The service implementation for " + _currentTable.PascalName + " object");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	public partial class " + _currentTable.PascalName + "Service : " + this.DefaultNamespace + ".Service.Interfaces.I" + _currentTable.PascalName + "Service");
			sb.AppendLine("	{");
			this.GenerateIServiceMembers();
			this.GenerateEvents();
			sb.AppendLine("	}");
		}

		private void GenerateIServiceMembers()
		{
			sb.AppendLine("		#region I" + _currentTable.PascalName + "Service Members");
			sb.AppendLine();
			this.AppendMethodSelectByPK();
			this.AppendMethodRunSelect();
			this.AppendMethodSelectByRelation();
			this.AppendMethodSelectByField();
			this.AppendMethodPersist();
			this.AppendMethodDelete();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		private void AppendMethodRunSelect()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Run the mock for select all");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public List<" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + "> RunSelect()");
			sb.AppendLine("		{");
			sb.AppendLine("			var eventArgs = new " + _currentTable.PascalName + "RunSelectEventArgs();");
			sb.AppendLine("			this.OnMock" + _currentTable.PascalName + "RunSelect(eventArgs);");
			sb.AppendLine("			return eventArgs.ReturnValue;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Run the mock for select with LINQ");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public List<" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + "> RunSelect(string linq)");
			sb.AppendLine("		{");
			sb.AppendLine("			var eventArgs = new " + _currentTable.PascalName + "RunSelectLINQEventArgs(linq);");
			sb.AppendLine("			this.OnMock" + _currentTable.PascalName + "RunSelectLINQ(eventArgs);");
			sb.AppendLine("			return eventArgs.ReturnValue;");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Run the mock for paged select");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + this.DefaultNamespace + ".DataTransfer.PagedQueryResults<" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + "> RunSelect(int pageIndex, int pageSize, bool ascending, string sortColumn, string linq)");
			sb.AppendLine("		{");
			sb.AppendLine("			var eventArgs = new " + _currentTable.PascalName + "RunSelectPagedEventArgs(pageIndex, pageSize, ascending, sortColumn, linq);");
			sb.AppendLine("			this.OnMock" + _currentTable.PascalName + "RunSelectPaged(eventArgs);");
			sb.AppendLine("			return eventArgs.ReturnValue;");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendMethodSelectByPK()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Run the mock for the select by the primary key");
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
			sb.Append("			var eventArgs = new " + _currentTable.PascalName + "PrimaryKeyEventArgs(");

			ii = 0;
			foreach (var column in _currentTable.PrimaryKeyColumns.OrderBy(x => x.Name))
			{
				sb.Append(column.CamelName);
				if (ii < _currentTable.PrimaryKeyColumns.Count - 1)
					sb.Append(", ");
				ii++;
			}
			sb.AppendLine(");");
			sb.AppendLine("			this.OnMock" + _currentTable.PascalName + "SelectByPrimaryKey(eventArgs);");
			sb.AppendLine("			return eventArgs.ReturnValue;");
			sb.AppendLine("		}");
			sb.AppendLine();
		}

		private void AppendMethodSelectByRelation()
		{
			//Get by parent relations
			foreach (var relation in _currentTable.GetRelations().Where(x => x.IsGenerated))
			{
				if (!relation.ParentTable.Generated || !relation.ChildTable.Generated)
				{
					//Do Nothing
				}

				//Do not render relations from inheritance
				else if (!relation.ChildTable.IsInheritedFrom(relation.ParentTable) && relation.ChildTable.Generated && relation.IsOneToOne)
				{
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Return a related " + relation.ChildTable.PascalName);
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public " + this.DefaultNamespace + ".DataTransfer." + relation.ChildTable.PascalName + " Get" + relation.PascalRoleName + relation.ChildTable.PascalName + "Item(" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + " item)");
					sb.AppendLine("		{");
					sb.AppendLine("			var eventArgs = new " + _currentTable.PascalName + "DTOEventArgs(item);");
					sb.AppendLine("			this.OnMockGet" + relation.PascalRoleName + relation.ChildTable.PascalName + "Item(eventArgs);");
					sb.AppendLine("			var retval = new List<" + this.DefaultNamespace + ".DataTransfer." + relation.ChildTable.PascalName + ">();");
					sb.AppendLine("			retval.AddRange(eventArgs.ReturnDTOList.Select(x => (" + this.DefaultNamespace + ".DataTransfer." + relation.ChildTable.PascalName + ")x));");
					sb.AppendLine("			return retval.FirstOrDefault();");
					sb.AppendLine("		}");
					sb.AppendLine();
				}
				else if (!relation.ChildTable.IsInheritedFrom(relation.ParentTable) && relation.ChildTable.Generated && relation.IsManyToMany)
				{
					//Relationship N:M
					var otherTable = relation.GetSecondaryAssociativeTable();

					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// Return a list of related " + otherTable.PascalName + " objects");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public List<" + this.DefaultNamespace + ".DataTransfer." + otherTable.PascalName + "> Get" + relation.PascalRoleName + otherTable.PascalName + "List(" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + " item)");
					sb.AppendLine("		{");
					sb.AppendLine("			var eventArgs = new " + _currentTable.PascalName + "DTOEventArgs(item);");
					sb.AppendLine("			this.OnMockGet" + relation.PascalRoleName + otherTable.PascalName + "List(eventArgs);");
					sb.AppendLine("			var retval = new List<" + this.DefaultNamespace + ".DataTransfer." + otherTable.PascalName + ">();");
					sb.AppendLine("			retval.AddRange(eventArgs.ReturnDTOList.Select(x => (" + this.DefaultNamespace + ".DataTransfer." + otherTable.PascalName + ")x));");
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
					sb.AppendLine("			var eventArgs = new " + _currentTable.PascalName + "DTOEventArgs(item);");
					sb.AppendLine("			this.OnMockGet" + relation.PascalRoleName + relation.ChildTable.PascalName + "List(eventArgs);");
					sb.AppendLine("			var retval = new List<" + this.DefaultNamespace + ".DataTransfer." + relation.ChildTable.PascalName + ">();");
					sb.AppendLine("			retval.AddRange(eventArgs.ReturnDTOList.Select(x => (" + this.DefaultNamespace + ".DataTransfer." + relation.ChildTable.PascalName + ")x));");
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

					sb.AppendLine("			var eventArgs = new " + _currentTable.PascalName + "DTOEventArgs(item);");
					sb.AppendLine("			this.OnMockGet" + relation.PascalRoleName + relation.ParentTable.PascalName + "Item(eventArgs);");
					sb.AppendLine("			retval = (" + this.DefaultNamespace + ".DataTransfer." + relation.ParentTable.PascalName + ")eventArgs.ReturnDTOItem;");
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
				sb.AppendLine("			var eventArgs = new " + _currentTable.PascalName + "DTOEventArgs(item);");
				sb.AppendLine("			this.OnMockSelectBy" + column.PascalName + "(eventArgs);");
				sb.AppendLine("			retval.AddRange(eventArgs.ReturnDTOList.Select(x => (" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + ")x));");
				sb.AppendLine("			return retval;");
				sb.AppendLine("		}");
				sb.AppendLine();
			}
		}

		private void AppendMethodPersist()
		{
			if (!_currentTable.Immutable)
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Persists a single item");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public bool Persist(" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + " dto)");
				sb.AppendLine("		{");
				sb.AppendLine("			var eventArgs = new " + _currentTable.PascalName + "PersistEventArgs(dto);");
				sb.AppendLine("			this.OnMock" + _currentTable.PascalName + "Persist(eventArgs);");
				sb.AppendLine("			return eventArgs.ReturnValue;");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Persists a list");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public bool Persist(List<" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + "> list)");
				sb.AppendLine("		{");
				sb.AppendLine("			var eventArgs = new " + _currentTable.PascalName + "PersistEventArgs(list);");
				sb.AppendLine("			this.OnMock" + _currentTable.PascalName + "Persist(eventArgs);");
				sb.AppendLine("			return eventArgs.ReturnValue;");
				sb.AppendLine("		}");
				sb.AppendLine();
			}
		}

		private void AppendMethodDelete()
		{
			if (!_currentTable.Immutable)
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Delete a single item");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public bool Delete(" + this.DefaultNamespace + ".DataTransfer." + _currentTable.PascalName + " dto)");
				sb.AppendLine("		{");
				sb.AppendLine("			var eventArgs = new " + _currentTable.PascalName + "DeleteEventArgs(dto);");
				sb.AppendLine("			this.OnMock" + _currentTable.PascalName + "Delete(eventArgs);");
				sb.AppendLine("			return eventArgs.ReturnValue;");
				sb.AppendLine("		}");
				sb.AppendLine();
			}
		}

		private void GenerateEvents()
		{
			sb.AppendLine("		#region " + _currentTable.PascalName + " Events");
			sb.AppendLine();
			sb.AppendLine("		partial void GetSelectByPrimaryKey();");
			sb.AppendLine();

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The event for the SelectByPrimaryKey method");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public event EventHandler<" + _currentTable.PascalName + "PrimaryKeyEventArgs> Mock" + _currentTable.PascalName + "SelectByPrimaryKey;");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Raises the OnMockRegistrationCodeSelectByPrimaryKey event");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected virtual void OnMock" + _currentTable.PascalName + "SelectByPrimaryKey(" + _currentTable.PascalName + "PrimaryKeyEventArgs e)");
			sb.AppendLine("		{");
			sb.AppendLine("			if (this.Mock" + _currentTable.PascalName + "SelectByPrimaryKey != null)");
			sb.AppendLine("				this.Mock" + _currentTable.PascalName + "SelectByPrimaryKey(this, e);");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The event for the RunSelect method");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public event EventHandler<" + _currentTable.PascalName + "RunSelectEventArgs> Mock" + _currentTable.PascalName + "RunSelect;");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Raises the OnMockRunSelect event");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected virtual void OnMock" + _currentTable.PascalName + "RunSelect(" + _currentTable.PascalName + "RunSelectEventArgs e)");
			sb.AppendLine("		{");
			sb.AppendLine("			if (this.Mock" + _currentTable.PascalName + "RunSelect != null)");
			sb.AppendLine("				this.Mock" + _currentTable.PascalName + "RunSelect(this, e);");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The event for the RunSelectLINQ method");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public event EventHandler<" + _currentTable.PascalName + "RunSelectLINQEventArgs> Mock" + _currentTable.PascalName + "RunSelectLINQ;");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The event for the OnMockRunSelectLINQ method");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected virtual void OnMock" + _currentTable.PascalName + "RunSelectLINQ(" + _currentTable.PascalName + "RunSelectLINQEventArgs e)");
			sb.AppendLine("		{");
			sb.AppendLine("			if (this.Mock" + _currentTable.PascalName + "RunSelectLINQ != null)");
			sb.AppendLine("				this.Mock" + _currentTable.PascalName + "RunSelectLINQ(this, e);");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The event for the RunSelectPaged method");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public event EventHandler<" + _currentTable.PascalName + "RunSelectPagedEventArgs> Mock" + _currentTable.PascalName + "RunSelectPaged;");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The event for the OnMockRunSelectPaged method");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		protected virtual void OnMock" + _currentTable.PascalName + "RunSelectPaged(" + _currentTable.PascalName + "RunSelectPagedEventArgs e)");
			sb.AppendLine("		{");
			sb.AppendLine("			if (this.Mock" + _currentTable.PascalName + "RunSelectPaged != null)");
			sb.AppendLine("				this.Mock" + _currentTable.PascalName + "RunSelectPaged(this, e);");
			sb.AppendLine("		}");
			sb.AppendLine();

			//Get by parent relations
			foreach (var relation in _currentTable.GetRelations().Where(x => x.IsGenerated))
			{
				if (!relation.ParentTable.Generated || !relation.ChildTable.Generated)
				{ 
					//Do Nothing
				}
			
				//Do not render relations from inheritance
				else if (!relation.ChildTable.IsInheritedFrom(relation.ParentTable) && relation.IsOneToOne)
				{
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// The event for the Get" + relation.PascalRoleName + relation.ChildTable.PascalName + " method");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public event EventHandler<" + _currentTable.PascalName + "DTOEventArgs> MockGet" + relation.PascalRoleName + relation.ChildTable.PascalName + "Item;");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// The event for the OnMockGet" + relation.PascalRoleName + relation.ChildTable.PascalName + " method");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		protected virtual void OnMockGet" + relation.PascalRoleName + relation.ChildTable.PascalName + "Item(" + _currentTable.PascalName + "DTOEventArgs e)");
					sb.AppendLine("		{");
					sb.AppendLine("			if (this.MockGet" + relation.PascalRoleName + relation.ChildTable.PascalName + "Item != null)");
					sb.AppendLine("				this.MockGet" + relation.PascalRoleName + relation.ChildTable.PascalName + "Item(this, e);");
					sb.AppendLine("		}");
					sb.AppendLine();
				}
				else if (!relation.ChildTable.IsInheritedFrom(relation.ParentTable) && relation.IsManyToMany)
				{
					var otherTable = relation.GetSecondaryAssociativeTable();

					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// The event for the Get" + relation.PascalRoleName + relation.ChildTable.PascalName + "List method");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public event EventHandler<" + _currentTable.PascalName + "DTOEventArgs> MockGet" + relation.PascalRoleName + otherTable.PascalName + "List;");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// The event for the OnMockGet" + relation.PascalRoleName + otherTable.PascalName + "List method");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		protected virtual void OnMockGet" + relation.PascalRoleName + otherTable.PascalName + "List(" + _currentTable.PascalName + "DTOEventArgs e)");
					sb.AppendLine("		{");
					sb.AppendLine("			if (this.MockGet" + relation.PascalRoleName + otherTable.PascalName + "List != null)");
					sb.AppendLine("				this.MockGet" + relation.PascalRoleName + otherTable.PascalName + "List(this, e);");
					sb.AppendLine("		}");
					sb.AppendLine();
				}
				else if (!relation.ChildTable.IsInheritedFrom(relation.ParentTable))
				{
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// The event for the Get" + relation.PascalRoleName + relation.ChildTable.PascalName + "List method");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public event EventHandler<" + _currentTable.PascalName + "DTOEventArgs> MockGet" + relation.PascalRoleName + relation.ChildTable.PascalName + "List;");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// The event for the OnMockGet" + relation.PascalRoleName + relation.ChildTable.PascalName + "List method");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		protected virtual void OnMockGet" + relation.PascalRoleName + relation.ChildTable.PascalName + "List(" + _currentTable.PascalName + "DTOEventArgs e)");
					sb.AppendLine("		{");
					sb.AppendLine("			if (this.MockGet" + relation.PascalRoleName + relation.ChildTable.PascalName + "List != null)");
					sb.AppendLine("				this.MockGet" + relation.PascalRoleName + relation.ChildTable.PascalName + "List(this, e);");
					sb.AppendLine("		}");
					sb.AppendLine();
				}

			}

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
					sb.AppendLine("		/// The event for the Get" + relation.PascalRoleName + relation.ParentTable.PascalName + "Item method");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public event EventHandler<" + _currentTable.PascalName + "DTOEventArgs> MockGet" + relation.PascalRoleName + relation.ParentTable.PascalName + "Item;");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// The event for the OnMockGet" + relation.PascalRoleName + relation.ParentTable.PascalName + "Item method");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		protected virtual void OnMockGet" + relation.PascalRoleName + relation.ParentTable.PascalName + "Item(" + _currentTable.PascalName + "DTOEventArgs e)");
					sb.AppendLine("		{");
					sb.AppendLine("			if (this.MockGet" + relation.PascalRoleName + relation.ParentTable.PascalName + "Item != null)");
					sb.AppendLine("				this.MockGet" + relation.PascalRoleName + relation.ParentTable.PascalName + "Item(this, e);");
					sb.AppendLine("		}");
					sb.AppendLine();
				}
			}

			foreach (var column in _currentTable.GetColumnsFullHierarchy().Where(x => x.Generated && x.IsSearchable))
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// The event for the SelectBy" + column.PascalName + " method");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public event EventHandler<" + _currentTable.PascalName + "DTOEventArgs> MockSelectBy" + column.PascalName + ";");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// The event for the OnMockSelectBy" + column.PascalName + " method");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		protected virtual void OnMockSelectBy" + column.PascalName + "(" + _currentTable.PascalName + "DTOEventArgs e)");
				sb.AppendLine("		{");
				sb.AppendLine("			if (this.MockSelectBy" + column.PascalName + " != null)");
				sb.AppendLine("				this.MockSelectBy" + column.PascalName + "(this, e);");
				sb.AppendLine("		}");
				sb.AppendLine();
			}

			if (!_currentTable.Immutable)
			{
				//PERSIST
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// The event for the Persist method");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public event EventHandler<" + _currentTable.PascalName + "PersistEventArgs> Mock" + _currentTable.PascalName + "Persist;");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Raises the OnMockPersist event");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		protected virtual void OnMock" + _currentTable.PascalName + "Persist(" + _currentTable.PascalName + "PersistEventArgs e)");
				sb.AppendLine("		{");
				sb.AppendLine("			if (this.Mock" + _currentTable.PascalName + "Persist != null)");
				sb.AppendLine("				this.Mock" + _currentTable.PascalName + "Persist(this, e);");
				sb.AppendLine("		}");
				sb.AppendLine();

				//DELETE
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// The event for the Delete method");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public event EventHandler<" + _currentTable.PascalName + "DeleteEventArgs> Mock" + _currentTable.PascalName + "Delete;");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Raises the OnMockDelete event");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		protected virtual void OnMock" + _currentTable.PascalName + "Delete(" + _currentTable.PascalName + "DeleteEventArgs e)");
				sb.AppendLine("		{");
				sb.AppendLine("			if (this.Mock" + _currentTable.PascalName + "Delete != null)");
				sb.AppendLine("				this.Mock" + _currentTable.PascalName + "Delete(this, e);");
				sb.AppendLine("		}");
				sb.AppendLine();
			}


			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		#endregion

	}
}