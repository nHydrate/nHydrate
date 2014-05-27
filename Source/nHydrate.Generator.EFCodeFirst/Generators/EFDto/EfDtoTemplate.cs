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
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.Common.Util;
using System.Collections;
using System.Collections.ObjectModel;
using Widgetsphere.Generator.EFDAL;
using System.IO;
using System.Xml.Linq;
using System.Data.Entity.Design;
using System.Data.Metadata.Edm;
using System.Xml;
using System.Data.Mapping;
using ConceptualEdmGen;
using Widgetsphere.Generator.EFDAL.Generators.EFCSDL;

namespace Widgetsphere.Generator.EFDAL.Generators.EFDto
{
	public class EfDtoTemplate : EFDALBaseTemplate
	{
		public EfDtoTemplate(ModelRoot model)
		{
			_model = model;
		}

		#region BaseClassTemplate overrides

		public override string FileName
		{
			get { return ""; }
		}

		public override string FileContent
		{
			get
			{
				try
				{
					this.GenerateContent();
					return "";
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
			GenerateCode();
		}

		private void GenerateCode()
		{
			CSDLTemplate csdl = new CSDLTemplate(_model);
			XDocument document = XDocument.Parse(csdl.FileContent);
			XElement c = EdmGen2.GetCsdlFromEdmx(document, EntityFrameworkVersions.Version2);

			// generate code
			StringWriter sw = new StringWriter();
			EntityCodeGenerator codeGen = new EntityCodeGenerator(LanguageOption.GenerateCSharpCode);
			IList<EdmSchemaError> errors = codeGen.GenerateCode(c.CreateReader(), sw);
			if (errors.Count != 0)
			{
				//throw new Exception("The Entity Framework generation failed. Please email feedback@nhydrate.org.");
				StringBuilder sb = new StringBuilder();
				foreach (EdmSchemaError error in errors)
				{
				  sb.AppendFormat("error.Column:{0} error.ErrorCode:{1} error.Line:{2} error.Message:{3} error.SchemaLocation:{4} error.SchemaName:{5} error.Severity:{6} error.StackTrace: {7}", error.Column, error.ErrorCode, error.Line, error.Message, error.SchemaLocation, error.SchemaName, error.Severity, error.StackTrace).AppendLine();
				}
				System.Diagnostics.Debug.WriteLine(sb.ToString());
			}

		}

		#endregion
	}
}