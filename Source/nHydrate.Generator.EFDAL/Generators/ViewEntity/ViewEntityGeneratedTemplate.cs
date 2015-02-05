#region Copyright (c) 2006-2015 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2015 All Rights reserved                   *
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.EFDAL.Generators.EFCSDL
{
    public class ViewEntityGeneratedTemplate : EFDALBaseTemplate
    {
        private StringBuilder sb = new StringBuilder();
        private readonly CustomView _item;

        public ViewEntityGeneratedTemplate(ModelRoot model, CustomView currentTable)
            : base(model)
        {
            _item = currentTable;
        }

        #region BaseClassTemplate overrides
        public override string FileName
        {
            get { return string.Format("{0}.Generated.cs", _item.PascalName); }
        }

        public string ParentItemName
        {
            get { return string.Format("{0}.cs", _item.PascalName); }
        }

        public override string FileContent
        {
            get
            {
                try
                {
                    sb = new StringBuilder();
                    this.GenerateContent();
                    return sb.ToString();
                }
                catch (Exception ex)
                {
                    throw;
                }
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
                this.AppendEntityClass();
                sb.AppendLine("}");
                sb.AppendLine();
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
            sb.AppendLine("using System.Runtime.Serialization;");
            sb.AppendLine("using System.Data.Objects.DataClasses;");
            sb.AppendLine("using System.Xml.Serialization;");
            sb.AppendLine("using System.ComponentModel;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Data.Objects;");
            sb.AppendLine("using System.Text;");
            sb.AppendLine("using " + this.GetLocalNamespace() + ";");
            sb.AppendLine("using nHydrate.EFCore.DataAccess;");
            sb.AppendLine("using nHydrate.EFCore.EventArgs;");
            sb.AppendLine("using System.Text.RegularExpressions;");
            sb.AppendLine("using System.Linq.Expressions;");
            sb.AppendLine("using System.Data.Linq;");
            sb.AppendLine();
        }

        private void AppendEntityClass()
        {
            var doubleDerivedClassName = _item.PascalName;
            if (_item.GeneratesDoubleDerived)
            {
                doubleDerivedClassName = _item.PascalName + "Base";

                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// The collection to hold '" + _item.PascalName + "' entities");
                if (!string.IsNullOrEmpty(_item.Description))
                    StringHelper.LineBreakCode(sb, _item.Description, "	/// ");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	public partial class " + _item.PascalName + " : " + doubleDerivedClassName);
                sb.AppendLine("	{");
                sb.AppendLine("	}");
                sb.AppendLine();
            }

            sb.AppendLine("	/// <summary>");
            if (_item.GeneratesDoubleDerived)
            {
                sb.AppendLine("	/// Double-derived base class for " + _item.PascalName);
            }
            else
            {
                sb.AppendLine("	/// The collection to hold '" + _item.PascalName + "' entities");
                if (!string.IsNullOrEmpty(_item.Description))
                    StringHelper.LineBreakCode(sb, _item.Description, "	/// ");
            }
            sb.AppendLine("	/// </summary>");
            sb.AppendLine("	[EdmEntityTypeAttribute(NamespaceName = \"" + this.GetLocalNamespace() + ".Entity" + "\", Name = \"" + _item.PascalName + "\")]");
            sb.AppendLine("	[Serializable]");
            sb.AppendLine("	[DataContractAttribute(IsReference = true)]");
            sb.AppendLine("	[nHydrate.EFCore.Attributes.FieldNameConstantsAttribute(typeof(" + this.GetLocalNamespace() + ".Interfaces.Entity." + _item.PascalName + "FieldNameConstants))]");

            if (!string.IsNullOrEmpty(_item.Description))
                sb.AppendLine("	[System.ComponentModel.Description(\"" + StringHelper.ConvertTextToSingleLineCodeString(_item.Description) + "\")]");

            sb.AppendLine("	[System.ComponentModel.ImmutableObject(true)]");
            sb.AppendLine("	[System.CodeDom.Compiler.GeneratedCode(\"nHydrateModelGenerator\", \"" + _model.ModelToolVersion + "\")]");
            sb.AppendLine("	public partial class " + doubleDerivedClassName + " : nHydrate.EFCore.DataAccess.NHEntityObject, nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject, " + GetLocalNamespace() + ".Interfaces.Entity.I" + _item.PascalName + ", System.IEquatable<" + GetLocalNamespace() + ".Interfaces.Entity.I" + _item.PascalName + ">");

            sb.AppendLine("	{");
            //this.AppendedFieldEnum();
            this.AppendConstructors(doubleDerivedClassName);
            this.AppendProperties();
            this.AppendGenerateEvents();
            this.AppendRegionGetMaxLength();
            this.AppendParented();
            this.AppendIsEquivalent();
            this.AppendIEquatable();
            this.AppendRegionGetValue();
            this.AppendRegionGetDatabaseFieldName();
            sb.AppendLine("	}");
            sb.AppendLine();
        }

        private void AppendConstructors(string className)
        {
            var scope = "protected internal";

            sb.AppendLine("		#region Constructors");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Initializes a new instance of the " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + " class");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		" + scope + " " + className + "()");
            sb.AppendLine("		{");
            if (_item.PrimaryKeyColumns.Count == 1 && _item.PrimaryKeyColumns[0].DataType == System.Data.SqlDbType.UniqueIdentifier)
                sb.AppendLine("			this." + _item.PrimaryKeyColumns[0].PascalName + " = Guid.NewGuid();");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendParented()
        {
            sb.AppendLine("		#region IsParented");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines if this object is part of a collection or is detached");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		[System.ComponentModel.Browsable(false)]");

            sb.AppendLine("		public virtual bool IsParented");

            sb.AppendLine("		{");
            sb.AppendLine("			get { return (this.EntityState != System.Data.EntityState.Detached); }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendIsEquivalent()
        {
            sb.AppendLine("		#region IsEquivalent");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines if all of the fields for the specified object exactly matches the current object.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		/// <param name=\"item\">The object to compare</param>");
            sb.AppendLine("		public override bool IsEquivalent(nHydrate.EFCore.DataAccess.INHEntityObject item)");
            sb.AppendLine("		{");
            sb.AppendLine("			return ((System.IEquatable<" + this.GetLocalNamespace() + ".Interfaces.Entity.I" + _item.PascalName + ">)this).Equals(item as " + this.GetLocalNamespace() + ".Interfaces.Entity.I" + _item.PascalName + ");");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendRegionGetValue()
        {
            sb.AppendLine("		#region GetValue");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the value of one of this object's properties.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public object GetValue(" + this.GetLocalNamespace() + ".Interfaces.Entity." + _item.PascalName + "FieldNameConstants field)");
            sb.AppendLine("		{");
            sb.AppendLine("			return GetValue(field, null);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the value of one of this object's properties.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public object GetValue(" + this.GetLocalNamespace() + ".Interfaces.Entity." + _item.PascalName + "FieldNameConstants field, object defaultValue)");
            sb.AppendLine("		{");
            var allColumns = _item.GetColumns().Where(x => x.Generated).ToList();
            foreach (var column in allColumns.OrderBy(x => x.Name))
            {
                var relationParentTable = (CustomView)column.ParentViewRef.Object;
                sb.AppendLine("			if (field == " + this.GetLocalNamespace() + ".Interfaces.Entity." + _item.PascalName + "FieldNameConstants." + column.PascalName + ")");
                if (column.AllowNull)
                    sb.AppendLine("				return ((this." + column.PascalName + " == null) ? defaultValue : this." + column.PascalName + ");");
                else
                    sb.AppendLine("				return this." + column.PascalName + ";");
            }

            //Now do the primary keys
            foreach (var dbColumn in _item.PrimaryKeyColumns.OrderBy(x => x.Name))
            {
                //TODO
            }

            sb.AppendLine("			throw new Exception(\"Field '\" + field.ToString() + \"' not found!\");");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the value of one of this object's properties.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public int GetInteger(" + this.GetLocalNamespace() + ".Interfaces.Entity." + _item.PascalName + "FieldNameConstants field)");
            sb.AppendLine("		{");
            sb.AppendLine("			return this.GetInteger(field, int.MinValue);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the value of one of this object's properties.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public int GetInteger(" + this.GetLocalNamespace() + ".Interfaces.Entity." + _item.PascalName + "FieldNameConstants field, int defaultValue)");
            sb.AppendLine("		{");
            sb.AppendLine("			var o = this.GetValue(field, defaultValue);");
            sb.AppendLine("			if (o is string)");
            sb.AppendLine("			{");
            sb.AppendLine("				int a;");
            sb.AppendLine("				if (int.TryParse((string)o, out a))");
            sb.AppendLine("					return a;");
            sb.AppendLine("				else");
            sb.AppendLine("					throw new System.InvalidCastException();");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				return (int)o;");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the value of one of this object's properties.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public double GetDouble(" + this.GetLocalNamespace() + ".Interfaces.Entity." + _item.PascalName + "FieldNameConstants field)");
            sb.AppendLine("		{");
            sb.AppendLine("			return this.GetDouble(field, double.MinValue);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the value of one of this object's properties.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public double GetDouble(" + this.GetLocalNamespace() + ".Interfaces.Entity." + _item.PascalName + "FieldNameConstants field, double defaultValue)");
            sb.AppendLine("		{");
            sb.AppendLine("			var o = this.GetValue(field, defaultValue);");
            sb.AppendLine("			if (o is string)");
            sb.AppendLine("			{");
            sb.AppendLine("				double a;");
            sb.AppendLine("				if (double.TryParse((string)o, out a))");
            sb.AppendLine("					return a;");
            sb.AppendLine("				else");
            sb.AppendLine("					throw new System.InvalidCastException();");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				return (double)o;");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the value of one of this object's properties.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public System.DateTime GetDateTime(" + this.GetLocalNamespace() + ".Interfaces.Entity." + _item.PascalName + "FieldNameConstants field)");
            sb.AppendLine("		{");
            sb.AppendLine("			return this.GetDateTime(field, System.DateTime.MinValue);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the value of one of this object's properties.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public System.DateTime GetDateTime(" + this.GetLocalNamespace() + ".Interfaces.Entity." + _item.PascalName + "FieldNameConstants field, System.DateTime defaultValue)");
            sb.AppendLine("		{");
            sb.AppendLine("			var o = this.GetValue(field, defaultValue);");
            sb.AppendLine("			if (o is string)");
            sb.AppendLine("			{");
            sb.AppendLine("				System.DateTime a;");
            sb.AppendLine("				if (System.DateTime.TryParse((string)o, out a))");
            sb.AppendLine("					return a;");
            sb.AppendLine("				else");
            sb.AppendLine("					throw new System.InvalidCastException();");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				return (System.DateTime)o;");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the value of one of this object's properties.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public string GetString(" + this.GetLocalNamespace() + ".Interfaces.Entity." + _item.PascalName + "FieldNameConstants field)");
            sb.AppendLine("		{");
            sb.AppendLine("			return this.GetString(field, string.Empty);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the value of one of this object's properties.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public string GetString(" + this.GetLocalNamespace() + ".Interfaces.Entity." + _item.PascalName + "FieldNameConstants field, string defaultValue)");
            sb.AppendLine("		{");
            sb.AppendLine("			var o = this.GetValue(field, defaultValue);");
            sb.AppendLine("			if (o is string)");
            sb.AppendLine("			{");
            sb.AppendLine("				return (string)o;");
            sb.AppendLine("			}");
            sb.AppendLine("			else");
            sb.AppendLine("			{");
            sb.AppendLine("				return o.ToString();");
            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        //private void AppendedFieldEnum()
        //{
        //    IEnumerable<CustomViewColumn> imageColumnList = _item.GetColumns().Where(x => x.DataType == System.Data.SqlDbType.Image).OrderBy(x => x.Name);
        //    if (imageColumnList.Count() != 0)
        //    {
        //        sb.AppendLine("		#region FieldImageConstants Enumeration");
        //        sb.AppendLine();
        //        sb.AppendLine("		/// <summary>");
        //        sb.AppendLine("		/// An enumeration of this object's image type fields");
        //        sb.AppendLine("		/// </summary>");
        //        sb.AppendLine("		public enum FieldImageConstants");
        //        sb.AppendLine("		{");

        //        foreach (var column in imageColumnList.OrderBy(x => x.Name))
        //        {
        //            sb.AppendLine("			/// <summary>");
        //            sb.AppendLine("			/// Field mapping for the image parameter '" + column.PascalName + "' property");
        //            sb.AppendLine("			/// </summary>");
        //            sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the image parameter '" + column.PascalName + "' property\")]");
        //            sb.AppendLine("			" + column.PascalName + ",");
        //        }
        //        sb.AppendLine("		}");
        //        sb.AppendLine();
        //        sb.AppendLine("		#endregion");
        //        sb.AppendLine();
        //    }

        //    sb.AppendLine("		#region FieldNameConstants Enumeration");
        //    sb.AppendLine();
        //    sb.AppendLine("		/// <summary>");
        //    sb.AppendLine("		/// Enumeration to define each property that maps to a database field for the '" + _item.PascalName + "' customView.");
        //    sb.AppendLine("		/// </summary>");
        //    sb.AppendLine("		public enum FieldNameConstants");
        //    sb.AppendLine("		{");

        //    foreach (var column in _item.GeneratedColumns)
        //    {
        //        sb.AppendLine("			/// <summary>");
        //        sb.AppendLine("			/// Field mapping for the '" + column.PascalName + "' property" + (column.PascalName != column.DatabaseName ? " (Database column: " + column.DatabaseName + ")" : string.Empty));
        //        sb.AppendLine("			/// </summary>");

        //        sb.AppendLine("			[System.ComponentModel.ReadOnlyAttribute(true)]");
        //        sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + column.PascalName + "' property\")]");
        //        sb.AppendLine("			" + column.PascalName + ",");
        //    }

        //    sb.AppendLine("		}");
        //    sb.AppendLine("		#endregion");
        //    sb.AppendLine();
        //}

        private void AppendProperties()
        {
            sb.AppendLine("		#region Properties");
            sb.AppendLine();

            //Fake Primary Key
            sb.AppendLine("		[System.ComponentModel.Browsable(false)]");
            sb.AppendLine("		[EdmScalarPropertyAttribute(EntityKeyProperty = true, IsNullable = false)]");
            sb.AppendLine("		[DataMemberAttribute()]");
            sb.AppendLine("		[System.ComponentModel.DisplayName(\"pk\")]");
            sb.AppendLine("		[System.ComponentModel.ReadOnly(true)]");
            sb.AppendLine("		[System.Diagnostics.DebuggerNonUserCode]");
            sb.AppendLine("		private string pk");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return _pk; }");
            sb.AppendLine("			set { _pk = value; }");
            sb.AppendLine("		}");
            sb.AppendLine("		private string _pk;");
            sb.AppendLine();

            foreach (var column in _item.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
            {
#if notimplementedtyet
                if (_currentView.IsColumnRelatedToTypeTable(column))
                {
                    Table typeTable = _currentView.GetRelatedTypeTableByColumn(column);

                    //If this column is a type table column then generate a special property
                    sb.AppendLine("		/// <summary>");
                    if (!string.IsNullOrEmpty(column.Description))
                        StringHelper.LineBreakCode(sb, column.Description, "		/// ");
                    else
                        sb.AppendLine("		/// The property that maps back to the database '" + (column.ParentTableRef.Object as Table).DatabaseName + "." + column.DatabaseName + "' field");
                    sb.AppendLine("		/// </summary>");
                    sb.AppendLine("		/// <remarks>" + column.GetIntellisenseRemarks() + "</remarks>");
                    sb.AppendLine("		[EdmComplexPropertyAttribute()]");
                    //sb.AppendLine("		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]");
                    //sb.AppendLine("		[XmlElement(IsNullable=" + column.AllowNull.ToString().ToLower() + ")]");
                    //sb.AppendLine("		[SoapElement(IsNullable=" + column.AllowNull.ToString().ToLower() + ")]");
                    sb.AppendLine("		[DataMemberAttribute()]");
                    if (column.PrimaryKey)
                        sb.AppendLine("		[System.ComponentModel.DataAnnotations.Key()]");
                    if (column.PrimaryKey || _currentView.Immutable || column.ComputedColumn)
                        sb.AppendLine("		[System.ComponentModel.ReadOnly(true)]");

                    if (!string.IsNullOrEmpty(column.Description))
                        sb.AppendLine("		[System.ComponentModel.Description(\"" + StringHelper.ConvertTextToSingleLineCodeString(column.Description) + "\")]");

                    sb.AppendLine("		[nHydrate.EFCore.Attributes.EntityFieldMetadata(\"" + column.PascalName + "\", " + column.SortOrder + ", " + column.UIVisible.ToString().ToLower() + ", " + column.Length + ", \"" + column.Mask + "\", \"" + column.GetFriendlyName() + "\", \"" + column.Default + "\", " + column.AllowNull.ToString().ToLower() + ", \"" + column.Description + "\", " + column.ComputedColumn.ToString().ToLower() + ", " + column.IsUnique.ToString().ToLower() + ", " + (double.IsNaN(column.Min) ? "double.NaN" : column.Min.ToString()) + ", " + (double.IsNaN(column.Max) ? "double.NaN" : column.Max.ToString()) + ", " + column.PrimaryKey.ToString().ToLower() + ")]");

                    sb.AppendLine("		public virtual " + typeTable.PascalName + "Wrapper " + typeTable.PascalName);
                    sb.AppendLine("		{");
                    sb.AppendLine("			get { return _" + column.CamelName + "; }");
                    
                    //OLD CODE - we tried to hide the setter but serialization hates this
                    //sb.AppendLine("			" + (column.PrimaryKey || _currentTable.Immutable ? "protected " : "") + "set");
                    sb.AppendLine("			set");

                    sb.AppendLine("			{");
                    if (column.PrimaryKey)
                    {
                        sb.AppendLine("				ReportPropertyChanging(\"" + column.PascalName + "\");");
                        //sb.AppendLine("				this.OnPropertyChanging(\"" + column.PascalName + "\");");
                        //sb.AppendLine("				_" + column.CamelName + " = StructuralObject.SetValidValue(value);");
                        sb.AppendLine("				_" + column.CamelName + " = value;");
                        sb.AppendLine("				ReportPropertyChanged(\"" + column.PascalName + "\");");
                        //sb.AppendLine("				this.OnPropertyChanged(\"" + column.PascalName + "\");");
                    }
                    else
                    {
                        sb.AppendLine("				var eventArg = new nHydrate.EFCore.EventArgs.ChangingEventArgs<" + typeTable.PascalName + "Constants>(value, \"" + typeTable.PascalName + "\");");
                        if (_model.EnableCustomChangeEvents)
                            sb.AppendLine("				this.On" + typeTable.PascalName + "Changing(eventArg);");
                        sb.AppendLine("				if (eventArg.Cancel) return;");
                        sb.AppendLine("				ReportPropertyChanging(\"" + typeTable.PascalName + "\");");
                        //sb.AppendLine("				this.OnPropertyChanging(\"" + column.PascalName + "\");");
                        //sb.AppendLine("				_" + column.CamelName + " = StructuralObject.SetValidValue(value);");
                        sb.AppendLine("				_" + column.CamelName + " = eventArg.Value;");
                        sb.AppendLine("				ReportPropertyChanged(\"" + typeTable.PascalName + "\");");
                        //sb.AppendLine("				this.OnPropertyChanged(\"" + column.PascalName + "\");");
                        if (_model.EnableCustomChangeEvents)
                            sb.AppendLine("				this.On" + typeTable.PascalName + "Changed(eventArg);");
                    }
                    sb.AppendLine("			}");
                    sb.AppendLine("		}");
                    sb.AppendLine();
                }
                else
                {
#endif
                sb.AppendLine("		/// <summary>");
                if (!string.IsNullOrEmpty(column.Description))
                    StringHelper.LineBreakCode(sb, column.Description, "		/// ");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		[System.ComponentModel.Browsable(" + column.IsBrowsable.ToString().ToLower() + ")]");
                //sb.AppendLine("		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]");
                //sb.AppendLine("		[XmlElement(IsNullable=" + column.AllowNull.ToString().ToLower() + ")]");
                //sb.AppendLine("		[SoapElement(IsNullable=" + column.AllowNull.ToString().ToLower() + ")]");
                sb.AppendLine("		[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = " + (column.AllowNull ? "true" : "false") + ")]");
                sb.AppendLine("		[DataMemberAttribute()]");
                sb.AppendLine("		[System.ComponentModel.DisplayName(\"" + column.GetFriendlyName() + "\")]");

                sb.AppendLine("		[System.ComponentModel.ReadOnly(true)]");
                sb.AppendLine("		[System.Diagnostics.DebuggerNonUserCode]");

                if (!string.IsNullOrEmpty(column.Description))
                    sb.AppendLine("		[System.ComponentModel.Description(\"" + StringHelper.ConvertTextToSingleLineCodeString(column.Description) + "\")]");

                sb.AppendLine("		public virtual " + column.GetCodeType() + " " + column.PascalName);
                sb.AppendLine("		{");
                sb.AppendLine("			get { return _" + column.CamelName + "; }");

                //OLD CODE - we tried to hide the setter but serialization hates this
                //sb.AppendLine("			" + (column.PrimaryKey || _currentTable.Immutable ? "protected " : "") + "set");
                sb.AppendLine("			set");

                sb.AppendLine("			{");

                //Error Check for field size
                if (ModelHelper.IsTextType(column.DataType))
                {
                    sb.Append("				if ((value != null) && (value.Length > GetMaxLength(" + this.GetLocalNamespace() + ".Interfaces.Entity." + _item.PascalName + "FieldNameConstants." + column.PascalName + ")))");
                    sb.AppendLine(" throw new Exception(string.Format(GlobalValues.ERROR_DATA_TOO_BIG, value, \"" + _item.PascalName + "." + column.PascalName + "\", GetMaxLength(" + this.GetLocalNamespace() + ".Interfaces.Entity." + _item.PascalName + "FieldNameConstants." + column.PascalName + ")));");
                }
                else if (column.DataType == System.Data.SqlDbType.DateTime)
                {
                    //Error check date value
                    sb.AppendLine("				if (" + (column.AllowNull ? "(value != null) && " : "") + "(value < GlobalValues.MIN_DATETIME)) throw new Exception(\"The DateTime value '" + column.PascalName + "' (\" + value" + (column.AllowNull ? ".Value" : "") + ".ToString(\"yyyy-MM-dd HH:mm:ss\") + \") cannot be less than \" + GlobalValues.MIN_DATETIME.ToString());");
                    sb.AppendLine("				if (" + (column.AllowNull ? "(value != null) && " : "") + "(value > GlobalValues.MAX_DATETIME)) throw new Exception(\"The DateTime value '" + column.PascalName + "' (\" + value" + (column.AllowNull ? ".Value" : "") + ".ToString(\"yyyy-MM-dd HH:mm:ss\") + \") cannot be greater than \" + GlobalValues.MAX_DATETIME.ToString());");
                }

#if notdone
                    if (column.PrimaryKey)
                    {
                        sb.AppendLine("				ReportPropertyChanging(\"" + column.PascalName + "\");");
                        //sb.AppendLine("				this.OnPropertyChanging(\"" + column.PascalName + "\");");
                        //sb.AppendLine("				_" + column.CamelName + " = StructuralObject.SetValidValue(value);");
                        sb.AppendLine("				_" + column.CamelName + " = value;");
                        sb.AppendLine("				ReportPropertyChanged(\"" + column.PascalName + "\");");
                        //sb.AppendLine("				this.OnPropertyChanged(\"" + column.PascalName + "\");");
                    }
                    else
                    {
#endif
                sb.AppendLine("				var eventArg = new nHydrate.EFCore.EventArgs.ChangingEventArgs<" + column.GetCodeType() + ">(value, \"" + column.PascalName + "\");");
                if (_model.EnableCustomChangeEvents)
                    sb.AppendLine("				this.On" + column.PascalName + "Changing(eventArg);");
                else
                    sb.AppendLine("				//this.On" + column.PascalName + "Changing(eventArg);");

                sb.AppendLine("				if (eventArg.Cancel) return;");
                sb.AppendLine("				ReportPropertyChanging(\"" + column.PascalName + "\");");
                //sb.AppendLine("				this.OnPropertyChanging(\"" + column.PascalName + "\");");
                //sb.AppendLine("				_" + column.CamelName + " = StructuralObject.SetValidValue(value);");
                sb.AppendLine("				_" + column.CamelName + " = eventArg.Value;");
                sb.AppendLine("				ReportPropertyChanged(\"" + column.PascalName + "\");");
                //sb.AppendLine("				this.OnPropertyChanged(\"" + column.PascalName + "\");");
                if (_model.EnableCustomChangeEvents)
                    sb.AppendLine("				this.On" + column.PascalName + "Changed(eventArg);");
                else
                    sb.AppendLine("				//this.On" + column.PascalName + "Changed(eventArg);");

#if notdone
                    }
#endif
                sb.AppendLine("			}");
                sb.AppendLine("		}");
                sb.AppendLine();
            }
#if notimplementedyet
            }
#endif

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendGenerateEvents()
        {
            sb.AppendLine("		#region Events");
            sb.AppendLine();

            foreach (var column in _item.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
            {
#if notdone
                if (_currentView.IsColumnRelatedToTypeTable(column))
                {
                    Table typeTable = _currentView.GetRelatedTypeTableByColumn(column);

                    //If this column is a type table column then generate a special property
                    sb.AppendLine("		/// <summary>");
                    sb.AppendLine("		/// The internal reference variable for the '" + column.PascalName + "' property");
                    sb.AppendLine("		/// </summary>");
                    sb.AppendLine("		protected " + typeTable.PascalName + "Wrapper _" + column.CamelName + ";");
                    sb.AppendLine();
                    this.AppendPropertyEventDeclarations(column, typeTable.PascalName + "Constants");
                }
                else
                {
#endif
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// The internal reference variable for the '" + column.PascalName + "' property");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		protected " + column.GetCodeType() + " _" + column.CamelName + ";");
                sb.AppendLine();
                this.AppendPropertyEventDeclarations(column, column.GetCodeType());
            }
#if notdone
            }
#endif

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendPropertyEventDeclarations(CustomViewColumn column, string codeType)
        {
#if notdone
            CustomView typetable = _currentView.GetRelatedTypeTableByColumn(column);
            if (typetable != null)
            {
                this.AppendPropertyEventDeclarations(typetable.PascalName, codeType);
            }
            else
            {
#endif
            this.AppendPropertyEventDeclarations(column.PascalName, codeType);
#if notdone
            }
#endif
        }

        private void AppendPropertyEventDeclarations(string columnName, string codeType)
        {
            //Do not support custom events
            if (!_model.EnableCustomChangeEvents) return;

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Occurs when the '" + columnName + "' property value change is a pending.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public event EventHandler<nHydrate.EFCore.EventArgs.ChangingEventArgs<" + codeType + ">> " + columnName + "Changing;");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Raises the On" + columnName + "Changing event.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		protected virtual void On" + columnName + "Changing(nHydrate.EFCore.EventArgs.ChangingEventArgs<" + codeType + "> e)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (this." + columnName + "Changing != null)");
            sb.AppendLine("				this." + columnName + "Changing(this, e);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Occurs when the '" + columnName + "' property value has changed.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public event EventHandler<ChangedEventArgs<" + codeType + ">> " + columnName + "Changed;");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Raises the On" + columnName + "Changed event.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		protected virtual void On" + columnName + "Changed(ChangedEventArgs<" + codeType + "> e)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (this." + columnName + "Changed != null)");
            sb.AppendLine("				this." + columnName + "Changed(this, e);");
            sb.AppendLine("		}");
            sb.AppendLine();
        }

        private void AppendRegionGetMaxLength()
        {
            sb.AppendLine("		#region GetMaxLength");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the maximum size of the field value.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public static int GetMaxLength(" + this.GetLocalNamespace() + ".Interfaces.Entity." + _item.PascalName + "FieldNameConstants field)");
            sb.AppendLine("		{");
            sb.AppendLine("			switch (field)");
            sb.AppendLine("			{");
            foreach (var column in _item.GetColumns().Where(x => x.Generated).OrderBy(x => x.Name))
            {
                sb.AppendLine("				case " + this.GetLocalNamespace() + ".Interfaces.Entity." + _item.PascalName + "FieldNameConstants." + column.PascalName + ":");
                if (_item.GeneratedColumns.Contains(column))
                {
                    //This is an actual column in this table
                    switch (column.DataType)
                    {
                        case System.Data.SqlDbType.Text:
                            sb.AppendLine("					return int.MaxValue;");
                            break;
                        case System.Data.SqlDbType.NText:
                            sb.AppendLine("					return int.MaxValue;");
                            break;
                        case System.Data.SqlDbType.Image:
                            sb.AppendLine("					return int.MaxValue;");
                            break;
                        case System.Data.SqlDbType.Xml:
                            sb.AppendLine("					return int.MaxValue;");
                            break;
                        case System.Data.SqlDbType.Char:
                        case System.Data.SqlDbType.NChar:
                        case System.Data.SqlDbType.NVarChar:
                        case System.Data.SqlDbType.VarChar:
                            if ((column.Length == 0) && (ModelHelper.SupportsMax(column.DataType)))
                                sb.AppendLine("					return int.MaxValue;");
                            else
                                sb.AppendLine("					return " + column.Length + ";");
                            break;
                        default:
                            sb.AppendLine("					return 0;");
                            break;
                    }
                }
            }
            sb.AppendLine("			}");
            sb.AppendLine("			return 0;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		int nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetMaxLength(Enum field)");
            sb.AppendLine("		{");
            sb.AppendLine("			return GetMaxLength((" + this.GetLocalNamespace() + ".Interfaces.Entity." + _item.PascalName + "FieldNameConstants)field);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();

            //GetFieldType
            sb.AppendLine("		#region GetFieldType");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Gets the system type of a field on this object");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public static System.Type GetFieldType(" + this.GetLocalNamespace() + ".Interfaces.Entity." + _item.PascalName + "FieldNameConstants field)");
            sb.AppendLine("		{");
            sb.AppendLine("			var o = new " + _item.PascalName + "();");
            sb.AppendLine("			return ((nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject)o).GetFieldType(field);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		System.Type nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetFieldType(Enum field)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (field.GetType() != typeof(" + this.GetLocalNamespace() + ".Interfaces.Entity." + _item.PascalName + "FieldNameConstants))");
            sb.AppendLine("				throw new Exception(\"The '\" + field.GetType().ReflectedType.ToString() + \".FieldNameConstants' value is not valid. The field parameter must be of type '\" + this.GetType().ToString() + \".FieldNameConstants'.\");");
            sb.AppendLine();
            sb.AppendLine("			switch ((" + this.GetLocalNamespace() + ".Interfaces.Entity." + _item.PascalName + "FieldNameConstants)field)");
            sb.AppendLine("			{");
            foreach (var column in _item.GeneratedColumns)
            {
                sb.AppendLine("				case " + this.GetLocalNamespace() + ".Interfaces.Entity." + _item.PascalName + "FieldNameConstants." + column.PascalName + ": return typeof(" + column.GetCodeType() + ");");
            }
            sb.AppendLine("			}");
            sb.AppendLine("			return null;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();

            //GetFieldNameConstants
            sb.AppendLine("		#region GetFieldNameConstants");
            sb.AppendLine();
            sb.AppendLine("		System.Type nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetFieldNameConstants()");
            sb.AppendLine("		{");
            sb.AppendLine("			return typeof(" + this.GetLocalNamespace() + ".Interfaces.Entity." + _item.PascalName + "FieldNameConstants);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();

            sb.AppendLine("		#region Get/Set Value");
            sb.AppendLine();
            sb.AppendLine("		object nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetValue(System.Enum field)");
            sb.AppendLine("		{");
            sb.AppendLine("			return ((nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject)this).GetValue(field, null);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		object nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.GetValue(System.Enum field, object defaultValue)");
            sb.AppendLine("		{");
            sb.AppendLine("			if (field.GetType() != typeof(" + this.GetLocalNamespace() + ".Interfaces.Entity." + _item.PascalName + "FieldNameConstants))");
            sb.AppendLine("				throw new Exception(\"The '\" + field.GetType().ReflectedType.ToString() + \".FieldNameConstants' value is not valid. The field parameter must be of type '\" + this.GetType().ToString() + \".FieldNameConstants'.\");");
            sb.AppendLine("			return this.GetValue((" + this.GetLocalNamespace() + ".Interfaces.Entity." + _item.PascalName + "FieldNameConstants)field, defaultValue);");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();

            //If this is not derived then add the Primary key stuff
            sb.AppendLine("		#region PrimaryKey");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Hold the primary key for this object");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		protected nHydrate.EFCore.DataAccess.IPrimaryKey _primaryKey = null;");
            sb.AppendLine("		nHydrate.EFCore.DataAccess.IPrimaryKey nHydrate.EFCore.DataAccess.IReadOnlyBusinessObject.PrimaryKey");
            sb.AppendLine("		{");
            sb.AppendLine("			get { return null; }");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void GenerateAuditField(string columnName, string codeType, string description, string propertyScope)
        {
            if (!string.IsNullOrEmpty(description))
            {
                sb.AppendLine("		/// <summary>");
                StringHelper.LineBreakCode(sb, description, "		/// ");
                sb.AppendLine("		/// </summary>");
            }
            sb.AppendLine("		[EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = true)]");
            sb.AppendLine("		[DataMemberAttribute()]");
            sb.AppendLine("		" + propertyScope + " virtual " + codeType + " " + columnName);
            sb.AppendLine("		{");
            sb.AppendLine("			get { return _" + StringHelper.DatabaseNameToCamelCase(columnName) + "; }");
            if (propertyScope == "public")
            {
                //OLD CODE - we tried to hide the setter but serialization hates this
                //sb.AppendLine("			protected internal set");
                sb.AppendLine("			set");
            }
            else
            {
                sb.AppendLine("			set");
            }

            sb.AppendLine("			{");
            sb.AppendLine("				var eventArg = new nHydrate.EFCore.EventArgs.ChangingEventArgs<" + codeType + ">(value, \"" + columnName + "\");");
            if (_model.EnableCustomChangeEvents)
                sb.AppendLine("				On" + columnName + "Changing(eventArg);");
            else
                sb.AppendLine("				//On" + columnName + "Changing(eventArg);");

            sb.AppendLine("				if (eventArg.Cancel) return;");
            sb.AppendLine("				ReportPropertyChanging(\"" + columnName + "\");");
            //sb.AppendLine("				this.OnPropertyChanging(\"" + columnName + "\");");
            sb.AppendLine("				_" + StringHelper.DatabaseNameToCamelCase(columnName) + " = eventArg.Value;");
            sb.AppendLine("				ReportPropertyChanged(\"" + columnName + "\");");
            //sb.AppendLine("				this.OnPropertyChanged(\"" + columnName + "\");");
            if (_model.EnableCustomChangeEvents)
                sb.AppendLine("				On" + columnName + "Changed(eventArg);");
            else
                sb.AppendLine("				//On" + columnName + "Changed(eventArg);");

            sb.AppendLine("			}");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// The internal reference variable for the '" + StringHelper.DatabaseNameToCamelCase(columnName) + "' property");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		protected " + codeType + " _" + StringHelper.DatabaseNameToCamelCase(columnName) + ";");
            sb.AppendLine();
        }

        private void AppendRegionGetDatabaseFieldName()
        {
            sb.AppendLine("		#region GetDatabaseFieldName");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Returns the actual database name of the specified field.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		internal static string GetDatabaseFieldName(" + this.GetLocalNamespace() + ".Interfaces.Entity." + _item.PascalName + "FieldNameConstants field)");
            sb.AppendLine("		{");
            sb.AppendLine("			return GetDatabaseFieldName(field.ToString());");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Returns the actual database name of the specified field.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		internal static string GetDatabaseFieldName(string field)");
            sb.AppendLine("		{");
            sb.AppendLine("			switch (field)");
            sb.AppendLine("			{");
            foreach (var column in _item.GeneratedColumns)
            {
                if (column.Generated)
                    sb.AppendLine("				case \"" + column.PascalName + "\": return \"" + column.Name + "\";");
            }

            sb.AppendLine("			}");
            sb.AppendLine("			return string.Empty;");
            sb.AppendLine("		}");
            sb.AppendLine();
            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        private void AppendIEquatable()
        {
            sb.AppendLine("		#region Equals");
            sb.AppendLine("		bool System.IEquatable<" + this.GetLocalNamespace() + ".Interfaces.Entity.I" + _item.PascalName + ">.Equals(" + this.GetLocalNamespace() + ".Interfaces.Entity.I" + _item.PascalName + " other)");
            sb.AppendLine("		{");
            sb.AppendLine("			return this.Equals(other);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Determines whether the specified object is equal to the current object.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public override bool Equals(object obj)");
            sb.AppendLine("		{");
            sb.AppendLine("			var other = obj as " + this.GetLocalNamespace() + ".Entity." + _item.PascalName + ";");
            sb.AppendLine("			if (other == null) return false;");
            sb.AppendLine("			return (");

            var allColumns = _item.GeneratedColumns.ToList();
            var index = 0;
            foreach (var column in allColumns)
            {
#if notimplementedyet
                Table typeTable = _currentView.GetRelatedTypeTableByColumn(column, true);
                if (typeTable != null)
                {
                    sb.Append("				other." + typeTable.PascalName + " == this." + typeTable.PascalName);
                }
                else
                {
                    sb.Append("				other." + column.PascalName + " == this." + column.PascalName);
                }
#else
                sb.Append("				other." + column.PascalName + " == this." + column.PascalName);
#endif
                if (index < allColumns.Count() - 1) sb.Append(" &&");
                sb.AppendLine();
                index++;
            }

            sb.AppendLine("				);");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		/// <summary>");
            sb.AppendLine("		/// Serves as a hash function for a particular type.");
            sb.AppendLine("		/// </summary>");
            sb.AppendLine("		public override int GetHashCode()");
            sb.AppendLine("		{");
            sb.AppendLine("			return base.GetHashCode();");
            sb.AppendLine("		}");
            sb.AppendLine();

            sb.AppendLine("		#endregion");
            sb.AppendLine();
        }

        #endregion

    }
}