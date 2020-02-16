using System.Linq;

namespace nHydrate.Generator.Common
{
    public static class ValidationHelper
    {
        public static readonly string ErrorTextInvalidCompany = "The company name is the wrong format or contains invalid characters.";
        public static readonly string ErrorTextInvalidProject = "The project name is the wrong format or contains invalid characters.";
        public static readonly string ErrorTextInvalidNamespace = "The defined namespace is not valid. It must be in the format A.B.*.";

        public const string ValidCodeChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
        //private const string reservedSQLWords = "ENCRYPTION,ORDER,ADD,END,OUTER,ALL,ERRLVL,OVER,ALTER,ESCAPE,PERCENT,AND,EXCEPT,PLAN,ANY,EXEC,PRECISION,AS,EXECUTE,PRIMARY,ASC,EXISTS,PRINT,AUTHORIZATION,EXIT,PROC,AVG,EXPRESSION,PROCEDURE,BACKUP,FETCH,PUBLIC,BEGIN,FILE,RAISERROR,BETWEEN,FILLFACTOR,READ,BREAK,FOR,READTEXT,BROWSE,FOREIGN,RECONFIGURE,BULK,FREETEXT,REFERENCES,BY,FREETEXTTABLE,REPLICATION,CASCADE,FROM,RESTORE,CASE,FULL,RESTRICT,CHECK,RETURN,CHECKPOINT,GOTO,REVOKE,CLOSE,GRANT,RIGHT,CLUSTERED,GROUP,ROLLBACK,COALESCE,HAVING,ROWCOUNT,COLLATE,HOLDLOCK,ROWGUIDCOL,COLUMN,IDENTITY,RULE,COMMIT,IDENTITY_INSERT,SAVE,COMPUTE,IDENTITYCOL,SCHEMA,CONSTRAINT,IF,SELECT,CONTAINS,IN,SESSION_USER,CONTAINSTABLE,INDEX,SET,CONTINUE,INNER,SETUSER,CONVERT,INSERT,SHUTDOWN,COUNT,INTERSECT,SOME,CREATE,INTO,STATISTICS,CROSS,IS,SUM,CURRENT,JOIN,SYSTEM_USER,CURRENT_DATE,KEY,TABLE,CURRENT_TIME,KILL,TEXTSIZE,CURRENT_TIMESTAMP,LEFT,THEN,CURRENT_USER,LIKE,TO,CURSOR,LINENO,TOP,DATABASE,LOAD,TRAN,DATABASEPASSWORD,MAX,TRANSACTION,DATEADD,MIN,TRIGGER,DATEDIFF,NATIONAL,TRUNCATE,DATENAME,NOCHECK,TSEQUAL,DATEPART,NONCLUSTERED,UNION,DBCC,NOT,UNIQUE,DEALLOCATE,NULL,UPDATE,DECLARE,NULLIF,UPDATETEXT,DEFAULT,OF,USE,DELETE,OFF,USER,DENY,OFFSETS,VALUES,DESC,ON,VARYING,DISK,OPEN,VIEW,DISTINCT,OPENDATASOURCE,WAITFOR,DISTRIBUTED,OPENQUERY,WHEN,DOUBLE,OPENROWSET,WHERE,DROP,OPENXML,WHILE,DUMP,OPTION,WITH,ELSE,OR,WRITETEXT";
        private const string reservedSQLWords = "";
        //REMOVED: region
        //private const string reservedCSharpWords = "_Bool,_Complex,_Imaginary,_Packed,abstract,as,auto,base,bool,break,byte,case,catch,char,checked,class,const,continue,decimal,default,delegate,do,double,else,enum,event,exception,explicit,extern,false,finally,fixed,float,for,foreach,goto,if,implicit,in,inline,int,interface,internal,is,lock,long,namespace,new,null,object,operator,out,override,params,private,protected,public,readonly,ref,register,restrict,return,sbyte,sealed,short,signed,sizeof,stackalloc,static,string,struct,switch,this,throw,true,try,typedef,typeof,uint,ulong,unchecked,union,unsafe,unsigned,ushort,using,virtual,void,volatile,while,PrimaryKey,attribute,businessobject,attribute,system,BusinessCollectionBase,BusinessCollectionPersistableBase,BusinessObjectBase,BusinessObjectList,BusinessObjectPersistableBase,IVisitee,IVisitor,IAuditable,IPagingObject,IPagingFieldItem,DomainCollectionBase,Enumerations,IBusinessCollection,IBusinessObject,IBusinessObjectSearch,IDomainCollection,IDomainObject,IErrorRow,IPersistableBusinessCollection,IPersistableBusinessObject,IPersistableDomainObject,IPrimaryKey,IPropertyBag,IPropertyDefine,IReadOnlyBusinessCollection,IReadOnlyBusinessObject,ITyped,IWrappingClass,PersistableDomainCollectionBase,PersistableDomainObjectBase,PrimaryKey,ReadOnlyDomainCollection,ReadOnlyDomainObjectBase,SelectCommand,StoredProcedure,StoredProcedureFactory,SubDomainBase,SubDomainWrapper,BusinessObjectCancelEventArgs,BusinessObjectEventArgs,ConcurrencyException,DefaultLogConfiguration,ILogClass,ILogConfiguration,MultiProcessTraceListener,TraceSwitchAttribute,IXmlable,IBoolFieldDescriptor,IDateFieldDescriptor,IFieldDescriptor,IFloatFieldDescriptor,IIntegerFieldDescriptor,IStringFieldDescriptor,As,Assembly,Auto,Base,Boolean,ByRef,Byte,ByVal,Call,Case,Catch,CBool,CByte,CChar,CDate,CDec,CDbl,Char,CInt,Class,CLng,CObj,Const,CShort,CSng,CStr,CType,Date,Decimal,Declare,Default,Delegate,Dim,Do,Double,Each,Else,ElseIf,End,Enum,Erase,Error,Event,Exit,ExternalSource,False,Finalize,Finally,Float,For,Friend,Get,GetType,Goto,Handles,If,Implements,Imports,In,Inherits,Integer,Interface,Is,Let,Lib,Like,Long,Loop,Me,Mod,MustInherit,MustOverride,MyBase,MyClass,Namespace,New,Next,Not,Nothing,NotInheritable,NotOverridable,Object,On,Option,Optional,Or,Overloads,Overridable,Overrides,ParamArray,Preserve,Private,Property,Protected,Public,RaiseEvent,ReadOnly,ReDim,REM,RemoveHandler,Resume,Return,Select,Set,Shadows,Shared,Short,Single,Static,Step,Stop,String,Structure,Sub,SyncLock,Then,Throw,To,True,Try,TypeOf,Unicode,Until,volatile,When,While,With,WithEvents,WriteOnly,Xor,eval,extends,instanceof,package,var";
        private const string reservedCSharpWords = "_Bool,_Complex,_Imaginary,_Packed,abstract,as,auto,base,bool,break,byte,case,catch,char,checked,class,const,continue,decimal,default,delegate,do,double,else,enum,event,exception,explicit,extern,false,finally,fixed,float,for,foreach,goto,if,implicit,in,inline,int,interface,internal,is,lock,long,namespace,new,null,object,operator,out,override,params,private,protected,public,readonly,ref,register,restrict,return,sbyte,sealed,short,signed,sizeof,stackalloc,static,string,struct,switch,this,throw,true,try,typedef,typeof,uint,ulong,unchecked,union,unsafe,unsigned,ushort,using,virtual,void,volatile,while,PrimaryKey,attribute,businessobject,attribute,system,BusinessCollectionBase,BusinessCollectionPersistableBase,BusinessObjectBase,BusinessObjectList,BusinessObjectPersistableBase,IVisitee,IVisitor,IAuditable,IPagingObject,IPagingFieldItem,DomainCollectionBase,Enumerations,IBusinessCollection,IBusinessObject,IBusinessObjectSearch,IDomainCollection,IDomainObject,IErrorRow,IPersistableBusinessCollection,IPersistableBusinessObject,IPersistableDomainObject,IPrimaryKey,IPropertyBag,IPropertyDefine,IReadOnlyBusinessCollection,IReadOnlyBusinessObject,ITyped,IWrappingClass,PersistableDomainCollectionBase,PersistableDomainObjectBase,PrimaryKey,ReadOnlyDomainCollection,ReadOnlyDomainObjectBase,SelectCommand,StoredProcedure,StoredProcedureFactory,SubDomainBase,SubDomainWrapper,BusinessObjectCancelEventArgs,BusinessObjectEventArgs,ConcurrencyException,DefaultLogConfiguration,ILogClass,ILogConfiguration,MultiProcessTraceListener,TraceSwitchAttribute,IXmlable,IBoolFieldDescriptor,IDateFieldDescriptor,IFieldDescriptor,IFloatFieldDescriptor,IIntegerFieldDescriptor,IStringFieldDescriptor,As,Assembly,Auto,Base,Boolean,ByRef,Byte,ByVal,Call,Case,Catch,CBool,CByte,CChar,CDate,CDec,CDbl,Char,CInt,Class,CLng,CObj,Const,CShort,CSng,CStr,CType,Date,Decimal,Declare,Default,Delegate,Dim,Do,Double,Enum,Erase,Error,Event,Exit,ExternalSource,False,Finalize,Finally,Float,For,Friend,Get,GetType,Handles,If,Implements,Imports,In,Inherits,Integer,Interface,Is,Let,Lib,Like,Long,Loop,Me,Mod,Namespace,New,Next,Not,Nothing,Object,On,Option,Optional,Or,Preserve,Private,Property,Protected,Public,RaiseEvent,ReadOnly,ReDim,REM,RemoveHandler,Return,Select,Set,Shadows,Shared,Short,Single,Static,Step,Stop,String,Structure,Sub,SyncLock,Then,Throw,To,True,Try,TypeOf,Unicode,Until,volatile,When,While,With,WithEvents,WriteOnly,Xor,eval,extends,instanceof,package,var";
        private const string reservedFields = "clone,isparented,container,delete,equals,getdatetime,getdefault,getdouble,getfieldlength,GetHashCode,GetInteger,GetString,GetValue,IsEquivalent,ItemState,OnValidate,ParentCollection,Persist,RejectChanges,PrimaryKey,ReleaseNonIdentifyingRelationships,Remove,SelectUsingPK,SetCreatedDate,SetModifiedDate,SetValue,Validate,wrappedClass,DeleteData,GetDatabaseFieldName,GetFieldAliasFromFieldNameSqlMapping,GetMaxLength,GetPagedSQL,GetRemappedLinqSql,GetTableFromFieldAliasSqlMapping,GetTableFromFieldNameSqlMapping,UpdateData";

        public static bool ValidDatabaseIdenitifer(string name)
        {
            if (name.Length == 0)
                return false;

            var words = reservedSQLWords.ToLower().Split(',');

            var q = (from x in words
                     where name.ToLower() == x
                     select x).FirstOrDefault();

            if (q != null)
                return false;

            //The database does actualy allow spaces
            const string validchars2 = ValidCodeChars + " /#";

            foreach (var c in name)
            {
                if (validchars2.IndexOf(c) == -1)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Determines if the specified value is a valid C# token
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool ValidCodeIdentifier(string name)
        {
            if (name.Length == 0)
                return false;

            var words = reservedCSharpWords.ToLower().Split(',');

            var q = (from x in words
                     where name.ToLower() == x
                     select x).FirstOrDefault();

            if (q != null)
                return false;

            foreach (var c in name)
            {
                if (ValidCodeChars.IndexOf(c) == -1)
                    return false;
            }

            //First 
            var firstChar = name.First();
            int n;
            if (int.TryParse(firstChar.ToString(), out n))
                return false;

            return true;
        }

        /// <summary>
        /// Determines if the specified value matches any reserved words for objects
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool ValidFieldIdentifier(string name)
        {
            if (name.Length == 0)
                return false;

            var words = reservedFields.ToLower().Split(',');

            var q = (from x in words
                     where name.ToLower() == x
                     select x).FirstOrDefault();

            if (q != null)
                return false;

            foreach (var c in name)
            {
                if (ValidCodeChars.IndexOf(c) == -1)
                    return false;
            }

            //First 
            var firstChar = name.First();
            int n;
            if (int.TryParse(firstChar.ToString(), out n))
                return false;

            return true;
        }

        public static string MakeCodeIdentifer(string name)
        {
            if (name.Length == 0)
                return "";

            var retval = string.Empty;
            var validchars2 = ValidCodeChars;

            foreach (var c in name)
            {
                if (validchars2.IndexOf(c) == -1)
                    retval += "_";
                else
                    retval += c;
            }
            return retval;
        }

        public static string MakeDatabaseIdentifier(string name)
        {
            if (name.Length == 0)
                return "";

            var retval = string.Empty;
            var validchars2 = ValidCodeChars + " /#";

            foreach (var c in name)
            {
                if (validchars2.IndexOf(c) == -1)
                    retval += "_";
                else
                    retval += c;
            }
            return retval;
        }

        public static string MakeDatabaseScriptIdentifier(string name)
        {
            if (name.Length == 0)
                return "";

            var retval = string.Empty;
            foreach (var c in name)
            {
                if (ValidCodeChars.IndexOf(c) == -1)
                    retval += "_";
                else
                    retval += c;
            }
            return retval;
        }

        public static bool IsValidNamespace(string namespaceValue)
        {
            if (namespaceValue != namespaceValue.Trim()) return false;
            var arr = namespaceValue.Split('.');
            if (arr.Length == 0) return false;

            foreach (var s in arr)
            {
                if (!ValidCodeIdentifier(s)) return false;
            }

            return true;
        }

    }
}
