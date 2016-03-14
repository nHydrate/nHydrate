#region Copyright (c) 2006-2016 nHydrate.org, All Rights Reserved
// -------------------------------------------------------------------------- *
//                           NHYDRATE.ORG                                     *
//              Copyright (c) 2006-2016 All Rights reserved                   *
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
using System.Data.Common;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;

namespace nHydrate.EFCore.DataAccess
{
    public class BusinessEntityQuery
    {
        /// <summary>
        /// Gets the DbCommand from the Dynamic Query but Ensures that CAS Rules are taken to consideration
        /// </summary>
        /// <typeparam name="TEntity">The Type of Entity</typeparam>
        /// <param name="dataContext">Linq Data Context</param>
        /// <param name="template">Table </param>
        /// <param name="where">The Dynamic Linq</param>
        /// <returns></returns>
        public static DbCommand GetCommand<TEntity>(
                DataContext dataContext,
                Table<TEntity> template,
                Expression<Func<TEntity, bool>> where)
                where TEntity : class
        {
            //FileIOPermission permission = new FileIOPermission(PermissionState.Unrestricted);
            //permission.AllFiles = FileIOPermissionAccess.Write;
            //permission.Deny();

            return dataContext.GetCommand(template
                    .Where(where)
                    .Select(x => x));
        }

        /// <summary>
        /// Gets the DbCommand from the Dynamic Query but Ensures that CAS Rules are taken to consideration
        /// </summary>
        /// <typeparam name="TEntity">The Type of Entity</typeparam>
        /// <param name="dataContext">Linq Data Context</param>
        /// <param name="template">Table </param>
        /// <param name="where">The Dynamic Linq</param>
        /// <returns></returns>
        public static DbCommand GetCommand<TEntity, TResult>(
                DataContext dataContext,
                Table<TEntity> template,
                Expression<Func<TEntity, TResult>> select,
                Expression<Func<TEntity, bool>> where)
                where TEntity : class
        {
            //FileIOPermission permission = new FileIOPermission(PermissionState.Unrestricted);
            //permission.AllFiles = FileIOPermissionAccess.Write;
            //permission.Deny();

            return dataContext.GetCommand(template
                    .Where(where)
                    .Select(select));
        }
    }
}