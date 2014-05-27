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
using nHydrate.Generator.Common;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.DataTransfer
{
	public class DataTransferObjectSPGeneratedTemplate : BaseDataTransferTemplate
	{
		private readonly StringBuilder sb = new StringBuilder();
		private readonly CustomStoredProcedure _item;

		public DataTransferObjectSPGeneratedTemplate(ModelRoot model, CustomStoredProcedure storedProcedure)
			: base(model)
		{
			_item = storedProcedure;
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
			sb.AppendLine("using System.Xml;");
			sb.AppendLine("using System.ComponentModel;");
			sb.AppendLine("using " + this.GetLocalNamespace() + ";");
			sb.AppendLine("using System.Runtime.Serialization;");
			sb.AppendLine();
		}

		private void AppendClass()
		{
			try
			{
				sb.AppendLine("	/// <summary>");
				sb.AppendLine("	/// Object data transfer definition for the '" + _item.DatabaseName + "' table");
				if (!string.IsNullOrEmpty(_item.Description))
					StringHelper.LineBreakCode(sb, _item.Description, "	/// ");
				sb.AppendLine("	/// </summary>");

				sb.AppendLine("	[Serializable]");
				sb.AppendLine("	[DataContract]");
				sb.AppendLine("	public partial class " + _item.PascalName + " : " + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + _item.PascalName + ", " + this.DefaultNamespace + ".EFDAL.Interfaces.IDTO");
				sb.AppendLine("	{");

				sb.AppendLine("		#region Class Members");
				sb.AppendLine();

				var imageColumnList = _item.GetColumnsByType(System.Data.SqlDbType.Image);
				if (imageColumnList.Count() != 0)
				{
					sb.AppendLine("		#region FieldImageConstants Enumeration");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// An enumeration of this object's image type fields");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public enum FieldImageConstants");
					sb.AppendLine("		{");
					foreach (var column in imageColumnList.OrderBy(x => x.Name))
					{
						sb.AppendLine("			 /// <summary>");
						sb.AppendLine("			 /// Field mapping for the image column '" + column.PascalName + "' property");
						sb.AppendLine("			 /// </summary>");
						sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the image column '" + column.PascalName + "' property\")]");
						sb.AppendLine("			" + column.PascalName + ",");
					}
					sb.AppendLine("		}");
					sb.AppendLine();
					sb.AppendLine("		#endregion");
					sb.AppendLine();
				}

				sb.AppendLine("		#region FieldNameConstants Enumeration");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// An enumeration of this object's fields");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public enum FieldNameConstants");
				sb.AppendLine("		{");
				foreach (var column in _item.GeneratedColumns)
				{
					sb.AppendLine("			 /// <summary>");
					sb.AppendLine("			 /// Field mapping for the '" + column.PascalName + "' property");
					sb.AppendLine("			 /// </summary>");
					sb.AppendLine("			[System.ComponentModel.Description(\"Field mapping for the '" + column.PascalName + "' property\")]");
					sb.AppendLine("			" + column.PascalName + ",");
				}

				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();

				sb.AppendLine("		#region Constructors");
				sb.AppendLine();
				sb.AppendLine("		/// <summary>");
				sb.AppendLine("		/// The default contructor for the " + _item.PascalName + " DTO");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + _item.PascalName + "()");
				sb.AppendLine("		{");


				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();
				sb.AppendLine("		#region Object Properties");
				sb.AppendLine();

				foreach (var column in _item.GeneratedColumns)
				{
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// The " + column.PascalName + " field");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		[DataMember]");
					sb.AppendLine("		public virtual " + column.GetCodeType(true) + " " + column.PascalName + " { get; set; }");
					sb.AppendLine();
				}

				this.AppendClone();

				sb.AppendLine("		#endregion");
				sb.AppendLine();

				sb.AppendLine("	}");
				sb.AppendLine();
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private void AppendClone()
		{
			var modifieraux = "virtual";
			sb.AppendLine("		#region Clone");
			sb.AppendLine();
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a shallow copy of this object of all simple properties");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		/// <returns></returns>");
			sb.AppendLine("		public " + modifieraux + " object Clone()");
			sb.AppendLine("		{");
			sb.AppendLine("			var newItem = new " + _item.PascalName + "();");
			foreach (var column in _item.GeneratedColumns.OrderBy(x => x.Name))
			{
				sb.AppendLine("			newItem." + column.PascalName + " = this." + column.PascalName + ";");
			}
			sb.AppendLine("			return newItem;");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// Creates a shallow copy of this object of all simple properties");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("		public static " + _item.PascalName + " Clone(" + this.DefaultNamespace + ".EFDAL.Interfaces.Entity.I" + _item.PascalName + " item)");
			sb.AppendLine("		{");
			sb.AppendLine("			var newItem = new " + _item.PascalName + "();");
			foreach (var column in _item.GeneratedColumns.OrderBy(x => x.Name))
			{
				sb.AppendLine("			newItem." + column.PascalName + " = item." + column.PascalName + ";");
			}
			sb.AppendLine("			return newItem;");
			sb.AppendLine("		}");
			sb.AppendLine();

			sb.AppendLine("		#endregion");
			sb.AppendLine();
		}

		#endregion

	}
}
