#region Copyright (c) 2006-2018 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2018 All Rights reserved                   *
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
    public class ProjectItemDeletedEventArgs : System.EventArgs
    {
        #region Constructors

        private ProjectItemDeletedEventArgs()
        {
        }

        public ProjectItemDeletedEventArgs(string projectItemName, string projectName, IProjectItemGenerator baseGenerator)
            : this()
        {
            this.BaseGenerator = baseGenerator;
            this.ProjectItemName = projectItemName;
            this.ProjectName = projectName;
        }

        #endregion

        #region Property Implementations
        public bool DeleteFile { get; set; } = false;

        public EnvDTEHelper.FileStateConstants FileState { get; set; } = EnvDTEHelper.FileStateConstants.Success;

        public string FullName { get; set; } = string.Empty;

        public IProjectItemGenerator BaseGenerator { get; private set; } = null;

        public ProjectItemType ParentItemType { get; private set; } = ProjectItemType.File;

        public ProjectItemContentType ContentType { get; set; } = ProjectItemContentType.String;

        public string ParentItemName { get; private set; } = string.Empty;

        public string ProjectName { get; private set; } = string.Empty;

        public string ProjectItemName { get; private set; } = string.Empty;

        public Hashtable Properties { get; set; } = new Hashtable();

        #endregion

    }
}