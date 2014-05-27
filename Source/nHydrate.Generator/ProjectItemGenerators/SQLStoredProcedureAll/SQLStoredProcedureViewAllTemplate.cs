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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.Common.Util;

namespace Widgetsphere.Generator.ProjectItemGenerators.SQLStoredProcedureAll
{
	class SQLStoredProcedureViewAllTemplate : BaseDbScriptTemplate
	{
		private StringBuilder sb = new StringBuilder();
		private CustomView _view;

		#region Constructors
		public SQLStoredProcedureViewAllTemplate(ModelRoot model, CustomView view)
		{
			_model = model;
			_view = view;
		}
		#endregion

		#region BaseClassTemplate overrides

		public override string FileContent
		{
			get
			{
				this.GenerateContent();
				return sb.ToString();
			}
		}

		public override string FileName
		{
			get { return string.Format("{0}.sql", _view.PascalName); }
		}

		#endregion

		#region GenerateContent
		private void GenerateContent()
		{
			try
			{
				ISQLGenerate generator = null;

				sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
				sb.AppendLine("--Data Schema For Version " + _model.Version);
				ValidationHelper.AppendCopyrightInSQL(sb, _model);

				generator = new SQLSelectViewByAndTemplate(_model, _view);
				generator.GenerateContent(sb);

				generator = new SQLSelectViewByOrTemplate(_model, _view);
				generator.GenerateContent(sb);

				generator = new SQLSelectViewTemplate(_model, _view);
				generator.GenerateContent(sb);

				generator = new SQLPagedSelectViewTemplate(_model, _view);
				generator.GenerateContent(sb);


			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

	}
}