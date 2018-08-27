#region Copyright (c) 2006-2018 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2018 All Rights reserved                   *
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

namespace nHydrate.Generator.EFDAL.Generators.IncludeTreeLINQ
{
    class IncludeTreeLINQGeneratedTemplate : EFDALBaseTemplate
    {
        private readonly StringBuilder sb = new StringBuilder();

        public IncludeTreeLINQGeneratedTemplate(ModelRoot model)
            : base(model)
        {
        }

        #region BaseClassTemplate overrides
        public override string FileName
        {
            get { return _model.ProjectName + "EntitiesInclude.Generated.cs"; }
        }

        public string ParentItemName
        {
            get { return _model.ProjectName + "EntitiesIncludes.cs"; }
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
                foreach (var table in _model.Database.Tables.Where(x => x.Generated && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.Name))
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
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Data.Linq.Mapping;");
            sb.AppendLine("using " + this.GetLocalNamespace() + ";");
            sb.AppendLine();
        }

        private void AppendTableClass(Table table)
        {
            try
            {
                var allTables = table.GetTableHierarchy();

                sb.AppendLine("	#region " + table.PascalName + "Include");
                sb.AppendLine();

                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// This is a helper object for creating LINQ definitions for context includes on the " + table.PascalName + " collection.");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	[Serializable]");
                sb.AppendLine("	[Table(Name = \"" + table.DatabaseName + "\")]");
                sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
                sb.AppendLine("	public partial class " + table.PascalName + "Include : nHydrate.EFCore.DataAccess.IContextInclude");
                sb.AppendLine("	{");

                //Add child relationships
                foreach (var relation in _model.Database.Relations.FindByParentTable(table, true).Where(x => x.IsGenerated))
                {
                    var parentTable = relation.ParentTableRef.Object as Table;
                    var childTable = relation.ChildTableRef.Object as Table;
                    if (parentTable.Generated && childTable.Generated)
                    {
                        if (childTable.AssociativeTable)
                        {
                            var middleTable = childTable;
                            var relationlist = middleTable.GetRelationsWhereChild();
                            if (relationlist.First() == relation)
                                childTable = (Table)relationlist.Last().ParentTableRef.Object;
                            else
                                childTable = (Table)relationlist.First().ParentTableRef.Object;

                            if (childTable.Generated &&
                                parentTable.Generated &&
                                !childTable.IsInheritedFrom(parentTable) &&
                                (!allTables.Contains(childTable)))
                            {
                                sb.AppendLine("		/// <summary>");
                                sb.AppendLine("		/// This is a mapping of the relationship with the " + childTable.PascalName + " entity. This is a N:M relation with two relationships though an intermediary table. (" + parentTable.PascalName + " -> " + middleTable.PascalName + " -> " + childTable.PascalName + ")");
                                sb.AppendLine("		/// </summary>");
                                //sb.AppendLine("		[Association(ThisKey = \"" + thisKey + "\", OtherKey = \"" + otherKey + "\")]");
                                if (relation.IsOneToOne && relation.AreAllFieldsPK)
                                    sb.AppendLine("		public " + this.GetLocalNamespace() + "." + childTable.PascalName + "Include " + relation.PascalRoleName + childTable.PascalName + " { get; private set; }");
                                else
                                    sb.AppendLine("		public " + this.GetLocalNamespace() + "." + childTable.PascalName + "Include " + relation.PascalRoleName + childTable.PascalName + "List { get; private set; }");
                            }
                        }
                        else
                        {
                            var thisKey = string.Empty;
                            var otherKey = string.Empty;
                            foreach (ColumnRelationship columnRelationship in relation.ColumnRelationships)
                            {
                                thisKey += ((Column)columnRelationship.ParentColumnRef.Object).PascalName + ",";
                                otherKey += ((Column)columnRelationship.ChildColumnRef.Object).PascalName + ",";
                            }
                            if (!string.IsNullOrEmpty(thisKey)) thisKey = thisKey.Substring(0, thisKey.Length - 1);
                            if (!string.IsNullOrEmpty(otherKey)) otherKey = otherKey.Substring(0, otherKey.Length - 1);

                            if (childTable.Generated &&
                                parentTable.Generated &&
                                !childTable.IsInheritedFrom(parentTable) &&
                                (!allTables.Contains(childTable)))
                            {
                                sb.AppendLine("		/// <summary>");
                                sb.AppendLine("		/// This is a mapping of the relationship with the " + childTable.PascalName + " entity." + (relation.PascalRoleName == "" ? "" : " (Role: '" + relation.RoleName + "')"));
                                sb.AppendLine("		/// </summary>");
                                sb.AppendLine("		[Association(ThisKey = \"" + thisKey + "\", OtherKey = \"" + otherKey + "\")]");
                                if (relation.IsOneToOne && relation.AreAllFieldsPK)
                                    sb.AppendLine("		public " + this.GetLocalNamespace() + "." + childTable.PascalName + "Include " + relation.PascalRoleName + childTable.PascalName + " { get; private set; }");
                                else
                                    sb.AppendLine("		public " + this.GetLocalNamespace() + "." + childTable.PascalName + "Include " + relation.PascalRoleName + childTable.PascalName + "List { get; private set; }");

                                sb.AppendLine();
                            }
                        }
                    }
                }

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
                            sb.AppendLine("		public " + this.GetLocalNamespace() + "." + parentTable.PascalName + "Include " + relation.PascalRoleName + parentTable.PascalName + " { get; private set; }");
                            sb.AppendLine();
                        }

                    }
                }

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