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
using nHydrate.Generator.Common.EventArgs;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    public delegate void ProjectItemGeneratedEventHandler(object sender, ProjectItemGeneratedEventArgs e);
    public delegate void ProjectItemDeletedEventHandler(object sender, ProjectItemDeletedEventArgs e);
    public delegate void ProjectItemGenerationCompleteEventHandler(object sender, ProjectItemGenerationCompleteEventArgs e);
    public delegate void ProjectItemExistsEventHandler(object sender, ProjectItemExistsEventArgs e);
    public delegate void ProjectItemGeneratedErrorEventHandler(object sender, ProjectItemGeneratedErrorEventArgs e);


    public enum ProjectItemType
    {
        Folder = 0,
        File = 1
    }

    public enum ProjectItemContentType
    {
        String = 0,
        File = 1,
        Binary = 2,
    }

    public interface IProjectItemGenerator
    {
        void Initialize(IModelObject model);
        void Generate();
        int FileCount { get; }
        string DefaultNamespace { get; }
        string LocalNamespaceExtension { get; }
        string GetLocalNamespace();

        event ProjectItemGeneratedEventHandler ProjectItemGenerated;
        event ProjectItemDeletedEventHandler ProjectItemDeleted;
        event ProjectItemGenerationCompleteEventHandler GenerationComplete;
        event ProjectItemExistsEventHandler ProjectItemExists;
        event ProjectItemGeneratedErrorEventHandler ProjectItemGenerationError;
    }

}