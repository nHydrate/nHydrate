#region Copyright (c) 2006-2019 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2019 All Rights reserved                   *
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
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.EFCodeFirst.Generators.LINQ
{
    class LINQGeneratedTemplate : EFCodeFirstBaseTemplate
    {
        private readonly StringBuilder sb = new StringBuilder();

        public LINQGeneratedTemplate(ModelRoot model)
            : base(model)
        {
        }

        #region BaseClassTemplate overrides
        public override string FileName
        {
            get { return _model.ProjectName + "EntitiesQueries.Generated.cs"; }
        }

        public string ParentItemName
        {
            get { return _model.ProjectName + "EntitiesQueries.cs"; }
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
                sb.AppendLine("namespace " + this.GetLocalNamespace());
                sb.AppendLine("{");
                foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.Name))
                {
                    this.AppendTableClass(table);
                }
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
            sb.AppendLine($"using {this.GetLocalNamespace()};");
            sb.AppendLine();
        }

        private void AppendTableClass(Table table)
        {
            try
            {
                sb.AppendLine("	#region " + table.PascalName + "Query");
                sb.AppendLine();

                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// This is a helper object for running LINQ queries on the " + table.PascalName + " collection.");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	[Serializable]");
                if (table.IsTenant)
                    sb.AppendLine("	[Table(Name = \"" + _model.TenantPrefix + "_" + table.DatabaseName + "\")]");
                else
                    sb.AppendLine("	[Table(Name = \"" + table.DatabaseName + "\")]");
                sb.AppendLine($"	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"{_model.ModelToolVersion}\")]");
                sb.AppendLine("	public partial class " + table.PascalName + "Query : IBusinessObjectLINQQuery");
                sb.AppendLine("	{");

                sb.AppendLine("		#region Properties");
                var allTables = table.GetTableHierarchy();

                var columnList = table.GetColumnsFullHierarchy().Where(x => x.Generated).ToList();
                foreach (var c in columnList.OrderBy(x => x.Name))
                {
                    var description = c.Description.Trim();
                    if (!string.IsNullOrEmpty(description)) description += "\r\n";
                    description += "(Maps to the '" + table.DatabaseName + "." + c.DatabaseName + "' database field)";

                    sb.AppendLine("		/// <summary>");
                    StringHelper.LineBreakCode(sb, description, "		/// ");
                    sb.AppendLine("		/// </summary>");
                    sb.Append("		[Column(");
                    sb.Append("Name = \"" + c.DatabaseName + "\", ");
                    sb.Append("DbType = \"" + c.DatabaseTypeRaw + "\", ");
                    sb.Append("CanBeNull = " + c.AllowNull.ToString().ToLower() + ", ");
                    sb.Append("IsPrimaryKey = " + table.PrimaryKeyColumns.Contains(c).ToString().ToLower());
                    sb.AppendLine(")]");
                    sb.AppendLine("		[System.Diagnostics.DebuggerNonUserCode()]");
                    sb.AppendLine("		public virtual " + c.GetCodeType(true) + " " + c.PascalName + " { get; set; }");
                }

                if (table.AllowCreateAudit)
                {
                    sb.AppendLine("		/// <summary>");
                    sb.AppendLine("		/// The date of creation");
                    sb.AppendLine("		/// </summary>");
                    sb.AppendLine("		[Column(Name = \"" + _model.Database.CreatedDateColumnName + "\", DbType = \"DateTime\", CanBeNull = true)]");
                    sb.AppendLine("		[System.Diagnostics.DebuggerNonUserCode()]");
                    sb.AppendLine("		public virtual DateTime? " + _model.Database.CreatedDatePascalName + " { get; set; }");

                    sb.AppendLine("		/// <summary>");
                    sb.AppendLine("		/// The name of the creating entity");
                    sb.AppendLine("		/// </summary>");
                    sb.AppendLine("		[Column(Name = \"" + _model.Database.CreatedByColumnName + "\", DbType = \"VarChar(100)\", CanBeNull = true)]");
                    sb.AppendLine("		[System.Diagnostics.DebuggerNonUserCode()]");
                    sb.AppendLine("		public virtual string " + _model.Database.CreatedByPascalName + " { get; set; }");
                }

                if (table.AllowModifiedAudit)
                {
                    sb.AppendLine("		/// <summary>");
                    sb.AppendLine("		/// The date of last modification");
                    sb.AppendLine("		/// </summary>");
                    sb.AppendLine("		[Column(Name = \"" + _model.Database.ModifiedDateColumnName + "\", DbType = \"DateTime\", CanBeNull = true)]");
                    sb.AppendLine("		public virtual DateTime? " + _model.Database.ModifiedDatePascalName + " { get; set; }");

                    sb.AppendLine("		/// <summary>");
                    sb.AppendLine("		/// The name of the last modifing entity");
                    sb.AppendLine("		/// </summary>");
                    sb.AppendLine("		[Column(Name = \"" + _model.Database.ModifiedByColumnName + "\", DbType = \"VarChar(100)\", CanBeNull = true)]");
                    sb.AppendLine("		[System.Diagnostics.DebuggerNonUserCode()]");
                    sb.AppendLine("		public virtual string " + _model.Database.ModifiedByPascalName + " { get; set; }");
                }

                if (table.AllowTimestamp)
                {
                    sb.AppendLine("		/// <summary>");
                    sb.AppendLine("		/// This is an internal field and is not to be used.");
                    sb.AppendLine("		/// </summary>");
                    sb.AppendLine("		[Column(Name = \"" + _model.Database.TimestampColumnName + "\", DbType = \"Binary\", CanBeNull = false)]");
                    sb.AppendLine("		[System.Diagnostics.DebuggerNonUserCode()]");
                    sb.AppendLine("		public virtual byte[] " + _model.Database.TimestampPascalName + " { get; set; }");
                }

                ////Add child relationships
                //foreach (var relation in _model.Database.Relations.FindByParentTable(table, true).Where(x => x.IsGenerated))
                //{
                //    //Relation relation = (Relation)reference.Object;
                //    var parentTable = (Table)relation.ParentTableRef.Object;
                //    var childTable = (Table)relation.ChildTableRef.Object;
                //    //Column pkColumn = (Column)relation.ColumnRelationships[0].ChildColumnRef.Object;

                //    var thisKey = string.Empty;
                //    var otherKey = string.Empty;
                //    foreach (var columnRelationship in relation.ColumnRelationships.AsEnumerable())
                //    {
                //        thisKey += ((Column)columnRelationship.ParentColumnRef.Object).PascalName + ",";
                //        otherKey += ((Column)columnRelationship.ChildColumnRef.Object).PascalName + ",";
                //    }
                //    if (!string.IsNullOrEmpty(thisKey)) thisKey = thisKey.Substring(0, thisKey.Length - 1);
                //    if (!string.IsNullOrEmpty(otherKey)) otherKey = otherKey.Substring(0, otherKey.Length - 1);

                //    if (childTable.Generated & (childTable.TypedTable != TypedTableConstants.EnumOnly) && (!allTables.Contains(childTable)))
                //    {
                //        sb.AppendLine("		/// <summary>");
                //        sb.AppendLine("		/// This is a mapping of the relationship with the " + childTable.PascalName + " entity." + (relation.PascalRoleName == "" ? "" : " (Role: '" + relation.RoleName + "')"));
                //        sb.AppendLine("		/// </summary>");
                //        sb.AppendLine("		[Association(ThisKey = \"" + thisKey + "\", OtherKey = \"" + otherKey + "\")]");
                //        if (relation.IsOneToOne)
                //            sb.AppendLine("		public " + this.GetLocalNamespace() + "." + childTable.PascalName + "Query " + relation.PascalRoleName + childTable.PascalName + " { get; private set; }");
                //        else
                //            sb.AppendLine("		public " + this.GetLocalNamespace() + "." + childTable.PascalName + "Query " + relation.PascalRoleName + childTable.PascalName + "List { get; private set; }");
                //        sb.AppendLine();
                //    }

                //}

                //Add parent relationships
                foreach (var relation in _model.Database.Relations.FindByChildTable(table, true).Where(x => x.IsGenerated))
                {
                    var parentTable = (Table)relation.ParentTableRef.Object;
                    var childTable = (Table)relation.ChildTableRef.Object;

                    //Do not process self-referencing relationships
                    if (parentTable != table)
                    {
                        var thisKey = string.Empty;
                        var otherKey = string.Empty;
                        foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
                        {
                            thisKey += ((Column)columnRelationship.ChildColumnRef.Object).PascalName + ",";
                            otherKey += ((Column)columnRelationship.ParentColumnRef.Object).PascalName + ",";
                        }
                        if (!string.IsNullOrEmpty(thisKey)) thisKey = thisKey.Substring(0, thisKey.Length - 1);
                        if (!string.IsNullOrEmpty(otherKey)) otherKey = otherKey.Substring(0, otherKey.Length - 1);

                        if (parentTable.Generated && (parentTable.TypedTable != TypedTableConstants.EnumOnly) && (!allTables.Contains(parentTable)))
                        {
                            sb.AppendLine("		/// <summary>");
                            sb.AppendLine("		/// This is a mapping of the relationship with the " + parentTable.PascalName + " entity." + (relation.PascalRoleName == "" ? "" : " (Role: '" + relation.RoleName + "')"));
                            sb.AppendLine("		/// </summary>");
                            sb.AppendLine("		[Association(ThisKey = \"" + thisKey + "\", OtherKey = \"" + otherKey + "\")]");
                            sb.AppendLine("		public " + this.GetLocalNamespace() + "." + parentTable.PascalName + "Query " + relation.PascalRoleName + parentTable.PascalName + " { get; private set; }");
                            sb.AppendLine();
                        }

                    }
                }

                sb.AppendLine();
                sb.AppendLine("		#endregion");
                sb.AppendLine();
                sb.AppendLine("	}");
                sb.AppendLine();

                sb.AppendLine("	#endregion");
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