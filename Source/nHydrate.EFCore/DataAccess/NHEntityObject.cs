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
using System.Data.Objects.DataClasses;
using System.Linq;

namespace nHydrate.EFCore.DataAccess
{
    /// <summary>
    /// The base class for all entity objects
    /// </summary>
    [Serializable]
    [System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    public abstract partial class NHEntityObject : EntityObject, INHEntityObject
    {
        /// <summary>
        /// Get the validation rule violations
        /// </summary>
        public virtual IEnumerable<IRuleViolation> GetRuleViolations()
        {
            var retval = new List<IRuleViolation>();
            return retval;
        }

        /// <summary>
        /// Determines if all of the validation rules have been met
        /// </summary>
        public virtual bool IsValid()
        {
            return (!GetRuleViolations().Any());
        }

        /// <summary>
        /// Test another entity object for equivalence against the current instance
        /// </summary>
        public abstract bool IsEquivalent(INHEntityObject item);

        /// <summary>
        /// This method may be overridden to provide validation for entity objects
        /// </summary>
        /// <returns></returns>
        protected virtual string GetObjectDataErrorInfo()
        {
            return string.Empty;
        }

        /// <summary>
        /// This method may be overridden to provide validation for entity properties
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        protected virtual string GetObjectPropertyDataErrorInfo(string columnName)
        {
            return string.Empty;
        }

    }
}