using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nHydrate.Core.SQLGeneration
{
    public static class HtmlEmit
    {
        public static string FormatHTMLSQL(string sql)
        {
            var sb = new StringBuilder();
            sql = sql.Replace("\r\n", "\n");
            var lines = sql.Split('\n');

            var connectors = HTMLSQLConnectors();
            var functions = HTMLSQLFunctions();
            var keywords = HTMLSQLKeywords();
            var metadata = HTMLSQLMeta();

            foreach (var line in lines)
            {
                var words = line.Split(' ');
                if (line.Trim().StartsWith("--"))
                {
                    //This is a comment
                    sb.AppendLine("<span class=\"sql_comm\">" + line + "</span><br />");
                }
                else
                {
                    foreach (var word in words)
                    {
                        if (connectors.Contains(word.ToLower().Trim()))
                            sb.Append("<span class=\"sql_conn\">" + word + "</span> ");
                        else if (functions.Contains(word.ToLower().Trim()))
                            sb.Append("<span class=\"sql_func\">" + word + "</span> ");
                        else if (keywords.Contains(word.ToLower().Trim()))
                            sb.Append("<span class=\"sql_key\">" + word + "</span> ");
                        else if (metadata.Contains(word.ToLower().Trim()))
                            sb.Append("<span class=\"sql_meta\">" + word + "</span> ");
                        else
                            sb.Append(word + " ");
                    }
                    sb.AppendLine("<br />");
                }
            }
            return sb.ToString();
        }

        public static string FormatHTMLCode(string code)
        {
            var sb = new StringBuilder();
            code = code.Replace("\r\n", "\n");
            var lines = code.Split('\n');

            var keywords = HTMLCSharpKeywords();

            foreach (var line in lines)
            {
                var words = line.Split(new char[] { ' ' });
                if (line.Trim().StartsWith("//"))
                {
                    //This is a comment
                    sb.AppendLine("<span class=\"code_comm\">" + line + "</span><br />");
                }
                else
                {
                    for (var ii = 0; ii < words.Length; ii++)
                    {
                        var word = words[ii];

                        //If the last loop marked this as an object then parse it
                        if (keywords.Contains(word.Trim()))
                        {
                            sb.Append("<span class=\"code_key\">" + word + "</span> ");
                        }
                        else if ((ii > 0) && (words[ii - 1] == "new") && word.Contains("("))
                        {
                            var mark = word.IndexOf("(");
                            var token = word.Substring(0, mark);
                            sb.Append("<span class=\"new_object\">" + token + "</span>" + word.Substring(mark, word.Length - mark) + " ");
                        }
                        else
                        {
                            sb.Append(word + " ");
                        }

                    }
                    sb.AppendLine("<br />");
                }
            }
            return sb.ToString().Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
        }

        #region Keyword Methods

        private static List<string> HTMLCSharpKeywords()
        {
            var list = new List<string>();
            list.Add("abstract");
            list.Add("event");
            list.Add("new");
            list.Add("struct");
            list.Add("as");
            list.Add("explicit");
            list.Add("null");
            list.Add("switch");
            list.Add("base");
            list.Add("extern");
            list.Add("object");
            list.Add("this");
            list.Add("bool");
            list.Add("false");
            list.Add("operator");
            list.Add("throw");
            list.Add("break");
            list.Add("finally");
            list.Add("out");
            list.Add("true");
            list.Add("byte");
            list.Add("fixed");
            list.Add("override");
            list.Add("try");
            list.Add("case");
            list.Add("float");
            list.Add("params");
            list.Add("typeof");
            list.Add("catch");
            list.Add("for");
            list.Add("private");
            list.Add("uint");
            list.Add("char");
            list.Add("foreach");
            list.Add("protected");
            list.Add("ulong");
            list.Add("checked");
            list.Add("goto");
            list.Add("public");
            list.Add("unchecked");
            list.Add("class");
            list.Add("if");
            list.Add("readonly");
            list.Add("unsafe");
            list.Add("const");
            list.Add("implicit");
            list.Add("ref");
            list.Add("ushort");
            list.Add("continue");
            list.Add("in");
            list.Add("return");
            list.Add("using");
            list.Add("decimal");
            list.Add("int");
            list.Add("sbyte");
            list.Add("virtual");
            list.Add("default");
            list.Add("interface");
            list.Add("sealed");
            list.Add("volatile");
            list.Add("delegate");
            list.Add("internal");
            list.Add("short");
            list.Add("void");
            list.Add("do");
            list.Add("is");
            list.Add("sizeof");
            list.Add("while");
            list.Add("double");
            list.Add("lock");
            list.Add("stackalloc");
            list.Add("else");
            list.Add("long");
            list.Add("static");
            list.Add("enum");
            list.Add("namespace");
            list.Add("string");
            list.Add("var");
            return list;
        }

        private static List<string> HTMLSQLMeta()
        {
            var list = new List<string>();
            list.Add("sysaltfiles");
            list.Add("syslockinfo");
            list.Add("syscacheobjects");
            list.Add("syslogins");
            list.Add("syscharsets");
            list.Add("sysmessages");
            list.Add("sysconfigures");
            list.Add("sysoledbusers");
            list.Add("syscurconfigs");
            list.Add("sysperfinfo");
            list.Add("sysdatabases");
            list.Add("sysprocesses");
            list.Add("sysdevices");
            list.Add("sysremotelogins");
            list.Add("syslanguages");
            list.Add("sysservers");
            list.Add("syscolumns");
            list.Add("sysindexkeys");
            list.Add("syscomments");
            list.Add("sysmembers");
            list.Add("sysconstraints");
            list.Add("sysobjects");
            list.Add("sysdepends");
            list.Add("syspermissions");
            list.Add("sysfilegroups");
            list.Add("sysprotects");
            list.Add("sysfiles");
            list.Add("sysreferences");
            list.Add("sysforeignkeys");
            list.Add("systypes");
            list.Add("sysfulltextcatalogs");
            list.Add("sysusers");
            list.Add("sysindexes ");
            list.Add("sysalerts");
            list.Add("sysjobsteps");
            list.Add("syscategories");
            list.Add("sysnotifications");
            list.Add("sysdownloadlist");
            list.Add("sysoperators");
            list.Add("sysjobhistory");
            list.Add("systargetservergroupmembers");
            list.Add("sysjobs");
            list.Add("systargetservergroups");
            list.Add("sysjobschedules");
            list.Add("systargetservers");
            list.Add("sysjobservers");
            list.Add("systaskids");
            list.Add("backupfile");
            list.Add("restorefile");
            list.Add("backupmediafamily");
            list.Add("restorefilegroup");
            list.Add("backupmediaset");
            list.Add("restorehistory");
            list.Add("backupset ");
            list.Add("sysdatabases");
            list.Add("sysservers");
            return list;
        }

        private static List<string> HTMLSQLConnectors()
        {
            var list = new List<string>();
            list.Add("ALL");
            list.Add("AND");
            list.Add("EXISTS");
            list.Add("ANY");
            list.Add("BETWEEN");
            list.Add("RIGHT");
            list.Add("IN");
            list.Add("INNER");
            list.Add("IS");
            list.Add("SOME");
            list.Add("OUTER");
            list.Add("JOIN");
            list.Add("CROSS");
            list.Add("LEFT");
            list.Add("LIKE");
            list.Add("NOT");
            list.Add("NULL");
            list.Add("OR");
            list.Add("NULL");
            list.Add("NOT");
            list.Add("CROSS");
            list.Add("LIKE");
            list.Add("SOME");
            list.Add("IS");
            list.Add("JOIN");
            for (var ii = 0; ii < list.Count; ii++) list[ii] = list[ii].ToLower();
            return list;
        }

        private static List<string> HTMLSQLFunctions()
        {
            var list = new List<string>();
            list.Add("COALESCE");
            list.Add("SESSION_USER");
            list.Add("CONVERT");
            list.Add("SYSTEM_USER");
            list.Add("CURRENT_TIMESTAMP");
            list.Add("CURRENT_USER");
            list.Add("USER");
            list.Add("NULLIF");
            list.Add("UPPER");
            list.Add("YEAR");
            list.Add("NULLIF");
            list.Add("MONTH");
            list.Add("CURRENT_TIMESTAMP");
            list.Add("CURRENT_USER");
            list.Add("MAX");
            list.Add("MIN DAY ");
            list.Add("LOWER");
            list.Add("COUNT");
            list.Add("AVG");
            list.Add("CAST");
            list.Add("SESSION_USER");
            list.Add("COALESCE");
            list.Add("SPACE");
            list.Add("SUM");
            list.Add("SYSTEM_USER");
            list.Add("CONVERT");
            list.Add("SUBSTRING");
            for (var ii = 0; ii < list.Count; ii++) list[ii] = list[ii].ToLower();
            return list;
        }

        private static List<string> HTMLSQLKeywords()
        {
            var list = new List<string>();
            list.Add("ADD");
            list.Add("EXCEPT");
            list.Add("PERCENT");
            list.Add("EXEC");
            list.Add("PLAN");
            list.Add("ALTER");
            list.Add("EXECUTE");
            list.Add("PRECISION");
            list.Add("PRIMARY");
            list.Add("EXIT");
            list.Add("PRINT");
            list.Add("AS");
            list.Add("FETCH");
            list.Add("PROC");
            list.Add("ASC");
            list.Add("FILE");
            list.Add("PROCEDURE");
            list.Add("AUTHORIZATION");
            list.Add("FILLFACTOR");
            list.Add("PUBLIC");
            list.Add("BACKUP");
            list.Add("FOR");
            list.Add("RAISERROR");
            list.Add("BEGIN");
            list.Add("FOREIGN");
            list.Add("READ");
            list.Add("FREETEXT");
            list.Add("READTEXT");
            list.Add("BREAK");
            list.Add("FREETEXTTABLE");
            list.Add("RECONFIGURE");
            list.Add("BROWSE");
            list.Add("FROM");
            list.Add("REFERENCES");
            list.Add("BULK");
            list.Add("FULL");
            list.Add("REPLICATION");
            list.Add("BY");
            list.Add("FUNCTION");
            list.Add("RESTORE");
            list.Add("CASCADE");
            list.Add("GOTO");
            list.Add("RESTRICT");
            list.Add("CASE");
            list.Add("GRANT");
            list.Add("RETURN");
            list.Add("CHECK");
            list.Add("GROUP");
            list.Add("REVOKE");
            list.Add("CHECKPOINT");
            list.Add("HAVING");
            list.Add("CLOSE");
            list.Add("HOLDLOCK");
            list.Add("ROLLBACK");
            list.Add("CLUSTERED");
            list.Add("IDENTITY");
            list.Add("ROWCOUNT");
            list.Add("INSERT");
            list.Add("ROWGUIDCOL");
            list.Add("COLLATE");
            list.Add("IDENTITYCOL");
            list.Add("RULE");
            list.Add("COLUMN");
            list.Add("IF");
            list.Add("SAVE");
            list.Add("COMMIT");
            list.Add("SCHEMA");
            list.Add("COMPUTE");
            list.Add("INDEX");
            list.Add("SELECT");
            list.Add("CONSTRAINT");
            list.Add("CONTAINS");
            list.Add("INSERT");
            list.Add("SET");
            list.Add("CONTAINSTABLE");
            list.Add("INTERSECT");
            list.Add("SETUSER");
            list.Add("CONTINUE");
            list.Add("INTO");
            list.Add("SHUTDOWN");
            list.Add("CREATE");
            list.Add("STATISTICS");
            list.Add("KEY");
            list.Add("CURRENT");
            list.Add("KILL");
            list.Add("TABLE");
            list.Add("CURRENT_DATE");
            list.Add("TEXTSIZE");
            list.Add("CURRENT_TIME");
            list.Add("THEN");
            list.Add("LINENO");
            list.Add("TO");
            list.Add("LOAD");
            list.Add("TOP");
            list.Add("CURSOR");
            list.Add("NATIONAL");
            list.Add("TRAN");
            list.Add("DATABASE");
            list.Add("NOCHECK");
            list.Add("TRANSACTION");
            list.Add("DBCC");
            list.Add("NONCLUSTERED");
            list.Add("TRIGGER");
            list.Add("DEALLOCATE");
            list.Add("TRUNCATE");
            list.Add("DECLARE");
            list.Add("TSEQUAL");
            list.Add("DEFAULT");
            list.Add("UNION");
            list.Add("DELETE");
            list.Add("OF");
            list.Add("UNIQUE");
            list.Add("DENY");
            list.Add("OFF");
            list.Add("UPDATE");
            list.Add("DESC");
            list.Add("OFFSETS");
            list.Add("UPDATETEXT");
            list.Add("DISK");
            list.Add("ON");
            list.Add("USE");
            list.Add("DISTINCT");
            list.Add("OPEN");
            list.Add("DISTRIBUTED");
            list.Add("OPENDATASOURCE");
            list.Add("VALUES");
            list.Add("DOUBLE");
            list.Add("OPENQUERY");
            list.Add("VARYING");
            list.Add("DROP");
            list.Add("OPENROWSET");
            list.Add("VIEW");
            list.Add("OPENXML");
            list.Add("WAITFOR");
            list.Add("DUMP");
            list.Add("OPTION");
            list.Add("WHEN");
            list.Add("ELSE");
            list.Add("WHERE");
            list.Add("END");
            list.Add("ORDER");
            list.Add("WHILE");
            list.Add("ERRLVL");
            list.Add("WITH");
            list.Add("ESCAPE");
            list.Add("OVER");
            list.Add("WRITETEXT");
            list.Add("ABSOLUTE");
            list.Add("EXEC");
            list.Add("ACTION");
            list.Add("EXECUTE");
            list.Add("PARTIAL");
            list.Add("ADD");
            list.Add("EXTERNAL");
            list.Add("PRECISION");
            list.Add("ALTER");
            list.Add("FETCH");
            list.Add("FIRST");
            list.Add("FLOAT");
            list.Add("PRIMARY");
            list.Add("FOR");
            list.Add("PRIOR");
            list.Add("AS");
            list.Add("FOREIGN");
            list.Add("ASC");
            list.Add("PROCEDURE");
            list.Add("PUBLIC");
            list.Add("FROM");
            list.Add("READ");
            list.Add("AUTHORIZATION");
            list.Add("FULL");
            list.Add("REAL");
            list.Add("GET");
            list.Add("REFERENCES");
            list.Add("BEGIN");
            list.Add("GLOBAL");
            list.Add("RELATIVE");
            list.Add("GO");
            list.Add("RESTRICT");
            list.Add("BIT");
            list.Add("GOTO");
            list.Add("REVOKE");
            list.Add("BIT_LENGTH");
            list.Add("GRANT");
            list.Add("GROUP");
            list.Add("ROLLBACK");
            list.Add("BY");
            list.Add("HAVING");
            list.Add("ROWS");
            list.Add("CASCADE");
            list.Add("HOUR");
            list.Add("SCHEMA");
            list.Add("IDENTITY");
            list.Add("SCROLL");
            list.Add("CASE");
            list.Add("IMMEDIATE");
            list.Add("SECOND");
            list.Add("CATALOG");
            list.Add("INCLUDE");
            list.Add("SELECT");
            list.Add("CHAR");
            list.Add("INDEX");
            list.Add("SESSION");
            list.Add("CHARACTER");
            list.Add("SET");
            list.Add("CHECK");
            list.Add("SMALLINT");
            list.Add("CLOSE");
            list.Add("INSENSITIVE");
            list.Add("INSERT");
            list.Add("COLLATE");
            list.Add("INT");
            list.Add("SQL");
            list.Add("INTEGER");
            list.Add("COLUMN");
            list.Add("INTERSECT");
            list.Add("COMMIT");
            list.Add("CONNECT");
            list.Add("INTO");
            list.Add("CONSTRAINT");
            list.Add("ISOLATION");
            list.Add("CONTINUE");
            list.Add("KEY");
            list.Add("LANGUAGE");
            list.Add("TABLE");
            list.Add("LAST");
            list.Add("THEN");
            list.Add("CREATE");
            list.Add("TIME");
            list.Add("LEVEL");
            list.Add("TIMESTAMP");
            list.Add("CURRENT");
            list.Add("CURRENT_DATE");
            list.Add("LOCAL");
            list.Add("CURRENT_TIME");
            list.Add("TO");
            list.Add("TRANSACTION");
            list.Add("CURSOR");
            list.Add("DATE");
            list.Add("MINUTE");
            list.Add("DEALLOCATE");
            list.Add("DEC");
            list.Add("UNION");
            list.Add("DECIMAL");
            list.Add("NATIONAL");
            list.Add("UNIQUE");
            list.Add("DECLARE");
            list.Add("DEFAULT");
            list.Add("NCHAR");
            list.Add("UPDATE");
            list.Add("NEXT");
            list.Add("NO");
            list.Add("DELETE");
            list.Add("NONE");
            list.Add("DESC");
            list.Add("USING");
            list.Add("VALUES");
            list.Add("NUMERIC");
            list.Add("VARCHAR");
            list.Add("OCTET_LENGTH");
            list.Add("VARYING");
            list.Add("DISTINCT");
            list.Add("OF");
            list.Add("VIEW");
            list.Add("ON");
            list.Add("WHEN");
            list.Add("DOUBLE");
            list.Add("DROP");
            list.Add("OPEN");
            list.Add("WHERE");
            list.Add("ELSE");
            list.Add("OPTION");
            list.Add("WITH");
            list.Add("END");
            list.Add("END-EXEC");
            list.Add("ORDER");
            list.Add("ESCAPE");
            list.Add("EXCEPT");
            list.Add("OUTPUT");
            for (var ii = 0; ii < list.Count; ii++) list[ii] = list[ii].ToLower();
            return list;
        }

        #endregion
    }

}
