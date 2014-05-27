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
using System.Collections;
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Common.Util;

namespace nHydrate.Generator.Common.EventArgs
{
	public class ProjectItemGeneratedEventArgs : System.EventArgs
	{
		#region Constructors

		private ProjectItemGeneratedEventArgs()
		{
			this.ParentItemType = ProjectItemType.File;
			this.Properties = new Hashtable();
			this.ProjectItemName = string.Empty;
			this.ProjectItemContent = string.Empty;
			this.ProjectName = string.Empty;
			this.ParentItemName = string.Empty;
			this.ContentType = ProjectItemContentType.String;
			this.Overwrite = true;
			this.FileState = EnvDTEHelper.FileStateConstants.Success;
			this.FullName = string.Empty;
			this.BaseGenerator = null;
			this.RunCustomTool = false;
		}

		public ProjectItemGeneratedEventArgs(string projectItemName, string projectItemContent, string projectName, IProjectItemGenerator baseGenerator, bool overwrite)
			: this()
		{
			this.BaseGenerator = baseGenerator;
			this.ProjectItemName = projectItemName;
			this.ProjectItemContent = projectItemContent;
			this.ProjectName = projectName;
			this.Overwrite = overwrite;
		}

		public ProjectItemGeneratedEventArgs(string projectItemName, string projectItemContent, string projectName, string parentItemName, IProjectItemGenerator baseGenerator, bool overwrite)
			: this()
		{
			this.BaseGenerator = baseGenerator;
			this.ProjectItemName = projectItemName;
			this.ProjectItemContent = projectItemContent;
			this.ProjectName = projectName;
			this.ParentItemName = parentItemName;
			this.Overwrite = overwrite;
		}

		public ProjectItemGeneratedEventArgs(string projectItemName, string projectItemContent, string projectName, string parentItemName, ProjectItemType parentItemType, IProjectItemGenerator baseGenerator, bool overwrite)
			: this()
		{
			this.BaseGenerator = baseGenerator;
			this.ProjectItemName = projectItemName;
			this.ProjectItemContent = projectItemContent;
			this.ProjectName = projectName;
			this.ParentItemName = parentItemName;
			this.ParentItemType = parentItemType;
			this.Overwrite = overwrite;
		}

		#endregion

		#region Property Implementations

		public EnvDTEHelper.FileStateConstants FileState { get; set; }

		public string FullName { get; set; }

		public IProjectItemGenerator BaseGenerator { get; private set; }

		public ProjectItemType ParentItemType { get; private set; }

		public ProjectItemContentType ContentType { get; set; }

		public string ParentItemName { get; private set; }

		public string ProjectName { get; private set; }

		public string ProjectItemName { get; internal set; }

		public string ProjectItemContent { get; private set; }

		public byte[] ProjectItemBinaryContent { get; set; }

		public Hashtable Properties { get; set; }

		public bool Overwrite { get; private set; }

		public bool RunCustomTool { get; set; }

		public string CustomToolName { get; set; }

		#endregion

	}
}

