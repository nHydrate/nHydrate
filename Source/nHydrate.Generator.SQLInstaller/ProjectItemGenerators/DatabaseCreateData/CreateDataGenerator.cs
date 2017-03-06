#region Copyright (c) 2006-2017 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2017 All Rights reserved                   *
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
using System.Text;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using nHydrate.Generator.ProjectItemGenerators;
using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.DatabaseSchema
{
	[GeneratorItemAttribute("CreateData", typeof(DatabaseProjectGenerator))]
	public class CreateDataGenerator : BaseDbScriptGenerator
	{
		#region Class Members

		private const string PARENT_ITEM_NAME = @"3_GeneratedTablesAndData";

		#endregion

		#region Overrides

		public override int FileCount
		{
			get { return 1; }
		}
		
		public override void Generate()
		{
			var template = new CreateDataTemplate(_model);
			var fullFileName = template.FileName;
			var eventArgs = new ProjectItemGeneratedEventArgs(fullFileName, template.FileContent, ProjectName, PARENT_ITEM_NAME, ProjectItemType.Folder, this, true);
			eventArgs.Properties.Add("BuildAction", 3);
			OnProjectItemGenerated(this, eventArgs);
			var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
			OnGenerationComplete(this, gcEventArgs);
		}

		#endregion

	}
}

