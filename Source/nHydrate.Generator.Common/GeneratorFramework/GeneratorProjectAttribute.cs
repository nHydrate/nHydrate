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
using System;

namespace nHydrate.Generator.Common.GeneratorFramework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class GeneratorProjectAttribute : GeneratorItemAttribute
    {
        protected Type _currentType;
        protected string[] _dependencyList = new string[0];
        protected string[] _exclusionList = new string[0];

        public GeneratorProjectAttribute(string name, string description, string generatorGuid, Type parentType, Type currentType, bool isMain, string[] dependencyList)
            : base(name, parentType)
        {
            _dependencyList = dependencyList;
            _currentType = currentType;
            this.Description = description;
            this.GeneratorGuid = generatorGuid;
            this.IsMain = isMain;
        }

        public GeneratorProjectAttribute(string name, string description, string generatorGuid, Type parentType, Type currentType, bool isMain, string[] dependencyList, string[] exclusionList)
            : this(name, description, generatorGuid, parentType, currentType, isMain, dependencyList)
        {
            _exclusionList = exclusionList;
        }

        public Type CurrentType
        {
            get { return _currentType; }
        }

        public string[] DependencyList
        {
            get { return _dependencyList; }
        }

        public string[] ExclusionList
        {
            get { return _exclusionList; }
        }

        public string Description { get; set; }
        public string GeneratorGuid { get; set; }
        public bool IsMain { get; set; }
    }
}