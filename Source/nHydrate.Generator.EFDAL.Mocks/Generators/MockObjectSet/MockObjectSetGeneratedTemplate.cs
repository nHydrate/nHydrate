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
using System;
using System.Text;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.EFDAL.Mocks.Generators.MockObjectSets
{
    public class MockObjectSetGeneratedTemplate : EFDALMockBaseTemplate
    {
        private readonly StringBuilder sb = new StringBuilder();

        public MockObjectSetGeneratedTemplate(ModelRoot model)
            : base(model)
        {
        }

        #region BaseClassTemplate overrides
        public override string FileName
        {
            get { return "MockObjectSet.Generated.cs"; }
        }

        public string ParentItemName
        {
            get { return "MockObjectSet.cs"; }
        }

        public override string FileContent
        {
            get
            {
                GenerateContent();
                return sb.ToString();
            }
        }
        #endregion

        #region GenerateContent

        private void GenerateContent()
        {
            try
            {
                nHydrate.Generator.GenerationHelper.AppendFileGeneatedMessageInCode(sb);
                nHydrate.Generator.GenerationHelper.AppendCopyrightInCode(sb, _model);
                this.AppendUsingStatements();
                sb.AppendLine("namespace " + this.GetLocalNamespace());
                sb.AppendLine("{");
                sb.AppendLine("	/// <summary>");
                sb.AppendLine("	/// This object is used to mock entity sets on the mock context");
                sb.AppendLine("	/// </summary>");
                sb.AppendLine("	public partial class MockObjectSet<T> : IObjectSet<T> where T : class");
                sb.AppendLine("	{");
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// ");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		protected HashSet<T> _data;");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// ");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		protected IQueryable _query;");
                sb.AppendLine();
                sb.AppendLine("		#region Constructor");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Default constructor for MockObjectSet");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public MockObjectSet() : this(new List<T>()) { }");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// ");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public MockObjectSet(IEnumerable<T> initialData)");
                sb.AppendLine("		{");
                sb.AppendLine("			_data = new HashSet<T>(initialData);");
                sb.AppendLine("			_query = _data.AsQueryable();");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		#endregion");
                sb.AppendLine();
                sb.AppendLine("		#region Methods");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Adds an item to this list");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public void Add(T item)");
                sb.AppendLine("		{");
                sb.AppendLine("			_data.Add(item);");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Adds an item to this list");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public void AddObject(T item)");
                sb.AppendLine("		{");
                sb.AppendLine("			_data.Add(item);");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Removes an item to this list");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public void Remove(T item)");
                sb.AppendLine("		{");
                sb.AppendLine("			_data.Remove(item);");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Removes an item to this list");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public void DeleteObject(T item)");
                sb.AppendLine("		{");
                sb.AppendLine("			_data.Remove(item);");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Attaches an item to this list");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public void Attach(T item)");
                sb.AppendLine("		{");
                sb.AppendLine("			_data.Add(item);");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		/// <summary>");
                sb.AppendLine("		/// Detaches an item to this list");
                sb.AppendLine("		/// </summary>");
                sb.AppendLine("		public void Detach(T item)");
                sb.AppendLine("		{");
                sb.AppendLine("			_data.Remove(item);");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		#endregion");
                sb.AppendLine();
                sb.AppendLine("		#region Base Functionality");
                sb.AppendLine();
                sb.AppendLine("		Type IQueryable.ElementType");
                sb.AppendLine("		{");
                sb.AppendLine("			get { return _query.ElementType; }");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		System.Linq.Expressions.Expression IQueryable.Expression");
                sb.AppendLine("		{");
                sb.AppendLine("			get { return _query.Expression; }");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		IQueryProvider IQueryable.Provider");
                sb.AppendLine("		{");
                sb.AppendLine("			get { return _query.Provider; }");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()");
                sb.AppendLine("		{");
                sb.AppendLine("			return _data.GetEnumerator();");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		IEnumerator<T> IEnumerable<T>.GetEnumerator()");
                sb.AppendLine("		{");
                sb.AppendLine("			return _data.GetEnumerator();");
                sb.AppendLine("		}");
                sb.AppendLine();
                sb.AppendLine("		#endregion");
                sb.AppendLine();
                sb.AppendLine("	}");
                sb.AppendLine();
                sb.AppendLine("}");
                sb.AppendLine();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void AppendUsingStatements()
        {
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine("using System.Data.Objects;");
            sb.AppendLine("using System.Data.Objects.DataClasses;");
            sb.AppendLine("using System.ComponentModel;");
            sb.AppendLine("using System.Runtime.Serialization;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using " + this.DefaultNamespace + ".EFDAL.Interfaces;");
            sb.AppendLine();
        }

        #endregion

    }
}