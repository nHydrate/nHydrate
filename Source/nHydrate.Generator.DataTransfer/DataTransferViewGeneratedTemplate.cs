#region Copyright (c) 2006-2010 Widgetsphere LLC, All Rights Reserved
//--------------------------------------------------------------------- *
//                          Widgetsphere  LLC                           *
//             Copyright (c) 2006-2010 All Rights reserved              *
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
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.Common.Util;
using System.Collections;
using System.Collections.ObjectModel;

namespace Widgetsphere.Generator.DataTransfer
{
	public class DataTransferViewGeneratedTemplate : BaseDataTransferTemplate
	{
		private StringBuilder sb = new StringBuilder();
		private CustomView _currentView;

		public DataTransferViewGeneratedTemplate(ModelRoot model, CustomView currentView)
		{
			_model = model;
			_currentView = currentView;
		}

		#region BaseClassTemplate overrides

		public override string FileName
		{
			get { return string.Format("{0}DTO.Generated.cs", _currentView.PascalName); }
		}

		public string ParentItemName
		{
			get { return string.Format("{0}DTO.cs", _currentView.PascalName); }
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
				ValidationHelper.AppendCopyrightInCode(sb, _model);
				this.AppendUsingStatements();
				sb.AppendLine("namespace " + DefaultNamespace);
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
			sb.AppendLine("using " + _model.CompanyName + "." + _model.ProjectName + ".DataTransfer;");
			sb.AppendLine("using System.Runtime.Serialization;");
			sb.AppendLine();
		}

		private void AppendClass()
		{
			try
			{
				sb.AppendLine("	/// <summary>");
				sb.AppendLine("	/// Object data transfer definition for the '" + _currentView.DatabaseName + "' table");
				if (_currentView.Description != "")
					sb.AppendLine("	/// " + _currentView.Description);
				sb.AppendLine("	/// </summary>");

				sb.AppendLine("	[Serializable()]");
				sb.AppendLine("	[DataContract()]");
				sb.AppendLine("	public partial class " + _currentView.PascalName + "DTO : IDTO" );

				sb.AppendLine("	{");

				sb.AppendLine("		#region Class Members");
				sb.AppendLine();

				List<CustomViewColumn> imageColumnList = _currentView.GetColumnsByType(System.Data.SqlDbType.Image);
				if (imageColumnList.Count != 0)
				{
					sb.AppendLine("		#region FieldImageConstants Enumeration");
					sb.AppendLine();
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// An enumeration of this object's image type fields");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		public enum FieldImageConstants");
					sb.AppendLine("		{");
					foreach (CustomViewColumn column in imageColumnList)
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
				foreach (CustomViewColumn column in _currentView.GetColumns().Where(x => x.Generated))
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
				sb.AppendLine("		/// The default contructor for the " + _currentView.PascalName + " DTO");
				sb.AppendLine("		/// </summary>");
				sb.AppendLine("		public " + _currentView.PascalName + "DTO()");
				sb.AppendLine("		{");
				sb.AppendLine("		}");
				sb.AppendLine();
				sb.AppendLine("		#endregion");
				sb.AppendLine();				
				sb.AppendLine("		#region Object Properties");
				sb.AppendLine();

				foreach (CustomViewColumn column in _currentView.GeneratedColumns)
				{					
					sb.AppendLine("		/// <summary>");
					sb.AppendLine("		/// The " + column.PascalName + " field");
					sb.AppendLine("		/// </summary>");
					sb.AppendLine("		[DataMember()]");
					sb.AppendLine("		public virtual " + column.GetCodeType(true) + " " + column.PascalName + " { get; set; }");
				}

				sb.AppendLine();
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

		#endregion

	}
}