#region Copyright (c) 2006-2019 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2019 All Rights reserved                   *
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
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;
using nHydrate.Generator.ProjectItemGenerators;
using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.PostgresInstaller.ProjectItemGenerators.Functions
{
	[GeneratorItemAttribute("Functions", typeof(DatabaseProjectGenerator))]
	public class FunctionsGenerator : BaseDbScriptGenerator
	{
		#region Properties

		private string ParentItemPath
		{
			get { return @"5_Programmability\Functions\Model"; }
		}

		#endregion

		#region Overrides

		public override int FileCount
		{
			get { return 1; }
		}

		public override void Generate()
		{
			try
			{
				var template = new FunctionsTemplate(_model);
				var fullFileName = template.FileName;
				var eventArgs = new ProjectItemGeneratedEventArgs(fullFileName, template.FileContent, ProjectName, this.ParentItemPath, ProjectItemType.Folder, this, true);
				eventArgs.Properties.Add("BuildAction", 3);
				OnProjectItemGenerated(this, eventArgs);
				var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
				OnGenerationComplete(this, gcEventArgs);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		#endregion

	}
}
