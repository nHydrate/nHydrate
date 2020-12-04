using System.Linq;

namespace nHydrate.Generator.Common
{
    public static class ValidationHelper
    {
        public static readonly string ErrorTextInvalidCompany = "The company name is the wrong format or contains invalid characters.";
        public static readonly string ErrorTextInvalidProject = "The project name is the wrong format or contains invalid characters.";
        public static readonly string ErrorTextInvalidNamespace = "The defined namespace is not valid. It must be in the format A.B.*.";

        public const string ValidCodeChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
        private const string reservedSQLWords = "";
        private const string reservedCSharpWords = "_Bool,_Complex,_Imaginary,_Packed,abstract,as,auto,base,bool,break,byte,case,catch,char,checked,class,const,continue,decimal,default,delegate,do,double,else,enum,event,exception,explicit,extern,false,finally,fixed,float,for,foreach,goto,if,implicit,in,inline,int,interface,internal,is,lock,long,namespace,new,null,object,operator,out,override,params,private,protected,public,readonly,ref,register,restrict,return,sbyte,sealed,short,signed,sizeof,stackalloc,static,string,struct,switch,this,throw,true,try,typedef,typeof,uint,ulong,unchecked,union,unsafe,unsigned,ushort,using,virtual,void,volatile,while,PrimaryKey,attribute,businessobject,attribute,system,BusinessCollectionBase,BusinessCollectionPersistableBase,BusinessObjectBase,BusinessObjectList,BusinessObjectPersistableBase,IVisitee,IVisitor,IAuditable,IPagingObject,IPagingFieldItem,DomainCollectionBase,Enumerations,IBusinessCollection,IBusinessObject,IBusinessObjectSearch,IDomainCollection,IDomainObject,IErrorRow,IPersistableBusinessCollection,IPersistableBusinessObject,IPersistableDomainObject,IPrimaryKey,IPropertyBag,IPropertyDefine,IReadOnlyBusinessCollection,IReadOnlyBusinessObject,ITyped,IWrappingClass,PersistableDomainCollectionBase,PersistableDomainObjectBase,PrimaryKey,ReadOnlyDomainCollection,ReadOnlyDomainObjectBase,SelectCommand,StoredProcedure,StoredProcedureFactory,SubDomainBase,SubDomainWrapper,BusinessObjectCancelEventArgs,BusinessObjectEventArgs,ConcurrencyException,DefaultLogConfiguration,ILogClass,ILogConfiguration,MultiProcessTraceListener,TraceSwitchAttribute,IXmlable,IBoolFieldDescriptor,IDateFieldDescriptor,IFieldDescriptor,IFloatFieldDescriptor,IIntegerFieldDescriptor,IStringFieldDescriptor,As,Assembly,Auto,Base,Boolean,ByRef,Byte,ByVal,Call,Case,Catch,CBool,CByte,CChar,CDate,CDec,CDbl,Char,CInt,Class,CLng,CObj,Const,CShort,CSng,CStr,CType,Date,Decimal,Declare,Default,Delegate,Dim,Do,Double,Enum,Erase,Error,Event,Exit,ExternalSource,False,Finalize,Finally,Float,For,Friend,Get,GetType,Handles,If,Implements,Imports,In,Inherits,Integer,Interface,Is,Let,Lib,Like,Long,Loop,Me,Mod,Namespace,New,Next,Not,Nothing,Object,On,Option,Optional,Or,Preserve,Private,Property,Protected,Public,RaiseEvent,ReadOnly,ReDim,REM,RemoveHandler,Return,Select,Set,Shadows,Shared,Short,Single,Static,Step,Stop,String,Structure,Sub,SyncLock,Then,Throw,To,True,Try,TypeOf,Unicode,Until,volatile,When,While,With,WithEvents,WriteOnly,Xor,eval,extends,instanceof,package,var";
        private const string reservedFields = "clone,isparented,container,delete,equals,getdatetime,getdefault,getdouble,getfieldlength,GetHashCode,GetInteger,GetString,GetValue,IsEquivalent,ItemState,OnValidate,ParentCollection,Persist,RejectChanges,PrimaryKey,ReleaseNonIdentifyingRelationships,Remove,SelectUsingPK,SetCreatedDate,SetModifiedDate,SetValue,Validate,wrappedClass,DeleteData,GetDatabaseFieldName,GetFieldAliasFromFieldNameSqlMapping,GetMaxLength,GetPagedSQL,GetRemappedLinqSql,GetTableFromFieldAliasSqlMapping,GetTableFromFieldNameSqlMapping,UpdateData";

        public static string MakeCodeIdentifer(string name)
        {
            if (name.Length == 0)
                return string.Empty;

            var retval = string.Empty;
            var validchars2 = ValidCodeChars;

            foreach (var c in name)
            {
                if (!validchars2.Contains(c))
                    retval += "_";
                else
                    retval += c;
            }
            return retval;
        }

        public static string MakeDatabaseIdentifier(string name)
        {
            if (name.Length == 0)
                return string.Empty;

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
    }
}
