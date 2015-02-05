#region Copyright (c) 2006-2015 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2015 All Rights reserved                   *
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
using nHydrate.Generator.Common.GeneratorFramework;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.ProjectItemGenerators
{
    public abstract class BaseClassTemplate : IFileTemplate
    {
        protected ModelRoot _model;

        public BaseClassTemplate(ModelRoot model)
            //: this()
        {
            _model = model;
        }

        //public BaseClassTemplate()
        //{
        //}

        public virtual string DefaultNamespace
        {
            get { return nHydrateGeneratorProject.DomainProjectName(_model); }
        }

        public virtual string GetLocalNamespace()
        {
            if (string.IsNullOrEmpty(this.LocalNamespaceExtension))
                return this.DefaultNamespace;
            else
                return this.DefaultNamespace + "." + this.LocalNamespaceExtension;
        }

        public abstract string FileContent { get; }
        public abstract string FileName { get; }
        public abstract string LocalNamespaceExtension { get; }

    }
}