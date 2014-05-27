#region Copyright (c) 2006-2014 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2014 All Rights reserved                   *
//                                                                            *
//                                                                            *
// Permission is hereby granted, free of charge, to any person obtaining a    *
// copy of this software and associated documentation files (the "Software"), *
// to deal in the Software without restriction, including without limitation  *
// the rights to use, copy, modify, merge, publish, distribute, sublicense,   *
// and/or sell copies of the Software, and to permit persons to whom the      *
// Software is furnished to do so, subject to the following conditions:       *
//                                                                            *
// The above copyright notice and this permission notice shall be included    *
// in all copies or substantial portions of the Software.                     *
//                                                                            *
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,            *
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES            *
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  *
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY       *
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,       *
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE          *
// SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                     *
// -------------------------------------------------------------------------- *
#endregion
using System;
using System.Linq;
using System.Text;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.EFDAL.Mocks.Generators.EntityPrimaryKey
{
	class EntityPrimaryKeyTemplate : EFDALMockBaseTemplate
	{
		private readonly StringBuilder sb = new StringBuilder();
		private readonly Table _currentTable;

		#region Constructors
		public EntityPrimaryKeyTemplate(ModelRoot model, Table currentTable)
			: base(model)
		{
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
				nHydrate.Generator.GenerationHelper.AppendFileGeneatedMessageInCode(sb);
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
			foreach (var reference in _currentTable.Relationships.AsEnumerable())
			{
				var relation = reference.Object as Relation;
				if (!relation.IsPrimaryKeyRelation())
				{
					this.BuildNonPrimaryRelations(relation);
				}
			}
			
		}

		#endregion

		private void BuildNonPrimaryRelations(Relation relation)
		{
			var childTable = (Table)relation.ChildTableRef.Object;
			var objectName = _currentTable.PascalName + childTable.PascalName + relation.RoleName + "RelationKey";
			sb.AppendLine("	#region " + objectName);
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// A relation key object for a '" + _currentTable.PascalName + "' => '" + childTable.PascalName + "' relationship.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
			sb.AppendLine("	[Serializable]");
			sb.AppendLine("	public class " + objectName + " : nHydrate.EFCore.DataAccess.IPrimaryKey");
			sb.AppendLine("	{");
			foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
			{
				var column = (Column)columnRelationship.ParentColumnRef.Object;
				sb.AppendLine("		private " + column.GetCodeType() + " m" + column.PascalName + ";");
			}
			sb.AppendLine();

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The constructor for this object which takes the fields that comprise the key for the '" + _currentTable.PascalName + "' view.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + objectName + "(" + RelationKeyParameterList(relation) + ")");
			sb.AppendLine("		{");

			foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
			{
				var column = (Column)columnRelationship.ParentColumnRef.Object;
				sb.AppendLine("			m" + column.PascalName + " = " + column.CamelName + ";");
			}
			sb.AppendLine("		}");
			sb.AppendLine();

			foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
			{
				var column = (Column)columnRelationship.ParentColumnRef.Object;
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// A key for the '" + _currentTable.PascalName + "' view.");
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
				foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
				{
					var column = (Column)columnRelationship.ParentColumnRef.Object;
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
			sb.AppendLine("	/// A strongly-typed primary key object for the '" + _currentTable.PascalName + "' view.");
			sb.AppendLine("	/// </summary>");
			sb.AppendLine("	[Serializable]");
			sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
			sb.AppendLine("	public partial class " + objectName + " : nHydrate.EFCore.DataAccess.IPrimaryKey");
			sb.AppendLine("	{");
			foreach (var dbColumn in _currentTable.PrimaryKeyColumns.OrderBy(x => x.Name))
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// A primary key field of the " + _currentTable.PascalName + " entity");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		protected " + dbColumn.GetCodeType() + " _" + dbColumn.PascalName + ";");
			}
			sb.AppendLine();

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// The constructor for this object which takes the fields that comprise the primary key for the '" + _currentTable.PascalName + "' view.");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public " + objectName + "(" + PrimaryKeyParameterList() + ")");
			sb.AppendLine("		{");

			foreach (var dbColumn in _currentTable.PrimaryKeyColumns.OrderBy(x => x.Name))
			{
				sb.AppendLine("			_" + dbColumn.PascalName + " = " + dbColumn.CamelName + ";");

			}
			sb.AppendLine("		}");
			sb.AppendLine();

			foreach (var dbColumn in _currentTable.PrimaryKeyColumns.OrderBy(x => x.Name))
			{
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// A primary key for the '" + _currentTable.PascalName + "' view.");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + dbColumn.GetCodeType() + " " + dbColumn.PascalName);
				sb.AppendLine("		{");
				sb.AppendLine("			get { return _" + dbColumn.PascalName + "; }");
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
				sb.AppendLine("			var retval = true;");
				foreach (var column in _currentTable.PrimaryKeyColumns.OrderBy(x => x.Name))
				{
					sb.AppendLine("			retval &= (this." + column.PascalName + " == ((" + this.GetLocalNamespace() + ".Entity." + objectName + ")obj)." + column.PascalName + ");");
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
