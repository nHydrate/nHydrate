#pragma warning disable 0168
using System;
using System.Linq;
using System.Text;
using nHydrate.Generator.Models;
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.Contexts
{
    public class ContextInterfaceGeneratedTemplate : EFCodeFirstNetCoreBaseTemplate
    {
        private readonly StringBuilder sb = new StringBuilder();

        public ContextInterfaceGeneratedTemplate(ModelRoot model)
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
                sb.AppendLine("#pragma warning disable 0168"); //Suppress variable declared not used
                sb.AppendLine("#pragma warning disable 0108"); //Hides inherited member audit fields from IAudit
                this.AppendUsingStatements();
                sb.AppendLine("namespace " + this.GetLocalNamespace());
                sb.AppendLine("{");
                this.AppendTypeTableEnums();
                this.AppendContextClass();
                sb.AppendLine("}");
                sb.AppendLine();
                sb.AppendLine("#pragma warning restore 0168"); //Suppress variable declared not used
                sb.AppendLine("#pragma warning restore 0108"); //Hides inherited member audit fields from IAudit
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
            sb.AppendLine("using System.ComponentModel;");
            sb.AppendLine("using System.Runtime.Serialization;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine($"using {GetLocalNamespace()}.Entity;");
            sb.AppendLine();
        }

        private void AppendContextClass()
        {
            sb.AppendLine("	#region Entity Context");
            sb.AppendLine();
            sb.AppendLine("	/// <summary />");
            sb.AppendLine($"	[System.CodeDom.Compiler.GeneratedCode(\"nHydrate\", \"{_model.ModelToolVersion}\")]");
            sb.AppendLine("	public partial interface I" + _model.ProjectName + "Entities : System.IDisposable");
            sb.AppendLine("	{");
            //NETCORE REMOVED
            //sb.AppendLine("		/// <summary />");
            //sb.AppendLine("		event EventHandler<" + this.GetLocalNamespace() + ".EventArguments.EntityListEventArgs> BeforeSaveAddedEntity;");
            //sb.AppendLine("		/// <summary />");
            //sb.AppendLine("		event EventHandler<" + this.GetLocalNamespace() + ".EventArguments.EntityListEventArgs> BeforeSaveModifiedEntity;");
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		int SaveChanges();");
            sb.AppendLine();

            #region Tables
            foreach (var item in _model.Database.Tables.Where(x => x.Generated && (x.TypedTable != Models.TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		IQueryable<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + "> " + item.PascalName + " { get ; }");
                sb.AppendLine();
            }
            #endregion

            #region Views
            foreach (var item in _model.Database.CustomViews.Where(x => x.Generated).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		IQueryable<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + "> " + item.PascalName + " { get ; }");
                sb.AppendLine();
            }
            #endregion

            #region Stored Proc
            foreach (var item in _model.Database.CustomStoredProcedures.Where(x => x.Generated).OrderBy(x => x.PascalName))
            {
                var paramset = item.GetParameters().Where(x => x.Generated).ToList();
                var paramString = string.Join(", ", paramset.Select(x => (x.IsOutputParameter ? "out " : "") + x.GetCodeType(true) + " " + x.CamelName).ToList());
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		IQueryable<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + "> " + item.PascalName + "(" + paramString + ");");
                sb.AppendLine();
            }
            #endregion

            #region Functions
            foreach (var item in _model.Database.Functions.Where(x => x.Generated && x.IsTable).OrderBy(x => x.PascalName))
            {
                var paramset = item.GetParameters().Where(x => x.Generated).ToList();
                var paramString = string.Join(", ", paramset.Select(x => x.GetCodeType(true) + " " + x.CamelName).ToList());
                sb.AppendLine("		/// <summary />");
                sb.AppendLine("		IQueryable<" + this.GetLocalNamespace() + ".Entity." + item.PascalName + "> " + item.PascalName + "(" + paramString + ");");
                sb.AppendLine();
            }
            #endregion

            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		" + this.GetLocalNamespace() + ".IBusinessObject AddItem(" + this.GetLocalNamespace() + ".IBusinessObject entity);");
            sb.AppendLine();
            sb.AppendLine("		/// <summary />");
            sb.AppendLine($"		void RemoveItem({this.GetLocalNamespace()}.IBusinessObject entity);");
            sb.AppendLine();
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		void ReloadItem(BaseEntity entity);");
            sb.AppendLine();
            //NETCORE REMOVED
            //sb.AppendLine("		/// <summary />");
            //sb.AppendLine("		void DetachItem(BaseEntity entity);");
            //sb.AppendLine();
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		ContextStartup ContextStartup { get; }");
            sb.AppendLine();
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		System.Guid InstanceKey { get; }");
            sb.AppendLine("	}");
            sb.AppendLine();
            sb.AppendLine("	#endregion");
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
                            if (int.TryParse(sort, out var svalue))
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

        #endregion

    }
}