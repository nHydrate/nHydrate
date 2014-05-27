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
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Widgetsphere.Generator.Common.GeneratorFramework;
using Widgetsphere.Generator.Models;
using Widgetsphere.Generator.ProjectItemGenerators;
using System.IO;

namespace Widgetsphere.Generator.ProjectItemGenerators.DataService.DataServiceObjectListPages
{
	[GeneratorItemAttribute("DataServiceJQueryGenerator", typeof(DataTransferServiceProjectGenerator))]
	class DataServiceJQueryGenerator : BaseGenerator
	{
		#region Class Members

		private const string RELATIVE_OUTPUT_LOCATION = @"\scripts\";

		#endregion

    #region Overrides

    public override int FileCount
    {
      get { return _model.Database.Tables.Count; }
    }

		public override void Generate()
		{
			string[] fileList = Directory.GetFiles(Path.Combine(AddinAppData.Instance.ExtensionDirectory, "jquery"), "*.js", SearchOption.AllDirectories);

			foreach (string fileName in fileList)
			{
				string tmpFileName = fileName.Replace(AddinAppData.Instance.ExtensionDirectory, "");
				string fullFileName = RELATIVE_OUTPUT_LOCATION + tmpFileName;
				ProjectItemGeneratedEventArgs eventArgs = new ProjectItemGeneratedEventArgs(fullFileName, fileName, ProjectName, this, true);
				eventArgs.ContentType = ProjectItemContentType.File;
				OnProjectItemGenerated(this, eventArgs);
				ProjectItemGenerationCompleteEventArgs gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
				OnGenerationComplete(this, gcEventArgs);
			}
		}

		protected override string ProjectName
		{
			get
			{
				ModelRoot modelRoot = ((ModelRoot)_model);
				return string.Format("{1}.{0}.DataService", modelRoot.ProjectName, modelRoot.CompanyName);
			}
		}

    #endregion

  }
}
