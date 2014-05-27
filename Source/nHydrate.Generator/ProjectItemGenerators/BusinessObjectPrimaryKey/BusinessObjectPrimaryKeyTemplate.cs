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

namespace Widgetsphere.Generator.ProjectItemGenerators.PrimaryKey
{
	class PrimaryKeyTemplate : BaseClassTemplate
	{
		private StringBuilder sb = new StringBuilder();
		private Table _currentTable;

		#region Constructors
		public PrimaryKeyTemplate(ModelRoot model, Table currentTable)
		{
			_model = model;
			_currentTable = currentTable;
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
				return string.Format("{0}PrimaryKey.cs", _currentTable.PascalName);
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
				sb.AppendLine("namespace " + DefaultNamespace + ".Business.Objects");
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
			sb.AppendLine("using Widgetsphere.Core.DataAccess;");
			sb.AppendLine();

		}
	
		public void AppendClass()
		{
			this.BuildPrimaryKeys();
			foreach (Reference reference in _currentTable.Relationships)
			{
				Relation relation = (Relation)reference.Object;
				if (!relation.IsPrimaryKeyRelation())
				{
					this.BuildNonPrimaryRelations(relation);
				}
			}
			
		}

		#endregion

		#region append regions
		#endregion

		#region append member variables
		#endregion

		#region append constructors
		#endregion

		#region append properties
		#endregion

		#region append methods
		#endregion

		#region append operator overloads
		#endregion

		private void BuildNonPrimaryRelations(Relation relation)
		{
			Table childTable = (Table)relation.ChildTableRef.Object;
			string objectName = _currentTable.PascalName + childTable.PascalName + relation.RoleName + "RelationKey";
			sb.AppendLine("	#region " + objectName);
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// A relation key object for a '" + _currentTable.PascalName + "' => '" + childTable.PascalName + "' relationship.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("	[Serializable()]");
			sb.AppendLine("	public class " + objectName + " : IPrimaryKey");
			sb.AppendLine("	{");
			foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
			{
				Column column = (Column)columnRelationship.ParentColumnRef.Object;
				sb.AppendLine("		private " + column.GetCodeType() + " m" + column.PascalName + ";");
			}
			sb.AppendLine();

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The constructor for this object which takes the fields that comprise the key for the '" + _currentTable.PascalName + "' table.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + objectName + "(" + RelationKeyParameterList(relation) + ")");
			sb.AppendLine("		{");

			foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
			{
				Column column = (Column)columnRelationship.ParentColumnRef.Object;
				sb.AppendLine("			m" + column.PascalName + " = " + column.CamelName + ";");
			}
			sb.AppendLine("		}");
			sb.AppendLine();

			foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
			{
				Column column = (Column)columnRelationship.ParentColumnRef.Object;
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// A key for the '" + _currentTable.PascalName + "' table.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + column.GetCodeType() + " " + column.PascalName);
				sb.AppendLine("		{");
				sb.AppendLine("			get { return m" + column.PascalName + "; }");
				sb.AppendLine("		}");
				sb.AppendLine();
			}

			//Equals (Only works for non-composite keys)
			if (_currentTable.PrimaryKeyColumns.Count > 0)
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Returns a value indicating whether the current object is equal to a specified object.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override bool Equals(object obj)");
				sb.AppendLine("		{");
				
				int index = 0;
				sb.AppendLine("			bool retval = true;");
				foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
				{
					Column column = (Column)columnRelationship.ParentColumnRef.Object;
					sb.AppendLine("			retval &= (this." + column.PascalName + " == ((" + objectName + ")obj)." + column.PascalName + ");");
					index++;
				}
				sb.AppendLine("			return retval;");
				sb.AppendLine();

				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Serves as a hash function for this particular type.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override int GetHashCode()");
				sb.AppendLine("		{");
				sb.AppendLine("		return base.GetHashCode();");
				sb.AppendLine("		}");
				sb.AppendLine();
			}
			sb.AppendLine("	}");
			sb.AppendLine();
			sb.AppendLine("	#endregion");
			sb.AppendLine();
		}		

		private void BuildPrimaryKeys()
		{
			string objectName = _currentTable.PascalName + "PrimaryKey";
			sb.AppendLine("	#region " + objectName);
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// A strongly-typed primary key object for the '" + _currentTable.PascalName + "' table.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("	[Serializable()]");
			sb.AppendLine("	public class " + objectName + " : IPrimaryKey");
			sb.AppendLine("	{");
			foreach (Column dbColumn in _currentTable.PrimaryKeyColumns)
			{
				sb.AppendLine("		private " + dbColumn.GetCodeType() + " m" + dbColumn.PascalName + ";");
			}
			sb.AppendLine();

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The constructor for this object which takes the fields that compreise the primary key for the '" + _currentTable.PascalName + "' table.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + objectName + "(" + PrimaryKeyParameterList() + ")");
			sb.AppendLine("		{");

			foreach (Column dbColumn in _currentTable.PrimaryKeyColumns)
			{
				sb.AppendLine("			m" + dbColumn.PascalName + " = " + dbColumn.CamelName + ";");

			}
			sb.AppendLine("		}");
			sb.AppendLine();

			foreach (Column dbColumn in _currentTable.PrimaryKeyColumns)
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// A primary key for the '" + _currentTable.PascalName + "' table.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + dbColumn.GetCodeType() + " " + dbColumn.PascalName);
				sb.AppendLine("		{");
				sb.AppendLine("			get { return m" + dbColumn.PascalName + "; }");
				sb.AppendLine("		}");
				sb.AppendLine();
			}
			
			if (_currentTable.PrimaryKeyColumns.Count > 0)
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Returns a value indicating whether the current object is equal to a specified object.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override bool Equals(object obj)");
				sb.AppendLine("		{");

				int index = 0;
				sb.AppendLine("			bool retval = true;");
				foreach (Column column in _currentTable.PrimaryKeyColumns)
				{
					sb.AppendLine("			retval &= (this." + column.PascalName + " == ((" + objectName + ")obj)." + column.PascalName + ");");
					index++;
				}
				sb.AppendLine("			return retval;");				
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// Serves as a hash function for this particular type.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public override int GetHashCode()");
				sb.AppendLine("		{");
				sb.AppendLine("			return base.GetHashCode();");
				sb.AppendLine("		}");
				sb.AppendLine();
			}
			sb.AppendLine("	}");
			sb.AppendLine();
			sb.AppendLine("	#endregion");
			sb.AppendLine();
		}

		#region string builders

		protected string PrimaryKeyParameterList()
		{
			StringBuilder output = new StringBuilder();
			try
			{
				foreach (Column dc in _currentTable.PrimaryKeyColumns)
				{
					output.Append(dc.GetCodeType() + " ");
					output.Append(dc.CamelName);
					output.Append(", ");
				}
				if (output.Length > 2)
				{
					output.Remove(output.Length - 2, 2);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(_currentTable.DatabaseName + ": cannot get primary key as parameter list", ex);
			}
			return output.ToString();
		}

		protected string RelationKeyParameterList(Relation relation)
		{
			StringBuilder output = new StringBuilder();
			try
			{
				foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
				{
					Column column = (Column)columnRelationship.ParentColumnRef.Object;
					output.Append(column.GetCodeType() + " ");
					output.Append(column.CamelName);
					output.Append(", ");
				}
				if (output.Length > 2)
				{
					output.Remove(output.Length - 2, 2);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(_currentTable.DatabaseName + ": cannot get key as parameter list", ex);
			}
			return output.ToString();
		}

		#endregion

	}
}