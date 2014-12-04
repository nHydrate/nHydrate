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

namespace nHydrate.Generator.EFDAL.Mocks.Generators.Contexts
{
    public class ContextGeneratedTemplate : EFDALMockBaseTemplate
    {
        private readonly StringBuilder sb = new StringBuilder();

        public ContextGeneratedTemplate(ModelRoot model)
            : base(model)
        {
        }

        #region BaseClassTemplate overrides
        public override string FileName
        {
            get { return string.Format("{0}Entities.Generated.cs", _model.ProjectName); }
        }

        public string ParentItemName
        {
            get { return string.Format("{0}Entities.cs", _model.ProjectName); }
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
                AppendTableMapping();
                sb.AppendLine("	#region Entity Context");
                sb.AppendLine();
                sb.AppendLine("	/// <summary />");
                sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
                sb.AppendLine("	public partial class " + _model.ProjectName + "Entities : " + this.DefaultNamespace + ".EFDAL.Interfaces" + ".I" + _model.ProjectName + "Entities, System.IDisposable, nHydrate.EFCore.DataAccess.IContext");
                sb.AppendLine("	{");
                sb.AppendLine();

                sb.AppendLine("		private static Dictionary<string, SequentialIdGenerator> _sequentialIdGeneratorCache = new Dictionary<string, SequentialIdGenerator>();");
                sb.AppendLine("		private static object _seqCacheLock = new object();");
                sb.AppendLine();

                sb.AppendLine("		#region I" + _model.ProjectName + "Entities Members");
                sb.AppendLine();
                AppendAddDelete();
                AppendProperties();
                AppendSaveDispose();
                AppendVersioning();
                AppendGetEntityField();
                AppendSeqId();
                sb.AppendLine("	}");
                sb.AppendLine();
                sb.AppendLine("	#endregion");
                sb.AppendLine();
                sb.AppendLine("}");
                sb.AppendLine();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void AppendSeqId()
        {
            #region SequentialID functionality

            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public static void ResetSequentialGuid(EntityMappingConstants entity, string key, Guid seed)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (string.IsNullOrEmpty(key))");
            sb.AppendLine("				throw new Exception(\"Invalid key\");");
            sb.AppendLine();
            sb.AppendLine("			lock (_seqCacheLock)");
            sb.AppendLine("			{");
            sb.AppendLine("				var k = entity.ToString() + \"|\" + key;");
            sb.AppendLine("				if (!_sequentialIdGeneratorCache.ContainsKey(k))");
            sb.AppendLine("					_sequentialIdGeneratorCache.Add(k, new SequentialIdGenerator(seed));");
            sb.AppendLine("				else");
            sb.AppendLine("					_sequentialIdGeneratorCache[k].LastValue = seed;");
            sb.AppendLine("			}");
            sb.AppendLine();
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary />");
            sb.AppendLine("		public static Guid GetNextSequentialGuid(EntityMappingConstants entity, string key)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (string.IsNullOrEmpty(key))");
            sb.AppendLine("				throw new Exception(\"Invalid key\");");
            sb.AppendLine();
            sb.AppendLine("			lock (_seqCacheLock)");
            sb.AppendLine("			{");
            sb.AppendLine("				var k = entity.ToString() + \"|\" + key;");
            sb.AppendLine("				if (!_sequentialIdGeneratorCache.ContainsKey(k))");
            sb.AppendLine("					ResetSequentialGuid(entity, key, Guid.NewGuid());");
            sb.AppendLine("				return _sequentialIdGeneratorCache[k].NewId();");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();

            #endregion
        }

        private void AppendUsingStatements()
        {
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Data.Objects;");
            sb.AppendLine("using System.Data.Objects.DataClasses;");
            sb.AppendLine("using System.ComponentModel;");
            sb.AppendLine("using System.Runtime.Serialization;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using " + this.DefaultNamespace + ".EFDAL.Interfaces;");
            sb.AppendLine("using " + this.DefaultNamespace + ".EFDAL.Interfaces.Entity;");
            sb.AppendLine();
        }

        private void AppendSaveDispose()
        {
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Persists all changes made to this context since creation or load");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public int SaveChanges()");
            sb.AppendLine("		{");
            sb.AppendLine("			return 1;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Defines a method to release allocated resources.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public void Dispose() { }");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendAddDelete()
        {
            #region Tables AddItem
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && !x.Immutable && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Adds an item of type '" + table.PascalName + "' to the object context.");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"entity\">The entity to add</param>");
                sb.AppendLine("		public virtual void AddItem(" + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + table.PascalName + " entity)");
                sb.AppendLine("		{");
                sb.AppendLine("			this." + table.PascalName + ".AddObject((" + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + table.PascalName + ")entity);");
                sb.AppendLine("		}");
                sb.AppendLine();

                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Adds an item of type '" + table.PascalName + "' to the object context.");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"entity\">The entity to add</param>");
                sb.AppendLine("		void " + this.DefaultNamespace + ".EFDAL.Interfaces.I" + _model.ProjectName + "Entities.AddItem(" + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + table.PascalName + " entity)");
                sb.AppendLine("		{");
                sb.AppendLine("			this." + table.PascalName + ".AddObject(entity);");
                sb.AppendLine("		}");
                sb.AppendLine();
            }
            #endregion

            #region Tables DeleteItem
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && !x.Immutable && x.TypedTable != TypedTableConstants.EnumOnly).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Deletes an item of type '" + table.PascalName + "' to the object context.");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"entity\">The entity to Delete</param>");
                sb.AppendLine("		public virtual void DeleteItem(" + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + table.PascalName + " entity)");
                sb.AppendLine("		{");
                sb.AppendLine("			this." + table.PascalName + ".DeleteObject((" + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + table.PascalName + ")entity);");
                sb.AppendLine("		}");
                sb.AppendLine();

                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Deletes an item of type '" + table.PascalName + "' to the object context.");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		/// <param name=\"entity\">The entity to Delete</param>");
                sb.AppendLine("		void " + this.DefaultNamespace + ".EFDAL.Interfaces.I" + _model.ProjectName + "Entities.DeleteItem(" + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + table.PascalName + " entity)");
                sb.AppendLine("		{");
                sb.AppendLine("			this." + table.PascalName + ".DeleteObject(entity);");
                sb.AppendLine("		}");
                sb.AppendLine();
            }
            #endregion

        }

        private void AppendProperties()
        {
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// The mock set for the '" + table.PascalName + "' entity");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public IObjectSet<" + this.InterfaceProjectNamespace + ".Entity.I" + table.PascalName + "> " + table.PascalName + "");
                sb.AppendLine("		{");
                sb.AppendLine("			get { return _" + table.PascalName + " ?? (_" + table.PascalName + " = new MockObjectSet<" + this.InterfaceProjectNamespace + ".Entity.I" + table.PascalName + ">()); }");
                sb.AppendLine("			set { _" + table.PascalName + " = value as MockObjectSet<" + this.InterfaceProjectNamespace + ".Entity.I" + table.PascalName + ">; }");
                sb.AppendLine("		}");
                sb.AppendLine("		private MockObjectSet<" + this.InterfaceProjectNamespace + ".Entity.I" + table.PascalName + "> _" + table.PascalName + ";");
                sb.AppendLine();
            }
        }

        private void AppendVersioning()
        {
            sb.AppendLine("		#region Version");
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines the version of the model that created this library.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public virtual string Version");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return \"" + _model.Version + "." + _model.GeneratedVersion + "\"; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines the key of the model that created this library.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public virtual string ModelKey");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return \"" + _model.Key + "\"; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines if the API matches the database connection");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <returns></returns>");
            sb.AppendLine("		public virtual bool IsValidConnection()");
            sb.AppendLine("		{");
            sb.AppendLine("			return IsValidConnection(true);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines if the API matches the database connection");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"checkVersion\">Determines if the check also includes the exact version of the model</param>");
            sb.AppendLine("		/// <returns></returns>");
            sb.AppendLine("		public virtual bool IsValidConnection(bool checkVersion)");
            sb.AppendLine("		{");
            sb.AppendLine("			return true;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendTableMapping()
        {
            sb.AppendLine("	#region EntityMappingConstants Enumeration");
            sb.AppendLine();
            sb.AppendLine("	/// <summary>");
            sb.AppendLine("	/// A map for all entity types in this library");
            sb.AppendLine("	/// </summary>");
            sb.AppendLine("	public enum EntityMappingConstants");
            sb.AppendLine("	{");

            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// A mapping for the the " + table.PascalName + " entity");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		" + table.PascalName + ",");
            }

            sb.AppendLine("	}");
            sb.AppendLine();
            sb.AppendLine("	#endregion");
            sb.AppendLine();
        }

        private void AppendGetEntityField()
        {
            sb.AppendLine("		#region GetEntityFromField");
            sb.AppendLine();

            sb.AppendLine("		Enum nHydrate.EFCore.DataAccess.IContext.GetEntityFromField(Enum field)");
            sb.AppendLine("		{");
            sb.AppendLine("			return GetEntityFromField(field);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		object nHydrate.EFCore.DataAccess.IContext.GetMetaData(Enum entity)");
            sb.AppendLine("		{");
            sb.AppendLine("			return GetMetaData((EntityMappingConstants)entity);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		System.Type nHydrate.EFCore.DataAccess.IContext.GetFieldType(Enum field)");
            sb.AppendLine("		{");

            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("			if (field is " + this.DefaultNamespace + ".EFDAL.Interfaces.Entity." + table.PascalName + "FieldNameConstants)");
                sb.AppendLine("			{");
                sb.AppendLine("				switch ((" + this.DefaultNamespace + ".EFDAL.Interfaces.Entity." + table.PascalName + "FieldNameConstants)field)");
                sb.AppendLine("				{");
                foreach (var column in table.GeneratedColumnsFullHierarchy)
                {
                    sb.AppendLine("					case " + this.DefaultNamespace + ".EFDAL.Interfaces.Entity." + table.PascalName + "FieldNameConstants." + column.PascalName + ": return typeof(" + column.GetCodeType() + ");");
                }
                sb.AppendLine("				}");
                sb.AppendLine("			}");
            }
            sb.AppendLine("			return null;");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines the entity from one of its fields");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public static EntityMappingConstants GetEntityFromField(Enum field)");
            sb.AppendLine("		{");
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
            {
                sb.AppendLine("			if (field is " + this.DefaultNamespace + ".EFDAL.Interfaces.Entity." + table.PascalName + "FieldNameConstants) return " + this.GetLocalNamespace() + ".EntityMappingConstants." + table.PascalName + ";");
            }
            sb.AppendLine("			throw new Exception(\"Unknown field type!\");");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the meta data object for an entity");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public static " + this.DefaultNamespace + ".EFDAL.Interfaces.IMetadata GetMetaData(" + this.GetLocalNamespace() + ".EntityMappingConstants table)");
            sb.AppendLine("		{");
            sb.AppendLine("			switch (table)");
            sb.AppendLine("			{");
            foreach (var table in _model.Database.Tables.Where(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly)).OrderBy(x => x.PascalName))
            {
                sb.Append("				case " + this.GetLocalNamespace() + ".EntityMappingConstants." + table.PascalName + ": ");
                //sb.AppendLine("return Activator.CreateInstance(((System.ComponentModel.DataAnnotations.MetadataTypeAttribute)typeof(" + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + table.PascalName + ").GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.MetadataTypeAttribute), true).FirstOrDefault()).MetadataClassType) as " + this.GetLocalNamespace() + ".Interfaces.Entity.Metadata." + table.PascalName + "Metadata;");
                sb.AppendLine("return new " + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.Metadata." + table.PascalName + "Metadata();");
            }
            sb.AppendLine("			}");
            sb.AppendLine("			throw new Exception(\"Entity not found!\");");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        #endregion

    }
}