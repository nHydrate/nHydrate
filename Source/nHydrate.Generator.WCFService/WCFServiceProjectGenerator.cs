#region Copyright (c) 2006-2011 Widgetsphere LLC, All Rights Reserved
//--------------------------------------------------------------------- *
//                          Widgetsphere  LLC                           *
//             Copyright (c) 2006-2011 All Rights reserved              *
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
using System.IO;
using Widgetsphere.Generator.Common.GeneratorFramework;
using Widgetsphere.Generator.Common.Util;
using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.ProjectItemGenerators;

namespace Widgetsphere.Generator.WCFService
{
	[GeneratorProjectAttribute(
		"WCF Service",
		"",
		"b8bd6b27-b9f2-4291-82e8-88e1295eef15",
		typeof(WidgetsphereGeneratorProject),
		typeof(WCFServiceProjectGenerator),
		new string[] { 
			"Widgetsphere.Generator.DAL.ProjectItemGenerators.DomainProjectGenerator", 
			"Widgetsphere.Generator.DALProxy.DALProxyProjectGenerator", 
			"Widgetsphere.Generator.DataTransfer.DataTransferProjectGenerator", 
		}
		)]
	public class WCFServiceProjectGenerator : BaseProjectGenerator
	{
		protected override string ProjectTemplate
		{
			get
			{
				//GenerateCompanySpecificFile("wcfservice.csproj");
				//GenerateCompanySpecificFile("wcfservice.vstemplate");
				//return string.Format("{0}wcfservice.vstemplate", ((ModelRoot)_model).CompanyName);
				return "wcfservice.vstemplate";
			}
		}

		public override string LocalNamespaceExtension
		{
			get { return WCFServiceProjectGenerator.NamespaceExtension; }
		}

		private void GenerateCompanySpecificFile(string fileName)
		{
			try
			{
				var defaultProjectTemplate = StringHelper.EnsureDirectorySeperatorAtEnd(AddinAppData.Instance.ExtensionDirectory) + fileName;
				var fileData = string.Empty;
				using (var sr = File.OpenText(defaultProjectTemplate))
				{
					fileData = sr.ReadToEnd();
				}

				var newFileText = fileData.Replace("Acme", _model.CompanyName);
				newFileText = newFileText.Replace("$generatedproject$", this.DefaultNamespace);

				var newFileName = ((ModelRoot)_model).CompanyName + fileName;
				using (var sw = File.CreateText(StringHelper.EnsureDirectorySeperatorAtEnd(AddinAppData.Instance.ExtensionDirectory) + newFileName))
				{
					sw.Write(newFileText);
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		protected override void OnAfterGenerate()
		{
			base.OnAfterGenerate();
			var project = EnvDTEHelper.Instance.GetProject(ProjectName);
			if (project != null)
			{
				var preBuildProperty = project.Properties.Item("PreBuildEvent");
				preBuildProperty.Value = "attrib -r \"$(SolutionDir)Bin\\*.*\"";
				var postBuildProperty = project.Properties.Item("PostBuildEvent");
				postBuildProperty.Value = "copy \"$(TargetDir)$(TargetName).*\" \"$(SolutionDir)Bin\\\"";
			}
		}

		protected override void OnInitialize(IModelObject model)
		{
			WidgetsphereGeneratorProject.AddWidgetsphereCoreToBinFolder();
		}

		public static string NamespaceExtension
		{
			get { return "WCFService"; }
		}

	}

}
