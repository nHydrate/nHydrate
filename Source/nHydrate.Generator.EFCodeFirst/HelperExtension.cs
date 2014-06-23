using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nHydrate.Generator.Models;

namespace nHydrate.Generator.EFCodeFirst
{
    public static class RelationExtension
    {
        public static string GetCodeFkName(this Relation relation)
        {
            Table parentEntity = (Table)relation.ParentTableRef.Object;
            Table childEntity = (Table)relation.ChildTableRef.Object;
            string fkName = string.Format("FK_{0}_{1}_{2}", relation.PascalRoleName, childEntity.PascalName, parentEntity.PascalName);
            return fkName;
        }

        public static string GetDatabaseFkName(this Relation relation)
        {
            return GetCodeFkName(relation);
            //Table parentEntity = (Table)relation.ParentTableRef.Object;
            //Table childEntity = (Table)relation.ChildTableRef.Object;
            //string fkName = string.Format("FK_{0}{1}{2}", relation.DatabaseRoleName, parentEntity.DatabaseName, childEntity.DatabaseName);
            //return fkName;
        }

        public static bool IsRequired(this Relation relation)
        {
            bool retVal = false;
            foreach (ColumnRelationship columnRelation in relation.ColumnRelationships)
            {
                Column childColumn = (Column)columnRelation.ChildColumnRef.Object;
                if (!childColumn.AllowNull)
                {
                    retVal = true;
                }
            }

            return retVal;
        }

        public static string ParentMultiplicity(this Relation relation)
        {
            if (relation.IsOneToOne)
                return "1";
            else if (relation.IsRequired())
                return "1";
            else
                return "0..1";
        }

        public static string ChildMultiplicity(this Relation relation)
        {
            if (relation.IsOneToOne)
                return "0..1";
            else
                return "*";
        }
    }


    public static class ColumnExtension
    {
         public static string EFDatabaseType(this ColumnBase column)
        {
            return column.EFDatabaseType(true);
        }

        public static string EFDatabaseType(this ColumnBase column, bool appendMax)
        {
            string retVal;
            retVal = column.DataType.ToString().ToLower();
            if (column.GetLengthString().ToLower() == "max" && appendMax)
            {
                retVal += "(max)";
            }
            return retVal;
        }

    }
}
