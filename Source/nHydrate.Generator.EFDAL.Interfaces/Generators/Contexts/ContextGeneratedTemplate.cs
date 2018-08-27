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
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.EFDAL.Interfaces.Generators.Contexts
{
    public class ContextGeneratedTemplate : EFDALInterfaceBaseTemplate
    {
        private readonly StringBuilder sb = new StringBuilder();

        public ContextGeneratedTemplate(ModelRoot model)
            : base(model)
        {
        }

        #region BaseClassTemplate overrides
        public override string FileName
        {
            get { return string.Format("I{0}Entities.Generated.cs", _model.ProjectName); }
        }

        public string ParentItemName
        {
            get { return string.Format("I{0}Entities.cs", _model.ProjectName); }
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
                sb.AppendLine("#pragma warning disable 0168"); //Supress variable declared not used
                sb.AppendLine("#pragma warning disable 0108"); //Hides inherited member audit fields from IAudit
                this.AppendUsingStatements();
                sb.AppendLine("namespace " + this.GetLocalNamespace());
                sb.AppendLine("{");
                this.AppendTypeTableEnums();
                this.AppendContextClass();
                this.AppendOtherInterfaces();
                sb.AppendLine("}");
                sb.AppendLine();
                sb.AppendLine("#pragma warning restore 0168"); //Supress variable declared not used
                sb.AppendLine("#pragma warning restore 0108"); //Hides inherited member audit fields from IAudit

                //sb.AppendLine("namespace " + this.GetLocalNamespace() + ".Entity");
                //sb.AppendLine("{");
                //this.AppendTypeTableWrappers();
                //sb.AppendLine("}");
                //sb.AppendLine();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void AppendUsingStatements()
        {
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Data.Objects;");
            sb.AppendLine("using System.ComponentModel;");
            sb.AppendLine("using System.Runtime.Serialization;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using " + GetLocalNamespace() + ".Entity;");
            sb.AppendLine();
        }

        private void AppendContextClass()
        {
            sb.AppendLine("	#region Entity Context");
            sb.AppendLine();
            sb.AppendLine("	/// <summary />");
            sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
            sb.AppendLine("	public partial interface I" + _model.ProjectName + "Entities : System.IDisposable");
            sb.AppendLine("	{");

            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		int SaveChanges();");
            sb.AppendLine();

            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && !x.Security.IsValid() && (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary />");
                if (table.ParentTable != null)
                {
                    sb.AppendLine("		IQueryable<" + this.GetLocalNamespace() + ".Entity.I" + table.PascalName + "> " + table.PascalName + " { get ; }");
                }
                else
                {
                    sb.AppendLine("		IQueryable<" + this.GetLocalNamespace() + ".Entity.I" + table.PascalName + "> " + table.PascalName + " { get ; }");
                }
                sb.AppendLine();
            }

            //Add an strongly-typed extension for "AddItem" method
            sb.AppendLine("		#region AddItem Method");
            sb.AppendLine();

            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && !x.Immutable && (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName)) // && !x.IsTypeTable
            {
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		void AddItem(" + this.GetLocalNamespace() + ".Entity.I" + table.PascalName + " entity);");
                sb.AppendLine();
            }

            sb.AppendLine("		#endregion");
            sb.AppendLine();

            //Add an strongly-typed extension for "DeleteItem" method
            sb.AppendLine("		#region DeleteItem Methods");
            sb.AppendLine();

            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && !x.Immutable && (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName)) // && !x.IsTypeTable
            {
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		void DeleteItem(" + this.GetLocalNamespace() + ".Entity.I" + table.PascalName + " entity);");
                sb.AppendLine();
            }

            sb.AppendLine("		#endregion");
            sb.AppendLine();

            sb.AppendLine("	}");
            sb.AppendLine();

            sb.AppendLine("	#endregion");
            sb.AppendLine();
        }

        private void AppendOtherInterfaces()
        {
            sb.AppendLine("	/// <summary />");
            sb.AppendLine("	public partial interface IMetadata");
            sb.AppendLine("	{");
            sb.AppendLine("	}");
            sb.AppendLine();

            sb.AppendLine("	/// <summary />");
            sb.AppendLine("	public partial interface IDTO");
            sb.AppendLine("	{");
            sb.AppendLine("	}");
            sb.AppendLine();
        }

        private void AppendTypeTableEnums()
        {
            try
            {
                foreach (var table in _model.Database.Tables.Where(x => x.Generated && x.TypedTable != TypedTableConstants.None).OrderBy(x => x.Name))
                {
                    if (table.PrimaryKeyColumns.Count == 1)
                    {
                        var pk = table.PrimaryKeyColumns.First();
                        sb.AppendLine("	#region StaticDataConstants Enumeration for '" + table.PascalName + "' entity");
                        sb.AppendLine("	/// <summary>");
                        sb.AppendLine("	/// Enumeration to define static data items and their ids '" + table.PascalName + "' table.");
                        sb.AppendLine("	/// </summary>");
                        sb.Append("	public enum " + table.PascalName + "Constants");

                        //Non-integer types must explicitly add the type
                        if (pk.DataType != System.Data.SqlDbType.Int)
                            sb.Append(" : " + pk.GetCodeType(false));

                        sb.AppendLine();
                        sb.AppendLine("	{");
                        foreach (RowEntry rowEntry in table.StaticData)
                        {
                            var idValue = rowEntry.GetCodeIdValue(table);
                            var identifier = rowEntry.GetCodeIdentifier(table);
                            var description = rowEntry.GetCodeDescription(table);
                            var raw = rowEntry.GetDataRaw(table);
                            var sort = rowEntry.GetDataSort(table);
                            if (!string.IsNullOrEmpty(description))
                            {
                                sb.AppendLine("		/// <summary>");
                                StringHelper.LineBreakCode(sb, description, "		/// ");
                                sb.AppendLine("		/// </summary>");
                                sb.AppendLine("		[Description(\"" + description + "\")]");
                            }
                            else
                            {
                                sb.AppendLine("		/// <summary>");
                                sb.AppendLine("		/// Enumeration for the '" + identifier + "' item");
                                sb.AppendLine("		/// </summary>");
                            }

                            var key = ValidationHelper.MakeDatabaseIdentifier(identifier.Replace(" ", string.Empty));
                            if ((key.Length > 0) && ("0123456789".Contains(key[0])))
                                key = "_" + key;

                            //If there is a sort value then format as attribute
                            int svalue;
                            if (int.TryParse(sort, out svalue))
                            {
                                sort = ", Order = " + svalue;
                            }
                            else
                            {
                                sort = string.Empty;
                            }

                            sb.AppendLine("		[System.ComponentModel.DataAnnotations.Display(Name = \"" + raw + "\"" + sort + ")]");
                            sb.AppendLine("		" + key + " = " + idValue + ",");
                        }
                        sb.AppendLine("	}");
                        sb.AppendLine("	#endregion");
                        sb.AppendLine();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #region Table Wrapper (Not used)
        //private void AppendTypeTableWrappers()
        //{
        //  if (_model.Database.Tables.Count(x => x.IsTypeTable) == 0)
        //    return;

        //  sb.AppendLine("	#region Type Tables");
        //  sb.AppendLine();

        //  foreach (Table table in _model.Database.Tables.Where(x => x.IsTypeTable))
        //  {
        //    sb.AppendLine("	/// <summary>");
        //    sb.AppendLine("	/// The wrapper class for the type table '" + table.PascalName + "' used to define properties on related entities");
        //    sb.AppendLine("	/// </summary>");
        //    sb.AppendLine("	public partial interface I" + table.PascalName + "Wrapper");
        //    sb.AppendLine("	{");
        //    sb.AppendLine("		/// <summary>");
        //    sb.AppendLine("		/// The numeric value associated with the type");
        //    sb.AppendLine("		/// </summary>");
        //    sb.AppendLine("		int Value { get; set; }");
        //    sb.AppendLine();
        //    sb.AppendLine("		/// <summary>");
        //    sb.AppendLine("		/// The enum value associated with the type");
        //    sb.AppendLine("		/// </summary>");
        //    sb.AppendLine("		Enum EnumValue { get; set; }");
        //    sb.AppendLine();
        //    sb.AppendLine("	}");
        //    sb.AppendLine();
        //  }

        //  sb.AppendLine("	#endregion");
        //  sb.AppendLine();

        //}
        #endregion

        #endregion

    }
}