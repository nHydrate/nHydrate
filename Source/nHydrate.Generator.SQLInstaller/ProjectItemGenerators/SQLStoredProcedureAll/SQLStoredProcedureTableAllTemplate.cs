#pragma warning disable 0168
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using nHydrate.Generator.Models;
using nHydrate.Generator.Common.Util;
using nHydrate.Generator.ProjectItemGenerators;

namespace nHydrate.Generator.SQLInstaller.ProjectItemGenerators.SQLStoredProcedureAll
{
    class SQLStoredProcedureTableAllTemplate : BaseDbScriptTemplate
    {
        private StringBuilder sb = new StringBuilder();
        private Table _table;
        private bool _useSingleFile = false;

        #region Constructors
        public SQLStoredProcedureTableAllTemplate(ModelRoot model, Table table, bool useSingleFile)
            : base(model)
        {
            _table = table;
            _useSingleFile = useSingleFile;
        }
        #endregion

        #region BaseClassTemplate overrides

        public override string FileContent
        {
            get
            {
                try
                {
                    this.GenerateContent();
                    return sb.ToString();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public override string FileName
        {
            get { return string.Format("{0}.sql", _table.PascalName); }
        }

        #endregion

        #region GenerateContent
        private void GenerateContent()
        {
            //if (_model.Database.AllowZeroTouch)
            //{
            //  //Add drop SP here
            //  return;
            //}

            try
            {
                ISQLGenerate generator = null;

                if (!_useSingleFile)
                {
                    sb.AppendLine("--DO NOT MODIFY THIS FILE. IT IS ALWAYS OVERWRITTEN ON GENERATION.");
                    sb.AppendLine();
                }

                sb.AppendLine("--This SQL is generated for internal stored procedures for table [" + _table.DatabaseName + "]");
                generator = new SQLSelectAuditBusinessObjectTemplate(_model, _table);
                generator.GenerateContent(sb);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

    }
}