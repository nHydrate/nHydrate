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
using System.Text;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.EFDAL.Generators.EFCSDL.EntityPrimaryKey
{
	class EntityPrimaryKeyTemplate : EFDALBaseTemplate
	{
		private readonly StringBuilder sb = new StringBuilder();
		private readonly Table _currentTable;

		#region Constructors
		public EntityPrimaryKeyTemplate(ModelRoot model, Table currentTable)
		{
			_model = model;
			_currentTable = currentTable;
		}
		#endregion

		#region BaseClassTemplate overrides
		public override string FileName
		{
			get { return string.Format("{0}PrimaryKey.Generated.cs", _currentTable.PascalName); }
		}

		public string ParentItemName
		{
			get { return string.Format("{0}PrimaryKey.cs", _currentTable.PascalName); }
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
				nHydrate.Generator.GenerationHelper.AppendCopyrightInCode(sb, _model);
				this.AppendUsingStatements();
				sb.AppendLine("namespace " + this.GetLocalNamespace() + ".Entity");
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
			sb.AppendLine("using nHydrate.EFCore.DataAccess;");
			sb.AppendLine();

		}

		public void AppendClass()
		{
			this.BuildPrimaryKeys();
			foreach (var reference in _currentTable.Relationships.ToList())
			{
				var relation = reference.Object as Relation;
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
			var childTable = relation.ChildTableRef.Object as Table;
			var objectName = _currentTable.PascalName + childTable.PascalName + relation.RoleName + "RelationKey";
			sb.AppendLine("	#region " + objectName);
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// A relation key object for a '" + _currentTable.PascalName + "' => '" + childTable.PascalName + "' relationship.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("	[Serializable()]");
			sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
			sb.AppendLine("	public class " + objectName + " : nHydrate.EFCore.DataAccess.IPrimaryKey");
			sb.AppendLine("	{");
			foreach (var columnRelationship in relation.ColumnRelationships.ToList())
			{
				var column = columnRelationship.ParentColumnRef.Object as Column;
				sb.AppendLine("		private " + column.GetCodeType() + " m" + column.PascalName + ";");
			}
			sb.AppendLine();

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The constructor for this object which takes the fields that comprise the key for the '" + _currentTable.PascalName + "' table.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + objectName + "(" + RelationKeyParameterList(relation) + ")");
			sb.AppendLine("		{");

			foreach (var columnRelationship in relation.ColumnRelationships.ToList())
			{
				var column = columnRelationship.ParentColumnRef.Object as Column;
				sb.AppendLine("			m" + column.PascalName + " = " + column.CamelName + ";");
			}
			sb.AppendLine("		}");
			sb.AppendLine();

			foreach (var columnRelationship in relation.ColumnRelationships.ToList())
			{
				var column = columnRelationship.ParentColumnRef.Object as Column;
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
				sb.AppendLine("			if (obj == null) return false;");
				sb.AppendLine("			if (obj.GetType() == this.GetType())");
				sb.AppendLine("			{");
				var index = 0;
				sb.AppendLine("				var retval = true;");
				foreach (var columnRelationship in relation.ColumnRelationships.ToList())
				{
					var column = columnRelationship.ParentColumnRef.Object as Column;
					sb.AppendLine("				retval &= (this." + column.PascalName + " == ((" + this.GetLocalNamespace() + ".Entity." + objectName + ")obj)." + column.PascalName + ");");
					index++;
				}
				sb.AppendLine("				return retval;");
				sb.AppendLine("			}");
				sb.AppendLine("			return false;");
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
			var objectName = _currentTable.PascalName + "PrimaryKey";
			sb.AppendLine("	#region " + objectName);
			sb.AppendLine();
			sb.AppendLine("	/// <summary>");
			sb.AppendLine("	/// A strongly-typed primary key object for the '" + _currentTable.PascalName + "' table.");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	[Serializable()]");
			sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
			sb.AppendLine("	public partial class " + objectName + " : nHydrate.EFCore.DataAccess.IPrimaryKey");
			sb.AppendLine("	{");
			foreach (var column in _currentTable.PrimaryKeyColumns.OrderBy(x => x.Name))
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// A primary key field of the " + _currentTable.PascalName + " entity");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		protected " + column.GetCodeType() + " _" + column.CamelName + ";");
			}
			sb.AppendLine();

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The constructor for this object which takes the fields that comprise the primary key for the '" + _currentTable.PascalName + "' table.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + objectName + "(" + PrimaryKeyParameterList() + ")");
			sb.AppendLine("		{");

			foreach (var column in _currentTable.PrimaryKeyColumns.OrderBy(x => x.Name))
			{
				sb.AppendLine("			_" + column.CamelName + " = " + column.CamelName + ";");

			}
			sb.AppendLine("		}");
			sb.AppendLine();

			foreach (var column in _currentTable.PrimaryKeyColumns.OrderBy(x => x.Name))
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// A primary key for the '" + _currentTable.PascalName + "' table.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + column.GetCodeType() + " " + column.PascalName);
				sb.AppendLine("		{");
				sb.AppendLine("			get { return _" + column.CamelName + "; }");
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
				sb.AppendLine("			if (obj == null) return false;");
				sb.AppendLine("			if (obj.GetType() == this.GetType())");
				sb.AppendLine("			{");

				var index = 0;
				sb.AppendLine("				var retval = true;");
				foreach (var column in _currentTable.PrimaryKeyColumns.OrderBy(x => x.Name))
				{
					sb.AppendLine("				retval &= (this." + column.PascalName + " == ((" + this.GetLocalNamespace() + ".Entity." + objectName + ")obj)." + column.PascalName + ");");
					index++;
				}
				sb.AppendLine("				return retval;");
				sb.AppendLine("			}");
				sb.AppendLine("			return false;");
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
			var output = new StringBuilder();
			try
			{
				foreach (var dc in _currentTable.PrimaryKeyColumns.OrderBy(x => x.Name))
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
			var output = new StringBuilder();
			try
			{
				foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
				{
					var column = (Column)columnRelationship.ParentColumnRef.Object;
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