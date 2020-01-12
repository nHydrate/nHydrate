#region Copyright (c) 2006-2020 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2020 All Rights reserved                   *
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

namespace nHydrate.Generator.EFCodeFirstNetCore.Generators.Contexts
{
    [GeneratorItem("ContextAuditGeneratedGenerator", typeof(ContextAuditExtenderGenerator))]
    public class ContextAuditGeneratedGenerator : EFCodeFirstNetCoreProjectItemGenerator
    {
        #region Class Members

        private const string RELATIVE_OUTPUT_LOCATION = @"\Audit\";

        #endregion

        #region Overrides

        public override int FileCount
        {
            get
            {
                if (_model.Database.Tables.Any(x => x.Generated && !x.AssociativeTable && (x.TypedTable != TypedTableConstants.EnumOnly) && x.AllowAuditTracking))
                    return 1;
                else
                    return 0;
            }
        }

        public override void Generate()
        {
            var template = new ContextAuditGeneratedTemplate(_model);
            var fullParentName = RELATIVE_OUTPUT_LOCATION + template.ParentItemName;

            if (this.FileCount == 0)
            {
                var eventArgs = new ProjectItemDeletedEventArgs(RELATIVE_OUTPUT_LOCATION + template.FileName, ProjectName, this);
                OnProjectItemDeleted(this, eventArgs);
            }
            else
            {
                var eventArgs = new ProjectItemGeneratedEventArgs(template.FileName, template.FileContent, ProjectName, fullParentName, this, true);
                OnProjectItemGenerated(this, eventArgs);
                var gcEventArgs = new ProjectItemGenerationCompleteEventArgs(this);
                OnGenerationComplete(this, gcEventArgs);
            }
        }

        #endregion

    }
}