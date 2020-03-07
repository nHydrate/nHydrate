using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nHydrate.Generator.Common.Util
{
    public static class Extensions
    {
        public static List<EnvDTE.Project> GetProjects(this EnvDTE80.Solution2 solution)
        {
            var projects = new List<EnvDTE.Project>();
            for (var ii = 1; ii <= solution.Count; ii++)
            {
                var project = solution.Item(ii);
                switch (project.Kind)
                {
                    //List: https://www.codeproject.com/reference/720512/list-of-visual-studio-project-type-guids
                    case "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}": //C#
                    case "{8BB2217D-0F2D-49D1-97BC-3654ED321F3B}": //ASP.NET 5
                    case "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}": //.NET Core
                        projects.Add(project); break;
                    default:
                        break;
                }
            }

            var folders = solution.GetFolders();
            foreach (var f in folders)
            {
                for (var ii = 1; ii <= f.ProjectItems.Count; ii++)
                {
                    var project = f.ProjectItems.Item(ii);
                    var p = project.Object as EnvDTE.Project;
                    if (p != null)
                        projects.Add(p);
                }
                //((EnvDTE.ProjectItem)(CurrentSolution.GetFolders()[0].ProjectItems.Item(1))).Name
            }

            return projects;
        }

        public static List<EnvDTE.Project> GetFolders(this EnvDTE80.Solution2 solution)
        {
            var folders = new List<EnvDTE.Project>();
            for (var ii = 1; ii <= solution.Count; ii++)
            {
                var project = solution.Item(ii);
                switch(project.Kind)
                {
                    case "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}":
                    case "{66A26722-8FB5-11D2-AA7E-00C04F688DDE}":
                        folders.Add(project);
                        break;
                    default:
                        break;
                }
            }
            return folders;
        }

        public static List<EnvDTE.Project> GetFolders(this EnvDTE.Project project)
        {
            var folders = new List<EnvDTE.Project>();
            for (var ii = 1; ii <= project.ProjectItems.Count; ii++)
            {
                var child = project.ProjectItems.Item(ii);
                switch(child.Kind)
                {
                    case "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}":
                    case "{66A26722-8FB5-11D2-AA7E-00C04F688DDE}":
                        folders.Add(child as EnvDTE.Project);
                        break;
                    default:
                        break;
                }
            }
            return folders;
        }

        /// <summary>
        /// Determines if the data type is a database type of Int,BigInt,TinyInt,SmallInt
        /// </summary>
        public static bool IsIntegerType(this System.Data.SqlDbType type)
        {
            switch (type)
            {
                case System.Data.SqlDbType.BigInt:
                case System.Data.SqlDbType.Int:
                case System.Data.SqlDbType.TinyInt:
                case System.Data.SqlDbType.SmallInt:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if the data type is a database character type of Binary,Image,VarBinary
        /// </summary>
        public static bool IsBinaryType(this System.Data.SqlDbType type)
        {
            switch (type)
            {
                case System.Data.SqlDbType.Binary:
                case System.Data.SqlDbType.Image:
                case System.Data.SqlDbType.VarBinary:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines if the data type is a database character type of Char,NChar,NText,NVarChar,Text,VarChar,Xml
        /// </summary>
        public static bool IsTextType(this System.Data.SqlDbType type)
        {
            switch (type)
            {
                case System.Data.SqlDbType.Char:
                case System.Data.SqlDbType.NChar:
                case System.Data.SqlDbType.NText:
                case System.Data.SqlDbType.NVarChar:
                case System.Data.SqlDbType.Text:
                case System.Data.SqlDbType.VarChar:
                case System.Data.SqlDbType.Xml:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines if the data type is a database character type of DateTime,DateTime2,DateTimeOffset,SmallDateTime
        /// </summary>
        public static bool IsDateType(this System.Data.SqlDbType type)
        {
            switch (type)
            {
                case System.Data.SqlDbType.Date:
                case System.Data.SqlDbType.DateTime:
                case System.Data.SqlDbType.DateTime2:
                case System.Data.SqlDbType.DateTimeOffset:
                case System.Data.SqlDbType.SmallDateTime:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if the type is a number wither floating point or integer
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumericType(this System.Data.SqlDbType type)
        {
            return IsDecimalType(type) || IsIntegerType(type) || IsMoneyType(type);
        }

        /// <summary>
        /// Determines if the data type is a database type of Money,SmallMoney
        /// </summary>
        public static bool IsMoneyType(this System.Data.SqlDbType type)
        {
            switch (type)
            {
                case System.Data.SqlDbType.Money:
                case System.Data.SqlDbType.SmallMoney:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if the data type is a database type of Decimal,Float,Real
        /// </summary>
        public static bool IsDecimalType(this System.Data.SqlDbType type)
        {
            switch (type)
            {
                case System.Data.SqlDbType.Decimal:
                case System.Data.SqlDbType.Float:
                case System.Data.SqlDbType.Real:
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if this type supports the MAX syntax in SQL 2008
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool SupportsMax(this System.Data.SqlDbType type)
        {
            switch (type)
            {
                case System.Data.SqlDbType.VarChar:
                case System.Data.SqlDbType.NVarChar:
                case System.Data.SqlDbType.VarBinary:
                    return true;
                default:
                    return false;
            }
        }

        public static bool DefaultIsString(this System.Data.SqlDbType type)
        {
            switch (type)
            {
                case System.Data.SqlDbType.BigInt: return false;
                case System.Data.SqlDbType.Binary: return true;
                case System.Data.SqlDbType.Bit: return false;
                case System.Data.SqlDbType.Char: return true;
                case System.Data.SqlDbType.DateTime: return false;
                case System.Data.SqlDbType.Decimal: return false;
                case System.Data.SqlDbType.Float: return false;
                case System.Data.SqlDbType.Image: return true;
                case System.Data.SqlDbType.Int: return false;
                case System.Data.SqlDbType.Money: return false;
                case System.Data.SqlDbType.NChar: return true;
                case System.Data.SqlDbType.NText: return true;
                case System.Data.SqlDbType.NVarChar: return true;
                case System.Data.SqlDbType.Real: return false;
                case System.Data.SqlDbType.SmallDateTime: return false;
                case System.Data.SqlDbType.SmallInt: return false;
                case System.Data.SqlDbType.SmallMoney: return false;
                case System.Data.SqlDbType.Text: return true;
                case System.Data.SqlDbType.Timestamp: return false;
                case System.Data.SqlDbType.TinyInt: return false;
                case System.Data.SqlDbType.Udt: return false;
                case System.Data.SqlDbType.UniqueIdentifier: return true;
                case System.Data.SqlDbType.VarBinary: return true;
                case System.Data.SqlDbType.VarChar: return true;
                case System.Data.SqlDbType.Variant: return true;
                case System.Data.SqlDbType.Xml: return true;
                default: return false;
            }
        }

        /// <summary>
        /// Gets the SQL equivalent for this default value
        /// </summary>
        /// <returns></returns>
        public static string GetSQLDefault(this System.Data.SqlDbType dataType, string defaultValue)
        {
            var retval = string.Empty;
            if ((dataType == System.Data.SqlDbType.DateTime) || (dataType == System.Data.SqlDbType.SmallDateTime))
            {
                if (defaultValue == "getdate")
                {
                    //retval = defaultValue;
                }
                else if (defaultValue == "sysdatetime")
                {
                    //retval = defaultValue;
                }
                else if (defaultValue == "getutcdate")
                {
                    //retval = defaultValue;
                }
                else if (defaultValue.StartsWith("getdate+"))
                {
                    //Do Nothing - Cannot calculate
                }
                //else if (daatType == System.Data.SqlDbType.SmallDateTime)
                //{
                //  defaultValue = String.Format("new DateTime(1900, 1, 1)", this.PascalName);
                //}
                //else
                //{
                //  defaultValue = String.Format("new DateTime(1753, 1, 1)", this.PascalName);
                //}
            }
            else if (dataType == System.Data.SqlDbType.Char)
            {
                retval = "' '";
                if (defaultValue.Length == 1)
                    retval = "'" + defaultValue[0].ToString().Replace("'", "''") + "'";
            }
            else if (dataType.IsBinaryType())
            {
                //Do Nothing - Cannot calculate
            }
            //else if (dataType == System.Data.SqlDbType.DateTimeOffset)
            //{
            //  defaultValue = "DateTimeOffset.MinValue";
            //}
            //else if (this.IsDateType)
            //{
            //  defaultValue = "System.DateTime.MinValue";
            //}
            //else if (dataType == System.Data.SqlDbType.Time)
            //{
            //  defaultValue = "0";
            //}
            else if (dataType == System.Data.SqlDbType.UniqueIdentifier)
            {
                //Do Nothing
                //if ((StringHelper.Match(defaultValue, "newid", true)) || (StringHelper.Match(defaultValue, "newid()", true)))
                //  retval = "newid";
                //else if (string.IsNullOrEmpty(defaultValue))
                //  retval = string.Empty;
                //else
                //{
                //  Guid g;
                //  if (Guid.TryParse(defaultValue, out g))
                //    retval = "'" + g.ToString() + "'";
                //}
            }
            else if (dataType.IsIntegerType())
            {
                int i;
                if (int.TryParse(defaultValue, out i))
                    retval = defaultValue;
            }
            else if (dataType.IsNumericType())
            {
                double d;
                if (double.TryParse(defaultValue, out d))
                {
                    retval = defaultValue;
                }
            }
            else if (dataType == System.Data.SqlDbType.Bit)
            {
                if (defaultValue == "0" || defaultValue == "1")
                    retval = defaultValue;
            }
            else
            {
                if (dataType.IsTextType() && !string.IsNullOrEmpty(defaultValue))
                    retval = "'" + defaultValue.Replace("'", "''") + "'";
            }
            return retval;
        }

    }
}
