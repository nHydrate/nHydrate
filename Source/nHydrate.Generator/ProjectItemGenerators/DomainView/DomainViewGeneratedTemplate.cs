#region Copyright (c) 2006-2009 Widgetsphere LLC, All Rights Reserved
//--------------------------------------------------------------------- *
//                          Widgetsphere  LLC                           *
//             Copyright (c) 2006-2009 All Rights reserved              *
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
using System.Collections.Generic;
using System.Text;
using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.Common.Util;

namespace Widgetsphere.Generator.ProjectItemGenerators.DomainView
{
	class DomainViewGeneratedTemplate : BaseClassTemplate
	{

		private StringBuilder sb = new StringBuilder();
		private CustomView _currentView;

		public DomainViewGeneratedTemplate(ModelRoot model, CustomView currentView)
		{
			_model = model;
			_currentView = currentView;
		}

		#region BaseClassTemplate overrides
		public override string FileName
		{
			get
			{
				return string.Format("Domain{0}.Generated.cs", _currentView.PascalName);
			}
		}

		public string ParentItemName
		{
			get
			{
				return string.Format("Domain{0}.cs", _currentView.PascalName);
			}
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

		private void GenerateContent()
		{
			try
			{
				ValidationHelper.AppendCopyrightInCode(sb, _model);
				this.AppendUsingStatements();
				sb.AppendLine("namespace " + DefaultNamespace + ".Domain.Views");
				sb.AppendLine("{");
				this.AppendClass();
				sb.AppendLine("}");
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#region namespace / objects
		public void AppendUsingStatements()
		{
			sb.AppendLine("using System;");
			sb.AppendLine("using System.Data;");
			sb.AppendLine("using System.Xml;");
			sb.AppendLine("using System.Runtime.Serialization;");
			sb.AppendLine("using System.Collections;");
			sb.AppendLine("using Widgetsphere.Core.Exceptions;");
			sb.AppendLine("using Widgetsphere.Core.DataAccess;");
			sb.AppendLine("using " + DefaultNamespace + ".Business;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.Objects;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.Views;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.Rules;");
			sb.AppendLine("using " + DefaultNamespace + ".Business.SelectCommands;");
			sb.AppendLine();
		}

		private void AppendClass()
		{
			sb.AppendLine("		/// <summary>");
			sb.AppendLine("		/// This is an customizable extender for the domain class associated with the '" + _currentView.PascalName + "' object");
			sb.AppendLine("		/// </summary>");
			sb.AppendLine("	[Serializable()]");
			sb.AppendLine("	partial class Domain" + _currentView.PascalName + " : ReadOnlyDomainObjectBase ");
			sb.AppendLine("	{");
			this.AppendTemplate();
			sb.AppendLine("	}");
		}

		#endregion

		#region append regions
		private void AppendRegionRelationshipMethods()
		{
			sb.AppendLine("		#region Relationship Methods");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
		}
		#endregion

		#region append member variables
		public void AppendMemberVariables()
		{
			sb.AppendLine("		private Domain" + _currentView.PascalName + "Collection col" + _currentView.PascalName + "List;");
		}
		#endregion

		#region append constructors
		public void AppendConstructor()
		{
			sb.AppendLine("		Domain " + _currentView.PascalName + "CollectionRules(Domain" + _currentView.PascalName + "Collection in" + _currentView.PascalName + "List)");
			sb.AppendLine("		{");
			sb.AppendLine("			col" + _currentView.PascalName + "List = in" + _currentView.PascalName + "List;");
			sb.AppendLine("			Initialize();");
			sb.AppendLine("		}");
		}
		#endregion

		#region append properties
		#endregion

		#region append methods
		public void AppendInitializeMethod()
		{
			sb.AppendLine("		private void Initialize()");
			sb.AppendLine("		{");
			sb.AppendLine("		}");
		}

		#endregion

		private void AppendTemplate()
		{
			sb.AppendLine("		#region Member Variables");
			sb.AppendLine();
			sb.AppendLine("		internal Domain" + _currentView.PascalName + "Collection ParentCol;");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			sb.AppendLine("		#region Constructor");
			sb.AppendLine();
			sb.AppendLine("		internal Domain" + _currentView.PascalName + "(DataRowBuilder rb) : base(rb) ");
			sb.AppendLine("		{");
			sb.AppendLine("			this.ParentCol = ((Domain" + _currentView.PascalName + "Collection)(base.Table));");
			sb.AppendLine("		}");
			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			sb.AppendLine("		#region Properties");
			sb.AppendLine();

			foreach (Reference reference in _currentView.GeneratedColumns)
			{
				CustomViewColumn dbColumn = (CustomViewColumn)reference.Object;
				sb.AppendLine();
				sb.AppendLine("		internal " + dbColumn.GetCodeType() + " " + dbColumn.PascalName);
				sb.AppendLine("		{");
				sb.AppendLine("			get ");
				sb.AppendLine("			{");
				sb.AppendLine("				try ");
				sb.AppendLine("				{");
				sb.AppendLine("					return ((" + dbColumn.GetCodeType() + ")(base[this.ParentCol." + dbColumn.PascalName + "Column]));");
				sb.AppendLine("				}");
				sb.Append("				catch ");
				if (StringHelper.Match(dbColumn.GetCodeType(), "string", true))
				{

					sb.AppendLine(" (InvalidCastException) ");
					sb.AppendLine("				{");
					sb.AppendLine("					return string.Empty;");
				}
				else
				{
					sb.AppendLine("				(InvalidCastException e) ");
					sb.AppendLine("				{");
					sb.AppendLine("					throw new StrongTypingException(\"Cannot get value because it is DBNull.\", e);");
				}				
				sb.AppendLine("				}");
				sb.AppendLine("			}");
				sb.AppendLine();
				sb.AppendLine("		}");

			}

			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();


			sb.AppendLine("		#region Null Methods");
			sb.AppendLine();

			foreach (Reference reference in _currentView.GeneratedColumns)
			{
				CustomViewColumn dbColumn = (CustomViewColumn)reference.Object;
				if (dbColumn.AllowNull)
				{
					sb.AppendLine();
					sb.AppendLine("		internal bool Is" + dbColumn.PascalName + "Null() ");
					sb.AppendLine("		{");
					sb.AppendLine("			return base.IsNull(this.ParentCol." + dbColumn.PascalName + "Column);");
					sb.AppendLine("		}");
					sb.AppendLine("						");
				}
			}

			sb.AppendLine();
			sb.AppendLine("		#endregion");
			sb.AppendLine();
			AppendRegionRelationshipMethods();
			sb.Append("		internal void Remove()").AppendLine();
			sb.Append("		{").AppendLine();
			sb.Append("			this.ParentCol.Remove" + _currentView.PascalName + "(this);").AppendLine();
			sb.Append("		}").AppendLine();

		}

	}
}